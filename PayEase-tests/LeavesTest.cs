using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using PayEase_CaseStudy.Models;
using PayEase_CaseStudy.Repository;
using PayEase_CaseStudy.DTOs;

namespace PayEase_CaseStudy.Tests
{
    [TestFixture]
    public class LeavesTest
    {
        private PayDbContext _context;
        private LeaveRepo _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;


            _context = new PayDbContext(options); _context.Database.EnsureCreated();

            // Seed some leaves
            _context.Leaves.AddRange(new List<Leave>
            {
                new Leave { LeaveId = 1, EmpId = 1, LeaveType = "Sick", StartDate = DateTime.Now.AddDays(-5), EndDate = DateTime.Now.AddDays(-3), Status = "Approved" },
                new Leave { LeaveId = 2, EmpId = 2, LeaveType = "Casual", StartDate = DateTime.Now.AddDays(-2), EndDate = DateTime.Now, Status = "Pending" }
            });
            _context.SaveChanges();

            _repo = new LeaveRepo(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllLeaves_Should_Return_All_Leaves()
        {
            var result = await _repo.GetAllLeaves();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetLeaveById_Should_Return_Correct_Leave()
        {
            var result = await _repo.GetLeaveById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.LeaveId, Is.EqualTo(1));
            Assert.That(result.LeaveType, Is.EqualTo("Sick"));
        }

        [Test]
        public void GetLeaveById_InvalidId_Should_Throw_KeyNotFoundException()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _repo.GetLeaveById(999);
            });
        }

        [Test]
        public async Task AddLeave_Should_Add_New_Leave()
        {
            var dto = new LeaveDTO
            {
                EmpId = 3,
                LeaveType = "Casual",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                Status = "Pending"
            };

            var result = await _repo.AddLeave(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.EmpId, Is.EqualTo(3));
            Assert.That(result.LeaveType, Is.EqualTo("Casual"));
        }

        [Test]
        public async Task UpdateLeave_Should_Update_Leave_Status()
        {
            var result = await _repo.UpdateLeave(1, "Approved");

            Assert.That(result.Status, Is.EqualTo("Approved"));
        }


        [Test]
        public async Task DeleteLeave_Should_Remove_Leave()
        {
            await _repo.DeleteLeave(2);

            var leaves = await _repo.GetAllLeaves();
            Assert.That(leaves.Count, Is.EqualTo(1));
            Assert.That(leaves.Exists(l => l.LeaveId == 2), Is.False);
        }
    }
}