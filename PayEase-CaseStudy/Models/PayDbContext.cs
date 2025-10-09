using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PayEase_CaseStudy.Authentication;

namespace PayEase_CaseStudy.Models
{
    public class PayDbContext : IdentityDbContext<ApplicationUser>
    {
        public PayDbContext(DbContextOptions<PayDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<PayrollDetail> PayrollDetails { get; set; }
        public DbSet<CompensationAdjustment> CompensationAdjustments { get; set; }
        public DbSet<Leave> Leaves { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }  

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // This creates all Identity tables

            // Fluent API configurations (if needed)
            builder.Entity<Department>().HasKey(d => d.DeptId);
            builder.Entity<Employee>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(e => e.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Employee>()
                .HasOne(d => d.Department)
                .WithMany()
                .HasForeignKey(e => e.DeptId);

            builder.Entity<CompensationAdjustment>()
                .HasOne(e => e.Employee)
                .WithMany()
                .HasForeignKey(c => c.EmpId);

            builder.Entity<Leave>()
                .HasOne(e => e.Employee)
                .WithMany()
                .HasForeignKey(l => l.EmpId);

            builder.Entity<PayrollDetail>()
                .HasOne(e => e.Employee)
                .WithMany()
                .HasForeignKey(p => p.EmpId);

            builder.Entity<PayrollDetail>()
                .HasOne(p => p.Payroll)
                .WithMany()
                .HasForeignKey(p => p.PayrollId);

           
        }
    }
}
