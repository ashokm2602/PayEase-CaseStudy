using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PayEase_CaseStudy.Models
{
    public class Department
    {
        [Key]
        public int DeptId { get; set; }

        [Required, MaxLength(100)]
        public string DeptName { get; set; }

        // Navigation
        [JsonIgnore]
        public ICollection<Employee> Employees { get; set; }

    }
}
