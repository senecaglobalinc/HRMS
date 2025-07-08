using HRMS.KRA.API.Controllers;
using HRMS.KRA.Database;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Service;
using HRMS.KRA.Types;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections;
using System.Threading.Tasks;

namespace HRMS.KRA.Tests
{
    [TestClass]
    public class StatusControllerUnitTest
    {
        #region GlobalVariables
        private Mock<ILogger<StatusController>> _logger = new Mock<ILogger<StatusController>>();
        private Mock<ILogger<StatusService>> logger = new Mock<ILogger<StatusService>>();
        private StatusController controller;
        private string controllerName = "Status";
        StatusModel model = new StatusModel()
        {
            StatusId = 1,
            StatusText = "Draft",
            StatusDescription = "While HR is defining the KRAs for the first time",            
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
            model.StatusText = "Draft";
            //Act
            var response = await controller.Create(model);
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
            Assert.AreEqual(collection.Count, 2);
        }  

        #endregion        

        #region ConfigureTest
        private void ConfigureTest(KRAContext dbContext, out StatusController controller)
        {
            IStatusService m_StatuService = new StatusService(logger.Object, dbContext);
            controller = new StatusController(m_StatuService, _logger.Object);
        }
        #endregion
    }
}
