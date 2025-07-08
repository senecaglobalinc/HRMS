using System;
using HRMS.Admin.API.Controllers;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Service;
using HRMS.Admin.Types;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections;
using System.Threading.Tasks;

namespace HRMS.Admin.Tests
{
    [TestClass]
    public class KeyFunctionControllerUnitTest
    {
        #region Global Variables 
        private Mock<ILogger<KeyFunctionController>> _logger = new Mock<ILogger<KeyFunctionController>>();
        private Mock<ILogger<KeyFunctionService>> logger = new Mock<ILogger<KeyFunctionService>>();
        private KeyFunctionController controller;
        private string controllerName = "keyFunction";
        SGRole keyFunction = new SGRole()
        {
            SGRoleID = 10,
            SGRoleName = "Network Engineer",
            DepartmentId = 1,
            CurrentUser = "Anonymous",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = null,
            SystemInfo = null,
            IsActive = true,
            CreatedBy = "Anonymous",
            ModifiedBy = null
        };
        #endregion

        #region Get TestCases
        [TestMethod]
        public async Task GetAllAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(GetAllAsync));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.GetAll();
            var expectedResult = response.Result.GetType();
            ICollection collection = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as ICollection;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(collection.Count, 1);
        }

        [TestMethod]
        public async Task GetAllAsync_Active()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(GetAllAsync_Active));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.GetAll(true);
            var expectedResult = response.Result.GetType();
            ICollection collection = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as ICollection;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(collection.Count, 1);
        }

        [TestMethod]
        public async Task GetAllAsync_InActive()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(GetAllAsync_InActive));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.GetAll(false);
            var expectedResult = response.Result.GetType();
            ICollection collection = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as ICollection;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(collection.Count, 1);
        }

        #endregion

        #region Create Testcases
        [TestMethod]
        public async Task CreateAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(CreateAsync));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.Create(keyFunction);
            SGRole data = (SGRole)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(data.SGRoleName, "Network Engineer");
        }

        #endregion

        #region Update TestCases

        [TestMethod]
        public async Task UpdateAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(UpdateAsync));
            ConfigureTest(dbContext, out controller);
            keyFunction.SGRoleName = "Network Engineer";
            keyFunction.SGRoleID = 1;
            //Act
            var response = await controller.Update(keyFunction);
            SGRole updatedData = (SGRole)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(updatedData.SGRoleName, "Network Engineer");
        }

        [TestMethod]
        public async Task UpdateAsync_BadRequest_InvalidSuffixID()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(UpdateAsync_BadRequest_InvalidSuffixID));
            ConfigureTest(dbContext, out controller);
            keyFunction.SGRoleName = "Network Engineer";
            keyFunction.SGRoleID = 100;
            //Act
            var response = await controller.Update(keyFunction);
            var expectedResult = response.GetType();

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "BadRequestObjectResult");
        }

        #endregion

        #region ConfigureTest
        private void ConfigureTest(AdminContext dbContext, out KeyFunctionController controller)
        {
            IKeyFunctionService m_KeyFunctionService = new KeyFunctionService(dbContext, logger.Object);
            controller = new KeyFunctionController(m_KeyFunctionService, _logger.Object);
        }
        #endregion
    }
}
