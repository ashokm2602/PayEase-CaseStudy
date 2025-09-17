using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public interface IPayrollDetail
    {
        public Task<List<PayrollDetail>> GetAllPayrollDetails();
        public Task<PayrollDetail> GetPayrollDetailById(int id);
        public Task<PayrollDetail> AddPayrollDetail(PayrollDetailDTO payrollDetail);
        public Task<PayrollDetail> UpdatePayrollDetail(int id, PayrollDetailDTO payrollDetail);
        public Task DeletePayrollDetail(int id);
    }
}
