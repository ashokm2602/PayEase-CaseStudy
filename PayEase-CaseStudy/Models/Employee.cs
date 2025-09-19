using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PayEase_CaseStudy.Authentication;

namespace PayEase_CaseStudy.Models
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }

        // FK to Identity User
        public string? ApplicationUserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? User { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        public DateTime? DOB { get; set; }
        public DateTime HireDate { get; set; }

        public int DeptId { get; set; }
        [JsonIgnore]
        public Department Department { get; set; }

        [MaxLength(15)]
        public string ContactNumber { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseSalary { get; set; }

        // Navigation
        [JsonIgnore]
        public ICollection<Leave> Leaves { get; set; }
        [JsonIgnore]
        public ICollection<PayrollDetail> PayrollDetails { get; set; }
        [JsonIgnore]
        public ICollection<CompensationAdjustment> CompensationAdjustments { get; set; }
    }

}
