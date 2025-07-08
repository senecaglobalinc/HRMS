using HRMS.Admin.API.Controllers;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Service;
using HRMS.Admin.Types;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace HRMS.Admin.Tests
{
    [TestClass]
    public class DomainControllerUnitTest
    {
        #region Global Variables 
        private Mock<ILogger<DomainController>> _logger = new Mock<ILogger<DomainController>>();
        private Mock<ILogger<DomainService>> logger = new Mock<ILogger<DomainService>>();
        private DomainController controller;
        private string controllerName = "domain";
        Domain domain = new Domain()
        {
            DomainID = 10,
            DomainName = "HealthCare",
            CurrentUser = "Anonymous",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = null,
            SystemInfo = null,
            IsActive = true,
            CreatedBy = "Anonymous",
            ModifiedBy = null
        };
        #endregion

        #region Get TestCases
        [TestMethod]
        public async Task GetAllAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(GetAllAsync));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.GetAll();
            var expectedResult = response.Result.GetType();
            ICollection collection = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as ICollection;
            
            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(collection.Count, 3);
        }

        [TestMethod]
        public async Task GetAllAsync_Active()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(GetAllAsync_Active));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.GetAll(true);
            var expectedResult = response.Result.GetType();
            ICollection collection = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as ICollection;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(collection.Count, 3);
        }

        [TestMethod]
        public async Task GetAllAsync_InActive()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(GetAllAsync_InActive));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.GetAll(false);
            var expectedResult = response.Result.GetType();
            ICollection collection = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as ICollection;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(collection.Count, 1);
        }

        #endregion

        #region Create Testcases
        [TestMethod]
        public async Task CreateAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(CreateAsync));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.Create(domain);
            Domain data = (Domain)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;
            
            dbContext.Dispose();
            
            //Assert
            Assert.AreEqual(data.DomainName, "HealthCare");
        }

        #endregion

        #region Update TestCases

        [TestMethod]
        public async Task UpdateAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(UpdateAsync));
            ConfigureTest(dbContext, out controller);
            domain.DomainName = "HR";
            domain.DomainID = 1;
            //Act
            var response = await controller.Update(domain);
            Domain updatedData = (Domain)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;
            
            dbContext.Dispose();

            //Assert
            Assert.AreEqual(updatedData.DomainName, "HR");
        }

        [TestMethod]
        public async Task UpdateAsync_BadRequest_InvalidDomainID()
        {
            //Arrange
            var dbContext = DbContextMocker.GetAdminClientDbContext(controllerName + nameof(UpdateAsync_BadRequest_InvalidDomainID));
            ConfigureTest(dbContext, out controller);
            domain.DomainName = "HR";
            domain.DomainID = 100;
            //Act
            var response = await controller.Update(domain);
            var expectedResult = response.GetType();

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(expectedResult.Name.ToString(), "BadRequestObjectResult");
        }

        #endregion

        #region ConfigureTest
        private void ConfigureTest(AdminContext dbContext, out DomainController controller)
        {
            IDomainService m_DomainService = new DomainService(dbContext, logger.Object);
            controller = new DomainController(m_DomainService, _logger.Object);

        }
        #endregion
    }
}
