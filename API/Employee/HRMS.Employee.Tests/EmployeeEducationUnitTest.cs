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
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Tests
{
    [TestClass]
    public class EmployeeEducationUnitTest
    {
        private Mock<ILogger<EmployeeEducationController>> _logger;
        private Mock<ILogger<EmployeeEducationService>> logger;

        #region GetEducationDetailsbyId Test Cases

        #region GetEducationDetailsbyIdAsync
        [TestMethod]
        public async Task GetEducationDetailsbyIdAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(nameof(GetEducationDetailsbyIdAsync));
            IEmployeeEducationService m_EducationService;
            EmployeeEducationController m_Controller;

            ConfigureTest(dbContext, out m_EducationService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetById(1);
            var educationDetails = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<EducationDetails>;
            dbContext.Dispose();

            // Assert
            Assert.AreEqual(educationDetails[0].EducationalQualification, "Graduation");
        }
        #endregion

        #region GetEducationDetailsByIdReturnEmpty
        [TestMethod]
        public async Task GetEducationDetailsByIdReturnEmpty()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(nameof(GetEducationDetailsByIdReturnEmpty));
            IEmployeeEducationService m_EducationService;
            EmployeeEducationController m_Controller;

            ConfigureTest(dbContext, out m_EducationService,
                out m_Controller);

            // Act 
            var response = await m_Controller.GetById(3);
            var educationDetails = ((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value as List<EducationDetails>;
            dbContext.Dispose();

            // Assert
            Assert.IsTrue(educationDetails.Count == 0);
        }
        #endregion

        #endregion

        #region Save Test Cases

        #region CreateAsync
        [TestMethod]
        public async Task CreateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(nameof(CreateAsync));
            IEmployeeEducationService m_EducationService;
            EmployeeEducationController m_Controller;

            ConfigureTest(dbContext, out m_EducationService,
                out m_Controller);

            //Input
            List<EducationDetails> educationDetailsList = new List<EducationDetails>();
            educationDetailsList.Add(new EducationDetails
            {
                Id = 3,
                EmployeeId = 3,
                EducationalQualification = "MBA",
                AcademicCompletedYear = null,
                Institution = "VIT",
                Specialization = "CSE",
                ProgramType = "FullTime",
                Grade = "Percentage",
                Marks = "76",
                AcademicYearFrom = null,
                AcademicYearTo = null,
                ProgramTypeId = 0,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });
            var educationDetails = new EmployeeDetails()
            {
                EmpId = 3,
                Qualifications = educationDetailsList
            };

            //Act  
            var response = await m_Controller.Save(educationDetails);
            EducationDetails data = (EducationDetails)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("MBA".Equals(data.EducationalQualification) && "VIT".Equals(data.Institution));
        }
        #endregion

        #region UpdateAsync
        [TestMethod]
        public async Task UpdateAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.GetEmployeeClientDbContext(nameof(UpdateAsync));
            IEmployeeEducationService m_EducationService;
            EmployeeEducationController m_Controller;

            ConfigureTest(dbContext, out m_EducationService,
                out m_Controller);

            //Input
            List<EducationDetails> educationDetailsList = new List<EducationDetails>();
            educationDetailsList.Add(new EducationDetails
            {
                Id = 2,
                EmployeeId = 2,
                EducationalQualification = "MBA",
                AcademicCompletedYear = null,
                Institution = "SRM",
                Specialization = "CSE",
                ProgramType = "FullTime",
                Grade = "Percentage",
                Marks = "80",
                AcademicYearFrom = null,
                AcademicYearTo = null,
                ProgramTypeId = 0,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });
            var educationDetails = new EmployeeDetails()
            {
                EmpId = 2,
                Qualifications = educationDetailsList
            };

            //Act  
            var response = await m_Controller.Save(educationDetails);
            EducationDetails data = (EducationDetails)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value;

            //Dispose DBContext
            dbContext.Dispose();

            // Assert
            Assert.IsTrue("MBA".Equals(data.EducationalQualification) && "SRM".Equals(data.Institution));
        }
        #endregion
        #endregion

        #region ConfigureTest
        private void ConfigureTest(EmployeeDBContext dbContext, out IEmployeeEducationService m_EducationService,
            out EmployeeEducationController m_Controller)
        {
            _logger = new Mock<ILogger<EmployeeEducationController>>();
            logger = new Mock<ILogger<EmployeeEducationService>>();
            m_EducationService = new EmployeeEducationService(dbContext, logger.Object);
            m_Controller = new EmployeeEducationController(m_EducationService, _logger.Object);
        }
        #endregion
    }
}
