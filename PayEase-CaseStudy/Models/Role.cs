using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PayEase_CaseStudy.Models
{
    public class Role
    {

        [Key]
        public int RoleId { get; set; }

        [Required, MaxLength(50)]
        public string RoleName { get; set; }

        // Navigation
        [JsonIgnore]
        public ICollection<User> Users { get; set; }

         

    }
}
