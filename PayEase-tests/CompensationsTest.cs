using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayEase_CaseStudy.Models;
using PayEase_CaseStudy.Repository;
using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Services;

namespace PayEase_CaseStudy.Tests
{
    // Fake service to simulate current user
   

    [TestFixture]
    public class CompensationsTest
    {
        private PayDbContext _context;
        private CompensationRepo _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            // Reuse the FakeCurrentUserService from AuditLogs test
            var fakeUserService = new FakeCurrentUserService("TestUser");

            _context = new PayDbContext(options, fakeUserService);
            _context.Database.EnsureCreated();

            // Seed some compensations
            _context.CompensationAdjustments.AddRange(new List<CompensationAdjustment>
    {
        new CompensationAdjustment { AdjustmentId = 1, EmpId = 1, Amount = 1000, Category = "Bonus", AppliedDate = DateTime.Now.AddDays(-10), AdjustmentType = "Benefit" },
        new CompensationAdjustment { AdjustmentId = 2, EmpId = 2, Amount = 500, Category = "Penalty", AppliedDate = DateTime.Now.AddDays(-5), AdjustmentType = "Deduction" }
    });
            _context.SaveChanges();

            _repo = new CompensationRepo(_context);
        }


        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllCompensations_Should_Return_All()
        {
            var result = await _repo.GetAllCompensations();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetCompensationById_Should_Return_Correct()
        {
            var result = await _repo.GetCompensationById(1);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.EmpId, Is.EqualTo(1));
            Assert.That(result.Amount, Is.EqualTo(1000));
        }

        [Test]
        public void GetCompensationById_Invalid_Should_Throw()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _repo.GetCompensationById(999));
        }

        [Test]
        public async Task AddCompensation_Should_Add_New()
        {
            var dto = new CompensationDTO
            {
                EmpId = 3,
                Amount = 2000,
                Category = "Incentive",
                AppliedDate = DateTime.Now,
                AdjustmentType = "Benefit"
            };

            var result = await _repo.AddCompensation(dto);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.EmpId, Is.EqualTo(3));
            Assert.That(result.Amount, Is.EqualTo(2000));
        }

        [Test]
        public async Task UpdateCompensation_Should_Update_Existing()
        {
            var dto = new CompensationDTO
            {
                EmpId = 1,
                Amount = 1500,
                Category = "Bonus Updated",
                AppliedDate = DateTime.Now,
                AdjustmentType = "Benefit"
            };

            var result = await _repo.UpdateCompensation(1, dto);
            Assert.That(result.Amount, Is.EqualTo(1500));
            Assert.That(result.Category, Is.EqualTo("Bonus Updated"));
        }

        [Test]
        public async Task DeleteCompensation_Should_Remove()
        {
            await _repo.DeleteCompensation(2);
            var all = await _repo.GetAllCompensations();
            Assert.That(all.Count, Is.EqualTo(1));
            Assert.That(all.Exists(c => c.AdjustmentId == 2), Is.False);
        }
    }
}
