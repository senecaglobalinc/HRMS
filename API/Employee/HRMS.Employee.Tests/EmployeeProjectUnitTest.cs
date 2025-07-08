using HRMS.Employee.API.Controllers;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
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
    public class EmployeeProjectUnitTest
    {
        private Mock<ILogger<EmployeeProjectController>> _logger = new Mock<ILogger<EmployeeProjectController>>();
        private Mock<ILogger<EmployeeProjectService>> logger = new Mock<ILogger<EmployeeProjectService>>();
        private EmployeeProjectController controller;
        private string controllerName = "EmployeeProject";

        #region GetByEmployeeIdAsync Test Cases

        #region GetByEmployeeIdAsync
        [TestMethod]
        public async Task GetByEmployeeIdAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(nameof(GetByEmployeeIdAsync));
            ConfigureTest(dbContext, out controller);

            // Act 
            var response = await controller.GetByEmployeeId(3);
            List<EmployeeProject> projects = (List<EmployeeProject>)((Microsoft.AspNetCore.Mvc.ObjectResult)((Microsoft.AspNetCore.Mvc.ActionResult<List<EmployeeProject>>)response.Result).Result).Value;
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(projects.Count, 2);
        }
        #endregion

        #region GetByEmployeeIdReturnEmpty
        [TestMethod]
        public async Task GetByEmployeeIdReturnEmpty()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(nameof(GetByEmployeeIdReturnEmpty));
            ConfigureTest(dbContext, out controller);

            // Act 
            var response = await controller.GetByEmployeeId(10);
            List<EmployeeProject> projects =
                (List<EmployeeProject>)((Microsoft.AspNetCore.Mvc.ObjectResult)((Microsoft.AspNetCore.Mvc.ActionResult<List<EmployeeProject>>)response.Result).Result).Value;

            dbContext.Dispose();

            // Assert
            Assert.AreEqual(projects.Count, 0);
        }
        #endregion

        #endregion


        #region CreateAsync
        [TestMethod]
        public async Task CreateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(controllerName + nameof(CreateAsync));
            ConfigureTest(dbContext, out controller);

            //Input
            List<EmployeeProject> projects = new List<EmployeeProject>() 
            {
                new EmployeeProject(){
                    Id = 10,
                    EmployeeId = 3,
                    DomainId = 1,
                    ProjectName = "HRMS",
                    OrganizationName = "Seneca",
                    KeyAchievements = "",
                    Duration = 12,
                    CurrentUser = "Anonymous",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = null,
                    SystemInfo = null,
                    IsActive = true,
                    CreatedBy = "Anonymous",
                    ModifiedBy = null
                } 
            };
            var employeeDetails = new EmployeeDetails()
            {
                EmpId = 3,
                Projects = projects 
            };

            //Act  
            var response = await controller.Create(employeeDetails);
            EmployeeProject data = (EmployeeProject)((Microsoft.AspNetCore.Mvc.ObjectResult)((Microsoft.AspNetCore.Mvc.ActionResult<EmployeeProject>)response.Result).Result).Value;
            
            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("HRMS".Equals(data.ProjectName) && "Seneca".Equals(data.OrganizationName));
        }
        #endregion

        #region ConfigureTest
        private void ConfigureTest(EmployeeDBContext dbContext,out EmployeeProjectController controller)
        {
            IEmployeeProjectService projectService = new EmployeeProjectService(logger.Object, dbContext);
            controller = new EmployeeProjectController(projectService, _logger.Object);
        }
        #endregion
    }
}
