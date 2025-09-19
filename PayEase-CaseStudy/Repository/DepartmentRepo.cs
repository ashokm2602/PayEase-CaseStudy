using Microsoft.EntityFrameworkCore;
using PayEase_CaseStudy.Models;
using PayEase_CaseStudy.Services;

namespace PayEase_CaseStudy.Repository
{
    public class DepartmentRepo : IDepartment
    {
        private readonly PayDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuditLog _auditLogRepo;

        public DepartmentRepo(PayDbContext context, ICurrentUserService currentUserService, IAuditLog auditLogRepo)
        {
            _context = context;
            _currentUserService = currentUserService;
            _auditLogRepo = auditLogRepo;
        }

        public async Task<List<Department>> GetAllDepartments()
        {
            try
            {
                var departments = await _context.Departments.ToListAsync();
                return departments;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving departments", ex);
            }
        }

        public async Task<Department> GetDepartmentById(int id)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);
                if (department == null)
                    return null;
                return department;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving department with ID {id}", ex);
            }
        }

        public async Task<Department> AddDepartment(string departmentName)
        {
            try
            {
                var newDepartment = new Department
                {
                    DeptName = departmentName
                };
                if (newDepartment == null)
                    return null;
                _context.Departments.Add(newDepartment);
                await _context.SaveChangesAsync();
                


                return newDepartment;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding department", ex);
            }
        }

        public async Task<Department> UpdateDepartment(int id, string departmentName)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);
                if (department == null)
                    return null;
                department.DeptName = departmentName;
                _context.Departments.Update(department);
                await _context.SaveChangesAsync();
                return department;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating department with ID {id}", ex);
            }
        }

        public async Task DeleteDepartment(int id)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);
                if (department == null)
                    throw new Exception($"Department with ID {id} not found");
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting department with ID {id}", ex);
            }
        }
    }
}
