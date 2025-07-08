using HRMS.Common.Enums;
using HRMS.Common.Extensions;
using HRMS.Project.API;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Service;
using HRMS.Project.Service.External;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Moq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Project.Tests
{
    [TestClass]
    public class ClientBillingRoleControllerUnitTest
    {
        private Mock<ILogger<ClientBillingRolesController>> _logger;
        private Mock<ILogger<ClientBillingRolesService>> logger;
        private Mock<ILogger<AssociateAllocationService>> logger_AssociateAllocationService;
        private Mock<ILogger<ClientBillingRoleHistoryService>> logger_ClientBillingRoleHistoryService;
        private Mock<ILogger<OrganizationService>> logger_OrganizationService;

        #region Get

        #region GetAllByProjectIdAsync
        [TestMethod]
        public async Task GetAllByProjectIdAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(GetAllByProjectIdAsync));

            ClientBillingRolesController controller = ConfigureTest(dbContext);

            // Act 
            var response = await controller.GetAllByProjectId(1);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion

        #endregion

        #region Delete

        //#region DeleteAsync
        //[TestMethod]
        //public async Task DeleteAsync()
        //{
        //    // Arrange
        //    var dbContext = DbContextMocker.GetProjectDbContext(nameof(DeleteAsync));

        //    ClientBillingRolesController controller = ConfigureTest(dbContext);

        //    // Act 
        //    var response = await controller.Delete(1);
        //    bool updated = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

        //    //Dispose DBContext
        //    dbContext.Dispose();

        //    // Assert
        //    Assert.AreEqual(updated, true);
        //}
        //#endregion

        //#region DeleteAsync_BadRequest
        //[TestMethod]
        //public async Task DeleteAsync_BadRequest()
        //{
        //    // Arrange
        //    var dbContext = DbContextMocker.GetProjectDbContext(nameof(DeleteAsync_BadRequest));

        //    ClientBillingRolesController controller = ConfigureTest(dbContext);

        //    // Act 
        //    var response = await controller.Delete(15);
        //    string errorMessage = ((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value.ToString();

        //    //Dispose DBContext
        //    dbContext.Dispose();

        //    // Assert
        //    Assert.AreEqual(errorMessage, "SOW not found for delete.");
        //}
        //#endregion

        #endregion

        #region Create

        #region CreateAsync
        [TestMethod]
        public async Task CreateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(CreateAsync));

            ClientBillingRolesController controller = ConfigureTest(dbContext);

            var cbr = new ClientBillingRoles()
            {
                ClientBillingRoleId = 3,
                ClientBillingRoleCode = "Sr. Dev",
                ClientBillingRoleName = "Senior Developer",
                NoOfPositions = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(5),
                ClientBillingPercentage = 4
            };

            //Act  
            var response = await controller.Create(cbr);
            var okResult = response as OkObjectResult;
            var actualResult = okResult.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion


        #endregion

        #region Update

        #region UpdateAsync
        [TestMethod]
        public async Task UpdateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(UpdateAsync));

            ClientBillingRolesController controller = ConfigureTest(dbContext);

            var cbr = new ClientBillingRoles()
            {
                ClientBillingRoleId = 1,
                ClientBillingRoleCode = "Sr. Dev",
                ClientBillingRoleName = "Senior Developer",
                NoOfPositions = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(5),
                ClientBillingPercentage = 4
            };

            //Act  
            var response = await controller.Update(cbr);
            var okResult = response as OkObjectResult;
            var actualResult = okResult.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion


        #endregion

        #region ConfigureTest
        private ClientBillingRolesController ConfigureTest(ProjectDBContext dbContext)
        {
            ClientBillingRolesController controller = null;

            _logger = new Mock<ILogger<ClientBillingRolesController>>();
            logger = new Mock<ILogger<ClientBillingRolesService>>();
            logger_AssociateAllocationService = new Mock<ILogger<AssociateAllocationService>>();
            logger_ClientBillingRoleHistoryService = new Mock<ILogger<ClientBillingRoleHistoryService>>();

            _logger = new Mock<ILogger<ClientBillingRolesController>>();
            logger = new Mock<ILogger<ClientBillingRolesService>>();
            logger_OrganizationService = new Mock<ILogger<OrganizationService>>();
            IAssociateAllocationService m_AssociateAllocationService =
                new AssociateAllocationService(logger_AssociateAllocationService.Object, dbContext, null, null,null, null, null, null, null);

            IClientBillingRoleHistoryService m_ClientBillingRoleHistoryService =
                new ClientBillingRoleHistoryService(logger_ClientBillingRoleHistoryService.Object, dbContext, null, null,null);

            IOrganizationService m_OrgService =
                new OrganizationService(logger_OrganizationService.Object,null,null);


            IClientBillingRoleService m_ClientBillingRoleService = new ClientBillingRolesService(logger.Object, dbContext, null, null,
                m_AssociateAllocationService, m_ClientBillingRoleHistoryService, m_OrgService);

            controller = new ClientBillingRolesController(m_ClientBillingRoleService, _logger.Object);

            return controller;
        }
        #endregion
    }
}
