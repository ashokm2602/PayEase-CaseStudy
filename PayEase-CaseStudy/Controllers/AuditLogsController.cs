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
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLog _audit;

        public AuditLogsController(IAuditLog audit)
        {
            _audit = audit;
        }

        // GET: api/AuditLogs
        [HttpGet("GetAllAuditLogs")]
        public async Task<ActionResult<List<AuditLog>>> GetAllAuditLogs()
        {
            try
            {
                var list = await _audit.GetAllAuditLogs();
                if (list == null || list.Count == 0)
                {
                    return NotFound("No audit logs found.");
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving audit logs", e);
            }
        }

        // GET: api/AuditLogs/5
        [HttpGet("GetAuditLogById{id}")]
        public async Task<ActionResult<AuditLog>> GetAuditLogById(int id)
        {
            try
            {
                var auditLog = await _audit.GetAuditLogById(id);
                if (auditLog == null)
                {
                    return NotFound();
                }
                return Ok(auditLog);
            }
            catch (Exception e)
            {
                throw new Exception($"Error retrieving audit log with ID {id}", e);
            }
        }

        
        
    }
}
