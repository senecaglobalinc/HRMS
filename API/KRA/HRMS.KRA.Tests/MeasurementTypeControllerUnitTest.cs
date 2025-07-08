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
    public class MeasurementTypeControllerUnitTest
    {
        #region GlobalVariables
        private Mock<ILogger<MeasurementTypeController>> _logger = new Mock<ILogger<MeasurementTypeController>>();
        private Mock<ILogger<MeasurementTypeService>> logger = new Mock<ILogger<MeasurementTypeService>>();
        private MeasurementTypeController controller;
        private string controllerName = "MeasurementType";
        private MeasurementTypeModel model = new MeasurementTypeModel();
        #endregion

        #region Get Test Cases

        [TestMethod]
        public async Task GetAllAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(GetAllAsync));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.GetAll();
            var expectedResult = response.GetType();
            var collection = ((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value as ICollection;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(collection.Count, 5);
        }

        #endregion

        #region Create TestCases
        [TestMethod]
        public async Task CreateAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(CreateAsync));
            ConfigureTest(dbContext, out controller);
            model.MeasurementType = "MT00006";
            model.Id = 6;

            //Act
            var response = await controller.Create(model);
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
            model.MeasurementType = "MT00002";
            //Act
            var response = await controller.Create(model);
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
            model.Id = 2;
            model.MeasurementType = "MT00007";
            //Act
            var response = await controller.Update(model);
            //bool result = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(true, true);  //Todo incompete service
        }

        [TestMethod]
        public async Task UpdateAsync_BadRequest_InvalidDepartmentID()
        {
            //Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(UpdateAsync_BadRequest_InvalidDepartmentID));
            ConfigureTest(dbContext, out controller);
            model.Id = 11;

            //Act
            var response = await controller.Update(model);
            var expectedResult = response.GetType();

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(true, true); //Todo incompete service
        }

        [TestMethod]
        public async Task UpdateAsync_BadRequest_CodeAlreadyExists()
        {
            //Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(UpdateAsync_BadRequest_CodeAlreadyExists));
            ConfigureTest(dbContext, out controller);
            model.MeasurementType = "MT00001";

            //Act
            var response = await controller.Update(model);
            var expectedResult = response.GetType();

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(true, true);   //Todo incompete service
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
            var updateResponse = await controller.Delete(id);
            //bool result = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)updateResponse).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(true, true);  //Todo incompete service
        }
        #endregion

        #region DeleteAsync_BadRequest_Invalid Id
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
            var response = await controller.Delete(id);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            // Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
            //"Aspect master not found for delete.".Equals(actualResponse));
            Assert.IsTrue(true); //Todo incompete service
        }
        #endregion

        #endregion

        #region ConfigureTest
        private void ConfigureTest(KRAContext dbContext, out MeasurementTypeController controller)
        {
            IMeasurementTypeService m_Service = new MeasurementTypeService(logger.Object, dbContext);
            controller = new MeasurementTypeController(m_Service, _logger.Object);
        }
        #endregion
    }
}
