using AutoMapper;
using HRMS.Common.Enums;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Constants;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Associate = HRMS.Employee.Entities;

namespace HRMS.Employee.Service
{
    /// <summary>
    /// Service class to create associate resignation
    /// To get associates for resignation
    /// To revoke resignation
    /// </summary>
    public class AssociateResignationService : IAssociateResignationService
    {
        #region Global Varibles

        private readonly ILogger<AssociateResignationService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IProjectService m_projectService;
        private readonly IEmployeeService m_employeeService;
        private readonly IOrganizationService m_OrgService;

        #endregion

        #region Constructor
        public AssociateResignationService(EmployeeDBContext employeeDBContext,
                                           ILogger<AssociateResignationService> logger,
                                           IProjectService projectService,
                                           IEmployeeService employeeService,
                                           IOrganizationService organizationService)
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            m_projectService = projectService;
            m_employeeService = employeeService;
            m_OrgService = organizationService;
        }

        #endregion

        #region GetAssociatesBySearchString
        public async Task<ServiceResponse<AssociateResignationData>> GetAssociatesBySearchString(int resignEmployeeId, int employeeID)
        {
            var response = new ServiceResponse<AssociateResignationData>();
            try
            {
                ServiceListResponse<Project> project = null;
                int programManagerId = 0;

                // get employee project by emp id
                var associateProject = await m_projectService.GetEmployeeProjectIdByEmpId(resignEmployeeId);
                if (associateProject.Item != null)
                {
                    project = await m_projectService.GetProjectById(new List<int> { associateProject.Item.ProjectId.Value });

                    // get project Managers
                    var projectManagers = await m_projectService.GetActiveProjectManagers();

                    if (projectManagers != null)
                    {
                        programManagerId = (from pm in projectManagers.Items
                                            where pm.ProjectId == associateProject.Item.ProjectId && pm.IsActive == true
                                            select pm.ProgramManagerId.Value).FirstOrDefault();
                    }
                }

                List<Employee.Entities.Employee> employees = m_EmployeeContext.Employees.ToList();

                var associateResignation = (from ar in m_EmployeeContext.AssociateResignations
                                            where ar.IsActive == true && ar.EmployeeId == resignEmployeeId
                                            select ar).FirstOrDefault();

                var associateLongLeave = (from al in m_EmployeeContext.AssociateLongLeaves
                                          where al.IsActive == true && al.EmployeeId == resignEmployeeId
                                          select al).FirstOrDefault();

                var associate = (from e in m_EmployeeContext.Employees.ToList()
                                 where e.IsActive == true && e.EmployeeId != employeeID && e.EmployeeId == resignEmployeeId
                                 select new AssociateResignationData
                                 {
                                     EmployeeId = e.EmployeeId,
                                     EmployeeName = e.FirstName + " " + e.LastName,
                                     Gender = e.Gender,
                                     ReportingManagerName = (associateProject.Item != null) ? m_employeeService.GetEmployeeName(associateProject.Item.ReportingManagerId.Value) : null,
                                     ProgramManagerName = (programManagerId != 0) ? m_employeeService.GetEmployeeName(programManagerId) : null,
                                     ProjectName = (project != null) ? project.Items.FirstOrDefault().ProjectName : null,
                                     IsBillable = (associateProject.Item != null) ? associateProject.Item.IsBillable : null,
                                     IsCritical = (associateProject.Item != null) ? associateProject.Item.IsCritical : null,
                                     ResignationDate = associateResignation == null ? null : string.Format("{0:yyyy-MM-dd}", associateResignation.ResignationDate),
                                     LastWorkingDate = associateResignation == null ? null : string.Format("{0:yyyy-MM-dd}", associateResignation.LastWorkingDate),
                                     ReasonDescription = associateResignation == null ? null : associateResignation.ReasonDescription,
                                     IsResigned = associateResignation == null ? null : associateResignation.IsActive != null ? associateResignation.IsActive : false,
                                     IsLongLeave = associateLongLeave == null ? null : associateLongLeave.IsActive != null ? associateLongLeave.IsActive : false,
                                     Reason = associateLongLeave == null ? null : associateLongLeave.Reason,
                                     LongLeaveStartDate = associateLongLeave == null ? null : string.Format("{0:yyyy-MM-dd}", associateLongLeave.LongLeaveStartDate),
                                     TentativeJoinDate = associateLongLeave == null ? null : string.Format("{0:yyyy-MM-dd}", associateLongLeave.TentativeJoinDate),
                                 }).FirstOrDefault();

                if (associate != null)
                {
                    response.IsSuccessful = true;
                    response.Item = associate;
                }
                else
                {
                    response.Item = null;
                    response.IsSuccessful = false;
                    response.Message = "No data found..";
                }
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching employees";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region CreateAssociateResignation
        /// <summary>
        /// CreateAssociateResignation
        /// </summary>
        /// <param name="resignationDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> CreateAssociateResignation(AssociateResignationData resignationDetails)
        {
            int isCreated = 0;
            var response = new ServiceResponse<bool>();
            m_Logger.LogInformation("AssociateResignationService: Calling \"CreateAssociateResignation\" method.");
            try
            {
                var resigDetails = m_EmployeeContext.AssociateResignations
                                                                 .Where(id => id.EmployeeId == resignationDetails.EmployeeId && id.IsActive == true)
                                                                 .ToList();

                //Make the resignation details inactive.
                if (resigDetails != null)
                {
                    resigDetails.ForEach(d => d.IsActive = false);
                }

                //Get the employee Resign status.
                var resignStatus = await m_OrgService.GetStatusByCategoryAndStatusCode(StatusCategory.AssociateExit.ToString(),
                                                                    AssociateExitStatusCodes.Resigned.ToString());
                if (!resignStatus.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = resignStatus.Message;
                    return response;
                }

                if (resignationDetails != null)
                {
                    var resignDetails = new AssociateResignation();
                    resignDetails.EmployeeId = resignationDetails.EmployeeId;
                    resignDetails.ReasonId = 0;
                    resignDetails.ReasonDescription = resignationDetails.ReasonDescription;
                    resignDetails.ResignationDate = Convert.ToDateTime(resignationDetails.ResignationDate);
                    resignDetails.LastWorkingDate = Convert.ToDateTime(resignationDetails.LastWorkingDate);
                    resignDetails.StatusId = resignStatus.Item.StatusId;
                    resignDetails.IsActive = true;

                    //add the resignDetails to list.
                    m_EmployeeContext.AssociateResignations.Add(resignDetails);

                }
                var employee = (from emp in m_EmployeeContext.Employees where emp.EmployeeId == resignationDetails.EmployeeId select emp).FirstOrDefault();
                employee.ResignationDate = Convert.ToDateTime(resignationDetails.ResignationDate);

                m_Logger.LogInformation("Calling SaveChanges method on DB Context in AssociateResignationService");
                isCreated = await m_EmployeeContext.SaveChangesAsync();
                if (isCreated > 0)
                {
                    await SendResignationNotification(resignationDetails.EmployeeId, StringConstants.NotificationTypeResignation);
                    response.IsSuccessful = true;
                    response.Item = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while creating associate resignation";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while creating associate resignation";
                m_Logger.LogError("Error occurred in creating associate resignation" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region CalculateNoticePeriod
        public async Task<ServiceResponse<string>> CalculateNoticePeriod(string resignationDate)
        {
            var response = new ServiceResponse<string>();
            try
            {
                // Get Notice period
                var iNoticePeriod = await m_employeeService.GetBusinessValues(StringConstants.NoticePeriod);
                DateTime lastWorkingDate = Convert.ToDateTime(resignationDate).AddMonths(Convert.ToInt32(iNoticePeriod.Items.FirstOrDefault().ValueId));

                if (lastWorkingDate == DateTime.MinValue)
                {
                    response.Item = null;
                    response.IsSuccessful = false;
                    response.Message = "failed to calculate last working date";
                }
                else
                {
                    response.IsSuccessful = true;
                    response.Item = lastWorkingDate.ToString("yyyy-MM-dd");
                }

                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching employees";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region RevokeResignationByID
        /// <summary>
        /// RevokeResignationByID
        /// </summary>
        /// <param name="empID"></param>
        /// <param name="reason"></param>
        /// <param name="revokedDate"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> RevokeResignationByID(int empID, string reason, string revokedDate)
        {
            int isUpdated = 0;
            var response = new ServiceResponse<bool>();
            m_Logger.LogInformation("AssociateResignationService: Calling \"CreateAssociateResignation\" method.");
            try
            {
                var resigDetails = m_EmployeeContext.AssociateResignations
                                                    .Where(id => id.EmployeeId == empID && id.IsActive == true)
                                                    .ToList();

                //Make the resignation details inactive.
                resigDetails.ForEach(d => { d.IsActive = false; d.ReasonDescription = reason; });

                var employee = (from emp in m_EmployeeContext.Employees where emp.EmployeeId == empID select emp).FirstOrDefault();
                employee.ResignationDate = null;

                m_Logger.LogInformation("Calling SaveChanges method on DB Context in AssociateResignationService");
                isUpdated = await m_EmployeeContext.SaveChangesAsync();
                if (isUpdated > 0)
                {
                    await SendResignationNotification(empID, StringConstants.NotificationTypeRevoke, revokedDate);
                    response.IsSuccessful = true;
                    response.Item = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while revoking resignation";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred  while revoking resignation";
                m_Logger.LogError("Error occurred  while revoking resignation" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        public async Task<ServiceResponse<bool>> SendResignationNotification(int employeeId, string notificationType, string revokedDate = null)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                ServiceResponse<NotificationConfiguration> emailNotificationConfig = null;
                AssociateResignation associateResignation = new AssociateResignation();

                if (notificationType == StringConstants.NotificationTypeRevoke)
                {
                    emailNotificationConfig = await m_OrgService.GetByNotificationTypeAndCategory((int)NotificationType.Revoke, (int)CategoryMaster.AssociateExit);
                    if (!emailNotificationConfig.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = emailNotificationConfig.Message;
                        return response;
                    }
                }
                else
                {
                    emailNotificationConfig = await m_OrgService.GetByNotificationTypeAndCategory((int)NotificationType.Resignation, (int)CategoryMaster.AssociateExit);
                    if (!emailNotificationConfig.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = emailNotificationConfig.Message;
                        return response;
                    }
                    associateResignation = m_EmployeeContext.AssociateResignations.Where(e => e.EmployeeId == employeeId && e.IsActive == true).FirstOrDefault();
                }

                NotificationDetail notificationDetail = new NotificationDetail();
                StringBuilder emailContent = new StringBuilder(WebUtility.HtmlDecode(emailNotificationConfig.Item.EmailContent));

                var programManagerId = 0;
                ServiceListResponse<Project> project = null;
                Associate.Employee emp = m_EmployeeContext.Employees.Where(e => e.EmployeeId == employeeId).FirstOrDefault(); ;
                // get employee project by emp id
                var associateProject = await m_projectService.GetEmployeeProjectIdByEmpId(employeeId);
                if (associateProject.Item != null)
                {
                    programManagerId = associateProject.Item.ProgramManagerId.Value;
                    project = await m_projectService.GetProjectById(new List<int> { associateProject.Item.ProjectId.Value });
                }

                if (emp != null)
                {
                    if (project != null)
                    {
                        emailContent = emailContent.Replace("@AssociateName", string.Concat(emp.FirstName, " ", emp.LastName))
                                                               .Replace("@ProjectName", project.Items != null ? project.Items.FirstOrDefault().ProjectName : "N/A");
                    }
                    if (emailNotificationConfig != null)
                    {
                        string subject = emailNotificationConfig.Item.EmailSubject;
                        subject = subject.Replace("@AssociateName", string.Concat(emp.FirstName, " ", emp.LastName));


                        if (notificationType == StringConstants.NotificationTypeRevoke)
                        {
                            emailContent = emailContent.Replace("@RevokedDate", revokedDate != null ? revokedDate : DateTime.UtcNow.ToString("dd-MM-yyyy"));
                        }
                        else
                        {
                            emailContent = emailContent.Replace("@ResignedDate", associateResignation.ResignationDate.ToString("dd-MM-yyyy"));
                        }

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
                        StringBuilder sbToaddress = new StringBuilder();
                        if (programManagerId != 0)
                        {
                            var programManagerEmail = m_EmployeeContext.Employees.Where(e => e.EmployeeId == programManagerId).Select(e => e.WorkEmailAddress).FirstOrDefault();
                            sbToaddress.Append(emailNotificationConfig.Item.EmailTo).Append(";").Append(programManagerEmail).Append(";");
                        }
                        else { sbToaddress.Append(emailNotificationConfig.Item.EmailTo).Append(";"); }
                        notificationDetail.ToEmail = sbToaddress.ToString();


                        StringBuilder sbCcaddress = new StringBuilder();
                        sbCcaddress.Append(emailNotificationConfig.Item.EmailCC).Append(";");
                        notificationDetail.CcEmail = sbCcaddress.ToString();

                        notificationDetail.Subject = subject;
                        notificationDetail.EmailBody = emailContent.ToString();
                    }
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
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while fetching employee details";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while sending email notification";
                m_Logger.LogError("Error occurred while sending email notification" + ex.StackTrace);
            }
            return response;
        }
    }
}
