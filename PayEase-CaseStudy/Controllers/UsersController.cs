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
    public class UsersController : ControllerBase
    {
        private readonly IUser _user;

        public UsersController(IUser user)
        {
            _user = user;
        }

        // GET: api/Users
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {
                var users = await _user.GetAllUsers();
                if(users == null)
                    return NotFound("No users found.");
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Users/5
        [HttpGet("GetUserById{id}")]
        public async Task<ActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _user.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateUser{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDTO user)
        {
            try
            {
                var updateduser = await _user.UpdateUser(id, user);
                if (updateduser == null)
                    return NotFound();
                return Ok(updateduser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> AddUser(UserDTO user)
        {
            try
            {
                var newuser = await _user.AddUser(user);
                if (newuser == null)
                    return BadRequest("Could not create user");
                return CreatedAtAction("GetUserById", new { id = newuser.UserId }, newuser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _user.DeleteUser(id);
                return NoContent();
            }
            catch(Exception e)
            {
                throw new Exception($"Error deleting user with ID {id}", e);
            }

        }

        
    }
}
