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
    /// Service class to create associate long leave
    /// To calculate maternity period, if long leave type is maternity
    /// To rejoin associate
    /// </summary>
    public class AssociateLongLeaveService : IAssociateLongLeaveService
    {
        #region Global Varibles

        private readonly ILogger<AssociateLongLeaveService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IEmployeeService m_employeeService;
        private readonly IOrganizationService m_OrgService;
        private readonly IProjectService m_projectService;

        #endregion

        #region Constructor
        public AssociateLongLeaveService(EmployeeDBContext employeeDBContext,
                                           ILogger<AssociateLongLeaveService> logger,
                                           IEmployeeService employeeService,
                                           IOrganizationService organizationService,
                                           IProjectService projectService)
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            m_employeeService = employeeService;
            m_OrgService = organizationService;
            m_projectService = projectService;
        }
        #endregion

        #region CreateAssociateLongLeave
        /// <summary>
        /// CreateAssociateLongLeave
        /// </summary>
        /// <param name="resignationDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> CreateAssociateLongLeave(AssociateLongLeaveData leaveDetails)
        {
            int isCreated = 0;
            var response = new ServiceResponse<bool>();
            m_Logger.LogInformation("AssociateLongLeaveService: Calling \"CreateAssociateLongLeave\" method.");
            try
            {
                var longLeaveDetails = m_EmployeeContext.AssociateLongLeaves
                                                                 .Where(id => id.EmployeeId == leaveDetails.EmployeeId && id.IsActive == true)
                                                                 .ToList();

                //Make the long leave details inactive.
                longLeaveDetails.ForEach(d => d.IsActive = false);

                //Get the employee Resign status.
                var longLeaveStatus = await m_OrgService.GetStatusByCategoryAndStatusCode(StatusCategory.LongLeave.ToString(),
                                                                    TalentRequisitionStatusCodes.LongLeave.ToString());
                if (!longLeaveStatus.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = longLeaveStatus.Message;
                    return response;
                }

                if (leaveDetails != null)
                {
                    var longLeave = new AssociateLongLeave();
                    longLeave.EmployeeId = leaveDetails.EmployeeId;
                    longLeave.IsMaternity = leaveDetails.IsMaternity;
                    longLeave.Reason = leaveDetails.Reason;
                    longLeave.LongLeaveStartDate = Convert.ToDateTime(leaveDetails.LongLeaveStartDate);
                    longLeave.TentativeJoinDate = Convert.ToDateTime(leaveDetails.TentativeJoinDate);
                    longLeave.StatusId = longLeaveStatus.Item.StatusId;
                    longLeave.IsActive = true;

                    //add the leaveDetails to list.
                    m_EmployeeContext.AssociateLongLeaves.Add(longLeave);

                }

                m_Logger.LogInformation("Calling SaveChanges method on DB Context in AssociateLongLeaveService");
                isCreated = await m_EmployeeContext.SaveChangesAsync();
                if (isCreated > 0)
                {
                    await SendLongLeaveNotification(leaveDetails.EmployeeId, StringConstants.NotificationTypeLongLeave);
                    response.IsSuccessful = true;
                    response.Item = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while creating associate LongLeave";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while creating associate LongLeave";
                m_Logger.LogError("Error occurred in creating associate LongLeave" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region CalculateMaternityPeriod
        public async Task<ServiceResponse<string>> CalculateMaternityPeriod(string maternityStartDate)
        {
            var response = new ServiceResponse<string>();
            try
            {
                // Get Maternity period
                var iMaternityPeriod = await m_employeeService.GetBusinessValues(StringConstants.NotificationTypeLongLeave);
                DateTime tentativeJoinDate = Convert.ToDateTime(maternityStartDate).AddMonths(Convert.ToInt32(iMaternityPeriod.Items.FirstOrDefault().ValueId));

                if (tentativeJoinDate == DateTime.MinValue)
                {
                    response.Item = null;
                    response.IsSuccessful = false;
                    response.Message = "failed to calculate tentative join date";
                }
                else
                {
                    response.IsSuccessful = true;
                    response.Item = tentativeJoinDate.ToString("yyyy-MM-dd");
                }
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while calculating tentative join date";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region RejoinAssociateByID
        /// <summary>
        /// RejoinAssociateByID
        /// </summary>
        /// <param name="empID"></param>
        /// <param name="reason"></param>
        /// <param name="rejoinedDate"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> ReJoinAssociateByID(int empID, string reason, string rejoinedDate)
        {
            int isUpdated = 0;
            var response = new ServiceResponse<bool>();
            m_Logger.LogInformation("AssociateLongLeaveService: Calling \"CreateAssociateLongLeave\" method.");
            try
            {
                var leaveDetails = m_EmployeeContext.AssociateLongLeaves
                                                    .Where(id => id.EmployeeId == empID && id.IsActive == true)
                                                    .ToList();

                //Make the leave details inactive.
                leaveDetails.ForEach(d => { d.IsActive = false; d.Reason = reason; });

                m_Logger.LogInformation("Calling SaveChanges method on DB Context in AssociateLongLeaveService");
                isUpdated = await m_EmployeeContext.SaveChangesAsync();
                if (isUpdated > 0)
                {
                    await SendLongLeaveNotification(empID, StringConstants.NotificationTypeRejoin, rejoinedDate);
                    response.IsSuccessful = true;
                    response.Item = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while rejoining associate";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred  while rejoining associate";
                m_Logger.LogError("Error occurred  while rejoining associate" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        public async Task<ServiceResponse<bool>> SendLongLeaveNotification(int employeeId, string notificationType, string rejoinedDate = null)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var emailNotificationConfig = new ServiceResponse<NotificationConfiguration>();
                AssociateLongLeave associateLongLeave = new AssociateLongLeave();

                if (notificationType == StringConstants.NotificationTypeRejoin)
                {
                    emailNotificationConfig = await m_OrgService.GetByNotificationTypeAndCategory((int)NotificationType.Rejoin, (int)CategoryMaster.LongLeave);
                    if (!emailNotificationConfig.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = emailNotificationConfig.Message;
                        return response;
                    }
                }
                else
                {
                    emailNotificationConfig = await m_OrgService.GetByNotificationTypeAndCategory((int)NotificationType.LongLeave, (int)CategoryMaster.LongLeave);
                    if (!emailNotificationConfig.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = emailNotificationConfig.Message;
                        return response;
                    }
                    associateLongLeave = m_EmployeeContext.AssociateLongLeaves.Where(e => e.EmployeeId == employeeId && e.IsActive == true).FirstOrDefault();
                }

                NotificationDetail notificationDetail = new NotificationDetail();
                StringBuilder emailContent = new StringBuilder(WebUtility.HtmlDecode(emailNotificationConfig.Item.EmailContent));
                var programManagerId = 0;
                ServiceListResponse<Project> project = null;
                Associate.Employee emp = m_EmployeeContext.Employees.Where(e => e.EmployeeId == employeeId).FirstOrDefault();
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
                    else
                    {
                        emailContent = emailContent.Replace("@AssociateName", string.Concat(emp.FirstName, " ", emp.LastName))
                         .Replace("@ProjectName",  "N/A");
                    }

                    if (emailNotificationConfig != null)
                    {
                        string subject = emailNotificationConfig.Item.EmailSubject;
                        subject = subject.Replace("@AssociateName", string.Concat(emp.FirstName, " ", emp.LastName));

                        if (notificationType == StringConstants.NotificationTypeRejoin)
                        {
                            emailContent = emailContent.Replace("@RejoinedDate", rejoinedDate ?? DateTime.UtcNow.ToString("dd-MM-yyyy"));
                        }
                        else
                        {
                            if (associateLongLeave != null)
                            {
                                emailContent = emailContent
                                                    .Replace("@LongLeaveStartDate", associateLongLeave.LongLeaveStartDate.ToString("dd-MM-yyyy"))
                                                    .Replace("@TentativeJoinDate", associateLongLeave.TentativeJoinDate.ToString("dd-MM-yyyy"));
                            }
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
                        else 
                        { 
                            sbToaddress.Append(emailNotificationConfig.Item.EmailTo).Append(";");
                        }

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
