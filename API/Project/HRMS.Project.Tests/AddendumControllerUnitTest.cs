using AutoMapper;
using HRMS.Common.Enums;
using HRMS.Common.Extensions;
using HRMS.Project.API;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Service;
using HRMS.Project.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Project.Tests
{
    [TestClass]
    public class AddendumControllerUnitTest
    {
        private Mock<ILogger<AddendumController>> _logger;
        private Mock<ILogger<AddendumService>> logger;

        #region Get

        #region GetAllBySOWIdAndProjectId
        [TestMethod]
        public async Task GetAllBySOWIdAndProjectId()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(GetAllBySOWIdAndProjectId));

            IAddendumService m_AddendumService;
            AddendumController controller;

            ConfigureTest(dbContext, out m_AddendumService, out controller);

            // Act 
            var response = await controller.GetAllBySOWIdAndProjectId(1, 1);
            var value = (List<Addendum>)((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("OkObjectResult".Equals(actualResult.Name.ToString()) && value != null && value.Count > 0);
        }
        #endregion

        #region GetAllBySOWIdAndProjectId_BadRequest
        [TestMethod]
        public async Task GetAllBySOWIdAndProjectId_BadRequest()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(GetAllBySOWIdAndProjectId_BadRequest));

            IAddendumService m_AddendumService;
            AddendumController controller;

            ConfigureTest(dbContext, out m_AddendumService, out controller);

            // Act 
            var response = await controller.GetAllBySOWIdAndProjectId(15, 1);
            var value = (List<Addendum>)((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value;

            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("OkObjectResult".Equals(actualResult.Name.ToString()) && (value == null || value != null && value.Count == 0));
        }
        #endregion

        #region GetByIdAndProjectIdAsync_DepartmentHead
        [TestMethod]
        public async Task GetByIdAndProjectIdAsync_DepartmentHead()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(GetByIdAndProjectIdAsync_DepartmentHead));

            IAddendumService m_AddendumService;
            AddendumController controller;

            ConfigureTest(dbContext, out m_AddendumService, out controller);

            // Act 
            var response = await controller.GetByIdAndProjectId(1, 1, Roles.DepartmentHead.GetEnumDescription());
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue(actualResult.Name.ToString().Equals("OkObjectResult"));
        }
        #endregion

        #region GetByIdAndProjectIdAsync_ProgramManager
        [TestMethod]
        public async Task GetByIdAndProjectIdAsync_ProgramManager()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(GetByIdAndProjectIdAsync_ProgramManager));

            IAddendumService m_AddendumService;
            AddendumController controller;

            ConfigureTest(dbContext, out m_AddendumService, out controller);

            // Act 
            var response = await controller.GetByIdAndProjectId(1, 1, Roles.ProgramManager.GetEnumDescription());
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue(actualResult.Name.ToString().Equals("OkObjectResult"));
        }
        #endregion

        #region GetByIdAndProjectId_InvalidRole
        [TestMethod]
        public async Task GetByIdAndProjectId_InvalidRole()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(GetByIdAndProjectId_InvalidRole));

            IAddendumService m_AddendumService;
            AddendumController controller;

            ConfigureTest(dbContext, out m_AddendumService, out controller);

            // Act 
            var response = await controller.GetByIdAndProjectId(1, 1, Roles.Delivery.GetEnumDescription());
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue(actualResult.Name.ToString().Equals("NotFoundResult"));
        }
        #endregion


        #region GetByIdAndProjectId_InvalidInputs
        [TestMethod]
        public async Task GetByIdAndProjectId_InvalidInputs()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(GetByIdAndProjectId_InvalidInputs));

            IAddendumService m_AddendumService;
            AddendumController controller;

            ConfigureTest(dbContext, out m_AddendumService, out controller);

            // Act 
            var response = await controller.GetByIdAndProjectId(15, 1, Roles.ProgramManager.GetEnumDescription());
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue(actualResult.Name.ToString().Equals("NotFoundResult"));
        }
        #endregion


        #endregion

        #region Create

        #region CreateAsync_ProgramManager
        [TestMethod]
        public async Task CreateAsync_ProgramManager()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(CreateAsync_ProgramManager));

            IAddendumService m_AddendumService;
            AddendumController controller;

            ConfigureTest(dbContext, out m_AddendumService, out controller);

            var addendum = new Addendum()
            {
                AddendumId = 6,
                Id = 5,
                SOWId = "SOW5",
                AddendumDate = DateTime.Now.AddDays(-1),
                AddendumNo = "ADN06",
                Note = "Test Note",
                RecipientName = "Test User",
                CurrentUser = "Anonymous",
                ProjectId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            };

            AddendumRequest addendumRequest = new AddendumRequest()
            {
                //Addendum = addendum,
                RoleName = Roles.ProgramManager.GetEnumDescription()
            };
            //Act  
            var response = await controller.Create(addendumRequest);

            var okResult = response as OkObjectResult;
            var actualResult = okResult.Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult, true);
            
        }
        #endregion

        #region CreateAsync_DepartmentHead
        [TestMethod]
        public async Task CreateAsync_DepartmentHead()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(CreateAsync_DepartmentHead));

            IAddendumService m_AddendumService;
            AddendumController controller;

            ConfigureTest(dbContext, out m_AddendumService, out controller);

            var addendum = new Addendum()
            {
                AddendumId = 7,
                Id = 5,
                SOWId = "SOW5",
                AddendumDate = DateTime.Now.AddDays(-1),
                AddendumNo = "ADN07",
                Note = "Test Note",
                RecipientName = "Test User",
                CurrentUser = "Anonymous",
                ProjectId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            };

            AddendumRequest addendumRequest = new AddendumRequest()
            {
                //Addendum = addendum,
                RoleName = Roles.DepartmentHead.GetEnumDescription()
            };
            //Act  
            var response = await controller.Create(addendumRequest);
            var okResult = response as OkObjectResult;
            var actualResult = okResult.Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult, true);
           
        }
        #endregion

        #region CreateAsync_InvalidRole
        [TestMethod]
        public async Task CreateAsync_InvalidRole()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(CreateAsync_InvalidRole));

            IAddendumService m_AddendumService;
            AddendumController controller;

            ConfigureTest(dbContext, out m_AddendumService, out controller);

            var addendum = new Addendum()
            {
                AddendumId = 8,
                Id = 5,
                SOWId = "SOW5",
                AddendumDate = DateTime.Now.AddDays(-1),
                AddendumNo = "ADN08",
                Note = "Test Note",
                RecipientName = "Test User",
                CurrentUser = "Anonymous",
                ProjectId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            };

            AddendumRequest addendumRequest = new AddendumRequest()
            {
                //Addendum = addendum,
                RoleName = Roles.Delivery.GetEnumDescription()
            };
            //Act  
            var response = await controller.Create(addendumRequest);
            string errorMessage = ((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value.ToString();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("Your current role does not have permission to create Addendum.".Equals(errorMessage));
        }
        #endregion 

        #endregion

        #region Update

        #region UpdateAsync_ProgramManager
        [TestMethod]
        public async Task UpdateAsync_ProgramManager()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(UpdateAsync_ProgramManager));

            IAddendumService m_AddendumService;
            AddendumController controller;

            ConfigureTest(dbContext, out m_AddendumService, out controller);


            AddendumRequest addendumRequest = new AddendumRequest()
            {
                //Addendum = addendum,
                Id = 1,
                SOWId = "SOW1",
                AddendumId = 1,
                AddendumDate = DateTime.Now.AddDays(-1),
                AddendumNo = "UADN01",
                Note = "Updated Test Note",
                RecipientName = "Updated Test User",
                CurrentUser = "Anonymous",
                ProjectId = 1,
                RoleName = Roles.ProgramManager.GetEnumDescription()
            };
            //Act  
            var response = await controller.Update(addendumRequest);
            var okResult = response as OkObjectResult;
            var actualResult = okResult.Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult, true);
            
        }
        #endregion

        #region UpdateAsync_DepartmentHead
        [TestMethod]
        public async Task UpdateAsync_DepartmentHead()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(UpdateAsync_DepartmentHead));

            IAddendumService m_AddendumService;
            AddendumController controller;

            ConfigureTest(dbContext, out m_AddendumService, out controller);

            
            AddendumRequest addendumRequest = new AddendumRequest()
            {
                //AddendumId = addendum.AddendumId,
                Id = 1,
                SOWId = "SOW1",
                AddendumId = 1,
                AddendumDate = DateTime.Now.AddDays(-1),
                AddendumNo = "UADN01",
                Note = "Updated Test Note",
                RecipientName = "Updated Test User",
                CurrentUser = "Anonymous",
                ProjectId = 1,
                RoleName = Roles.DepartmentHead.GetEnumDescription()
            };
            //Act  
            var response = await controller.Update(addendumRequest);
            var okResult = response as OkObjectResult;
            var actualResult = okResult.Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult, true);

        }
        #endregion

        #region UpdateAsync_InvalidRole
        [TestMethod]
        public async Task UpdateAsync_InvalidRole()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(UpdateAsync_InvalidRole));

            IAddendumService m_AddendumService;
            AddendumController controller;

            ConfigureTest(dbContext, out m_AddendumService, out controller);

            var addendum = new Addendum()
            {
                Id = 1,
                SOWId = "SOW1",
                AddendumId = 1,
                AddendumDate = DateTime.Now.AddDays(-1),
                AddendumNo = "UADN01",
                Note = "Updated Test Note",
                RecipientName = "Updated Test User",
                CurrentUser = "Anonymous",
                ProjectId = 1,
            };

            AddendumRequest addendumRequest = new AddendumRequest()
            {
                //Addendum = addendum,
                RoleName = Roles.Delivery.GetEnumDescription()
            };
            //Act  
            var response = await controller.Update(addendumRequest);
            string errorMessage = ((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value.ToString();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("Your current role does not have permission to update Addendum.".Equals(errorMessage));
        }
        #endregion 

        #endregion

        #region ConfigureTest
        private void ConfigureTest(ProjectDBContext dbContext, out IAddendumService m_AddendumService, out AddendumController controller)
        {
            _logger = new Mock<ILogger<AddendumController>>();
            logger = new Mock<ILogger<AddendumService>>();
           

            m_AddendumService = new AddendumService(logger.Object, dbContext, null, null);

            controller = new AddendumController(m_AddendumService, _logger.Object);
        }
        #endregion
    }
}
