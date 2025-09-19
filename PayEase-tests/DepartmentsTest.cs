using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PayEase_CaseStudy.Models;
using PayEase_CaseStudy.Repository;
using PayEase_CaseStudy.Services;

namespace PayEase_CaseStudy.Tests
{
    [TestFixture]
    public class DepartmentsTest
    {
        private PayDbContext _context;
        private DepartmentRepo _departmentRepo;
        private readonly ICurrentUserService currentUserService;
        private readonly IAuditLog auditLogRepo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Fresh DB for each test
                .Options;
            var fakeUserService = new FakeCurrentUserService("TestUser");

            _context = new PayDbContext(options);
            _context.Database.EnsureCreated();

            _departmentRepo = new DepartmentRepo(_context,currentUserService,auditLogRepo);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddDepartment_Should_Add_Department_To_Db()
        {
            var deptName = "Finance";

            var result = await _departmentRepo.AddDepartment(deptName);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.DeptName, Is.EqualTo(deptName));
        }

        [Test]
        public async Task GetAllDepartments_Should_Return_All_Departments()
        {
            await _departmentRepo.AddDepartment("HR");
            await _departmentRepo.AddDepartment("IT");

            var departments = await _departmentRepo.GetAllDepartments();

            Assert.That(departments, Is.Not.Null);
            Assert.That(departments.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetDepartmentById_Should_Return_Correct_Department()
        {
            var addedDept = await _departmentRepo.AddDepartment("Operations");

            var result = await _departmentRepo.GetDepartmentById(addedDept.DeptId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.DeptName, Is.EqualTo("Operations"));
        }

        [Test]
        public async Task UpdateDepartment_Should_Update_DepartmentName()
        {
            var addedDept = await _departmentRepo.AddDepartment("Support");

            var updatedDept = await _departmentRepo.UpdateDepartment(addedDept.DeptId, "Customer Support");

            Assert.That(updatedDept, Is.Not.Null);
            Assert.That(updatedDept.DeptName, Is.EqualTo("Customer Support"));
        }

        [Test]
        public async Task DeleteDepartment_Should_Remove_Department_From_Db()
        {
            var dept = await _departmentRepo.AddDepartment("Legal");

            await _departmentRepo.DeleteDepartment(dept.DeptId);

            var result = await _departmentRepo.GetDepartmentById(dept.DeptId);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void DeleteDepartment_NonExistentDepartment_Should_Throw_Exception()
        {
            Assert.ThrowsAsync<Exception>(async () => await _departmentRepo.DeleteDepartment(999));
        }
    }
}
