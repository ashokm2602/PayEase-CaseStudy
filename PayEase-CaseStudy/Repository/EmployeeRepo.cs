using Microsoft.EntityFrameworkCore;
using PayEase_CaseStudy.Authentication;
using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;
using PayEase_CaseStudy.Services;

namespace PayEase_CaseStudy.Repository
{
    public class EmployeeRepo : IEmployee
    {
        private readonly PayDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public EmployeeRepo(PayDbContext context,ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<int> GetEmployeesCount()
        {
            return await _context.Employees.CountAsync();
        }
        public async Task<Employee> AddEmployee(EmployeeDTO emp)
        {
            try
            {
                var newEmp = new Employee
                {
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    DOB = emp.DOB,
                    HireDate = emp.HireDate,
                    DeptId = emp.DeptId,
                    ContactNumber = emp.ContactNumber,
                    Address = emp.Address,
                    ApplicationUserId = emp.UserId,
                    BaseSalary = emp.BaseSalary,

                };
                if (newEmp == null)
                    return null;
                 _context.Employees.Add(newEmp);
                await _context.SaveChangesAsync();
                return newEmp;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding employee", ex);
            }
        }
        public async Task<Employee> UpdateEmployeeWithUserId(string userId,EmployeeUpdateDTO employee)
        {
            try
            {
                if (employee == null)
                    throw new ArgumentNullException(nameof(employee), "Employee DTO cannot be null");
                
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("Current user ID is null. Make sure the user is authenticated.");

                var emp = await _context.Employees
                    .Include(e => e.User)
                    .FirstOrDefaultAsync(u => u.ApplicationUserId == userId);

                if (emp == null)
                    throw new Exception($"Employee not found for userId {userId}");

                // Update fields
                emp.ContactNumber = employee.ContactNumber;
                emp.Address = employee.Address;
                emp.FirstName = employee.FirstName;
                emp.LastName = employee.LastName;
                emp.DOB = employee.DOB;

                _context.Employees.Update(emp);
                await _context.SaveChangesAsync();

                return emp;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating employee", ex);
            }
        }


        public async Task<List<Employee>> GetAllEmployees()
        {
            try
            {
                var employees = await _context.Employees.Include(e => e.User).ToListAsync();
                
                return employees;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving employees", ex);
            }
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            try
            {
                var emp = await _context.Employees.FirstOrDefaultAsync(u => u.EmpId == id);
                if (emp == null)
                    return null;
                return emp;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving employee with ID {id}", ex);
            }
        }

        public async Task<Employee> UpdateEmployee(int id, EmployeeUpdateDTO emp)
        {
            try
            {
                var existingEmp = await _context.Employees.FindAsync(id);
                if (existingEmp == null)
                    return null;
                existingEmp.FirstName = emp.FirstName;
                existingEmp.LastName = emp.LastName;
                existingEmp.DOB = emp.DOB;
                existingEmp.ContactNumber = emp.ContactNumber;
                existingEmp.Address = emp.Address;

                _context.Employees.Update(existingEmp);
                await _context.SaveChangesAsync();
                return existingEmp;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating employee with ID {id}", ex);
            }
        }

        public async Task<Employee> GetEmployeeByUserId(string userId)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.ApplicationUserId == userId);
        }


        public async Task DeleteEmployee(int id)
        {
            try
            {
                var emp = await _context.Employees.FindAsync(id);
                if (emp == null)
                    throw new Exception($"Employee with ID {id} not found");
                _context.Employees.Remove(emp);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting employee with ID {id}", ex);
            }
        }
    }
}
