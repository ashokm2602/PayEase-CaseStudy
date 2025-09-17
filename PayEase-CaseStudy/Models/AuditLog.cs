using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayEase_CaseStudy.Models
{
    public class AuditLog
    {
        [Key]
        public int LogId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public string Action { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        // Navigation
        public User User { get; set; }

    }
}
