using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HRMS.KRA.API.Controllers;
using HRMS.KRA.Database;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Service;
using HRMS.KRA.Types;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HRMS.KRA.Tests
{
    [TestClass]
    public class DefinitionControllerUnitTest
    {
        #region GlobalVariables
        private Mock<ILogger<DefinitionController>> _logger = new Mock<ILogger<DefinitionController>>();
        private Mock<ILogger<DefinitionService>> logger = new Mock<ILogger<DefinitionService>>();
        private DefinitionController controller;
        private string controllerName = "Definition";
        private DefinitionModel model = new DefinitionModel()
        { 
            DefinitionId = Guid.NewGuid(),
            //ApplicableRoleTypeId = 1,
            AspectId = 2,
            //IsHODApproved = false,
            //IsCEOApproved = false,
            //IsDeleted = false,
            //SourceDefinitionId = 0,
            //DefinitionDetailsId = 0,
            Metric = "Metric String",
            OperatorId = 1,
            MeasurementTypeId = 1,
            ScaleId = 1,
            TargetValue = "3",
            TargetPeriodId = 3
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
            
            model.AspectId = 1;

            //Act
            var response = await controller.Create(model);
            var expectedResult = response.GetType();

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "BadRequestObjectResult");
        }
        #endregion

        #region Delete TestCases
        [TestMethod]
        public async Task DeleteAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(DeleteAsync));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.Delete(1);
            bool result = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public async Task DeleteAsync_InvalidID()
        {
            //Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(DeleteAsync));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.Delete(2);
            bool result = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(result, false);
        }
        #endregion 

        #region ConfigureTest
        //private void ConfigureTest(KRAContext dbContext, out DefinitionController controller)
        //{
        //    //IDefinitionService m_DefinitionService = new DefinitionService(logger.Object, dbContext, null, null, null, null, null, null, null, null);
        //    //controller = new DefinitionController(m_DefinitionService, _logger.Object);
        //}
        #endregion
    }
}
