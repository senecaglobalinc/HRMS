using HRMS.Project.API;
using HRMS.Project.API.Controllers;
using HRMS.Project.Database;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Service;
using HRMS.Project.Service.External;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace HRMS.Project.Tests
{
    [TestClass]
    public class AllocationPercentageControllerUnitTest
    {
        private Mock<ILogger<AllocationPercentageController>> _logger;
        private Mock<ILogger<AllocationPercentageService>> logger;
        

        #region Get

        #region GetAll

        [TestMethod]
        public async Task GetAll()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(GetAll));

            AllocationPercentageController controller = ConfigureTest(dbContext);

            // Act 
            var response = await controller.GetAll();
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion

        #region GetById
        [TestMethod]
        public async Task GetById()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(GetById));

            AllocationPercentageController controller = ConfigureTest(dbContext);

            // Act 
            var response = await controller.GetById(1);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion

        #endregion

        #region ConfigureTest
        private AllocationPercentageController ConfigureTest(ProjectDBContext dbContext)
        {
            AllocationPercentageController controller = null;

            _logger = new Mock<ILogger<AllocationPercentageController>>();
            logger = new Mock<ILogger<AllocationPercentageService>>();


            IAllocationPercentageService m_AllocationPercentageService = new AllocationPercentageService(logger.Object, dbContext, null, null);



            controller = new AllocationPercentageController(m_AllocationPercentageService, _logger.Object);

            return controller;
        }
        #endregion
    }
}
