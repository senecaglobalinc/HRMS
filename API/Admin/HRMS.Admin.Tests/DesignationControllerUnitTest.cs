using HRMS.Admin.API.Controllers;
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
    public class DesignationControllerUnitTest
    {
        private Mock<ILogger<DesignationController>> _logger;
        private Mock<ILogger<DesignationService>> logger;

        [TestMethod]
        public async Task TestGetDesignationsAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(TestGetDesignationsAsync));

            IDesignationService DesignationService = new DesignationService(null, dbContext);
            _logger = new Mock<ILogger<DesignationController>>();

            var controller = new DesignationController(DesignationService, _logger.Object);

            // Act 
            var response = await controller.GetAll();
            var value = response.Value;
            var expectedResult = response.Result.GetType();

            dbContext.Dispose();

            // Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult");
        }

        [TestMethod]
        public async Task TestCreateDesignationAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(TestCreateDesignationAsync));

            _logger = new Mock<ILogger<DesignationController>>();
            logger = new Mock<ILogger<DesignationService>>();

            IDesignationService m_DesignationService = new DesignationService(logger.Object, dbContext);
            var controller = new DesignationController(m_DesignationService, _logger.Object);

            var designationData = new Designation()
            {
                DesignationId = 7,
                DesignationCode = "Test7",
                DesignationName = "Name7",
                CurrentUser = "Anonymous",
                GradeId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            };

            //Act  
            var response = await controller.Create(designationData);
            Designation data = (Designation)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(data.DesignationCode, "Test7");
        }

        [TestMethod]
        public async Task CreateAsync_Invalid()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(CreateAsync_Invalid));

            _logger = new Mock<ILogger<DesignationController>>();
            logger = new Mock<ILogger<DesignationService>>();

            IDesignationService m_DesignationService = new DesignationService(logger.Object, dbContext);
            var controller = new DesignationController(m_DesignationService, _logger.Object);

            //Input
            var designationData = new Designation()
            {
                DesignationId = 8,
                DesignationCode = "Test6",
                CurrentUser = "Anonymous",
                GradeId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            };
            controller.ModelState.AddModelError("DesignationName", "Required");
            //Act  
            var response = await controller.Create(designationData);
            var expectedResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "BadRequestObjectResult");
        }


        [TestMethod]
        public async Task UpdateAsync_BadRequest_DesignationID()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_DesignationID));

            _logger = new Mock<ILogger<DesignationController>>();
            logger = new Mock<ILogger<DesignationService>>();

            IDesignationService m_DesignationService = new DesignationService(logger.Object, dbContext);
            var controller = new DesignationController(m_DesignationService, _logger.Object);
            //Input
            var designationData = new Designation()
            {
                DesignationId = 10,
                DesignationCode = null,
                DesignationName = "Name7",
                GradeId = 1,
                IsActive = true
            };

            //Act  
            var response = await controller.Update(designationData);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Designation not found for update.".Equals(actualResponse));
        }
      

        [TestMethod]
        public async Task TestUpdateDesignationAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(TestUpdateDesignationAsync));
            _logger = new Mock<ILogger<DesignationController>>();
            logger = new Mock<ILogger<DesignationService>>();
            IDesignationService m_DesignationService = new DesignationService(logger.Object, dbContext);
            var controller = new DesignationController(m_DesignationService, _logger.Object);

            //Act  
            var response = dbContext.Designations.Find(6);
           
            var designationData = new Designation();
            designationData.DesignationId = response.DesignationId;
            designationData.DesignationCode = "Designation code updated";
            designationData.DesignationName = "Test 100";
            designationData.GradeId = 1;
            var updateResponse = await controller.Update(designationData);
            Designation updatedData = (Designation)((Microsoft.AspNetCore.Mvc.ObjectResult)updateResponse).Value;
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(updatedData.DesignationName, "Test 100");
        }

       
        [TestMethod]
        public async Task UpdateAsync_BadRequest_CodeAlreadyExists()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(UpdateAsync_BadRequest_CodeAlreadyExists));
            _logger = new Mock<ILogger<DesignationController>>();
            logger = new Mock<ILogger<DesignationService>>();
            IDesignationService m_DesignationService = new DesignationService(logger.Object, dbContext);
            var controller = new DesignationController(m_DesignationService, _logger.Object);

            var designationData = new Designation()
            {
                DesignationId = 3,
                DesignationCode = "Test2",
                DesignationName = "Name2 updated",
                GradeId = 1,
                IsActive = true
            };

            //Act  
            var response = await controller.Update(designationData);
            string actualResponse = Convert.ToString(((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value);
            var actualResult = response.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("BadRequestObjectResult".Equals(actualResult.Name.ToString()) &&
                          "Designation code already exists".Equals(actualResponse));
        }
      
    }
}
