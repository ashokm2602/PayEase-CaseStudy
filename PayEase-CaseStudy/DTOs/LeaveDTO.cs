using System.ComponentModel.DataAnnotations;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.DTOs
{
    public class LeaveDTO
    {
        [Required]
        public int EmpId { get; set; }

        [Required, MaxLength(50)]
        public string LeaveType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; }
    }
}
