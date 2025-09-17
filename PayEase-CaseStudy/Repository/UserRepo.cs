using Microsoft.EntityFrameworkCore;
using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public class UserRepo : IUser
    {
        private readonly PayDbContext _context;
        public UserRepo(PayDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                var users = await _context.Users.Include(u => u.Role).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error retrieving users", ex);
            }
        }

        public async Task<User> GetUserById(int id)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
                if (user == null)
                    return null;
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user with ID {id}", ex);
            }
        }

        public async Task<User> AddUser(UserDTO user)
        {
            try
            {
                var newUser = new User
                {
                    Username = user.Username,
                    Email = user.Email,
                    PasswordHash = user.Password,
                    RoleId = user.RoleId,
                    CreatedAt = DateTime.Now

                };
                if (newUser == null)
                    return null;
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return newUser;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding new user", ex);
            }
        }

        public async Task<User> UpdateUser(int id, UserDTO user)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser == null)
                    return null;
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.PasswordHash = user.Password;
                existingUser.RoleId = user.RoleId;
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
                return existingUser;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating user with ID {id}", ex);
            }
        }
        
        public async Task DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting user with ID {id}", ex);
            }
        }

        
    }
}
