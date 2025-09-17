using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PayEase_CaseStudy.Authentication;
using PayEase_CaseStudy.Services;

namespace PayEase_CaseStudy.Models
{
    public class PayDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly ICurrentUserService _currentUserService;

        public PayDbContext(DbContextOptions<PayDbContext> options, ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<PayrollDetail> PayrollDetails { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<CompensationAdjustment> CompensationAdjustments { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await AddAuditLogs();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private async Task AddAuditLogs()
        {
            var userId = _currentUserService.GetUserId() ?? "SYSTEM";

            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is not AuditLog &&
                            (e.State == EntityState.Added ||
                             e.State == EntityState.Modified ||
                             e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                var audit = new AuditLog
                {
                    Action = entry.State.ToString(), // Added, Modified, Deleted
                    Timestamp = DateTime.UtcNow,
                    ApplicationUserId = userId
                };

                await AuditLogs.AddAsync(audit);
            }
        }


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
