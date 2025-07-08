using AutoMapper;
using HRMS.Project.API;
using HRMS.Project.Database;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Service;
using HRMS.Project.Service.External;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace HRMS.Project.Tests
{
    [TestClass]
    public class ProjectControllerUnitTest
    {
        private Mock<ILogger<ProjectController>> _logger;
        private Mock<ILogger<ProjectService>> logger;
        private Mock<ILogger<AssociateAllocationService>> logger_AssociateAllocationService;
        private Mock<ILogger<ClientBillingRolesService>> logger_ClientBillingRolesService;
        private Mock<ILogger<ClientBillingRoleHistoryService>> logger_ClientBillingRoleHistoryService;
        private Mock<ILogger<ProjectManagerService>> logger_ProjectManagerService;
        private Mock<ILogger<EmployeeService>> logger_EmployeeService;
        private Mock<ILogger<OrganizationService>> logger_OrganizationService;
        private Mock<ILogger<ProjectClosureReportService>> logger_ProjectClosureReportService;
        private Mock<ILogger<ProjectClosureActivityService>> logger_ProjectClosureActivityService;
        private Mock<ILogger<ProjectClosureService>> logger_ProjectClosureService;
        IMapper mapper;
        IConfiguration configuration;

        #region Get

        #region GetAll

        [TestMethod]
        public async Task GetAll()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(GetAll));

            ProjectController controller = ConfigureTest(dbContext);

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

            ProjectController controller = ConfigureTest(dbContext);

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

        #region HasActiveClientBillingRoles

        [TestMethod]
        public async Task HasActiveClientBillingRoles()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(HasActiveClientBillingRoles));

            ProjectController controller = ConfigureTest(dbContext);

            // Act 
            var response = await controller.HasActiveClientBillingRoles(1);
            var value = response;
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }

        #endregion

        #region Create

        #region CreateProject
        [TestMethod]
        public async Task CreateProject()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(CreateProject));

            ProjectController controller = ConfigureTest(dbContext);

            var projectData = new ProjectRequest()
            {
                ProjectId = 1,
                ProjectCode = "ULN14003",
                ProjectName = "Uline",
                PlannedStartDate = null,
                PlannedEndDate = null,
                ClientId = 17,
                StatusId = 0,
                ProjectTypeId = 2,
                ProjectStateId = 16,
                ActualStartDate = DateTime.Now,
                ActualEndDate = DateTime.Now.AddYears(5),
                DepartmentId = 1,
                PracticeAreaId = 2,
                DomainId = 0
            };

            ProjectRequest projectRequest = new ProjectRequest();

            projectRequest = (ProjectRequest)projectData;

            //Act  
            var response = await controller.Create(projectRequest);
            var value = response;
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion

        #endregion

        #region Update

        #region UpdateProject
        [TestMethod]
        public async Task UpdateProject()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(UpdateProject));

            ProjectController controller = ConfigureTest(dbContext);

            var projectData = new Entities.Project()
            {
                ProjectId = 1,
                ProjectCode = "ULN14003",
                ProjectName = "Uline",
                PlannedStartDate = null,
                PlannedEndDate = null,
                ClientId = 17,
                StatusId = 0,
                ProjectTypeId = 2,
                ProjectStateId = 16,
                ActualStartDate = DateTime.Now,
                ActualEndDate = DateTime.Now.AddYears(5),
                DepartmentId = 1,
                PracticeAreaId = 2,
                DomainId = 0
            };


            ProjectRequest projectRequest = new ProjectRequest();

            projectRequest = (ProjectRequest)projectData;

            //Act  
            var response = await controller.Update(projectRequest);
            ProjectRequest data = (ProjectRequest)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("18".Equals(data.ClientId));
        }
        #endregion

        #endregion

        #region ConfigureTest
        private ProjectController ConfigureTest(ProjectDBContext dbContext)
        {
            ProjectController controller = null;

            _logger = new Mock<ILogger<ProjectController>>();
            logger = new Mock<ILogger<ProjectService>>();
            logger_AssociateAllocationService = new Mock<ILogger<AssociateAllocationService>>();
            logger_ClientBillingRolesService = new Mock<ILogger<ClientBillingRolesService>>();
            logger_ProjectManagerService = new Mock<ILogger<ProjectManagerService>>();
            logger_ClientBillingRoleHistoryService = new Mock<ILogger<ClientBillingRoleHistoryService>>();
            logger_EmployeeService = new Mock<ILogger<EmployeeService>>();
            logger_OrganizationService = new Mock<ILogger<OrganizationService>>();
            logger_ProjectClosureReportService = new Mock<ILogger<ProjectClosureReportService>>();
            logger_ProjectClosureActivityService = new Mock<ILogger<ProjectClosureActivityService>>();
            logger_ProjectClosureService = new Mock<ILogger<ProjectClosureService>>();
            IEmployeeService m_EmployeeService =
                new EmployeeService(logger_EmployeeService.Object, null, null, null);

            IOrganizationService m_OrganizationService =
                new OrganizationService(logger_OrganizationService.Object, null, null);

            IAssociateAllocationService m_AssociateAllocationService =
                new AssociateAllocationService(logger_AssociateAllocationService.Object, dbContext, null, null, m_EmployeeService, m_OrganizationService, null, null, null);

            IClientBillingRoleHistoryService m_ClientBillingRoleHistoryService =
                new ClientBillingRoleHistoryService(logger_ClientBillingRoleHistoryService.Object, dbContext, null, null, mapper);


            IClientBillingRoleService m_ClientBillingRolesService =
                new ClientBillingRolesService(logger_ClientBillingRolesService.Object, dbContext, null, null,
                m_AssociateAllocationService, m_ClientBillingRoleHistoryService, m_OrganizationService);


            IProjectManagerService m_ProjectManagerService =
                new ProjectManagerService(logger_ProjectManagerService.Object, dbContext, null, null, m_OrganizationService, m_EmployeeService, null);

            IProjectService m_ProjectService = new ProjectService(logger.Object, dbContext, null, null,
                m_ProjectManagerService, m_AssociateAllocationService, m_OrganizationService, m_ClientBillingRolesService, m_EmployeeService, mapper, mapper, null,null);

            IProjectClosureReportService m_ProjectClosureReportService = new ProjectClosureReportService(logger_ProjectClosureReportService.Object, dbContext, m_ProjectManagerService, m_OrganizationService, m_EmployeeService, null , m_ProjectService, mapper, null);

            IProjectClosureActivityService m_ProjectClosureActivityService = new ProjectClosureActivityService(logger_ProjectClosureActivityService.Object, dbContext, m_OrganizationService, m_ProjectService, m_ProjectClosureReportService, m_EmployeeService ,mapper);

            controller = new ProjectController(m_ProjectService, _logger.Object);

            return controller;
        }
        #endregion
    }
}
