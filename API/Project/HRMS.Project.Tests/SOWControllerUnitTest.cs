using AutoMapper;
using HRMS.Common.Enums;
using HRMS.Common.Extensions;
using HRMS.Project.API;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Service;
using HRMS.Project.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace HRMS.Project.Tests
{
    [TestClass]
    public class SOWControllerUnitTest
    {
        private Mock<ILogger<SOWController>> _logger;
        private Mock<ILogger<SOWService>> logger;

        #region Get

        #region GetAllByProjectIdAsync
        [TestMethod]
        public async Task GetAllByProjectIdAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(GetAllByProjectIdAsync));

            ISOWService m_SOWService;
            SOWController controller;

            ConfigureTest(dbContext, out m_SOWService, out controller);

            // Act 
            var response = await controller.GetAllByProjectId(1);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion

        #region GetAllByProjectIdDepartmentHeadAsync
        [TestMethod]
        public async Task GetAllByProjectIdDepartmentHeadAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(GetAllByProjectIdDepartmentHeadAsync));

            ISOWService m_SOWService;
            SOWController controller;

            ConfigureTest(dbContext, out m_SOWService, out controller);

            // Act 
            var response = await controller.GetByIdAndProjectId(1, 1, Roles.DepartmentHead.GetEnumDescription());
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion

        #region GetAllByProjectIdProjectManagerAsync
        [TestMethod]
        public async Task GetAllByProjectIdProjectManagerAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(GetAllByProjectIdProjectManagerAsync));

            ISOWService m_SOWService;
            SOWController controller;

            ConfigureTest(dbContext, out m_SOWService, out controller);

            // Act 
            var response = await controller.GetByIdAndProjectId(1, 1, Roles.ProgramManager.GetEnumDescription());
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
        }
        #endregion

        #endregion

        #region Delete

        #region DeleteAsync
        [TestMethod]
        public async Task DeleteAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(DeleteAsync));

            ISOWService m_SOWService;
            SOWController controller;

            ConfigureTest(dbContext, out m_SOWService, out controller);

            // Act 
            var response = await controller.Delete(1);
            bool updated = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(updated, true);
        }
        #endregion

        #region DeleteAsync_BadRequest
        [TestMethod]
        public async Task DeleteAsync_BadRequest()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(DeleteAsync_BadRequest));

            ISOWService m_SOWService;
            SOWController controller;

            ConfigureTest(dbContext, out m_SOWService, out controller);

            // Act 
            var response = await controller.Delete(15);
          
            var okResult = response as OkObjectResult;
            var actualResult = okResult.Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult, false);
            
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

            ISOWService m_SOWService;
            SOWController controller;

            ConfigureTest(dbContext, out m_SOWService, out controller);


            SOWRequest sowRequest = new SOWRequest()
            {
                SOW = true,
                Id = 6,
                ProjectId = 4,
                SOWId = "SOW06",
                SOWFileName = "SOWFile.pdf",
                SOWSignedDate = DateTime.Now,
                RoleName = Roles.ProgramManager.GetEnumDescription()
            };
            //Act  
            var response = await controller.Create(sowRequest);
            
            var okResult = response as OkObjectResult;
            var actualResult = okResult.Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult, 1);
        }
        #endregion

        #region CreateAsync_DepartmentHead
        [TestMethod]
        public async Task CreateAsync_DepartmentHead()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(CreateAsync_DepartmentHead));

            ISOWService m_SOWService;
            SOWController controller;

            ConfigureTest(dbContext, out m_SOWService, out controller);

            SOWRequest sowRequest = new SOWRequest()
            {
                Id = 7,
                ProjectId = 4,
                SOWId = "SOW07",
                SOWFileName = "SOWFile.pdf",
                SOWSignedDate = DateTime.Now,
                SOW = true,
                RoleName = Roles.DepartmentHead.GetEnumDescription()
            };
            //Act  
            var response = await controller.Create(sowRequest);
            var okResult = response as OkObjectResult;
            var actualResult = okResult.Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult, 1);
            
        }
        #endregion

        #region CreateAsync_InvalidRole
        [TestMethod]
        public async Task CreateAsync_InvalidRole()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(CreateAsync_InvalidRole));

            ISOWService m_SOWService;
            SOWController controller;

            ConfigureTest(dbContext, out m_SOWService, out controller);

            var sow = new SOW()
            {
                Id = 8,
                ProjectId = 4,
                SOWId = "SOW08",
                SOWFileName = "SOWFile.pdf",
                SOWSignedDate = DateTime.Now,
            };

            SOWRequest sowRequest = new SOWRequest()
            {
                SOW = true,
                RoleName = Roles.Delivery.GetEnumDescription()
            };
            //Act  
            var response = await controller.Create(sowRequest);
            //string errorMessage = ((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value.ToString();
            var okResult = response as OkObjectResult;
            var actualResult = okResult.Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            //Assert.IsTrue("Your current role does not have permission to create SOW.".Equals(errorMessage));
            Assert.AreEqual(actualResult, 0);

            //var value = response;
            //var actualResult = response.GetType();
            ////Dispose DBContext
            //dbContext.Dispose();

            //// Assert
            //Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
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

            ISOWService m_SOWService;
            SOWController controller;

            ConfigureTest(dbContext, out m_SOWService, out controller);

            SOWRequest sowRequest = new SOWRequest()
            {
                SOW = true,
                Id = 1,
                ProjectId = 1,
                SOWId = "USOW01",
                SOWFileName = "SOWFile.pdf",
                SOWSignedDate = DateTime.Now,
                RoleName = Roles.ProgramManager.GetEnumDescription()
            };

            //Act  
            var response = await controller.Update(sowRequest);
            var okResult = response as OkObjectResult;
            var actualResult = okResult.Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult, 1);
        }
        #endregion

        #region UpdateAsync_DepartmentHead
        [TestMethod]
        public async Task UpdateAsync_DepartmentHead()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(UpdateAsync_DepartmentHead));

            ISOWService m_SOWService;
            SOWController controller;

            ConfigureTest(dbContext, out m_SOWService, out controller);

            SOWRequest sowRequest = new SOWRequest()
            {
                SOW = true,
                Id = 1,
                ProjectId = 1,
                SOWId = "USOW01",
                SOWFileName = "SOWFile.pdf",
                SOWSignedDate = DateTime.Now,
                RoleName = Roles.DepartmentHead.GetEnumDescription()
            };
            //Act  
            var response = await controller.Update(sowRequest);
            var okResult = response as OkObjectResult;
            var actualResult = okResult.Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult, 1);
        }
        #endregion

        #region UpdateAsync_InvalidRole
        [TestMethod]
        public async Task UpdateAsync_InvalidRole()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(UpdateAsync_InvalidRole));

            ISOWService m_SOWService;
            SOWController controller;

            ConfigureTest(dbContext, out m_SOWService, out controller);

            SOWRequest sowRequest = new SOWRequest()
            {
                SOW = true,
                Id = 1,
                ProjectId = 1,
                SOWId = "USOW01",
                SOWFileName = "SOWFile.pdf",
                SOWSignedDate = DateTime.Now,
                RoleName = Roles.Delivery.GetEnumDescription()
            };
            //Act  
            var response = await controller.Update(sowRequest);
            var okResult = response as OkObjectResult;
            var actualResult = okResult.Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult, -2);
            
        }
        #endregion 

        #region UpdateAsync_InvalidSOW
        [TestMethod]
        public async Task UpdateAsync_InvalidSOW()
        {
            // Arrange
            var dbContext = DbContextMocker.GetProjectDbContext(nameof(UpdateAsync_InvalidSOW));

            ISOWService m_SOWService;
            SOWController controller;

            ConfigureTest(dbContext, out m_SOWService, out controller);

            SOWRequest sowRequest = new SOWRequest()
            {
                SOW = true,
                Id = 15,
                ProjectId = 1,
                SOWId = "USOW01",
                SOWFileName = "SOWFile.pdf",
                SOWSignedDate = DateTime.Now,
                RoleName = Roles.Delivery.GetEnumDescription()
            };
            //Act  
            var response = await controller.Update(sowRequest);
            var okResult = response as OkObjectResult;
            var actualResult = okResult.Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult, -2);

        }
        #endregion 
        
        #endregion

        #region ConfigureTest
        private void ConfigureTest(ProjectDBContext dbContext, out ISOWService m_SOWService, out SOWController controller)
        {
            _logger = new Mock<ILogger<SOWController>>();
            logger = new Mock<ILogger<SOWService>>();

            m_SOWService = new SOWService(logger.Object, dbContext, null, null) ;

            controller = new SOWController(m_SOWService, _logger.Object);
        }
        #endregion
    }
}
