using HRMS.KRA.API.Controllers;
using HRMS.KRA.Database;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Service;
using HRMS.KRA.Types;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace HRMS.KRA.Tests
{
    [TestClass]
    public class AspectControllerUnitTest
    {
        #region GlobalVariables
        private Mock<ILogger<AspectController>> _logger = new Mock<ILogger<AspectController>>();
        private Mock<ILogger<AspectService>> logger = new Mock<ILogger<AspectService>>();
        private AspectController controller;
        private string controllerName = "Aspect";
        private AspectModel model = new AspectModel();
        #endregion

        #region Create TestCases
        [TestMethod]
        public async Task CreateAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(CreateAsync));
            ConfigureTest(dbContext, out controller);
            model.AspectName = "CM00006";
            //Act
            var response = await controller.CreateAsync(model);
            bool result = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public async Task CreateAsync_BadRequest_CodeAlreadyExists()
        {
            //Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(CreateAsync_BadRequest_CodeAlreadyExists));
            ConfigureTest(dbContext, out controller);
            model.AspectName = "CM00002";
            //Act
            var response = await controller.CreateAsync(model);
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
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(UpdateAsync));
            ConfigureTest(dbContext, out controller);
            model.AspectId = 2;
            model.AspectName = "CM00007";
            //Act
            var response = await controller.UpdateAsync(model);
            bool result = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(result, true);
        }

        //[TestMethod]
        //public async Task UpdateAsync_BadRequest_InvalidDepartmentID()
        //{
        //    //Arrange
        //    var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(UpdateAsync_BadRequest_InvalidDepartmentID));
        //    ConfigureTest(dbContext, out controller);
        //    model.AspectId = 11;

        //    //Act
        //    var response = await controller.Update(model);
        //    var expectedResult = response.GetType();

        //    dbContext.Dispose();

        //    //Assert
        //    Assert.AreEqual(expectedResult.Name.ToString(), "BadRequestObjectResult");
        //}

        [TestMethod]
        public async Task UpdateAsync_BadRequest_CodeAlreadyExists()
        {
            //Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(UpdateAsync_BadRequest_CodeAlreadyExists));
            ConfigureTest(dbContext, out controller);
            model.AspectName = "CM00001";

            //Act
            var response = await controller.UpdateAsync(model);
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
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(GetAllAsync));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.GetAllAsync();
            var expectedResult = response.GetType();
            var collection = ((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value as ICollection;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(collection.Count, 5);
        }

        #endregion

        #region Delete Test Cases

        #region DeleteAsync
        [TestMethod]
        public async Task DeleteAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(nameof(DeleteAsync));

            ConfigureTest(dbContext, out controller);

            //Input
            int id = 2;

            //Act  
            var updateResponse = await controller.DeleteAsync(id);
            bool result = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)updateResponse).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(result, true);
        }
        #endregion

        #region DeleteAsync_BadRequest_InvalidCategoryID
        [TestMethod]
        public async Task DeleteAsync_BadRequest_InvalidID()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetKRAClientDbContext(nameof(DeleteAsync_BadRequest_InvalidID));

            ConfigureTest(dbContext, out controller);

            //Input
            int id = 15;

            //Act  
            var response = await controller.DeleteAsync(id);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                            "Aspect record not found for delete.".Equals(actualResponse));
        }
        #endregion

        #endregion

        #region ConfigureTest
        private void ConfigureTest(KRAContext dbContext, out AspectController controller)
        {
            IAspectService m_AspectService = new AspectService(logger.Object, dbContext);
            controller = new AspectController(m_AspectService, _logger.Object);
        }
        #endregion
    }
}
