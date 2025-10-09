using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public class PayrollDetailRepo : IPayrollDetail
    {
        private readonly PayDbContext _context;
        public PayrollDetailRepo(PayDbContext context)
        {
            _context = context;
        }
        public async Task<PayrollDetail> AddPayrollDetail(PayrollDetailDTO payrollDetail)
        {
            try
            {
                if (payrollDetail == null)
                    throw new ArgumentNullException(nameof(payrollDetail), "PayrollDetail cannot be null");

                // Find employee
                var emp = await _context.Employees.FindAsync(payrollDetail.EmpId);
                if (emp == null)
                {
                    throw new KeyNotFoundException("EmployeeId is not found.");
                }

                // ✅ Create new PayrollDetail with BasicSalary from Employee
                var newpay = new PayrollDetail
                {
                    PayrollId = payrollDetail.PayrollId,
                    EmpId = payrollDetail.EmpId,
                    BasicSalary = emp.BaseSalary // fetch from employee
                };

                // Save first so EF generates PayrollDetailId
                _context.PayrollDetails.Add(newpay);
                await _context.SaveChangesAsync();

                // ✅ Now calculate net salary
                var netSalary = await CalculateNetSalary(newpay.PayrollDetailId);
                newpay.NetSalary = netSalary;

                // Save updated net salary
                _context.PayrollDetails.Update(newpay);
                await _context.SaveChangesAsync();

                return newpay;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the payroll detail.", ex);
            }
        }


        public async Task<PayrollDetail> GetLatestPayrollDetailByEmployee(int empId)
        {
            return await _context.PayrollDetails
                .Where(pd => pd.EmpId == empId)
                .OrderByDescending(pd => pd.PayrollId) // or .OrderByDescending(pd => pd.PayrollDetailId) if that's the chronology
                .FirstOrDefaultAsync();
        }


        public async Task<PayrollDetail?> GetPayrollDetailById(int id)
        {
            try
            {
                var payrollDetail = await _context.PayrollDetails.FindAsync(id);
                if (payrollDetail == null)
                {
                    throw new KeyNotFoundException($"PayrollDetail with ID {id} not found.");
                }
                payrollDetail.NetSalary = await CalculateNetSalary(id);
                await _context.SaveChangesAsync();

                return payrollDetail;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the payroll detail.", ex);
            }
        }
        public async Task<List<PayrollDetail>> GetAllPayrollDetails()
        {
            try
            {
                var payrollDetails = await _context.PayrollDetails.ToListAsync();
                if (payrollDetails == null || !payrollDetails.Any())
                {
                    throw new Exception("No payroll details found.");
                }

                return payrollDetails;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                throw new Exception("An error occurred while retrieving payroll details.", ex);
            }
        }
        public async Task<PayrollDetail> UpdatePayrollDetail(int id, PayrollDetailDTO payrollDetail)
        {
            try
            {
                if (payrollDetail == null)
                    throw new ArgumentNullException(nameof(payrollDetail), "PayrollDetail cannot be null");
                var existingPayrollDetail = await _context.PayrollDetails.FindAsync(id);
                if (existingPayrollDetail == null)
                {
                    throw new KeyNotFoundException($"PayrollDetail with ID {id} not found.");
                }
                existingPayrollDetail.PayrollId = payrollDetail.PayrollId;
                existingPayrollDetail.EmpId = payrollDetail.EmpId;

                var emp = await _context.Employees.FindAsync(payrollDetail.EmpId);
                if (emp == null)
                {
                    throw new KeyNotFoundException("EmployeeId is not found.");
                }
                existingPayrollDetail.BasicSalary = emp.BaseSalary;

                _context.PayrollDetails.Update(existingPayrollDetail);
                await CalculateNetSalary(existingPayrollDetail.PayrollDetailId);
                _context.PayrollDetails.Update(existingPayrollDetail);
                await _context.SaveChangesAsync();
                return existingPayrollDetail;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                throw new Exception("An error occurred while updating the payroll detail.", ex);
            }

        }
        public async Task DeletePayrollDetail(int id)
        {
            try
            {
                var payrollDetail = await _context.PayrollDetails.FindAsync(id);
                if (payrollDetail == null)
                {
                    throw new KeyNotFoundException($"PayrollDetail with ID {id} not found.");
                }
                _context.PayrollDetails.Remove(payrollDetail);
                await _context.SaveChangesAsync();



            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                throw new Exception("An error occurred while deleting the payroll detail.", ex);
            }
        }

        public async Task<Decimal> CalculateNetSalary(int payrollDetailId)
        {
            try
            {
                var payrollDetail = await _context.PayrollDetails.FindAsync(payrollDetailId);
                if (payrollDetail == null)
                {
                    throw new KeyNotFoundException($"PayrollDetail with ID {payrollDetailId} not found.");
                }
                var empId = payrollDetail.EmpId;
                var basicSalary = payrollDetail.BasicSalary;
                var compensations = await _context.CompensationAdjustments
                    .Where(c => c.EmpId == empId)
                    .ToListAsync();
                decimal totalAdditions = compensations
                    .Where(c => c.AdjustmentType.Equals("Benefit", StringComparison.OrdinalIgnoreCase))
                    .Sum(c => c.Amount);
                decimal totalDeductions = compensations
                    .Where(c => c.AdjustmentType.Equals("Deduction", StringComparison.OrdinalIgnoreCase))
                    .Sum(c => c.Amount);
                decimal netSalary = basicSalary + totalAdditions - totalDeductions;
                payrollDetail.NetSalary = netSalary;
                _context.PayrollDetails.Update(payrollDetail);
                await _context.SaveChangesAsync();
                return netSalary;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                throw new Exception("An error occurred while calculating the net salary.", ex);
            }
        }
        [HttpPut("updateBaseSalaryByEmpId{id}")]
        [Authorize(Roles = "Payroll Processor")]
        public async Task<PayrollDetail> UpdateBaseSalaryByEmpId(int id,decimal sal)
        {
            try
            {
                var pay = await _context.PayrollDetails.Where(p => p.EmpId == id).FirstOrDefaultAsync();
                
                if (pay == null)
                    throw new KeyNotFoundException($"No PayrollDetails found for Employee ID {id}.");
                pay.BasicSalary = sal;
                return pay;
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving payroll details for the employee.", ex);
            }

        }
        public async Task<List<PayrollDetail>> GetPayrollDetailsByEmployeeId(int employeeId)
        {
            try
            {
                var payrollDetails = _context.PayrollDetails.Where(p => p.EmpId == employeeId).ToListAsync();
                if (payrollDetails == null || !payrollDetails.Result.Any())
                {
                    throw new KeyNotFoundException($"No PayrollDetails found for Employee ID {employeeId}.");
                }
                return await payrollDetails;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                throw new Exception("An error occurred while retrieving payroll details for the employee.", ex);
            }
        }

        Task<List<PayrollDetail>> IPayrollDetail.GetPayrollDetailsByEmployeeId(int employeeId)
        {
            try
            {
                var payrollDetails = _context.PayrollDetails.Where(p => p.EmpId == employeeId).ToListAsync();

                if (payrollDetails == null || !payrollDetails.Result.Any())
                {
                    throw new KeyNotFoundException($"No PayrollDetails found for Employee ID {employeeId}.");
                }

                return payrollDetails;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                throw new Exception("An error occurred while retrieving payroll details for the employee.", ex);
            }
        }
    }
}
