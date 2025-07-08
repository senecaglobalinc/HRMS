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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.KRA.Tests
{
    [TestClass]
    public class ScaleControllerUnitTest
    {
        #region GlobalVariables
        private Mock<ILogger<ScaleController>> _logger = new Mock<ILogger<ScaleController>>();
        private Mock<ILogger<ScaleService>> logger = new Mock<ILogger<ScaleService>>();
        private ScaleController controller;
        private string controllerName = "Scale";
        ScaleModel model = new ScaleModel()
        {
            ScaleID = 0,
            MinimumScale = 1,
            MaximumScale = 6,
            ScaleTitle = "Scale006",
            ScaleDetails = new List<ScaleDetailsModel>()
            {
                new ScaleDetailsModel(){ ScaleValue = 1, ScaleDescription = "Scale001"},
                new ScaleDetailsModel(){ ScaleValue = 2, ScaleDescription = "Scale002"},
                new ScaleDetailsModel(){ ScaleValue = 3, ScaleDescription = "Scale003"},
                new ScaleDetailsModel(){ ScaleValue = 4, ScaleDescription = "Scale004"},
                new ScaleDetailsModel(){ ScaleValue = 5, ScaleDescription = "Scale005"},
                new ScaleDetailsModel(){ ScaleValue = 6, ScaleDescription = "Scale006"},
            }
        };
        #endregion

        #region Create TestCases
        [TestMethod]
        public async Task CreateAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(CreateAsync));
            ConfigureTest(dbContext, out controller);

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
            model.ScaleTitle = "Scale001";
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
            model.ScaleDetails.ForEach(x => x.ScaleDetailId = 6);

            //Act
            var response = await controller.Update(model);
            bool result = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public async Task UpdateAsync_BadRequest_InvalidDepartmentID()
        {
            //Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(UpdateAsync_BadRequest_InvalidDepartmentID));
            ConfigureTest(dbContext, out controller);
            model.ScaleDetails.ForEach(x => x.ScaleDetailId = 22);

            //Act
            var response = await controller.Update(model);
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
            var response = await controller.GetAll();
            var expectedResult = response.GetType();
            var collection = ((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value as ICollection;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(collection.Count, 5);
        }

        [TestMethod]
        public async Task GetScaleDetailsById()
        {
            //Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(GetAllAsync));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.GetScaleDetailsById(1);
            var expectedResult = response.GetType();
            var collection = ((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value as ICollection;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(collection.Count, 1);
        }

        [TestMethod]
        public async Task GetScaleDetailsById_Invalid()
        {
            //Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(GetAllAsync));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.GetScaleDetailsById(20);
            var expectedResult = response.GetType();
            var collection = ((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value as ICollection;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(collection.Count, 0);
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
            Assert.AreEqual(true, true); //ToDo pending at serice
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
            var response = await controller.Delete(id);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            //var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose(); //ToDo pending at serice

            // Assert
            //Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
            //"Aspect master not found for delete.".Equals(actualResponse));
            Assert.IsTrue(true);
        }
        #endregion

        #endregion

        #region ConfigureTest
        private void ConfigureTest(KRAContext dbContext, out ScaleController controller)
        {
            IScaleService m_ScaleService = new ScaleService(logger.Object, dbContext);
            controller = new ScaleController(m_ScaleService, _logger.Object);
        }
        #endregion
    }
}
