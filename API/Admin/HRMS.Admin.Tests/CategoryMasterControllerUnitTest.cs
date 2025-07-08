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
    public class CategoryMasterControllerUnitTest
    {
        private Mock<ILogger<CategoryMasterController>> _logger;
        private Mock<ILogger<CategoryMasterService>> logger;

        #region Create Test Cases

        #region CreateAsync
        [TestMethod]
        public async Task CreateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(CreateAsync));

            ICategoryMasterService m_CategoryMasterService;
            CategoryMasterController controller;

            ConfigureTest(dbContext, out m_CategoryMasterService, out controller);

            var categoryMaster = new CategoryMaster()
            {
                CategoryMasterId = 6,
                CategoryName = "CM00006",
                ParentId = 0
            };
            //Act  
            var response = await controller.Create(categoryMaster);
            CategoryMaster data = (CategoryMaster)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("CM00006".Equals(data.CategoryName));
        }
        #endregion

        #region CreateAsync_BadRequest_CategoryNameAlreadyExists
        [TestMethod]
        public async Task CreateAsync_BadRequest_CategoryNameAlreadyExists()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(CreateAsync_BadRequest_CategoryNameAlreadyExists));

            ICategoryMasterService m_CategoryMasterService;
            CategoryMasterController controller;

            ConfigureTest(dbContext, out m_CategoryMasterService, out controller);

            var categoryMaster = new CategoryMaster()
            {
                CategoryMasterId = 7,
                CategoryName = "CM00001",
                ParentId = 0
            };
            //Act  
            var response = await controller.Create(categoryMaster);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Category name already exists.".Equals(actualResponse));
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

            ICategoryMasterService m_CategoryMasterService;
            CategoryMasterController controller;

            ConfigureTest(dbContext, out m_CategoryMasterService, out controller);

            //Input
            int categoryMasterID = 5;

            //Act  
            var updateResponse = await controller.Delete(categoryMasterID);
            bool updated = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)updateResponse).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(updated, true);
        }
        #endregion

        #region DeleteAsync_BadRequest_InvalidCategoryMasterID
        [TestMethod]
        public async Task DeleteAsync_BadRequest_InvalidCategoryMasterID()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(DeleteAsync_BadRequest_InvalidCategoryMasterID));

            ICategoryMasterService m_CategoryMasterService;
            CategoryMasterController controller;

            ConfigureTest(dbContext, out m_CategoryMasterService, out controller);

            //Input
            int categoryMasterID = 15;

            //Act  
            var response = await controller.Delete(categoryMasterID);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Category master not found for delete.".Equals(actualResponse));
        }
        #endregion

        #endregion

        #region Get Test Cases

        #region GetAllAsync
        [TestMethod]
        public async Task GetAllAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetAllAsync));

            ICategoryMasterService m_CategoryMasterService;
            CategoryMasterController controller;

            ConfigureTest(dbContext, out m_CategoryMasterService, out controller);

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

            ICategoryMasterService m_CategoryMasterService;
            CategoryMasterController controller;

            ConfigureTest(dbContext, out m_CategoryMasterService, out controller);

            // Act 
            var response = await controller.GetAll(true);

            var categories = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<CategoryMaster>;
            int inActiveRecords = categories.FindAll(cm => cm.IsActive == false).Count;

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

            ICategoryMasterService m_CategoryMasterService;
            CategoryMasterController controller;

            ConfigureTest(dbContext, out m_CategoryMasterService, out controller);

            // Act 
            var response = await controller.GetAll(false);

            var categories = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<CategoryMaster>;
            int activeRecords = categories.FindAll(cm => cm.IsActive == true).Count;

            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("OkObjectResult".Equals(actualResult.Name.ToString()) && activeRecords == 0);

        }
        #endregion

        #endregion

        #region Get By Category Name
        [TestMethod]
        public async Task GetByCategoryName()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetByCategoryName));

            ICategoryMasterService m_CategoryMasterService;
            CategoryMasterController controller;

            ConfigureTest(dbContext, out m_CategoryMasterService, out controller);

            string categoryName = "CM00001";

            // Act 
            var response = await controller.GetByCategoryName(categoryName);
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("OkObjectResult".Equals(actualResult.Name.ToString()));

        }
        #endregion

        #region Get Parent Categoies Async
        [TestMethod]
        public async Task GetParentCategoiesAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(GetParentCategoiesAsync));

            ICategoryMasterService m_CategoryMasterService;
            CategoryMasterController controller;

            ConfigureTest(dbContext, out m_CategoryMasterService, out controller);

            // Act 
            var response = await controller.GetParentCategoies();

            var categories = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<CategoryMaster>;
            int nonParentCategoryRecords = categories.FindAll(cm => cm.ParentId != 0).Count;

            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("OkObjectResult".Equals(actualResult.Name.ToString()) && nonParentCategoryRecords == 0);

        }
        #endregion

        #region Update Test Cases

        #region UpdateAsync
        [TestMethod]
        public async Task UpdateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(UpdateAsync));

            ICategoryMasterService m_CategoryMasterService;
            CategoryMasterController controller;

            ConfigureTest(dbContext, out m_CategoryMasterService, out controller);

            var categoryMaster = new CategoryMaster()
            {
                CategoryMasterId = 1,
                CategoryName = "UCM00001",
                ParentId = 0
            };

            //Act  
            var updateResponse = await controller.Update(categoryMaster);
            CategoryMaster updatedCategoryMaster =
                (CategoryMaster)((Microsoft.AspNetCore.Mvc.ObjectResult)updateResponse).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(updatedCategoryMaster.CategoryName, "UCM00001");
        }
        #endregion

        #region UpdateAsync_BadRequest_InvalidCategoryMasterID
        [TestMethod]
        public async Task UpdateAsync_BadRequest_InvalidCategoryMasterID()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_InvalidCategoryMasterID));

            ICategoryMasterService m_CategoryMasterService;
            CategoryMasterController controller;

            ConfigureTest(dbContext, out m_CategoryMasterService, out controller);

            var categoryMaster = new CategoryMaster()
            {
                CategoryMasterId = 15,
                CategoryName = "UCM000015",
                ParentId = 0
            };

            //Act  
            var response = await controller.Update(categoryMaster);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Category master not found for update.".Equals(actualResponse));
        }
        #endregion

        #region UpdateAsync_BadRequest_CategoryNameAlreadyExists
        [TestMethod]
        public async Task UpdateAsync_BadRequest_CategoryNameAlreadyExists()
        {
            // Arrange
            var dbContext = DbContextMocker
                .GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_CategoryNameAlreadyExists));

            ICategoryMasterService m_CategoryMasterService;
            CategoryMasterController controller;

            ConfigureTest(dbContext, out m_CategoryMasterService, out controller);

            var categoryMaster = new CategoryMaster()
            {
                CategoryMasterId = 1,
                CategoryName = "CM00002",
                ParentId = 0
            };

            //Act  
            var response = await controller.Update(categoryMaster);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Category name already exists.".Equals(actualResponse));
        }
        #endregion

        #endregion

        #region ConfigureTest
        private void ConfigureTest(AdminContext dbContext, out ICategoryMasterService m_CategoryMasterService,
            out CategoryMasterController controller)
        {
            _logger = new Mock<ILogger<CategoryMasterController>>();
            logger = new Mock<ILogger<CategoryMasterService>>();

            m_CategoryMasterService = new CategoryMasterService(logger.Object, dbContext);

            controller = new CategoryMasterController(m_CategoryMasterService, _logger.Object);
        }
        #endregion
    }
}
