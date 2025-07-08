using AutoMapper;
using HRMS.Project.API;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Service;
using HRMS.Project.Service.External;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace HRMS.Project.Tests
{
    [TestClass]
    public class ProjectManagerControllerUnitTest
    {
        private Mock<ILogger<ProjectManagerController>> _logger;
        private Mock<ILogger<ProjectManagerService>> logger;
        private Mock<ILogger<EmployeeService>> logger_EmployeeService;
        private Mock<ILogger<OrganizationService>> logger_OrganizationService;
        IMapper mapper;

        #region Get

        #region GetAll

        [TestMethod]
        public async Task GetAll()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(GetAll));

            ProjectManagerController controller = ConfigureTest(dbContext);

            // Act 
            var response = await controller.GetAll(true);
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

            ProjectManagerController controller = ConfigureTest(dbContext);

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

        #region GetActiveProjectManagers
        [TestMethod]
        public async Task GetActiveProjectManagers()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(GetActiveProjectManagers));

            ProjectManagerController controller = ConfigureTest(dbContext);

            // Act 
            var response = await controller.GetActiveProjectManagers(true);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion

        #endregion

        #region Create

        #region CreateProjectManager
        [TestMethod]
        public async Task CreateProjectManager()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(CreateProjectManager));

            ProjectManagerController controller = ConfigureTest(dbContext);

            var projectManagerData = new ProjectManager()
            {
                Id = 3,
                ProjectId = 40,
                ReportingManagerId = 244,
                ProgramManagerId = 244,
                LeadId = 7,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            };


            //Act  
            var response = await controller.Create(projectManagerData);
            var value = response;
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion

        #endregion

        #region SaveManagersToProject
        [TestMethod]
        public async Task SaveManagersToProject()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(SaveManagersToProject));

            ProjectManagerController controller = ConfigureTest(dbContext);

            var projectManagerData = new ProjectManagersData()
            {
                ProjectId = 50,
                ReportingManagerId = 522,
                ProgramManagerId = 523,
                LeadId = 524
            };


            //Act  
            var response = await controller.SaveManagersToProject(projectManagerData);
            var value = response;
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion

        #region ConfigureTest
        private ProjectManagerController ConfigureTest(ProjectDBContext dbContext)
        {
            ProjectManagerController controller = null;

            _logger = new Mock<ILogger<ProjectManagerController>>();
            logger = new Mock<ILogger<ProjectManagerService>>();
            logger_EmployeeService = new Mock<ILogger<EmployeeService>>();
            logger_OrganizationService = new Mock<ILogger<OrganizationService>>();


            IEmployeeService m_EmployeeService =
                new EmployeeService(logger_EmployeeService.Object, null, null, null);

            IOrganizationService m_OrganizationService =
                new OrganizationService(logger_OrganizationService.Object, null, null);


            IProjectManagerService m_ProjectManagerService = new ProjectManagerService(logger.Object, dbContext, null, null,
                 m_OrganizationService, m_EmployeeService, null);



            controller = new ProjectManagerController(m_ProjectManagerService, _logger.Object);

            return controller;
        }
        #endregion
    }
}
