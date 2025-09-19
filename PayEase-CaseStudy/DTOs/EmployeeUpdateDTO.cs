using System.ComponentModel.DataAnnotations;

namespace PayEase_CaseStudy.DTOs
{
    public class EmployeeUpdateDTO
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        public DateTime? DOB { get; set; }

        [MaxLength(15)]
        public string ContactNumber { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

    }
}
