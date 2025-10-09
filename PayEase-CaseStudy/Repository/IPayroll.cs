using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public interface IPayroll
    {
        public Task<List<Payroll>> GetAllPayrolls();
        public Task<Payroll> GetPayrollById(int id);
        public Task<Payroll> AddPayroll(PayrollDTO payroll);
        public Task<Payroll> UpdatePayroll(int id, PayrollDTO payroll);
        public Task DeletePayroll(int id);

    }
}
