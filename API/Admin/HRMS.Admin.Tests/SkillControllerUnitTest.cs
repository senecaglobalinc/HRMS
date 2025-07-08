using HRMS.Admin.API.Controllers;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Service;
using HRMS.Admin.Types;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Admin.Tests
{
    [TestClass]
    public class SkillControllerUnitTest
    {
        private Mock<ILogger<SkillController>> _logger;
        private Mock<ILogger<SkillService>> logger;

        #region GetAllAsync
        [TestMethod]
        public async Task GetAllAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllAsync));
            ISkillService m_SkillService;
            SkillController m_Controller;

            ConfigureTest(dbContext, out m_SkillService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetAll(null);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(value, null);
        }
        #endregion

        #region GetAllAsync_Active
        [TestMethod]
        public async Task GetAllAsync_Active()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllAsync_Active));

            ISkillService m_SkillService;
            SkillController m_Controller;

            ConfigureTest(dbContext, out m_SkillService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetAll(true);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion

        #region GetAllAsync_InActive
        [TestMethod]
        public async Task GetAllAsync_InActive()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllAsync_InActive));
            ISkillService m_SkillService;
            SkillController m_Controller;

            ConfigureTest(dbContext, out m_SkillService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetAll(false);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(value, null);
        }
        #endregion

        #region Create Test Cases

        #region CreateAsync
        [TestMethod]
        public async Task CreateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(CreateAsync));
            ISkillService m_SkillService;
            SkillController m_Controller;

            ConfigureTest(dbContext, out m_SkillService,
                out m_Controller);

            //Input
            var skill = new Skill()
            {
                SkillId = 8,
                SkillName = "Test8",
                SkillCode = "code8",
                SkillDescription = "desc8",
                IsActive = true,
                IsApproved = true,
                CompetencyAreaId = 7,
                SkillGroupId = 7
            };

            //Act  
            var response = await m_Controller.Create(skill);
            Skill data = (Skill)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("Test8".Equals(data.SkillName) && "desc8".Equals(data.SkillDescription));
        }
        #endregion

        #endregion

        #region Update Test Cases

        #region UpdateAsync
        [TestMethod]
        public async Task UpdateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(UpdateAsync));
            ISkillService m_SkillService;
            SkillController m_Controller;

            ConfigureTest(dbContext, out m_SkillService,
                out m_Controller);

            //Input
            var skill = new Skill()
            {
                SkillId = 6,
                SkillName = "Test6",
                SkillCode = "code6",
                SkillDescription = "desc6 updated",
                IsActive = true,
                IsApproved = true,
                CompetencyAreaId = 6,
                SkillGroupId = 6
            };

            //Act  
            var updateResponse = await m_Controller.Update(skill);
            Skill updatedSkill = (Skill)((Microsoft.AspNetCore.Mvc.ObjectResult)updateResponse).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(updatedSkill.SkillDescription, "desc6 updated");
        }
        #endregion

        #region UpdateAsync_BadRequest_InvalidSkillID
        [TestMethod]
        public async Task UpdateAsync_BadRequest_InvalidSkillID()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_InvalidSkillID));
            ISkillService m_SkillService;
            SkillController m_Controller;

            ConfigureTest(dbContext, out m_SkillService,
                out m_Controller);

            //Input
            var skill = new Skill()
            {
                SkillId = 10,
                SkillName = "Test10",
                SkillCode = "code10",
                SkillDescription = "desc10 updated",
                IsActive = true,
                IsApproved = true,
                CompetencyAreaId = 10,
                SkillGroupId = 10
            };

            //Act  
            var response = await m_Controller.Update(skill);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Skill not found for update.".Equals(actualResponse));
        }
        #endregion
        #endregion

        #region ConfigureTest
        private void ConfigureTest(AdminContext dbContext, out ISkillService m_SkillService,
            out SkillController m_Controller)
        {
            _logger = new Mock<ILogger<SkillController>>();
            logger = new Mock<ILogger<SkillService>>();
            m_SkillService = new SkillService(logger.Object, dbContext);
            m_Controller = new SkillController(m_SkillService, _logger.Object);
        }
        #endregion
    }
}
