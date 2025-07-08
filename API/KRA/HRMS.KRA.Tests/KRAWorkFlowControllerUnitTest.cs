using HRMS.KRA.API.Controllers;
using HRMS.KRA.Database;
using HRMS.KRA.Service;
using HRMS.KRA.Types;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace HRMS.KRA.Tests
{
    [TestClass]
    public class KRAWorkFlowControllerUnitTest
    {
        #region GlobalVariables
        private Mock<ILogger<KRAWorkFlowController>> _logger = new Mock<ILogger<KRAWorkFlowController>>();
        private Mock<ILogger<KRAWorkFlowService>> logger = new Mock<ILogger<KRAWorkFlowService>>();
        private KRAWorkFlowController controller;
        private string controllerName = "KRAStatus";
        #endregion

        #region GetStatusByFinancialYearIdAsync
        [TestMethod]
        public async Task GetKRAStatusByFinancialYearIdAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(GetKRAStatusByFinancialYearIdAsync));
            ConfigureTest(dbContext, out controller);

            // Act 
            var response = await controller.GetKRAStatusByFinancialYearId(6);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(value, null);
        }
        #endregion

        #region ConfigureTest
        private void ConfigureTest(KRAContext dbContext, out KRAWorkFlowController controller)
        {
            IKRAWorkFlowService m_KRAStatusService = new KRAWorkFlowService(logger.Object, dbContext, null, null, null, null, null, null, null, null, null, null);
            controller = new KRAWorkFlowController(m_KRAStatusService, _logger.Object);
        }
        #endregion
    }
}
