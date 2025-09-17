using Microsoft.AspNetCore.Identity;
using PayEase_CaseStudy.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PayEase_CaseStudy.Repository
{
    public class UserRepo : IUser
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepo(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Get all users
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        // Get single user by ID
        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
    }
}
