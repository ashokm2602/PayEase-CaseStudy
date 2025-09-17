using PayEase_CaseStudy.Authentication;
using PayEase_CaseStudy.DTOs;

namespace PayEase_CaseStudy.Repository
{
    public interface IUser
    {
        Task<List<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> GetUserById(string id);
        
    }
}
