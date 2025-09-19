using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PayEase_CaseStudy.Authentication
{
    [Table("AspNetUsers")]

    public class ApplicationUser : IdentityUser
    {

    }
}
