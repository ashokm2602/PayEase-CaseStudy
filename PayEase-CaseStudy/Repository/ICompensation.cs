using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.Repository
{
    public interface ICompensation
    {
        public Task<List<CompensationAdjustment>> GetAllCompensations();
        public Task<CompensationAdjustment> GetCompensationById(int id);
        public Task<CompensationAdjustment> AddCompensation(CompensationDTO compensation);
        public Task<CompensationAdjustment> UpdateCompensation(int id, CompensationDTO compensation);
        public Task DeleteCompensation(int id);
    }
}
