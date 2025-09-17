using Microsoft.EntityFrameworkCore;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public class RoleRepo : IRole
    {
        private readonly PayDbContext _context;
        public RoleRepo(PayDbContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            try
            {
                var roles = await _context.Roles.ToListAsync();
                return roles;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving roles", ex);
            }
        }

        public async Task<Role> GetRoleById(int id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                    return null;
                return role;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving role with ID {id}", ex);
            }
        }

        public async Task<Role> AddRole(string roleName)
        {
            try
            {
                var newRole = new Role
                {
                    RoleName = roleName
                };
                if (newRole == null)
                    return null;
                _context.Roles.Add(newRole);
                await _context.SaveChangesAsync();
                return newRole;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding role", ex);
            }
        }
        public async Task DeleteRole(int id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                    throw new Exception("Role not found");
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting role with ID {id}", ex);
            }
        }

        public async Task<Role> UpdateRole(int id, string roleName)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                    return null;
                role.RoleName = roleName;
                _context.Roles.Update(role);
                await _context.SaveChangesAsync();
                return role;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating role with ID {id}", ex);
            }
        }
    }
}
