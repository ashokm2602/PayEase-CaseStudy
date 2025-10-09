using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public interface IEmployee
    {
        public Task<List<Employee>> GetAllEmployees();

        Task<int> GetEmployeesCount();

        public Task<Employee> GetEmployeeById(int id);
        public Task<Employee> AddEmployee(EmployeeDTO employee);
        public Task<Employee> UpdateEmployee(int id, EmployeeUpdateDTO employee);
        public Task<Employee> UpdateEmployeeWithUserId(string userid,EmployeeUpdateDTO employee);
        Task<Employee> GetEmployeeByUserId(string userId);

        public Task DeleteEmployee(int id); 
    }
}
