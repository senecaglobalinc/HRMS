using HRMS.Admin.API.Controllers;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Service;
using HRMS.Admin.Types;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Admin.Tests
{
    [TestClass]
    public class StatusControllerUnitTest
    {
        private Mock<ILogger<StatusController>> _logger;
        private Mock<ILogger<StatusService>> logger;

        #region Get Test Cases

        #region GetAllAsync
        [TestMethod]
        public async Task GetAllAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllAsync));
            IStatusService m_StatusService;
            StatusController m_Controller;

            ConfigureTest(dbContext, out m_StatusService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetAll(null);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(value, null);
        }
        #endregion

        #region GetAllAsync_Active
        [TestMethod]
        public async Task GetAllAsync_Active()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllAsync_Active));

            IStatusService m_StatusService;
            StatusController m_Controller;

            ConfigureTest(dbContext, out m_StatusService,
                out m_Controller);

            var response = await m_Controller.GetAll(true);

            var statuses = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<Status>;
            int inActiveRecords = statuses.FindAll(s => s.IsActive == false).Count;

            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("OkObjectResult".Equals(actualResult.Name.ToString()) && inActiveRecords == 0);
        }
        #endregion

        #endregion


        #region GetbyStatus Code Test Cases

        #region GetByCodeAsync
        [TestMethod]
        public async Task GetByCodeAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetByCodeAsync));
            IStatusService m_StatusService;
            StatusController m_Controller;

            ConfigureTest(dbContext, out m_StatusService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetStatusByCode("ApprovedByPM");
            Status data = (Status)((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value;
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(data.StatusDescription, "ApprovedByPM");
        }
        #endregion

        #region TestGetStatusByCodeReturnNotFoundResult
        [TestMethod]
        public async Task TestGetStatusByCodeReturnNotFoundResult()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(TestGetStatusByCodeReturnNotFoundResult));
            IStatusService m_StatusService;
            StatusController m_Controller;

            ConfigureTest(dbContext, out m_StatusService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetStatusByCode("test");
            var expectedResult = response.Result.GetType();

            //Assert  
            Assert.AreEqual(expectedResult.Name.ToString(), "NotFoundResult");
        }
        #endregion

        #endregion

        #region GetbyCategoryAndStatusCode

        #region GetbyCategoryAndStatusCode
        [TestMethod]
        public async Task GetbyCategoryAndStatusCode()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetbyCategoryAndStatusCode));
            IStatusService m_StatusService;
            StatusController m_Controller;

            ConfigureTest(dbContext, out m_StatusService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetByCategoryAndStatusCode("AssociateExit", "ApprovedByPM");
            Status data = (Status)((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value;
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(data.StatusDescription, "ApprovedByPM");
        }
        #endregion
        #endregion

        #region ConfigureTest
        private void ConfigureTest(AdminContext dbContext, out IStatusService m_StatusService,
            out StatusController m_Controller)
        {
            _logger = new Mock<ILogger<StatusController>>();
            logger = new Mock<ILogger<StatusService>>();
            m_StatusService = new StatusService(logger.Object, dbContext);
            m_Controller = new StatusController(m_StatusService, _logger.Object);
        }
        #endregion
    }
}
