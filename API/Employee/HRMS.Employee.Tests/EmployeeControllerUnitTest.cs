using HRMS.Common.Redis;
using HRMS.Employee.API.Controllers;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Service;
using HRMS.Employee.Types;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Employee.Tests
{
    [TestClass]
    public class EmployeeControllerUnitTest
    {
        private Mock<ILogger<EmployeeController>> _logger;
        private Mock<ILogger<EmployeeService>> logger;

        #region GetAllAsync
        [TestMethod]
        public async Task GetAllAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(nameof(GetAllAsync));
            IEmployeeService m_EmployeeService;
            EmployeeController m_Controller;

            ConfigureTest(dbContext, out m_EmployeeService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetAll(true);
            var value = response.Value;
            var actualResult = response.Result.GetType();
            var employees = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<Entities.Employee>;
            int ActiveRecords = employees.FindAll(cm => cm.IsActive == true).Count;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue(ActiveRecords == 2);
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(value, null);
        }
        #endregion

        #region GetEmpTypesAsync
        [TestMethod]
        public async Task GetEmpTypesAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(nameof(GetEmpTypesAsync));
            IEmployeeService m_EmployeeService;
            EmployeeController m_Controller;

            ConfigureTest(dbContext, out m_EmployeeService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetEmpTypes();
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(value, null);
        }
        #endregion

        #region ConfigureTest
        private void ConfigureTest(EmployeeDBContext dbContext, out IEmployeeService m_EmployeeService,
            out EmployeeController m_Controller)
        {
            _logger = new Mock<ILogger<EmployeeController>>();
            logger = new Mock<ILogger<EmployeeService>>();
            m_EmployeeService = new EmployeeService(dbContext, logger.Object, null, null, null, null);
            m_Controller = new EmployeeController(m_EmployeeService, _logger.Object);
        }
        #endregion
    }
}
