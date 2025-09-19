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
    public class PayrollsController : ControllerBase
    {
        private readonly IPayroll _payroll;

        public PayrollsController(IPayroll payroll)
        {
            _payroll = payroll;
        }

        // GET: api/Payrolls
        [HttpGet("GetAllPayrolls")]
        [Authorize(Roles = "Payroll Processor")]
        public async Task<ActionResult<IEnumerable<Payroll>>> GetAllPayrolls()
        {
            try
            {
                var payrolls = await _payroll.GetAllPayrolls();
                if (payrolls == null || payrolls.Count == 0)
                {
                    return NotFound("No payrolls found.");
                }
                return Ok(payrolls);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving payrolls", ex);
            }
        }

        // GET: api/Payrolls/5
        [HttpGet("GetPayrollById{id}")]
        [Authorize(Roles = "Payroll Processor")]
        public async Task<ActionResult<Payroll>> GetPayrollById(int id)
        {
            try
            {
                var payroll = await _payroll.GetPayrollById(id);
                if (payroll == null)
                {
                    return NotFound($"Payroll with ID {id} not found.");
                }
                return Ok(payroll);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error retrieving payroll with ID {id}", ex);
            }
        }

        // PUT: api/Payrolls/5
        [HttpPut("UpdatePayroll{id}")]
        [Authorize(Roles = "Payroll Processor")]
        public async Task<IActionResult> UpdatePayroll(int id, PayrollDTO payroll)
        {
            try
            {
                var updatedPayroll = await _payroll.UpdatePayroll(id, payroll);
                if (updatedPayroll == null)
                {
                    return NotFound($"Payroll with ID {id} not found.");
                }
                return Ok(updatedPayroll);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating payroll with ID {id}", ex);
            }
        }

        // POST: api/Payrolls
        [HttpPost("AddPayroll")]
        [Authorize(Roles = "Payroll Processor")]
        public async Task<ActionResult<Payroll>> AddPayroll(PayrollDTO payroll)
        {
            try
            {
                var newPayroll = await _payroll.AddPayroll(payroll);
                if (newPayroll == null)
                {
                    return BadRequest("Failed to add payroll.");
                }
                return CreatedAtAction("GetPayrollById", new { id = newPayroll.PayrollId }, newPayroll);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding payroll", ex);
            }
        }
            // DELETE: api/Payrolls/5
            [HttpDelete("DeletePayroll{id}")]
        [Authorize(Roles = "Payroll Processor")]
        public async Task<IActionResult> DeletePayroll(int id)
            {
                try
                {
                    var payroll = await _payroll.GetPayrollById(id);
                    if (payroll == null)
                    {
                        return NotFound($"Payroll with ID {id} not found.");
                    }
                    await _payroll.DeletePayroll(id);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting payroll with ID {id}", ex);
                }
            }
        }

        
    }

