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
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartment _department;

        public DepartmentsController(IDepartment department)
        {
            _department = department;
        }

        // GET: api/Departments
        [HttpGet("GetAllDepartments")]
        public async Task<ActionResult<IList<Department>>> GetAllDepartments()
        {
            try
            {
                 var departments = await _department.GetAllDepartments();
                if (departments == null || departments.Count == 0)
                    {
                        return NotFound("No departments found.");
                }
                return Ok(departments);
            }
            catch(Exception e)
            {
                throw new Exception("Error retrieving departments", e);
            }
        }

        // GET: api/Departments/5
        [HttpGet("GetDepartmentById{id}")]
        public async Task<ActionResult<Department>> GetDepartmentById(int id)
        {
            try
            {
                var department = await _department.GetDepartmentById(id);

                if (department == null)
                {
                    return NotFound();
                }

                return Ok(department);
            }
            catch(Exception e)
            {
                throw new Exception($"Error retrieving department with ID {id}", e);
            }
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    
        

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Department>> AddDepartment(string departmentname)
        {
            try
            {
                var department = await _department.AddDepartment(departmentname);
                if (department == null)
                    return BadRequest("Failed to add department.");
                return Ok(department);
            }
            catch(Exception e)
            {
                throw new Exception("Error adding department", e);
            }
        }

        // DELETE: api/Departments/5
        [HttpDelete("DeleteDepartment{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var department = await _department.GetDepartmentById(id);
                if (department == null)
                {
                    return NotFound();
                }
                await _department.DeleteDepartment(id);
                return Ok(department);
            }
            catch (Exception e)
            {
                throw new Exception($"Error deleting department with ID {id}", e);
            }

        }
    }
}
