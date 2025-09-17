using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PayEase_CaseStudy.Authentication;

namespace PayEase_CaseStudy.Models
{
    public class AuditLog
    {
        [Key]
        public int LogId { get; set; }

        // Foreign key to Identity User
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public string Action { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
