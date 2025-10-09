using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PayEase_CaseStudy.Models;
using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Repository;

namespace PayEase_CaseStudy.Tests
{
    [TestFixture]
    public class PayrollsTest
    {
        private PayDbContext _context;
        private PayrollRepo _payrollRepo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;


            _context = new PayDbContext(options); _context.Database.EnsureCreated();

            _payrollRepo = new PayrollRepo(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddPayroll_Should_Add_Payroll_To_Db()
        {
            var dto = new PayrollDTO
            {
                PayrollPeriodStart = new DateTime(2025, 1, 1),
                PayrollPeriodEnd = new DateTime(2025, 1, 31),
                ProcessedDate = DateTime.Today,
                Status = "Processed"
            };

            var result = await _payrollRepo.AddPayroll(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo("Processed"));
            Assert.That(result.PayrollPeriodStart, Is.EqualTo(new DateTime(2025, 1, 1)));
        }

        [Test]
        public async Task GetAllPayrolls_Should_Return_All_Payrolls()
        {
            await _payrollRepo.AddPayroll(new PayrollDTO { PayrollPeriodStart = DateTime.Today.AddMonths(-1), PayrollPeriodEnd = DateTime.Today, ProcessedDate = DateTime.Today, Status = "Processed" });
            await _payrollRepo.AddPayroll(new PayrollDTO { PayrollPeriodStart = DateTime.Today.AddMonths(-2), PayrollPeriodEnd = DateTime.Today.AddMonths(-1), ProcessedDate = DateTime.Today, Status = "Pending" });

            var payrolls = await _payrollRepo.GetAllPayrolls();

            Assert.That(payrolls, Is.Not.Null);
            Assert.That(payrolls.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetPayrollById_Should_Return_Correct_Payroll()
        {
            var payroll = await _payrollRepo.AddPayroll(new PayrollDTO { PayrollPeriodStart = DateTime.Today, PayrollPeriodEnd = DateTime.Today.AddDays(15), ProcessedDate = DateTime.Today, Status = "Processed" });

            var result = await _payrollRepo.GetPayrollById(payroll.PayrollId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo("Processed"));
        }

        [Test]
        public async Task UpdatePayroll_Should_Update_Payroll_Details()
        {
            var payroll = await _payrollRepo.AddPayroll(new PayrollDTO { PayrollPeriodStart = DateTime.Today, PayrollPeriodEnd = DateTime.Today.AddDays(15), ProcessedDate = DateTime.Today, Status = "Pending" });

            var updated = await _payrollRepo.UpdatePayroll(payroll.PayrollId, new PayrollDTO { PayrollPeriodStart = payroll.PayrollPeriodStart, PayrollPeriodEnd = payroll.PayrollPeriodEnd, ProcessedDate = payroll.ProcessedDate, Status = "Processed" });

            Assert.That(updated.Status, Is.EqualTo("Processed"));
        }

        [Test]
        public async Task DeletePayroll_Should_Remove_Payroll_From_Db()
        {
            var payroll = await _payrollRepo.AddPayroll(new PayrollDTO { PayrollPeriodStart = DateTime.Today, PayrollPeriodEnd = DateTime.Today.AddDays(15), ProcessedDate = DateTime.Today, Status = "Pending" });

            await _payrollRepo.DeletePayroll(payroll.PayrollId);

            var result = await _payrollRepo.GetPayrollById(payroll.PayrollId);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void DeletePayroll_NonExistent_Should_Throw_Exception()
        {
            Assert.ThrowsAsync<Exception>(async () => await _payrollRepo.DeletePayroll(999));
        }
    }
}