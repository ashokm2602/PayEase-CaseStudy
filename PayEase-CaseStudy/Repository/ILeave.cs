using System.Collections.Generic;
using System.Threading.Tasks;
using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public interface ILeave
    {
        Task<List<Leave>> GetAllLeaves();
        Task<Leave> GetLeaveById(int id);
        Task<Leave> AddLeave(LeaveDTO leave);
        Task<Leave> UpdateLeave(int id, string leave);
        Task DeleteLeave(int id);
        // New method to get leaves with employee names

        Task<List<LeaveWithEmployeeDTO>> GetLeavesByEmployeeId(int employeeId); 
        Task<List<LeaveWithEmployeeDTO>> GetAllLeavesWithEmployeeNames();
        Task<int> GetPendingLeavesCount();
        Task<int> GetPendingLeavesCountByEmployee(int employeeId);
        Task<int> GetTotalLeavesCountByEmployee(int employeeId);

    }
}
