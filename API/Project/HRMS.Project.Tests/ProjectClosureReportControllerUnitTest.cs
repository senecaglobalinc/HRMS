using System;
using System.Collections.Generic;
using System.Text;
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
using System.Threading.Tasks;
using HRMS.Project.API.Controllers;
using HRMS.Project.Infrastructure.Models.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.Web.CodeGeneration;
using HRMS.Project.Infrastructure;
using Microsoft.Extensions.Options;

namespace HRMS.Project.Tests
{
    [TestClass]
    public class ProjectClosureReportControllerUnitTest
    {
        #region MockLoggers
        private Mock<ILogger<ProjectClosureReportController>> _logger = new Mock<ILogger<ProjectClosureReportController>>();
        private Mock<ILogger<ProjectService>> logger_ProjectService = new Mock<ILogger<ProjectService>>();
        private Mock<ILogger<AssociateAllocationService>> logger_AssociateAllocationService = new Mock<ILogger<AssociateAllocationService>>();
        private Mock<ILogger<ClientBillingRolesService>> logger_ClientBillingRolesService = new Mock<ILogger<ClientBillingRolesService>>();
        private Mock<ILogger<ProjectManagerService>> logger_ProjectManagerService = new Mock<ILogger<ProjectManagerService>>();
        private Mock<ILogger<EmployeeService>> logger_EmployeeService = new Mock<ILogger<EmployeeService>>();
        private Mock<ILogger<OrganizationService>> logger_OrganizationService = new Mock<ILogger<OrganizationService>>();
        private Mock<ILogger<ProjectClosureReportService>> logger_ProjectClosureReportService = new Mock<ILogger<ProjectClosureReportService>>();
        private Mock<ILogger<ProjectClosureActivityService>> logger_ProjectClosureActivityService = new Mock<ILogger<ProjectClosureActivityService>>();
        private Mock<ILogger<ProjectClosureService>> logger = new Mock<ILogger<ProjectClosureService>>();
        private Mock<ILogger<ClientBillingRoleHistoryService>> logger_ClientBillingRoleHistoryService = new Mock<ILogger<ClientBillingRoleHistoryService>>();
        Mock<IMapper> mapper = new Mock<IMapper>();
        #endregion

        #region MockServices
        Mock<IOrganizationService> organizationService = new Mock<IOrganizationService>();
        Mock<IProjectService> projectService = new Mock<IProjectService>();
        Mock<IProjectClosureReportService> projectClosureReport = new Mock<IProjectClosureReportService>();
        Mock<IProjectClosureActivityService> projectClosureActivity = new Mock<IProjectClosureActivityService>();
        Mock<IProjectManagerService> projectManagerService = new Mock<IProjectManagerService>();
        Mock<IAssociateAllocationService> associateAllocationService = new Mock<IAssociateAllocationService>();
        Mock<IEmployeeService> employeeService = new Mock<IEmployeeService>();
        Mock<IConfiguration> configuration = new Mock<IConfiguration>();
        Mock<IFormFile> filemock = new Mock<IFormFile>();

        #endregion      

        #region ProjectClosureReportCreate
        [TestMethod]
        public async Task ProjectClosureReportCreate()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext("HRMSProjectDB_Dev_SETP");
           ProjectClosureReportController controller;

            int projectId = 2;
            //Act  
            
            controller = ConfigureTest(dbContext);
            var response = await controller.Create(projectId);
            Int32 data = (Int32)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;
            var mockResult = 1.GetType();
            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(data.GetType(), mockResult);
        }
        #endregion

        #region ProjectClosureReportUpdate
        [TestMethod]
        public async Task ProjectClosureReportUpdate()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext("HRMSProjectDB_Dev_SETP");
            ProjectClosureReportController controller;

            var projectCloseReportData = new ProjectClosureReportRequest()
            {
              ProjectId=5,
              ProjectClosureId=2,
                ValueDelivered="good",
                ManagementChallenges="fine",
                TechnologyChallenges="teck",
                type="update",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null,
               
            };
            //Act  

            controller = ConfigureTest(dbContext);
            var response = await controller.Update(projectCloseReportData);
            Int32 data = (Int32)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;
            var mockResult = 1.GetType();
            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(data.GetType(), mockResult);
        }
        #endregion

        #region GetProjectClosureReport
        [TestMethod]
        public async Task GetProjectClosureReport()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext("HRMSProjectDB_Dev_SETP");
            ProjectClosureReportController controller;

            int projectId = 5;
            //Act  

            controller = ConfigureTest(dbContext);
            var response = await controller.GetClosureReportByProjectId(projectId);
            var value = response.Value;
            var actualResult = response.Result.GetType();
            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion

        #region save
        [TestMethod]
        public async Task save()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext("HRMSProjectDB_Dev_SETP");
            ProjectClosureReportController controller;

            var projectCloseReportData = new UploadFiles()
            {
                ProjectId = 5,
                FileType = "ClientFeedback",
                ClientFeedbackFile = "123",
                UploadedFiles = filemock.Object,

            };
            //Act  

            controller = ConfigureTest(dbContext);
            var response = await controller.Save(projectCloseReportData);
            var value = response.Value;
            var actualResult = response.Result.GetType();
            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion

        #region delete
        [TestMethod]
        public async Task delete()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext("HRMSProjectDB_Dev_SETP");
            ProjectClosureReportController controller;


            int ProjectId = 5;
            string FileType = "ClientFeedback";
               

           
            //Act  

            controller = ConfigureTest(dbContext);
            var response = await controller.Delete(FileType, ProjectId);
            var value = response.Value;
            var actualResult = response.Result.GetType();
            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion

        #region download
        [TestMethod]
        public async Task download()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext("HRMSProjectDB_Dev_SETP");
            ProjectClosureReportController controller;


            int ProjectId = 5;
            string FileType = "ClientFeedback";



            //Act  

            controller = ConfigureTest(dbContext);
            var response = await controller.Download(FileType, ProjectId);
            var value = response;
            var actualResult = response.GetType();
            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "FileStreamResult");
        }
        #endregion

        #region ConfigureTest
        private ProjectClosureReportController ConfigureTest(ProjectDBContext dbContext)
        {
            ProjectClosureReportController controller = null;
            var allocation = new ServiceListResponse<AssociateAllocation>();
            var mockStatus = new ServiceResponse<Status>();
            var notification = new ServiceResponse<NotificationConfiguration>();
            var checklist = new ServiceResponse<int>();
            var emp = new ServiceResponse<Employee>();
            var user = new ServiceResponse<User>();
            var project = new ServiceResponse<ProjectResponse>();

            Status status = new Status
            {
                StatusCode = "ClosureInitiated",
                StatusDescription = "ClosureInitiated",
                StatusId = 20
            };
            var notificationConfig = new NotificationConfiguration()
            {
                EmailFrom = "it@senecaglobal.com",
                EmailCC = "nishant.giri@senecaglobal.com;hemanth.maddula@senecaglobal.com",
                EmailContent = "<span>Test from DEV &lt;!DOCTYPE html&gt; &lt;html&gt; &lt;head&gt; &lt;style&gt; table { font-family: arial, sans-serif; border-collapse: collapse; } td, th { border: 1px solid #dddddd; text-align: left; padding: 8px; } td:nth-child(1) { background-color: #dddddd; } &lt;/style&gt; &lt;/head&gt; &lt;body&gt; &lt;p&gt;Dear All,&lt;/p&gt;&lt;p&gt;Submitted project details: &lt;table&gt; &lt;tr&gt; &lt;td&gt;&lt;strong&gt;Project Code&lt;/strong&gt;&lt;/td&gt; &lt;td&gt;{ProjectCode}&lt;/td&gt; &lt;/tr&gt; &lt;tr&gt; &lt;td&gt;&lt;strong&gt;Project Name&lt;/strong&gt;&lt;/td&gt; &lt;td&gt;{ProjectName}&lt;/td&gt; &lt;/tr&gt; &lt;tr&gt; &lt;td&gt;&lt;strong&gt;Manager Name&lt;/strong&gt;&lt;/td&gt; &lt;td&gt;{ManagerName}&lt;/td&gt; &lt;/tr&gt; &lt;tr&gt; &lt;td&gt;&lt;strong&gt;Project State&lt;/strong&gt;&lt;/td&gt; &lt;td&gt;{ProjectState}&lt;/td&gt; &lt;/tr&gt; &lt;tr&gt;&lt;td&gt;&lt;strong&gt;Project End Date&lt;/strong&gt;&lt;/td&gt;&lt;td&gt;{ProjectEndDate}&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt; &lt;/body&gt; &lt;p&gt;Thanks !&lt;/p&gt;&lt;p&gt;&lt;br&gt;&lt;/p&gt;&lt;p&gt;&lt;strong&gt;&lt;u&gt;Disclaimer: This is an auto generated email.&lt;/u&gt;&lt;/strong&gt;&lt;/p&gt; &lt;/html&gt;</span>",
                EmailSubject = "Project Closure Initiated",
                EmailTo = "jayanth.chincholi@senecaglobal.com",
                CategoryMasterId = 7
            };
            var employee = new Employee()
            {
                UserId = 24
            };
            var User = new User()
            {
                UserId = 24
            };
            var projectDetails = new ProjectResponse()
            {
                ProjectId = 5,
                ProjectName = "Project 5",
                ProjectCode = "POJ5",
                ProjectState = "ClosureInitiated",
                ManagerName = "",
                ActualEndDate = DateTime.Now.AddDays(10)
            };
            var pro = new ProjectClosureReport();
           
            mockStatus.Item = status;
            notification.Item = notificationConfig;
            checklist.Item = 1;
            emp.Item = employee;
            user.Item = User;
            project.Item = projectDetails;

            mapper.Setup(mock => mock.Map<ProjectClosureReportRequest, Entities.ProjectClosureReport>(It.IsAny<ProjectClosureReportRequest>())).Returns(pro);

            organizationService.Setup(x => x.GetStatusByCategoryAndStatusCode(It.IsAny<string>(),It.IsAny<string>() )).ReturnsAsync(mockStatus);
            organizationService.Setup(x => x.GetUserById(24)).ReturnsAsync(user);
            organizationService.Setup(x => x.GetNotificationConfiguration(4, 7)).ReturnsAsync(notification);


            projectService.Setup(x => x.GetProjectById(5)).ReturnsAsync(project);
            projectClosureReport.Setup(x => x.Create(5)).ReturnsAsync(checklist);
            projectClosureActivity.Setup(x => x.CreateActivityChecklist(5)).ReturnsAsync(checklist);


            associateAllocationService.Setup(x => x.GetByProjectId(5)).ReturnsAsync(allocation);


            employeeService.Setup(x => x.GetEmployeeById(7)).ReturnsAsync(emp);
            var _configurationSection = new Mock<IConfigurationSection>();
            _configurationSection.Setup(a => a.Value).Returns($"D:\\HRMS\\Project\\");
            configuration.Setup(x => x.GetSection(It.IsAny<string>())).Returns(new Mock<IConfigurationSection>().Object);

            Mock<IFileSystem> _fileSystem=new Mock<IFileSystem>();
            _fileSystem.Setup(f => f.DirectoryExists(It.IsAny<String>())).Returns(true);
            

            IProjectClosureReportService m_ProjectClosureReportService = new ProjectClosureReportService(logger_ProjectClosureReportService.Object, 
                dbContext, projectManagerService.Object, organizationService.Object, employeeService.Object, configuration.Object, projectService.Object, mapper.Object,
                null);

            controller = new ProjectClosureReportController(m_ProjectClosureReportService, _logger.Object);

            return controller;
        }
        #endregion
    }
}
