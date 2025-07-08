using HRMS.Admin.API.Controllers;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Service;
using HRMS.Admin.Types;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Admin.Tests
{
    [TestClass]
    public class UserControllerUnitTest
    {
        private Mock<ILogger<UserController>> _logger;
        private Mock<ILogger<UserService>> logger;

        #region Get Test Cases

        #region GetAllAsync
        [TestMethod]
        public async Task GetAllAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllAsync));
            IUserService m_UserService;
            UserController m_Controller;

            ConfigureTest(dbContext, out m_UserService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetAllUsers(null);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(value, null);
        }
        #endregion

        #region GetAllAsync_Active
        [TestMethod]
        public async Task GetAllAsync_Active()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllAsync_Active));

            IUserService m_UserService;
            UserController m_Controller;

            ConfigureTest(dbContext, out m_UserService,
                out m_Controller);

            var response = await m_Controller.GetAllUsers(true);

            var users = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<User>;
            int inActiveRecords = users.FindAll(s => s.IsActive == false).Count;

            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("OkObjectResult".Equals(actualResult.Name.ToString()) && inActiveRecords == 0);
        }
        #endregion  

        #endregion

        #region ConfigureTest
        private void ConfigureTest(AdminContext dbContext, out IUserService m_UserService,
            out UserController m_Controller)
        {
            _logger = new Mock<ILogger<UserController>>();
            logger = new Mock<ILogger<UserService>>();
            m_UserService = new UserService(logger.Object, dbContext, null, null,null, null);
            m_Controller = new UserController(m_UserService, _logger.Object);
        }
        #endregion
    }
}
