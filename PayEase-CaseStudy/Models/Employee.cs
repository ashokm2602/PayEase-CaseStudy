using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayEase_CaseStudy.Models
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        public DateTime? DOB { get; set; }
        public DateTime HireDate { get; set; }

        [ForeignKey("Department")]
        public int DeptId { get; set; }

        [MaxLength(15)]
        public string ContactNumber { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseSalary { get; set; }

        // Navigation
        public User User { get; set; }
        public Department Department { get; set; }
        public ICollection<Leave> Leaves { get; set; }
        public ICollection<PayrollDetail> PayrollDetails { get; set; }
        public ICollection<CompensationAdjustment> CompensationAdjustments { get; set; }

    }
}
