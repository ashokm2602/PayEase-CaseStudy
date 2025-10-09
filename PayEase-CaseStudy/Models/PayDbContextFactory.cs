using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PayEase_CaseStudy.Models
{
    public class PayDbContextFactory : IDesignTimeDbContextFactory<PayDbContext>
    {
        public PayDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PayDbContext>();

            // Use the same connection string as in Program.cs
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=PayRoll_Database;Integrated Security=True;TrustServerCertificate=True;");

            return new PayDbContext(optionsBuilder.Options);
        }
    }
}

