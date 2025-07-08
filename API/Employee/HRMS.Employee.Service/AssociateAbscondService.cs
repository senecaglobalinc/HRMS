using HRMS.Common;
using HRMS.Common.Enums;
using HRMS.Common.Extensions;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Constants;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    /// <summary>
    /// Service class to Maintain Associate Abscond
    /// </summary>
    public class AssociateAbscondService : IAssociateAbscondService
    {
        #region Global Varibles
        private readonly ILogger<AssociateAbscondService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IOrganizationService m_OrgService;
        private readonly IProjectService m_ProjectService;
        private readonly IConfiguration m_configuration;
        private readonly IAssociateExitActivityService m_AssociateExitActivityService;
        private readonly AssociateExitMailSubjects m_exitMailSubjects;
        private readonly EmailConfigurations m_EmailConfigurations;       
        #endregion

        #region Constructor
        public AssociateAbscondService(EmployeeDBContext employeeDBContext,
            ILogger<AssociateAbscondService> logger,
            IOrganizationService orgService,
            IProjectService projectService,
            IConfiguration configuration,
            IAssociateExitActivityService associateExitActivityService,
            IOptions<AssociateExitMailSubjects> exitMailSubjects,
            IOptions<EmailConfigurations> emailConfigurations)
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            m_OrgService = orgService;
            m_ProjectService = projectService;
            m_configuration = configuration;
            m_AssociateExitActivityService = associateExitActivityService;
            m_exitMailSubjects = exitMailSubjects.Value;
            m_EmailConfigurations = emailConfigurations.Value;
        }
        #endregion

        #region GetAssociateByLead
        /// <summary>
        /// GetAssociateByLead
        /// </summary>
        /// <param name="leadId">leadId</param>
        /// <param name="deptId">deptId</param>
        /// <returns>bool</returns>
        public async Task<ServiceListResponse<AssociateModel>> GetAssociateByLead(int leadId, int deptId)
        {
            var response = new ServiceListResponse<AssociateModel>();
            ServiceListResponse<AssociateAllocation> associateAllocation = null;
            List<Entities.Employee> employees = null;
            List<AssociateAllocation> allocationDtls = null;
            List<AssociateModel> associates = null;

            try
            {
                if (leadId != 0 && deptId == 1)
                {
                    employees = await m_EmployeeContext.Employees.Where(x => x.IsActive == true).ToListAsync();
                    associateAllocation = await m_ProjectService.GetAssociateAllocationsByLeadId(leadId);
                    allocationDtls = associateAllocation?.Items?.Where(x => x.IsPrimary == true).Select(x => x).ToList();

                    associates = (from emp in employees
                                  join all in allocationDtls on emp.EmployeeId equals all.EmployeeId
                                  join exits in m_EmployeeContext.AssociateExit.Where(x => x.IsActive == true)
                                  on emp.EmployeeId equals exits.EmployeeId
                                  into ae
                                  from exits in ae.DefaultIfEmpty()
                                  select new AssociateModel
                                  {
                                      AssociateId = emp.EmployeeId,
                                      AssociateName = emp.FirstName + " " + emp.LastName,
                                      AssociateEmail = emp.WorkEmailAddress,
                                      AssociateCode = emp.EmployeeCode,
                                      AssociateExitFlag = exits != null && (bool)exits.IsActive
                                  }).OrderBy(e => e.AssociateId).ToList();
                }
                else if (leadId != 0 && deptId != 1)
                {
                    associates = (from emp in m_EmployeeContext.Employees.Where(x => x.IsActive == true && x.ReportingManager == leadId)
                                  join exits in m_EmployeeContext.AssociateExit.Where(x => x.IsActive == true)
                                  on emp.EmployeeId equals exits.EmployeeId
                                  into ae
                                  from exits in ae.DefaultIfEmpty()
                                  select new AssociateModel
                                  {
                                      AssociateId = emp.EmployeeId,
                                      AssociateName = emp.FirstName + " " + emp.LastName,
                                      AssociateEmail = emp.WorkEmailAddress,
                                      AssociateCode = emp.EmployeeCode,
                                      AssociateExitFlag = exits != null && (bool)exits.IsActive
                                  }).OrderBy(e => e.AssociateId).ToList();
                }
                else
                {
                    associates = null;
                }

                if (associates != null && associates.Count > 0)
                {
                    response.IsSuccessful = true;
                    response.Items = associates;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No Employee Details Found!";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data";
                m_Logger.LogError(ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetAssociatesAbscondDashboard
        /// <summary>
        /// Gets abscond employees list by user role, employeeId and dashboard
        /// </summary>
        /// <param name="userRole"></param>
        /// <param name="employeeId"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<AbscondDashboardResponse>> GetAssociatesAbscondDashboard(string userRole, int employeeId, int departmentId)
        {
            ServiceListResponse<AbscondDashboardResponse> response = new ServiceListResponse<AbscondDashboardResponse>();

            try
            {
                if (string.IsNullOrEmpty(userRole) || employeeId == 0)
                {
                    return response = new ServiceListResponse<AbscondDashboardResponse>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "Invalid Request."
                    };
                }

                int markedForAbscondStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.MarkedForAbscond);
                int abscondAcknowledgedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.AbscondAcknowledged);
                int readyForClearanceStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.ReadyForClearance);
                int abscondConfirmedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.AbscondConfirmed);
                int abscondedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.Absconded);
                int blacklistedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.Blacklisted);
                int inProgressStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.DepartmentActivityInProgress);
                int completedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.DepartmentActivityCompleted);

                ServiceListResponse<Status> statusesforAssociateExit = await m_OrgService.GetStatusesByCategoryName(CategoryMaster.AssociateExit.ToString());
                if (statusesforAssociateExit.Items == null || (statusesforAssociateExit.Items != null && statusesforAssociateExit.Items.Count <= 0))
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "Status for Associate Abscond not found.";
                    return response;
                }

                List<AbscondDashboardResponse> abscondResponse = null;

                if (Roles.HRA.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()))
                {
                    abscondResponse = await (m_EmployeeContext.Employees.Where(x => x.IsActive == true)
                                        .Join(m_EmployeeContext.AssociateAbscond.Where(x => x.IsActive == true),
                                        emp => emp.EmployeeId, abs => abs.AssociateId, (emp, abs) => new { emp, abs })
                                        .Where(y => y.abs.StatusId == markedForAbscondStatusId || y.abs.StatusId == abscondAcknowledgedStatusId
                                        || y.abs.StatusId == abscondConfirmedStatusId || y.abs.StatusId == readyForClearanceStatusId)
                                        .OrderByDescending(o => o.abs.CreatedDate)
                                        .Select(z => new AbscondDashboardResponse
                                        {
                                            AssociateId = z.emp.EmployeeId,
                                            AssociateCode = z.emp.EmployeeCode,
                                            AssociateName = $"{z.emp.FirstName} {z.emp.LastName}",
                                            AssociateAbscondId = z.abs.AssociateAbscondId,
                                            StatusId = z.abs.StatusId,
                                            EditAction = z.abs.StatusId == markedForAbscondStatusId,
                                            ViewAction = z.abs.StatusId != markedForAbscondStatusId,
                                            ViewActivity = z.abs.StatusId == abscondConfirmedStatusId || z.abs.StatusId == readyForClearanceStatusId
                                        })).ToListAsync();

                    abscondResponse.ForEach(e => e.StatusDesc = statusesforAssociateExit.Items.FirstOrDefault(st => st.StatusId == e.StatusId).StatusDescription);
                }
                else if (Roles.HRM.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()))
                {
                    abscondResponse = await m_EmployeeContext.Employees.Where(x => x.IsActive == true)
                                        .Join(m_EmployeeContext.AssociateAbscond.Where(x => x.IsActive == true),
                                        emp => emp.EmployeeId, abs => abs.AssociateId, (emp, abs) => new { emp, abs })
                                        .GroupJoin(m_EmployeeContext.AssociateExitActivity.Where(x => x.DepartmentId == departmentId),
                                        ea => ea.abs.AssociateAbscondId, act => act.AssociateAbscondId, (ea, act) => new { eaa = ea, actt = act })
                                        .SelectMany(x => x.actt.DefaultIfEmpty(), (x, y) => new { x.eaa, actt = y })
                                        .Where(y => y.eaa.abs.StatusId == abscondAcknowledgedStatusId
                                        || y.eaa.abs.StatusId == abscondConfirmedStatusId
                                        || y.eaa.abs.StatusId == readyForClearanceStatusId)
                                        .OrderByDescending(o => o.eaa.abs.CreatedDate)
                                        .Select(z => new AbscondDashboardResponse
                                        {
                                            AssociateId = z.eaa.emp.EmployeeId,
                                            AssociateCode = z.eaa.emp.EmployeeCode,
                                            AssociateName = $"{z.eaa.emp.FirstName} {z.eaa.emp.LastName}",
                                            AssociateAbscondId = z.eaa.abs.AssociateAbscondId,
                                            StatusId = z.eaa.abs.StatusId,
                                            EditAction = z.eaa.abs.StatusId == abscondAcknowledgedStatusId,
                                            ViewAction = z.eaa.abs.StatusId == abscondConfirmedStatusId,
                                            EditActivity = z.eaa.abs.StatusId == abscondConfirmedStatusId && z.actt.StatusId == inProgressStatusId,
                                            ViewActivity = z.eaa.abs.StatusId == abscondConfirmedStatusId && z.actt.StatusId == completedStatusId,
                                            EditClearance = z.eaa.abs.StatusId == readyForClearanceStatusId
                                        }).ToListAsync();

                    abscondResponse.ForEach(e => e.StatusDesc = statusesforAssociateExit.Items.FirstOrDefault(st => st.StatusId == e.StatusId).StatusDescription);
                }
                else if (Roles.ITManager.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim())
                        || Roles.FinanceManager.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim())
                        || Roles.AdminManager.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim())
                        || Roles.TrainingDepartmentHead.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()))
                {
                    abscondResponse = await m_EmployeeContext.Employees.Where(x => x.IsActive == true)
                                        .Join(m_EmployeeContext.AssociateAbscond.Where(x => x.IsActive == true),
                                        emp => emp.EmployeeId, abs => abs.AssociateId, (emp, abs) => new { emp, abs })
                                        .Join(m_EmployeeContext.AssociateExitActivity,
                                        ea => ea.abs.AssociateAbscondId, act => act.AssociateAbscondId, (ea, act) => new { ea.emp, ea.abs, act })
                                        .Where(y => y.abs.StatusId == abscondConfirmedStatusId && y.act.DepartmentId == departmentId)
                                        .OrderByDescending(o => o.abs.CreatedDate)
                                        .Select(z => new AbscondDashboardResponse
                                        {
                                            AssociateId = z.emp.EmployeeId,
                                            AssociateCode = z.emp.EmployeeCode,
                                            AssociateName = $"{z.emp.FirstName} {z.emp.LastName}",
                                            AssociateAbscondId = z.abs.AssociateAbscondId,
                                            StatusId = z.abs.StatusId,
                                            EditActivity = z.abs.StatusId == abscondConfirmedStatusId && z.act.StatusId == inProgressStatusId,
                                            ViewActivity = z.abs.StatusId == abscondConfirmedStatusId && z.act.StatusId == completedStatusId
                                        }).ToListAsync();

                    abscondResponse.ForEach(e => e.StatusDesc = statusesforAssociateExit.Items.FirstOrDefault(st => st.StatusId == e.StatusId).StatusDescription);
                }

                response.Items = abscondResponse;
                response.IsSuccessful = true;
                response.Message = string.Empty;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to create Associate Exit.";

                m_Logger.LogError("Error occured in Create method." + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region GetAbscondDetailByAssociateId
        /// <summary>
        /// GetAbscondDetailByAssociateId
        /// </summary>
        /// <param name="associateId">associateId</param>
        /// <returns></returns>
        public async Task<ServiceResponse<AssociateAbscondModel>> GetAbscondDetailByAssociateId(int associateId)
        {
            var response = new ServiceResponse<AssociateAbscondModel>();
            try
            {
                AssociateAbscondModel associateAbscondModel =
                    await m_EmployeeContext.Employees.Where(x => x.IsActive == true)
                    .Join(m_EmployeeContext.AssociateAbscond.Where(x => x.IsActive == true),
                    emp => emp.EmployeeId, abs => abs.AssociateId, (emp, abs) => new { emp, abs })
                    .Where(y => y.abs.AssociateId == associateId)
                    .Select(z => new AssociateAbscondModel
                    {
                        AssociateAbscondId = z.abs.AssociateAbscondId,
                        AssociateId = z.abs.AssociateId,
                        AssociateName = $"{z.emp.FirstName} {z.emp.LastName}",
                        AbsentFromDate = z.abs.AbsentFromDate,
                        AbsentToDate = z.abs.AbsentToDate,
                        IsAbscond = z.abs.IsAbscond,
                        StatusId = z.abs.StatusId,
                        TLId = z.abs.TLId,
                        RemarksByTL = String.IsNullOrEmpty(z.abs.RemarksByTL) ? "" : (IsBase64(z.abs.RemarksByTL) ? Utility.DecryptStringAES(z.abs.RemarksByTL) : ""),
                        HRAId = z.abs.HRAId,
                        RemarksByHRA = String.IsNullOrEmpty(z.abs.RemarksByHRA) ? "" : (IsBase64(z.abs.RemarksByHRA) ? Utility.DecryptStringAES(z.abs.RemarksByHRA) : ""),
                        HRMId = z.abs.HRMId,
                        RemarksByHRM = String.IsNullOrEmpty(z.abs.RemarksByHRM) ? "" : (IsBase64(z.abs.RemarksByHRM) ? Utility.DecryptStringAES(z.abs.RemarksByHRM) : "")
                    }).FirstOrDefaultAsync();

                response.IsSuccessful = true;
                response.Item = associateAbscondModel;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Associate Abscond";
                m_Logger.LogError(ex.StackTrace);
            }
            return response;
        }
        #endregion 

        #region CreateAbscond
        /// <summary>
        /// CreateAbscond
        /// <param name="abscondReq">abscondReq</param>
        /// <returns>bool</returns>
        /// </summary>
        public async Task<ServiceResponse<bool>> CreateAbscond(AssociateAbscondModel abscondReq)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                m_Logger.LogInformation("Calling CreateAbscondByLead method in AssociateExitService");

                var (employeeDtls, abscondDtls) = await GetEmployeeAbscondDtls(abscondReq.AssociateId);

                if (employeeDtls != null && abscondDtls == null)
                {
                    var status = await m_OrgService.GetStatusByCategoryAndStatusCode(StatusCategory.AssociateExit.ToString(), AssociateExitStatusCodesNew.MarkedForAbscond.ToString());

                    //Assign abscond details
                    abscondDtls = new AssociateAbscond
                    {
                        AssociateId = abscondReq.AssociateId,
                        AbsentFromDate = abscondReq.AbsentFromDate,
                        AbsentToDate = abscondReq.AbsentToDate,
                        IsAbscond = false,
                        TLId = abscondReq.TLId,
                        RemarksByTL = string.IsNullOrEmpty(abscondReq.RemarksByTL) ? null : Utility.EncryptStringAES(abscondReq.RemarksByTL),
                        IsActive = true,
                        StatusId = status.Item.StatusId
                    };

                    await m_EmployeeContext.AssociateAbscond.AddAsync(abscondDtls);
                    int isCreated = await m_EmployeeContext.SaveChangesAsync();

                    if (isCreated > 0)
                    {
                        ServiceResponse<int> notification = await AssociateAbscondNotification(employeeDtls, abscondDtls, AssociateExitStatusCodesNew.MarkedForAbscond.ToString());
                        if (!notification.IsSuccessful)
                        {
                            response.Item = false;
                            response.IsSuccessful = false;
                            response.Message = notification.Message;
                            return response;
                        }

                        response.Item = true;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Associate Abscond Request Not Inserted.";
                        return response;
                    }
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Associate Abscond Request Already Exist.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to create Associate Abscond.";

                m_Logger.LogError("Error occured in Create method." + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region AcknowledgeAbscond
        /// <summary>
        /// AcknowledgeAbscond        
        /// <param name="abscondReq">abscondReq</param>
        /// <returns></returns>
        /// </summary>
        public async Task<ServiceResponse<bool>> AcknowledgeAbscond(AssociateAbscondModel abscondReq)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                m_Logger.LogInformation("Calling AcknowledgeAbscond method in AssociateAbscondService");

                var (employeeDtls, abscondDtls) = await GetEmployeeAbscondDtls(abscondReq.AssociateId);

                if (employeeDtls != null && abscondDtls != null)
                {
                    var status = await m_OrgService.GetStatusByCategoryAndStatusCode(StatusCategory.AssociateExit.ToString(), AssociateExitStatusCodesNew.AbscondAcknowledged.ToString());

                    //Updating Abscond details
                    abscondDtls.IsAbscond = false;
                    abscondDtls.StatusId = status.Item.StatusId;
                    abscondDtls.HRAId = abscondReq.HRAId;
                    abscondDtls.RemarksByHRA = string.IsNullOrEmpty(abscondReq.RemarksByHRA) ? null : Utility.EncryptStringAES(abscondReq.RemarksByHRA);

                    int isUpdated = await m_EmployeeContext.SaveChangesAsync();
                    if (isUpdated > 0)
                    {
                        ServiceResponse<int> notification = await AssociateAbscondNotification(employeeDtls, abscondDtls, AssociateExitStatusCodesNew.AbscondAcknowledged.ToString());
                        if (!notification.IsSuccessful)
                        {
                            response.Item = false;
                            response.IsSuccessful = false;
                            response.Message = notification.Message;
                            return response;
                        }

                        response.Item = true;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Associate Abscond Request Not Updated.";
                        return response;
                    }
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Rquested Details Not Found.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to acknowledge Abscond request.";

                m_Logger.LogError("Error occured in AcknowledgeAbscond method." + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region ConfirmAbscond
        /// <summary>
        /// ConfirmAbscond
        /// </summary>
        /// <param name="abscondReq">abscondReq</param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> ConfirmAbscond(AssociateAbscondModel abscondReq)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                m_Logger.LogInformation("Calling ConfirmAbscond method in AssociateAbscondService");

                var (employeeDtls, abscondDtls) = await GetEmployeeAbscondDtls(abscondReq.AssociateId);

                if (employeeDtls != null && abscondDtls != null)
                {
                    //Getting status details
                    var status = await m_OrgService.GetStatusByCategoryAndStatusCode(StatusCategory.AssociateExit.ToString(),
                        abscondReq.IsAbscond ? AssociateExitStatusCodesNew.AbscondConfirmed.ToString() : AssociateExitStatusCodesNew.AbscondDisproved.ToString());

                    //Updating Abscond details
                    abscondDtls.HRMId = abscondReq.HRMId;
                    abscondDtls.RemarksByHRM = string.IsNullOrEmpty(abscondReq.RemarksByHRM) ? null : Utility.EncryptStringAES(abscondReq.RemarksByHRM);
                    abscondDtls.IsAbscond = abscondReq.IsAbscond;
                    abscondDtls.StatusId = status.Item.StatusId;

                    int isUpdated = await m_EmployeeContext.SaveChangesAsync();
                    if (isUpdated > 0 && abscondReq.IsAbscond)
                    {
                        //Remove UserRoles
                        var removeUserRole = await m_OrgService.RemoveUserRoleOnExit(employeeDtls.UserId.Value);

                        //Inactivate the user
                        var inactiveUser = await m_OrgService.UpdateUser(employeeDtls.UserId.Value);

                        //Create Exit Activity
                        var createActivity = await m_AssociateExitActivityService.CreateActivityChecklist(abscondReq.AssociateId, 0);
                        if (createActivity.IsSuccessful)
                        {
                            ServiceResponse<int> notification = await AssociateAbscondNotification(employeeDtls, abscondDtls, AssociateExitStatusCodesNew.AbscondConfirmed.ToString());
                            if (!notification.IsSuccessful)
                            {
                                response.Item = false;
                                response.IsSuccessful = false;
                                response.Message = notification.Message;
                                return response;
                            }
                        }

                        response.Item = true;
                        response.IsSuccessful = true;
                    }
                    else if (isUpdated > 0 && !abscondReq.IsAbscond)
                    {
                        ServiceResponse<int> notification = await AssociateAbscondNotification(employeeDtls, abscondDtls, AssociateExitStatusCodesNew.AbscondDisproved.ToString());
                        if (!notification.IsSuccessful)
                        {
                            response.Item = false;
                            response.IsSuccessful = false;
                            response.Message = notification.Message;
                            return response;
                        }

                        response.Item = true;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Associate Abscond Request Not Updated.";
                        return response;
                    }
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Rquested Details Not Found.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to confirm/disprove the Abscond request.";

                m_Logger.LogError("Error occured in ConfirmAbscond method." + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region AbscondClearance
        /// <summary>
        /// AbscondClearance
        /// </summary>
        /// <param name="abscondReq">abscondReq</param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> AbscondClearance(AssociateAbscondModel abscondReq)
        {
            int isUpdated = 0;
            var response = new ServiceResponse<bool>();
            try
            {
                m_Logger.LogInformation("Calling AbscondClearance method in AssociateAbscondService");

                var (employeeDtls, abscondDtls) = await GetEmployeeAbscondDtls(abscondReq.AssociateId);

                if (employeeDtls != null && abscondDtls != null)
                {
                    //Getting status details
                    var status = await m_OrgService.GetStatusesByCategoryName(StatusCategory.AssociateExit.ToString());

                    //Updating Abscond details
                    abscondDtls.RemarksByHRM = string.IsNullOrEmpty(abscondReq.RemarksByHRM) ? null : Utility.EncryptStringAES(abscondReq.RemarksByHRM);
                    abscondDtls.IsActive = false;
                    abscondDtls.StatusId = status.Items.FirstOrDefault(w => w.StatusId == Convert.ToInt32(AssociateExitStatusCodesNew.Absconded)).StatusId;

                    //Update allocation table
                    var releaseOnExit = await m_ProjectService.ReleaseOnExit(employeeDtls.EmployeeId, abscondDtls.AbsentFromDate.ToShortDateString());

                    //Update employee
                    employeeDtls.IsActive = false;
                    employeeDtls.StatusId = status.Items.FirstOrDefault(w => w.StatusId == Convert.ToInt32(AssociateExitStatusCodesNew.Blacklisted)).StatusId;
                    employeeDtls.UserId = null;

                    isUpdated = await m_EmployeeContext.SaveChangesAsync();
                    if (isUpdated > 0)
                    {
                        ServiceResponse<int> notification = await AssociateAbscondNotification(employeeDtls, abscondDtls, AssociateExitStatusCodesNew.Absconded.ToString());
                        if (!notification.IsSuccessful)
                        {
                            response.Item = false;
                            response.IsSuccessful = false;
                            response.Message = notification.Message;
                            return response;
                        }

                        response.Item = true;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Associate Abscond Request not updated.";
                        return response;
                    }
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Rquested Details Not Found.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to give clearance the Abscond request.";

                m_Logger.LogError("Error occured in AbscondClearance method." + ex.StackTrace);
            }
            return response;
        }
        #endregion 

        #region AssociateAbscondWFStatus
        /// <summary>
        /// Get AssociateAbscond Department Work Flow Status
        /// </summary>
        /// <param name="associateId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<AssociateAbscondWFStatus>> AssociateAbscondWFStatus(int associateId)
        {
            m_Logger.LogInformation("Calling AssociateAbscondWFStatus method in AssociateAbscondService");

            var response = new ServiceResponse<AssociateAbscondWFStatus>();
            AssociateAbscondWFStatus abscondWFStatus = new AssociateAbscondWFStatus();
            int associateAbscondId;
            Status statusItem;

            try
            {
                var associateAbscond = await m_EmployeeContext.AssociateAbscond.SingleOrDefaultAsync(pa => pa.AssociateId == associateId && pa.IsActive == true);

                if (associateAbscond != null)
                {
                    associateAbscondId = associateAbscond.AssociateAbscondId;

                    var exitStatusMaster = await m_OrgService.GetStatusesByCategoryName(CategoryMaster.AssociateExit.ToString());
                    statusItem = exitStatusMaster.Items.SingleOrDefault(x => x.StatusId == associateAbscond.StatusId);

                    abscondWFStatus.AssociateExitId = associateAbscond.AssociateAbscondId;
                    abscondWFStatus.AssociateId = associateAbscond.AssociateId;
                    abscondWFStatus.AssociateExitStatusCode = statusItem.StatusCode;
                    abscondWFStatus.AssociateExitStatusDesc = statusItem.StatusDescription;

                    if (associateAbscond.StatusId == Convert.ToInt32(AssociateExitStatusCodesNew.AbscondConfirmed))
                    {
                        List<ActivityStatus> activityStatuses = new List<ActivityStatus>();

                        var deptActivitesResult = await m_EmployeeContext.AssociateExitActivity.Where(x => x.AssociateAbscondId == associateAbscondId).ToListAsync();

                        if (deptActivitesResult?.Count > 0)
                        {
                            foreach (var activity in deptActivitesResult)
                            {
                                statusItem = exitStatusMaster.Items.Where(x => x.StatusId == activity.StatusId).Select(y => y).FirstOrDefault();

                                activityStatuses.Add(new ActivityStatus
                                {
                                    DepartmentId = activity.DepartmentId,
                                    ActivityStatusCode = statusItem.StatusCode,
                                    ActivityStatusDesc = statusItem.StatusDescription,
                                    IsCompleted = activity.StatusId == Convert.ToInt32(AssociateExitStatusCodesNew.DepartmentActivityCompleted)
                                });
                            }
                        }
                        else
                        {
                            var activites = await m_OrgService.GetExitActivitiesByDepartment();
                            var departments = activites?.Items.Select(st => st.DepartmentId).Distinct().ToList();

                            if (departments != null)
                            {
                                foreach (var departmentId in departments)
                                {
                                    activityStatuses.Add(new ActivityStatus
                                    {
                                        DepartmentId = departmentId,
                                        ActivityStatusCode = "ActivityNotYetStarted",
                                        ActivityStatusDesc = "Activity Not Yet Started",
                                        IsCompleted = false
                                    });
                                }
                            }
                        }

                        abscondWFStatus.ActivitiesSubStatus = activityStatuses;
                    }

                    response.Item = abscondWFStatus;
                    response.IsSuccessful = true;
                    response.Message = "Associate Abscond Status and Sub-Status fetched successfully";
                }
                else
                {
                    response.Item = null;
                    response.IsSuccessful = false;
                    response.Message = "Associate Abscond not found";
                }
            }
            catch (Exception ex)
            {
                response.Item = null;
                response.IsSuccessful = false;
                response.Message = $"Error Occured while Fetching Associate Abscond Status and Sub-Status.";

                m_Logger.LogError("Error occured in AssociateAbscondWFStatus method." + ex.StackTrace);
            }

            return response;
        }
        #endregion

        #region Private Methods

        #region AssociateAbscondNotification
        /// <summary>
        /// AssociateAbscondNotification
        /// <param name = "associateId" > associateId </ param >
        /// <param name="statusType">notificationType</param>
        /// < returns ></ returns >
        /// </summary>
        private async Task<ServiceResponse<int>> AssociateAbscondNotification(Entities.Employee employee, AssociateAbscond abscondDetails, string statusType)
        {
            #region Variables
            ServiceResponse<int> response = new ServiceResponse<int>();
            string empStatus = string.Empty;
            string empDesignation = string.Empty;
            string empDepartment = string.Empty;
            string tlEmail = string.Empty;
            string programManagerMail = string.Empty;
            string departmentCodes = string.Empty;

            string hraEmail = string.Empty;
            string hrDLEmailAddress = string.Empty;

            string fromEmailAddress = string.Empty;
            string toEmailAddress = string.Empty;
            string ccEmailAddress = string.Empty;
            string mailSubject = string.Empty;            
            string templateFilePath = null;
            string templateBody = null;
            NotificationDetail activityMailObj = null;
            #endregion

            try
            {
                m_Logger.LogInformation("Calling \"AssociateAbscondNotification\" method in AssociateAbscond Service");

                bool isSendEmail = m_EmailConfigurations.SendEmail;
                if (!isSendEmail)
                {
                    m_Logger.LogInformation("SendEmail configured to false. Returning back without sending any notification");

                    response.Item = 0;
                    response.IsSuccessful = true;
                    response.Message = "Notification not sent as per SendEmail configuration";
                    return response;
                }

                #region ReadConfigSection

                fromEmailAddress = m_EmailConfigurations.FromEmail;
                departmentCodes = m_EmailConfigurations.DeptCodes;

                #endregion

                #region GettingMailDetails   

                if (abscondDetails == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "Employee Abscond Details Not Found";
                    return response;
                }

                var designationDetails = m_OrgService.GetDesignationById(employee.DesignationId ?? 0);
                var departmentDetails = m_OrgService.GetDepartmentById(employee.DepartmentId ?? 0);
                var statusDetails = m_OrgService.GetStatusByCategoryAndStatusCode(StatusCategory.AssociateExit.ToString(), statusType);

                await Task.WhenAll(designationDetails, departmentDetails, statusDetails).ConfigureAwait(false);

                var designationsResult = designationDetails.GetAwaiter().GetResult();
                var departmentsResult = departmentDetails.GetAwaiter().GetResult();
                var statusResult = statusDetails.GetAwaiter().GetResult();

                if (!designationsResult.IsSuccessful)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "Designation Details Not Found";
                    return response;
                }

                empDesignation = designationsResult.Item.DesignationName;

                if (!departmentsResult.IsSuccessful)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "Department Details Not Found";
                    return response;
                }

                empDepartment = departmentsResult.Item.DepartmentCode;

                if (!statusResult.IsSuccessful)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "Abscond Status Details Not Found";
                    return response;
                }

                empStatus = statusResult.Item.StatusDescription;

                //HRM Mail
                var (hrmId, hrmEmail) = await GetHRMId();

                //HRA Mail
                hraEmail = await GetWorkMailbyEmpId(abscondDetails.HRAId ?? 0);

                //Lead Mail
                tlEmail = await GetWorkMailbyEmpId(abscondDetails.TLId ?? 0);

                ServiceListResponse<AssociateAllocation> allocation = new ServiceListResponse<AssociateAllocation>();
                //Getting Team Lead Email
                if (empDepartment.ToLower() == "delivery")
                {
                    allocation = await m_ProjectService.GetAssociateAllocationsByEmployeeId(employee.EmployeeId);

                    if (allocation.Items != null && allocation.IsSuccessful)
                    {
                        var associateAllocation = allocation.Items.FirstOrDefault(x => x.IsPrimary == true);
                        programManagerMail = await GetWorkMailbyEmpId(associateAllocation.ProgramManagerId ?? 0);
                    }
                }
                else
                {
                    programManagerMail = await GetWorkMailbyEmpId(employee.ReportingManager ?? 0); //TODO: validate in non-delivery dept and for program manager and above associates
                }

                //Approval Dept Details
                var departmentDLs = await m_OrgService.GetAllDepartmentsWithDLs();
                if (departmentDLs.IsSuccessful && departmentDLs.Items != null)
                {
                    hrDLEmailAddress = departmentDLs.Items?.FirstOrDefault(x => x.DepartmentCode == "HR")?.DepartmentDLAddress;
                }

                #endregion

                #region EmailConfig
                templateFilePath = Utility.GetNotificationTemplatePath(NotificationTemplatePaths.currentDirectory,NotificationTemplatePaths.subDirectories_Associate_Abscond);                               
                templateBody = GetNotificationTemplate(templateFilePath);

                //Replace email content with current employee details
                templateBody = templateBody.Replace("{AssociateName}", $"{employee.FirstName} {employee.LastName}")
                                   .Replace("{Designation}", empDesignation)
                                   .Replace("{Department}", empDepartment)
                                   .Replace("{AssociateCode}", employee.EmployeeCode)
                                   .Replace("{AbsentFromDate}", abscondDetails.AbsentFromDate.ToShortDateString())
                                   .Replace("{AbsentToDate}", abscondDetails.AbsentToDate.ToShortDateString())
                                   .Replace("{AbscondStatus}", empStatus)
                                   .Replace("{NoteToHRDL}", $"<p>Please find below the details of the associate.</p>");
                #endregion

                #region MarkedForAbscondNotification

                if (statusType == AssociateExitStatusCodesNew.MarkedForAbscond.ToString())
                {
                    toEmailAddress = $"{hrDLEmailAddress}";
                    ccEmailAddress = $"{hrmEmail};{programManagerMail};{tlEmail}";
                    mailSubject = m_exitMailSubjects.MarkedForAbscond.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region AcknowledgeAbscondNotification

                if (statusType == AssociateExitStatusCodesNew.AbscondAcknowledged.ToString())
                {
                    toEmailAddress = $"{hrmEmail};";
                    ccEmailAddress = $"{hraEmail};{hrDLEmailAddress};{programManagerMail};{tlEmail}";
                    mailSubject = m_exitMailSubjects.AcknowledgeAbscond.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region ConfirmAbscondNotification

                if (statusType == AssociateExitStatusCodesNew.AbscondConfirmed.ToString())
                {
                    toEmailAddress = $"{hraEmail};{tlEmail}";
                    ccEmailAddress = $"{hrDLEmailAddress};{programManagerMail};{hrmEmail}";
                    mailSubject = m_exitMailSubjects.ConfirmAbscond.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);

                    string deptDLEmail = string.Empty;

                    var activityDeptDetails = await m_EmployeeContext.AssociateExitActivity.Where(w => w.AssociateAbscondId == abscondDetails.AssociateAbscondId && w.IsActive == true).Select(x => x).ToListAsync();
                    if (activityDeptDetails == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Email To cannot be blank";
                        return response;
                    }

                    foreach (AssociateExitActivity dept in activityDeptDetails)
                    {
                        deptDLEmail += $"{departmentDLs.Items?.FirstOrDefault(x => x.DepartmentId == dept.DepartmentId)?.DepartmentDLAddress};";
                    }

                    // HR Talent Acquisition 
                    var hrTagTeamEmail = departmentDLs.Items?.FirstOrDefault(x => x.DepartmentCode == "HR TAG")?.DepartmentDLAddress;

                    activityMailObj = new NotificationDetail
                    {
                        FromEmail = fromEmailAddress,
                        ToEmail = $"{deptDLEmail};{hrTagTeamEmail}",
                        CcEmail = $"{hrmEmail};{hraEmail}",
                        Subject = m_exitMailSubjects.ActivitiesInProgressNotification.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName),
                        EmailBody = templateBody.Replace("{loginURL}", "<p>Please login to <a href='https://hrms.senecaglobal.com'>https://hrms.senecaglobal.com</a> and complete assigned activities.</p>")
                    };
                }

                #endregion

                #region DisproveAbscondNotification

                if (statusType == AssociateExitStatusCodesNew.AbscondDisproved.ToString())
                {
                    toEmailAddress = $"{hraEmail};{tlEmail}";
                    ccEmailAddress = $"{hrDLEmailAddress};{programManagerMail};{hrmEmail}";
                    mailSubject = m_exitMailSubjects.DisproveAbscond.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region AbscondedNotification

                if (statusType == AssociateExitStatusCodesNew.Absconded.ToString())
                {
                    toEmailAddress = $"{hraEmail};{tlEmail}";
                    ccEmailAddress = $"{hrDLEmailAddress};{programManagerMail};{hrmEmail}";
                    mailSubject = m_exitMailSubjects.Absconded.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region RemoveUnNecessaryPlaceHolders

                //These fields will be replaced in respective NotificationType section. If these placeholders survived till this point, which means they can be cleared from mail body.
                if (templateBody.Contains("{NoteToHRDL}") || templateBody.Contains("{loginURL}"))
                {
                    templateBody = templateBody.Replace("{NoteToHRDL}", "").Replace("{loginURL}", "");
                }

                #endregion

                // Validate email Notification details
                if (string.IsNullOrEmpty(fromEmailAddress))
                {
                    response.IsSuccessful = false;
                    response.Message = "Email From cannot be blank";
                    return response;
                }

                // Validate email Notification details
                if (string.IsNullOrEmpty(toEmailAddress))
                {
                    response.IsSuccessful = false;
                    response.Message = "Email To cannot be blank";
                    return response;
                }

                NotificationDetail mailObj = new NotificationDetail
                {
                    FromEmail = fromEmailAddress,
                    ToEmail = toEmailAddress,
                    CcEmail = string.IsNullOrEmpty(ccEmailAddress) ? string.Empty : ccEmailAddress,
                    Subject = mailSubject,
                    EmailBody = templateBody
                };

                var emailStatus = await m_OrgService.SendEmail(mailObj);

                if (emailStatus.IsSuccessful && activityMailObj != null)
                {
                    emailStatus = await m_OrgService.SendEmail(activityMailObj);
                }

                if (!emailStatus.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = $"Error occurred while sending email. Message: {emailStatus.Message}";
                    return response;
                }

                response.IsSuccessful = true;
                response.Item = 1;
                return response;
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in AssociateAbscondNotification.");
                m_Logger.LogError(ex, ex.GetBaseException().Message);
                m_Logger.LogError(ex, ex.GetBaseException().StackTrace);

                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Exception occured in AssociateAbscondNotification."
                };
            }
        }
        #endregion

        #region GetHRMDtls
        private async Task<(int hrmId, string hrmEmail)> GetHRMId()
        {
            var hrDesgDetails = await m_OrgService.GetDesignationByCode(m_EmailConfigurations.HRManagerRole);

            var hrmDtls = await m_EmployeeContext.Employees.Where(e => e.DesignationId == hrDesgDetails.Item.DesignationId)
                          .Select(x => new { x.EmployeeId, x.WorkEmailAddress }).FirstOrDefaultAsync();

            return (hrmDtls.EmployeeId, hrmDtls?.WorkEmailAddress);
        }
        #endregion

        #region GetWorkEmailDtls
        private async Task<string> GetWorkMailbyEmpId(int empId)
        {
            return (empId > 0) ? await m_EmployeeContext.Employees.Where(e => e.EmployeeId == empId && e.IsActive == true).Select(x => x.WorkEmailAddress)?.FirstOrDefaultAsync() : string.Empty;
        }
        #endregion

        #region GetNotifiationTemplate
        private string GetNotificationTemplate(string templatePath)
        {
            string mailBody = null;

            try
            {
                if (!string.IsNullOrEmpty(templatePath))
                {
                    string filePath = Environment.CurrentDirectory + templatePath;
                    using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    using var stream = new StreamReader(fs, Encoding.UTF8);

                    mailBody = stream.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex, ex.GetBaseException().Message);
                throw ex;
            }

            return mailBody;
        }
        #endregion

        #region IsBase64
        private static bool IsBase64(string inputString)
        {
            if (inputString.Replace(" ", "").Length % 4 != 0)
            {
                return false;
            }

            try
            {
                Convert.FromBase64String(inputString);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        #endregion

        #region GetEmployeeAbscondDtls
        private async Task<(Entities.Employee employeeDtls, AssociateAbscond abscondDtls)> GetEmployeeAbscondDtls(int associateId)
        {
            Entities.Employee employeeDtls = null;
            AssociateAbscond abscondDtls = null;

            if (associateId != 0)
            {
                employeeDtls = await m_EmployeeContext.Employees.FirstOrDefaultAsync(n => n.EmployeeId == associateId && n.IsActive == true);
                abscondDtls = await m_EmployeeContext.AssociateAbscond.FirstOrDefaultAsync(n => n.AssociateId == associateId && n.IsActive == true);
            }

            return (employeeDtls, abscondDtls);
        }
        #endregion

        #endregion
    }
}

