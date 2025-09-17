using Microsoft.EntityFrameworkCore;
using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public class EmployeeRepo : IEmployee
    {
        private readonly PayDbContext _context;
        public EmployeeRepo(PayDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> AddEmployee(EmployeeDTO emp)
        {
            try
            {
                var newEmp = new Employee
                {
                    UserId = emp.UserId,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    DOB = emp.DOB,
                    HireDate = emp.HireDate,
                    DeptId = emp.DeptId,
                    ContactNumber = emp.ContactNumber,
                    Address = emp.Address,
                    BaseSalary = emp.BaseSalary
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

        public async Task<List<Employee>> GetAllEmployees()
        {
            try
            {
                var employees = await _context.Employees.Include(e => e.User).ToListAsync();
                if (employees == null || employees.Count == 0)
                    return null;
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
                var emp = _context.Employees.Include(u=>u.User).FirstOrDefault(u => u.EmpId == id);
                if (emp == null)
                    return null;
                return emp;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving employee with ID {id}", ex);
            }
        }

        public async Task<Employee> UpdateEmployee(int id, EmployeeDTO emp)
        {
            try
            {
                var existingEmp = await _context.Employees.FindAsync(id);
                if (existingEmp == null)
                    return null;
                existingEmp.FirstName = emp.FirstName;
                existingEmp.LastName = emp.LastName;
                existingEmp.DOB = emp.DOB;
                existingEmp.HireDate = emp.HireDate;
                existingEmp.DeptId = emp.DeptId;
                existingEmp.ContactNumber = emp.ContactNumber;
                existingEmp.Address = emp.Address;
                existingEmp.BaseSalary = emp.BaseSalary;
                _context.Employees.Update(existingEmp);
                await _context.SaveChangesAsync();
                return existingEmp;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating employee with ID {id}", ex);
            }
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
