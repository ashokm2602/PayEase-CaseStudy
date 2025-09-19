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

        // 📌 Get all audit logs
        public async Task<List<AuditLog>> GetAllAuditLogs()
        {
            return await _context.AuditLogs
                .Include(a => a.User) // optional: include user info
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }

        // 📌 Get a single audit log by ID
        public async Task<AuditLog> GetAuditLogById(int id)
        {
            var auditLog = await _context.AuditLogs
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.LogId == id);

            if (auditLog == null)
                throw new KeyNotFoundException($"AuditLog with ID {id} not found.");

            return auditLog;
        }

        // 📌 Create an audit log entry
       
    }
}
