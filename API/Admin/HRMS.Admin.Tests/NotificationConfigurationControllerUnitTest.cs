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
    public class NotificationConfigurationControllerUnitTest
    {
        private Mock<ILogger<NotificationConfigurationController>> _logger;
        private Mock<ILogger<NotificationConfigurationService>> logger;
        private Mock<ILogger<CategoryMasterService>> logger_CategoryMasterService;
        private Mock<ILogger<NotificationTypeService>> logger_NotificationTypeService;

        #region Get Test Cases

        #region GetAllAsync
        [TestMethod]
        public async Task GetAllAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllAsync));

            INotificationConfigurationService m_NotificationConfigurationService;
            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationConfigurationController controller;

            ConfigureTest(dbContext, out m_NotificationConfigurationService, out m_NotificationTypeService,
                          out m_CategoryMasterService, out controller);

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

            INotificationConfigurationService m_NotificationConfigurationService;
            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationConfigurationController controller;

            ConfigureTest(dbContext, out m_NotificationConfigurationService, out m_NotificationTypeService,
                          out m_CategoryMasterService, out controller);

            // Act 
            var response = await controller.GetAll(true);

            var notificationConfigurations = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<NotificationConfiguration>;
            int inActiveRecords = notificationConfigurations.FindAll(cm => cm.IsActive == false).Count;

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

            INotificationConfigurationService m_NotificationConfigurationService;
            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationConfigurationController controller;

            ConfigureTest(dbContext, out m_NotificationConfigurationService, out m_NotificationTypeService,
                          out m_CategoryMasterService, out controller);

            // Act 
            var response = await controller.GetAll(false);

            var notificationConfigurations = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<NotificationConfiguration>;
            int activeRecords = notificationConfigurations.FindAll(cm => cm.IsActive == true).Count;

            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("OkObjectResult".Equals(actualResult.Name.ToString()) && activeRecords == 0);

        }
        #endregion

        #endregion

        #region Get by Notification Type and Category

        [TestMethod]
        public async Task GetByNotificationTypeAndCategoryAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetByNotificationTypeAndCategoryAsync));

            INotificationConfigurationService m_NotificationConfigurationService;
            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationConfigurationController controller;

            ConfigureTest(dbContext, out m_NotificationConfigurationService, out m_NotificationTypeService,
                          out m_CategoryMasterService, out controller);

            int? notificationTypeId = 1;
            int? categoryMasterId = 1;

            // Act
            var response = await controller.GetByNotificationTypeAndCategory(notificationTypeId, categoryMasterId);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }

        #endregion

        #region Get by Notification Type and Category

        [TestMethod]
        public async Task GetByNotificationTypeAndEmailToAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetByNotificationTypeAndEmailToAsync));

            INotificationConfigurationService m_NotificationConfigurationService;
            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationConfigurationController controller;

            ConfigureTest(dbContext, out m_NotificationConfigurationService, out m_NotificationTypeService,
                          out m_CategoryMasterService, out controller);

            int? notificationTypeId = 1;
            string emailTo = "Test1@senecaglobal.com";

            // Act
            var response = await controller.GetByNotificationTypeAndEmailTo(notificationTypeId, emailTo);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }

        #endregion

        #region Create Test Cases

        #region CreateAsync
        [TestMethod]
        public async Task CreateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(CreateAsync));

            INotificationConfigurationService m_NotificationConfigurationService;
            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationConfigurationController controller;

            ConfigureTest(dbContext, out m_NotificationConfigurationService, out m_NotificationTypeService,
                          out m_CategoryMasterService, out controller);

            var notificationConfiguration = new NotificationConfiguration()
            {
                NotificationConfigurationId = 4,
                NotificationTypeId = 4,
                EmailFrom = "Test@senecaglobal.com",
                EmailTo = "Test1@senecaglobal.com",
                EmailCC = "Test2@senecaglobal.com",
                EmailSubject = "Test 4",
                EmailContent = "Test 4",
                SLA = 0,
                CategoryMasterId = 1
            };

            //Act  
            var response = await controller.Create(notificationConfiguration);
            NotificationConfiguration data = (NotificationConfiguration)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("Test 4".Equals(data.EmailSubject));
        }
        #endregion

        #region CreateAsync_BadRequest_AlreadyExists
        [TestMethod]
        public async Task CreateAsync_BadRequest_AlreadyExists()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(CreateAsync_BadRequest_AlreadyExists));

            INotificationConfigurationService m_NotificationConfigurationService;
            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationConfigurationController controller;

            ConfigureTest(dbContext, out m_NotificationConfigurationService, out m_NotificationTypeService,
                          out m_CategoryMasterService, out controller);

            var notificationConfiguration = new NotificationConfiguration()
            {
                NotificationConfigurationId = 5,
                NotificationTypeId = 1,
                EmailFrom = "Test@senecaglobal.com",
                EmailTo = "Test1@senecaglobal.com",
                EmailCC = "Test2@senecaglobal.com",
                EmailSubject = "Test 5",
                EmailContent = "Test 5",
                SLA = 0,
                CategoryMasterId = 1
            };

            //Act  
            var response = await controller.Create(notificationConfiguration);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Notification configuration already exists.".Equals(actualResponse));
        }
        #endregion

        #region CreateAsync_BadRequest_CategoryNotFound
        [TestMethod]
        public async Task CreateAsync_BadRequest_CategoryNotFound()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(CreateAsync_BadRequest_CategoryNotFound));

            INotificationConfigurationService m_NotificationConfigurationService;
            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationConfigurationController controller;

            ConfigureTest(dbContext, out m_NotificationConfigurationService, out m_NotificationTypeService,
                          out m_CategoryMasterService, out controller);

            var notificationConfiguration = new NotificationConfiguration()
            {
                NotificationConfigurationId = 6,
                NotificationTypeId = 1,
                EmailFrom = "Test@senecaglobal.com",
                EmailTo = "Test1@senecaglobal.com",
                EmailCC = "Test2@senecaglobal.com",
                EmailSubject = "Test 6",
                EmailContent = "Test 6",
                SLA = 0,
                CategoryMasterId = 10
            };

            //Act  
            var response = await controller.Create(notificationConfiguration);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Category not found.".Equals(actualResponse));
        }
        #endregion

        #region CreateAsync_BadRequest_NotificationTypeNotFound
        [TestMethod]
        public async Task CreateAsync_BadRequest_NotificationTypeNotFound()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(CreateAsync_BadRequest_NotificationTypeNotFound));

            INotificationConfigurationService m_NotificationConfigurationService;
            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationConfigurationController controller;

            ConfigureTest(dbContext, out m_NotificationConfigurationService, out m_NotificationTypeService,
                          out m_CategoryMasterService, out controller);

            var notificationConfiguration = new NotificationConfiguration()
            {
                NotificationConfigurationId = 7,
                NotificationTypeId = 10,
                EmailFrom = "Test@senecaglobal.com",
                EmailTo = "Test1@senecaglobal.com",
                EmailCC = "Test2@senecaglobal.com",
                EmailSubject = "Test 7",
                EmailContent = "Test 7",
                SLA = 0,
                CategoryMasterId = 1
            };

            //Act  
            var response = await controller.Create(notificationConfiguration);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Notification Type not found.".Equals(actualResponse));
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

            INotificationConfigurationService m_NotificationConfigurationService;
            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationConfigurationController controller;

            ConfigureTest(dbContext, out m_NotificationConfigurationService, out m_NotificationTypeService,
                          out m_CategoryMasterService, out controller);

            var notificationConfiguration = new NotificationConfiguration()
            {
                NotificationConfigurationId = 1,
                NotificationTypeId = 1,
                EmailFrom = "UTest@senecaglobal.com",
                EmailTo = "UTest1@senecaglobal.com",
                EmailCC = "UTest2@senecaglobal.com",
                EmailSubject = "UTest 1",
                EmailContent = "UTest 1",
                SLA = 0,
                CategoryMasterId = 1
            };

            //Act  
            var updateResponse = await controller.Update(notificationConfiguration);
            NotificationConfiguration updatedCompetencyArea =
                (NotificationConfiguration)((Microsoft.AspNetCore.Mvc.ObjectResult)updateResponse).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(updatedCompetencyArea.EmailSubject, "UTest 1");
        }
        #endregion

        #region UpdateAsync_BadRequest_NotificationConfigurationNotFound
        [TestMethod]
        public async Task UpdateAsync_BadRequest_NotificationConfigurationNotFound()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_NotificationConfigurationNotFound));

            INotificationConfigurationService m_NotificationConfigurationService;
            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationConfigurationController controller;

            ConfigureTest(dbContext, out m_NotificationConfigurationService, out m_NotificationTypeService,
                          out m_CategoryMasterService, out controller);

            var notificationConfiguration = new NotificationConfiguration()
            {
                NotificationConfigurationId = 15,
                NotificationTypeId = 5,
                EmailFrom = "UTest@senecaglobal.com",
                EmailTo = "UTest1@senecaglobal.com",
                EmailCC = "UTest2@senecaglobal.com",
                EmailSubject = "UTest 15",
                EmailContent = "UTest 15",
                SLA = 0,
                CategoryMasterId = 2
            };

            //Act  
            var response = await controller.Update(notificationConfiguration);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Notification configuration not found for update.".Equals(actualResponse));
        }
        #endregion

        #region UpdateAsync_BadRequest_CategoryNotFound
        [TestMethod]
        public async Task UpdateAsync_BadRequest_CategoryNotFound()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_CategoryNotFound));

            INotificationConfigurationService m_NotificationConfigurationService;
            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationConfigurationController controller;

            ConfigureTest(dbContext, out m_NotificationConfigurationService, out m_NotificationTypeService,
                          out m_CategoryMasterService, out controller);

            var notificationConfiguration = new NotificationConfiguration()
            {
                NotificationConfigurationId = 15,
                NotificationTypeId = 4,
                EmailFrom = "UTest@senecaglobal.com",
                EmailTo = "UTest1@senecaglobal.com",
                EmailCC = "UTest2@senecaglobal.com",
                EmailSubject = "UTest 15",
                EmailContent = "UTest 15",
                SLA = 0,
                CategoryMasterId = 15
            };

            //Act  
            var response = await controller.Update(notificationConfiguration);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Category not found.".Equals(actualResponse));
        }
        #endregion

        #region UpdateAsync_BadRequest_NotificationTypeNotFound
        [TestMethod]
        public async Task UpdateAsync_BadRequest_NotificationTypeNotFound()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_NotificationTypeNotFound));

            INotificationConfigurationService m_NotificationConfigurationService;
            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationConfigurationController controller;

            ConfigureTest(dbContext, out m_NotificationConfigurationService, out m_NotificationTypeService,
                          out m_CategoryMasterService, out controller);

            var notificationConfiguration = new NotificationConfiguration()
            {
                NotificationConfigurationId = 15,
                NotificationTypeId = 15,
                EmailFrom = "UTest@senecaglobal.com",
                EmailTo = "UTest1@senecaglobal.com",
                EmailCC = "UTest2@senecaglobal.com",
                EmailSubject = "UTest 15",
                EmailContent = "UTest 15",
                SLA = 0,
                CategoryMasterId = 1
            };

            //Act  
            var response = await controller.Update(notificationConfiguration);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Notification Type not found.".Equals(actualResponse));
        }
        #endregion

        #endregion

        #region ConfigureTest
        private void ConfigureTest(AdminContext dbContext, out INotificationConfigurationService m_NotificationConfigurationService,
            out INotificationTypeService m_NotificationTypeService, out ICategoryMasterService m_CategoryMasterService,
            out NotificationConfigurationController controller)
        {
            _logger = new Mock<ILogger<NotificationConfigurationController>>();
            logger = new Mock<ILogger<NotificationConfigurationService>>();
            logger_CategoryMasterService = new Mock<ILogger<CategoryMasterService>>();
            logger_NotificationTypeService = new Mock<ILogger<NotificationTypeService>>();

            m_CategoryMasterService = new CategoryMasterService(logger_CategoryMasterService.Object, dbContext);
            m_NotificationTypeService = new NotificationTypeService(logger_NotificationTypeService.Object, dbContext, m_CategoryMasterService);
            m_NotificationConfigurationService = new NotificationConfigurationService(logger.Object, dbContext,
                m_CategoryMasterService, m_NotificationTypeService);

            controller = new NotificationConfigurationController(m_NotificationConfigurationService, _logger.Object);
        }
        #endregion
    }
}
