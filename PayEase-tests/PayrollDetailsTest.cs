using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using PayEase_CaseStudy.Models;
using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Repository;

namespace PayEase_CaseStudy.Tests
{
    [TestFixture]
    public class PayrollDetailRepoTests
    {
        private PayDbContext _context;
        private PayrollDetailRepo _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var fakeUserService = new FakeCurrentUserService("TestUser");

            _context = new PayDbContext(options, fakeUserService); _context.Database.EnsureCreated();

            // Seed an Employee for PayrollDetail tests
            _context.Employees.Add(new Employee { EmpId = 1, BaseSalary = 5000, FirstName = "John", LastName = "Doe" });
            _context.SaveChanges();

            _repo = new PayrollDetailRepo(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddPayrollDetail_Should_Add_PayrollDetail()
        {
            var dto = new PayrollDetailDTO
            {
                PayrollId = 1,
                EmpId = 1,
                BasicSalary = 6000
            };

            var result = await _repo.AddPayrollDetail(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.BasicSalary, Is.EqualTo(6000));
            Assert.That(result.EmpId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetPayrollDetailById_Should_Return_Correct_PayrollDetail()
        {
            var dto = new PayrollDetailDTO { PayrollId = 1, EmpId = 1, BasicSalary = 6000 };
            var added = await _repo.AddPayrollDetail(dto);

            var result = await _repo.GetPayrollDetailById(added.PayrollDetailId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.PayrollDetailId, Is.EqualTo(added.PayrollDetailId));
        }

        [Test]
        public async Task GetAllPayrollDetails_Should_Return_All_Details()
        {
            await _repo.AddPayrollDetail(new PayrollDetailDTO { PayrollId = 1, EmpId = 1, BasicSalary = 6000 });
            await _repo.AddPayrollDetail(new PayrollDetailDTO { PayrollId = 2, EmpId = 1, BasicSalary = 7000 });

            var result = await _repo.GetAllPayrollDetails();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task UpdatePayrollDetail_Should_Update_BasicSalary()
        {
            var dto = new PayrollDetailDTO { PayrollId = 1, EmpId = 1, BasicSalary = 6000 };
            var added = await _repo.AddPayrollDetail(dto);

            var updateDto = new PayrollDetailDTO { PayrollId = 1, EmpId = 1, BasicSalary = 8000 };
            var updated = await _repo.UpdatePayrollDetail(added.PayrollDetailId, updateDto);

            Assert.That(updated.BasicSalary, Is.EqualTo(8000));
        }

        [Test]
        public async Task DeletePayrollDetail_Should_Remove_From_Db()
        {
            var dto = new PayrollDetailDTO { PayrollId = 1, EmpId = 1, BasicSalary = 6000 };
            var added = await _repo.AddPayrollDetail(dto);

            await _repo.DeletePayrollDetail(added.PayrollDetailId);

            var result = await _context.PayrollDetails.FindAsync(added.PayrollDetailId);
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CalculateNetSalary_Should_Correctly_Calculate_NetSalary()
        {
            var dto = new PayrollDetailDTO { PayrollId = 1, EmpId = 1, BasicSalary = 5000 };
            var added = await _repo.AddPayrollDetail(dto);

            // Seed compensation adjustments
            _context.CompensationAdjustments.Add(new CompensationAdjustment { EmpId = 1, Amount = 1000, AdjustmentType = "Benefit" });
            _context.CompensationAdjustments.Add(new CompensationAdjustment { EmpId = 1, Amount = 500, AdjustmentType = "Deduction" });
            await _context.SaveChangesAsync();

            var netSalary = await _repo.CalculateNetSalary(added.PayrollDetailId);

            Assert.That(netSalary, Is.EqualTo(5500)); // 5000 + 1000 - 500
        }
    }
}
