using Microsoft.EntityFrameworkCore;
using PayEase_CaseStudy.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PayEase_CaseStudy.Models
{
    public class PayDbContext : DbContext
    {

        public PayDbContext(DbContextOptions<PayDbContext> options)
            : base(options)
        {
        }

        

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<PayrollDetail> PayrollDetails { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<CompensationAdjustment> CompensationAdjustments { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DeptId);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.PayrollDetails)
                .WithOne(pd => pd.Employee)
                .HasForeignKey(pd => pd.EmpId);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Leaves)
                .WithOne(l => l.Employee)
                .HasForeignKey(l => l.EmpId);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.CompensationAdjustments)
                .WithOne(ca => ca.Employee)
                .HasForeignKey(ca => ca.EmpId);

            modelBuilder.Entity<Payroll>()
                .HasMany(p => p.PayrollDetails)
                .WithOne(pd => pd.Payroll)
                .HasForeignKey(pd => pd.PayrollId);
        }
    }
}
