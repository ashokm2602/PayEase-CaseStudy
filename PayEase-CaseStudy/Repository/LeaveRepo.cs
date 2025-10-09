using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                throw new Exception("An error occurred while retrieving leaves.", ex);
            }
        }

        // Fetch leaves with employee names with null-safety
        public async Task<List<LeaveWithEmployeeDTO>> GetAllLeavesWithEmployeeNames()
        {
            try
            {
                var leavesWithEmployees = await _context.Leaves
    // .Include(l => l.Employee)  // Temporarily comment out
    .Select(l => new LeaveWithEmployeeDTO
    {
        LeaveId = l.LeaveId,
        EmpId = l.EmpId,
        EmployeeName = (l.Employee != null) ? l.Employee.FirstName + " " + l.Employee.LastName : "Unknown",

        LeaveType = l.LeaveType,
        StartDate = l.StartDate,
        EndDate = l.EndDate,
        Status = l.Status
    })
    .ToListAsync();


                return leavesWithEmployees ?? new List<LeaveWithEmployeeDTO>();
                

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving leaves with employee names.", ex);
            }
        }

        public async Task<int> GetPendingLeavesCount()
        {
            return await _context.Leaves.CountAsync(l => l.Status == "pending");
        }

        public async Task<Leave> GetLeaveById(int id)
        {
            try
            {
                var leave = await _context.Leaves.FindAsync(id);
                if (leave == null)
                    throw new KeyNotFoundException($"Leave with ID {id} not found.");
                return leave;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the leave.", ex);
            }
        }

        public async Task<Leave> AddLeave(LeaveDTO leave)
        {
            try
            {
                if (leave == null)
                    throw new ArgumentNullException(nameof(leave), "Leave cannot be null");

                if (leave.StartDate < DateTime.Today || leave.EndDate < DateTime.Today)
                    throw new ArgumentException("Leave start and end dates cannot be before today.");

                var lev = new Leave
                {
                    EmpId = leave.EmpId,
                    LeaveType = leave.LeaveType,
                    StartDate = leave.StartDate,
                    EndDate = leave.EndDate,
                    Status = "Pending"
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

        public async Task<Leave> UpdateLeave(int id, string leave)
        {
            try
            {
                if (leave == null)
                    throw new ArgumentNullException(nameof(leave), "Leave cannot be null");

                var existingLeave = await _context.Leaves.FindAsync(id);
                if (existingLeave == null)
                    throw new KeyNotFoundException($"Leave with ID {id} not found.");

                existingLeave.Status = leave;

                _context.Leaves.Update(existingLeave);
                await _context.SaveChangesAsync();
                return existingLeave;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating leave with ID {id}", ex);
            }
        }
        public async Task<List<LeaveWithEmployeeDTO>> GetLeavesByEmployeeId(int employeeId)
        {
            try
            {
                var leaves = await _context.Leaves
                    .Where(l => l.EmpId == employeeId)
                    //.Include(l => l.Employee) // if needed
                    .Select(l => new LeaveWithEmployeeDTO
                    {
                        LeaveId = l.LeaveId,
                        EmpId = l.EmpId,
                        EmployeeName = (l.Employee != null) ? l.Employee.FirstName + " " + l.Employee.LastName : "Unknown",
                        LeaveType = l.LeaveType,
                        StartDate = l.StartDate,
                        EndDate = l.EndDate,
                        Status = l.Status,
                       
                    })
                    .ToListAsync();

                return leaves;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve leaves for employee", ex);
            }
        }

        public async Task<int> GetPendingLeavesCountByEmployee(int employeeId)
        {
            return await _context.Leaves
                .Where(l => l.EmpId == employeeId && l.Status == "Pending")
                .CountAsync();
        }

        public async Task<int> GetTotalLeavesCountByEmployee(int employeeId)
        {
            return await _context.Leaves
                .Where(l => l.EmpId == employeeId)
                .CountAsync();
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
