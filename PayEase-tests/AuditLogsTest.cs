using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayEase_CaseStudy.Models;
using PayEase_CaseStudy.Repository;
using PayEase_CaseStudy.Services;

namespace PayEase_CaseStudy.Tests
{
    // Fake service to simulate logged-in user
    public class FakeCurrentUserService : ICurrentUserService
    {
        private readonly string _userId;

        public FakeCurrentUserService(string userId)
        {
            _userId = userId;
        }

        public string GetUserId()
        {
            return _userId;
        }
    }

    [TestFixture]
    public class AuditLogsTest
    {
        private PayDbContext _context;
        private AuditLogRepo _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            // Register FakeCurrentUserService
            var services = new ServiceCollection();
            services.AddScoped<ICurrentUserService, FakeCurrentUserService>();
            var serviceProvider = services.BuildServiceProvider();

            // Resolve the fake user service from DI
            var fakeUserService = serviceProvider.GetRequiredService<ICurrentUserService>();

            // ✅ Pass the fake service directly, not the whole provider
            _context = new PayDbContext(options);
            _context.Database.EnsureCreated();

            // Seed some audit logs
            _context.AuditLogs.AddRange(new List<AuditLog>
    {
        new AuditLog { LogId = 1, Action = "Add User", Timestamp = DateTime.Now },
        new AuditLog { LogId = 2, Action = "Update Payroll", Timestamp = DateTime.Now }
    });
            _context.SaveChanges();

            _repo = new AuditLogRepo(_context);
        }


        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllAuditLogs_Should_Return_All_Logs()
        {
            var result = await _repo.GetAllAuditLogs();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAuditLogById_Should_Return_Correct_Log()
        {
            var result = await _repo.GetAuditLogById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.LogId, Is.EqualTo(1));
            Assert.That(result.Action, Is.EqualTo("Add User"));
        }

        [Test]
        public void GetAuditLogById_InvalidId_Should_Throw_KeyNotFoundException()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _repo.GetAuditLogById(999);
            });
        }

        [Test]
        public void GetAllAuditLogs_EmptyDb_Should_Throw_Exception()
        {
            _context.AuditLogs.RemoveRange(_context.AuditLogs);
            _context.SaveChanges();

            Assert.ThrowsAsync<Exception>(async () =>
            {
                await _repo.GetAllAuditLogs();
            });
        }
    }
}
