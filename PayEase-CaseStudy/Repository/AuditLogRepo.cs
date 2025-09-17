using Microsoft.EntityFrameworkCore;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public class AuditLogRepo : IAuditLog
    {
        private readonly PayDbContext _context;
        public AuditLogRepo(PayDbContext context)
        {
            _context = context;
        }
        public async Task<List<AuditLog>> GetAllAuditLogs()
        {
            try
            {
                var auditLogs = await _context.AuditLogs.ToListAsync();
                if (auditLogs == null || !auditLogs.Any())
                {
                    throw new Exception("No audit logs found.");
                }
                return auditLogs;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                throw new Exception("An error occurred while retrieving audit logs.", ex);
            }
        }

        public async Task<AuditLog> GetAuditLogById(int id)
        {
            try
            {
                var auditLog = await _context.AuditLogs.FindAsync(id);
                if (auditLog == null)
                {
                    throw new KeyNotFoundException($"AuditLog with ID {id} not found.");
                }
                return auditLog;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                throw new Exception("An error occurred while retrieving the audit log.", ex);
            }
        }
    }
}
