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
    public class UserRoleControllerUnitTest
    {
        private Mock<ILogger<UserRoleController>> _logger;
        private Mock<ILogger<UserRoleService>> logger;

        #region Get Test Cases

        #region GetAllUserRolesAsync
        [TestMethod]
        public async Task GetAllUserRolesAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllUserRolesAsync));
            IUserRoleService m_UserRoleService;
            UserRoleController m_Controller;

            ConfigureTest(dbContext, out m_UserRoleService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetAllUserRoles(null);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(value, null);
        }
        #endregion

        #region GetAllUserRolesAsync_Active
        [TestMethod]
        public async Task GetAllUserRolesAsync_Active()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllUserRolesAsync_Active));

            IUserRoleService m_UserRoleService;
            UserRoleController m_Controller;

            ConfigureTest(dbContext, out m_UserRoleService,
                out m_Controller);

            var response = await m_Controller.GetAllUserRoles(true);

            var UserRoles = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<UserRole>;
            int inActiveRecords = UserRoles.FindAll(s => s.IsActive == false).Count;

            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("OkObjectResult".Equals(actualResult.Name.ToString()) && inActiveRecords == 0);
        }
        #endregion

        #region GetAllRolesAsync
        [TestMethod]
        public async Task GetAllRolesAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllRolesAsync));
            IUserRoleService m_UserRoleService;
            UserRoleController m_Controller;

            ConfigureTest(dbContext, out m_UserRoleService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetAllRoles(null);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(value, null);
        }
        #endregion

        #region GetAllRolesAsync_Active
        [TestMethod]
        public async Task GetAllRolesAsync_Active()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllRolesAsync_Active));

            IUserRoleService m_UserRoleService;
            UserRoleController m_Controller;

            ConfigureTest(dbContext, out m_UserRoleService,
                out m_Controller);

            var response = await m_Controller.GetAllRoles(true);

            var Roles = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<Role>;
            int inActiveRecords = Roles.FindAll(s => s.IsActive == false).Count;

            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("OkObjectResult".Equals(actualResult.Name.ToString()) && inActiveRecords == 0);
        }
        #endregion

        #region GetUserNameOnLoginAsync
        [TestMethod]
        public async Task GetUserNameOnLoginAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetUserNameOnLoginAsync));

            IUserRoleService m_UserRoleService;
            UserRoleController m_Controller;

            ConfigureTest(dbContext, out m_UserRoleService,
                out m_Controller);

            var response = await m_Controller.GetUserRoleOnLogin("TestUser1@senecaglobal.com");

            var Roles = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<Role>;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("OkObjectResult".Equals(actualResult.Name.ToString()) && 1.Equals(Roles.Count));
        }
        #endregion

        #endregion

        #region ConfigureTest
        private void ConfigureTest(AdminContext dbContext, out IUserRoleService m_UserRoleService,
            out UserRoleController m_Controller)
        {
            _logger = new Mock<ILogger<UserRoleController>>();
            logger = new Mock<ILogger<UserRoleService>>();
            m_UserRoleService = new UserRoleService(logger.Object, dbContext, null, null, null);
            m_Controller = new UserRoleController(m_UserRoleService, _logger.Object);
        }
        #endregion
    }
}
