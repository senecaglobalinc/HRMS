using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AssociateExitInterviewReviewController : Controller
    {
        #region Global Variables

        private readonly IAssociateExitInterviewReviewService m_AssociateExitIntReviewService;
        private readonly ILogger<AssociateExitInterviewReviewController> m_Logger;

        #endregion

        #region Constructor
        public AssociateExitInterviewReviewController(IAssociateExitInterviewReviewService associateExitIntReviewService, ILogger<AssociateExitInterviewReviewController> logger)
        {
            m_AssociateExitIntReviewService = associateExitIntReviewService;
            m_Logger = logger;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets Associate Exit Interview Review
        /// </summary>
        /// <param name="exitInterviewReviewRequest"></param>
        /// <returns>Associate Exit Interview Review</returns>
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(ExitInterviewReviewGetAllRequest exitInterviewReviewRequest)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Fetching records from AssociateExitInterview & AssociateExitInterviewReview tables.");
            try
            {
                var response = await m_AssociateExitIntReviewService.GetAll(exitInterviewReviewRequest);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while fetching AssociateExitInterviewReview: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAll() in AssociateExitInterviewReviewController:" + stopwatch.Elapsed);
                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully fetched records from AssociateExitInterview & AssociateExitInterviewReview tables.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAll() in AssociateExitInterviewReviewController:" + stopwatch.Elapsed);
                    return Ok(response.Items);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while fetching AssociateExitInterviewReview: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetAll() in AssociateExitInterviewReviewController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while fetching AssociateExitInterviewReview in GetAll method.");
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates Associate Exit Interview Review
        /// </summary>
        /// <param name="exitInterviewReviewRequest"></param>
        /// <returns>Integer value 0-represents UnSuccessful response and >0- represents Successful response</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ExitInterviewReviewCreateRequest exitInterviewReviewRequest)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Updating record in AssociateExitInterviewReview table.");
            try
            {
                var response = await m_AssociateExitIntReviewService.Create(exitInterviewReviewRequest);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating AssociateExitInterviewReview: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in AssociateExitInterviewReviewController:" + stopwatch.Elapsed);
                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in AssociateExitInterviewReview table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in AssociateExitInterviewReviewController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while updating AssociateExitInterviewReview: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Create() in AssociateExitInterviewReviewController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while updating AssociateExitInterviewReview in Create method.");
            }
        }
        #endregion 
    }
}
