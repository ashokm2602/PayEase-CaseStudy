using Microsoft.EntityFrameworkCore;

namespace PayEase_CaseStudy.Models
{
    public class PayDbContext : DbContext
    {
        public PayDbContext(DbContextOptions<PayDbContext> options) : base(options) 
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<PayrollDetail> PayrollDetails { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<CompensationAdjustment> CompensationAdjustments { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relationships and constraints

            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<Employee>(e => e.UserId);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);

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

            modelBuilder.Entity<User>()
                .HasMany(u => u.AuditLogs)
                .WithOne(al => al.User)
                .HasForeignKey(al => al.UserId);
        }
    }
}
