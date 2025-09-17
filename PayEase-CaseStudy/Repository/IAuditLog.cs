using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public interface IAuditLog
    {
        public Task<List<AuditLog>> GetAllAuditLogs();
        public Task<AuditLog> GetAuditLogById(int id);
    }
}
