using System.ComponentModel.DataAnnotations;

namespace PayEase_CaseStudy.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string UserId { get; set; }  // Foreign Key to AspNetUsers

        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime Created { get; set; }
    }

}