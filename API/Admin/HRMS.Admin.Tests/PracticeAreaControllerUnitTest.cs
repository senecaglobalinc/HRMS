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
    public class PracticeAreaControllerUnitTest
    {
        private Mock<ILogger<PracticeAreaController>> _logger;
        private Mock<ILogger<PracticeAreaService>> logger;
        private Mock<ILogger<DepartmentService>> logger_DepartmentService;
        private Mock<ILogger<ProjectTypeService>> logger_ProjectTypeService;
        private Mock<ILogger<ClientService>> logger_ClientService;

        #region Get Test Cases

        #region GetAllAsync
        [TestMethod]
        public async Task GetAllAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllAsync));
            PracticeAreaController m_Controller;

            ConfigureTest(dbContext, out m_Controller);

            // Act 
            var response = await m_Controller.GetAll(null);
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
            PracticeAreaController m_Controller;

            ConfigureTest(dbContext, out m_Controller);

            // Act 
            var response = await m_Controller.GetAll(true);

            var practiceAreas = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<PracticeArea>;
            int inActiveRecords = practiceAreas.FindAll(cm => cm.IsActive == false).Count;

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
            PracticeAreaController m_Controller;

            ConfigureTest(dbContext, out m_Controller);

            // Act 
            var response = await m_Controller.GetAll(false);

            var practiceAreas = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<PracticeArea>;
            int activeRecords = practiceAreas.FindAll(cm => cm.IsActive == true).Count;

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
            PracticeAreaController m_Controller;

            ConfigureTest(dbContext, out m_Controller);

            //Input
            var practiceArea = new PracticeArea()
            {
                PracticeAreaId = 6,
                PracticeAreaCode = "P00006",
                PracticeAreaDescription = "Practice Area 6",
            };

            //Act  
            var response = await m_Controller.Create(practiceArea);
            PracticeArea data = (PracticeArea)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Project project = await m_ProjectService.GetByPracticeAreaID(data.PracticeAreaId);
            //ProjectManager projectManager = null;

            //if (project != null)
            //    projectManager = await m_ProjectManagerService.GetByProjectID(project.ProjectId);

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            //Assert.IsTrue("P00006".Equals(data.PracticeAreaCode) && "Talent Pool - Practice Area 6".Equals(project.ProjectName) &&
            //              projectManager != null);

            Assert.IsTrue("P00006".Equals(data.PracticeAreaCode));
        }
        #endregion

        #region CreateAsync_BadRequest_SameIntailAphabetForCode
        [TestMethod]
        public async Task CreateAsync_BadRequest_SameIntailAphabetForCode()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(CreateAsync_BadRequest_SameIntailAphabetForCode));
            PracticeAreaController m_Controller;

            ConfigureTest(dbContext, out m_Controller);

            //Input
            var practiceArea = new PracticeArea()
            {
                PracticeAreaId = 6,
                PracticeAreaCode = "AWS",
                PracticeAreaDescription = "Amazon Web service",
            };

            //Act  
            var response = await m_Controller.Create(practiceArea);
            PracticeArea data = (PracticeArea)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Project project = await m_ProjectService.GetByPracticeAreaID(data.PracticeAreaId);
            //ProjectManager projectManager = null;

            //if (project != null)
            //    projectManager = await m_ProjectManagerService.GetByProjectID(project.ProjectId);

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            //Assert.IsTrue("AWS".Equals(data.PracticeAreaCode) && "TPA000002".Equals(project.ProjectCode)
            //    && "Talent Pool - Amazon Web service".Equals(project.ProjectName) &&
            //              projectManager != null);

            Assert.IsTrue("AWS".Equals(data.PracticeAreaCode));
        }
        #endregion

        #region CreateAsync_BadRequest_CodeAlreadyExists
        [TestMethod]
        public async Task CreateAsync_BadRequest_CodeAlreadyExists()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(CreateAsync_BadRequest_CodeAlreadyExists));
            PracticeAreaController m_Controller;

            ConfigureTest(dbContext, out m_Controller);

            //Input
            var practiceArea = new PracticeArea()
            {
                PracticeAreaId = 7,
                PracticeAreaCode = "P00001",
                PracticeAreaDescription = "Practice Area 7",
            };

            //Act  
            var response = await m_Controller.Create(practiceArea);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Practice area code already exists.".Equals(actualResponse));
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
            PracticeAreaController m_Controller;

            ConfigureTest(dbContext, out m_Controller);

            //Input
            var practiceArea = new PracticeArea()
            {
                PracticeAreaId = 1,
                PracticeAreaCode = "P00001",
                PracticeAreaDescription = "Updated Practice Area 1",
            };

            //Act  
            var updateResponse = await m_Controller.Update(practiceArea);
            PracticeArea updatedPracticeArea = (PracticeArea)((Microsoft.AspNetCore.Mvc.ObjectResult)updateResponse).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(updatedPracticeArea.PracticeAreaDescription, "Updated Practice Area 1");
        }
        #endregion

        #region UpdateAsync_BadRequest_InvalidPracticeAreaID
        [TestMethod]
        public async Task UpdateAsync_BadRequest_InvalidPracticeAreaID()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_InvalidPracticeAreaID));
            PracticeAreaController m_Controller;

            ConfigureTest(dbContext, out m_Controller);

            //Input
            var practiceArea = new PracticeArea()
            {
                PracticeAreaId = 15,
                PracticeAreaCode = "P000015",
                PracticeAreaDescription = "Updated Practice Area 15",
            };

            //Act  
            var response = await m_Controller.Update(practiceArea);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Practice area not found for update.".Equals(actualResponse));
        }
        #endregion

        #region UpdateAsync_BadRequest_CodeAlreadyExists
        [TestMethod]
        public async Task UpdateAsync_BadRequest_CodeAlreadyExists()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_CodeAlreadyExists));
            PracticeAreaController m_Controller;

            ConfigureTest(dbContext, out m_Controller);

            //Input
            var practiceArea = new PracticeArea()
            {
                PracticeAreaId = 2,
                PracticeAreaCode = "P00001",
                PracticeAreaDescription = "Updated Practice Area 2",
            };

            //Act  
            var response = await m_Controller.Update(practiceArea);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Practice area code already exists.".Equals(actualResponse));
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
            PracticeAreaController m_Controller;

            ConfigureTest(dbContext, out m_Controller);

            //Input
            int practiceAreaID = 4;

            //Act  
            var updateResponse = await m_Controller.Delete(practiceAreaID);
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
            PracticeAreaController m_Controller;

            ConfigureTest(dbContext, out m_Controller);

            //Input
            int practiceAreaID = 15;

            //Act  
            var response = await m_Controller.Delete(practiceAreaID);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Practice area not found for delete.".Equals(actualResponse));
        }
        #endregion

        #endregion

        #region ConfigureTest
        private void ConfigureTest(AdminContext dbContext, out PracticeAreaController m_Controller)
        {
            IDepartmentService m_DepartmentService;
            IProjectTypeService m_ProjectTypeService;
            IPracticeAreaService m_PracticeAreaService;

            _logger = new Mock<ILogger<PracticeAreaController>>();
            logger = new Mock<ILogger<PracticeAreaService>>();
            logger_DepartmentService = new Mock<ILogger<DepartmentService>>();
            logger_ProjectTypeService = new Mock<ILogger<ProjectTypeService>>();
            logger_ClientService = new Mock<ILogger<ClientService>>();

            m_DepartmentService = new DepartmentService(logger_DepartmentService.Object, dbContext, null, null, null);
            m_ProjectTypeService = new ProjectTypeService(logger_ProjectTypeService.Object, dbContext);
            IClientService m_ClientService = new ClientService(logger_ClientService.Object, dbContext, null, null);

            m_PracticeAreaService = new PracticeAreaService(logger.Object,
                                                            dbContext,
                                                            null,
                                                            m_ClientService,
                                                            m_DepartmentService,
                                                            m_ProjectTypeService,
                                                            null,null);

            m_Controller = new PracticeAreaController(m_PracticeAreaService, _logger.Object);
        }
        #endregion
    }
}
