using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;
using PayEase_CaseStudy.Repository;

namespace PayEase_CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        private readonly ILeave _leave;

        public LeavesController(ILeave leave)
        {
            _leave = leave;
        }

        // GET: api/Leaves/GetAllLeaves
        [HttpGet("GetAllLeaves")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IList<LeaveWithEmployeeDTO>>> GetAllLeaves()
        {
            try
            {
                var leaves = await _leave.GetAllLeavesWithEmployeeNames();

                if (leaves == null || leaves.Count == 0)
                {
                    return NotFound("No leaves found.");
                }

                return Ok(leaves);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error retrieving leaves: {e.Message}");
            }
        }

        // GET: api/Leaves/GetLeaveById5
        [HttpGet("GetLeaveById{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Leave>> GetLeave(int id)
        {
            try
            {
                var leave = await _leave.GetLeaveById(id);
                if (leave == null)
                {
                    return NotFound();
                }
                return Ok(leave);
            }
            catch (Exception e)
            {
                throw new Exception($"Error retrieving leave with ID {id}", e);
            }
        }

        // PUT: api/Leaves/UpdateLeave5
        [HttpPut("UpdateLeave{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLeave(int id, string leave)
        {
            try
            {
                var updatedLeave = await _leave.UpdateLeave(id, leave);
                if (updatedLeave == null)
                {
                    return NotFound($"Leave with ID {id} not found.");
                }
                return Ok(updatedLeave);
            }
            catch (Exception e)
            {
                throw new Exception($"Error updating leave with ID {id}", e);
            }
        }

        // POST: api/Leaves/AddLeave
        [HttpPost("AddLeave")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<Leave>> PostLeave(LeaveDTO leave)
        {
            try
            {
                var newLeave = await _leave.AddLeave(leave);
                if (newLeave == null)
                {
                    return BadRequest("Failed to create leave.");
                }
                return CreatedAtAction("GetLeave", new { id = newLeave.LeaveId }, newLeave);
            }
            catch (Exception e)
            {
                throw new Exception("Error creating leave", e);
            }
        }
        // GET: api/Leaves/GetLeavesByEmployeeId5
        [HttpGet("GetLeavesByEmployeeId{employeeId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IList<LeaveWithEmployeeDTO>>> GetLeavesByEmployeeId(int employeeId)
        {
            try
            {
                var leaves = await _leave.GetLeavesByEmployeeId(employeeId);

                if (leaves == null || leaves.Count == 0)
                {
                    return NotFound("No leaves found for this employee.");
                }

                return Ok(leaves);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error retrieving leaves: {e.Message}");
            }
        }

        // DELETE: api/Leaves/DeleteLeave5
        [HttpDelete("DeleteLeave{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteLeave(int id)
        {
            try
            {
                var leave = await _leave.GetLeaveById(id);
                if (leave == null)
                {
                    return NotFound();
                }
                await _leave.DeleteLeave(id);
                return NoContent();
            }
            catch (Exception e)
            {
                throw new Exception($"Error deleting leave with ID {id}", e);
            }
        }
    }
}
