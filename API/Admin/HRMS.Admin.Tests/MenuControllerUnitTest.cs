using HRMS.Admin.API.Controllers;
using HRMS.Admin.Database;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Service;
using HRMS.Admin.Types;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Tests
{
    [TestClass]
    public class MenuControllerUnitTest
    {
        #region GlobalVariables
        private Mock<ILogger<MenuController>> _logger = new Mock<ILogger<MenuController>>();
        private Mock<ILogger<MenuService>> logger = new Mock<ILogger<MenuService>>();
        private MenuController controller;
        private MenuRoleDetails menuRoles = new MenuRoleDetails()
        {
            RoleId = 1,
            MenuList = new List<Menus>()
            {
                new Menus{MenuId=5},
                new Menus{MenuId=6},
                new Menus{MenuId=7},
                new Menus{MenuId=8}
            }
        };
        private string controllerName = "Menu";
        #endregion

        #region Get Test Cases

        [TestMethod]
        public async Task GetSourceMenuRoles()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(GetSourceMenuRoles));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.GetSourceMenuRoles(1);
            var expectedResult = response.GetType();
            ICollection collection = ((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value as ICollection;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(collection.Count, 2);
        }

        [TestMethod]
        public async Task GetSourceMenuRoles_BadRequest()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(GetSourceMenuRoles_BadRequest));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.GetSourceMenuRoles(0);
            var expectedResult = response.GetType();
            ICollection collection = ((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value as ICollection;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(collection, null);
        }

        [TestMethod]
        public async Task GetTargetMenuRoles()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(GetTargetMenuRoles));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.GetTargetMenuRoles(1);
            var expectedResult = response.GetType();
            ICollection collection = ((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value as ICollection;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(collection.Count, 4);
        }

        #endregion

        #region Create TestCases
        //Transactions are not supported in Memory database
        //[TestMethod]  
        //public async Task CreateAsync()
        //{
        //    //Arrange
        //    var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(CreateAsync));
        //    ConfigureTest(dbContext, out controller);

        //    //Act
        //    var response = await controller.AddTargetMenuRoles(menuRoles);
        //    var result = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

        //    dbContext.Dispose();

        //    //Assert
        //    Assert.AreEqual(result, true);
        //}

        #endregion

        #region ConfigureTest
        private void ConfigureTest(AdminContext dbContext, out MenuController controller)
        {
            IMenuService m_Service = new MenuService(logger.Object, dbContext);
            controller = new MenuController(m_Service, _logger.Object);
        }
        #endregion
    }
}