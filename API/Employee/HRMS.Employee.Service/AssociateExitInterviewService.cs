using AutoMapper;
using HRMS.Common;
using HRMS.Common.Enums;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    public class AssociateExitInterviewService : IAssociateExitInterviewService
    {
        #region Global Varibles

        private readonly ILogger<AssociateExitInterviewService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;
        private readonly IOrganizationService m_OrgService;
        private readonly IAssociateExitService m_AssociateExitService;
        #endregion

        #region Constructor
        public AssociateExitInterviewService(
            ILogger<AssociateExitInterviewService> logger,
            EmployeeDBContext employeeDBContext,
            IOrganizationService orgService,
            IAssociateExitService associateExitService
            )
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AssociateExitInterviewService, AssociateExitInterviewService>();
            });
            m_mapper = config.CreateMapper();

            m_OrgService = orgService;
            m_AssociateExitService = associateExitService;
        }

        #endregion

        #region CreateExitFeedback
        /// <summary>
        /// Create an Exit Feedback Form
        /// </summary>
        /// <param name="interviewRequest"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> CreateExitFeedback(ExitInterviewRequest interviewRequest)
        {
            var response = new ServiceResponse<int>();
            ServiceResponse<Status> interviewStatus = new ServiceResponse<Status>();
            try
            {
                var exitDetails = await m_EmployeeContext.AssociateExit.FirstOrDefaultAsync(st => st.EmployeeId == interviewRequest.EmployeeId && st.IsActive == true);
                if (exitDetails == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"No employee existes with employeeId {interviewRequest.EmployeeId}";
                    return response;
                }

                interviewStatus = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.AssociateExit.ToString(), AssociateExitStatusCodesNew.ExitInterviewCompleted.ToString());
                if (interviewStatus.Item == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"No Exit Interview Completed Status found in ORG Service";
                    return response;
                }

                AssociateExitInterview associateExitInterview = new AssociateExitInterview
                {
                    AssociateExitId = exitDetails.AssociateExitId,
                    ReasonDetail = interviewRequest.ReasonDetail,
                    ReasonId = interviewRequest.ReasonId,
                    IsActive = true,
                    ShareEmploymentInfo = interviewRequest.ShareEmploymentInfo,
                    IncludeInExAssociateGroup = interviewRequest.IncludeInExAssociateGroup,
                    Remarks = string.IsNullOrEmpty(interviewRequest.Remarks) ? null : Utility.EncryptStringAES(interviewRequest.Remarks),
                    SystemInfo = interviewRequest.SystemInfo,
                    AlternateAddress = interviewRequest.AlternateAddress,
                    AlternateEmail = interviewRequest.AlternateEmail,
                    AlternateMobileNo = interviewRequest.AlternateMobileNo,
                    IsNotified = interviewRequest.IsNotified
                };

                await m_EmployeeContext.AssociateExitInterview.AddAsync(associateExitInterview);

                exitDetails.StatusId = Convert.ToInt32(AssociateExitStatusCodesNew.ResignationInProgress);
                //Saving Details to database
                int created = await m_EmployeeContext.SaveChangesAsync();

                if (created > 0)
                {
                    //Get Clearance Status by checking all the stake holders
                    ServiceResponse<bool> readyForCleranceResponse = await m_AssociateExitService.AssociateClearanceStatus(interviewRequest.EmployeeId);
                    if (readyForCleranceResponse.IsSuccessful && readyForCleranceResponse.Item)
                    {
                        m_Logger.LogInformation("Updating AssociateExit - Status column to ReadyForClearance. Got clerance from all stakeholders");

                        interviewStatus = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.AssociateExit.ToString(), AssociateExitStatusCodesNew.ReadyForClearance.ToString());
                        exitDetails.StatusId = interviewStatus.Item.StatusId;
                        _ = await m_EmployeeContext.SaveChangesAsync();
                    }
                }

                if (created > 0)
                {
                    ServiceResponse<int> notification = await m_AssociateExitService.AssociateExitSendNotification(exitDetails.EmployeeId, Convert.ToInt32(NotificationType.ExitInterviewCompleted), null, null, null, interviewRequest.IsNotified ? interviewRequest.Remarks : null);
                    if (!notification.IsSuccessful)
                    {
                        response.Item = 0;
                        response.IsSuccessful = false;
                        response.Message = notification.Message;
                        return response;
                    }

                    response.Item = created;
                    response.IsSuccessful = true;
                    response.Message = $"Associate Feedback Updated Successfully";
                    return response;
                }
                else
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"Error Occured While Updating Database";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Item = 0;
                response.IsSuccessful = false;
                response.Message = $"Error Occured while Updating Associate Feedback";
                return response;
            }
        }
        #endregion

        #region GetExitInterview
        /// <summary>
        /// Get an Exit Feedback Form
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<ExitInterviewRequest>> GetExitInterview(int employeeId)
        {
            var response = new ServiceResponse<ExitInterviewRequest>();
            ExitInterviewRequest associateExitInterview = null;

            try
            {
                int resignedStatus = Convert.ToInt32(AssociateExitStatusCodesNew.Resigned);
                var exitDetails = await m_EmployeeContext.AssociateExit.FirstOrDefaultAsync(pa => pa.EmployeeId == employeeId && ((pa.IsActive == true) || (pa.StatusId == resignedStatus && pa.IsActive == false)));
                if (exitDetails == null)
                {
                    response.Item = null;
                    response.IsSuccessful = false;
                    response.Message = $"No employee existes with employeeId {employeeId}";
                    return response;
                }

                var interviewDetail = await m_EmployeeContext.AssociateExitInterview.FirstOrDefaultAsync(st => st.AssociateExitId == exitDetails.AssociateExitId);
                if (interviewDetail != null)
                {
                    associateExitInterview = new ExitInterviewRequest
                    {
                        AssociateExitId = exitDetails.AssociateExitId,
                        ReasonDetail = interviewDetail.ReasonDetail,
                        ReasonId = interviewDetail.ReasonId,
                        EmployeeId = employeeId,
                        ShareEmploymentInfo = interviewDetail.ShareEmploymentInfo,
                        IncludeInExAssociateGroup = interviewDetail.IncludeInExAssociateGroup,
                        Remarks = string.IsNullOrEmpty(interviewDetail.Remarks) ? null : Utility.DecryptStringAES(interviewDetail.Remarks),
                        SystemInfo = interviewDetail.SystemInfo,
                        AlternateAddress = interviewDetail.AlternateAddress,
                        AlternateEmail = interviewDetail.AlternateEmail,
                        AlternateMobileNo = interviewDetail.AlternateMobileNo
                    };
                }                

                response.Item = associateExitInterview;
                response.IsSuccessful = true;
                response.Message = $"Associate Feedback Fetched Successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Item = null;
                response.IsSuccessful = false;
                response.Message = $"Error Occured while Fetching Associate Feedback";
                return response;
            }
        }
        #endregion
    }
}
