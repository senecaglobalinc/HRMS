using AutoMapper;
using HRMS.Common.Enums;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    /// <summary>
    /// Service class to get Employee Education details
    /// To Create EducationDetails
    /// To Update EducationDetails
    /// </summary>
    public class EmployeeEducationService : IEmployeeEducationService
    {
        #region Global Varibles

        private readonly ILogger<EmployeeEducationService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;

        #endregion

        #region Constructor
        public EmployeeEducationService(EmployeeDBContext employeeDBContext, ILogger<EmployeeEducationService> logger)
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EducationDetails, EducationDetails>();
            });
            m_mapper = config.CreateMapper();
        }

        #endregion

        #region Save
        /// <summary>
        /// Save method performs both creation and updation of education details
        /// </summary>
        /// <param name="educationDetails">educationDetails</param>
        /// <returns></returns>
        public async Task<ServiceResponse<EmployeeDetails>> Save(EmployeeDetails educationDetailsIn)
        {
            int isSaved;
            var response = new ServiceResponse<EmployeeDetails>();
            try
            {
                m_Logger.LogInformation("Calling Save method in EmployeeEducationService");

                // Check whether there are any duplicate education details for the employee.
                var duplicateEducationDetails = m_EmployeeContext.EducationDetails
                                                                 .Where(id => id.EmployeeId == educationDetailsIn.EmpId && id.IsActive == true)
                                                                 .ToList();

                //Make the duplicate education details inactive.
                duplicateEducationDetails.ForEach(d => d.IsActive = false);

                //Iterate through the Qualifications list
                foreach (EducationDetails ed in educationDetailsIn.Qualifications)
                {
                    EducationDetails educationDetails;

                    //If Id = 0 Create the education details.
                    if (ed.Id == 0)
                    {
                        //If educationDetails is null Create the education details.
                        educationDetails = new EducationDetails();
                        m_mapper.Map<EducationDetails, EducationDetails>(ed, educationDetails);
                        educationDetails.AcademicCompletedYear = Convert.ToDateTime(educationDetails.AcademicCompletedYear);
                        educationDetails.ProgramType = Enum.GetName(typeof(ProgramType), educationDetails.ProgramTypeId);
                        educationDetails.EmployeeId = educationDetailsIn.EmpId;
                        educationDetails.IsActive = true;

                        //add the educationDetails to list.
                        m_EmployeeContext.EducationDetails.Add(educationDetails);
                    }
                    else
                    {
                        educationDetails = m_EmployeeContext.EducationDetails.SingleOrDefault(e => e.Id == ed.Id);
                        EducationDetails educationDetail = educationDetailsIn.Qualifications.SingleOrDefault(e => e.Id == ed.Id);

                        // if educationDetails is not null perform updation.
                        if (educationDetails != null)
                        { 
                            m_mapper.Map<EducationDetails, EducationDetails>(educationDetail, educationDetails);
                            educationDetails.AcademicCompletedYear = Convert.ToDateTime(educationDetails.AcademicCompletedYear);
                            educationDetails.ProgramType = Enum.GetName(typeof(ProgramType), educationDetails.ProgramTypeId);
                            educationDetails.EmployeeId = educationDetailsIn.EmpId;
                            educationDetails.IsActive = true;
                        }
                    }                
                }

                isSaved = await m_EmployeeContext.SaveChangesAsync();

                if (isSaved > 0)
                {
                    response.Item = educationDetailsIn;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "EducationDetails not saved.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to save educationDetails.";

                m_Logger.LogError("Error Occured in Save() method in EmployeeEducationService"+ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetById
        /// <summary>
        /// Gets the EmployeeEducationDetails based on empId
        /// </summary>
        /// <param name="empId">empId</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<Entities.EducationDetails>> GetById(int empId)
        {
            var response = new ServiceListResponse<Entities.EducationDetails>();
            try
            {
                m_Logger.LogInformation("Calling GetById method in EmployeeEducationService");

                //check whether the employee exist.
                var isEmployeeExist =  m_EmployeeContext.EducationDetails
                                                        .Where(x => x.EmployeeId == empId)
                                                        .Select(x => new { x.EmployeeId }).ToList().Count;

                if (isEmployeeExist > 0)
                {
                    var getEducationDetails = await (from ed in m_EmployeeContext.EducationDetails
                                                     where ed.EmployeeId == empId && ed.IsActive == true
                                                     select new EducationDetails
                                                     {
                                                         Id = ed.Id,
                                                         EmployeeId = ed.EmployeeId,
                                                         ProgramTypeId = (ed.ProgramType != null) ? getEnumValue(ed.ProgramType) : 0,
                                                         ProgramType = ed.ProgramType,
                                                         EducationalQualification = ed.EducationalQualification,
                                                         AcademicCompletedYear = ed.AcademicCompletedYear,
                                                         Institution = ed.Institution,
                                                         Specialization = ed.Specialization,
                                                         Grade = ed.Grade,
                                                         Marks = ed.Marks
                                                     }).ToListAsync();

                    response.Items = getEducationDetails;
                    response.IsSuccessful = true;
                }
                else {

                    response.Items = Enumerable.Empty<EducationDetails>().ToList();
                    response.IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to fetch educationDetails by Id.";

                m_Logger.LogError("Error occured in GetById() method in EmployeeEducationService" + ex.Message);
            }
            return response;
        }
        #endregion

        //This method is to get Enumvalue by given enumString
        public static int getEnumValue(string enumString) 
        {
            return (int)(Enum.Parse(typeof(ProgramType), enumString,true));
        }
    }
}
