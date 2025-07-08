using HRMS.Employee.API.Controllers;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Service;
using HRMS.Employee.Types;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Tests
{
    [TestClass]
    public class EmployeeEmploymentUnitTest
    {
        private Mock<ILogger<EmployeeEmploymentController>> _logger;
        private Mock<ILogger<EmployeeEmploymentService>> logger;

        #region GetEmploymentDetailsbyId Test Cases

        #region GetEmploymentDetailsbyIdAsync
        [TestMethod]
        public async Task GetEmploymentDetailsbyIdAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(nameof(GetEmploymentDetailsbyIdAsync));
            IEmployeeEmploymentService m_EmployeeEmploymentService;
            EmployeeEmploymentController m_Controller;

            ConfigureTest(dbContext, out m_EmployeeEmploymentService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetPrevEmploymentDetailsById(1);
            var employmentDetails = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<PreviousEmploymentDetails>;
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(employmentDetails[0].Designation, "Database Administrator");
        }
        #endregion

        #region GetEmploymentDetailsByIdReturnEmpty
        [TestMethod]
        public async Task GetEmploymentDetailsByIdReturnEmpty()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(nameof(GetEmploymentDetailsByIdReturnEmpty));
            IEmployeeEmploymentService m_EmployeeEmploymentService;
            EmployeeEmploymentController m_Controller;

            ConfigureTest(dbContext, out m_EmployeeEmploymentService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetPrevEmploymentDetailsById(3);
            var employmentDetails = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<PreviousEmploymentDetails>;
            dbContext.Dispose();

            // Assert
            Assert.IsTrue(employmentDetails.Count == 0);
        }
        #endregion

        #region GetProfessionalReferencesbyIdAsync
        [TestMethod]
        public async Task GetProfessionalReferencesbyIdAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(nameof(GetProfessionalReferencesbyIdAsync));
            IEmployeeEmploymentService m_EmployeeEmploymentService;
            EmployeeEmploymentController m_Controller;

            ConfigureTest(dbContext, out m_EmployeeEmploymentService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetProfReferencesById(1);
            var professionalReferences = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<ProfessionalReferences>;
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(professionalReferences[0].Designation, "Sr. Manager");
        }
        #endregion

        #region GetProfessionalReferencesByIdReturnEmpty
        [TestMethod]
        public async Task GetProfessionalReferencesByIdReturnEmpty()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(nameof(GetProfessionalReferencesByIdReturnEmpty));
            IEmployeeEmploymentService m_EmployeeEmploymentService;
            EmployeeEmploymentController m_Controller;

            ConfigureTest(dbContext, out m_EmployeeEmploymentService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetProfReferencesById(3);
            var professionalRefs = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<ProfessionalReferences>;
            dbContext.Dispose();

            // Assert
            Assert.IsTrue(professionalRefs.Count == 0);
        }
        #endregion

        #endregion

        #region Save Test Cases

        #region CreateAsync
        [TestMethod]
        public async Task CreateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(nameof(GetProfessionalReferencesByIdReturnEmpty));
            IEmployeeEmploymentService m_EmployeeEmploymentService;
            EmployeeEmploymentController m_Controller;

            ConfigureTest(dbContext, out m_EmployeeEmploymentService,
                out m_Controller);

            //Input
            List<PreviousEmploymentDetails> preEmploymentData = new List<PreviousEmploymentDetails>();
            preEmploymentData.Add(new PreviousEmploymentDetails
            {
                Id = 4,
                EmployeeId = 4,
                Name = "Name3",
                Address = "Hyderabad",
                Designation = "Lead",
                ServiceFrom = null,
                ServiceTo = null,
                LeavingReason = "Reason3",
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = null,
                CreatedBy = null,
                ModifiedBy = null
            });

            List<ProfessionalReferences> profReferencesData = new List<ProfessionalReferences>();
            profReferencesData.Add(new ProfessionalReferences
            {
                Id = 4,
                EmployeeId = 4,
                Name = "Name3",
                Designation = "Manager",
                CompanyName = "DST World Wide",
                CompanyAddress = "Hyderabad",
                OfficeEmailAddress = "xxx@gmail.com",
                MobileNo = "9550326770",
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            var employmentDetails = new EmployeeDetails()
            {
                EmpId = 4,
                PrevEmploymentDetails = preEmploymentData,
                ProfReferences = profReferencesData
            };

            //Act  
            var response = await m_Controller.Save(employmentDetails);
            EmployeeDetails data = (EmployeeDetails)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue(4.Equals(data.EmpId));
        }
        #endregion

        #region UpdateAsync
        [TestMethod]
        public async Task UpdateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(nameof(GetProfessionalReferencesByIdReturnEmpty));
            IEmployeeEmploymentService m_EmployeeEmploymentService;
            EmployeeEmploymentController m_Controller;

            ConfigureTest(dbContext, out m_EmployeeEmploymentService,
                out m_Controller);

            //Input
            List<PreviousEmploymentDetails> preEmploymentData = new List<PreviousEmploymentDetails>();
            preEmploymentData.Add(new PreviousEmploymentDetails
            {
                Id = 2,
                EmployeeId = 2,
                Name = "Name2 updated",
                Address = "Hyderabad",
                Designation = "Lead",
                ServiceFrom = null,
                ServiceTo = null,
                LeavingReason = "Reason2",
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = null,
                CreatedBy = null,
                ModifiedBy = null
            });

            List<ProfessionalReferences> profReferencesData = new List<ProfessionalReferences>();
            profReferencesData.Add(new ProfessionalReferences
            {
                Id = 2,
                EmployeeId = 2,
                Name = "Name2 Updated",
                Designation = "Manager",
                CompanyName = "DST World Wide",
                CompanyAddress = "Hyderabad",
                OfficeEmailAddress = "xxx@gmail.com",
                MobileNo = "9550326770",
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            var employmentDetails = new EmployeeDetails()
            {
                EmpId = 3,
                PrevEmploymentDetails = preEmploymentData,
                ProfReferences = profReferencesData
            };


            //Act  
            var response = await m_Controller.Save(employmentDetails);
            EmployeeDetails data = (EmployeeDetails)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue(3.Equals(data.EmpId));
        }
        #endregion
        #endregion

        #region ConfigureTest
        private void ConfigureTest(EmployeeDBContext dbContext, out IEmployeeEmploymentService m_EmploymentService,
            out EmployeeEmploymentController m_Controller)
        {
            _logger = new Mock<ILogger<EmployeeEmploymentController>>();
            logger = new Mock<ILogger<EmployeeEmploymentService>>();
            m_EmploymentService = new EmployeeEmploymentService(dbContext, logger.Object);
            m_Controller = new EmployeeEmploymentController(m_EmploymentService, _logger.Object);
        }
        #endregion
    }
}
