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
    public class SkillGroupControllerUnitTest
    {
        private Mock<ILogger<SkillGroupController>> _logger;
        private Mock<ILogger<SkillGroupService>> logger;
        
        #region Get Test Cases

        #region GetAllAsync
        [TestMethod]
        public async Task GetAllAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllAsync));
            ISkillGroupService m_SkillGroupService;
            SkillGroupController m_Controller;

            ConfigureTest(dbContext,out m_SkillGroupService,
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

            ISkillGroupService m_SkillGroupService;
            SkillGroupController m_Controller;

            ConfigureTest(dbContext, out m_SkillGroupService,
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
            ISkillGroupService m_SkillGroupService;
            SkillGroupController m_Controller;

            ConfigureTest(dbContext, out m_SkillGroupService,
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

        #endregion

        #region Create Test Cases

        #region CreateAsync
        [TestMethod]
        public async Task CreateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(CreateAsync));
            ISkillGroupService m_SkillGroupService;
            SkillGroupController m_Controller;

            ConfigureTest(dbContext, out m_SkillGroupService,
                out m_Controller);

            //Input
            var skillGroup = new SkillGroup()
            {
                SkillGroupId = 7,
                SkillGroupName = "Test7",
                Description = "desc7",
                CompetencyAreaId = 7
            };

            //Act  
            var response = await m_Controller.Create(skillGroup);
            SkillGroup data = (SkillGroup)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("Test7".Equals(data.SkillGroupName) && "desc7".Equals(data.Description));
        }
        #endregion

        #region CreateAsync_Invalid
        [TestMethod]
        public async Task CreateAsync_Invalid()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(CreateAsync_Invalid));
            ISkillGroupService m_SkillGroupService;
            SkillGroupController m_Controller;

            ConfigureTest(dbContext, out m_SkillGroupService,
                out m_Controller);

            //Input
            var skillGroup = new SkillGroup()
            {
                SkillGroupId = 7,
                Description = "desc7",
                CompetencyAreaId = 7
            };
            m_Controller.ModelState.AddModelError("SkillGroupName", "Required");
            //Act  
            var response = await m_Controller.Create(skillGroup);          
            var expectedResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "BadRequestObjectResult");
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
            ISkillGroupService m_SkillGroupService;
            SkillGroupController m_Controller;

            ConfigureTest(dbContext, out m_SkillGroupService,
                out m_Controller);

            //Input
            var skillGroup = new SkillGroup()
            {
                SkillGroupId = 5,
                SkillGroupName = "Test5",
                Description = "desc5 updated",
                CompetencyAreaId = 5
            };

            //Act  
            var updateResponse = await m_Controller.Update(skillGroup);
            SkillGroup updatedSkillGroup = (SkillGroup)((Microsoft.AspNetCore.Mvc.ObjectResult)updateResponse).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(updatedSkillGroup.Description, "desc5 updated");
        }
        #endregion

        #region UpdateAsync_BadRequest_InvalidPracticeAreaID
        [TestMethod]
        public async Task UpdateAsync_BadRequest_InvalidPracticeAreaID()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_InvalidPracticeAreaID));
            ISkillGroupService m_SkillGroupService;
            SkillGroupController m_Controller;

            ConfigureTest(dbContext, out m_SkillGroupService,
                out m_Controller);
            //Input
            var skillGroup = new SkillGroup()
            {
                SkillGroupId = 10,
                SkillGroupName = "Test10",
                Description = "desc10 updated",
                CompetencyAreaId = 7
            };

            //Act  
            var response = await m_Controller.Update(skillGroup);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "SkillGroup not found for update.".Equals(actualResponse));
        }
        #endregion
        #endregion

        #region Delete Test Cases

        #region DeleteAsync
        [TestMethod]
        public async Task DeleteAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(DeleteAsync));

            ISkillGroupService m_SkillGroupService;
            SkillGroupController m_Controller;

            ConfigureTest(dbContext, out m_SkillGroupService, out m_Controller);

            //Input
            int SkillGroupId = 4;

            //Act  
            var updateResponse = await m_Controller.Delete(SkillGroupId);
            bool updated = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)updateResponse).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(updated, true);
        }
        #endregion

        #region DeleteAsync_BadRequest_InvalidSkillGroupId
        [TestMethod]
        public async Task DeleteAsync_BadRequest_InvalidSkillGroupId()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(DeleteAsync_BadRequest_InvalidSkillGroupId));

            ISkillGroupService m_SkillGroupService;
            SkillGroupController m_Controller;

            ConfigureTest(dbContext, out m_SkillGroupService, out m_Controller);

            //Input
            int SkillGroupId = 15;

            //Act  
            var response = await m_Controller.Delete(SkillGroupId);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "skillGroup not found for delete.".Equals(actualResponse));
        }
        #endregion

        #endregion

        #region ConfigureTest
        private void ConfigureTest(AdminContext dbContext,out ISkillGroupService m_SkillGroupService,
            out SkillGroupController m_Controller)
        {
            _logger = new Mock<ILogger<SkillGroupController>>();
            logger = new Mock<ILogger<SkillGroupService>>();         
            m_SkillGroupService = new SkillGroupService(logger.Object, dbContext);
            m_Controller = new SkillGroupController(m_SkillGroupService, _logger.Object);
        }
        #endregion
    }
}
