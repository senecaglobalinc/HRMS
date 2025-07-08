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
    public class ProficiencyLevelControllerUnitTest
    {
        #region Global Variables
        private Mock<ILogger<ProficiencyLevelController>> _logger = new Mock<ILogger<ProficiencyLevelController>>();
        private Mock<ILogger<ProficiencyLevelService>> logger = new Mock<ILogger<ProficiencyLevelService>>();
        private ProficiencyLevelController controller;
        private ProficiencyLevel proficiencyLevel = new ProficiencyLevel()
        {
            ProficiencyLevelId = 10,
            ProficiencyLevelCode = "Intermediate",
            ProficiencyLevelDescription = "Expert",
            CurrentUser = "Anonymous",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = null,
            SystemInfo = null,
            IsActive = true,
            CreatedBy = "Anonymous",
            ModifiedBy = null

        };
        private string controllerName = "ProficiencyLevel";
        #endregion

        #region Create TestCases
        [TestMethod]
        public async Task CreateAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(CreateAsync));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.Create(proficiencyLevel);
            ProficiencyLevel data = (ProficiencyLevel)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(data.ProficiencyLevelCode, "Intermediate");
        }

        [TestMethod]
        public async Task CreateAsync_BadRequest_CodeAlreadyExists()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(CreateAsync_BadRequest_CodeAlreadyExists));
            ConfigureTest(dbContext, out controller);
            proficiencyLevel.ProficiencyLevelCode = "Advance";

            //Act
            var response = await controller.Create(proficiencyLevel);
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
            proficiencyLevel.ProficiencyLevelId = 4;
            proficiencyLevel.ProficiencyLevelCode = "Preliminary";

            //Act
            var response = await controller.Update(proficiencyLevel);
            ProficiencyLevel data = (ProficiencyLevel)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(data.ProficiencyLevelCode, "Preliminary");
        }

        [TestMethod]
        public async Task UpdateAsync_BadRequest_InvalidProficiencyLevelID()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(UpdateAsync_BadRequest_InvalidProficiencyLevelID));
            ConfigureTest(dbContext, out controller);
            proficiencyLevel.ProficiencyLevelId = 11;

            //Act
            var response = await controller.Update(proficiencyLevel);
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
            proficiencyLevel.ProficiencyLevelCode = "Advance";

            //Act
            var response = await controller.Update(proficiencyLevel);
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
        private void ConfigureTest(AdminContext dbContext, out ProficiencyLevelController controller)
        {
            IProficiencyLevelService m_ProficiencyLevelService = new ProficiencyLevelService(logger.Object, dbContext);
            controller = new ProficiencyLevelController(m_ProficiencyLevelService, _logger.Object);
        }
        #endregion
    }
}
