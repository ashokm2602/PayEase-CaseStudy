using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PayEase_CaseStudy.Models;

namespace PayEase_CaseStudy.DTOs
{
    public class PayrollDetailDTO
    {
        [Required]
        public int PayrollId { get; set; }
        [Required]
        public int EmpId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicSalary { get; set; }
        
    }
}
