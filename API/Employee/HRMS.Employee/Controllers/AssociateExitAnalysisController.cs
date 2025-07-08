using System;
using System.Diagnostics;
using System.Threading.Tasks;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AssociateExitAnalysisController : ControllerBase
    {
        #region Global Variables

        private readonly IAssociateExitAnalysisService m_AssociateExitAnalysisService;
        private readonly ILogger<AssociateExitAnalysisController> m_Logger;

        #endregion

        #region Constructor
        public AssociateExitAnalysisController(IAssociateExitAnalysisService associateExitAnalysisService,
            ILogger<AssociateExitAnalysisController> logger)
        {
            m_AssociateExitAnalysisService = associateExitAnalysisService;
            m_Logger = logger;
        }
        #endregion


        #region GetAssociateExitAnalysis
        /// <summary>
        /// This method fetches the associateExit Analysis Details
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns>associateExit Analysis Details</returns>
        [HttpGet("GetAssociateExitAnalysis")]
        public async Task<ActionResult<GetExitAnalysis>> GetAssociateExitAnalysis(DateTime? fromDate, DateTime? toDate, int? employeeId)
        {
            m_Logger.LogInformation("Retrieving records from associateExit and AssociateExitAnalysis table.");

            try
            {
                var deptact = await m_AssociateExitAnalysisService.GetAssociateExitAnalysis(fromDate, toDate, employeeId);
                if (!deptact.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in associateExit table.");
                    return Ok(deptact.Items);
                }
                else
                {
                    m_Logger.LogInformation($"Returning associateExit Analysis Details.");
                    return Ok(deptact.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation($"Error Occured in GetAssociateExitAnalysis() method");
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion
        #region CreateExitFeedback
        /// <summary>
        /// Create Exit Analysis form
        /// </summary>
        /// <param name="interviewRequest"></param>
        /// <returns>Integer value 0-represents UnSuccessful response and >0- represents Successful response</returns>
        [HttpPost("CreateExitAnalysis")]
        public async Task<IActionResult> CreateExitAnalysis(ExitAnalysis exitRequest)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Updating record in AssociateExitAnalysis table.");
            try
            {
                var response = await m_AssociateExitAnalysisService.CreateExitAnalysis(exitRequest);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating AssociateExitAnalysis record: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute CreateExitAnalysis() in m_AssociateExitAnalysisController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in AssociateExitAnalysisService table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute CreateExitAnalysis() in m_AssociateExitAnalysisController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while updating AssociateExitAnalysis: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute CreateExitAnalysis() in m_AssociateExitAnalysisController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while updating AssociateExitAnalysisService in CreateExitAnalysis method.");
            }
        }
        #endregion

    }
}
