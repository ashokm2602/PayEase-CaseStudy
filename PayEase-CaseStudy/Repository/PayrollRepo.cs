using Microsoft.EntityFrameworkCore;
using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public class PayrollRepo : IPayroll
    {
        private readonly PayDbContext _context;
        public PayrollRepo(PayDbContext context)
        {
            _context = context;
        }

        public async Task<Payroll> AddPayroll(PayrollDTO payroll)
        {
            try
            {
                var newPayroll = new Payroll
                {
                    PayrollPeriodStart = payroll.PayrollPeriodStart,
                    PayrollPeriodEnd = payroll.PayrollPeriodEnd,
                    ProcessedDate = payroll.ProcessedDate,
                    Status = payroll.Status
                };
                _context.Payrolls.Add(newPayroll);
                await _context.SaveChangesAsync();
                if(newPayroll == null)
                    return null;
                return newPayroll;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding payroll", ex);
            }
        }

        public async Task DeletePayroll(int id)
        {
            try
            {
                var payroll =await _context.Payrolls.FindAsync(id);
                if (payroll == null)
                    throw new Exception($"Payroll with ID {id} not found");
                _context.Payrolls.Remove(payroll);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting payroll with ID {id}", ex);
            }
        }

        public async Task<List<Payroll>> GetAllPayrolls()
        {
            try
            {
                var payrolls = await _context.Payrolls.ToListAsync();
                if (payrolls == null || payrolls.Count == 0)
                    return null;
                return payrolls;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving payrolls", ex);
            }
        }
        public async Task<Payroll> GetPayrollById(int id)
        {
            try
            {
                var payroll = await _context.Payrolls.FindAsync(id);
                if (payroll == null)
                    return null;
                return payroll;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving payroll with ID {id}", ex);
            }
        }


        public async Task<Payroll> UpdatePayroll(int id, PayrollDTO payroll)
        {
            try
            {
                var existingPayroll = await _context.Payrolls.FindAsync(id);
                if (existingPayroll == null)
                    return null;
                existingPayroll.PayrollPeriodStart = payroll.PayrollPeriodStart;
                existingPayroll.PayrollPeriodEnd = payroll.PayrollPeriodEnd;
                existingPayroll.ProcessedDate = payroll.ProcessedDate;
                existingPayroll.Status = payroll.Status;
                await _context.SaveChangesAsync();
                return existingPayroll;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating payroll with ID {id}", ex);
            }
        }

    }
}
