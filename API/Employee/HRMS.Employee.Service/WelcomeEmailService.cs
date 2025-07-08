using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net.Http;
using AutoMapper;
using System.Linq;
using HRMS.Employee.Types;
//using HRMS.Common.Redis;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Common;
using HRMS.Common.Enums;
using HRMS.Employee.Types.External;
using HRMS.Employee.Infrastructure.Models.Response;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using HRMS.Employee.Infrastructure.Models.Request;
using System.Net.Mime;
using HRMS.Employee.Infrastructure.Models.Domain;

namespace HRMS.Employee.Service
{
    /// <summary>
    /// Service class to get the Employee details
    /// </summary>
    public class WelcomeEmailService : IWelcomeEmailService
    {
        #region Global Varibles

        private readonly ILogger<EmployeeService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;
        //private readonly ICacheService m_CacheService;
        private readonly IOrganizationService m_OrgService;
        private readonly IProjectService m_ProjectService;
        private readonly IConfiguration m_configuration;
        private readonly WelcomeEmailConfigurations m_WelcomeEmailConfigurations;

        #endregion

        #region Constructor
        public WelcomeEmailService(EmployeeDBContext employeeDBContext,
                                ILogger<EmployeeService> logger,
                                IHttpClientFactory clientFactory,
                                IOptions<APIEndPoints> apiEndPoints,
                                //ICacheService cacheService,
                                IProjectService projectService,
                                IOrganizationService orgService,
                                IConfiguration configuration,
                                IOptions<WelcomeEmailConfigurations> welcomeEmailConfigurations)
        {
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee.Entities.Employee, Employee.Entities.Employee>();
            });
            m_mapper = config.CreateMapper();
            //m_CacheService = cacheService;
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints?.Value;
            m_ProjectService = projectService;
            m_OrgService = orgService;
            m_configuration = configuration;
            m_WelcomeEmailConfigurations = welcomeEmailConfigurations.Value;
        }

        #endregion

        #region GetWelcomeEmployeeInfo
        /// <summary>
        /// Gets the Employee info based on departments and approved status.
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<WelcomeEmailEmpRequest>> GetWelcomeEmployeeInfo()
        {

            var response = new ServiceListResponse<WelcomeEmailEmpRequest>();
            try
            {
                var statusApproved = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.EPC.ToString(), EPCStatusCode.Approved.ToString());
                if (!statusApproved.IsSuccessful)
                {
                    response.Message = statusApproved.Message;
                    response.IsSuccessful = false;
                    return response;
                }

                var departments = await m_OrgService.GetAllDepartments();
                if (!departments.IsSuccessful)
                {
                    response.Message = departments.Message;
                    response.IsSuccessful = false;
                    return response;
                }
                var Employees = m_EmployeeContext.Employees.Where(e => e.IsActive == true).ToList();
                if (statusApproved.Item.StatusId <= 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "Approved Status is not found.";
                    return response;
                }

                var welcomeEmail = m_EmployeeContext.WelcomeEmail.Where(e => e.IsActive == true).ToList();
                var projectName = m_EmployeeContext.EmployeeProjects.Where(e => e.IsActive == true).ToList();

                var education = m_EmployeeContext.EducationDetails.Where(e => e.IsActive == true).ToList()
                    .GroupBy(x => x.EmployeeId, (key, g) => new
                    {
                        EmployeeId = key,
                        EducationInstitution = g.OrderBy(x => string.IsNullOrEmpty(x.AcademicYearTo)).ThenBy(y => y.AcademicYearTo).Select(x => x.Institution).First(),
                        Qualification = g.OrderBy(x => string.IsNullOrEmpty(x.AcademicYearTo)).ThenBy(y => y.AcademicYearTo).Select(x => x.EducationalQualification).First(),
                    }
                    ).ToList();

                var skills = await m_OrgService.GetAllSkills(true);
                if (!skills.IsSuccessful || skills.Items == null)
                {
                    response.IsSuccessful = false;
                    response.Message = skills.Message;
                    return response;
                }

                var skill = m_EmployeeContext.EmployeeSkills.Where(e => e.IsPrimary == true).ToList();

                var empskill = (from p in (from em in skill
                                           join sk in skills.Items on em.SkillId equals sk.SkillId
                                           select new WelcomeEmailSkillRequest
                                           {
                                               EmployeeId = em.EmployeeId,
                                               Skillname = sk.SkillName,
                                           })
                                group p by p.EmployeeId)
                                     .ToList();


                var certification = (from p in (from e in Employees
                                                join cert in m_EmployeeContext.AssociateCertifications on e.EmployeeId equals cert.EmployeeId
                                                join skillcert in skills.Items on cert.CertificationId equals skillcert.SkillId

                                                select new WelcomeEmailCertRequest
                                                {
                                                    EmployeeId = e.EmployeeId,
                                                    CertificationName = skillcert.SkillName,
                                                    Specialization = cert.Specialization,
                                                    Institution = cert.Institution
                                                })
                                     group p by p.EmployeeId)
                                     .ToList();

                var prevEmploy = m_EmployeeContext.PreviousEmploymentDetails.Where(e => e.IsActive == true).ToList();

                var designations = await m_OrgService.GetAllDesignations();
                if (!designations.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = designations.Message;
                    return response;
                }

                var employees = (from e in Employees
                                 join em in welcomeEmail on e.EmployeeId equals em.EmployeeId
                                 join pre in prevEmploy on e.EmployeeId equals pre.EmployeeId into g1
                                 from preEmp in g1.DefaultIfEmpty()
                                 join edu in education on e.EmployeeId equals edu.EmployeeId into g2
                                 from eduEmp in g2.DefaultIfEmpty()
                                 join emskill in empskill on e.EmployeeId equals emskill.Key into g3
                                 from skillEmp in g3.DefaultIfEmpty()
                                 join emptype in m_EmployeeContext.EmployeeTypes on e.EmployeeTypeId equals emptype.EmployeeTypeId into g4
                                 from typeEmp in g4.DefaultIfEmpty()
                                 join des in designations.Items on e.DesignationId equals des.DesignationId into g5
                                 from designation in g5.DefaultIfEmpty()
                                 join dep in departments.Items on e.DepartmentId equals dep.DepartmentId into g6
                                 from depEmp in g6.DefaultIfEmpty()
                                 join cert in certification on e.EmployeeId equals cert.Key into g7
                                 from certEmp in g7.DefaultIfEmpty()
                                 where e.IsActive == true && e.StatusId == statusApproved.Item.StatusId
                                 select new WelcomeEmailEmpRequest()
                                 {
                                     EmpId = e.EmployeeId,
                                     EmpCode = e.EmployeeCode,
                                     EmpName = e.FirstName + " " + e.LastName,
                                     MobileNo = Utility.DecryptStringAES(e.MobileNo),
                                     EncryptedMobileNumber = e.MobileNo,
                                     WorkEmail = e.WorkEmailAddress,
                                     PersonalEmailAddress = e.PersonalEmailAddress,
                                     DepartmentId = depEmp != null ? depEmp.DepartmentId : 0,
                                     Department = depEmp != null ? depEmp.Description : "",
                                     Experience = e.Experience,
                                     JoiningDate = e.JoinDate,
                                     PrevEmployeeDetails = preEmp != null ? preEmp.Name : "",
                                     EmploymentType = typeEmp != null ? typeEmp.EmpType : "",
                                     Designation = designation.DesignationName,
                                     PrevEmploymentDesignation = preEmp != null ? preEmp.Designation : "",
                                     Institution = eduEmp != null ? eduEmp.EducationInstitution : "",
                                     HighestQualification = eduEmp != null ? eduEmp.Qualification : "",
                                     SkillName = skillEmp != null ? skillEmp.ToList() : new List<WelcomeEmailSkillRequest>(),
                                     Gender = e.Gender,
                                     CertificationList = certEmp != null ? certEmp.ToList() : new List<WelcomeEmailCertRequest>()
                                 }).OrderByDescending(a => a.EmpId).ToList();


                response.IsSuccessful = true;
                response.Items = employees;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching Employee information";
                m_Logger.LogError(ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region CreateWelcomeEmailInfo
        /// <summary>
        /// Create WelcomeEmail
        /// </summary>
        /// <param name="employeeId">WelcomeEmailIn</param>
        /// <returns>WelcomeEmailActivitites</returns>
        public async Task<ServiceResponse<bool>> CreateWelcomeEmailInfo(int employeeId)
        {
            int isCreated = 0;
            var response = new ServiceResponse<bool>();
            try
            {
                m_Logger.LogInformation("Calling Create method in WelcomeEmailService");

                if (employeeId == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "Invalid Request....";
                    return response;
                }

                var empid = m_EmployeeContext.WelcomeEmail.Where(x => x.EmployeeId == employeeId).FirstOrDefault();
                if (empid == null)
                {
                    var welcomeEmail = new WelcomeEmail();
                    welcomeEmail.EmployeeId = employeeId;
                    welcomeEmail.IsWelcome = false;
                    welcomeEmail.IsActive = true;
                    m_EmployeeContext.WelcomeEmail.Add(welcomeEmail);
                    isCreated = await m_EmployeeContext.SaveChangesAsync();

                    if (isCreated == 0)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error While creating Welcome Email";
                        return response;
                    }
                    else
                    {
                        response.Item = true;
                        response.IsSuccessful = true;
                        response.Message = "Record Added Successfully";
                        return response;
                    }
                }
                else
                {
                    response.IsSuccessful = true;
                    response.Message = "Record already exist";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to create Welcome Email record.";

                m_Logger.LogError("Error occured in Create method." + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region SendWelcomeEmail
        /// <summary>
        /// SendWelcomeEmail
        /// </summary>            
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> SendWelcomeEmail(IFormFileCollection files, WelcomeEmailRequest welcomeEmailReq)
        {
            var response = new ServiceResponse<bool>();
            string newPath = string.Empty;
            try
            {
                NotificationDetail notificationDetail = new NotificationDetail();

                var welEmailReq = m_EmployeeContext.WelcomeEmail.Where(x => x.EmployeeId == welcomeEmailReq.FormMailEmpId && x.IsWelcome == false).FirstOrDefault();

                if (welEmailReq is null || welcomeEmailReq is null)
                {
                    response.IsSuccessful = false;
                    response.Item = false;
                    response.Message = "Employee is not yet approved from HRA";
                    return response;
                }

                notificationDetail.FromEmail = m_WelcomeEmailConfigurations.FromEmail;
                notificationDetail.ToEmail = m_WelcomeEmailConfigurations.ToEmail;

                var file = files.FirstOrDefault();
                string rootPath = Directory.GetCurrentDirectory();
                newPath = Path.Combine(rootPath, "WelcomeEmailImg");

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string fullPath = Path.Combine(newPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                notificationDetail.CcEmail = welcomeEmailReq.FormMailCC;
                notificationDetail.Subject = welcomeEmailReq.FormMailSubject;
                notificationDetail.EmailBody = welcomeEmailReq.FormMailContent;
                notificationDetail.InlineFilePath = fullPath;

                var emailStatus = await m_OrgService.SendEmail(notificationDetail);
                if (!emailStatus.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Item = false;
                    response.Message = "Error occurred while sending email";
                    return response;
                }

                welEmailReq.IsWelcome = true;
                welEmailReq.IsActive = false;

                var isUpdated = await m_EmployeeContext.SaveChangesAsync();
                if (isUpdated == 0)
                {
                    response.IsSuccessful = false;
                    response.Item = false;
                    response.Message = "Error While creating Welcome Email";
                    return response;
                }
                else
                {
                    response.IsSuccessful = true;
                    response.Item = true;
                    response.Message = "Record Added Successfully";
                    return response;
                }
            }

            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while sending welcome email";
                m_Logger.LogError("Error occurred while sending welcome email in WelcomeEmailService " + ex.StackTrace);
                return response;
            }

            finally
            {
                if (Directory.Exists(newPath))
                {
                    Directory.Delete(newPath, true);
                }
            }
        }
        #endregion
    }
}