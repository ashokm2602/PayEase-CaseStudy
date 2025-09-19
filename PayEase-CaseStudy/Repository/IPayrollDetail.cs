using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public interface IPayrollDetail
    {
        public Task<List<PayrollDetail>> GetAllPayrollDetails();
        public Task<PayrollDetail> GetPayrollDetailById(int id);
        public Task<List<PayrollDetail>> GetPayrollDetailsByEmployeeId(int employeeId);
        public Task<PayrollDetail> UpdateBaseSalaryByEmpId(int id, decimal sal);
        public Task<PayrollDetail> AddPayrollDetail(PayrollDetailDTO payrollDetail);
        public Task<PayrollDetail> UpdatePayrollDetail(int id, PayrollDetailDTO payrollDetail);
        public Task DeletePayrollDetail(int id);
    }
}
