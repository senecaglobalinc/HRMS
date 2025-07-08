using HRMS.Common.Enums;
using HRMS.KRA.Database;
using HRMS.KRA.Entities;
using HRMS.KRA.Infrastructure;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Infrastructure.Models.Domain;
using HRMS.KRA.Infrastructure.Models.Response;
using HRMS.KRA.Infrastructure.Response;
using HRMS.KRA.Types;
using HRMS.KRA.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace HRMS.KRA.Service
{
    public class KRAWorkFlowService : IKRAWorkFlowService
    {
        #region Global Varibles
        private readonly ILogger<KRAWorkFlowService> m_Logger;
        private readonly KRAContext m_kraContext;
        private readonly IOrganizationService m_OrganizationService;
        private readonly IKRAService m_kraService;
        private readonly IAspectService m_aspectService;
        private readonly IScaleService m_scaleService;
        private readonly IMeasurementTypeService m_measurementTypeService;
        private readonly IStatusService m_statusService;
        private readonly IEmployeeService m_EmployeeService;
        private readonly IConfiguration m_configuration;
        private KRAMailSubjects m_kraMailSubjects;
        private EmailConfigurations m_EmailConfigurations;
        #endregion

        #region KRAStatusService
        /// <summary>
        /// KRAStatusService
        /// </summary>
        public KRAWorkFlowService
        (
            ILogger<KRAWorkFlowService> logger,
            KRAContext kraContext,
            IKRAService kraService,
            IOrganizationService organizationService,
            IAspectService aspectService,
            IScaleService scaleService,
            IMeasurementTypeService measurementTypeService,
            IStatusService statusService,
            IEmployeeService employeeService,
            IConfiguration configuration,
            IOptions<KRAMailSubjects> kraMailSubjects,
            IOptions<EmailConfigurations> emailConfigurations
        )
        {
            m_Logger = logger;
            m_kraContext = kraContext;
            m_kraService = kraService;
            m_OrganizationService = organizationService;
            m_aspectService = aspectService;
            m_scaleService = scaleService;
            m_measurementTypeService = measurementTypeService;
            m_statusService = statusService;
            m_EmployeeService = employeeService;
            m_configuration = configuration;
            m_kraMailSubjects = kraMailSubjects.Value;
            m_EmailConfigurations = emailConfigurations?.Value;
        }
        #endregion

        #region UpdateDefinitionDetails
        /// <summary>
        /// UpdateDefinitionDetails--- SEND TO HOD
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> UpdateDefinitionDetails(int financialYearId, int departmentId)
        {
            throw new NotImplementedException();
            /*
            var response = new BaseServiceResponse();
            NotificationConfiguration emailNotificationConfig = null;
            DefinitionDetails definitionDetail = null;
            bool isUpdated = false;
            try
            {
                m_Logger.LogInformation("UpdateDefinitionDetails: Calling \"UpdateDefinitionDetails\" method.");

                var department = await m_OrganizationService.GetById(departmentId);
                if (department==null)
                {
                    response.IsSuccessful = false;
                    response.Message = "Department not found.";
                    return response;
                }

                // Gets records from applicableroletypes table for the passed financial year and department.
                var applicableroletypes = m_kraContext.ApplicableRoleTypes.
                                 Where(x => x.FinancialYearId == financialYearId
                                 && x.DepartmentId == departmentId
                               ).ToList();

                if (applicableroletypes.Count() == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "ApplicableRoleTypes not found.";
                    return response;
                }
                var approletypesWithFDStatus = applicableroletypes.Where(c => (c.StatusId == 
                StatusConstants.FinishedDrafting || c.StatusId == StatusConstants.FinishedEditByHR));

                if (approletypesWithFDStatus.Count() == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "No records found in Finished Drafting or FinishedEditByHR status.";
                    return response;
                }
                if (applicableroletypes.Count() != approletypesWithFDStatus.Count())
                {
                    response.IsSuccessful = false;
                    response.Message = "All the gradeRoleType in the ApplicableRoleTypes are not in Finished Drafting or FinishedEditByHR status.";
                    return response;
                }

                // Gets records from DefinitionTransaction table for the passed financial year and department.

                var definitionTransactions = await (from dt in m_kraContext.DefinitionTransactions.
                                 Where(x => x.DefinitionDetails.Definition.ApplicableRoleType.FinancialYearId == financialYearId
                                 && x.DefinitionDetails.Definition.ApplicableRoleType.DepartmentId == departmentId &&
                                 (x.DefinitionDetails.Definition.ApplicableRoleType.StatusId == StatusConstants.FinishedDrafting
                                 || x.DefinitionDetails.Definition.ApplicableRoleType.StatusId == StatusConstants.FinishedEditByHR))
                                                select new DefinitionTransaction
                                                {
                                                    IsActive = dt.IsActive,
                                                    DefinitionDetailsId = dt.DefinitionDetailsId,
                                                    Metric = dt.Metric,
                                                    OperatorId = dt.OperatorId,
                                                    MeasurementTypeId = dt.MeasurementTypeId,
                                                    ScaleId = dt.ScaleId,
                                                    TargetPeriodId = dt.TargetPeriodId,
                                                    TargetValue = dt.TargetValue,
                                                }).ToListAsync();

                if (definitionTransactions.Count() == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "No Definition transactions with Finished Drafting status or FinishedEditByHR.";
                    return response;
                }
                var inactivetransactions = definitionTransactions.Where(c => c.IsActive == false);

                // Gets records from DefinitionDetails table for the passed financial year and department.
                var definitionDetails = m_kraContext.DefinitionDetails.
                    Where(x => x.Definition.ApplicableRoleType.FinancialYearId == financialYearId &&
                    x.Definition.ApplicableRoleType.DepartmentId == departmentId && 
                    (x.Definition.ApplicableRoleType.StatusId == StatusConstants.FinishedDrafting
                    ||
                    x.Definition.ApplicableRoleType.StatusId == StatusConstants.FinishedEditByHR)).ToList();

                if (definitionDetails.Count() == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "No Definition details with Finished Drafting status or FinishedEditByHR status.";
                    return response;
                }
                if (inactivetransactions.Count() > 0)
                {
                    List<int> transactionIds = inactivetransactions.GroupBy(c => c.DefinitionDetailsId).Select(c => c.Key).ToList<int>();
                    var activetransactions = definitionTransactions.Where(c => c.IsActive == true && transactionIds.Contains(c.DefinitionDetailsId));

                    if (activetransactions.Count() == 0)
                    {
                        response.IsSuccessful = false;
                        response.Message = "No Definition Transactions with Finished Drafting or FinishedEditByHR status.";
                        return response;
                    }
                    string notificationCode = string.Empty;

                    using (var transaction = m_kraContext.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (DefinitionTransaction dt in activetransactions)
                            {
                                definitionDetail = definitionDetails.Where(x => x.DefinitionDetailsId == dt.DefinitionDetailsId).FirstOrDefault();
                                definitionDetail.Metric = dt.Metric;
                                definitionDetail.OperatorId = dt.OperatorId;
                                definitionDetail.MeasurementTypeId = dt.MeasurementTypeId;
                                definitionDetail.ScaleId = dt.ScaleId;
                                definitionDetail.TargetPeriodId = dt.TargetPeriodId;
                                definitionDetail.TargetValue = dt.TargetValue;
                            }
                            foreach (ApplicableRoleType approletype in approletypesWithFDStatus)
                            {
                                approletype.StatusId = StatusConstants.SentToHOD;
                            }
                            isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                            if (!isUpdated)
                            {
                                response.IsSuccessful = false;
                                response.Message = "Error occured while updating Definition Details!";
                                return response;
                            }
                            //get email content
                            if (department.Description.Contains("Delivery")) notificationCode = "SentToHODDelivery";
                            else if (department.Description.Contains("Service")) notificationCode = "SentToHODService";
                            else if (department.Description.Contains("Finance")) notificationCode = "SentToHODFinance";

                            var emailNotification = await m_OrganizationService.GetNotificationConfiguration(notificationCode, (int)CategoryMaster.KRA);
                            if (!emailNotification.IsSuccessful)
                            {
                                response.IsSuccessful = false;
                                response.Message = emailNotification.Message;
                                return response;
                            }
                            emailNotificationConfig = emailNotification.Item;

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

                            ServiceResponse<bool> emailStatus = await SendMail(emailNotificationConfig);
                            if (!emailStatus.IsSuccessful)
                            {
                                transaction.Rollback();
                                response.IsSuccessful = false;
                                response.Message = "Error occurred while sending email";
                                return response;
                            }
                            transaction.Commit();
                            response.IsSuccessful = true;
                            response.Message = "Records updated successfully!";
                            return response;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            response.IsSuccessful = false;
                            response.Message = "Error occurred while updating a DefinitionDetails record.";
                            m_Logger.LogError("Error occured in DefinitionDetails " + ex.StackTrace);
                            return response;
                        }
                    }
                }
                else
                {
                    string notificationCode = string.Empty;

                    using (var transaction = m_kraContext.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (ApplicableRoleType approletype in approletypesWithFDStatus)
                            {
                                approletype.StatusId = StatusConstants.SentToHOD;
                            }
                            isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                            if (!isUpdated)
                            {
                                response.IsSuccessful = false;
                                response.Message = "Error occured while updating ApplicableRoleType Status";
                                return response;
                            }
                            //get email content
                            if (department.Description.Contains("Delivery")) notificationCode = "SentToHODDelivery";
                            else if (department.Description.Contains("Service")) notificationCode = "SentToHODService";
                            else if (department.Description.Contains("Finance")) notificationCode = "SentToHODFinance";

                            var emailNotification = await m_OrganizationService.GetNotificationConfiguration(notificationCode, (int)CategoryMaster.KRA);
                            if (!emailNotification.IsSuccessful)
                            {
                                response.IsSuccessful = false;
                                response.Message = emailNotification.Message;
                                return response;
                            }
                            emailNotificationConfig = emailNotification.Item;

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

                            ServiceResponse<bool> emailStatus = await SendMail(emailNotificationConfig);
                            if (!emailStatus.IsSuccessful)
                            {
                                transaction.Rollback();
                                response.IsSuccessful = false;
                                response.Message = "Error occurred while sending email";
                                return response;
                            }
                            transaction.Commit();
                            response.IsSuccessful = true;
                            response.Message = "Records updated successfully!";
                            return response;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            response.IsSuccessful = false;
                            response.Message = "Error occurred while updating a DefinitionDetails record.";
                            m_Logger.LogError("Error occured in DefinitionDetails " + ex.StackTrace);
                            return response;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while Updating Definition Details";
                m_Logger.LogError("Error occurred while Updating Definition Details" + ex.StackTrace);
                return response;
            }
            //return response;
            */
        }

        #endregion

        #region SENDTOCEO
        /// <summary>
        /// SEND TO CEO
        /// </summary>
        /// <param name="financialYearId"></param>       
        /// <returns></returns>
        public async Task<BaseServiceResponse> SendToCEO(int financialYearId)
        {
            var response = new BaseServiceResponse();
            try
            {
                m_Logger.LogInformation("SendToCEO: Calling \"SendToCEO\" method.");

                var departments = await m_OrganizationService.GetAllDepartmentsAsync();
                if (!departments.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = departments.Message;
                    return response;
                }


                using (var transaction = m_kraContext.Database.BeginTransaction())
                {
                    try
                    {
                        
                        var isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;

                        if (!isUpdated)
                        {
                            response.IsSuccessful = false;
                            response.Message = "Error occured while updating ApplicableRoleType Status";
                            return response;
                        }
                        //get email content
                        var emailNotification = await m_OrganizationService.GetNotificationConfigurationAsync("SendToCEO", (int)CategoryMaster.KRA);
                        if (!emailNotification.IsSuccessful)
                        {
                            response.IsSuccessful = false;
                            response.Message = emailNotification.Message;
                            return response;
                        }
                        
                        var emailNotificationConfig = emailNotification.Item;

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

                        ServiceResponse<bool> emailStatus = await SendMail(emailNotificationConfig);

                        if (!emailStatus.IsSuccessful)
                        {
                            transaction.Rollback();
                            response.IsSuccessful = false;
                            response.Message = "Error occurred while sending email";
                            return response;
                        }

                        transaction.Commit();
                        response.IsSuccessful = true;
                        response.Message = "Records updated successfully!";
                        return response;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while updating a DefinitionDetails record.";
                        m_Logger.LogError("Error occured in DefinitionDetails " + ex.StackTrace);
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while Updating Definition Details";
                m_Logger.LogError("Error occurred while Updating Definition Details" + ex.StackTrace);
                return response;
            }
            //return response;
        }

        #endregion

        #region KRAWorkFlow
        /// <summary>
        /// Sent To HOD
        /// </summary>
        /// <param name="kRAWorkFlowModel"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> SentToHODAsync(KRAWorkFlowModel kRAWorkFlowModel)
        {
            var response = new BaseServiceResponse();
            m_Logger.LogInformation($"Calling {nameof(SentToHODAsync)} method.");

            using var transaction = m_kraContext.Database.BeginTransaction();

            try
            {
                // Add KRA Work Flow
                var isAdded = await AddKRAWorkFlowAsync(kRAWorkFlowModel, KRAWorkFlowStatusConstants.SentToHOD);

                if (!isAdded)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occured while adding KRAWorkFlow.";

                    return response;
                }
            
                Department department = await m_OrganizationService.GetByIdAsync(kRAWorkFlowModel.DepartmentId);

                var departmentHeadEmail = await m_EmployeeService.GetEmployeeWorkEmailAddress((int)department.DepartmentHeadId);

                //SendNotification
                string FilePath = Environment.CurrentDirectory + "\\NotificationTemplate\\KRA_SendToHOD.html";
                StreamReader stream = new StreamReader(FilePath);
                string mailBody = stream.ReadToEnd();
                stream.Close();

                NotificationDetail notificationDetail = new NotificationDetail();
                notificationDetail.FromEmail = m_EmailConfigurations.FromEmail;

                string testEmail = m_EmailConfigurations.TestEmail;
                notificationDetail.ToEmail = String.IsNullOrEmpty(testEmail) ? departmentHeadEmail.Item : testEmail;
                notificationDetail.Subject = m_kraMailSubjects.SendToHOD;

                // Validate email Notification details
                if (string.IsNullOrEmpty(notificationDetail.ToEmail))
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Email To cannot be blank";

                    return response;
                }

                notificationDetail.EmailBody = mailBody;
                var emailStatus = await m_OrganizationService.SendEmailAsync(notificationDetail);

                if (!emailStatus.IsSuccessful)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while sending email";
                    return response;
                }

                transaction.Commit();
                response.IsSuccessful = true;
                response.Message = "Records added successfully for KRAWorkFlow!";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response.IsSuccessful = false;
                response.Message = "Error occurred while adding KRAWorkFlow.";
                m_Logger.LogError($"{nameof(SentToHODAsync)} Error occured in KRAWorkFlowService {ex.StackTrace}");

                return response;
            }

            return response;
        }

        /// <summary>
        /// ApprovedbyHODAsync
        /// </summary>
        /// <param name="kRAWorkFlowModel"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> ApprovedbyHODAsync(KRAWorkFlowModel kRAWorkFlowModel)
        {
            var response = new BaseServiceResponse();
            m_Logger.LogInformation($"Calling {nameof(ApprovedbyHODAsync)} method.");

            using var transaction = m_kraContext.Database.BeginTransaction();

            try
            {
                var definitionTransactions = new List<DefinitionTransaction>();

                //Get KRA(s) from DefinitionTransaction if any
                foreach (var roleTypeId in kRAWorkFlowModel.RoleTypeIds)
                {
                    var transactions = await GetDefinitionTransactionAsync(kRAWorkFlowModel.FinancialYearId, roleTypeId);
                    definitionTransactions.AddRange(transactions);
                }

                //Delete KRA(s) from DefinitionTransaction if any
                if (definitionTransactions.Count > 0)
                {
                    m_kraContext.DefinitionTransactions.RemoveRange(definitionTransactions);
                    var isDeleted = await m_kraContext.SaveChangesAsync() > 0;

                    if (!isDeleted)
                    {
                        transaction.Rollback();
                        response = new BaseServiceResponse
                        {
                            IsSuccessful = false,
                            Message = "Error occurred while deleting Definition record in DefinitionTransaction table.",
                        };

                        return response;
                    }
                }

                // Update KRA Work Flow
                var isAdded = await UpdateKRAWorkFlowAsync(kRAWorkFlowModel, KRAWorkFlowStatusConstants.ApprovedbyHOD);

                if (!isAdded)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occured while adding KRAWorkFlow.";

                    return response;
                }

                transaction.Commit();
                response.IsSuccessful = true;
                response.Message = "Records added successfully for KRAWorkFlow!";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response.IsSuccessful = false;
                response.Message = "Error occurred while adding KRAWorkFlow.";
                m_Logger.LogError($"{nameof(ApprovedbyHODAsync)} Error occured in KRAWorkFlowService {ex.StackTrace}");

                return response;
            }

            return response;
        }

        /// <summary>
        /// EditedByHODAsync
        /// </summary>
        /// <param name="kRAWorkFlowModel"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> EditedByHODAsync(KRAWorkFlowModel kRAWorkFlowModel)
        {
            var response = new BaseServiceResponse();
            m_Logger.LogInformation($"Calling {nameof(EditedByHODAsync)} method.");

            using var transaction = m_kraContext.Database.BeginTransaction();

            try
            {
                // Update KRA Work Flow
                var isAdded = await UpdateKRAWorkFlowAsync(kRAWorkFlowModel, KRAWorkFlowStatusConstants.EditedByHOD);

                if (!isAdded)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occured while adding KRAWorkFlow.";

                    return response;
                }

                transaction.Commit();
                response.IsSuccessful = true;
                response.Message = "Records added successfully for KRAWorkFlow!";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response.IsSuccessful = false;
                response.Message = "Error occurred while adding KRAWorkFlow.";
                m_Logger.LogError($"{nameof(EditedByHODAsync)} Error occured in KRAWorkFlowService {ex.StackTrace}");

                return response;
            }

            return response;
        }

        /// <summary>
        /// SentToOpHeadAsync
        /// </summary>
        /// <param name="kRAWorkFlowModel"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> SentToOpHeadAsync(KRAWorkFlowModel kRAWorkFlowModel)
        {
            var response = new BaseServiceResponse();
            m_Logger.LogInformation($"Calling {nameof(SentToOpHeadAsync)} method.");

            using var transaction = m_kraContext.Database.BeginTransaction();

            try
            {
                // Update KRA Work Flow
                var isAdded = await UpdateKRAWorkFlowAsync(kRAWorkFlowModel, KRAWorkFlowStatusConstants.SentToOpHead);

                if (!isAdded)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occured while adding KRAWorkFlow.";

                    return response;
                }
                
                //SendNotification
                string FilePath = Environment.CurrentDirectory + "\\NotificationTemplate\\KRA_SendToOpHead.html";
                StreamReader stream = new StreamReader(FilePath);
                string mailBody = stream.ReadToEnd();
                stream.Close();

                NotificationDetail notificationDetail = new NotificationDetail();
                notificationDetail.FromEmail =m_EmailConfigurations.FromEmail;
                
                string testEmail =m_EmailConfigurations.TestEmail;
                notificationDetail.ToEmail = String.IsNullOrEmpty(testEmail) ? m_EmailConfigurations.HRMEmail : testEmail;
                notificationDetail.Subject = m_kraMailSubjects.SendToOpHead;

                // Validate email Notification details
                if (string.IsNullOrEmpty(notificationDetail.ToEmail))
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Email To cannot be blank";

                    return response;
                }

                notificationDetail.EmailBody = mailBody;
                var emailStatus = await m_OrganizationService.SendEmailAsync(notificationDetail);

                if (!emailStatus.IsSuccessful)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while sending email";
                    return response;
                }

                transaction.Commit();
                response.IsSuccessful = true;
                response.Message = "Records added successfully for KRAWorkFlow!";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response.IsSuccessful = false;
                response.Message = "Error occurred while adding KRAWorkFlow.";
                m_Logger.LogError($"{nameof(SentToOpHeadAsync)} Error occured in KRAWorkFlowService {ex.StackTrace}");

                return response;
            }

            return response;
        }

        /// <summary>
        /// SendToCEOAsync
        /// </summary>
        /// <param name="kRAWorkFlowModel"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> SendToCEOAsync(KRAWorkFlowModel kRAWorkFlowModel)
        {
            var response = new BaseServiceResponse();
            m_Logger.LogInformation($"Calling {nameof(SendToCEOAsync)} method.");

            using var transaction = m_kraContext.Database.BeginTransaction();

            try
            {
                //Select KRAWorkFlows for the given FinancialYearId
                var kRAWorkFlowStatus = await m_kraContext.KRAWorkFlows.Where(wf => wf.FinancialYearId == kRAWorkFlowModel.FinancialYearId
                                                                        && wf.StatusId != KRAWorkFlowStatusConstants.SendToCEO 
                                                                        && wf.StatusId != KRAWorkFlowStatusConstants.ApprovedByCEO)
                                                                        .Select(wf => wf.StatusId).Distinct().ToListAsync();

                if (kRAWorkFlowStatus.Count > 1)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "KRA workflows are having multiple statuses.";

                    return response;
                }
                else if (kRAWorkFlowStatus.First() != KRAWorkFlowStatusConstants.SentToOpHead)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "KRA workflows are not with operations head.";

                    return response;
                }

                if (m_kraContext.DefinitionTransactions.Any(def => def.FinancialYearId == kRAWorkFlowModel.FinancialYearId && def.IsAccepted == null))
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "KRA(s) are pending for operations head action.";

                    return response;
                }
                else if (m_kraContext.Definitions.Any(def => def.FinancialYearId == kRAWorkFlowModel.FinancialYearId && def.IsActive == false))
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "KRA(s) are pending for operations head action.";

                    return response;
                }

                // Update KRA Work Flow
                var isAdded = await SendToCEOKRAWorkFlowAsync(kRAWorkFlowModel, KRAWorkFlowStatusConstants.SendToCEO);

                if (!isAdded)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occured while adding KRAWorkFlow.";

                    return response;
                }

                //SendNotification
                string FilePath = Environment.CurrentDirectory + "\\NotificationTemplate\\KRA_SendToCEO.html";
                StreamReader stream = new StreamReader(FilePath);
                string mailBody = stream.ReadToEnd();
                stream.Close();

                NotificationDetail notificationDetail = new NotificationDetail();
                notificationDetail.FromEmail = m_EmailConfigurations.FromEmail;
                
                string testEmail = m_EmailConfigurations.TestEmail;
                notificationDetail.ToEmail = String.IsNullOrEmpty(testEmail) ? m_EmailConfigurations.CEOEmail : testEmail;
                notificationDetail.Subject = m_kraMailSubjects.SendToCEO;

                // Validate email Notification details
                if (string.IsNullOrEmpty(notificationDetail.ToEmail))
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Email To cannot be blank";

                    return response;
                }

                notificationDetail.EmailBody = mailBody;
                var emailStatus = await m_OrganizationService.SendEmailAsync(notificationDetail);

                if (!emailStatus.IsSuccessful)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while sending email";

                    return response;
                }

                transaction.Commit();
                response.IsSuccessful = true;
                response.Message = "Records added successfully for KRAWorkFlow!";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response.IsSuccessful = false;
                response.Message = "Error occurred while adding KRAWorkFlow.";
                m_Logger.LogError($"{nameof(SendToCEOAsync)} Error occured in KRAWorkFlowService {ex.StackTrace}");

                return response;
            }

            return response;
        }

        /// <summary>
        /// ApprovedByCEOAsync
        /// </summary>
        /// <param name="kRAWorkFlowModel"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> ApprovedByCEOAsync(KRAWorkFlowModel kRAWorkFlowModel)
        {
            var response = new BaseServiceResponse();
            m_Logger.LogInformation($"Calling {nameof(ApprovedByCEOAsync)} method.");

            using var transaction = m_kraContext.Database.BeginTransaction();

            try
            {
                // Update KRA Work Flow
                var isAdded = await AcceptedByCEOKRAWorkFlowAsync(kRAWorkFlowModel, KRAWorkFlowStatusConstants.ApprovedByCEO);

                if (!isAdded)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occured while adding KRAWorkFlow.";

                    return response;
                }

                //SendNotification
                string FilePath = Environment.CurrentDirectory + "\\NotificationTemplate\\KRA_ApprovedByCEO.html";
                StreamReader stream = new StreamReader(FilePath);
                string mailBody = stream.ReadToEnd();
                stream.Close();

                NotificationDetail notificationDetail = new NotificationDetail();
                notificationDetail.FromEmail = m_EmailConfigurations.FromEmail;

                string testEmail =m_EmailConfigurations.TestEmail;
                notificationDetail.ToEmail = String.IsNullOrEmpty(testEmail) ?m_EmailConfigurations.HRMEmail : testEmail;
                notificationDetail.Subject = m_kraMailSubjects.ApprovedByCEO;

                // Validate email Notification details
                if (string.IsNullOrEmpty(notificationDetail.ToEmail))
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Email To cannot be blank";

                    return response;
                }

                notificationDetail.EmailBody = mailBody;
                var emailStatus = await m_OrganizationService.SendEmailAsync(notificationDetail);

                if (!emailStatus.IsSuccessful)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while sending email";

                    return response;
                }

                transaction.Commit();
                response.IsSuccessful = true;
                response.Message = "Records added successfully for KRAWorkFlow!";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response.IsSuccessful = false;
                response.Message = "Error occurred while adding KRAWorkFlow.";
                m_Logger.LogError($"{nameof(ApprovedByCEOAsync)} Error occured in KRAWorkFlowService {ex.StackTrace}");

                return response;
            }

            return response;
        }

        /// <summary>
        /// HODAddAsync
        /// </summary>
        /// <param name="definitionModel"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> HODAddAsync(DefinitionModel definitionModel)
        {
            BaseServiceResponse response;
            m_Logger.LogInformation($"KRAWorkFlowService: Calling {nameof(HODAddAsync)} method");

            try
            {
                // This is when Measurement Type is not Scale
                if (definitionModel.ScaleId == 0)
                {
                    definitionModel.ScaleId = null;
                }

                var definitionTransaction = new DefinitionTransaction()
                {
                    FinancialYearId = definitionModel.FinancialYearId,
                    RoleTypeId = definitionModel.RoleTypeId,
                    AspectId = definitionModel.AspectId,
                    MeasurementTypeId = definitionModel.MeasurementTypeId,
                    ScaleId = definitionModel.ScaleId,
                    OperatorId = definitionModel.OperatorId,
                    Metric = definitionModel.Metric,
                    TargetValue = definitionModel.TargetValue,
                    TargetPeriodId = definitionModel.TargetPeriodId,
                };

                m_kraContext.DefinitionTransactions.Add(definitionTransaction);
                var isCreated = await m_kraContext.SaveChangesAsync() > 0;

                if (isCreated)
                {
                    response = new BaseServiceResponse
                    {
                        IsSuccessful = true,
                        Message = "Definition Record added successfully in DefinitionTransaction table.",
                    };
                }
                else
                {
                    response = new BaseServiceResponse
                    {
                        IsSuccessful = false,
                        Message = "Error occurred while adding a new Definition record in DefinitionTransaction table.",
                    };
                }
            }
            catch (Exception ex)
            {
                response = new BaseServiceResponse
                {
                    IsSuccessful = false,
                    Message = "Error occurred while adding a new Definition record in DefinitionTransaction table.",
                };
                m_Logger.LogError($"{nameof(HODAddAsync)}: Error occured in KRAWorkFlowService {ex?.StackTrace}");
            }

            return response;
        }

        /// <summary>
        /// HODUpdateAsync
        /// </summary>
        /// <param name="definitionModel"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> HODUpdateAsync(DefinitionModel definitionModel)
        {
            BaseServiceResponse response;
            m_Logger.LogInformation($"KRAWorkFlowService: Calling {nameof(HODUpdateAsync)} method");

            try
            {
                //Check if definition already exists in DefinitionTransactions table, if yes then delete and add new.
                if (m_kraContext.DefinitionTransactions.Any(def => def.DefinitionTransactionId == definitionModel.DefinitionTransactionId))
                {
                    var transaction = await (from tran in m_kraContext.DefinitionTransactions
                                             where tran.DefinitionTransactionId == definitionModel.DefinitionTransactionId
                                             select new DefinitionTransaction
                                             {
                                                 DefinitionId = (tran.DefinitionId == Guid.Empty) ? Guid.Empty : tran.DefinitionId,
                                                 DefinitionTransactionId = tran.DefinitionTransactionId,
                                             }).FirstOrDefaultAsync();

                    m_kraContext.DefinitionTransactions.Remove(transaction);

                }

                // This is when Measurement Type is not Scale
                if (definitionModel.ScaleId == 0)
                {
                    definitionModel.ScaleId = null;
                }

                var definitionTransaction = new DefinitionTransaction()
                {
                    DefinitionId = definitionModel.DefinitionId,
                    FinancialYearId = definitionModel.FinancialYearId,
                    RoleTypeId = definitionModel.RoleTypeId,
                    AspectId = definitionModel.AspectId,
                    MeasurementTypeId = definitionModel.MeasurementTypeId,
                    ScaleId = definitionModel.ScaleId,
                    OperatorId = definitionModel.OperatorId,
                    Metric = definitionModel.Metric,
                    TargetValue = definitionModel.TargetValue,
                    TargetPeriodId = definitionModel.TargetPeriodId,
                };

                m_kraContext.DefinitionTransactions.Add(definitionTransaction);
                var isCreated = await m_kraContext.SaveChangesAsync() > 0;

                if (isCreated)
                {
                    response = new BaseServiceResponse
                    {
                        IsSuccessful = true,
                        Message = "Definition record updated successfully in DefinitionTransaction table.",
                    };

                    return response;
                }
                else
                {
                    response = new BaseServiceResponse
                    {
                        IsSuccessful = false,
                        Message = "Error occurred while updating Definition record in DefinitionTransaction table.",
                    };

                    return response;
                }
            }
            catch (Exception ex)
            {
                response = new BaseServiceResponse
                {
                    IsSuccessful = false,
                    Message = "Error occurred while updating Definition record in DefinitionTransaction table.",
                };
                m_Logger.LogError($"{nameof(HODUpdateAsync)}: Error occured in KRAWorkFlowService {ex?.StackTrace}");

                return response;
            }
        }

        /// <summary>
        /// HODDeleteAsync
        /// </summary>
        /// <param name="definitionModel"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> HODDeleteAsync(DefinitionModel definitionModel)
        {
            BaseServiceResponse response;
            m_Logger.LogInformation($"KRAWorkFlowService: Calling {nameof(HODDeleteAsync)} method");

            var definition = new Definition();

            if (definitionModel.DefinitionId != null)
            {
                definition = await m_kraContext.Definitions.FirstOrDefaultAsync(def => def.DefinitionId == definitionModel.DefinitionId);
                try
                {
                    if (definition is { })
                    {
                        definition.IsActive = false;
                        var isUpdated = await m_kraContext.SaveChangesAsync() > 0;

                        if (isUpdated)
                        {
                            response = new BaseServiceResponse
                            {
                                IsSuccessful = true,
                                Message = "KRA Definition records deleted."
                            };
                        }
                        else
                        {
                            response = new BaseServiceResponse
                            {
                                IsSuccessful = false,
                                Message = "KRA Definition could not be deleted."
                            };
                        }
                    }
                    else
                    {
                        response = new BaseServiceResponse
                        {
                            IsSuccessful = false,
                            Message = "KRA Definition could not be deleted."
                        };
                    }
                }
                catch (Exception ex)
                {
                    response = new BaseServiceResponse
                    {
                        IsSuccessful = false,
                        Message = "KRA Definition could not be deleted.",
                    };
                    m_Logger.LogError($"{nameof(HODDeleteAsync)}: Error occured in DefinitionService {ex?.StackTrace}");
                }
            }
            else
            {
                if (m_kraContext.DefinitionTransactions.Any(def => def.DefinitionTransactionId == definitionModel.DefinitionTransactionId))
                {
                    var definitionTransaction = await (from tran in m_kraContext.DefinitionTransactions
                                                       where tran.DefinitionTransactionId == definitionModel.DefinitionTransactionId
                                                       select new DefinitionTransaction
                                                       {
                                                           DefinitionId = (tran.DefinitionId == Guid.Empty) ? Guid.Empty : tran.DefinitionId,
                                                           DefinitionTransactionId = tran.DefinitionTransactionId,
                                                       }).FirstOrDefaultAsync();

                    m_kraContext.DefinitionTransactions.Remove(definitionTransaction);
                    var isDeleted = await m_kraContext.SaveChangesAsync() > 0;

                    if (isDeleted)
                    {
                        response = new BaseServiceResponse
                        {
                            IsSuccessful = true,
                            Message = "KRA Definition records deleted."
                        };
                    }
                    else
                    {
                        response = new BaseServiceResponse
                        {
                            IsSuccessful = false,
                            Message = "KRA Definition could not be deleted."
                        };
                    }
                }
                else
                {
                    response = new BaseServiceResponse
                    {
                        IsSuccessful = false,
                        Message = "KRA Definition could not be deleted."
                    };
                }
            }

            return response;
        }

        /// <summary>
        /// GetHODDefinitionsAsync
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <param name="roleTypeId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<KRAModel>> GetHODDefinitionsAsync(int financialYearId, int roleTypeId)
        {
            var response = new ServiceListResponse<KRAModel>();
            List<KRAModel> lstDefinitions = new List<KRAModel>();
            try
            {
                var lstKRADefinitions = await GetDefinitionsAsync(financialYearId, roleTypeId, true);
                var lstKRADefinitionTransactions = await GetDefinitionTransactionsAsync(financialYearId, roleTypeId);

                //Get the status of roleTypeId
                var statusId = await m_kraContext.KRAWorkFlows.Where(wf => wf.FinancialYearId == financialYearId && wf.RoleTypeId == roleTypeId).Select(wf => wf.StatusId).FirstOrDefaultAsync();

                //Add newly added KRA's by HOD
                lstDefinitions.AddRange(lstKRADefinitionTransactions.Where(trasc => trasc.DefinitionId == null).ToList());

                foreach (var definition in lstKRADefinitions)
                {
                    //Check if any KRA modifed by HOD
                    var modifiedDefinition = lstKRADefinitionTransactions.FirstOrDefault(trans => trans.DefinitionId == definition.DefinitionId);

                    if (modifiedDefinition is { })
                    {
                        lstDefinitions.Add(modifiedDefinition);
                    }
                    else
                    {
                        lstDefinitions.Add(definition);
                    }
                }

                if (lstDefinitions.Count == 0)
                {
                    response.Items = lstDefinitions;
                    response.IsSuccessful = false;
                    response.Message = "KRAs not found.";
                    }
                else
                {
                    foreach (var definition in lstDefinitions)
                    {
                        definition.StatusId = statusId;

                        string status = "Draft";
                        if (statusId == KRAWorkFlowStatusConstants.SentToHOD) status = "SentToHOD";
                        else if (statusId == KRAWorkFlowStatusConstants.ApprovedbyHOD) status = "ApprovedbyHOD";
                        else if (statusId == KRAWorkFlowStatusConstants.EditedByHOD) status = "EditedByHOD";
                        else if (statusId == KRAWorkFlowStatusConstants.SentToOpHead) status = "SentToOpHead";
                        else if (statusId == KRAWorkFlowStatusConstants.SendToCEO) status = "SendToCEO";
                        else if (statusId == KRAWorkFlowStatusConstants.ApprovedByCEO) status = "ApprovedByCEO";
                        else if (statusId == KRAWorkFlowStatusConstants.SentToAssociates) status = "SentToAssociates";
                        definition.Status = status;

                        //Set IsUpdatedByHOD flag to true if modifications done by HOD
                        if (definition.IsActive.HasValue && !definition.IsActive.Value)
                        {
                            definition.IsUpdatedByHOD = true;
                        }

                        //Set IsUpdatedByHOD flag to true if modifications done by HOD
                        if (definition.DefinitionTransactionId != 0)
                        {
                            definition.IsUpdatedByHOD = true;
                        }
                    }

                    response.Items = lstDefinitions;
                    response.IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "KRAs not found.";

                m_Logger.LogError($"{nameof(GetHODDefinitionsAsync)}: Error occured in KRAWorkFlowService {ex?.StackTrace}");
            }

            return response;
        }

        /// <summary>
        /// GetOperationHeadStatusAsync
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<OperationHeadStatusModel>> GetOperationHeadStatusAsync(int financialYearId)
        {
            var response = new ServiceListResponse<OperationHeadStatusModel>();
            List<RoleTypeDepartment> lstDefinitions = new List<RoleTypeDepartment>();
            
            try
            {
                m_Logger.LogInformation($"KRAWorkFlowService: Calling GetOperationHeadStatusAsync service to get the department and  their respective role types");

                //Get all the RoleTypeIds for all the departments
                var lstRoleTypesAndDepartments = await m_OrganizationService.GetRoleTypesAndDepartmentsAsync();
                var operationHeadStatus = new List<OperationHeadStatusModel>();

                foreach (var roleDepartment in lstRoleTypesAndDepartments)
                {
                    var operationHeadStatusModel = new OperationHeadStatusModel
                    {
                        DepartmentName = roleDepartment.DepartmentName,
                        DepartmentId = roleDepartment.DepartmentId,
                        TotalRoleTypeCount = roleDepartment.RoleTypeIds.Count
                    };

                    var statusid = m_kraContext.KRAWorkFlows
                                             .Where
                                             (wf => wf.FinancialYearId == financialYearId
                                             && roleDepartment.RoleTypeIds.Contains(wf.RoleTypeId))
                                             .Select(df => df.StatusId)
                                             .OrderBy(cf => cf)
                                             .FirstOrDefault();

                    string status = string.Empty;
                    if (statusid == KRAWorkFlowStatusConstants.SentToHOD) status = "SentToHOD";
                    else if (statusid == KRAWorkFlowStatusConstants.ApprovedbyHOD) status = "ApprovedbyHOD";
                    else if (statusid == KRAWorkFlowStatusConstants.EditedByHOD) status = "EditedByHOD";
                    else if (statusid == KRAWorkFlowStatusConstants.SentToOpHead) status = "SentToOpHead";
                    else if (statusid == KRAWorkFlowStatusConstants.SendToCEO) status = "SendToCEO";
                    else if (statusid == KRAWorkFlowStatusConstants.ApprovedByCEO) status = "ApprovedByCEO";
                    else if (statusid == KRAWorkFlowStatusConstants.SentToAssociates) status = "SentToAssociates";
                    operationHeadStatusModel.Status = status;
                    var kraDefinedCount = m_kraContext.Definitions
                                          .Where
                                          (df => df.FinancialYearId == financialYearId
                                          && roleDepartment.RoleTypeIds.Contains(df.RoleTypeId))
                                          .Select(df => df.RoleTypeId)
                                          .Distinct()
                                          .Count();

                    var approvedKRAs = m_kraContext.KRAWorkFlows
                                             .Where
                                             (wf => wf.FinancialYearId == financialYearId
                                             && roleDepartment.RoleTypeIds.Contains(wf.RoleTypeId)
                                             && (
                                                    wf.StatusId == KRAWorkFlowStatusConstants.ApprovedbyHOD
                                                    || wf.StatusId == KRAWorkFlowStatusConstants.SendToCEO 
                                                    || wf.StatusId == KRAWorkFlowStatusConstants.ApprovedByCEO
                                                    || wf.StatusId == KRAWorkFlowStatusConstants.SentToAssociates 
                                                )
                                             ).Select(df => df.RoleTypeId)
                                              .Distinct()
                                              .ToList();

                    var EditedByHODKRAs = m_kraContext.DefinitionTransactions
                                         .Where
                                         (df => df.FinancialYearId == financialYearId
                                         && roleDepartment.RoleTypeIds.Contains(df.RoleTypeId)
                                         && df.IsAccepted == null)
                                         .Select(df => df.RoleTypeId)
                                         .Distinct()
                                         .ToList();

                    var DeletedByHODKRAs = m_kraContext.Definitions
                                        .Where
                                        (df => df.FinancialYearId == financialYearId
                                        && roleDepartment.RoleTypeIds.Contains(df.RoleTypeId)
                                        && df.IsActive == false)
                                        .Select(df => df.RoleTypeId)
                                        .Distinct()
                                        .ToList();

                    //Whether accepted/rejected or deleted by Operations Head
                    int consideredByOpHeadCount = 0;
                    foreach (var roleTypeId in roleDepartment.RoleTypeIds)
                    {
                        var statId = m_kraContext.KRAWorkFlows
                                             .Where
                                             (wf => wf.FinancialYearId == financialYearId
                                             && wf.RoleTypeId == roleTypeId)
                                             .Select(df => df.StatusId)
                                             .FirstOrDefault();
                        if (statId == KRAWorkFlowStatusConstants.SentToOpHead)
                        {
                            //If there are edits by HOD
                            var totalDTs = m_kraContext.DefinitionTransactions.Where
                                (df => df.FinancialYearId == financialYearId
                                && df.RoleTypeId == roleTypeId).Count();

                            if (totalDTs > 0)
                            {
                                var totalDTsConsidered = m_kraContext.DefinitionTransactions.Where
                                                                    (df => df.FinancialYearId == financialYearId
                                                                    && df.RoleTypeId == roleTypeId
                                                                    && df.IsAccepted != null).Count();

                                //Check if all the edits are accepted or rejected
                                if (totalDTs == totalDTsConsidered)
                                {
                                    //Check if there are any deleted definitions that needs to be accepted
                                    var definitions = m_kraContext.Definitions.Where
                                                                (df => df.FinancialYearId == financialYearId
                                                                && df.RoleTypeId == roleTypeId
                                                                && df.IsActive == false).ToList();

                                    if (definitions.Count == 0)
                                        consideredByOpHeadCount++;
                                }
                            }
                            else
                            {
                                //Check if there are any deleted definitions that needs to be accepted
                                var definitions = m_kraContext.Definitions.Where
                                                            (df => df.FinancialYearId == financialYearId
                                                            && df.RoleTypeId == roleTypeId
                                                            && df.IsActive == false).ToList();

                                if (definitions.Count == 0)
                                consideredByOpHeadCount++;
                            }
                        }                        
                    }

                    operationHeadStatusModel.AcceptedRoleTypesCount = approvedKRAs.Count + consideredByOpHeadCount;
                    operationHeadStatusModel.UnderReviewRoleTypesCount = m_kraContext.KRAWorkFlows.Where(wf => wf.FinancialYearId == financialYearId && roleDepartment.RoleTypeIds.Contains(wf.RoleTypeId) && wf.StatusId == KRAWorkFlowStatusConstants.SentToHOD).Count();
                    operationHeadStatusModel.EditedRoleTypesCount = EditedByHODKRAs.Intersect(DeletedByHODKRAs).Count() + EditedByHODKRAs.Except(DeletedByHODKRAs).Count() + DeletedByHODKRAs.Except(EditedByHODKRAs).Count();
                    operationHeadStatusModel.KRARoleTypesNotDefinedCount = operationHeadStatusModel.TotalRoleTypeCount - kraDefinedCount;
                    operationHeadStatusModel.IsEligilbeForReivew = (roleDepartment.RoleTypeIds.Count != 0 && operationHeadStatusModel.KRARoleTypesNotDefinedCount == 0);

                    operationHeadStatus.Add(operationHeadStatusModel);
                }

                if (operationHeadStatus.Count == 0)
                {
                    response.Items = operationHeadStatus;
                    response.IsSuccessful = false;
                    response.Message = "KRAs status not found.";
                }
                else
                {
                    response.Items = operationHeadStatus;
                    response.IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "KRAs status not found.";

                m_Logger.LogError($"{nameof(GetOperationHeadStatusAsync)}: Error occured in KRAWorkFlowService {ex?.StackTrace}");
            }

            return response;
        }

        /// <summary>
        /// AcceptedByOperationHeadAsync
        /// </summary>
        /// <param name="definitionModel"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> AcceptedByOperationHeadAsync(DefinitionModel definitionModel)
        {
            BaseServiceResponse response =  new BaseServiceResponse();
            m_Logger.LogInformation($"KRAWorkFlowService: Calling {nameof(AcceptedByOperationHeadAsync)} method");

            var definition = new Definition();

            if (definitionModel.DefinitionId != null)
            {
                definition = m_kraContext.Definitions.Find(definitionModel.DefinitionId);

                if (definition is null)
                {
                    response.IsSuccessful = false;
                    response.Message = "KRA Definition not found for update.";
                    return response;
                }
            }

            //Accept deleted KRA which is deleted by hod
            if (definitionModel.IsActive == false)
            {
                return await AcceptDeletedKRAAsync(definitionModel.DefinitionId.Value);
            }

            using var transaction = m_kraContext.Database.BeginTransaction();
            
            try
            {
                // This is when Measurement Type is not Scale
                if (definitionModel.ScaleId == 0)
                {
                    definitionModel.ScaleId = null;
                }

                //Add if DefinitionId is null else update the definition table
                if (definitionModel.DefinitionId is null)
                {
                    definition = new Definition
                    {
                        FinancialYearId = definitionModel.FinancialYearId,
                        RoleTypeId = definitionModel.RoleTypeId,
                        AspectId = definitionModel.AspectId,
                        MeasurementTypeId = definitionModel.MeasurementTypeId,
                        ScaleId = definitionModel.ScaleId,
                        OperatorId = definitionModel.OperatorId,
                        Metric = definitionModel.Metric,
                        TargetValue = definitionModel.TargetValue,
                        TargetPeriodId = definitionModel.TargetPeriodId,
                        IsActive = true,
                    };

                    m_kraContext.Definitions.Add(definition);
                }
                else
                {

                    definition.FinancialYearId = definitionModel.FinancialYearId;
                    definition.RoleTypeId = definitionModel.RoleTypeId;
                    definition.AspectId = definitionModel.AspectId;
                    definition.MeasurementTypeId = definitionModel.MeasurementTypeId;
                    definition.ScaleId = definitionModel.ScaleId;
                    definition.OperatorId = definitionModel.OperatorId;
                    definition.Metric = definitionModel.Metric;
                    definition.TargetValue = definitionModel.TargetValue;
                    definition.TargetPeriodId = definitionModel.TargetPeriodId;

                    m_kraContext.Definitions.Update(definition);
                }
                var isSuccess = await m_kraContext.SaveChangesAsync() > 0;

                if (isSuccess)
                {
                    //Check if definition already exists in DefinitionTransactions table, if yes then set IsApprvoed as true
                    var definitionTransaction = await GetDefinitionTransactionAsync(definitionModel.DefinitionTransactionId);

                    if (definitionTransaction is { })
                    {
                        definitionTransaction.IsAccepted = true;
                        if (definitionTransaction.DefinitionId is null && definition != null)
                            definitionTransaction.DefinitionId = definition.DefinitionId;
                        m_kraContext.DefinitionTransactions.Update(definitionTransaction);
                        isSuccess = await m_kraContext.SaveChangesAsync() > 0;

                        if (isSuccess)
                        {
                            transaction.Commit();
                            response = new BaseServiceResponse
                            {
                                IsSuccessful = true,
                                Message = "KRA accepted successfully KRA by operation head.",
                            };

                            return response;
                        }
                        else
                        {
                            transaction.Rollback();
                            response = new BaseServiceResponse
                            {
                                IsSuccessful = false,
                                Message = "Error occurred while accepting KRA by operation head.",
                            };

                            return response;
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        response = new BaseServiceResponse
                        {
                            IsSuccessful = false,
                            Message = "KRA definition not found",
                        };

                        return response;
                    }
                }
                else
                {
                    transaction.Rollback();
                    response = new BaseServiceResponse
                    {
                        IsSuccessful = false,
                        Message = "Error occurred while adding/updating KRA.",
                    };

                    return response;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response = new BaseServiceResponse
                {
                    IsSuccessful = false,
                    Message = "Error occurred while accepting KRA.",
                };
                m_Logger.LogError($"{nameof(AcceptedByOperationHeadAsync)}: Error occured in KRAWorkFlowService {ex?.StackTrace}");

                return response;
            }
        }

        /// <summary>
        /// RejectedByOperationHeadAsync
        /// </summary>
        /// <param name="definitionModel"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> RejectedByOperationHeadAsync(DefinitionModel definitionModel)
        {
            BaseServiceResponse response;
            m_Logger.LogInformation($"KRAWorkFlowService: Calling {nameof(RejectedByOperationHeadAsync)} method");

            try
            {
                if (definitionModel.DefinitionTransactionId != 0)
                {
                    //Check if definition already exists in DefinitionTransactions table, if yes then set IsApprvoed as false
                    var definitionTransaction = await GetDefinitionTransactionAsync(definitionModel.DefinitionTransactionId);

                    if (definitionTransaction is { })
                    {
                        definitionTransaction.IsAccepted = false;
                        m_kraContext.DefinitionTransactions.Update(definitionTransaction);
                        var isSuccess = await m_kraContext.SaveChangesAsync() > 0;

                        if (isSuccess)
                        {
                            response = new BaseServiceResponse
                            {
                                IsSuccessful = true,
                                Message = "KRA rejected successfully by operation head.",
                            };

                            return response;
                        }
                        else
                        {
                            response = new BaseServiceResponse
                            {
                                IsSuccessful = false,
                                Message = "Error occurred while rejecting KRA by operation head.",
                            };

                            return response;
                        }
                    }
                    else
                    {
                        response = new BaseServiceResponse
                        {
                            IsSuccessful = false,
                            Message = "KRA definition not found for update.",
                        };

                        return response;
                    }
                }
                else
                {
                    //Reject deleted KRA which is deleted by hod
                    return await RejectDeletedKRAAsync(definitionModel.DefinitionId.Value);
                }
            }
            catch (Exception ex)
            {
                response = new BaseServiceResponse
                {
                    IsSuccessful = false,
                    Message = "Error occurred while rejecting KRA.",
                };
                m_Logger.LogError($"{nameof(RejectedByOperationHeadAsync)}: Error occured in KRAWorkFlowService {ex?.StackTrace}");

                return response;
            }
        }

        /// <summary>
        /// GetOperationHeadDefinitionsAsync
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <param name="roleTypeId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<KRAModel>> GetOperationHeadDefinitionsAsync(int financialYearId, int roleTypeId)
        {
            var response = new ServiceListResponse<KRAModel>();
            List<KRAModel> lstDefinitions = new List<KRAModel>();
            try
            {
                var lstKRADefinitions = await GetDefinitionsAsync(financialYearId, roleTypeId);

                var statusId = await m_kraContext.KRAWorkFlows.Where(wf => wf.FinancialYearId == financialYearId && wf.RoleTypeId == roleTypeId).Select(wf => wf.StatusId).FirstOrDefaultAsync();
                string status = "Draft";
                if (statusId == KRAWorkFlowStatusConstants.SentToHOD) status = "SentToHOD";
                else if (statusId == KRAWorkFlowStatusConstants.ApprovedbyHOD) status = "ApprovedbyHOD";
                else if (statusId == KRAWorkFlowStatusConstants.EditedByHOD) status = "EditedByHOD";
                else if (statusId == KRAWorkFlowStatusConstants.SentToOpHead) status = "SentToOpHead";
                else if (statusId == KRAWorkFlowStatusConstants.SendToCEO) status = "SendToCEO";
                else if (statusId == KRAWorkFlowStatusConstants.ApprovedByCEO) status = "ApprovedByCEO";
                else if (statusId == KRAWorkFlowStatusConstants.SentToAssociates) status = "SentToAssociates";
                
                if (statusId < KRAWorkFlowStatusConstants.SentToOpHead) //Before KRAs are Sent to Operation Head
                {
                    //Add all KRA's from Definition table
                    lstDefinitions.AddRange(lstKRADefinitions);
                }
                else if (statusId == KRAWorkFlowStatusConstants.SentToOpHead) //When KRAs are sent to Operations Head
                {
                    var lstKRADefinitionTransactions = await GetDefinitionTransactionsAsync(financialYearId, roleTypeId);

                    //Add newly added KRA's by HOD
                    lstDefinitions.AddRange(lstKRADefinitionTransactions.Where(trasc => trasc.DefinitionId == null && trasc.IsAccepted == null).ToList());

                    //Add modified KRA's by HOD
                    lstDefinitions.AddRange(lstKRADefinitionTransactions.Where(trasc => trasc.DefinitionId != null && trasc.IsAccepted == null).ToList());

                    //Add Original KRA's from Definition table
                    lstDefinitions.AddRange(lstKRADefinitions);

                }
                else //When KRAs are Sent to CEO and later
                {
                    //Exclude the deleted KRA's by HOD
                    lstDefinitions.AddRange(lstKRADefinitions.Where(defs => defs.IsActive == true).ToList());
                }

                if (lstDefinitions.Count == 0)
                {
                    response.Items = lstDefinitions.OrderByDescending(kra => kra.AspectName).ToList();
                    response.IsSuccessful = false;
                    response.Message = "KRA(s) not found.";
                }
                else
                {
                    foreach (KRAModel defintion in lstDefinitions)
                    {
                        defintion.Status = status;
                    }                                           
                    response.Items = lstDefinitions;
                    response.IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "KRA(s) not found.";

                m_Logger.LogError($"{nameof(GetOperationHeadDefinitionsAsync)}: Error occured in KRAWorkFlowService {ex?.StackTrace}");
            }

            return response;
        }

        /// <summary>
        /// AcceptDeletedKRAAsync
        /// </summary>
        /// <param name="definitionId"></param>
        /// <returns></returns>
        private async Task<BaseServiceResponse> AcceptDeletedKRAAsync(Guid definitionId)
        {
            BaseServiceResponse response;
            m_Logger.LogInformation($"KRAWorkFlowService: Calling {nameof(AcceptDeletedKRAAsync)} method");

            try
            {
                //Check if definition already exists in DefinitionTransactions table, if yes then set IsApprvoed as false
                var KRAdefinition = await m_kraContext.Definitions.FindAsync(definitionId);

                if (KRAdefinition is { })
                {
                    //Delete KRA
                    m_kraContext.Definitions.Remove(KRAdefinition);
                    var isSuccess = await m_kraContext.SaveChangesAsync() > 0;

                    if (isSuccess)
                    {
                        response = new BaseServiceResponse
                        {
                            IsSuccessful = true,
                            Message = "KRA accepted successfully by operation head.",
                        };

                        return response;
                    }
                    else
                    {
                        response = new BaseServiceResponse
                        {
                            IsSuccessful = false,
                            Message = "Error occurred while accepting KRA by operation head.",
                        };

                        return response;
                    }
                }
                else
                {
                    response = new BaseServiceResponse
                    {
                        IsSuccessful = false,
                        Message = "KRA definition not found for update.",
                    };

                    return response;
                }
            }
            catch (Exception ex)
            {
                response = new BaseServiceResponse
                {
                    IsSuccessful = false,
                    Message = "Error occurred while accepting KRA by operation head.",
                };
                m_Logger.LogError($"{nameof(AcceptDeletedKRAAsync)}: Error occured in KRAWorkFlowService {ex?.StackTrace}");

                return response;
            }
        }

        /// <summary>
        /// RejectDeletedKRAAsync
        /// </summary>
        /// <param name="definitionId"></param>
        /// <returns></returns>
        private async Task<BaseServiceResponse> RejectDeletedKRAAsync(Guid definitionId)
        {
            BaseServiceResponse response;
            m_Logger.LogInformation($"KRAWorkFlowService: Calling {nameof(RejectDeletedKRAAsync)} method");

            try
            {
                //Check if definition already exists in DefinitionTransactions table, if yes then set IsApprvoed as false
                var KRAdefinition = await m_kraContext.Definitions.FindAsync(definitionId);

                if (KRAdefinition is { })
                {
                    KRAdefinition.IsActive = true;
                    m_kraContext.Definitions.Update(KRAdefinition);
                    var isSuccess = await m_kraContext.SaveChangesAsync() > 0;

                    if (isSuccess)
                    {
                        response = new BaseServiceResponse
                        {
                            IsSuccessful = true,
                            Message = "KRA rejected successfully by operation head.",
                        };

                        return response;
                    }
                    else
                    {
                        response = new BaseServiceResponse
                        {
                            IsSuccessful = false,
                            Message = "Error occurred while rejecting KRA by operation head.",
                        };

                        return response;
                    }
                }
                else
                {
                    response = new BaseServiceResponse
                    {
                        IsSuccessful = false,
                        Message = "KRA definition not found for update.",
                    };

                    return response;
                }
            }
            catch (Exception ex)
            {
                response = new BaseServiceResponse
                {
                    IsSuccessful = false,
                    Message = "Error occurred while rejecting KRA by operation head.",
                };
                m_Logger.LogError($"{nameof(RejectDeletedKRAAsync)}: Error occured in KRAWorkFlowService {ex?.StackTrace}");

                return response;
            }
        }

        /// <summary>
        /// GetDefinitionsAsync
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <param name="roleTypeId"></param>
        /// <param name="showActive"></param>
        /// <returns></returns>
        private async Task<List<KRAModel>> GetDefinitionsAsync(int financialYearId, int roleTypeId, bool showActive = false)
        {
            var lstDefinition = await m_kraContext.Definitions.Where(x => x.FinancialYearId == financialYearId
                                                                    && x.RoleTypeId == roleTypeId)
                                                               .AsNoTracking()
                                                               .ToListAsync();
            if (showActive && lstDefinition.Count > 0)
            {
                lstDefinition = lstDefinition.Where(x => x.IsActive.Value == true).ToList();
            }

            var lstOperators = await GetOperatorsAsync();
            var lstMeasurementTypes = await GetMeasurementAsync();
            var lstTargetPeriods = await GetTargetPeriodsAsync();
            var lstAspects = await GetAspectsAsync();
            var lstScale = await GetScaleAsync();
            var lstScaleDetails = await GetScaleDetailsAsync();

            if (lstDefinition is { } && lstDefinition.Count > 0)
            {
                var lstDefinitionModels = (from d in lstDefinition
                        join aspList in lstAspects
                        on d.AspectId equals aspList.AspectId into aspectData
                        from asp in aspectData.DefaultIfEmpty()

                        join opList in lstOperators
                        on d.OperatorId equals opList.OperatorId into opData
                        from op in opData.DefaultIfEmpty()

                        join mtList in lstMeasurementTypes
                        on d.MeasurementTypeId equals mtList.Id into mtype
                        from mt in mtype.DefaultIfEmpty()

                        join tpList in lstTargetPeriods
                        on d.TargetPeriodId equals tpList.TargetPeriodId into tperiod
                        from tp in tperiod.DefaultIfEmpty()

                        join scList in lstScale
                        on d.ScaleId equals scList.ScaleID into scale
                        from sc in scale.DefaultIfEmpty()

                        orderby asp.AspectName

                        select new KRAModel
                        {
                            DefinitionId = d.DefinitionId,
                            AspectName = asp.AspectName,
                            Date = d.ModifiedDate.HasValue ? "Modified on " + d.ModifiedDate.Value.ToString("dddd, dd MMMM yyyy") : "Added on " + d.CreatedDate.Value.ToString("dddd, dd MMMM yyyy"),
                            //
                            Metric = d.Metric + (sc == null ? "" : "(Scale: " + sc.MinimumScale + "-" + sc.MaximumScale + "; "
                            + string.Join("; ",
                            (
                            from sd in lstScaleDetails
                            where sc.ScaleID == sd.ScaleID
                            select sd.ScaleValue + "-" + sd.ScaleDescription).ToList()) + ")"),
                            //
                            Target = op.OperatorValue + " " + d.TargetValue + (mt.MeasurementType == "Percentage" ? "%" : "") + " (" + tp.TargetPeriodValue + ")",
                            ScaleId = d.ScaleId,
                            IsActive = d.IsActive,
                            AspectId = d.AspectId,
                            OperatorId = d.OperatorId,
                            MeasurementTypeId = d.MeasurementTypeId,
                            TargetPeriodId = d.TargetPeriodId,
                            TargetValue = d.TargetValue,
                            MeasurementType = mt.MeasurementType,
                            Status = "Draft"
                        }
                       ).ToList();

                if (lstDefinitionModels != null && lstDefinitionModels.Count > 0)
                {
                    //Get the status of RoleTypeId
                    var statusId = await m_kraContext.KRAWorkFlows.Where(wf => wf.FinancialYearId == financialYearId && wf.RoleTypeId == roleTypeId).Select(wf => wf.StatusId).FirstOrDefaultAsync();

                    if (statusId != 0)
                    {
                        string status = "Draft";
                        if (statusId == KRAWorkFlowStatusConstants.SentToHOD) status = "SentToHOD";
                        else if (statusId == KRAWorkFlowStatusConstants.ApprovedbyHOD) status = "ApprovedbyHOD";
                        else if (statusId == KRAWorkFlowStatusConstants.EditedByHOD) status = "EditedByHOD";
                        else if (statusId == KRAWorkFlowStatusConstants.SentToOpHead) status = "SentToOpHead";
                        else if (statusId == KRAWorkFlowStatusConstants.SendToCEO) status = "SendToCEO";
                        else if (statusId == KRAWorkFlowStatusConstants.ApprovedByCEO) status = "ApprovedByCEO";
                        else if (statusId == KRAWorkFlowStatusConstants.SentToAssociates) status = "SentToAssociates";

                        foreach (var definition in lstDefinitionModels)
                        {
                            definition.StatusId = statusId;
                            definition.Status = status;
                        }
                    }
                }
                return lstDefinitionModels;
            }

            return new List<KRAModel>();
        }

        /// <summary>
        /// GetDefinitionTransactionsAsync
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <param name="roleTypeId"></param>
        /// <returns></returns>
        private async Task<List<KRAModel>> GetDefinitionTransactionsAsync(int financialYearId, int roleTypeId)
        {
            var lstDefinition = await (from tran in m_kraContext.DefinitionTransactions
                                       where tran.FinancialYearId == financialYearId && tran.RoleTypeId == roleTypeId 
                                       && tran.IsAccepted != false
                                       select new DefinitionTransaction
                                       {
                                           DefinitionId = (tran.DefinitionId == Guid.Empty) ? Guid.Empty : tran.DefinitionId,
                                           DefinitionTransactionId = tran.DefinitionTransactionId,
                                           FinancialYearId = tran.FinancialYearId,
                                           RoleTypeId = tran.RoleTypeId,
                                           AspectId = tran.AspectId,
                                           Metric = tran.Metric,
                                           OperatorId = tran.OperatorId,
                                           MeasurementTypeId = tran.MeasurementTypeId,
                                           ScaleId = tran.ScaleId,
                                           TargetValue = tran.TargetValue,
                                           TargetPeriodId = tran.TargetPeriodId,
                                           IsAccepted = tran.IsAccepted,
                                           CreatedDate = tran.CreatedDate,
                                           ModifiedDate = tran.ModifiedDate,
                                       }).ToListAsync();


            var lstOperators = await GetOperatorsAsync();
            var lstMeasurementTypes = await GetMeasurementAsync();
            var lstTargetPeriods = await GetTargetPeriodsAsync();
            var lstAspects = await GetAspectsAsync();
            var lstScale = await GetScaleAsync();
            var lstScaleDetails = await GetScaleDetailsAsync();

            if (lstDefinition is { } && lstDefinition.Count > 0)
            {
                return (from d in lstDefinition
                        join aspList in lstAspects
                        on d.AspectId equals aspList.AspectId into aspectData
                        from asp in aspectData.DefaultIfEmpty()

                        join opList in lstOperators
                        on d.OperatorId equals opList.OperatorId into opData
                        from op in opData.DefaultIfEmpty()

                        join mtList in lstMeasurementTypes
                        on d.MeasurementTypeId equals mtList.Id into mtype
                        from mt in mtype.DefaultIfEmpty()

                        join tpList in lstTargetPeriods
                        on d.TargetPeriodId equals tpList.TargetPeriodId into tperiod
                        from tp in tperiod.DefaultIfEmpty()

                        join scList in lstScale
                        on d.ScaleId equals scList.ScaleID into scale
                        from sc in scale.DefaultIfEmpty()

                        select new KRAModel
                        {
                            DefinitionId = d.DefinitionId,
                            DefinitionTransactionId = d.DefinitionTransactionId,
                            AspectName = asp.AspectName,
                            Date = d.ModifiedDate.HasValue ? "Modified on " + d.ModifiedDate.Value.ToString("dddd, dd MMMM yyyy") : "Added on " + d.CreatedDate.Value.ToString("dddd, dd MMMM yyyy"),
                            //
                            Metric = d.Metric + (sc == null ? "" : "(Scale: " + sc.MinimumScale + "-" + sc.MaximumScale + "; "
                            + string.Join("; ",
                            (
                            from sd in lstScaleDetails
                            where sc.ScaleID == sd.ScaleID
                            select sd.ScaleValue + "-" + sd.ScaleDescription).ToList()) + ")"),
                            //
                            Target = op.OperatorValue + " " + d.TargetValue + (mt.MeasurementType == "Percentage" ? "%" : "") + " (" + tp.TargetPeriodValue + ")",
                            ScaleId = d.ScaleId,
                            IsActive = true,
                            IsAccepted = d.IsAccepted,
                            AspectId = d.AspectId,
                            OperatorId = d.OperatorId,
                            MeasurementTypeId = d.MeasurementTypeId,
                            TargetPeriodId = d.TargetPeriodId,
                            TargetValue = d.TargetValue,
                            MeasurementType = mt.MeasurementType,
                        }
                       ).ToList();
            }

            return new List<KRAModel>();
        }

        /// <summary>
        /// GetDefinitionTransactionAsync
        /// </summary>
        /// <param name="definitionTransactionId"></param>
        /// <returns></returns>
        private async Task<DefinitionTransaction> GetDefinitionTransactionAsync(int definitionTransactionId)
        {
            return await (from tran in m_kraContext.DefinitionTransactions
                          where tran.DefinitionTransactionId == definitionTransactionId
                          select new DefinitionTransaction
                          {
                              DefinitionTransactionId = tran.DefinitionTransactionId,
                              FinancialYearId = tran.FinancialYearId,
                              RoleTypeId = tran.RoleTypeId,
                              AspectId = tran.AspectId,
                              Metric = tran.Metric,
                              OperatorId = tran.OperatorId,
                              MeasurementTypeId = tran.MeasurementTypeId,
                              ScaleId = tran.ScaleId,
                              TargetValue = tran.TargetValue,
                              TargetPeriodId = tran.TargetPeriodId,
                              IsAccepted = tran.IsAccepted,
                              CreatedDate = tran.CreatedDate,
                          }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// GetDefinitionTransactionAsync
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <param name="roleTypeId"></param>
        /// <returns></returns>
        private async Task<List<DefinitionTransaction>> GetDefinitionTransactionAsync(int financialYearId, int roleTypeId)
        {
            return await (from tran in m_kraContext.DefinitionTransactions
                          where tran.FinancialYearId == financialYearId && tran.RoleTypeId == roleTypeId
                          select new DefinitionTransaction
                          {
                              DefinitionTransactionId = tran.DefinitionTransactionId,
                              FinancialYearId = tran.FinancialYearId,
                              RoleTypeId = tran.RoleTypeId,
                              AspectId = tran.AspectId,
                              Metric = tran.Metric,
                              OperatorId = tran.OperatorId,
                              MeasurementTypeId = tran.MeasurementTypeId,
                              ScaleId = tran.ScaleId,
                              TargetValue = tran.TargetValue,
                              TargetPeriodId = tran.TargetPeriodId,
                              IsAccepted = tran.IsAccepted,
                              CreatedDate = tran.CreatedDate,
                          }).ToListAsync();
        }

        /// <summary>
        /// GetScaleDetailsAsync
        /// </summary>
        /// <returns></returns>
        private async Task<List<ScaleDetailsModel>> GetScaleDetailsAsync()
        {
            return await m_scaleService.GetScaleDetailsAsync();
        }

        /// <summary>
        /// GetScaleAsync
        /// </summary>
        /// <returns></returns>
        private async Task<List<ScaleModel>> GetScaleAsync()
        {
            return await m_scaleService.GetAllAsync();
        }

        /// <summary>
        /// GetAspectsAsync
        /// </summary>
        /// <returns></returns>
        private async Task<List<AspectModel>> GetAspectsAsync()
        {
            return await m_aspectService.GetAllAsync();
        }

        /// <summary>
        /// GetTargetPeriodsAsync
        /// </summary>
        /// <returns></returns>
        private async Task<List<TargetPeriod>> GetTargetPeriodsAsync()
        {
            return await m_kraService.GetTargetPeriodsAsync();
        }

        /// <summary>
        /// GetMeasurementAsync
        /// </summary>
        /// <returns></returns>
        private async Task<List<MeasurementTypeModel>> GetMeasurementAsync()
        {
            return await m_measurementTypeService.GetAllAsync();
        }

        /// <summary>
        /// GetOperatorsAsync
        /// </summary>
        /// <returns></returns>
        private async Task<List<Operator>> GetOperatorsAsync()
        {
            return await m_kraService.GetOperatorsAsync();
        }

        /// <summary>
        /// AddKRAWorkFlowAsync
        /// </summary>
        /// <param name="kRAWorkFlowModel"></param>
        /// <returns></returns>
        private async Task<bool> AddKRAWorkFlowAsync(KRAWorkFlowModel kRAWorkFlowModel, int statusId)
        {
            m_Logger.LogInformation($"Calling {nameof(AddKRAWorkFlowAsync)} method.");
            var KRAWorkFlows = new List<KRAWorkFlow>();

            m_Logger.LogInformation($"KRAWorkFlowService: Calling AddKRAWorkFlowAsync service to update the status of the role type");

            //Get all the RoleTypeIds for the given departmentId
            var roleTypesAndDepartments = await m_OrganizationService.GetRoleTypesAndDepartmentsAsync(kRAWorkFlowModel.DepartmentId);

            if (roleTypesAndDepartments is { } && roleTypesAndDepartments.Count > 0)
            {
                //Get RoleTypeIds for already defined KRA's for the given financial year and departmentId
                var kraDefinedRoleTypeIds = await m_kraContext.Definitions
                                                  .Where(def => def.FinancialYearId == kRAWorkFlowModel.FinancialYearId
                                                   && roleTypesAndDepartments.First().RoleTypeIds.Contains(def.RoleTypeId))
                                                   .Select(def => def.RoleTypeId).Distinct().ToListAsync();

                //Get RoleTypeIds which are already in the workflow for given financial year and departmentId
                var getRoleTypeIdsAlreadyInWorkflow = await m_kraContext.KRAWorkFlows
                                                   .Where(def => def.FinancialYearId == kRAWorkFlowModel.FinancialYearId
                                                    && kraDefinedRoleTypeIds.Contains(def.RoleTypeId))
                                                    .Select(def => def.RoleTypeId).Distinct().ToListAsync();

                //List of RoleTypesIds to be sent for HOD's review.
                var sendToHodForReviewRoleTypeIds = kraDefinedRoleTypeIds.Except(getRoleTypeIdsAlreadyInWorkflow).ToList();

                foreach (var roleTypeId in sendToHodForReviewRoleTypeIds)
                {
                    var kRAWorkFlow = new KRAWorkFlow
                    {
                        FinancialYearId = kRAWorkFlowModel.FinancialYearId,
                        StatusId = statusId,
                        RoleTypeId = roleTypeId,
                    };

                    KRAWorkFlows.Add(kRAWorkFlow);
                }

                await m_kraContext.KRAWorkFlows.AddRangeAsync(KRAWorkFlows);
            }

            if (KRAWorkFlows.Count > 0)
            {
                return await m_kraContext.SaveChangesAsync() > 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// UpdateKRAWorkFlowAsync
        /// </summary>
        /// <param name="kRAWorkFlowModel"></param>
        /// <returns></returns>
        private async Task<bool> UpdateKRAWorkFlowAsync(KRAWorkFlowModel kRAWorkFlowModel, int statusId)
        {
            m_Logger.LogInformation($"Calling {nameof(UpdateKRAWorkFlowAsync)} method.");
            var KRAWorkFlows = new List<KRAWorkFlow>();

            foreach (var roleTypeId in kRAWorkFlowModel.RoleTypeIds)
            {
                var kRAWorkFlow = await m_kraContext.KRAWorkFlows.FirstOrDefaultAsync(wf => wf.RoleTypeId == roleTypeId 
                                        && wf.FinancialYearId == kRAWorkFlowModel.FinancialYearId);
                
                //Set the status
                if (kRAWorkFlow is { })
                {
                    kRAWorkFlow.StatusId = statusId;
                    KRAWorkFlows.Add(kRAWorkFlow);
                }
            }

            m_kraContext.KRAWorkFlows.UpdateRange(KRAWorkFlows);

            if (KRAWorkFlows.Count > 0)
            {
                return await m_kraContext.SaveChangesAsync() > 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// SendToCEOKRAWorkFlowAsync
        /// </summary>
        /// <param name="kRAWorkFlowModel"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        private async Task<bool> SendToCEOKRAWorkFlowAsync(KRAWorkFlowModel kRAWorkFlowModel, int statusId)
        {
            m_Logger.LogInformation($"Calling {nameof(SendToCEOKRAWorkFlowAsync)} method.");
            var KRAWorkFlows = new List<KRAWorkFlow>();

            //Select KRAWorkFlows for the given FinancialYearId where status is SentToOpHead
            var kRAWorkFlowList = await m_kraContext.KRAWorkFlows.Where(wf => wf.FinancialYearId == kRAWorkFlowModel.FinancialYearId
                                                                        && wf.StatusId != KRAWorkFlowStatusConstants.SendToCEO
                                                                        && wf.StatusId != KRAWorkFlowStatusConstants.ApprovedByCEO)
                                                                  .ToListAsync();

            foreach (var kRAWorkFlow in kRAWorkFlowList)
            {
                //Set the status
                if (kRAWorkFlow is { })
                {
                    kRAWorkFlow.StatusId = statusId;
                    KRAWorkFlows.Add(kRAWorkFlow);
                }

                m_kraContext.KRAWorkFlows.UpdateRange(KRAWorkFlows);
            }

            if (KRAWorkFlows.Count > 0)
            {
                return await m_kraContext.SaveChangesAsync() > 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// AcceptedByCEOKRAWorkFlowAsync
        /// </summary>
        /// <param name="kRAWorkFlowModel"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        private async Task<bool> AcceptedByCEOKRAWorkFlowAsync(KRAWorkFlowModel kRAWorkFlowModel, int statusId)
        {
            m_Logger.LogInformation($"Calling {nameof(AcceptedByCEOKRAWorkFlowAsync)} method.");
            var KRAWorkFlows = new List<KRAWorkFlow>();

            //Select KRAWorkFlows for the given FinancialYearId where status is SendToCEO for approval
            var kRAWorkFlowList = await m_kraContext.KRAWorkFlows.Where(wf => wf.FinancialYearId == kRAWorkFlowModel.FinancialYearId
                                                                        && wf.StatusId == KRAWorkFlowStatusConstants.SendToCEO)
                                                                  .ToListAsync();

            foreach (var kRAWorkFlow in kRAWorkFlowList)
            {
                //Set the status
                if (kRAWorkFlow is { })
                {
                    kRAWorkFlow.StatusId = statusId;
                    KRAWorkFlows.Add(kRAWorkFlow);
                }

                m_kraContext.KRAWorkFlows.UpdateRange(KRAWorkFlows);
            }

            if (KRAWorkFlows.Count > 0)
            {
                return await m_kraContext.SaveChangesAsync() > 0;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region UpdateRoleTypeStatus
        /// <summary>
        /// UpdateRoleTypeStatus -- Send to HR
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> UpdateRoleTypeStatus(int financialYearId, int departmentId)
        {
            throw new NotImplementedException();
            /*
            var response = new BaseServiceResponse();
            NotificationConfiguration emailNotificationConfig = null;
            DefinitionDetails definitionDetail = null;
            bool isUpdated = false;
            try
            {
                m_Logger.LogInformation("UpdateRoleTypeStatus: Calling \"UpdateRoleTypeStatus\" method.");

                // Gets records from applicableroletypes table for the passed financial year and department.
                var applicableroletypes = m_kraContext.ApplicableRoleTypes.
                                 Where(x => x.FinancialYearId == financialYearId
                                 && x.DepartmentId == departmentId
                               ).ToList();

                if (applicableroletypes.Count() == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "ApplicableRoleTypes not found.";
                    return response;
                }
                var approletypeStatus = applicableroletypes.Where(c => c.StatusId == StatusConstants.FinishedEditByHOD
                || c.StatusId == StatusConstants.ApprovedbyHOD);

                if (approletypeStatus.Count() == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "No records found.";
                    return response;
                }
                if (applicableroletypes.Count() != approletypeStatus.Count())
                {
                    response.IsSuccessful = false;
                    response.Message = "All the gradeRoleType in the ApplicableRoleTypes are not in FinishedEditByHOD or ApprovedbyHOD status.";
                    return response;
                }
                var department = await m_OrganizationService.GetById(departmentId);
                if (department == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "Department not found.";
                    return response;
                }

                string notificationCode = string.Empty;
                // Gets records from DefinitionTransaction table for the passed financial year and department.
                var definitionTransactions = m_kraContext.DefinitionTransactions.
                                 Where(x => x.DefinitionDetails.Definition.ApplicableRoleType.FinancialYearId == financialYearId
                                 && x.DefinitionDetails.Definition.ApplicableRoleType.DepartmentId == departmentId &&
                                 (x.DefinitionDetails.Definition.ApplicableRoleType.StatusId == StatusConstants.FinishedEditByHOD
                                 ||
                                 x.DefinitionDetails.Definition.ApplicableRoleType.StatusId == StatusConstants.ApprovedbyHOD)
                                 ).ToList();

                if (definitionTransactions.Count() == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "No Definition transactions with FinishedEditByHOD or ApprovedbyHOD status.";
                    return response;
                }
                var inactivetransactions = definitionTransactions.Where(c => c.IsActive == false);

                // Gets records from DefinitionDetails table for the passed financial year and department.
                var definitionDetails = m_kraContext.DefinitionDetails.
                    Where(x => x.Definition.ApplicableRoleType.FinancialYearId == financialYearId &&
                    x.Definition.ApplicableRoleType.DepartmentId == departmentId && (x.Definition.ApplicableRoleType.StatusId == StatusConstants.FinishedEditByHOD
                    || x.Definition.ApplicableRoleType.StatusId == StatusConstants.ApprovedbyHOD)).ToList();

                if (definitionDetails.Count() == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "No Definition details with FinishedEditByHOD or ApprovedbyHOD status.";
                    return response;
                }
                if (inactivetransactions.Count() > 0)
                {
                    List<int> transactionIds = inactivetransactions.GroupBy(c => c.DefinitionDetailsId).Select(c => c.Key).ToList<int>();
                    var activetransactions = definitionTransactions.Where(c => c.IsActive == true && transactionIds.Contains(c.DefinitionDetailsId));

                    if (activetransactions.Count() == 0)
                    {
                        response.IsSuccessful = false;
                        response.Message = "No Definition Transactions with FinishedEditByHOD or ApprovedbyHOD status.";
                        return response;
                    }
                    using (var transaction = m_kraContext.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (DefinitionTransaction dt in activetransactions)
                            {
                                definitionDetail = definitionDetails.Where(x => x.DefinitionDetailsId == dt.DefinitionDetailsId).FirstOrDefault();
                                definitionDetail.Metric = dt.Metric;
                                definitionDetail.OperatorId = dt.OperatorId;
                                definitionDetail.MeasurementTypeId = dt.MeasurementTypeId;
                                definitionDetail.ScaleId = dt.ScaleId;
                                definitionDetail.TargetPeriodId = dt.TargetPeriodId;
                                definitionDetail.TargetValue = dt.TargetValue;
                            }
                            foreach (ApplicableRoleType approletype in approletypeStatus)
                            {
                                approletype.StatusId = StatusConstants.SentToHR;
                            }
                            isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                            if (!isUpdated)
                            {
                                response.IsSuccessful = false;
                                response.Message = "Error occured while updating Definition Details!";
                                return response;
                            }
                            //get email content
                            if (department.Description.Contains("Delivery")) notificationCode = "SentToHRDelivery";
                            else if (department.Description.Contains("Service")) notificationCode = "SentToHRService";
                            else if (department.Description.Contains("Finance")) notificationCode = "SentToHRFinance";

                            var emailNotification = await m_OrganizationService.GetNotificationConfiguration(notificationCode, (int)CategoryMaster.KRA);
                            if (!emailNotification.IsSuccessful)
                            {
                                response.IsSuccessful = false;
                                response.Message = emailNotification.Message;
                                return response;
                            }
                            emailNotificationConfig = emailNotification.Item;

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

                            ServiceResponse<bool> emailStatus = await SendMail(emailNotificationConfig);
                            if (!emailStatus.IsSuccessful)
                            {
                                transaction.Rollback();
                                response.IsSuccessful = false;
                                response.Message = "Error occurred while sending email";
                                return response;
                            }
                            transaction.Commit();
                            response.IsSuccessful = true;
                            response.Message = "Records updated successfully!";
                            return response;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            response.IsSuccessful = false;
                            response.Message = "Error occurred while updating a DefinitionDetails record.";
                            m_Logger.LogError("Error occured in DefinitionDetails " + ex.StackTrace);
                            return response;
                        }
                    }
                }
                else
                {
                    using (var transaction = m_kraContext.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (ApplicableRoleType approletype in approletypeStatus)
                            {
                                approletype.StatusId = StatusConstants.SentToHR;
                            }
                            isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                            if (!isUpdated)
                            {
                                response.IsSuccessful = false;
                                response.Message = "Error occured while updating ApplicableRoleType Status";
                                return response;
                            }
                            //get email content
                            if (department.Description.Contains("Delivery")) notificationCode = "SentToHRDelivery";
                            else if (department.Description.Contains("Service")) notificationCode = "SentToHRService";
                            else if (department.Description.Contains("Finance")) notificationCode = "SentToHRFinance";

                            var emailNotification = await m_OrganizationService.GetNotificationConfiguration(notificationCode, (int)CategoryMaster.KRA);
                            if (!emailNotification.IsSuccessful)
                            {
                                response.IsSuccessful = false;
                                response.Message = emailNotification.Message;
                                return response;
                            }
                            emailNotificationConfig = emailNotification.Item;

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

                            ServiceResponse<bool> emailStatus = await SendMail(emailNotificationConfig);
                            if (!emailStatus.IsSuccessful)
                            {
                                transaction.Rollback();
                                response.IsSuccessful = false;
                                response.Message = "Error occurred while sending email";
                                return response;
                            }
                            transaction.Commit();
                            response.IsSuccessful = true;
                            response.Message = "Records updated successfully!";
                            return response;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            response.IsSuccessful = false;
                            response.Message = "Error occurred while updating a DefinitionDetails record.";
                            m_Logger.LogError("Error occured in DefinitionDetails " + ex.StackTrace);
                            return response;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while Updating Definition Details";
                m_Logger.LogError("Error occurred while Updating Definition Details" + ex.StackTrace);
                return response;
            }
            //return response;
            */
        }

        #endregion

        #region EditByHR
        /// <summary>
        /// EditByHR -- Start HR Review
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> EditByHR(int financialYearId, int departmentId)
        {
            throw new NotImplementedException();
            /*
            var response = new BaseServiceResponse();
            bool isUpdated = false;
            try
            {
                m_Logger.LogInformation("EditByHR: Calling \"EditByHR\" method.");

                // Gets records from applicableroletypes table for the passed financial year and department.
                var applicableroletypes = m_kraContext.ApplicableRoleTypes.
                                 Where(x => x.FinancialYearId == financialYearId
                                 && x.DepartmentId == departmentId
                                 && x.StatusId == StatusConstants.SentToHR
                               ).ToList();

                if (applicableroletypes.Count() == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "ApplicableRoleTypes not found.";
                    return response;
                }

                // Gets records from definitions table for the passed financial year and department.
                var definitions = m_kraContext.Definitions.
                                 Where(x => x.ApplicableRoleType.FinancialYearId == financialYearId
                                 && x.ApplicableRoleType.DepartmentId == departmentId &&
                                 (x.ApplicableRoleType.StatusId == StatusConstants.SentToHR)
                                 ).ToList();

                if (definitions.Count() == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "No definitions with SentToHR status.";
                    return response;
                }

                using (var transaction = m_kraContext.Database.BeginTransaction())
                {
                    try
                    {
                        int approvedByHodDefinitionsCnt = 0;
                        int totaldefinationsCnt = 0;
                        foreach (ApplicableRoleType approletype in applicableroletypes)
                        {
                            approvedByHodDefinitionsCnt = definitions.Where(c => c.IsHODApproved.HasValue && c.IsHODApproved.Value == true
                            && c.ApplicableRoleTypeId == approletype.ApplicableRoleTypeId).Count();

                            totaldefinationsCnt = definitions.Where(c => c.ApplicableRoleTypeId == approletype.ApplicableRoleTypeId).Count();

                            if (approvedByHodDefinitionsCnt != totaldefinationsCnt)
                                approletype.StatusId = StatusConstants.EditByHR;
                        }
                        isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                        if (!isUpdated)
                        {
                            response.IsSuccessful = false;
                            response.Message = "Error occured while updating role type!";
                            return response;
                        }
                        transaction.Commit();
                        response.IsSuccessful = true;
                        response.Message = "Records updated successfully!";
                        return response;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred in EditByHR.";
                        m_Logger.LogError("Error occured in EditByHR " + ex.StackTrace);
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while Updating Definition Details";
                m_Logger.LogError("Error occurred while Updating Definition Details" + ex.StackTrace);
                return response;
            }
            //return response;
            */
        }

        #endregion

        #region GetStatusByFinancialYearId FOR HR Statuses
        /// <summary>
        /// Gets Status by FinancialYearId.FOR HR Statuses
        /// </summary>
        ///  <param name="financialYearId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<KRAStatusModel>> GetKRAStatusByFinancialYearId(int financialYearId)
        {
            throw new NotImplementedException();
            /*
            var response = new ServiceListResponse<KRAStatusModel>();
            List<KRAStatusModel> lstStatus = new List<KRAStatusModel>();
            KRAStatusModel model = null;
            try
            {
                m_Logger.LogInformation("KRAStatusService: Calling \"GetStatusByFinancialYearId\" method.");

                var departments = await m_OrganizationService.GetAllDepartments();
                if (!departments.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Items = null;
                    response.Message = departments.Message;
                    return response;
                }

                //Gets Applicable Roletypes for the passed financial year.
                var applicableRoleTypes = await m_kraContext.ApplicableRoleTypes
                                                .Where(x => x.FinancialYearId == financialYearId).ToListAsync();

                if (applicableRoleTypes != null || applicableRoleTypes.Count != 0)
                {
                    List<int> departmentIds = applicableRoleTypes.GroupBy(c => c.DepartmentId).Select(c => c.Key).ToList<int>();
                    List<ApplicableRoleType> sentTOHODRoletypes = null;
                    int draftStatus;
                    int sentToHODCount = 0;
                    int ceoApprovedCount = 0;
                    int sentToCEOCount = 0;
                    int totalIsHodApprovedCount = 0;
                    int finishedEditByHODCount = 0;
                    int finishedEditByHRCount = 0;
                    int finishedDraftingCount = 0;                   
                    List<ApplicableRoleType> approletypes = null;
                    ApplicableRoleType approletype = null;
                    ApplicableRoleType modifiedapproletype = null;  
                    string date = string.Empty;
                    List<int> applicableroletypeIds;

                    foreach (int departmentId in departmentIds)
                    {
                        totalIsHodApprovedCount = finishedEditByHODCount = 0;
                        sentToHODCount = 0;
                        date = string.Empty;

                        approletypes = applicableRoleTypes.Where(c => c.DepartmentId == departmentId)
                         .OrderByDescending(c => c.CreatedDate).ToList();

                        if (approletypes != null && approletypes.Count > 0)
                        {
                            model = new KRAStatusModel();
                            modifiedapproletype = approletypes.Where(c => c.ModifiedDate.HasValue).OrderByDescending(c => c.ModifiedDate).FirstOrDefault();
                            approletype = approletypes[0];

                            model.DepartmentId = approletype.DepartmentId;
                            model.DepartmentName = (from dept in departments.Items
                                                    where approletype.DepartmentId == dept.DepartmentId
                                                    select dept.Description).SingleOrDefault();

                            model.TotalRoleTypes = approletypes.Count();

                            applicableroletypeIds = approletypes.GroupBy(c => c.ApplicableRoleTypeId).Select(c => c.Key).ToList<int>();

                            var definitions = await m_kraContext.Definitions
                                                   .Where(x => applicableroletypeIds.Contains(x.ApplicableRoleTypeId)                                                 
                                                   && x.ApplicableRoleType.StatusId >= StatusConstants.SentToHR).ToListAsync();

                            if (definitions != null && definitions.Count > 0)
                            {
                                totalIsHodApprovedCount = definitions.Where(c=> c.IsHODApproved.HasValue && c.IsHODApproved==true).GroupBy(c => c.ApplicableRoleTypeId).Count();
                                finishedEditByHODCount = definitions.GroupBy(c => c.ApplicableRoleTypeId).Count()-totalIsHodApprovedCount;
                            }
                            model.AcceptedRoleTypes = totalIsHodApprovedCount;

                            sentTOHODRoletypes = applicableRoleTypes.Where(x => x.StatusId == StatusConstants.SentToHOD).ToList();
                            if (sentTOHODRoletypes != null) sentToHODCount = sentTOHODRoletypes.Count();

                            if (totalIsHodApprovedCount > 0)
                                model.ForReviewRoleTypes = model.TotalRoleTypes - totalIsHodApprovedCount;
                            else model.ForReviewRoleTypes = finishedEditByHODCount;

                            if (modifiedapproletype != null && modifiedapproletype.ModifiedDate.HasValue)
                            {
                                if (modifiedapproletype.ModifiedDate.Value > approletype.CreatedDate.Value)
                                    model.Date = modifiedapproletype.ModifiedDate.Value.ToLongDateString();
                                else model.Date = approletype.CreatedDate.Value.ToLongDateString();
                            }
                            else model.Date = approletype.CreatedDate.Value.ToLongDateString();

                            draftStatus = approletypes.Where(x => x.StatusId == StatusConstants.Draft).Count();

                            finishedDraftingCount = approletypes.Where(x => x.StatusId == StatusConstants.FinishedDrafting).ToList().Count();
                            finishedEditByHRCount = approletypes.Where(x => x.StatusId == StatusConstants.FinishedEditByHR).ToList().Count();
                            sentToHODCount = approletypes.Where(x => x.StatusId == StatusConstants.SentToHOD).ToList().Count();
                            sentToCEOCount = approletypes.Where(x => x.StatusId == StatusConstants.SendToCEO).Count();
                            ceoApprovedCount= approletypes.Where(x => x.StatusId == StatusConstants.ApprovedByCEO).Count();

                            if (finishedEditByHRCount == model.TotalRoleTypes) model.ForReviewRoleTypes = 0;
                            if (draftStatus > 0)
                            {
                                model.Status = "Drafting in-progress";
                                model.Action = "Send to HOD";
                            }
                            else if (finishedEditByHRCount > 0)
                            {
                                model.Status = "Awaiting to be send to HOD for acceptance";
                                model.Action = "Send to HOD";
                            }
                            else if (ceoApprovedCount== model.TotalRoleTypes)
                            {
                                model.Status = "Completed";
                                model.Action = "";
                            }
                            else if (model.TotalRoleTypes == finishedDraftingCount)
                            {
                                model.Status = "Awaiting to be send to HOD for acceptance";
                                model.Action = "Send to HOD";
                            }
                            else if (model.TotalRoleTypes == sentToHODCount)
                            {
                                model.Status = "Sent for acceptance to HOD, awaiting feedback";
                                model.Action = "Sent";
                            }
                            else if (model.TotalRoleTypes == sentToCEOCount)
                            {
                                model.Status = "Sent to CEO";
                                model.Action = "Sent";
                            }
                            else if (totalIsHodApprovedCount > 0 & model.TotalRoleTypes != totalIsHodApprovedCount)
                            {
                                model.Status = "KRAs modifed by HOD";
                                model.Action = "Start Review";
                            }
                            else if (model.TotalRoleTypes == totalIsHodApprovedCount)
                            {
                                model.Status = "KRAs accepted by HOD";
                                model.Action = "Ready to be sent to ceo";
                            }
                            else if (finishedEditByHODCount > 0)
                            {
                                model.Status = "KRAs modifed by HOD";
                                model.Action = "Start Review";
                            }
                            else if (finishedDraftingCount > 0)
                            {
                                model.Status = "Drafting in-progress";
                                model.Action = "Send to HOD";
                            }
                            lstStatus.Add(model);
                        }
                    }
                    if (lstStatus.Count > 0)
                    {
                        int totalreadytosenttoceo = lstStatus.Where(c => c.Action == "Ready to be sent to ceo").Count();
                        if (departmentIds != null && departmentIds.Count > 0 && departmentIds.Count == totalreadytosenttoceo)
                        {
                            foreach (KRAStatusModel item in lstStatus) item.SendtoCEOStatus = "SEND";
                            response.Items = lstStatus;
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.Items = lstStatus;
                            response.IsSuccessful = true;
                        }
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "No KRA status details found for this financial year!";
                    }
                }
                else
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "No KRA status details found for this financial year!";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Items = null;
                response.Message = "Error occured while fetching Status details.";
                m_Logger.LogError("Error occured while fetching Status details." + ex.StackTrace);
            }
            return response;
            */
        }
        #endregion

        #region GetKRAStatus FOR HOD Statuses
        /// <summary>
        /// Gets KRAStatus by FinancialYearId and DepartmentId. FOR HOD Statuses
        ///  ///  <param name="financialYearId"></param>
        ///  /// <param name="departmentId"></param>
        /// </summary>     
        /// <returns></returns>
        public async Task<ServiceListResponse<KRAStatusModel>> GetKRAStatus(int financialYearId, int departmentId)
        {
            throw new NotImplementedException();
            /*
            var response = new ServiceListResponse<KRAStatusModel>();
            List<KRAStatusModel> lstStatus = new List<KRAStatusModel>();
            List<Department> departmensofHOD;
            KRAStatusModel model = null;
            try
            {
                m_Logger.LogInformation("KRAStatusService: Calling \"GetStatusByFinancialYearId\" method.");                

                var departments = await m_OrganizationService.GetUserDepartmentDetails();
                //departments = departments.Items.Where(c => c.DepartmentHeadId.Value == departmentId).ToListDepartment);
                if (!departments.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Items = null;
                    response.Message = departments.Message;
                    return response;
                }
                departmensofHOD = (from dept in departments.Items
                                   where dept.DepartmentHeadId == departmentId
                                   select dept).ToList();

                List<int> departmentIds = departmensofHOD.GroupBy(c => c.DepartmentId).Select(c => c.Key).ToList<int>();

                //Gets Applicable Roletypes for the passed financial year and departmentId.
                var applicableRoleTypes = await m_kraContext.ApplicableRoleTypes
                                                .Where(x => x.FinancialYearId == financialYearId &&
                                                 departmentIds.Contains(x.DepartmentId)
                                                 && x.StatusId != StatusConstants.Draft &&
                                                 x.StatusId != StatusConstants.FinishedDrafting)
                                                .ToListAsync();

                if (applicableRoleTypes != null && applicableRoleTypes.Count > 0)
                {
                    List<ApplicableRoleType> acceptedRoletypes = null;                   
                    List<ApplicableRoleType> sentTOHODRoletypes = null;
                    // int draftStatus;
                    int sentToHODCount = 0;
                    int sentToCEOCount = 0;
                    int finishedEditByHODCount = 0;
                    int totalacceptedCount = 0;
                    int totalsendtoHODCount = 0;
                    int totalsendtoHRCount = 0;
                    int totalIsHodApprovedCount = 0;
                    int totalCEOAcceptedCount = 0;

                    List<ApplicableRoleType> approletypes = null;
                    ApplicableRoleType approletype = null;
                    ApplicableRoleType modifiedapproletype = null;

                    List<ApplicableRoleType> SentToHOD = null;
                    List<ApplicableRoleType> SentToHR = null;
                    string date = string.Empty;
                    List<int> applicableroletypeIds;

                    foreach (Int32 depatmentID in departmentIds)
                    {
                        approletypes = applicableRoleTypes.Where(c => c.DepartmentId == depatmentID)
                            .OrderByDescending(c => c.CreatedDate).ToList();

                        applicableroletypeIds = approletypes.GroupBy(c => c.ApplicableRoleTypeId).Select(c => c.Key).ToList<int>();

                        var definitions = await m_kraContext.Definitions
                                               .Where(x => applicableroletypeIds.Contains(x.ApplicableRoleTypeId)
                                               && x.IsHODApproved == true).ToListAsync();

                        if (definitions != null && definitions.Count > 0) totalIsHodApprovedCount = definitions.GroupBy(c => c.ApplicableRoleTypeId).Count();
                        if (approletypes != null && approletypes.Count > 0)
                        {
                            model = new KRAStatusModel();
                            modifiedapproletype = approletypes.Where(c => c.ModifiedDate.HasValue).OrderByDescending(c => c.ModifiedDate).FirstOrDefault();
                            approletype = approletypes[0];


                            model.DepartmentId = approletype.DepartmentId;
                            model.DepartmentName = (from dept in departments.Items
                                                    where approletype.DepartmentId == dept.DepartmentId
                                                    select dept.Description).FirstOrDefault();

                            model.TotalRoleTypes = approletypes.Count();

                            acceptedRoletypes = applicableRoleTypes.Where(x => x.DepartmentId == approletype.DepartmentId
                                                     && x.StatusId == StatusConstants.ApprovedbyHOD).ToList();

                            //var roletypes = await m_kraContext.ApplicableRoleTypes
                            //                        .Where(x => applicableroletypeIds.Contains(x.ApplicableRoleTypeId)                                                    
                            //                        && x.StatusId== StatusConstants.FinishedEditByHOD
                            //                        ).ToListAsync();

                            //if (roletypes != null && roletypes.Count > 0) finishedEditByHODCount = roletypes.GroupBy(c => c.ApplicableRoleTypeId).Count();

                            definitions = await m_kraContext.Definitions
                                                  .Where(x => applicableroletypeIds.Contains(x.ApplicableRoleTypeId)
                                                  && x.IsHODApproved.Value!=true
                                                  && x.ApplicableRoleType.StatusId >= StatusConstants.FinishedEditByHOD).ToListAsync();

                            if (definitions != null && definitions.Count > 0) finishedEditByHODCount = definitions.GroupBy(c => c.ApplicableRoleTypeId).Count();


                            if (acceptedRoletypes != null)
                            {
                                totalacceptedCount = acceptedRoletypes.Count();
                                model.AcceptedRoleTypes = totalacceptedCount == 0 ? totalIsHodApprovedCount : totalacceptedCount;
                            }
                            sentTOHODRoletypes = applicableRoleTypes.Where(x => x.StatusId == StatusConstants.SentToHOD).ToList();
                            sentToCEOCount = applicableRoleTypes.Where(x => x.StatusId == StatusConstants.SendToCEO).Count();
                            totalCEOAcceptedCount= applicableRoleTypes.Where(x => x.StatusId == StatusConstants.ApprovedByCEO).Count();
                            if (sentTOHODRoletypes != null) sentToHODCount = sentTOHODRoletypes.Count();
                            if (sentTOHODRoletypes != null) sentToHODCount = sentTOHODRoletypes.Count();
                            model.ForReviewRoleTypes = finishedEditByHODCount;
                            if (modifiedapproletype != null && modifiedapproletype.ModifiedDate.HasValue)
                            {
                                if (modifiedapproletype.ModifiedDate.Value > approletype.CreatedDate.Value)
                                    model.Date = modifiedapproletype.ModifiedDate.Value.ToLongDateString();
                                else model.Date = approletype.CreatedDate.Value.ToLongDateString();
                            }
                            else model.Date = approletype.CreatedDate.Value.ToLongDateString();

                            //draftStatus = approletypes.Where(x => x.StatusId == StatusConstants.Draft).Count();

                            //finishedDrafting = approletypes.Where(x => x.StatusId == StatusConstants.FinishedDrafting).ToList();
                            SentToHOD = approletypes.Where(x => x.StatusId == StatusConstants.SentToHOD).ToList();
                            SentToHR = approletypes.Where(x => x.StatusId == StatusConstants.SentToHR).ToList();
                            if (SentToHOD != null) totalsendtoHODCount = SentToHOD.Count();
                            if (SentToHR != null) totalsendtoHRCount = SentToHR.Count();

                            if (model.TotalRoleTypes == sentToCEOCount)
                            {
                                model.Status = "KRAs Sent to CEO";
                                model.Action = "Sent to CEO";
                            }
                            else if (model.TotalRoleTypes == totalsendtoHODCount)
                            {
                                model.Status = "KRAs Received from Head HR";
                                model.Action = "Start Review";
                            }
                            else if (model.TotalRoleTypes == totalsendtoHRCount)
                            {
                                model.Status = "KRAs Sent to HR";
                                model.Action = "Sent to HR";
                            }
                            else if (model.TotalRoleTypes == totalCEOAcceptedCount)
                            {
                                model.Status = "Completed";
                                model.Action = "";
                            }
                            else if (model.TotalRoleTypes == totalIsHodApprovedCount || model.TotalRoleTypes == finishedEditByHODCount
                                || model.TotalRoleTypes == totalIsHodApprovedCount + finishedEditByHODCount)
                            {
                                model.Status = "Ready to be send to HR";
                                model.Action = "Send to HR";
                            }
                            else if (model.TotalRoleTypes != totalIsHodApprovedCount)
                            {
                                model.Status = "Review is in progress";
                                model.Action = "Send to HR";
                            }
                            lstStatus.Add(model);
                        }
                    }

                    if (lstStatus.Count > 0)
                    {
                        response.Items = lstStatus;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "No KRAs sent for review!";
                    }
                }
                else
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "No KRAs sent for review!";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Items = null;
                response.Message = "Error occured while fetching KRA Status details.";
                m_Logger.LogError("Error occured while fetching KRA Status details." + ex.StackTrace);
            }
            return response;
            */
        }
        #endregion

        #region GetStatusByFinancialYearIdForCEO
        /// <summary>
        /// Gets Status by FinancialYearId for CEO.
        /// </summary>
        ///  <param name="financialYearId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<KRAStatusModel>> GetKRAStatusByFinancialYearIdForCEO(int financialYearId)
        {
            var response = new ServiceListResponse<KRAStatusModel>();
            List<KRAStatusModel> lstStatus = new List<KRAStatusModel>();
            //KRAStatusModel model = null;
            try
            {
                m_Logger.LogInformation("KRAStatusService: Calling \"GetStatusByFinancalYearIdForCEO\" method.");

                var departments = await m_OrganizationService.GetAllDepartmentsAsync();
                if (!departments.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Items = null;
                    response.Message = departments.Message;
                    return response;
                }

                ////Gets Applicable Roletypes for the passed financial year.
                //var applicableRoleTypes = await m_kraContext.ApplicableRoleTypes
                //                                .Where(x => x.FinancialYearId == financialYearId).ToListAsync();

                //if (applicableRoleTypes != null || applicableRoleTypes.Count != 0)
                //{
                //    List<int> departmentIds = applicableRoleTypes.GroupBy(c => c.DepartmentId).Select(c => c.Key).ToList<int>();
                //    List<ApplicableRoleType> approletypes = null;
                //    List<ApplicableRoleType> approletypesSendToCEO = null;
                //    string departmentHeadName = "";
                //    var employeeNames = await m_EmployeeService.GetEmployeeNames();
                    
                //    foreach (int departmentId in departmentIds)
                //    {
                //        approletypes = applicableRoleTypes.Where(c => c.DepartmentId == departmentId).ToList();
                //        approletypesSendToCEO = approletypes.Where(c => c.StatusId >= StatusConstants.SendToCEO).ToList();                       

                //        if (employeeNames != null && employeeNames.IsSuccessful)
                //        {
                //            departmentHeadName = (from dept in departments.Items
                //                                  join emp in employeeNames.Items on dept.DepartmentHeadId equals emp.EmpId
                //                                  where dept.DepartmentId == departmentId
                //                                  select emp.EmpName).FirstOrDefault();
                //        }

                //        model = new KRAStatusModel();

                //        model.DepartmentId = departmentId;
                //        model.DepartmentName = (from dept in departments.Items
                //                                where dept.DepartmentId == departmentId
                //                                select dept.Description).SingleOrDefault();
                //        model.DepartmentHeadName = departmentHeadName == null ? "" : departmentHeadName;

                //        if (approletypes != null && approletypes.Count > 0)
                //        {
                //            model.TotalRoleTypes = approletypesSendToCEO.Count();
                //            model.ForReviewRoleTypes = approletypes.Where(x => x.StatusId == StatusConstants.RejectedByCEO).Count();
                //            model.AcceptedRoleTypes = approletypes.Where(x => x.StatusId == StatusConstants.ApprovedByCEO).Count();
                //            if (model.AcceptedRoleTypes > 0)
                //                model.Status = "Completed";
                //            else if (model.ForReviewRoleTypes > 0)
                //                model.Status = "Rejected";
                //            else if (approletypesSendToCEO.Count > 0)
                //                model.Status = "KRA Recevied";                           
                //        }
                //        else if (approletypes.Count > 0 && approletypes != null)
                //        {
                //            model.TotalRoleTypes = approletypes.Count();
                //            model.Status = null;
                //        }
                //        lstStatus.Add(model);
                //    }
                //    if (lstStatus.Count > 0)
                //    {
                //        response.Items = lstStatus;
                //        response.IsSuccessful = true;
                //    }
                //    else
                //    {
                //        response.Items = null;
                //        response.IsSuccessful = false;
                //        response.Message = "No KRA status details found for this financial year!";
                //    }
                //}
                //else
                //{
                //    response.Items = null;
                //    response.IsSuccessful = false;
                //    response.Message = "No KRA status details found for this financial year!";
                //}
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Items = null;
                response.Message = "Error occured while fetching Status details.";
                m_Logger.LogError("Error occured while fetching Status details." + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region UpdateRoleTypeStatusForCEO
        /// <summary>
        /// UpdateRoleTypeStatusForCEO -- CEO Accepts/Rejects
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <param name="departmentId"></param>
        /// <param name="isAccepted"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> UpdateRoleTypeStatusForCEO(int financialYearId, int departmentId, bool isAccepted)
        {
            throw new NotImplementedException();
            /*
            var response = new BaseServiceResponse();
            bool isUpdated = false;
            try
            {
                m_Logger.LogInformation("KRAStatusService: Calling \"UpdateRoleTypeStatusForCEO\" method.");

                // Gets records from applicableroletypes table with status 'SendToCEO' for the passed financial year and department.
                var applicableroletypes = m_kraContext.ApplicableRoleTypes.
                                 Where(x => x.FinancialYearId == financialYearId
                                 && x.DepartmentId == departmentId
                                 && x.StatusId == StatusConstants.SendToCEO).ToList();

                if (applicableroletypes.Count() == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "Applicable RoleTypes not found.";
                    return response;
                }

                var department = await m_OrganizationService.GetById(departmentId);
                if (department == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "Department not found.";
                    return response;
                }

                string notificationCode = string.Empty;
                NotificationConfiguration emailNotificationConfig = null;

                List<int> appRoleTypeIds = applicableroletypes.GroupBy(c => c.ApplicableRoleTypeId).Select(c => c.Key).ToList<int>();

                var definitions = m_kraContext.Definitions.
                    Where(x => appRoleTypeIds.Contains(x.ApplicableRoleTypeId)).ToList();

                if (definitions.Count() == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "Definitions not found.";
                    return response;
                }
                using (var transaction = m_kraContext.Database.BeginTransaction())
                {
                    foreach (Definition def in definitions)
                    {
                        if (isAccepted) def.IsCEOApproved = true;
                        else def.IsCEOApproved = false;
                    }

                    foreach (ApplicableRoleType art in applicableroletypes)
                    {
                        if (isAccepted) art.StatusId = StatusConstants.ApprovedByCEO;
                        else art.StatusId = StatusConstants.RejectedByCEO;
                    }

                    isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                    if (!isUpdated)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while updating Definition table!";
                        return response;
                    }                   
                    //get email content
                    if (department.Description.Contains("Delivery")) notificationCode = "ApprovedByCEODelivery";
                    else if (department.Description.Contains("Service")) notificationCode = "ApprovedByCEOService";
                    else if (department.Description.Contains("Finance")) notificationCode = "ApprovedByCEOFinance";

                    var emailNotification = await m_OrganizationService.GetNotificationConfiguration(notificationCode, (int)CategoryMaster.KRA);

                    if (!emailNotification.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = emailNotification.Message;
                        return response;
                    }
                    emailNotificationConfig = emailNotification.Item;

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

                    ServiceResponse<bool> emailStatus = await SendMail(emailNotificationConfig);
                    if (!emailStatus.IsSuccessful)
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while sending email";
                        return response;
                    }
                    transaction.Commit();
                }
                response.IsSuccessful = true;
                response.Message = "Records updated successfully!";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while Updating Definition table";
                m_Logger.LogError("Error occurred while Updating Definition table" + ex.StackTrace);
                return response;
            }
            */
        }

        #endregion

        #region Private.Methods

        private async Task<ServiceResponse<bool>> ConfigureAndSendEmail(string emailFrom, string emailTo, string emailCC, string emailSubject, string emailContent)
        {
            NotificationConfiguration emailNotificationConfig = new NotificationConfiguration();
            emailNotificationConfig.EmailFrom = emailFrom;
            emailNotificationConfig.EmailTo = emailTo;
            emailNotificationConfig.EmailCC = emailCC;
            emailNotificationConfig.EmailSubject = emailSubject;
            emailNotificationConfig.EmailContent = emailContent;

            ServiceResponse<bool> emailStatus = await SendMail(emailNotificationConfig);

            return emailStatus;
        }

        private async Task<ServiceResponse<bool>> SendMail(NotificationConfiguration emailNotificationConfig)
        {
            NotificationDetail notificationDetail = new NotificationDetail();
            //StringBuilder emailContentEncode = new StringBuilder(WebUtility.HtmlEncode("Test from DEV <!DOCTYPE html><html><head><style>table {​​​​ font-family: arial, sans-serif; border-collapse: collapse; }​​​ td, th {​​​​ border: 1px solid #dddddd; text-align: left; padding: 8px; }​​​​td:nth-child(1) {​​​​ background-color: #dddddd; }​​​​</style></head> <body><br>KRA Definition waiting for approval</body>"));

            //StringBuilder emailContentDecode = new StringBuilder(WebUtility.HtmlDecode(emailContentEncode.ToString()));
            StringBuilder emailContent = new StringBuilder(WebUtility.HtmlDecode(emailNotificationConfig.EmailContent));
            notificationDetail.FromEmail = emailNotificationConfig.EmailFrom;
            notificationDetail.ToEmail = emailNotificationConfig.EmailTo;
            notificationDetail.CcEmail = emailNotificationConfig.EmailCC;
            notificationDetail.Subject = emailNotificationConfig.EmailSubject;
            notificationDetail.EmailBody = emailContent.ToString();// "KRA Definition waiting for approval";// emailNotificationConfig.EmailContent;

            var emailStatus = await m_OrganizationService.SendEmailAsync(notificationDetail);
            return emailStatus;
        }
        
        #endregion
    }
}
