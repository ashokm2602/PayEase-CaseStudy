using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PayEase_CaseStudy.Authentication;
using PayEase_CaseStudy.DTOs;

namespace PayEase_CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/Users/getallusers
        [HttpGet("getallusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var identityUsers = _userManager.Users.ToList();
            var usersWithRoles = new List<UserWithRolesDTO>();

            foreach (var identityUser in identityUsers)
            {
                var roles = await _userManager.GetRolesAsync(identityUser);

                usersWithRoles.Add(new UserWithRolesDTO
                {
                    UserId = identityUser.Id,
                    Username = identityUser.UserName,
                    Email = identityUser.Email,
                    Roles = roles.ToList()
                });
            }

            return Ok(usersWithRoles);
        }

        // GET: api/Users/getuser/{id}
        [HttpGet("getuser/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var identityUser = await _userManager.FindByIdAsync(id);
            if (identityUser == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(identityUser);

            var userWithRoles = new UserWithRolesDTO
            {
                UserId = identityUser.Id,
                Username = identityUser.UserName,
                Email = identityUser.Email,
                Roles = roles.ToList()
            };

            return Ok(userWithRoles);
        }

       
    }
}
