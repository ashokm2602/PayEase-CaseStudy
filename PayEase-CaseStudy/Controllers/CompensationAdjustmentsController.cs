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
    public class CompensationAdjustmentsController : ControllerBase
    {
        private readonly ICompensation _comp;

        public CompensationAdjustmentsController(ICompensation comp)
        {
           _comp = comp;
        }

        // GET: api/CompensationAdjustments
        [HttpGet("GetAllCompensationAdjustments")]
        public async Task<ActionResult<IEnumerable<CompensationAdjustment>>> GetCompensations()
        {
            try
            {
                var list = await _comp.GetAllCompensations();
                if (list == null || list.Count == 0)
                {
                    return NotFound("No compensation adjustments found.");
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving compensation adjustments", e);
            }
        }

        // GET: api/CompensationAdjustments/5
        [HttpGet("GetCompensationById{id}")]
        public async Task<ActionResult<CompensationAdjustment>> GetCompensationById(int id)
        {
            try
            {
                var compensationAdjustment = await _comp.GetCompensationById(id);
                if (compensationAdjustment == null)
                {
                    return NotFound();
                }
                return Ok(compensationAdjustment);
            }
            catch (Exception e)
            {
                throw new Exception($"Error retrieving compensation adjustment with ID {id}", e);
            }
        }
        

        // PUT: api/CompensationAdjustments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateCompensation{id}")]
        public async Task<IActionResult> UpdateCompensation(int id, CompensationDTO compensationAdjustment)
        {
            try
            {
                var comp = await _comp.GetCompensationById(id);
                if(comp == null)
                {
                    return NotFound($"Compensation with ID {id} not found.");
                }
                var updatedComp = await _comp.UpdateCompensation(id, compensationAdjustment);
                return Ok(updatedComp);
            }
            catch (Exception e)
            {
                throw new Exception($"Error updating compensation adjustment with ID {id}", e);
            }
        }

        // POST: api/CompensationAdjustments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddCompensation")]
        public async Task<ActionResult<CompensationAdjustment>> AddCompensation(CompensationDTO compensationAdjustment)
        {
            try
            {
                var comp = await _comp.AddCompensation(compensationAdjustment);
                if(comp == null)
                {
                    return BadRequest("Failed to add compensation adjustment.");
                }
                return CreatedAtAction("GetCompensationById", new { id = comp.AdjustmentId }, comp);

            }
            catch (Exception e)
            {
                throw new Exception("Error adding compensation adjustment", e);
            }
        }

        // DELETE: api/CompensationAdjustments/5
        [HttpDelete("DeleteCompensation{id}")]
        public async Task<IActionResult> DeleteCompensationAdjustment(int id)
        {
            try
            {
                var comp = await _comp.GetCompensationById(id);
                if(comp == null)
                {
                    return NotFound($"Compensation with ID {id} not found.");
                }
                await _comp.DeleteCompensation(id);
                return NoContent();
            }
            catch (Exception e)
            {
                throw new Exception($"Error deleting compensation adjustment with ID {id}", e);
            }
        }

        
    }
}
