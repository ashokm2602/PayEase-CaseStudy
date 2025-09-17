using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayEase_CaseStudy.Models
{
    public class Leave
    {
        [Key]
        public int LeaveId { get; set; }

        [ForeignKey("Employee")]
        public int EmpId { get; set; }
        public Employee Employee { get; set; }

        [Required, MaxLength(50)]
        public string LeaveType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    }
}
