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
    public class NotificationTypeControllerUnitTest
    {
        private Mock<ILogger<NotificationTypeController>> _logger;
        private Mock<ILogger<NotificationTypeService>> logger;
        private Mock<ILogger<CategoryMasterService>> logger_CategoryMasterService;

        #region Get Test Cases

        #region GetAllAsync
        [TestMethod]
        public async Task GetAllAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllAsync));

            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationTypeController controller;

            ConfigureTest(dbContext, out m_NotificationTypeService, out m_CategoryMasterService, out controller);

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

            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationTypeController controller;

            ConfigureTest(dbContext, out m_NotificationTypeService, out m_CategoryMasterService, out controller);

            // Act 
            var response = await controller.GetAll(true);

            var notificationTypes = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<NotificationType>;
            int inActiveRecords = notificationTypes.FindAll(cm => cm.IsActive == false).Count;

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

            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationTypeController controller;

            ConfigureTest(dbContext, out m_NotificationTypeService, out m_CategoryMasterService, out controller);

            // Act 
            var response = await controller.GetAll(false);
            
            var notificationTypes = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<NotificationType>;
            int activeRecords = notificationTypes.FindAll(cm => cm.IsActive == true).Count;

            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("OkObjectResult".Equals(actualResult.Name.ToString()) && activeRecords == 0);

        }
        #endregion

        #endregion

        #region Get By Notification Code
        [TestMethod]
        public async Task GetByNotificationCode()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetByNotificationCode));

            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationTypeController controller;

            ConfigureTest(dbContext, out m_NotificationTypeService, out m_CategoryMasterService, out controller);

            string notificationCode = "NT00001";

            // Act 
            var response = await controller.GetByNotificationCode(notificationCode);
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

            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationTypeController controller;

            ConfigureTest(dbContext, out m_NotificationTypeService, out m_CategoryMasterService, out controller);

            var notificationType = new NotificationType()
            {
                NotificationTypeId = 6,
                NotificationCode = "NT00006",
                NotificationDescription = "Notification Description 6",
                CategoryMasterId = 1
            };
            //Act  
            var response = await controller.Create(notificationType);
            NotificationType data = (NotificationType)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("NT00006".Equals(data.NotificationCode));
        }
        #endregion

        #region CreateAsync_BadRequest_CodeAlreadyExists
        [TestMethod]
        public async Task CreateAsync_BadRequest_CodeAlreadyExists()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(CreateAsync_BadRequest_CodeAlreadyExists));

            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationTypeController controller;

            ConfigureTest(dbContext, out m_NotificationTypeService, out m_CategoryMasterService, out controller);

            var notificationType = new NotificationType()
            {
                NotificationTypeId = 7,
                NotificationCode = "NT00002",
                NotificationDescription = "Notification Description 7",
                CategoryMasterId = 1
            };
            //Act  
            var response = await controller.Create(notificationType);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Notification code already exists.".Equals(actualResponse));
        }
        #endregion

        #region CreateAsync_BadRequest_CategoryNotFound
        [TestMethod]
        public async Task CreateAsync_BadRequest_CategoryNotFound()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(CreateAsync_BadRequest_CategoryNotFound));

            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationTypeController controller;

            ConfigureTest(dbContext, out m_NotificationTypeService, out m_CategoryMasterService, out controller);

            var notificationType = new NotificationType()
            {
                NotificationTypeId = 8,
                NotificationCode = "NT00008",
                NotificationDescription = "Notification Description 8",
                CategoryMasterId = 50
            };
            //Act  
            var response = await controller.Create(notificationType);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Category not found.".Equals(actualResponse));
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

            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationTypeController controller;

            ConfigureTest(dbContext, out m_NotificationTypeService, out m_CategoryMasterService, out controller);

            var notificationType = new NotificationType()
            {
                NotificationTypeId = 1,
                NotificationCode = "UNT00001",
                NotificationDescription = "Notification Description 1",
                CategoryMasterId = 1
            };

            //Act  
            var updateResponse = await controller.Update(notificationType);
            NotificationType updatedCompetencyArea =
                (NotificationType)((Microsoft.AspNetCore.Mvc.ObjectResult)updateResponse).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(updatedCompetencyArea.NotificationCode, "UNT00001");
        }
        #endregion

        #region UpdateAsync_BadRequest_InvalidNotificationTypeId
        [TestMethod]
        public async Task UpdateAsync_BadRequest_InvalidNotificationTypeId()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_InvalidNotificationTypeId));

            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationTypeController controller;

            ConfigureTest(dbContext, out m_NotificationTypeService, out m_CategoryMasterService, out controller);

            var notificationType = new NotificationType()
            {
                NotificationTypeId = 15,
                NotificationCode = "NT000015",
                NotificationDescription = "Notification Description 15",
                CategoryMasterId = 1
            };

            //Act  
            var response = await controller.Update(notificationType);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Notification type not found for update.".Equals(actualResponse));
        }
        #endregion

        #region UpdateAsync_BadRequest_CodeAlreadyExists
        [TestMethod]
        public async Task UpdateAsync_BadRequest_CodeAlreadyExists()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_CodeAlreadyExists));

            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationTypeController controller;

            ConfigureTest(dbContext, out m_NotificationTypeService, out m_CategoryMasterService, out controller);

            var notificationType = new NotificationType()
            {
                NotificationTypeId = 1,
                NotificationCode = "NT00002",
                NotificationDescription = "Notification Description 1",
                CategoryMasterId = 1
            };

            //Act  
            var response = await controller.Update(notificationType);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Notification code already exists.".Equals(actualResponse));
        }
        #endregion

        #region UpdateAsync_BadRequest_CategoryNotFound
        [TestMethod]
        public async Task UpdateAsync_BadRequest_CategoryNotFound()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_CategoryNotFound));

            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationTypeController controller;

            ConfigureTest(dbContext, out m_NotificationTypeService, out m_CategoryMasterService, out controller);

            var notificationType = new NotificationType()
            {
                NotificationTypeId = 1,
                NotificationCode = "NT00001",
                NotificationDescription = "Notification Description 1",
                CategoryMasterId = 50
            };

            //Act  
            var response = await controller.Update(notificationType);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Category not found.".Equals(actualResponse));
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

            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationTypeController controller;

            ConfigureTest(dbContext, out m_NotificationTypeService, out m_CategoryMasterService, out controller);

            //Input
            int notificationTypeID = 4;

            //Act  
            var updateResponse = await controller.Delete(notificationTypeID);
            bool updated = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)updateResponse).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(updated, true);
        }
        #endregion

        #region DeleteAsync_BadRequest_InvalidNotificationTypeID
        [TestMethod]
        public async Task DeleteAsync_BadRequest_InvalidNotificationTypeID()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(DeleteAsync_BadRequest_InvalidNotificationTypeID));

            INotificationTypeService m_NotificationTypeService;
            ICategoryMasterService m_CategoryMasterService;
            NotificationTypeController controller;

            ConfigureTest(dbContext, out m_NotificationTypeService, out m_CategoryMasterService, out controller);

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
                          "Notification type not found for delete.".Equals(actualResponse));
        }
        #endregion

        #endregion

        #region ConfigureTest
        private void ConfigureTest(AdminContext dbContext, out INotificationTypeService m_NotificationTypeService,
            out ICategoryMasterService m_CategoryMasterService, out NotificationTypeController controller)
        {
            _logger = new Mock<ILogger<NotificationTypeController>>();
            logger = new Mock<ILogger<NotificationTypeService>>();
            logger_CategoryMasterService = new Mock<ILogger<CategoryMasterService>>();

            m_CategoryMasterService = new CategoryMasterService(logger_CategoryMasterService.Object, dbContext);
            m_NotificationTypeService = new NotificationTypeService(logger.Object, dbContext, m_CategoryMasterService);

            controller = new NotificationTypeController(m_NotificationTypeService, _logger.Object);
        }
        #endregion
    }
}
