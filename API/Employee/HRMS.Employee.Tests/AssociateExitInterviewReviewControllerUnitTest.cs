using HRMS.Employee.API.Controllers;
using HRMS.Employee.Database;
using HRMS.Employee.Infrastructure.Models.Domain;
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
    public class AssociateExitInterviewReviewControllerUnitTest
    {
        private Mock<ILogger<AssociateExitInterviewReviewController>> _logger = new Mock<ILogger<AssociateExitInterviewReviewController>>();
        private Mock<ILogger<AssociateExitInterviewReviewService>> logger = new Mock<ILogger<AssociateExitInterviewReviewService>>();
        private AssociateExitInterviewReviewController m_controller;
        private string controllerName = "AssociateExitInterviewReview";

        #region ConfigureTest
        private void ConfigureTest(EmployeeDBContext dbContext, out AssociateExitInterviewReviewController controller)
        {
            IAssociateExitInterviewReviewService associateExitInterviewReviewService = new AssociateExitInterviewReviewService(dbContext, logger.Object);
            controller = new AssociateExitInterviewReviewController(associateExitInterviewReviewService, _logger.Object);
        }
        #endregion

        #region GetAllAsync
        [TestMethod]
        public async Task GetAllAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(controllerName + nameof(GetAllAsync));
            ConfigureTest(dbContext, out m_controller);

            //Input
            ExitInterviewReviewGetAllRequest exitInterviewReviewGetAllRequest = new ExitInterviewReviewGetAllRequest();
            exitInterviewReviewGetAllRequest.FromDate = DateTime.Now.Date.AddDays(-2).Date;
            exitInterviewReviewGetAllRequest.ToDate = DateTime.Now;

            // Act 
            var response = await m_controller.GetAll(exitInterviewReviewGetAllRequest);
            var exitInterviewReviews = ((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value as List<ExitInterviewReviewGetAllResponse>;

            //Dispose DBContext
            dbContext.Dispose();

            //Assert
            Assert.IsTrue(exitInterviewReviews.Count == 2);
        }
        #endregion

        #region CreateAsync
        [TestMethod]
        public async Task CreateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(controllerName + nameof(CreateAsync));
            ConfigureTest(dbContext, out m_controller);

            //Input
            ExitInterviewReviewCreateRequest exitInterviewReviewCreate = new ExitInterviewReviewCreateRequest();
            exitInterviewReviewCreate.AssociateExitInterviewId = 1;
            exitInterviewReviewCreate.FinalRemarks = "Updated Remarks of Associate Exit Interview";
            exitInterviewReviewCreate.ShowInitialRemarks = true;

            //Act  
            var response = await m_controller.Create(exitInterviewReviewCreate);
            int result = (int)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Dispose DBContext
            dbContext.Dispose();
          
            //Assert
            Assert.IsTrue(result == 1);
        }
        #endregion        
    }
}
