using AutoMapper;
using HRMS.Project.API;
using HRMS.Project.API.Controllers;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Service;
using HRMS.Project.Service.External;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Project.Tests
{
    [TestClass]
    public class ProjectClosureControllerUnitTest
    {
        #region MockLoggers
        private Mock<ILogger<ProjectClosureController>> _logger = new Mock<ILogger<ProjectClosureController>>();
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
        #endregion      

        #region ProjectClosureInitiation
        [TestMethod]
        public async Task ProjectClosureInitiation()
        {
            // Arrange
            ProjectClosureController controller = null;
            var dbContext = DbContextMocker.GetProjectDbContext("HRMSProjectDB_Dev_SETP");
            
            var projectCloseData = new ProjectClosureInitiationResponse()
            {
                ProjectId = 5,
                ProjectClosureId = 6,
                CurrentUser = "Anonymous",
                ExpectedClosureDate = DateTime.Now.AddDays(10),
                ActualClosureDate = DateTime.Now.AddYears(5),
                StatusId = 1,
                Remarks = "Good",
                IsTransitionRequired = false,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null,
                ActualEndDate = DateTime.Now.AddDays(10)
            };
            //Act             
            controller = ConfigureTest(dbContext);
            var response = await controller.ProjectClosureInitiation(projectCloseData);
            Int32 data = (Int32)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;
            var mockResult = 1.GetType();
            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(data.GetType(), mockResult);
        }
        #endregion

        #region RejectClosure
        [TestMethod]
        public async Task RejectClosure()
        {
            // Arrange
            ProjectClosureController controller;
            RejectClosureReport rejectClosure = new RejectClosureReport()
            {
                ProjectId = 5,
                RejectRemarks = "Needs Re work"
            };
            var dbContext = DbContextMocker.GetProjectDbContext("HRMSProjectDB_Dev_SETP");

          
            //Act       
            controller = ConfigureTest(dbContext);
            var response = await controller.RejectClosure(rejectClosure);
            Int32 data = (Int32)(response).Value;
            var mockResult = 1.GetType();
            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(data.GetType(), mockResult);
        }
        #endregion

        #region ApproveOrRejectClosureByDH_Approve
        [TestMethod]
        public async Task ApproveOrRejectClosureByDH_Approve()
        {
            // Arrange
            ProjectClosureController controller;
            var dbContext = DbContextMocker.GetProjectDbContext("HRMSProjectDB_Dev_SETP");
            var approveOrRejectClosureRequest = new ApproveOrRejectClosureRequest()
            {
                projectId = 5,
                status = "Approve",
                employeeId = 152
            };

            //Act       
            controller = ConfigureTest(dbContext);
            var response = await controller.ApproveOrRejectClosureByDH(approveOrRejectClosureRequest);
            Int32 data = (Int32)(response).Value;
            var mockResult = 1.GetType();
            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(data.GetType(), mockResult);
        }
        #endregion

        #region ApproveOrRejectClosureByDH_Reject
        [TestMethod]
        public async Task ApproveOrRejectClosureByDH_Reject()
        {
            // Arrange
            ProjectClosureController controller;
            var dbContext = DbContextMocker.GetProjectDbContext("HRMSProjectDB_Dev_SETP");
            var approveOrRejectClosureRequest = new ApproveOrRejectClosureRequest()
            {
                projectId = 5,
                status = "Reject",
                employeeId = 152
            };

            //Act       
            controller = ConfigureTest(dbContext);
            var response = await controller.ApproveOrRejectClosureByDH(approveOrRejectClosureRequest);
            Int32 data = (Int32)(response).Value;
            var mockResult = 1.GetType();
            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(data.GetType(), mockResult);
        }
        #endregion

        #region SubmitForClosureApproval
        [TestMethod]
        public async Task SubmitForClosureApproval()
        {
            // Arrange
            ProjectClosureController controller;
            var dbContext = DbContextMocker.GetProjectDbContext("HRMSProjectDB_Dev_SETP");
            var submitForClosureApproval = new SubmitForClosureApprovalRequest()
            {
                projectId = 5,
                userRole = "Program Manager",
                employeeId = 152
            };

            //Act       
            controller = ConfigureTest(dbContext);
            var response = await controller.SubmitForClosureApproval(submitForClosureApproval);
            Int32 data = (Int32)(response).Value;
            var mockResult = 1.GetType();
            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(data.GetType(), mockResult);
        }
        #endregion

        #region ConfigureTest
        private ProjectClosureController ConfigureTest(ProjectDBContext dbContext)
        {
            ProjectClosureController controller = null;

            var allocation = new ServiceListResponse<AssociateAllocation>();
            var mockStatus = new ServiceResponse<Status>();
            var notification = new ServiceResponse<NotificationConfiguration>();
            var checklist = new ServiceResponse<int>();
            var emp = new ServiceResponse<Employee>();
            var user = new ServiceResponse<User>();
            var project = new ServiceResponse<ProjectResponse>();
            var category = new ServiceResponse<Category>();
            var notificationType = new ServiceResponse<NotificationType>();

            Status status = new Status
            {
                StatusCode = "ClosureInitiated",
                StatusDescription = "ClosureInitiated",
                StatusId = 20
            };
            var notificationTypeDetail = new NotificationType()
            {
                NotificationTypeId = 4
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
            var categoryDetail = new Category()
            {
                CategoryMasterId = 7,
                CategoryName = "PPC",
                ParentId = 0
            };
            var pro = new ProjectClosure();

            mockStatus.Item = status; 
            notification.Item = notificationConfig;           
            checklist.Item = 1;
            emp.Item = employee;       
            user.Item = User;
            project.Item = projectDetails;
            category.Item = categoryDetail;
            notificationType.Item = notificationTypeDetail;

            mapper.Setup(mock => mock.Map<ProjectClosureInitiationResponse, ProjectClosure>(It.IsAny<ProjectClosureInitiationResponse>())).Returns(pro);
            
            organizationService.Setup(x => x.GetStatusByCategoryAndStatusCode(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockStatus);
            organizationService.Setup(x => x.GetUserById(24)).ReturnsAsync(user);
            organizationService.Setup(x => x.GetNotificationConfiguration(4, 7)).ReturnsAsync(notification);
            organizationService.Setup(x => x.GetCategoryByName(It.IsAny<string>())).ReturnsAsync(category);
            organizationService.Setup(x => x.GetStatusByCategoryIdAndStatusCode(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(mockStatus);
            organizationService.Setup(x => x.GetNotificationTypeByCode(It.IsAny<String>())).ReturnsAsync(notificationType);
            organizationService.Setup(x => x.GetByNotificationTypeAndCategoryId(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(notification);
            organizationService.Setup(x => x.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(user);
            
            projectService.Setup(x => x.GetProjectById(5)).ReturnsAsync(project);
            projectClosureReport.Setup(x => x.Create(5)).ReturnsAsync(checklist);
            projectClosureReport.Setup(x => x.NotificationConfiguration(It.IsAny<int>(), It.IsAny<int>(), null)).ReturnsAsync(checklist);
            projectClosureActivity.Setup(x => x.CreateActivityChecklist(5)).ReturnsAsync(checklist);

          
            associateAllocationService.Setup(x => x.GetByProjectId(5)).ReturnsAsync(allocation);

            
            employeeService.Setup(x => x.GetEmployeeById(7)).ReturnsAsync(emp);
            employeeService.Setup(x => x.GetActiveEmployeeById(It.IsAny<int>())).ReturnsAsync(emp);
            employeeService.Setup(x => x.GetEmployeeByUserId(It.IsAny<int>())).ReturnsAsync(emp);

            IProjectClosureService m_ProjectClosureService = new ProjectClosureService(logger.Object, dbContext, projectService.Object, projectManagerService.Object, organizationService.Object, employeeService.Object, associateAllocationService.Object, projectClosureReport.Object, projectClosureActivity.Object, mapper.Object );
            controller = new ProjectClosureController(m_ProjectClosureService, _logger.Object);

            return controller;
        }
        #endregion
    }
}
