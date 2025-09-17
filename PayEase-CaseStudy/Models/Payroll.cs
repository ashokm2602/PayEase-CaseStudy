using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PayEase_CaseStudy.Models
{
    public class Payroll
    {
        [Key]
        public int PayrollId { get; set; }

        [Required]
        public DateTime PayrollPeriodStart { get; set; }

        [Required]
        public DateTime PayrollPeriodEnd { get; set; }

        [Required]
        public DateTime ProcessedDate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Processed, Verified

        // Navigation
        [JsonIgnore]
        public ICollection<PayrollDetail> PayrollDetails { get; set; }
    }
}
