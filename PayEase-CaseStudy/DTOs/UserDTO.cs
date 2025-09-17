using System.ComponentModel.DataAnnotations;

namespace PayEase_CaseStudy.DTOs
{
    public class UserDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}
