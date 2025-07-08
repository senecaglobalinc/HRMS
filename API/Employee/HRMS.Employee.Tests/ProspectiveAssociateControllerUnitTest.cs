using HRMS.Employee.API.Controllers;
using HRMS.Employee.Database;
using HRMS.Employee.Service;
using HRMS.Employee.Types;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Tests
{
    [TestClass]
    public class ProspectiveAssociateControllerUnitTest
    {
        private Mock<ILogger<ProspectiveAssociateController>> _logger;
        private Mock<ILogger<ProspectiveAssociateService>> logger;


        #region GetAllAsync
        [TestMethod]
        public async Task GetAllAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(nameof(GetAllAsync));
            IProspectiveAssociateService m_ProspectiveAssociateService;
            ProspectiveAssociateController m_Controller;

            ConfigureTest(dbContext, out m_ProspectiveAssociateService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetProspectiveAssociates();
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
        private void ConfigureTest(EmployeeDBContext dbContext, out IProspectiveAssociateService m_ProspectiveAssociateService,
            out ProspectiveAssociateController m_Controller)
        {
            _logger = new Mock<ILogger<ProspectiveAssociateController>>();
            logger = new Mock<ILogger<ProspectiveAssociateService>>();
            m_ProspectiveAssociateService = new ProspectiveAssociateService(dbContext, logger.Object, null, null,null,null,null,null);            m_Controller = new ProspectiveAssociateController(m_ProspectiveAssociateService, _logger.Object);
        }
        #endregion
    }
}
