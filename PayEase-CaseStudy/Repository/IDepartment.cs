using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public interface IDepartment
    {
        public Task<List<Department>> GetAllDepartments();
        public Task<Department> GetDepartmentById(int id);
        public Task<Department> AddDepartment(string dept_name);
        public Task DeleteDepartment(int id);
        Task<int> GetDepartmentsCount();

    }
}
