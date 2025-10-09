using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using PayEase_CaseStudy.Authentication;
using PayEase_CaseStudy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace PayEase_CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly PayDbContext _context;

        private static Dictionary<string, string> _userRefreshTokens = new Dictionary<string, string>();

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ILogger<AuthenticationController> logger,
            PayDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userExists = await _userManager.FindByNameAsync(registerModel.Username);
            if (userExists != null)
            {
                return Conflict(new Response { Status = "Error", Message = "User already exists!" });
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = registerModel.Email,
                UserName = registerModel.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return BadRequest(new Response { Status = "Error", Message = $"User creation failed: {errors}" });
            }

            // Keep existing role logic but simplify repeated checks
            if (registerModel.Role == "Admin")
            {
                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            else if (registerModel.Role == "Payroll Processor")
            {
                if (!await _roleManager.RoleExistsAsync(UserRoles.PayrollProcessor))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.PayrollProcessor));
                await _userManager.AddToRoleAsync(user, UserRoles.PayrollProcessor);
            }
            else if (registerModel.Role == "Manager")
            {
                if (!await _roleManager.RoleExistsAsync(UserRoles.Manager))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));
                await _userManager.AddToRoleAsync(user, UserRoles.Manager);
            }
            else if (registerModel.Role == "Employee")
            {
                if (!await _roleManager.RoleExistsAsync(UserRoles.Employee))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Employee));
                await _userManager.AddToRoleAsync(user, UserRoles.Employee);
            }
            else
            {
                // Optional: handle unknown roles
                return BadRequest(new Response { Status = "Error", Message = "Invalid role specified." });
            }
            _logger.LogInformation($"New user created with Id: {user.Id}");

            return Ok(new
            {
                Status = "Success",
                Message = "User created successfully!",
                userId = user.Id
            });

        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
                return BadRequest("Invalid login request");

            // Efficiently fetch user without tracking unnecessary data
            var user = await _userManager.Users
                .AsNoTracking() // prevents EF from tracking and loading extra data
                .SingleOrDefaultAsync(u => u.UserName == loginModel.Username);

            if (user == null)
                return Unauthorized("User not found");

            // Verify password
            var passwordValid = await _userManager.CheckPasswordAsync(user, loginModel.Password);
            if (!passwordValid)
                return Unauthorized("Invalid password");

            // Fetch roles only if needed
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };
            foreach (var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            // Generate tokens
            var accessToken = GenerateAccessToken(authClaims);
            var refreshToken = GenerateRefreshToken();

            // Save refresh token in DB and revoke old ones
            var newRefreshToken = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                Expires = DateTime.Now.AddDays(3),
                Created = DateTime.Now,
                IsRevoked = false
            };

            var oldTokens = _context.RefreshTokens.Where(r => r.UserId == user.Id && !r.IsRevoked);
            foreach (var t in oldTokens) t.IsRevoked = true;

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                username = user.UserName,
                userId = user.Id,
                token = accessToken,
                refreshToken = refreshToken,
                roles = userRoles,
                expiration = DateTime.Now.AddMinutes(3)
            });
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenModel tokenModel)
        {
            if (tokenModel == null)
                return BadRequest("Invalid request");

            var accessToken = tokenModel.AccessToken;
            var refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity?.Name;

            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return Unauthorized();

            var storedToken = _context.RefreshTokens
                .FirstOrDefault(r => r.UserId == user.Id && r.Token == refreshToken);

            if (storedToken == null || storedToken.IsRevoked || storedToken.Expires < DateTime.Now)
                return Unauthorized("Invalid refresh token");

            // Rotate refresh token
            storedToken.IsRevoked = true;

            var newRefreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                UserId = user.Id,
                Expires = DateTime.Now.AddDays(3),
                Created = DateTime.Now,
                IsRevoked = false
            };

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            var newAccessToken = GenerateAccessToken(principal.Claims);

            return Ok(new
            {
                token = newAccessToken,
                refreshToken = newRefreshToken.Token
            });
        }

        // ---------------- HELPERS ----------------

        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(3), // short life
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
                ValidateLifetime = false // ignore expiration
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }

}