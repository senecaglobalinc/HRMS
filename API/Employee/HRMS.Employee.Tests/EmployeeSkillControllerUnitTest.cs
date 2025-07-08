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
    public class EmployeeSkillControllerUnitTest
    {
        private Mock<ILogger<EmployeeSkillController>> _logger = new Mock<ILogger<EmployeeSkillController>>();
        private Mock<ILogger<EmployeeSkillService>> logger = new Mock<ILogger<EmployeeSkillService>>();
        private EmployeeSkillController controller;
        private string controllerName = "EmployeeSkill";
        EmployeeSkillDetails employeeSkillDetails = new EmployeeSkillDetails()
        {
            Id = 10,
            EmployeeId = 3,
            LastUsed = 2019,
            IsPrimary = true,
            Experience = 12,
            SkillId = 10,
            SkillGroupId = 10,
            ProficiencyLevelId = 3,
            CompetencyAreaId = 10
        };

        #region Create TestCases
        #region CreateAsync
        [TestMethod]
        public async Task CreateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(controllerName + nameof(CreateAsync));
            ConfigureTest(dbContext, out controller);

            //Act  
            var response = await controller.Create(employeeSkillDetails);
            EmployeeSkill data = (EmployeeSkill)((Microsoft.AspNetCore.Mvc.ObjectResult)((Microsoft.AspNetCore.Mvc.ActionResult<EmployeeSkill>)response.Result).Result).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(10,data.SkillId);
        }
        #endregion

        #region CreateAsync_NotFound_SkillAlreadyExists
        [TestMethod]
        public async Task CreateAsync_NotFound_SkillAlreadyExists()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(controllerName + nameof(CreateAsync_NotFound_SkillAlreadyExists));
            ConfigureTest(dbContext, out controller);

            //input
            employeeSkillDetails.SkillId = 1;

            //Act  
            var response = await controller.Create(employeeSkillDetails);
            var result = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(result.Name.ToString(), "NotFoundObjectResult");
        }
        #endregion
        #endregion

        #region Update TestCases
        #region UpdateAsync
        [TestMethod]
        public async Task UpdateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(controllerName + nameof(UpdateAsync));
            ConfigureTest(dbContext, out controller);

            //input
            employeeSkillDetails.Id = 1;

            //Act  
            var response = await controller.Update(employeeSkillDetails);
            EmployeeSkill data = (EmployeeSkill)((Microsoft.AspNetCore.Mvc.ObjectResult)((Microsoft.AspNetCore.Mvc.ActionResult<EmployeeSkill>)response.Result).Result).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(10, data.SkillId);
        }
        #endregion
        #region UpdateAsync_NotFound_SkillAlreadyExists
        [TestMethod]
        public async Task UpdateAsync_NotFound_SkillAlreadyExists()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(controllerName + nameof(UpdateAsync_NotFound_SkillAlreadyExists));
            ConfigureTest(dbContext, out controller);

            //input
            employeeSkillDetails.SkillId = 1;

            //Act  
            var response = await controller.Update(employeeSkillDetails);
            var result = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(result.Name.ToString(), "NotFoundObjectResult");
        }
        #endregion
        #region UpdateAsync_Id_NotFound
        [TestMethod]
        public async Task UpdateAsync_Id_NotFound()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(controllerName + nameof(UpdateAsync_Id_NotFound));
            ConfigureTest(dbContext, out controller);

            //Input
            employeeSkillDetails.Id = 100;

            //Act  
            var response = await controller.Update(employeeSkillDetails);
            var result = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(result.Name.ToString(), "NotFoundObjectResult");
        }
        #endregion
        #endregion

        #region ConfigureTest
        private void ConfigureTest(EmployeeDBContext dbContext, out EmployeeSkillController controller)
        {
            IEmployeeSkillService skillService = new EmployeeSkillService(logger.Object, dbContext, null, null,null);
            controller = new EmployeeSkillController(skillService, _logger.Object);
        }
        #endregion

    }
}
