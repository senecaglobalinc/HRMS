using AutoMapper;
using HRMS.Employee.API.Controllers;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Service;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Tests
{

    [TestClass]
    public class AssociateExitControllerUnitTest
    {
        private Mock<ILogger<TransitionPlanController>> logger = new Mock<ILogger<TransitionPlanController>>();        
        private Mock<ILogger<TransitionPlanService>> m_logger = new Mock<ILogger<TransitionPlanService>>();        
        private TransitionPlanController controller;      
        private string controllerName = "TransitionPlanController";


        #region CreateAsync
        [TestMethod]
        public async Task CreateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(controllerName + nameof(CreateAsync));
            ConfigureTest(dbContext, out controller);

            //Input
            TransitionDetail transitionDetail = new TransitionDetail()
            {
                ProjectId = 36,
                EmployeeId = 1369,
                TransitionFrom=1369,
                TransitionTo = 829,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Others = "test",
                KnowledgeTransferred = false,
                UpdateTransitionDetail = new List<UpdateTransitionDetail>
                {
                    new UpdateTransitionDetail
                    {
                        ActivityId=73,
                        StartDate=DateTime.Now,
                        EndDate=DateTime.Now,
                        Remarks="test1",
                        Status="0"
                    },
                      new UpdateTransitionDetail
                    {
                        ActivityId=69,
                        StartDate=DateTime.Now,
                        EndDate=DateTime.Now,
                        Remarks="test2",
                        Status="0"
                    }
                }

            };

            //Act  
            var response = await controller.UpdateTransitionPlan(transitionDetail);
            TransitionPlan data = (TransitionPlan)response;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue(1369 == data.TransitionFrom && 829 == data.TransitionTo);
        }
        #endregion

        #region ConfigureTest
        private void ConfigureTest(EmployeeDBContext dbContext, out TransitionPlanController controller)
        {
            ITransitionPlanService projectService = new TransitionPlanService(dbContext,m_logger.Object,null,null,null,null,null);
            controller = new TransitionPlanController(projectService, logger.Object);
        }
        #endregion
    }
}
