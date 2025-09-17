using System.ComponentModel.DataAnnotations;

namespace PayEase_CaseStudy.DTOs
{
    public class PayrollDTO
    {
        [Required]
        public DateTime PayrollPeriodStart { get; set; }

        [Required]
        public DateTime PayrollPeriodEnd { get; set; }

        [Required]
        public DateTime ProcessedDate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; }
    }
}
