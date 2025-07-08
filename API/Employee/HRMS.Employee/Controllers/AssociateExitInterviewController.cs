using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AssociateExitInterviewController : ControllerBase
    {
        #region Global Variables

        private readonly IAssociateExitInterviewService m_AssociateExitInterviewService;
        private readonly ILogger<AssociateExitInterviewController> m_Logger;

        #endregion

        #region Constructor
        public AssociateExitInterviewController(IAssociateExitInterviewService associateExitInterviewService,
            ILogger<AssociateExitInterviewController> logger)
        {
            m_AssociateExitInterviewService = associateExitInterviewService;
            m_Logger = logger;
        }
        #endregion

        #region CreateExitFeedback
        /// <summary>
        /// Create Exit Feedback form
        /// </summary>
        /// <param name="interviewRequest"></param>
        /// <returns>Integer value 0-represents UnSuccessful response and >0- represents Successful response</returns>
        [HttpPost("CreateExitFeedback")]
        public async Task<IActionResult> CreateExitFeedback(ExitInterviewRequest interviewRequest)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Updating record in AssociateExitInterview table.");
            try
            {
                var response = await m_AssociateExitInterviewService.CreateExitFeedback(interviewRequest);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating AssociateExitInterview: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute CreateExitFeedback() in AssociateExitInterviewController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in AssociateExitInterview table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute CreateExitFeedback() in AssociateExitInterviewController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while updating AssociateExitInterview: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute CreateExitFeedback() in AssociateExitInterviewController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while updating AssociateExitInterview in CreateExitFeedback method.");
            }
        }
        #endregion

        #region GetExitInterview
        /// <summary>
        /// Get Exit Feedback form
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>Associate Exit Interview</returns>
        [HttpGet("GetExitInterview/{employeeId}")]
        public async Task<IActionResult> GetExitInterview(int employeeId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Fetching record from AssociateExitInterview table.");
            try
            {
                var response = await m_AssociateExitInterviewService.GetExitInterview(employeeId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while fetching AssociateExitInterview: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetExitInterview() in AssociateExitInterviewController:" + stopwatch.Elapsed);
                    return NotFound(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully fetched record from AssociateExitInterview table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetExitInterview() in AssociateExitInterviewController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while fetching AssociateExitInterview: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetExitInterview() in AssociateExitInterviewController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while fetching AssociateExitInterview in GetExitInterview method.");
            }
        }
        #endregion
    }
}
