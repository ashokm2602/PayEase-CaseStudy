using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayEase_CaseStudy.Models;
using PayEase_CaseStudy.Repository;

namespace PayEase_CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRole _role;

        public RolesController(IRole role)
        {
            _role = role;
        }

        // GET: api/Roles
        [HttpGet("GetAllRoles")]
        public async Task<ActionResult<IEnumerable<Role>>> GetAllRoles()
        {
            try
            {
                var roles = await _role.GetAllRoles();
                if (roles == null || roles.Count == 0)
                {
                    return NotFound("No roles found.");
                }
                return Ok(roles);
            }
            catch(Exception ex)
            {
                throw new Exception("Error retrieving roles", ex);
            }
        }

        // GET: api/Roles/5
        [HttpGet("GetRolesById{id}")]
        public async Task<ActionResult<Role>> GetRoleById(int id)
        {
            try
            {
                var role = await _role.GetRoleById(id);
                if (role == null)
                {
                    return NotFound($"Role with ID {id} not found.");
                }
                return Ok(role);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error retrieving role with ID {id}", ex);
            }
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateRole{id}")]
        public async Task<IActionResult> UpdateRole(int id, string rolename)
        {
            try
            {
                var role = await _role.UpdateRole(id, rolename);
                if (role == null)
                {
                    return NotFound($"Role with ID {id} not found.");
                }
                return Ok(role);
            } 
            catch(Exception ex)
            {
                throw new Exception($"Error updating role with ID {id}", ex);
            }

        }

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddRole")]
        public async Task<ActionResult<Role>> PostRole(string rolename)
        {
            try
            {
                var role = await _role.AddRole(rolename);
                if (role == null)
                {
                    return BadRequest("Failed to create role.");
                }
                return CreatedAtAction("GetRoleById", new { id = role.RoleId }, role);
            }
            catch(Exception ex)
            {
                throw new Exception("Error creating role", ex);
            }
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                await _role.DeleteRole(id);
                return NoContent();
            }
            catch(Exception ex)
            {
                throw new Exception($"Error deleting role with ID {id}", ex);
            }
        }

        
    }
}
