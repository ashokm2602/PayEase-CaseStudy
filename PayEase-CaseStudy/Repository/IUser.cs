using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public interface IUser
    {
        public Task<List<User>> GetAllUsers();
        public Task<User> GetUserById(int id);
        public Task<User> AddUser(UserDTO user);
        public Task<User> UpdateUser(int id, UserDTO user);
        public Task DeleteUser(int id);
    }
}
