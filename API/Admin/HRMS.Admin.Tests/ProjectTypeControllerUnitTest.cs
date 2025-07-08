using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using HRMS.Admin.API.Controllers;
using Moq;
using Microsoft.Extensions.Logging;
using HRMS.Admin.Service;
using HRMS.Admin.Types;
using HRMS.Admin.Entities;
using System.Collections;
using HRMS.Admin.Database;

namespace HRMS.Admin.Tests
{
    [TestClass]
    public class ProjectTypeControllerUnitTest
    {
        #region Global Variables
        private Mock<ILogger<ProjectTypeController>> _logger = new Mock<ILogger<ProjectTypeController>>();
        private Mock<ILogger<ProjectTypeService>> logger = new Mock<ILogger<ProjectTypeService>>();
        private ProjectTypeController controller;
        private string controllerName = "ProjectType";
        private ProjectType projectType = new ProjectType()
        {
            ProjectTypeId = 9,
            ProjectTypeCode = "PZ00009",
            Description = "something",
            CurrentUser = "Anonymous",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = null,
            SystemInfo = null,
            IsActive = true,
            CreatedBy = "Anonymous",
            ModifiedBy = null

        };
        #endregion

        #region Create TestCases
        [TestMethod]
        public async Task CreateAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(CreateAsync));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.Create(projectType);
            ProjectType data = (ProjectType)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(data.ProjectTypeCode, "PZ00009");
        }

        [TestMethod]
        public async Task CreateAsync_BadRequest_CodeAlreadyExists()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(CreateAsync_BadRequest_CodeAlreadyExists));
            ConfigureTest(dbContext, out controller);
            projectType.ProjectTypeCode = "PZ00001";

            //Act
            var response = await controller.Create(projectType);
            var expectedResult = response.GetType();

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "BadRequestObjectResult");
        }

        #endregion

        #region Update TestCases
        [TestMethod]
        public async Task UpdateAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(UpdateAsync));
            ConfigureTest(dbContext, out controller);
            projectType.ProjectTypeId = 2;
            projectType.ProjectTypeCode = "uPZ00002";

            //Act
            var response = await controller.Update(projectType);
            ProjectType data = (ProjectType)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(data.ProjectTypeCode, "uPZ00002");
        }

        [TestMethod]
        public async Task UpdateAsync_BadRequest_CodeAlreadyExists()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(UpdateAsync_BadRequest_CodeAlreadyExists));
            ConfigureTest(dbContext, out controller);
            projectType.ProjectTypeCode = "PZ00001";

            //Act
            var response = await controller.Update(projectType);
            var expectedResult = response.GetType();

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "BadRequestObjectResult");
        }

        [TestMethod]
        public async Task UpdateAsync_BadRequest_InvalidProjectTypeId()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(UpdateAsync_BadRequest_InvalidProjectTypeId));
            ConfigureTest(dbContext, out controller);
            projectType.ProjectTypeId = 11;

            //Act
            var response = await controller.Update(projectType);
            var expectedResult = response.GetType();

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "BadRequestObjectResult");
        }
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
            Assert.AreEqual(collection.Count, 3);
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
            Assert.AreEqual(collection.Count, 3);
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

        #region ConfigureTest
        private void ConfigureTest(AdminContext dbContext, out ProjectTypeController controller)
        {
            IProjectTypeService m_ProjectTypeService = new ProjectTypeService(logger.Object, dbContext);
            controller = new ProjectTypeController(m_ProjectTypeService, _logger.Object);

        }
        #endregion
    }
}
