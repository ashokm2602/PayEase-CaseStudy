using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PayEase_CaseStudy.Models
{
    public class CompensationAdjustment
    {
        [Key]
        public int AdjustmentId { get; set; }

        [ForeignKey("Employee")]
        public int EmpId { get; set; }
        [JsonIgnore]
        public Employee Employee { get; set; }

        [Required, MaxLength(50)]
        public string AdjustmentType { get; set; } // e.g. Bonus, Tax, PF, Loan

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required, MaxLength(20)]
        public string Category { get; set; } // "Benefit" or "Deduction"

        public DateTime AppliedDate { get; set; } = DateTime.Now;

    }
}
