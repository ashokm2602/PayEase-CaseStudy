using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;
using PayEase_CaseStudy.Repository;

namespace PayEase_CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployee _employee;

        public EmployeesController(IEmployee employee)
        {
            _employee = employee;
        }

        // GET: api/Employees
        [HttpGet("GetAllEmployees")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IList<Employee>>> GetAllEmployees()
        {
            try
            {
                var employees = await _employee.GetAllEmployees();
                if (employees == null || employees.Count == 0)
                {
                    return NotFound("No employees found.");
                }
                return Ok(employees);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retreiving Employees"+ex);
            }
        }

        // GET: api/Employees/5
        [HttpGet("GetEmployeeById")]
        [AllowAnonymous]
        public async Task<ActionResult<Employee>> GetEmployeeById([FromQuery] int id)
        {
            try
            {
                var employee = await _employee.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound($"Employee with ID {id} not found.");
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving employee with ID {id}", ex);
            }
        }


        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateEmployee")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody]EmployeeUpdateDTO employee)
        {
            try
            {
                var updatedEmployee = await _employee.UpdateEmployee(id, employee);
                if (updatedEmployee == null)
                {
                    return NotFound($"Employee with ID {id} not found.");
                }
                return Ok(updatedEmployee);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error updating employee with ID {id}", ex);
            }
        }

        [HttpPut("UpdateEmployeeWithUserId")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateEmployeeWithUserId(string userid,[FromBody]EmployeeUpdateDTO employee)
        {
            try
            {
                var updatedEmployee = await _employee.UpdateEmployeeWithUserId(userid,employee);
                if (updatedEmployee == null)
                {
                    return NotFound("Employee not found.");
                }
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating employee", ex);
            }
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeDTO employee)
        {
            try
            {
                var newEmployee = await _employee.AddEmployee(employee);
                if (newEmployee == null)
                {
                    return BadRequest("Failed to add employee.");
                }
                return Ok(newEmployee);
            }
            catch(Exception ex)
            {
                throw new Exception("Error adding employee", ex);
            }
        }
        [HttpGet("GetEmployeeByUserId")]
        [AllowAnonymous]
        public async Task<ActionResult<Employee>> GetEmployeeByUserId(string userId)
        {
            try
            {
                var employee = await _employee.GetEmployeeByUserId(userId);
                if (employee == null)
                {
                    return NotFound($"No employee found with User ID: {userId}");
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving employee by User ID", ex);
            }
        }

        // DELETE: api/Employees/5
        [HttpDelete("DeleteEmployee{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var existingEmployee = await _employee.GetEmployeeById(id);
                if (existingEmployee == null)
                {
                    return NotFound($"Employee with ID {id} not found.");
                }
                await _employee.DeleteEmployee(id);
                return NoContent();
            }
            catch(Exception ex)
            {
                throw new Exception($"Error deleting employee with ID {id}", ex);
            }
        }

       
    }
}
