using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.DTOs
{
    public class CompensationDTO
    {
        [Required]
        public int EmpId { get; set; }

        [Required, MaxLength(50)]
        public string AdjustmentType { get; set; } // e.g. Bonus, Tax, PF, Loan

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required, MaxLength(20)]
        public string Category { get; set; } // "Benefit" or "Deduction"
        [Required]
        public DateTime AppliedDate { get; set; }

    }
}
