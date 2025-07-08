using AutoMapper;
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
    /// Service class to get Employee previous Employment details
    /// to get Employee Professional References
    /// To Create Previous Employment details and Professional References
    /// To Update  Previous Employment details and Professional References
    /// </summary>
    public class EmployeeEmploymentService : IEmployeeEmploymentService
    {
        #region Global Varibles

        private readonly ILogger<EmployeeEmploymentService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;

        #endregion

        #region Constructor
        public EmployeeEmploymentService(EmployeeDBContext employeeDBContext, ILogger<EmployeeEmploymentService> logger)
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PreviousEmploymentDetails, PreviousEmploymentDetails>();
                cfg.CreateMap<ProfessionalReferences, ProfessionalReferences>();
            });
            m_mapper = config.CreateMapper();
        }

        #endregion

        #region GetPrevEmploymentDetailsById
        /// <summary>
        /// Gets the Previous Employment details based on empId
        /// </summary>
        /// <param name="empId">empId</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<PreviousEmploymentDetails>> GetPrevEmploymentDetailsById(int empId)
        {
            var response = new ServiceListResponse<PreviousEmploymentDetails>();
            try
            {
                m_Logger.LogInformation("Calling GetPrevEmploymentDetailsById method in EmployeeEmploymentService");

                //check whether the employee exist.
                var isEmployeeExist = m_EmployeeContext.PreviousEmploymentDetails
                                          .Where(x => x.EmployeeId == empId)
                                           .Select(x => new { x.EmployeeId }).ToList().Count;
                if (isEmployeeExist > 0)
                {
                    var getEmploymentDetails = await (from em in m_EmployeeContext.PreviousEmploymentDetails
                                                     where em.EmployeeId == empId && em.IsActive == true
                                                     select new PreviousEmploymentDetails
                                                     {
                                                         Id = em.Id,
                                                         Name = em.Name,
                                                         Address = em.Address,
                                                         Designation = em.Designation,
                                                         ServiceFrom = em.ServiceFrom,
                                                         ServiceTo = em.ServiceTo,
                                                         LeavingReason = em.LeavingReason                                                       
                                                     }).ToListAsync();

                    response.Items = getEmploymentDetails;
                    response.IsSuccessful = true;
                }
                else
                {

                    response.Items = Enumerable.Empty<PreviousEmploymentDetails>().ToList();
                    response.IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to fetch previous employmentDetails by Id.";

                m_Logger.LogError("Error occured in GetPrevEmploymentDetailsById() method in EmployeeEmploymentService" + ex.Message);
            }
            return response;
        }
        #endregion

        #region GetProfReferencesById
        /// <summary>
        /// Gets the Professional References based on empId
        /// </summary>
        /// <param name="empId">empId</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProfessionalReferences>> GetProfReferencesById(int empId)
        {
            var response = new ServiceListResponse<ProfessionalReferences>();
            try
            {
                m_Logger.LogInformation("Calling GetProfReferencesById method in EmployeeEmploymentService");

                //check whether the employee exist.
                var isEmployeeExist = m_EmployeeContext.ProfessionalReferences
                                          .Where(x => x.EmployeeId == empId)
                                           .Select(x => new { x.EmployeeId }).ToList().Count;
                if (isEmployeeExist > 0)
                {
                    var getEmploymentDetails = await (from pr in m_EmployeeContext.ProfessionalReferences
                                                      where pr.EmployeeId == empId && pr.IsActive == true
                                                      select new ProfessionalReferences
                                                      {
                                                          Id = pr.Id,
                                                          Name = pr.Name,
                                                          Designation = pr.Designation,
                                                          CompanyName = pr.CompanyName,
                                                          CompanyAddress = pr.CompanyAddress,
                                                          OfficeEmailAddress = pr.OfficeEmailAddress,
                                                          MobileNo = pr.MobileNo
                                                      }).ToListAsync();

                    response.Items = getEmploymentDetails;
                    response.IsSuccessful = true;
                }
                else
                {

                    response.Items = Enumerable.Empty<ProfessionalReferences>().ToList();
                    response.IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to fetch professional references by Id.";

                m_Logger.LogError("Error occured in GetProfReferencesById() method in EmployeeEmploymentService" + ex.Message);
            }
            return response;
        }
        #endregion

        #region Save
        /// <summary>
        /// Save method performs both creation and updation of employment details
        /// </summary>
        /// <param name="employmentDetailsIn">employmentDetailsIn</param>
        /// <returns></returns>
        public async Task<ServiceResponse<EmployeeDetails>> Save(EmployeeDetails employmentDetailsIn)
        {
            int isSaved;
            var response = new ServiceResponse<EmployeeDetails>();
            try
            {
                m_Logger.LogInformation("Calling Save method in EmployeeEmploymentService");

                // Check whether there are any duplicate employment details for the employee.
                var duplicateEmploymentDetails = m_EmployeeContext.PreviousEmploymentDetails.Where(id => id.EmployeeId == employmentDetailsIn.EmpId
                                                                && id.IsActive == true).ToList();

                //Make the duplicate employment details inactive.
                duplicateEmploymentDetails.ForEach(d => d.IsActive = false);

                //Iterate through the PreviousEmploymentDetails list
                foreach (PreviousEmploymentDetails pr in employmentDetailsIn.PrevEmploymentDetails)
                {
                    if (!string.IsNullOrEmpty(pr.Name))
                    {
                        PreviousEmploymentDetails preEmploymentDetails = m_EmployeeContext.PreviousEmploymentDetails.SingleOrDefault(p => p.Id == pr.Id);
                        PreviousEmploymentDetails employmentDetail = employmentDetailsIn.PrevEmploymentDetails.SingleOrDefault(p => p.Id == pr.Id);

                        // if preEmploymentDetails is not null perform updation.
                        if (preEmploymentDetails != null)
                        {
                            m_mapper.Map<PreviousEmploymentDetails, PreviousEmploymentDetails>(employmentDetail, preEmploymentDetails);

                            preEmploymentDetails.ServiceFrom = Convert.ToDateTime(pr.ServiceFrom);
                            preEmploymentDetails.ServiceTo = Convert.ToDateTime(pr.ServiceTo);
                            preEmploymentDetails.EmployeeId = employmentDetailsIn.EmpId;
                            preEmploymentDetails.IsActive = true;            
                        }
                        else
                        {
                            //If preEmploymentDetails is null, Create the employment details.
                            preEmploymentDetails = new PreviousEmploymentDetails();
                            m_mapper.Map<PreviousEmploymentDetails, PreviousEmploymentDetails>(employmentDetail, preEmploymentDetails);
                            preEmploymentDetails.ServiceFrom = Convert.ToDateTime(pr.ServiceFrom);
                            preEmploymentDetails.ServiceTo = Convert.ToDateTime(pr.ServiceTo);
                            preEmploymentDetails.EmployeeId = employmentDetailsIn.EmpId;
                            preEmploymentDetails.IsActive = true;

                            //add the preEmploymentDetails to list.
                            m_EmployeeContext.PreviousEmploymentDetails.Add(preEmploymentDetails);                                 
                        }
                    }
                }

                // Check whether there are any duplicate employment details for the employee.
                var duplicateprofReference = m_EmployeeContext.ProfessionalReferences.Where(p => p.EmployeeId == employmentDetailsIn.EmpId).ToList();

                //Make the duplicate employment details inactive.
                duplicateprofReference.ForEach(d => d.IsActive = false);             

                //Iterate through the ProfessionalReferences list
                foreach (ProfessionalReferences pr in employmentDetailsIn.ProfReferences)
                {
                    if (!string.IsNullOrEmpty(pr.Name))
                    {
                        ProfessionalReferences profReferenceDetails = m_EmployeeContext.ProfessionalReferences.SingleOrDefault(p => p.Id == pr.Id);
                        ProfessionalReferences profReferenceDetail = employmentDetailsIn.ProfReferences.SingleOrDefault(p => p.Id == pr.Id);

                        // if profReferenceDetails is not null perform updation.
                        if (profReferenceDetails != null)
                        {
                            m_mapper.Map<ProfessionalReferences, ProfessionalReferences>(profReferenceDetail, profReferenceDetails);
                            profReferenceDetails.EmployeeId = employmentDetailsIn.EmpId;
                            profReferenceDetails.IsActive = true;
                        }
                        else
                        {
                            //If profReferenceDetails is null, Create the Professional Reference details.
                            profReferenceDetails = new ProfessionalReferences();
                            m_mapper.Map<ProfessionalReferences, ProfessionalReferences>(profReferenceDetail, profReferenceDetails);
                            profReferenceDetails.EmployeeId = employmentDetailsIn.EmpId;
                            profReferenceDetails.IsActive = true;

                            //add the preEmploymentDetails to list.
                            m_EmployeeContext.ProfessionalReferences.Add(profReferenceDetails);                          
                        }                       
                    }
                }

                isSaved = await m_EmployeeContext.SaveChangesAsync();

                if (isSaved > 0)
                {
                    response.Item = employmentDetailsIn;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Employment details not saved.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to save educationDetails.";

                m_Logger.LogError("Error Occured in Save() method in EmployeeEducationService" + ex.StackTrace);
            }
            return response;
        }
        #endregion
    }
}
