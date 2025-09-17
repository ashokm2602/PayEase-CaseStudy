using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public interface IRole
    {
        public Task<List<Role>> GetAllRoles();
        public Task<Role> GetRoleById(int id);
        public Task<Role> AddRole(string role_name);
        public Task DeleteRole(int id);
        public Task<Role> UpdateRole(int id, string role_name);
    }
}
