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
    public class LeavesController : ControllerBase
    {
        private readonly ILeave _leave;

        public LeavesController(ILeave leave)
        {
            _leave = leave;
        }

        // GET: api/Leaves
        [HttpGet("GetAllLeaves")]
        public async Task<ActionResult<IList<Leave>>> GetLeaves()
        {
            try
            {
                var leaves = await _leave.GetAllLeaves();
                if (leaves == null || leaves.Count == 0)
                {
                    return NotFound("No leaves found.");
                }
                return Ok(leaves);
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving leaves", e);
            }
        }

        // GET: api/Leaves/5
        [HttpGet("GetLeaveById{id}")]
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

        // PUT: api/Leaves/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateLeave{id}")]
        public async Task<IActionResult> UpdateLeave(int id, LeaveDTO leave)
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

        // POST: api/Leaves
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddLeave")]
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


        // DELETE: api/Leaves/5
        [HttpDelete("DeleteLeave{id}")]
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
