using HRMS.Admin.API.Controllers;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Service;
using HRMS.Admin.Types;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Admin.Tests
{
    [TestClass]
    public class DepartmentControllerUnitTest
    {
        #region GlobalVariables
        private Mock<ILogger<DepartmentController>> _logger = new Mock<ILogger<DepartmentController>>();
        private Mock<ILogger<DepartmentService>> logger = new Mock<ILogger<DepartmentService>>();
        private DepartmentController controller;
        private Department department = new Department()
        {
            DepartmentId = 10,
            DepartmentCode = "Test",
            Description = "Test",
            DepartmentTypeId = 2,
            DepartmentHeadId = 213,
            CurrentUser = "Anonymous",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = null,
            SystemInfo = null,
            IsActive = true,
            CreatedBy = "Anonymous",
            ModifiedBy = null

        };
        private string controllerName = "Department";
        #endregion

        #region Create TestCases
        [TestMethod]
        public async Task CreateAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(CreateAsync));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.Create(department);
            Department data = (Department)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(data.DepartmentCode, "Test");
        }

        [TestMethod]
        public async Task CreateAsync_BadRequest_CodeAlreadyExists()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(CreateAsync_BadRequest_CodeAlreadyExists));
            ConfigureTest(dbContext, out controller);
            department.DepartmentCode = "Admin";

            //Act
            var response = await controller.Create(department);
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
            department.DepartmentId = 2;
            department.Description = "Admin Department";
            department.DepartmentCode = "Admin";
            department.DepartmentTypeId = 1;
            //Act
            var response = await controller.Update(department);
            Department data = (Department)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(data.Description, "Admin");
        }

        [TestMethod]
        public async Task UpdateAsync_BadRequest_InvalidDepartmentID()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(UpdateAsync_BadRequest_InvalidDepartmentID));
            ConfigureTest(dbContext, out controller);
            department.DepartmentId = 11;

            //Act
            var response = await controller.Update(department);
            var expectedResult = response.GetType();

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "BadRequestObjectResult");
        }

        [TestMethod]
        public async Task UpdateAsync_BadRequest_CodeAlreadyExists()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(UpdateAsync_BadRequest_CodeAlreadyExists));
            ConfigureTest(dbContext, out controller);
            department.DepartmentCode = "Admin";

            //Act
            var response = await controller.Update(department);
            var expectedResult = response.GetType();

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "BadRequestObjectResult");
        }
#endregion

        #region Get Test Cases

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
        private void ConfigureTest(AdminContext dbContext, out DepartmentController controller)
        {
            IDepartmentService m_DepartmentService = new DepartmentService(logger.Object, dbContext, null, null, null);
            controller = new DepartmentController(m_DepartmentService, _logger.Object);
        }
        #endregion

    }
}
