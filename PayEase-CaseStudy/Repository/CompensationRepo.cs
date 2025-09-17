using Microsoft.EntityFrameworkCore;
using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public class CompensationRepo : ICompensation
    {
        private readonly PayDbContext _context;
        public CompensationRepo(PayDbContext context)
        {
            _context = context;
        }
        public async Task<List<CompensationAdjustment>> GetAllCompensations()
        {
            try
            {
                var compensations = await _context.CompensationAdjustments.ToListAsync();
                if (compensations == null || !compensations.Any())
                {
                    throw new Exception("No compensations found.");
                }
                return compensations;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                throw new Exception("An error occurred while retrieving compensations.", ex);
            }
        }
        public async Task<CompensationAdjustment> GetCompensationById(int id)
        {
            try
            {
                var compensation = await _context.CompensationAdjustments.FindAsync(id);
                if (compensation == null)
                {
                    throw new KeyNotFoundException($"Compensation with ID {id} not found.");
                }
                return compensation;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                throw new Exception("An error occurred while retrieving the compensation.", ex);
            }
        }

        public async Task<CompensationAdjustment> AddCompensation(CompensationDTO compensation)
        {
            try
            {
                if (compensation == null)
                    throw new ArgumentNullException(nameof(compensation), "Compensation cannot be null");
                var comp = new CompensationAdjustment
                {
                    EmpId = compensation.EmpId,
                    Amount = compensation.Amount,
                    Category = compensation.Category,
                    AppliedDate = compensation.AppliedDate,
                    AdjustmentType = compensation.AdjustmentType
                };
                _context.CompensationAdjustments.Add(comp);
                await _context.SaveChangesAsync();
                return comp;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                throw new Exception("An error occurred while adding the compensation.", ex);
            }
        }

        public async Task<CompensationAdjustment> UpdateCompensation(int id, CompensationDTO compensation)
        {
            try
            {
                var existingCompensation = await _context.CompensationAdjustments.FindAsync(id);
                if (existingCompensation == null)
                    throw new Exception($"Compensation with ID {id} not found");
                existingCompensation.EmpId = compensation.EmpId;
                existingCompensation.Amount = compensation.Amount;
                existingCompensation.Category = compensation.Category;
                existingCompensation.AppliedDate = compensation.AppliedDate;
                existingCompensation.AdjustmentType = compensation.AdjustmentType;
                _context.CompensationAdjustments.Update(existingCompensation);
                await _context.SaveChangesAsync();
                return existingCompensation;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating compensation with ID {id}", ex);
            }
        }

        public async Task DeleteCompensation(int id)
        {
            try
            {
                var compensation = await _context.CompensationAdjustments.FindAsync(id);
                if (compensation == null)
                    throw new Exception($"Compensation with ID {id} not found");
                _context.CompensationAdjustments.Remove(compensation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting compensation with ID {id}", ex);
            }
        }
    }
}
