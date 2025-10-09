using Microsoft.AspNetCore.Mvc;
using PayEase_CaseStudy.Repository;
using System;
using System.Threading.Tasks;

namespace PayEase_CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDashboardController : ControllerBase
    {
        private readonly IEmployee _employee;
        private readonly ILeave _leave;
        private readonly IPayrollDetail _payroll;

        public EmployeeDashboardController(IEmployee employee, ILeave leave, IPayrollDetail payroll)
        {
            _employee = employee;
            _leave = leave;
            _payroll = payroll;
        }

        [HttpGet("stats/{employeeId}")]
        public async Task<IActionResult> GetEmployeeDashboardStats(int employeeId)
        {
            try
            {
                // Fetch total/pending leaves for employee
                var pendingLeaves = await _leave.GetPendingLeavesCountByEmployee(employeeId); // Implement in ILeave repo
                var totalLeaves = await _leave.GetTotalLeavesCountByEmployee(employeeId); // Implement in ILeave repo

                // Fetch payroll/salary info for employee
                var payroll = await _payroll.GetLatestPayrollDetailByEmployee(employeeId); // Implement in IPayroll repo

                return Ok(new
                {
                    pendingLeaves,
                    totalLeaves,
                    payrollAmount = payroll?.NetSalary ?? 0
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error retrieving stats: {e.Message}");
            }
        }
    }
}
