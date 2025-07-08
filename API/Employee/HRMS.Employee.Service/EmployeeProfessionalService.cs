using AutoMapper;
using HRMS.Common.Enums;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    public class EmployeeProfessionalService : IEmployeeProfessionalService
    {
        #region Global Variables
        private readonly ILogger<EmployeeProfessionalService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;
        private readonly IOrganizationService m_OrgService;
        #endregion

        #region EmployeeProfessionalService
        public EmployeeProfessionalService(ILogger<EmployeeProfessionalService> logger,
                    EmployeeDBContext employeeDBContext,
                    IOrganizationService organizationService)
        {
            m_Logger = logger;
            m_EmployeeContext = employeeDBContext;
            m_OrgService = organizationService;
            //Create mapper for certification
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AssociateMembership, AssociateMembership>();
                cfg.CreateMap<AssociateCertification, AssociateCertification>();
            });
            m_mapper = config.CreateMapper();

        }
        #endregion

        #region CreateCertificate
        /// <summary>
        /// inserts certification details of an employee 
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<AssociateCertification>> CreateCertificate(AssociateCertification certificateIn)
        {
            var response = new ServiceResponse<AssociateCertification>();
            int isCreated;
            try
            {
                m_Logger.LogInformation("EmployeeProfessionalService: Calling \"CreateCertificate\" method.");
                certificateIn.Id = 0;

                AssociateCertification certificate = new AssociateCertification();

                if (!certificateIn.IsActive.HasValue)
                    certificateIn.IsActive = true;
                //map fields
                m_mapper.Map<AssociateCertification, AssociateCertification>(certificateIn, certificate);

                //Add certificate to list
                m_EmployeeContext.AssociateCertifications.Add(certificate);

                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in EmployeeProfessionalService");
                isCreated = await m_EmployeeContext.SaveChangesAsync();

                if (isCreated > 0)
                {
                    response.IsSuccessful = true;
                    response.Item = certificate;
                    m_Logger.LogInformation("Certificate created successfully");
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No certificate created";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while creating certification";
                m_Logger.LogError("Error occurred in \"CreateCertificate\" of EmployeeProfessionalService " + ex.StackTrace);
            }
            return response;

        }

        #endregion

        #region CreateMembership
        /// <summary>
        /// inserts membership details of an employee 
        /// </summary>
        /// <param name="membership"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<AssociateMembership>> CreateMembership(AssociateMembership membershipIn)
        {
            var response = new ServiceResponse<AssociateMembership>();
            int isCreated;
            try
            {
                m_Logger.LogInformation("EmployeeProfessionalService: Calling \"CreateMembership\" method.");
                membershipIn.Id = 0;

                AssociateMembership membership = new AssociateMembership();

                if (!membershipIn.IsActive.HasValue)
                    membershipIn.IsActive = true;
                ////map fields
                m_mapper.Map<AssociateMembership, AssociateMembership>(membershipIn, membership);

                //Add membership to list
                m_EmployeeContext.AssociateMemberships.Add(membership);

                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in EmployeeProfessionalService");
                isCreated = await m_EmployeeContext.SaveChangesAsync();

                if (isCreated > 0)
                {
                    response.IsSuccessful = true;
                    response.Item = membership;
                    m_Logger.LogInformation("Membership created successfully");
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No membership created";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while creating membership";
                m_Logger.LogError("Error occurred in \"CreateMembership\" of EmployeeProfessionalService " + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete professional details 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="programType"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> Delete(int id, int programType)
        {
            m_Logger.LogInformation("EmployeeProfessionalService: Calling \"Delete\" method.");
            var response = new ServiceResponse<bool>();
            int isDeleted;
            try
            {
                //Perform deletion based on then given program type
                if ((int)ProgramType.Certification == programType)
                {
                    var certification = m_EmployeeContext.AssociateCertifications.SingleOrDefault(c => c.Id == id);
                    if (certification == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "No membership deleted";
                        return response;
                    }

                    //remove certification
                    m_EmployeeContext.AssociateCertifications.Remove(certification);
                }
                else if ((int)ProgramType.MemberShip == programType)
                {
                    var membership = m_EmployeeContext.AssociateMemberships.SingleOrDefault(c => c.Id == id);
                    if (membership == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "No membership deleted";
                        return response;
                    }

                    //remove membership
                    m_EmployeeContext.AssociateMemberships.Remove(membership);
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No membership deleted";
                    return response;
                }

                m_Logger.LogInformation("Calling SaveChanges method on DB Context in EmployeeProfessionalService");
                isDeleted = await m_EmployeeContext.SaveChangesAsync();

                if (isDeleted > 0)
                {
                    response.IsSuccessful = true;
                    response.Item = true;
                    m_Logger.LogInformation("Certificate deleted successfully");
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No certificate deleted";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while deleting record in EmployeeProfessionalService";
                m_Logger.LogError("Error occurred in \"Delete\" method of EmployeeProfessionalService" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetByEmployeeId
        /// <summary>
        /// Fetch professional details by employeeId  based on Skill and SkillGroup
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProfessionalDetails>> GetByEmployeeId(int employeeId)
        {
            List<ProfessionalDetails> certificationList;
            List<ProfessionalDetails> membershipsList;
            m_Logger.LogInformation("EmployeeProfessionalService: Calling \"GetByEmployeeId\" method.");

            var response = new ServiceListResponse<ProfessionalDetails>();
            try
            {
                //Fetch certifications of the given employee
                var certifications = await m_EmployeeContext.AssociateCertifications.
                                        Where(membership => membership.EmployeeId == employeeId).ToListAsync();

                if (certifications.Count > 0)
                {
                    //Fetch distinct SkillGroupId
                    List<int> certificationSkillGroupIds = certifications.Select(c => c.SkillGroupId).Distinct().ToList();

                    //Fetch the skills based on the above obtained distinct SkillGroupId 
                    var skills = await m_OrgService.GetSkillsBySkillGroupId(certificationSkillGroupIds);
                    if (!skills.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = skills.Message;
                        return response;
                    }

                    certificationList = (from certificate in certifications
                                         join skill in skills.Items on certificate.CertificationId equals skill.SkillId
                                         select new ProfessionalDetails
                                         {
                                             Id = certificate.Id,
                                             EmployeeId = certificate.EmployeeId,
                                             ValidFrom = certificate.ValidFrom,
                                             Institution = certificate.Institution,
                                             Specialization = certificate.Specialization,
                                             ValidUpto = certificate.ValidUpto,
                                             SkillGroupName = skill.SkillGroup.SkillGroupName,
                                             SkillGroupId = certificate.SkillGroupId,
                                             SkillName = skill.SkillName,
                                             CertificationId = certificate.CertificationId,
                                             ProgramType = (int)ProgramType.Certification,
                                             ProgramTitle = ""
                                         }).ToList();
                }
                else
                {
                    certificationList = new List<ProfessionalDetails>();
                }

                membershipsList = (from membership in m_EmployeeContext.AssociateMemberships
                                   where membership.EmployeeId == employeeId
                                   select new ProfessionalDetails
                                   {
                                       Id = membership.Id,
                                       EmployeeId = membership.EmployeeId,
                                       ProgramTitle = membership.ProgramTitle,
                                       ValidFrom = membership.ValidFrom,
                                       Institution = membership.Institution,
                                       Specialization = membership.Specialization,
                                       ValidUpto = membership.ValidUpto,
                                       SkillGroupName = "",
                                       SkillGroupId = 0,
                                       SkillName = "",
                                       CertificationId = 0,
                                       ProgramType = (int)ProgramType.MemberShip
                                   }).ToList();

                if (membershipsList.Count == 0)
                {
                    membershipsList = new List<ProfessionalDetails>();
                }

                //union on certifications and membership lists
                response.Items = certificationList.Union(membershipsList).ToList();
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while fetching records in EmployeeProfessionalService";
                m_Logger.LogError("Error occurred in \"GetByEmployeeId\" of EmployeeProfessionalService " + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region UpdateCertificate
        /// <summary>
        /// Update Certificate details
        /// </summary>
        /// <param name="certificateIn"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<AssociateCertification>> UpdateCertificate(AssociateCertification certificateIn)
        {
            m_Logger.LogInformation("EmployeeProfessionalService: Calling \"UpdateCertificate\" method.");
            var response = new ServiceResponse<AssociateCertification>();
            int isUpdated;
            try
            {
                //fetch certificate to update
                AssociateCertification certificate = m_EmployeeContext.AssociateCertifications.Find(certificateIn.Id);

                if (certificate == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "No certificate updated";
                    return response;
                }

                if (!certificateIn.IsActive.HasValue)
                    certificateIn.IsActive = certificate.IsActive;
                certificateIn.CreatedBy = certificate.CreatedBy;
                certificateIn.CreatedDate = certificate.CreatedDate;

                //map fields
                m_mapper.Map<AssociateCertification, AssociateCertification>(certificateIn, certificate);

                m_Logger.LogInformation("Calling SaveChanges method on DB Context in EmployeeProfessionalService");
                isUpdated = await m_EmployeeContext.SaveChangesAsync();

                if (isUpdated > 0)
                {
                    response.IsSuccessful = true;
                    response.Item = certificate;
                    m_Logger.LogInformation("Certificate updated successfully");
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No certificate updated";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while updating certification details";
                m_Logger.LogError("Error occurred in \"UpdateCertificate\" of EmployeeProfessionalService " + ex.StackTrace);
            }
            return response;

        }
        #endregion

        #region UpdateMembership
        /// <summary>
        /// Updates Membership details
        /// </summary>
        /// <param name="membershipIn"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<AssociateMembership>> UpdateMembership(AssociateMembership membershipIn)
        {
            m_Logger.LogInformation("EmployeeProfessionalService: Calling \"UpdateMembership\" method.");
            var response = new ServiceResponse<AssociateMembership>();
            int isUpdated;
            try
            {
                //fetch certificate to update
                AssociateMembership membership = m_EmployeeContext.AssociateMemberships.Find(membershipIn.Id);

                if (membership == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "No membership updated";
                    return response;
                }

                if (!membershipIn.IsActive.HasValue)
                    membershipIn.IsActive = membership.IsActive;
                    membershipIn.CreatedBy = membership.CreatedBy;
                    membershipIn.CreatedDate = membership.CreatedDate;
                //map fields
                m_mapper.Map<AssociateMembership, AssociateMembership>(membershipIn, membership);

                m_Logger.LogInformation("Calling SaveChanges method on DB Context in EmployeeProfessionalService");
                isUpdated = await m_EmployeeContext.SaveChangesAsync();

                if (isUpdated > 0)
                {
                    response.IsSuccessful = true;
                    response.Item = membership;
                    m_Logger.LogInformation("Membership updated successfully");
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No Membership updated";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while updating membership details";
                m_Logger.LogError("Error occurred in \"UpdateMembership\" of EmployeeProfessionalService" + ex.StackTrace);
            }
            return response;

        }
        #endregion

    }
}
