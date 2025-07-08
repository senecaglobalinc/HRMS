
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
using System.Threading.Tasks;

namespace HRMS.Admin.Tests
{
    [TestClass]
    public class SpecialityControllerUnitTest
    {
        #region Global Variables 
        private Mock<ILogger<SpecialityController>> _logger = new Mock<ILogger<SpecialityController>>();
        private Mock<ILogger<SpecialityService>> logger = new Mock<ILogger<SpecialityService>>();
        private SpecialityController controller;
        private string controllerName = "speciality";
        SGRoleSuffix speciality = new SGRoleSuffix()
        {
            SuffixID = 10,
            SuffixName = "UX",
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
            var response = await controller.Create(speciality);
            SGRoleSuffix data = (SGRoleSuffix)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(data.SuffixName, "UX");
        }

        #endregion

        #region Update TestCases

        [TestMethod]
        public async Task UpdateAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(UpdateAsync));
            ConfigureTest(dbContext, out controller);
            speciality.SuffixName = "UX";
            speciality.SuffixID = 1;
            //Act
            var response = await controller.Update(speciality);
            SGRoleSuffix updatedData = (SGRoleSuffix)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(updatedData.SuffixName, "UX");
        }

        [TestMethod]
        public async Task UpdateAsync_BadRequest_InvalidSuffixID()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(UpdateAsync_BadRequest_InvalidSuffixID));
            ConfigureTest(dbContext, out controller);
            speciality.SuffixName = "HR";
            speciality.SuffixID = 100;
            //Act
            var response = await controller.Update(speciality);
            var expectedResult = response.GetType();

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "BadRequestObjectResult");
        }

        #endregion

        #region ConfigureTest
        private void ConfigureTest(AdminContext dbContext, out SpecialityController controller)
        {
            ISpecialityService m_SpecialityService = new SpecialityService(dbContext, logger.Object);
            controller = new SpecialityController(m_SpecialityService, _logger.Object);
        }
        #endregion
    }
}
