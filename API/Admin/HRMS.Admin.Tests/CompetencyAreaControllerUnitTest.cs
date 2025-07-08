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
using System.Threading.Tasks;

namespace HRMS.Admin.Tests
{
    [TestClass]
    public class CompetencyAreaControllerUnitTest
    {
        private Mock<ILogger<CompetencyAreaController>> _logger;
        private Mock<ILogger<CompetencyAreaService>> logger;
        private Mock<ILogger<SkillService>> logger_SkillService;
        private Mock<ILogger<SkillGroupService>> logger_SkillGroupService;

        #region Get Test Cases

        #region GetAllAsync
        [TestMethod]
        public async Task GetAllAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllAsync));

            ICompetencyAreaService m_CompetencyAreaService;
            ISkillService m_SkillService;
            ISkillGroupService m_SkillGroupService;
            CompetencyAreaController controller;

            ConfigureTest(dbContext, out m_CompetencyAreaService, out m_SkillService, out m_SkillGroupService, out controller);

            // Act 
            var response = await controller.GetAll(null);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion

        #region GetAllAsync_Active
        [TestMethod]
        public async Task GetAllAsync_Active()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllAsync_Active));

            ICompetencyAreaService m_CompetencyAreaService;
            ISkillService m_SkillService;
            ISkillGroupService m_SkillGroupService;
            CompetencyAreaController controller;

            ConfigureTest(dbContext, out m_CompetencyAreaService, out m_SkillService, out m_SkillGroupService, out controller);

            // Act 
            var response = await controller.GetAll(true);

            var competencyAreas = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<CompetencyArea>;
            int inActiveRecords = competencyAreas.FindAll(cm => cm.IsActive == false).Count;

            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("OkObjectResult".Equals(actualResult.Name.ToString()) && inActiveRecords == 0);
        }
        #endregion

        #region GetAllAsync_InActive
        [TestMethod]
        public async Task GetAllAsync_InActive()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllAsync_InActive));

            ICompetencyAreaService m_CompetencyAreaService;
            ISkillService m_SkillService;
            ISkillGroupService m_SkillGroupService;
            CompetencyAreaController controller;

            ConfigureTest(dbContext, out m_CompetencyAreaService, out m_SkillService, out m_SkillGroupService, out controller);

            // Act 
            var response = await controller.GetAll(false);

            var competencyAreas = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<CompetencyArea>;
            int activeRecords = competencyAreas.FindAll(cm => cm.IsActive == true).Count;

            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("OkObjectResult".Equals(actualResult.Name.ToString()) && activeRecords == 0);
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

            ICompetencyAreaService m_CompetencyAreaService;
            ISkillService m_SkillService;
            ISkillGroupService m_SkillGroupService;
            CompetencyAreaController controller;

            ConfigureTest(dbContext, out m_CompetencyAreaService, out m_SkillService, out m_SkillGroupService, out controller);

            var competencyArea = new CompetencyArea()
            {
                CompetencyAreaId = 10,
                CompetencyAreaCode = "C00010",
                CompetencyAreaDescription = "Competency Area 10",
            };
            //Act  
            var response = await controller.Create(competencyArea);
            CompetencyArea data = (CompetencyArea)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("C00010".Equals(data.CompetencyAreaCode));
        }
        #endregion

        #region CreateAsync_BadRequest_CodeAlreadyExists
        [TestMethod]
        public async Task CreateAsync_BadRequest_CodeAlreadyExists()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(CreateAsync_BadRequest_CodeAlreadyExists));

            ICompetencyAreaService m_CompetencyAreaService;
            ISkillService m_SkillService;
            ISkillGroupService m_SkillGroupService;
            CompetencyAreaController controller;

            ConfigureTest(dbContext, out m_CompetencyAreaService, out m_SkillService, out m_SkillGroupService, out controller);

            var competencyArea = new CompetencyArea()
            {
                CompetencyAreaId = 7,
                CompetencyAreaCode = "C00001",
                CompetencyAreaDescription = "Competency Area 7",
            };

            //Act  
            var response = await controller.Create(competencyArea);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Competency area code already exists.".Equals(actualResponse));
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

            ICompetencyAreaService m_CompetencyAreaService;
            ISkillService m_SkillService;
            ISkillGroupService m_SkillGroupService;
            CompetencyAreaController controller;

            ConfigureTest(dbContext, out m_CompetencyAreaService, out m_SkillService, out m_SkillGroupService, out controller);

            var competencyArea = new CompetencyArea()
            {
                CompetencyAreaId = 7,
                CompetencyAreaCode = "C00007",
                CompetencyAreaDescription = "Updated Competency Area 7",
            };

            //Act  
            var updateResponse = await controller.Update(competencyArea);
            CompetencyArea updatedCompetencyArea = (CompetencyArea)((Microsoft.AspNetCore.Mvc.ObjectResult)updateResponse).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(updatedCompetencyArea.CompetencyAreaDescription, "Updated Competency Area 7");
        }
        #endregion

        #region UpdateAsync_BadRequest_InvalidCompetencyAreaId
        [TestMethod]
        public async Task UpdateAsync_BadRequest_InvalidCompetencyAreaId()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_InvalidCompetencyAreaId));

            ICompetencyAreaService m_CompetencyAreaService;
            ISkillService m_SkillService;
            ISkillGroupService m_SkillGroupService;
            CompetencyAreaController controller;

            ConfigureTest(dbContext, out m_CompetencyAreaService, out m_SkillService, out m_SkillGroupService, out controller);

            var competencyArea = new CompetencyArea()
            {
                CompetencyAreaId = 15,
                CompetencyAreaCode = "C000015",
                CompetencyAreaDescription = "Updated Competency Area 15",
            };

            //Act  
            var response = await controller.Update(competencyArea);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Competency area not found for update.".Equals(actualResponse));
        }
        #endregion

        #region UpdateAsync_BadRequest_CodeAlreadyExists
        [TestMethod]
        public async Task UpdateAsync_BadRequest_CodeAlreadyExists()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_CodeAlreadyExists));

            ICompetencyAreaService m_CompetencyAreaService;
            ISkillService m_SkillService;
            ISkillGroupService m_SkillGroupService;
            CompetencyAreaController controller;

            ConfigureTest(dbContext, out m_CompetencyAreaService, out m_SkillService, out m_SkillGroupService, out controller);

            var competencyArea = new CompetencyArea()
            {
                CompetencyAreaId = 7,
                CompetencyAreaCode = "C00001",
                CompetencyAreaDescription = "Updated Competency Area 2",
            };

            //Act  
            var response = await controller.Update(competencyArea);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Competency area code already exists.".Equals(actualResponse));
        }
        #endregion

        #region UpdateAsync_BadRequest_SkillExists
        [TestMethod]
        public async Task UpdateAsync_BadRequest_SkillExists()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_SkillExists));

            ICompetencyAreaService m_CompetencyAreaService;
            ISkillService m_SkillService;
            ISkillGroupService m_SkillGroupService;
            CompetencyAreaController controller;

            ConfigureTest(dbContext, out m_CompetencyAreaService, out m_SkillService, out m_SkillGroupService, out controller);

            var competencyArea = new CompetencyArea()
            {
                CompetencyAreaId = 1,
                CompetencyAreaCode = "C00001",
                CompetencyAreaDescription = "Updated Competency Area 1",
            };

            //Act  
            var response = await controller.Update(competencyArea);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Skills exists under this competency area.".Equals(actualResponse));
        }
        #endregion

        #region UpdateAsync_BadRequest_SkillGroupsExists
        [TestMethod]
        public async Task UpdateAsync_BadRequest_SkillGroupsExists()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_SkillGroupsExists));

            ICompetencyAreaService m_CompetencyAreaService;
            ISkillService m_SkillService;
            ISkillGroupService m_SkillGroupService;
            CompetencyAreaController controller;

            ConfigureTest(dbContext, out m_CompetencyAreaService, out m_SkillService, out m_SkillGroupService, out controller);

            var competencyArea = new CompetencyArea()
            {
                CompetencyAreaId = 8,
                CompetencyAreaCode = "C00008",
                CompetencyAreaDescription = "Updated Competency Area 8s",
            };

            //Act  
            var response = await controller.Update(competencyArea);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Skill Groups exists under this competency area.".Equals(actualResponse));
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

            ICompetencyAreaService m_CompetencyAreaService;
            ISkillService m_SkillService;
            ISkillGroupService m_SkillGroupService;
            CompetencyAreaController controller;

            ConfigureTest(dbContext, out m_CompetencyAreaService, out m_SkillService, out m_SkillGroupService, out controller);

            //Input
            int competencyAreaID = 7;

            //Act  
            var updateResponse = await controller.Delete(competencyAreaID);
            bool updated = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)updateResponse).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(updated, true);
        }
        #endregion

        #region DeleteAsync_BadRequest_InvalidPracticeAreaID
        [TestMethod]
        public async Task DeleteAsync_BadRequest_InvalidPracticeAreaID()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(DeleteAsync_BadRequest_InvalidPracticeAreaID));

            ICompetencyAreaService m_CompetencyAreaService;
            ISkillService m_SkillService;
            ISkillGroupService m_SkillGroupService;
            CompetencyAreaController controller;

            ConfigureTest(dbContext, out m_CompetencyAreaService, out m_SkillService, out m_SkillGroupService, out controller);

            //Input
            int competencyAreaID = 15;

            //Act  
            var response = await controller.Delete(competencyAreaID);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Competency area not found for delete.".Equals(actualResponse));
        }
        #endregion

        #region DeleteAsync_BadRequest_SkillExists
        [TestMethod]
        public async Task DeleteAsync_BadRequest_SkillExists()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(DeleteAsync_BadRequest_SkillExists));

            ICompetencyAreaService m_CompetencyAreaService;
            ISkillService m_SkillService;
            ISkillGroupService m_SkillGroupService;
            CompetencyAreaController controller;

            ConfigureTest(dbContext, out m_CompetencyAreaService, out m_SkillService, out m_SkillGroupService, out controller);

            //Input
            int competencyAreaID = 1;

            //Act  
            var response = await controller.Delete(competencyAreaID);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Skills exists under this competency area.".Equals(actualResponse));
        }
        #endregion

        #region DeleteAsync_BadRequest_SkillGroupsExists
        [TestMethod]
        public async Task DeleteAsync_BadRequest_SkillGroupsExists()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(DeleteAsync_BadRequest_SkillGroupsExists));

            ICompetencyAreaService m_CompetencyAreaService;
            ISkillService m_SkillService;
            ISkillGroupService m_SkillGroupService;
            CompetencyAreaController controller;

            ConfigureTest(dbContext, out m_CompetencyAreaService, out m_SkillService, out m_SkillGroupService, out controller);

            //Input
            int competencyAreaID = 8;

            //Act  
            var response = await controller.Delete(competencyAreaID);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Skill Groups exists under this competency area.".Equals(actualResponse));
        }
        #endregion

        #endregion

        #region ConfigureTest
        private void ConfigureTest(AdminContext dbContext, out ICompetencyAreaService m_CompetencyAreaService, out ISkillService m_SkillService, out ISkillGroupService m_SkillGroupService, out CompetencyAreaController controller)
        {
            _logger = new Mock<ILogger<CompetencyAreaController>>();
            logger = new Mock<ILogger<CompetencyAreaService>>();
            logger_SkillService = new Mock<ILogger<SkillService>>();
            logger_SkillGroupService = new Mock<ILogger<SkillGroupService>>();

            m_SkillService = new SkillService(logger_SkillService.Object, dbContext);
            m_SkillGroupService = new SkillGroupService(logger_SkillGroupService.Object, dbContext);
            m_CompetencyAreaService = new CompetencyAreaService(logger.Object, dbContext, m_SkillService, m_SkillGroupService);

            controller = new CompetencyAreaController(m_CompetencyAreaService, _logger.Object);
        }
        #endregion
    }
}
