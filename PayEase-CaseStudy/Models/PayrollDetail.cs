using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PayEase_CaseStudy.Models
{
    public class PayrollDetail
    {
        [Key]
        public int PayrollDetailId { get; set; }

        [ForeignKey("Payroll")]
        public int PayrollId { get; set; }
        
        public Payroll Payroll { get; set; }

        [ForeignKey("Employee")]
        public int EmpId { get; set; }
        [JsonIgnore]
        public Employee Employee { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicSalary { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetSalary { get; set; }


    }
}
