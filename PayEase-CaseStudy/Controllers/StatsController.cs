using Microsoft.AspNetCore.Mvc;
using PayEase_CaseStudy.Repository;
using System;
using System.Threading.Tasks;

namespace PayEase_CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IEmployee _employee;
        private readonly IDepartment _department;
        private readonly ILeave _leave;

        public StatsController(IEmployee employee, IDepartment department, ILeave leave)
        {
            _employee = employee;
            _department = department;
            _leave = leave;
        }

        [HttpGet("counts")]
        public async Task<IActionResult> GetCounts()
        {
            try
            {
                var totalEmployees = await _employee.GetEmployeesCount(); // You need to implement this method in IEmployee repo
                var totalDepartments = await _department.GetDepartmentsCount(); // Implement in IDepartment repo
                var pendingLeaves = await _leave.GetPendingLeavesCount(); // Implement in ILeave repo

                return Ok(new
                {
                    totalEmployees,
                    totalDepartments,
                    pendingLeaves
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error retrieving counts: {e.Message}");
            }
        }
    }
}
