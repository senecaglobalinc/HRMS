using AutoMapper;
using HRMS.Common;
using HRMS.Common.Enums;
using HRMS.Common.Extensions;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Constants;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Service.External;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    /// <summary>
    /// Service class to get Prospective Associate details
    /// To Create Prospective Associate
    /// To Update prospective Associate
    /// </summary>
    public class ProspectiveAssociateService : IProspectiveAssociateService
    {
        #region Global Varibles

        private readonly ILogger<ProspectiveAssociateService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;
        private readonly IOrganizationService m_OrgService;
        private readonly IProjectService m_ProjectService;
        private readonly IEmployeeService m_EmployeeService;
        private readonly IWelcomeEmailService m_WelcomeEmailService;
        #endregion

        #region Constructor
        public ProspectiveAssociateService(EmployeeDBContext employeeDBContext, ILogger<ProspectiveAssociateService> logger,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints,
            IOrganizationService orgService,
            IProjectService projectService,
            IEmployeeService employeeService,
            IWelcomeEmailService welcomeEmailService)
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProspectiveAssociate, ProspectiveAssociate>();
                cfg.CreateMap<HRMS.Employee.Entities.Employee, EmployeeDetails>();
                cfg.CreateMap<HRMS.Employee.Entities.Employee, HRMS.Employee.Entities.Employee>();
            });
            m_mapper = config.CreateMapper();
            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints?.Value;
            m_OrgService = orgService;
            m_ProjectService = projectService;
            m_EmployeeService = employeeService;
            m_WelcomeEmailService = welcomeEmailService;
        }

        #endregion

        #region Create
        /// <summary>
        /// Create prospectiveAssociate
        /// </summary>
        /// <param name="prospectiveAssociateIn">prospectiveAssociateIn</param>
        /// <returns></returns>
        public async Task<ServiceResponse<ProspectiveAssociate>> Create(ProspectiveAssociate prospectiveAssociateIn)
        {
            int isCreated;
            var response = new ServiceResponse<ProspectiveAssociate>();
            try
            {
                m_Logger.LogInformation("Calling Create method in ProspectiveAssociateService");

                var practiceArea = await m_OrgService.GetPracticeAreaByCode(PracticeAreaCodes.Training.ToString());
                if (!practiceArea.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = practiceArea.Message;
                    return response;
                }

                var department = await m_OrgService.GetDepartmentByCode(DepartmentCodes.TrainingDepartment.GetEnumDescription());
                if (!department.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = department.Message;
                    return response;
                }

                //Check whether the email id already exists
                m_Logger.LogInformation("Checking whether the email id already exists.");
                if (!string.IsNullOrEmpty(prospectiveAssociateIn.PersonalEmailAddress))
                {
                    ProspectiveAssociate associate = m_EmployeeContext.ProspectiveAssociates.Where(email => email.PersonalEmailAddress ==
                                                    prospectiveAssociateIn.PersonalEmailAddress && email.IsActive == true).FirstOrDefault();

                    if (associate != null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Email Id already Exists.";
                        return response;
                    }
                }

                //check whether the prospective associate exists with same name and mobile number.
                m_Logger.LogInformation("check whether the prospective associate exists with same name and mobile number.");
                ProspectiveAssociate prospectiveAssociate = m_EmployeeContext.ProspectiveAssociates.Where(n => (n.FirstName == prospectiveAssociateIn.FirstName
                                                        && n.LastName == prospectiveAssociateIn.LastName && n.MobileNo == Utility.EncryptStringAES(prospectiveAssociateIn.MobileNo) && n.IsActive == true)).FirstOrDefault();

                if (prospectiveAssociate == null)
                {
                    prospectiveAssociate = new ProspectiveAssociate();
                    if (!prospectiveAssociateIn.IsActive.HasValue)
                        prospectiveAssociateIn.IsActive = true;

                    m_mapper.Map<ProspectiveAssociate, ProspectiveAssociate>(prospectiveAssociateIn, prospectiveAssociate);
                    prospectiveAssociate.JoinDate = Utility.GetDateTimeInIST(prospectiveAssociate.JoinDate);
                    prospectiveAssociate.MobileNo = Utility.EncryptStringAES(prospectiveAssociate.MobileNo);
                    prospectiveAssociate.TechnologyID = (department.Item.DepartmentId == prospectiveAssociate.DepartmentId) ?
                                             practiceArea.Item.PracticeAreaId : prospectiveAssociate.TechnologyID; //Here TechnologyID refers to PracticeAreaId
                    m_EmployeeContext.ProspectiveAssociates.Add(prospectiveAssociate);
                    isCreated = await m_EmployeeContext.SaveChangesAsync();
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "User already exists with same name and mobile number.";
                    return response;
                }

                if (isCreated > 0)
                {
                    response.Item = prospectiveAssociate;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No prospective Associate created.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to create prospective Associate.";

                m_Logger.LogError("Error occured in Create method." + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets the active prospective Associates based on departments, designations and practiceArea.
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeDetails>> GetProspectiveAssociates(bool? isActive = true)
        {
            var response = new ServiceListResponse<EmployeeDetails>();
            try
            {
                m_Logger.LogInformation("Calling GetAll method in ProspectiveAssociateService");

                var practiceAreas = await m_OrgService.GetAllPracticeAreas();
                if (!practiceAreas.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = practiceAreas.Message;
                    return response;
                }

                var departments = await m_OrgService.GetAllDepartments();
                if (!departments.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = departments.Message;
                    return response;
                }

                var designations = await m_OrgService.GetAllDesignations();
                if (!designations.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = designations.Message;
                    return response;
                }

                var prospectiveAssociates = m_EmployeeContext.ProspectiveAssociates.Where(pa => pa.IsActive == isActive).ToList();
                var getProspectiveAssociates = (from pa in prospectiveAssociates
                                                join d in departments.Items on pa.DepartmentId equals d.DepartmentId into g1
                                                from dep in g1.DefaultIfEmpty()
                                                join des in designations.Items on pa.DesignationId equals des.DesignationId into g2
                                                from des in g2.DefaultIfEmpty()
                                                join tech in practiceAreas.Items on pa.TechnologyID equals tech.PracticeAreaId into g3
                                                from practiceArea in g3.DefaultIfEmpty()
                                                select new EmployeeDetails
                                                {
                                                    Id = pa.Id,
                                                    EmpName = pa.FirstName + " " + pa.LastName,
                                                    FirstName = pa.FirstName,
                                                    LastName = pa.LastName,
                                                    MiddleName = pa.MiddleName,
                                                    Gender = pa.Gender,
                                                    MaritalStatus = pa.MaritalStatus,
                                                    MobileNo = Utility.DecryptStringAES(pa.MobileNo),
                                                    PersonalEmailAddress = pa.PersonalEmailAddress,
                                                    joiningStatusID = pa.JoiningStatusId,
                                                    GradeId = pa.GradeId,
                                                    EmploymentType = pa.EmploymentType,
                                                    Technology = practiceArea != null ? practiceArea.PracticeAreaCode : String.Empty,
                                                    TechnologyID = pa.TechnologyID,
                                                    DesignationId = pa.DesignationId,
                                                    Designation = des.DesignationName,
                                                    DepartmentId = pa.DepartmentId,
                                                    Department = dep != null ? dep.DepartmentCode : String.Empty,
                                                    DepartmentDesc = dep != null ? dep.Description : String.Empty,
                                                    JoiningDate = pa.JoinDate == null ? null : string.Format("{0:dd/MM/yyyy}", pa.JoinDate),
                                                    HRAdvisorName = pa.HRAdvisorName,
                                                    BgvStatusId = pa.BGVStatusId,
                                                    RecruitedBy = pa.RecruitedBy,
                                                    EmpId = pa.EmployeeID ?? default(int),
                                                    ManagerId = pa.ManagerId ?? default(int)
                                                }).OrderBy(i => i.JoiningDate).ToList();

                response.Items = getProspectiveAssociates;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching ProspectiveAssociates";

                m_Logger.LogError("Error occured in GetProspectiveAssociates() method." + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region GetbyId
        /// <summary>
        /// Get ProspectiveAssociate by Id based on departments, designations and grades.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<EmployeeDetails>> GetbyId(int Id)
        {
            var response = new ServiceResponse<EmployeeDetails>();
            try
            {
                m_Logger.LogInformation("Calling GetbyId method in ProspectiveAssociateService");

                var designations = await m_OrgService.GetAllDesignations();
                if (!designations.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = designations.Message;
                    return response;
                }

                var departments = await m_OrgService.GetAllDepartments();
                if (!departments.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = departments.Message;
                    return response;
                }

                var grades = await m_OrgService.GetAllGrades();
                if (!grades.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = grades.Message;
                    return response;
                }

                var prospectiveAssociates = m_EmployeeContext.ProspectiveAssociates.Where(pa => pa.Id == Id && pa.IsActive == true).ToList();

                var prospectiveAssociate = (from pa in prospectiveAssociates
                                            join d in departments.Items on pa.DepartmentId equals d.DepartmentId
                                            join des in designations.Items on pa.DesignationId equals des.DesignationId
                                            join grade in grades.Items on pa.GradeId equals grade.GradeId
                                            select new EmployeeDetails
                                            {
                                                Id = pa.Id,
                                                EmpName = pa.FirstName + " " + pa.LastName,
                                                FirstName = pa.FirstName,
                                                LastName = pa.LastName,
                                                MiddleName = pa.MiddleName,
                                                Gender = pa.Gender,
                                                MaritalStatus = pa.MaritalStatus,
                                                MobileNo = Utility.DecryptStringAES(pa.MobileNo),
                                                PersonalEmailAddress = pa.PersonalEmailAddress,
                                                joiningStatusID = pa.JoiningStatusId,
                                                GradeId = pa.GradeId,
                                                GradeName = grade.GradeName,
                                                EmploymentType = pa.EmploymentType,
                                                Technology = pa.Technology,
                                                TechnologyID = pa.TechnologyID, //Here TechnologyID refers to PracticeAreaId
                                                DesignationId = pa.DesignationId,
                                                Designation = des.DesignationName,
                                                DepartmentId = pa.DepartmentId,
                                                Department = d.DepartmentCode,
                                                DateOfJoining = pa.JoinDate,
                                                JoiningDate = pa.JoinDate == null ? null : string.Format("{0:dd/MM/yyyy}", pa.JoinDate),
                                                HRAdvisorName = pa.HRAdvisorName,
                                                BgvStatusId = pa.BGVStatusId,
                                                RecruitedBy = pa.RecruitedBy,
                                                EmpId = pa.EmployeeID ?? default(int),
                                                ManagerId = pa.ManagerId ?? default(int),
                                                EmpCode = string.Empty,
                                                DropOutReason = pa.ReasonForDropOut,
                                                ReportingManagerId = pa.ManagerId == null ? 0 : pa.ManagerId.Value
                                            }).FirstOrDefault();

                response.Item = prospectiveAssociate;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching ProspectiveAssociate";

                m_Logger.LogError("Error occured in GetbyId() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update ProspectiveAssociate
        /// </summary>
        /// <param name="prospectiveAssociateIn">prospectiveAssociate information</param>
        /// <returns></returns>
        public async Task<ServiceResponse<ProspectiveAssociate>> Update(ProspectiveAssociate prospectiveAssociateIn)
        {
            int Updated;
            var response = new ServiceResponse<ProspectiveAssociate>();
            try
            {
                m_Logger.LogInformation("Calling Update method in ProspectiveAssociateService");

                var practiceArea = await m_OrgService.GetPracticeAreaByCode(PracticeAreaCodes.Training.ToString());
                if (!practiceArea.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = practiceArea.Message;
                    return response;
                }

                var department = await m_OrgService.GetDepartmentByCode(DepartmentCodes.TrainingDepartment.GetEnumDescription());
                if (!department.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = department.Message;
                    return response;
                }

                //check whether the email Id already exists
                if (!string.IsNullOrEmpty(prospectiveAssociateIn.PersonalEmailAddress))
                {
                    ProspectiveAssociate associate = m_EmployeeContext.ProspectiveAssociates.Where(ps => ps.PersonalEmailAddress == prospectiveAssociateIn.PersonalEmailAddress
                                                     && ps.IsActive == true && ps.Id != prospectiveAssociateIn.Id).FirstOrDefault();

                    if (associate != null && string.IsNullOrEmpty(prospectiveAssociateIn.ReasonForDropOut))
                    {
                        response.IsSuccessful = false;
                        response.Message = "Email already exists.";
                        return response;
                    }
                }

                m_Logger.LogInformation("Checking whether the user exists with same name and mobile number");
                ProspectiveAssociate prospectiveAsociate = m_EmployeeContext.ProspectiveAssociates.Where(p => (p.FirstName == prospectiveAssociateIn.FirstName
                                                           && p.LastName == prospectiveAssociateIn.LastName && p.MobileNo == Utility.EncryptStringAES(prospectiveAssociateIn.MobileNo) &&
                                                           p.IsActive == true && p.Id != prospectiveAssociateIn.Id)).FirstOrDefault();

                if (prospectiveAsociate != null && string.IsNullOrEmpty(prospectiveAssociateIn.ReasonForDropOut))
                {
                    response.IsSuccessful = false;
                    response.Message = "User already exists with same name and mobile number.";
                    return response;
                }

                ProspectiveAssociate prospectiveAssociate = m_EmployeeContext.ProspectiveAssociates.Find(prospectiveAssociateIn.Id);

                prospectiveAssociateIn.MobileNo = Utility.EncryptStringAES(prospectiveAssociateIn.MobileNo);
                prospectiveAssociateIn.JoinDate = Utility.GetDateTimeInIST(prospectiveAssociateIn.JoinDate);
                prospectiveAssociateIn.TechnologyID = (department.Item.DepartmentId == prospectiveAssociate.DepartmentId) ?
                                             practiceArea.Item.PracticeAreaId : prospectiveAssociateIn.TechnologyID; //Here TechnologyID refers to PracticeAreaId
                prospectiveAssociateIn.IsActive = string.IsNullOrEmpty(prospectiveAssociateIn.ReasonForDropOut) ? true : false;
                prospectiveAssociateIn.CreatedBy = prospectiveAssociate.CreatedBy;
                prospectiveAssociateIn.CreatedDate=prospectiveAssociate.CreatedDate;
                m_mapper.Map<ProspectiveAssociate, ProspectiveAssociate>(prospectiveAssociateIn, prospectiveAssociate);

                m_Logger.LogInformation("Calling SaveChanges method on DB Context in ProspectiveAssociateService");
                Updated = await m_EmployeeContext.SaveChangesAsync();

                if (Updated > 0)
                {
                    response.Item = prospectiveAssociate;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No prospective Associate updated.";
                }
            }

            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to Update prospective Associate.";

                m_Logger.LogError("Error occured in Update() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region UpdateEmployeeStatusToPending
        /// <summary>
        /// Update employee status to pending and send email pending notification to HRM to approve or reject
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> UpdateEmployeeStatusToPending(int employeeId)
        {
            int isUpdated;
            var response = new ServiceResponse<bool>();
            try
            {
                //Get StatusId whose statuscode is pending
                var status = await m_OrgService.GetStatusByCode(EPCStatusCode.Pending.ToString());
                if (!status.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = status.Message;
                    return response;
                }

                var employee = m_EmployeeContext.Employees.Where(emp => emp.EmployeeId == employeeId).FirstOrDefault();
                if (employee == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while fetching employee";
                    return response;
                }

                //update employee status to pending
                employee.StatusId = status.Item.StatusId;

                m_Logger.LogInformation("Calling SaveChanges method on DB Context in EmployeeStatusService");
                isUpdated = await m_EmployeeContext.SaveChangesAsync();
                if (!(isUpdated > 0))
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating employee status";
                    return response;
                }

                var emailNotificationConfig = await m_OrgService.GetByNotificationTypeAndCategory((int)EPCNotificationStatusCode.Pending, (int)CategoryMaster.EPC);
                if (!emailNotificationConfig.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = emailNotificationConfig.Message;
                    return response;
                }

                var department = await m_OrgService.GetDepartmentById((int)employee.DepartmentId);
                if (!department.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = department.Message;
                    return response;
                }
                var designation = await m_OrgService.GetDesignationById((int)employee.DesignationId);
                if (!designation.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = designation.Message;
                    return response;
                }
                NotificationDetail notificationDetail = new NotificationDetail();
                StringBuilder emailContent = new StringBuilder(WebUtility.HtmlDecode(emailNotificationConfig.Item.EmailContent));

                var reportingManagerDtls = await m_EmployeeService.GetActiveEmployeeById(employee.ReportingManager ?? 0);
                var practiceArea = (await m_OrgService.GetAllPracticeAreas()).Items.Where(x => x.PracticeAreaId == employee.CompetencyGroup).FirstOrDefault();
                emailContent = emailContent.Replace("{AssociateId}", employee.EmployeeCode)
                                           .Replace("{AssociateName}", employee.FirstName + " " + employee.LastName)
                                           .Replace("{Designation}", designation.Item.DesignationCode)
                                           .Replace("{Department}", department.Item.DepartmentCode)
                                           .Replace("{ReportingManager}", reportingManagerDtls.Item != null ?
                                           (reportingManagerDtls.Item.FirstName + " " + reportingManagerDtls.Item.LastName) : "")
                                           .Replace("{Technology}", practiceArea != null ? practiceArea.PracticeAreaDescription : "");

                if (string.IsNullOrEmpty(emailNotificationConfig.Item.EmailFrom))
                {
                    response.IsSuccessful = false;
                    response.Message = "Email From cannot be blank";
                    return response;
                }
                notificationDetail.FromEmail = emailNotificationConfig.Item.EmailFrom;

                if (string.IsNullOrEmpty(emailNotificationConfig.Item.EmailTo))
                {
                    response.IsSuccessful = false;
                    response.Message = "Email To cannot be blank";
                    return response;
                }
                notificationDetail.ToEmail = emailNotificationConfig.Item.EmailTo;
                notificationDetail.CcEmail = emailNotificationConfig.Item.EmailCC;
                notificationDetail.Subject = employee.FirstName + " " + employee.LastName + " " + emailNotificationConfig.Item.EmailSubject;
                notificationDetail.EmailBody = emailContent.ToString();

                var emailStatus = await m_OrgService.SendEmail(notificationDetail);
                if (!emailStatus.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while sending email";
                    return response;
                }
                response.IsSuccessful = true;
                response.Item = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while updating employee status";
                m_Logger.LogError("Error occurred in \"UpdateEmployeeStatus\" of EmployeeStatusService" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region UpdateEmployeeProfileStatus
        /// <summary>
        /// Update employee status and send respective email either approval or rejection notification to HRM 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="reqType"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> UpdateEmployeeProfileStatus(EmployeeProfileStatus employeeProfileStatus)
        {
            int isUpdated;
            var response = new ServiceResponse<bool>();
            NotificationConfiguration emailNotificationConfig = null;
            try
            {
                HRMS.Employee.Entities.Employee employee = m_EmployeeContext.Employees.Where(emp => emp.EmployeeId == employeeProfileStatus.EmpId).FirstOrDefault();
                if (employee == null && string.IsNullOrEmpty(employee.EmployeeCode))
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while fetching employee";
                    return response;
                }
            
                EmployeeDetails employee1 = new EmployeeDetails();

                //check the history table that department is prescent or not based on rescent hostory data and employee date we can compare that department changed or not
                var existingDepartmentHistory = m_EmployeeContext.EmployeeHistory.Where(hostory => hostory.EmployeeId == employee.EmployeeId).OrderByDescending(history => history.Id).FirstOrDefault();
                if (existingDepartmentHistory == null && employee.DepartmentId!=null)
                {
                    employee1.IsDepartmentChange = false;
                }
                else
                {
                    employee1.IsDepartmentChange = true;
                }
                m_mapper.Map<HRMS.Employee.Entities.Employee, EmployeeDetails>(employee, employee1);

                //check the request type
                if (employeeProfileStatus.Status == EPCStatusCode.Approved.ToString())
                {
                    //Fetch prospective associate based on employeeId
                    ProspectiveAssociate prospectiveAssociate = m_EmployeeContext.ProspectiveAssociates
                                                                                 .Where(p => p.EmployeeID == employeeProfileStatus.EmpId)
                                                                                 .FirstOrDefault();

                    //inactivate the employee record in prospective associates table
                    if (prospectiveAssociate != null)
                    {
                        prospectiveAssociate.IsActive = false;
                    }

                    //update employee status to approved
                    employee.StatusId = (int)EPCStatusCode.Approved;

                    //get email content for employee profile approval
                    var emailNotification = await m_OrgService.GetByNotificationTypeAndCategory((int)EPCNotificationStatusCode.Approved, (int)CategoryMaster.EPC);
                    if (!emailNotification.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = emailNotification.Message;
                        return response;
                    }
                    emailNotificationConfig = emailNotification.Item;
                  
                    if (employee.CompetencyGroup != null)
                    {
                        //allocate associate to talent pool based on competency group
                        var allocation = await m_ProjectService.AllocateAssociateToTalentPool(employee1);
                        if (!allocation.IsSuccessful)
                        {
                            response.IsSuccessful = false;
                            response.Message = "Error occurred while allocating employee to talent pool";
                            return response;
                        }

                        var updateTP_ProjectInAssociateAllocation = m_ProjectService.UpdatePracticeAreaOfTalentPoolProject(employee.EmployeeId, (int)employee.CompetencyGroup).Result.IsSuccessful;
                        if (!updateTP_ProjectInAssociateAllocation)
                        {
                            response.IsSuccessful = false;
                            response.Message = "Error occured while updating talent-pool project in associate allocation";
                            return response;
                        }
                    }
                    else
                    {
                        //if department changes from delivery to non-delivery then release all the active allocations.

                        ServiceListResponse<AssociateAllocation> associateAllocation = await m_ProjectService.GetAssociateAllocationsByEmployeeId(employee.EmployeeId);

                        if (associateAllocation.Items != null && associateAllocation.IsSuccessful && employee.CompetencyGroup == null)
                        {
                            var releasedAllocations=await m_ProjectService.ReleaseFromAllocations(employee.EmployeeId);
                            if(!releasedAllocations.IsSuccessful)
                            {
                                response.IsSuccessful = false;
                                response.Message = "Error occured while releasing associate allocation";
                                return response;
                            }                          
                        }
                    }
                }
                else
                {
                    //update employee status to rejected
                    employee.StatusId = (int)EPCStatusCode.Rejected;

                    //get email content for employee profile rejection
                    var emailNotification = await m_OrgService.GetByNotificationTypeAndCategory((int)EPCNotificationStatusCode.Rejected, (int)CategoryMaster.EPC);
                    if (!emailNotification.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = emailNotification.Message;
                        return response;
                    }
                    emailNotificationConfig = emailNotification.Item;
                }
                employee.Remarks = employeeProfileStatus.Remarks;

                m_Logger.LogInformation("Calling SaveChanges method on DB Context in EmployeeStatusService");
                isUpdated = await m_EmployeeContext.SaveChangesAsync();
                if (!(isUpdated > 0))
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating employee status";
                    return response;
                }

                //fetch the deparment details of the employee
                var department = await m_OrgService.GetDepartmentById((int)employee.DepartmentId);
                if (!department.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = department.Message;
                    return response;
                }

                //fetch the designation details of the employee
                var designation = await m_OrgService.GetDesignationById((int)employee.DesignationId);
                if (!designation.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = designation.Message;
                    return response;
                }

                NotificationDetail notificationDetail = new NotificationDetail();
                StringBuilder emailContent = new StringBuilder(WebUtility.HtmlDecode(emailNotificationConfig.EmailContent));

                //Replace email content with current employee details
                emailContent = emailContent.Replace("{AssociateName}", employee.FirstName + " " + employee.LastName)
                                           .Replace("{Designation}", designation.Item.DesignationCode)
                                           .Replace("{Department}", department.Item.DepartmentCode);

                //Validate email Notification details
                if (string.IsNullOrEmpty(emailNotificationConfig.EmailFrom))
                {
                    response.IsSuccessful = false;
                    response.Message = "Email From cannot be blank";
                    return response;
                }
                if (string.IsNullOrEmpty(emailNotificationConfig.EmailTo))
                {
                    response.IsSuccessful = false;
                    response.Message = "Email To cannot be blank";
                    return response;
                }
                notificationDetail.FromEmail = emailNotificationConfig.EmailFrom;
                notificationDetail.ToEmail = emailNotificationConfig.EmailTo;
                notificationDetail.CcEmail = emailNotificationConfig.EmailCC;
                notificationDetail.Subject = employee.FirstName + " " + employee.LastName + "(" + employee.EmployeeCode + ") " + emailNotificationConfig.EmailSubject;
                notificationDetail.EmailBody = emailContent.ToString();

                var emailStatus = await m_OrgService.SendEmail(notificationDetail);
                if (!emailStatus.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while sending email";
                    return response;
                }
                if (employee.StatusId == (int)EPCStatusCode.Approved)
                {
                    var welcomeEmailReq = await m_WelcomeEmailService.CreateWelcomeEmailInfo(employee.EmployeeId);
                    if (!welcomeEmailReq.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = welcomeEmailReq.Message;
                        return response;
                    }
                }
                response.IsSuccessful = true;
                response.Item = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while updating employee";
                m_Logger.LogError("Error occurred in \"UpdateEmployeeProfileStatus\" in EmployeeStatusService " + ex.StackTrace);
                return response;
            }
            return response;
        }
        #endregion
    }
}
