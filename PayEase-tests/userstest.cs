using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Identity;
using PayEase_CaseStudy.Authentication;
using PayEase_CaseStudy.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayEase_CaseStudy.Tests
{
    [TestFixture]
    public class UsersTest
    {
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private UserRepo _userRepo;

        [SetUp]
        public void Setup()
        {
            var storeMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                storeMock.Object, null, null, null, null, null, null, null, null);

            _userRepo = new UserRepo(_userManagerMock.Object);
        }

        [Test]
        public async Task GetAllUsers_Should_Return_All_Users()
        {
            // Arrange
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { UserName = "alice", Id = "1" },
                new ApplicationUser { UserName = "bob", Id = "2" }
            }.AsQueryable();

            _userManagerMock.Setup(um => um.Users).Returns(users);

            // Act
            var result = await _userRepo.GetAllUsers();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].UserName, Is.EqualTo("alice"));
            Assert.That(result[1].UserName, Is.EqualTo("bob"));
        }

        [Test]
        public async Task GetUserById_Should_Return_Correct_User()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "charlie", Id = "123" };
            _userManagerMock.Setup(um => um.FindByIdAsync("123"))
                .ReturnsAsync(user);

            // Act
            var result = await _userRepo.GetUserById("123");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserName, Is.EqualTo("charlie"));
        }
    }
}