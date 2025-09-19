using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace PayEase_CaseStudy.Services
{


    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetUserId()
        {
            var claims = _httpContextAccessor.HttpContext?.User?.Claims;

            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    // Temporary debug output
                    Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
                }
            }

            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

    }


}
