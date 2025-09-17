using Microsoft.EntityFrameworkCore;
using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public class LeaveRepo : ILeave
    {
        private readonly PayDbContext _context;
        public LeaveRepo(PayDbContext context)
        {
            _context = context;
        }

        public async Task<List<Leave>> GetAllLeaves()
        {
            try
            {
                var leaves = await _context.Leaves.ToListAsync();
                if (leaves == null || !leaves.Any())
                {
                    throw new Exception("No leaves found.");
                }
                return leaves;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                throw new Exception("An error occurred while retrieving leaves.", ex);
            }
        }

        public async Task<Leave> GetLeaveById(int id)
        {
            try
            {
                var leave = await _context.Leaves.FindAsync(id);
                if (leave == null)
                {
                    throw new KeyNotFoundException($"Leave with ID {id} not found.");
                }
                return leave;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                throw new Exception("An error occurred while retrieving the leave.", ex);
            }
        }

        public async Task<Leave> AddLeave(LeaveDTO leave)
        {
            try
            {
                if (leave == null)
                    throw new ArgumentNullException(nameof(leave), "Leave cannot be null");
                var lev = new Leave
                {
                    EmpId = leave.EmpId,
                    LeaveType = leave.LeaveType,
                    StartDate = leave.StartDate,
                    EndDate = leave.EndDate,
                };
                _context.Leaves.Add(lev);
                await _context.SaveChangesAsync();
                return lev;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding leave", ex);
            }
        }

        public async Task<Leave> UpdateLeave(int id, LeaveDTO leave)
        {
            try
            {
                if (leave == null)
                    throw new ArgumentNullException(nameof(leave), "Leave cannot be null");
                var existingLeave = await _context.Leaves.FindAsync(id);
                if (existingLeave == null)
                {
                    throw new KeyNotFoundException($"Leave with ID {id} not found.");
                }
                existingLeave.EmpId = leave.EmpId;
                existingLeave.LeaveType = leave.LeaveType;
                existingLeave.StartDate = leave.StartDate;
                existingLeave.EndDate = leave.EndDate;
                existingLeave.Status = leave.Status;
                _context.Leaves.Update(existingLeave);
                await _context.SaveChangesAsync();
                return existingLeave;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating leave with ID {id}", ex);
            }
        }

        public async Task DeleteLeave(int id)
        {
            try
            {
                var leave = await _context.Leaves.FindAsync(id);
                if (leave == null)
                    throw new Exception($"Leave with ID {id} not found");
                _context.Leaves.Remove(leave);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting leave with ID {id}", ex);
            }
        }
    }
}
