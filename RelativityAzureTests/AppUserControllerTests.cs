using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RelativityAzurePojekt.Controllers;
using RelativityAzurePojekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Reflection;

namespace RelativityAzureTests
{
    [TestClass]
    public class AppUserControllerTests
    {
        private AppUserController _controller;
        private MyDatabaseContext _context;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;

        [TestInitialize]
        public void Setup()
        {
            var loggerMock = new Mock<ILogger<AppUserController>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(context);

            var options = new DbContextOptionsBuilder<MyDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryAppUserDatabase")
                .Options;

            _context = new MyDatabaseContext(options);

            _controller = new AppUserController(loggerMock.Object, _context, _httpContextAccessorMock.Object);
        }

        [TestMethod]
        public void Index_ReturnsViewResult()
        {
            var result = _controller.Index() as ViewResult;

            Assert.IsNotNull(result);
        }

        /*
        [TestMethod]
        public async Task Login_ValidUser_ReturnsIndexView()
        {
            var user = new AppUser
            {
                FirstName = "Robert",
                LastName = "Kubica",
                Passwd = "haslo"
            };

            _context.AppUser.Add(user);
            await _context.SaveChangesAsync();

            var result = await _controller.Login(user) as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("~/Views/Home/Index.cshtml", result.ViewName);

        }
        */

        [TestMethod]
        public async Task Register_ValidUser_RedirectsToLoginView()
        {
            var user = new AppUser
            {
                FirstName = "Robert",
                LastName = "Kubica",
                Passwd = "haslo"
            };

            var result = await _controller.Register(user) as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("~/Views/AppUser/Login.cshtml", result.ViewName);

            var userInDatabase = await _context.AppUser.FirstOrDefaultAsync(u => u.FirstName == user.FirstName && u.LastName == user.LastName);
            Assert.IsNotNull(userInDatabase);
            Assert.AreEqual(user.FirstName, userInDatabase.FirstName);
            Assert.AreEqual(user.LastName, userInDatabase.LastName);
            Assert.AreEqual(user.Passwd, userInDatabase.Passwd);
        }

        [TestMethod]
        public async Task Register_InvalidUser_ReturnsIndexView()
        {
            var user = new AppUser();

            var result = await _controller.Register(user) as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("~/Views/Home/Index.cshtml", result.ViewName);

            var userInDatabase = await _context.AppUser.FirstOrDefaultAsync(u => u.FirstName == user.FirstName && u.LastName == user.LastName);
            Assert.IsNull(userInDatabase);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }
    }
}