using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class PayrollDetailsController : ControllerBase
    {
        private readonly IPayrollDetail _payrollDetail;

        public PayrollDetailsController(IPayrollDetail payrollDetail)
        {
            _payrollDetail = payrollDetail;
        }

        // GET: api/PayrollDetails
        [HttpGet("GetAllPayrollDetails")]
        public async Task<ActionResult<List<PayrollDetail>>> GetAllPayrollDetails()
        {
            try
            {
                var list = await _payrollDetail.GetAllPayrollDetails();
                if (list == null || list.Count == 0)
                {
                    return NotFound("No payroll details found.");
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving payroll details", e);
            }
        }

        // GET: api/PayrollDetails/5
        [HttpGet("GetPayrollDetailById{id}")]
        public async Task<ActionResult<PayrollDetail>> GetPayrollDetailById(int id)
        {
            try
            {
                var payrollDetail = await _payrollDetail.GetPayrollDetailById(id);
                if (payrollDetail == null)
                {
                    return NotFound();
                }
                return Ok(payrollDetail);
            }
            catch (Exception e)
            {
                throw new Exception($"Error retrieving payroll detail with ID {id}", e);
            }
        }

        // PUT: api/PayrollDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdatePayrollDetail{id}")]
        public async Task<IActionResult> UpdatePayrollDetail(int id, PayrollDetailDTO payrollDetail)
        {
            try
            {
                var updatedPayrollDetail = await _payrollDetail.UpdatePayrollDetail(id, payrollDetail);
                if (updatedPayrollDetail == null)
                {
                    return NotFound($"Payroll detail with ID {id} not found.");
                }
                return Ok(updatedPayrollDetail);
            }
            catch (Exception e)
            {
                throw new Exception($"Error updating payroll detail with ID {id}", e);
            }
        }

        // POST: api/PayrollDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddPayrollDetail")]
        public async Task<ActionResult<PayrollDetail>> AddPayrollDetail(PayrollDetailDTO payrollDetail)
        {
            try
            {
                var newPayrollDetail = await _payrollDetail.AddPayrollDetail(payrollDetail);
                if (newPayrollDetail == null)
                {
                    return BadRequest("Failed to add payroll detail.");
                }
                return CreatedAtAction("GetPayrollDetailById", new { id = newPayrollDetail.PayrollDetailId }, newPayrollDetail);
            }
            catch (Exception e)
            {
                throw new Exception("Error adding payroll detail", e);
            }
        }

        // DELETE: api/PayrollDetails/5
        [HttpDelete("DeletePayrollDetail{id}")]
        public async Task<IActionResult> DeletePayrollDetail(int id)
        {
            try
            {
                var detail = await _payrollDetail.GetPayrollDetailById(id);
                if(detail == null)
                {
                    return NotFound($"Payroll detail with ID {id} not found.");
                }
                await _payrollDetail.DeletePayrollDetail(id);
                return NoContent();
            }
            catch (Exception e)
            {
                throw new Exception($"Error deleting payroll detail with ID {id}", e);


            }
        }
        
    }
}
