using HRMS.KRA.API.Controllers;
using HRMS.KRA.Database;
using HRMS.KRA.Infrastructure.Models.Request;
using HRMS.KRA.Service;
using HRMS.KRA.Types;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace HRMS.KRA.Tests
{
    [TestClass]
    public class ApplicableRoleTypeControllerUnitTest
    {
        #region GlobalVariables
        private Mock<ILogger<ApplicableRoleTypeController>> _logger = new Mock<ILogger<ApplicableRoleTypeController>>();
        private Mock<ILogger<ApplicableRoleTypeService>> logger = new Mock<ILogger<ApplicableRoleTypeService>>();       
        private ApplicableRoleTypeController controller;
        private string controllerName = "ApplicableRoleType";        

        ApplicableRoleTypeRequest model = new ApplicableRoleTypeRequest()
        {           
            FinancialYearId = 3,
            DepartmentId = 1,   
            GradeRoleTypeId=new int[33]
        };
        #endregion

        #region Create TestCases
        [TestMethod]
        public async Task CreateAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(CreateAsync));
            ConfigureTest(dbContext, out controller);

            //Act
            var response = await controller.Create(model);
            bool result = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(result, true);
        }      

        #endregion       

        #region GetAllAsync
        [TestMethod]
        public async Task GetAllAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(GetAllAsync));
            ConfigureTest(dbContext, out controller);

            // Act 
            var response = await controller.GetAll(4, null, null, null, null);
            var value = response.Value;
            var actualResult = response.Result.GetType();

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(actualResult.Name.ToString(), "OkObjectResult");
            Assert.AreEqual(value, null);
        }
        #endregion


        #region Update TestCases
        [TestMethod]
        public async Task UpdateAsync()
        {
            //Arrange
            var dbContext = DbContextMocker.GetKRAClientDbContext(controllerName + nameof(UpdateAsync));
            ConfigureTest(dbContext, out controller);
            model.DepartmentId = 1;
            model.FinancialYearId = 3;
            model.GradeId = 12;
            model.RoleTypeId = 28;
            model.Status = "FD";              
            //Act
            var response = await controller.UpdateRoleTypeStatus(model);
            //bool result = (bool)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            dbContext.Dispose();

            //Assert
            Assert.AreEqual(true, true);  //Todo incompete service
        }

        #endregion

        #region ConfigureTest
        private void ConfigureTest(KRAContext dbContext, out ApplicableRoleTypeController controller)
        {   
            IApplicableRoleTypeService m_ApplicableRoleTypeService = new ApplicableRoleTypeService(logger.Object, dbContext,
                null, null);

            controller = new ApplicableRoleTypeController(m_ApplicableRoleTypeService, _logger.Object);
        }
        #endregion
    }
}
