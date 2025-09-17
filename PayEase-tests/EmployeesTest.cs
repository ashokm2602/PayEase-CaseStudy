using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PayEase_CaseStudy.Models;
using PayEase_CaseStudy.DTOs;
using PayEase_CaseStudy.Repository;
using PayEase_CaseStudy.Authentication;

namespace PayEase_CaseStudy.Tests
{
    [TestFixture]
    public class EmployeesTest
    {
        private PayDbContext _context;
        private EmployeeRepo _employeeRepo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;
            var fakeUserService = new FakeCurrentUserService("TestUser");

            _context = new PayDbContext(options, fakeUserService);

            _context.Database.EnsureCreated();

            _context.Add(new ApplicationUser {UserName = "admin", Email = "admin@mail.com", PasswordHash = "pass" });
            _context.Departments.Add(new Department { DeptId = 1, DeptName = "HR" });
            _context.SaveChanges();

            _employeeRepo = new EmployeeRepo(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddEmployee_Should_Add_Employee_To_Db()
        {
            var dto = new EmployeeDTO
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                DOB = new DateTime(1990, 1, 1),
                HireDate = DateTime.Today,
                DeptId = 1,
                ContactNumber = "1234567890",
                Address = "123 Main St",
                BaseSalary = 50000
            };

            var result = await _employeeRepo.AddEmployee(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.FirstName, Is.EqualTo("John"));
            Assert.That(result.BaseSalary, Is.EqualTo(50000));
        }

        [Test]
        public async Task GetAllEmployees_Should_Return_All_Employees()
        {
            await _employeeRepo.AddEmployee(new EmployeeDTO { UserId = 1, FirstName = "Alice", LastName = "Smith", DOB = DateTime.Today.AddYears(-30), HireDate = DateTime.Today, DeptId = 1, ContactNumber = "111111", Address = "Addr1", BaseSalary = 40000 });
            await _employeeRepo.AddEmployee(new EmployeeDTO { UserId = 1, FirstName = "Bob", LastName = "Brown", DOB = DateTime.Today.AddYears(-25), HireDate = DateTime.Today, DeptId = 1, ContactNumber = "222222", Address = "Addr2", BaseSalary = 45000 });

            var employees = await _employeeRepo.GetAllEmployees();

            Assert.That(employees, Is.Not.Null);
            Assert.That(employees.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetEmployeeById_Should_Return_Correct_Employee()
        {
            var emp = await _employeeRepo.AddEmployee(new EmployeeDTO { UserId = 1, FirstName = "Charlie", LastName = "Green", DOB = DateTime.Today.AddYears(-28), HireDate = DateTime.Today, DeptId = 1, ContactNumber = "333333", Address = "Addr3", BaseSalary = 48000 });

            var result = await _employeeRepo.GetEmployeeById(emp.EmpId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.FirstName, Is.EqualTo("Charlie"));
        }

        [Test]
        public async Task UpdateEmployee_Should_Update_Employee_Details()
        {
            var emp = await _employeeRepo.AddEmployee(new EmployeeDTO { UserId = 1, FirstName = "David", LastName = "White", DOB = DateTime.Today.AddYears(-32), HireDate = DateTime.Today, DeptId = 1, ContactNumber = "444444", Address = "Addr4", BaseSalary = 52000 });

            var updated = await _employeeRepo.UpdateEmployee(emp.EmpId, new EmployeeDTO { UserId = 1, FirstName = "DavidUpdated", LastName = "White", DOB = emp.DOB, HireDate = emp.HireDate, DeptId = 1, ContactNumber = "555555", Address = "New Addr", BaseSalary = 55000 });

            Assert.That(updated.FirstName, Is.EqualTo("DavidUpdated"));
            Assert.That(updated.BaseSalary, Is.EqualTo(55000));
        }

        [Test]
        public async Task DeleteEmployee_Should_Remove_Employee_From_Db()
        {
            var emp = await _employeeRepo.AddEmployee(new EmployeeDTO { UserId = 1, FirstName = "Eve", LastName = "Black", DOB = DateTime.Today.AddYears(-29), HireDate = DateTime.Today, DeptId = 1, ContactNumber = "666666", Address = "Addr5", BaseSalary = 47000 });

            await _employeeRepo.DeleteEmployee(emp.EmpId);

            var result = await _employeeRepo.GetEmployeeById(emp.EmpId);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void DeleteEmployee_NonExistentEmployee_Should_Throw_Exception()
        {
            Assert.ThrowsAsync<Exception>(async () => await _employeeRepo.DeleteEmployee(999));
        }
    }
}
