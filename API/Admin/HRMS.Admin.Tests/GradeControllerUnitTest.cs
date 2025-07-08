using HRMS.Admin.API.Controllers;
using HRMS.Admin.Service;
using HRMS.Admin.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HRMS.Admin.Entities;

namespace HRMS.Admin.Tests
{
    [TestClass]
    public class GradeControllerUnitTest
    {
        private  Mock<ILogger<GradeController>> _logger;
        private Mock<ILogger<GradeService>> logger;

        [TestMethod]
        public async Task TestGetGradesAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(TestGetGradesAsync));
            IGradeService m_GradeService = new GradeService(null, dbContext);
            _logger = new Mock<ILogger<GradeController>>();
            var controller = new GradeController(m_GradeService, _logger.Object);

            // Act 
            var response = await controller.GetAll();
            var value = response.Value;
            var expectedResult = response.Result.GetType();
            dbContext.Dispose();
            
            // Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult" );
        }

        [TestMethod]
        public async Task TestGetGradeByIdAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(TestGetGradeByIdAsync));
            IGradeService m_GradeService = new GradeService(null, dbContext);
            _logger = new Mock<ILogger<GradeController>>();
            var controller = new GradeController(m_GradeService, _logger.Object);

            // Act 
            var response = await controller.GetById(5);
            Grade data = (Grade)((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value;
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(data.GradeName, "Test5");
        }

        [TestMethod]
        public async Task TestGetGradeByIdReturnNotFoundResult()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(TestGetGradeByIdReturnNotFoundResult));
            IGradeService m_GradeService = new GradeService(null, dbContext);
            _logger = new Mock<ILogger<GradeController>>();
            var controller = new GradeController(m_GradeService, _logger.Object);
           
            //Act  
            var response = await controller.GetById(20);
            var expectedResult = response.Result.GetType();

            //Assert  
            Assert.AreEqual(expectedResult.Name.ToString(), "NotFoundResult");
        }
     

        [TestMethod]
        public async Task CreateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(CreateAsync));
            _logger = new Mock<ILogger<GradeController>>();
            logger = new Mock<ILogger<GradeService>>();
            IGradeService m_GradeService = new GradeService(logger.Object, dbContext);
            var controller = new GradeController(m_GradeService, _logger.Object);
            //Input
            var grade = new Grade()
            {
                GradeId = 11,
                GradeCode = "code2",  
                GradeName = "Test12",              
                IsActive = true
            };

            //Act  
            var response = await controller.Create(grade);
            Grade data = (Grade)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("Test12".Equals(data.GradeName));
        }

        [TestMethod]
        public async Task Test_Add_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(Test_Add_InvalidData_Return_BadRequest));
            _logger = new Mock<ILogger<GradeController>>();
            logger = new Mock<ILogger<GradeService>>();
            IGradeService m_GradeService = new GradeService(logger.Object, dbContext);
            var controller = new GradeController(m_GradeService, _logger.Object);
            var gradeData = new Grade()
            {
                GradeId = 13,
                GradeCode = null,  //GradeCode is Not NULL Field
                GradeName = "Test12",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            };

            //Act   
            var response = await controller.Create(gradeData);
            var expectedResult = response.GetType();

            //Assert  
            Assert.AreEqual(expectedResult.Name.ToString(), "BadRequestResult");
        }

        [TestMethod]
        public async Task TestUpdateGradeAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(TestUpdateGradeAsync));
            _logger = new Mock<ILogger<GradeController>>();
            logger = new Mock<ILogger<GradeService>>();
            IGradeService m_GradeService = new GradeService(logger.Object, dbContext);
            var controller = new GradeController(m_GradeService, _logger.Object);

            //Act  
            var response = await controller.GetById(5);
            Grade data = (Grade)((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value;
            var gradeData = new Grade();
            gradeData.GradeId = data.GradeId;
            gradeData.GradeCode = "Test5 Updated";
            gradeData.GradeName = data.GradeName;
            var updateResponse = await controller.Update(gradeData);
            Grade updatedData = (Grade)((Microsoft.AspNetCore.Mvc.ObjectResult)updateResponse).Value;
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(updatedData.GradeCode, "Test5 Updated");
        }

        [TestMethod]
        public async Task Test_Update_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var dbContext = DbContextMocker.GetAdminClientDbContext(nameof(Test_Update_InvalidData_Return_BadRequest));
            _logger = new Mock<ILogger<GradeController>>();
            logger = new Mock<ILogger<GradeService>>();
            IGradeService m_GradeService = new GradeService(logger.Object, dbContext);
            var controller = new GradeController(m_GradeService, _logger.Object);
            controller.ModelState.AddModelError("GradeCode", "Required");
            //Act  
            var gradeData = new Grade()
            {
                GradeId = 7,
                GradeName = "Test12",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            };
            var updatedData = await controller.Update(gradeData);
            var expectedResult = updatedData.GetType();

            //Assert  
            Assert.AreEqual(expectedResult.Name.ToString(), "BadRequestObjectResult");
        }

    }
}
