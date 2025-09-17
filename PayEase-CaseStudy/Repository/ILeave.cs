using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public interface ILeave
    {
        public Task<List<Leave>> GetAllLeaves();
        public Task<Leave> GetLeaveById(int id);
        public Task<Leave> AddLeave(LeaveDTO leave);
        public Task<Leave> UpdateLeave(int id, LeaveDTO leave);
        public Task DeleteLeave(int id);
    }
}
