using HRMS.Common;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    public class AssociateExitInterviewReviewService : IAssociateExitInterviewReviewService
    {
        #region Global Varibles
        private readonly ILogger<IAssociateExitInterviewReviewService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        #endregion

        #region Constructor
        public AssociateExitInterviewReviewService(EmployeeDBContext employeeDBContext,
            ILogger<AssociateExitInterviewReviewService> logger)
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets Associate Exit Interview Review
        /// </summary>
        /// <param name="exitInterviewReviewRequest"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<ExitInterviewReviewGetAllResponse>> GetAll(ExitInterviewReviewGetAllRequest exitInterviewReviewRequest)
        {
            var response = new ServiceListResponse<ExitInterviewReviewGetAllResponse>();
            List<ExitInterviewReviewGetAllResponse> lstExitInterviewReviewResponse = new List<ExitInterviewReviewGetAllResponse>();
            try
            {
                var lstExitInterview = await (from ai in m_EmployeeContext.AssociateExitInterview
                                        join ae in m_EmployeeContext.AssociateExit on ai.AssociateExitId equals ae.AssociateExitId
                                        where ai.CreatedDate.HasValue == true && ai.CreatedDate.Value.Date >= exitInterviewReviewRequest.FromDate.Date
                                              && ae.CreatedDate.Value.Date < exitInterviewReviewRequest.ToDate.AddDays(1).Date
                                        select new { AssociateExitInterviewId = ai.AssociateExitInterviewId,
                                            AssociateExitId = ae.AssociateExitId,
                                            EmployeeId = ae.EmployeeId,
                                            Remarks = ai.Remarks }).ToListAsync();
                                                                                                    
                if (lstExitInterview != null && lstExitInterview.Count > 0)
                {
                    List<int> interviewIDs = lstExitInterview.Select(ei => ei.AssociateExitInterviewId).Distinct().ToList();

                    var lstExitInterviewReview = await m_EmployeeContext.AssociateExitInterviewReview.Where(ae => interviewIDs.Contains(ae.AssociateExitInterviewId)).ToListAsync();

                    List<int> lstEmployeeIDs = lstExitInterview.Select(ei => ei.EmployeeId).Distinct().ToList();

                    var lstExitEmployees = await m_EmployeeContext.Employees.Where(ae => lstEmployeeIDs.Contains(ae.EmployeeId)).ToListAsync();

                    foreach (var exitInterview in lstExitInterview)
                    {
                        var exitInterviewReview = lstExitInterviewReview.Where(ae => ae.AssociateExitInterviewId == exitInterview.AssociateExitInterviewId).FirstOrDefault();

                        ExitInterviewReviewGetAllResponse exitIntReviewResponse = new ExitInterviewReviewGetAllResponse();
                        exitIntReviewResponse.AssociateExitInterviewId = exitInterview.AssociateExitInterviewId;
                        exitIntReviewResponse.AssociateExitId = exitInterview.AssociateExitId;
                        //Get Associate Name
                        exitIntReviewResponse.AssociateName = lstExitEmployees.Where(e => e.EmployeeId == exitInterview.EmployeeId)
                                                                                            .Select(e => e.FirstName + " " + e.LastName).FirstOrDefault();
                        exitIntReviewResponse.InitialRemarks = String.IsNullOrEmpty(exitInterview.Remarks)
                                                                    ? "" : (IsBase64(exitInterview.Remarks)? Utility.DecryptStringAES(exitInterview.Remarks) : "");
                        exitIntReviewResponse.InitialRemarksNoHtml = ExcludeHtmlTags(exitIntReviewResponse.InitialRemarks);

                        if (exitInterviewReview is { })
                        {
                            exitIntReviewResponse.FinalRemarks = String.IsNullOrEmpty(exitInterviewReview.FinalRemarks)
                                                                            ? "" : (IsBase64(exitInterviewReview.FinalRemarks) ? Utility.DecryptStringAES(exitInterviewReview.FinalRemarks) : "");
                            exitIntReviewResponse.FinalRemarksNoHtml = ExcludeHtmlTags(exitIntReviewResponse.FinalRemarks);
                            exitIntReviewResponse.ShowInitialRemarks = exitInterviewReview.ShowInitialRemarks;
                        }
                        else
                        {
                            exitIntReviewResponse.FinalRemarks = "";
                            exitIntReviewResponse.FinalRemarksNoHtml = "";
                            exitIntReviewResponse.ShowInitialRemarks = true;
                        }

                        lstExitInterviewReviewResponse.Add(exitIntReviewResponse);
                    }
                }

                response.Items = lstExitInterviewReviewResponse;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Items = lstExitInterviewReviewResponse;
                response.IsSuccessful = false;
                response.Message = "Associate Exit Interview Reviews not found.";

                m_Logger.LogError($"{nameof(GetAll)}: Error occured in AssociateExitInterviewReviewService {ex?.StackTrace}");
            }

            return response;
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates Associate Exit Interview Review
        /// </summary>
        /// <param name="exitInterviewReviewRequest"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> Create(ExitInterviewReviewCreateRequest exitInterviewReviewRequest)
        {
            var response = new ServiceResponse<int>();
            try
            {
                int isCreated = 0;
                m_Logger.LogInformation("Calling \"Create\" method in AssociateExitInterviewReviewService");
                var exitIntReview = await m_EmployeeContext.AssociateExitInterviewReview.FirstOrDefaultAsync(ae => ae.AssociateExitInterviewId == exitInterviewReviewRequest.AssociateExitInterviewId);
                if (exitIntReview == null)
                {
                    AssociateExitInterviewReview newExitIntReview = new AssociateExitInterviewReview();
                    newExitIntReview.AssociateExitInterviewId = exitInterviewReviewRequest.AssociateExitInterviewId;
                    newExitIntReview.ShowInitialRemarks = exitInterviewReviewRequest.ShowInitialRemarks;
                    newExitIntReview.FinalRemarks = String.IsNullOrEmpty(exitInterviewReviewRequest.FinalRemarks)
                                                 ? "" : Utility.EncryptStringAES(exitInterviewReviewRequest.FinalRemarks);

                    m_EmployeeContext.AssociateExitInterviewReview.Add(newExitIntReview);
                    isCreated = await m_EmployeeContext.SaveChangesAsync();
                }
                else
                {
                    exitIntReview.FinalRemarks = String.IsNullOrEmpty(exitInterviewReviewRequest.FinalRemarks)
                                                 ? "" : Utility.EncryptStringAES(exitInterviewReviewRequest.FinalRemarks);
                    exitIntReview.ShowInitialRemarks = exitInterviewReviewRequest.ShowInitialRemarks;

                    isCreated = await m_EmployeeContext.SaveChangesAsync();
                }

                if (isCreated > 0)
                {
                    response.Item = isCreated;
                    response.IsSuccessful = true;
                    response.Message = $"Associate Exit Interview Review Updated Successfully";
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
            catch (Exception)
            {
                response.Item = 0;
                response.IsSuccessful = false;
                response.Message = $"Error Occured while Updating Associate Exit Interview Review";
                return response;
            }
        }
        #endregion

        #region Private Methods
        private string ExcludeHtmlTags(string value)
        {
            value = value.Replace("&#160;", " ");
            string regex = @"<[^>]+>| ";
            string result = Regex.Replace(value, regex, "").Trim(); 
            return result;
        }

        private bool IsBase64(string inputString)
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
    }
}
