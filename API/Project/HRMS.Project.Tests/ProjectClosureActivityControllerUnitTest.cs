using AutoMapper;
using HRMS.Project.API.Controllers;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Service;
using HRMS.Project.Service.External;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Project.Tests
{
    [TestClass]
    public class ProjectClosureActivityControllerUnitTest
    {
        #region MockLoggers
        private Mock<ILogger<ProjectClosureActivityController>> _logger = new Mock<ILogger<ProjectClosureActivityController>>();
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

        #region Get

        #region GetActivitiesByProjectIdAndDepartmentId

        [TestMethod]
        public async Task GetActivitiesByProjectIdAndDepartmentId()
        {
            // Arrange
            // Arrange
            ProjectClosureActivityController controller = null;
            var dbContext = DbContextMocker.GetProjectDbContext("HRMSProjectDB_Dev_SETP");


            // Act 
            controller = ConfigureTest(dbContext);
            var response = await controller.GetActivitiesByProjectIdAndDepartmentId(2, 2);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion

        #region GetActivitiesByProjectIdForPM

        [TestMethod]
        public async Task GetActivitiesByProjectIdForPM()
        {
            // Arrange
            ProjectClosureActivityController controller = null;
            var dbContext = DbContextMocker.GetProjectDbContext("HRMSProjectDB_Dev_SETP");

            // Act 
            controller = ConfigureTest(dbContext);
            var response = await controller.GetActivitiesByProjectIdForPM(2);
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

        #region CreateActivityChecklist
        [TestMethod]
        public async Task CreateActivityChecklist()
        {
            // Arrange
            ProjectClosureActivityController controller = null;
            var dbContext = DbContextMocker.GetProjectDbContext("HRMSProjectDB_Dev_SETP");

            //Act       
            controller = ConfigureTest(dbContext);
            var response = await controller.CreateActivityChecklist(5);
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

        #region UpdateActivityChecklist
        [TestMethod]
        public async Task UpdateActivityChecklist()
        {
            // Arrange
            ProjectClosureActivityController controller = null;
            var dbContext = DbContextMocker.GetProjectDbContext("HRMSProjectDB_Dev_SETP");

            List<ProjectClosureActivityDetail> activityDetails = new List<ProjectClosureActivityDetail>();
            var activityChecklist = new ActivityChecklist()
            {
                ProjectId = 1,
                DepartmentId = 2,
                Remarks = "gud",
                StatusId = 1,
                IsActive = true,
                Type = "type",
            };


            //Act 
            controller = ConfigureTest(dbContext);
            var updateResponse = await controller.UpdateActivityChecklist(activityChecklist);
            ActivityChecklist updatedActivityChecklist =
                (ActivityChecklist)((Microsoft.AspNetCore.Mvc.ObjectResult)updateResponse).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(updatedActivityChecklist.Remarks, "gud");
        }
        #endregion
        #endregion



        #region ConfigureTest
        private ProjectClosureActivityController ConfigureTest(ProjectDBContext dbContext)
        {
            ProjectClosureActivityController controller = null;


            var mockStatus = new ServiceResponse<Status>();
            var notification = new ServiceResponse<NotificationConfiguration>();
            var checklist = new ServiceResponse<int>();
            var project = new ServiceResponse<ProjectResponse>();
            var activities = new ServiceListResponse<Activities>();
            var act = new ServiceListResponse<GetActivityChecklist>();
            List<Activity> activityChecklist = new List<Activity>();
            var closureAct = new ServiceListResponse<Activity>()
            { IsSuccessful = true };
            List<Department> departments = new List<Department>();
            var department = new ServiceListResponse<Department>()
            { IsSuccessful = true };
            List<Status> statuses = new List<Status>();
            var statusAct = new ServiceListResponse<Status>()
            { IsSuccessful = true };



            Status status = new Status
            {
                StatusCode = "ClosureInitiated",
                StatusDescription = "ClosureInitiated",
                StatusId = 20
            };
            Status statusChecklist = new Status
            {
                StatusCode = "Inprogress",
                StatusDescription = "Inprogress",
                StatusId = 4
            };

            var notificationConfig = new NotificationConfiguration()
            {
                EmailFrom = "it@senecaglobal.com",
                EmailCC = "nishant.giri@senecaglobal.com;hemanth.maddula@senecaglobal.com",
                EmailContent = "<span>Test from DEV &lt;!DOCTYPE html&gt; &lt;html&gt; &lt;head&gt; &lt;style&gt; table { font-family: arial, sans-serif; border-collapse: collapse; } td, th { border: 1px solid #dddddd; text-align: left; padding: 8px; } td:nth-child(1) { background-color: #dddddd; } &lt;/style&gt; &lt;/head&gt; &lt;body&gt; &lt;p&gt;Dear All,&lt;/p&gt;&lt;p&gt;Submitted project Closure activities: &lt;table&gt; &lt;tr&gt; &lt;td&gt;&lt;strong&gt;Project Code&lt;/strong&gt;&lt;/td&gt; &lt;td&gt;{ProjectCode}&lt;/td&gt; &lt;/tr&gt; &lt;tr&gt; &lt;td&gt;&lt;strong&gt;Project Name&lt;/strong&gt;&lt;/td&gt; &lt;td&gt;{ProjectName}&lt;/td&gt; &lt;/tr&gt; &lt;tr&gt; &lt;td&gt;&lt;strong&gt;Manager Name&lt;/strong&gt;&lt;/td&gt; &lt;td&gt;{ManagerName}&lt;/td&gt; &lt;/tr&gt; &lt;tr&gt; &lt;td&gt;&lt;strong&gt;Project State&lt;/strong&gt;&lt;/td&gt; &lt;td&gt;{ProjectState}&lt;/td&gt; &lt;/tr&gt; &lt;tr&gt;&lt;td&gt;&lt;strong&gt;Project End Date&lt;/strong&gt;&lt;/td&gt;&lt;td&gt;{ProjectEndDate}&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt; &lt;/body&gt; &lt;p&gt;Thanks !&lt;/p&gt;&lt;p&gt;&lt;br&gt;&lt;/p&gt;&lt;p&gt;&lt;strong&gt;&lt;u&gt;Disclaimer: This is an auto generated email.&lt;/u&gt;&lt;/strong&gt;&lt;/p&gt; &lt;/html&gt;</span>",
                EmailSubject = "Project Closure  activities submitted",
                EmailTo = "jayanth.chincholi@senecaglobal.com",
                CategoryMasterId = 7
            };
            var dep = new Department()
            {
                DepartmentId = 2,
                Description = "Desc",
                DepartmentCode = "abc",
                DepartmentHeadId = 1,
                DepartmentTypeId = 3
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
            var actDetail = new GetActivityChecklist()
            {

                ActivityId = 1,
                Value = "val",
                Remarks = "rem"

            };
            var activity = new Activities()
            {
                ProjectId = 2,
                DepartmentId = 2,
                DepartmentName = "IT",
                Remarks = "gud",
                StatusId = 1,
                StatusDescription = "status",

            };
            //activity.ActivityDetails.Add(actDetail);

            var closureActivity = new Activity()
            {
                ActivityId = 1,
                ActivityType = "type",
                Department = "cse",
                DepartmentId = 2,
                Description = "desc"
            };



            var pro = new Entities.Project();

            mockStatus.Item = status;
            mockStatus.IsSuccessful = true;
            notification.Item = notificationConfig;
            checklist.Item = 1;
            project.Item = projectDetails;
            //activities.Items.Add(activity);
            activityChecklist.Add(closureActivity);
            closureAct.Items = activityChecklist;
            departments.Add(dep);
            department.Items = departments;
            statuses.Add(statusChecklist);
            statusAct.Items = statuses;


            //act.Items.Add(actDetail);



            mapper.Setup(mock => mock.Map<Entities.Project, Entities.Project>(It.IsAny<Entities.Project>())).Returns(pro);

            organizationService.Setup(x => x.GetStatusByCategoryAndStatusCode(HRMS.Common.Enums.CategoryMaster.PPC.ToString(), HRMS.Common.Enums.ProjectClosureStatusCodes.InProgress.ToString())).ReturnsAsync(mockStatus);
            organizationService.Setup(x => x.GetNotificationConfiguration(4, 7)).ReturnsAsync(notification);
            organizationService.Setup(x => x.GetClosureActivitiesByDepartment(null)).ReturnsAsync(closureAct);
            organizationService.Setup(x => x.GetAllDepartment(true)).ReturnsAsync(department);
            organizationService.Setup(x => x.GetStatusesByCategoryName(HRMS.Common.Enums.CategoryMaster.PPC.ToString(), false)).ReturnsAsync(statusAct);

            projectClosureActivity.Setup(x => x.CreateActivityChecklist(5)).ReturnsAsync(checklist);

            IProjectClosureActivityService m_ProjectClosureActivityService = new ProjectClosureActivityService(logger_ProjectClosureActivityService.Object, dbContext, organizationService.Object, projectService.Object, projectClosureReport.Object, employeeService.Object, mapper.Object);
            controller = new ProjectClosureActivityController(m_ProjectClosureActivityService, _logger.Object);

            return controller;
        }
        #endregion


    }
}
