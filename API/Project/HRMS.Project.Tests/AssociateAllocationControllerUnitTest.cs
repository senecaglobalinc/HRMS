using HRMS.Project.API;
using HRMS.Project.API.Controllers;
using HRMS.Project.Database;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Service;
using HRMS.Project.Service.External;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Project.Tests
{
    [TestClass]
    public class AssociateAllocationControllerUnitTest
    {
        private Mock<ILogger<AssociateAllocationController>> _logger;
        private Mock<ILogger<AssociateAllocationService>> logger;
        private AssociateAllocationController controller;
        private string controllerName = "AssociateAllocation";
        private Mock<ILogger<EmployeeService>> logger_EmployeeService;
        private Mock<ILogger<OrganizationService>> logger_OrganizationService;
        private AssociateAllocationDetails associateAllocationDetails = new AssociateAllocationDetails()
        {
            TalentRequisitionId = 1,
            ProjectId = 71,
            EmployeeId = 159,
            RoleMasterId = 2,
            AllocationPercentage = 1,
            InternalBillingPercentage = 0,
            isCritical = true,
            EffectiveDate = DateTime.UtcNow,
            AllocationDate = DateTime.UtcNow,
            ReportingManagerId = 213,
            IsPrimary = true,
            IsBillable = true,
            NotifyAll = true,
            InternalBillingRoleId = 11,
            ReleaseDate = null,
            ClientBillingPercentage = 25,
            ProgramManagerId = 213,
            LeadId = 30
        };

        #region Get

        #region GetAll
        [TestMethod]
        public async Task GetAll()
        {
            //Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(controllerName + nameof(GetAll));
            ConfigureTest(dbContext);
            int employeeId = 159;
            //Act
            var response = await controller.GetEmpAllocationHistory(employeeId);
            var expectedResult = response.GetType();
            var collection = ((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value as ServiceListResponse<AssociateAllocationDetails>;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(collection.Items.Count, 2);
        }
        #endregion

        #region GetAll_BadRequest
        [TestMethod]
        public async Task GetAll_BadRequest()
        {
            //Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(controllerName + nameof(GetAll_BadRequest));
            ConfigureTest(dbContext);
            int employeeId = 601255;
            //Act
            var response = await controller.GetEmpAllocationHistory(employeeId);
            var expectedResult = response.GetType();
            var collection = ((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value as ServiceListResponse<AssociateAllocationDetails>;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "BadRequestObjectResult");
        }
        #endregion

        #endregion

        #region Create

        #region CreateAsync
        [TestMethod]
        public async Task CreateAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(controllerName + nameof(CreateAsync));
            ConfigureTest(dbContext);
            //Act  
            var response = await controller.Create(associateAllocationDetails);
            var result = (ServiceResponse<int>)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(result.IsSuccessful, true);
        }
        #endregion

        #endregion

        #region ConfigureTest
        private void ConfigureTest(ProjectDBContext dbContext)
        {
            _logger = new Mock<ILogger<AssociateAllocationController>>();
            logger = new Mock<ILogger<AssociateAllocationService>>();
            logger_EmployeeService = new Mock<ILogger<EmployeeService>>();
            logger_OrganizationService = new Mock<ILogger<OrganizationService>>();


            IEmployeeService m_EmployeeService =
                new EmployeeService(logger_EmployeeService.Object, null, null, null);

            IOrganizationService m_OrganizationService =
                new OrganizationService(logger_OrganizationService.Object, null, null);
            var m_IService = new AssociateAllocationService(logger.Object, dbContext, null, null, m_EmployeeService, m_OrganizationService, null, null, null);

            controller = new AssociateAllocationController(m_IService, null, _logger.Object);
        }
        #endregion
    }
}
