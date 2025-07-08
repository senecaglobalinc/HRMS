using HRMS.Employee.API.Auth;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
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
    public class AssociateAbscondController : ControllerBase
    {
        #region Global Variables
        private readonly IAssociateAbscondService m_AssociateAbscondService;
        private readonly ILogger<AssociateAbscondController> m_Logger;
        #endregion

        #region Constructor
        public AssociateAbscondController(IAssociateAbscondService associateAbscondService,
            ILogger<AssociateAbscondController> logger)
        {
            m_AssociateAbscondService = associateAbscondService;
            m_Logger = logger;
        }
        #endregion

        #region GetAssociateByLead
        /// <summary>
        /// Get Associate By Lead and Dept
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="deptId"></param>
        /// <returns>The list of Employees belongs to the Lead.</returns>
        [HttpGet("GetAssociateByLead/{leadId}/{deptId}")]
        public async Task<ActionResult<AssociateModel>> GetAssociateByLead(int leadId, int deptId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation($"Retrieving Associates by LeadId : {leadId}.");

            try
            {
                var response = await m_AssociateAbscondService.GetAssociateByLead(leadId, deptId);
                if (response == null)
                {
                    m_Logger.LogInformation($"No Associate found for LeadId : {leadId}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAssociateByLead() in AssociateAbscondController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation($"Associates found for LeadId : {leadId}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAssociateByLead() in AssociateAbscondController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while getting Associates by LeadId in GetAssociateByLead method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetAssociateByLead() in AssociateAbscondController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while getting AssociateAbscond by LeadId in GetAssociateByLead method");
            }
        }
        #endregion

        #region GetAssociatesAbscondDashboard
        /// <summary>
        /// Gets abscond employees list by user role, employeeId and dashboard
        /// </summary>
        /// <param name="userRole"></param>
        /// <param name="employeeId"></param>
        /// <param name="departmentId"></param>
        /// <returns>The list of Abscoded Associates based on role</returns>
        [HttpGet("GetAssociatesAbscondDashboard/{userRole}/{employeeId}/{departmentId}")]
        public async Task<ActionResult<AbscondDashboardResponse>> GetAssociatesAbscondDashboard(string userRole, int employeeId, int departmentId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation($"Retrieving Abscond Dashboard Data.");

            try
            {
                var response = await m_AssociateAbscondService.GetAssociatesAbscondDashboard(userRole, employeeId, departmentId);
                if (response == null)
                {
                    m_Logger.LogInformation($"No Associates Abscond Dashboard Data found.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAssociatesAbscondDashboard() in AssociateAbscondController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation($"Associates Abscond Dashboard Data found.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAssociatesAbscondDashboard() in AssociateAbscondController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while getting Associates Abscond Dashboard Data GetAssociatesAbscondDashboard method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetAssociatesAbscondDashboard() in AssociateAbscondController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while getting Associate Abscond data in GetAssociatesAbscondDashboard method");
            }
        }
        #endregion

        #region GetAbscondDetailByAssociateId
        /// <summary>
        /// Get Abscond Detail by AssociateId
        /// </summary>
        /// <param name="associateId">associateId</param>
        /// <returns>The details of Associate by Id</returns>
        [HttpGet("GetAbscondDetailByAssociateId/{associateId}")]
        public async Task<ActionResult<AssociateAbscondModel>> GetAbscondDetailByAssociateId(int associateId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation($"Retrieving Associate Abscond record by AssociateId : {associateId}.");

            try
            {
                var response = await m_AssociateAbscondService.GetAbscondDetailByAssociateId(associateId);
                if (response == null)
                {
                    m_Logger.LogInformation($"No Associate Abscond record found for AssociateId : {associateId}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAbscondDetailByAssociateId() in AssociateAbscondController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation($"Associate Abscond record found for AssociateId : {associateId}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAbscondDetailByAssociateId() in AssociateAbscondController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while getting Associate Abscond record by AssociateId in GetAbscondDetailByAssociateId method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetAbscondDetailByAssociateId() in AssociateAbscondController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while getting AssociateAbscond by AssociateId in GetAbscondDetailByAssociateId method");
            }
        }
        #endregion

        #region CreateAbscond
        /// <summary>
        /// Create Abscond Request
        /// </summary>
        /// <remarks>
        /// Sample request :
        /// 
        ///     POST /CreateAbscond
        ///     {
        ///         "associateAbscondId": 0,
        ///         "associateId": 0,
        ///         "associateName": "string",
        ///         "absentFromDate": "2023-04-18T11:19:00.397Z",
        ///         "absentToDate": "2023-04-18T11:19:00.397Z",
        ///         "isAbscond": true,
        ///         "statusId": 0,
        ///         "tlId": 0,
        ///         "remarksByTL": "string",
        ///         "hraId": 0,
        ///         "remarksByHRA": "string",
        ///         "hrmId": 0,
        ///         "remarksByHRM": "string"
        ///     }
        ///     
        /// </remarks>
        /// <param name="abscondReq">abscondReq</param>
        /// <returns>A newly created abscond request</returns>
        /// <response code="200">Returns the newly created request</response>
        /// <response code="400">If the item is null</response>
        [HttpPost("CreateAbscond")]
        public async Task<ActionResult<bool>> CreateAbscond(AssociateAbscondModel abscondReq)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation($"Inserting Associate Abscond Details by Lead.");

            try
            {
                var response = await m_AssociateAbscondService.CreateAbscond(abscondReq);
                if (response == null)
                {
                    m_Logger.LogInformation($"No Associate abscond details inserted.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute CreateAbscond() in AssociateAbscondController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation($"Associates abscond details inserted successfully.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute CreateAbscond() in AssociateAbscondController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while inserting Abscond details in CreateAbscond method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute CreateAbscond() in AssociateAbscondController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while inserting Abscond details in CreateAbscond method");
            }
        }
        #endregion

        #region AcknowledgeAbscond
        /// <summary>
        /// Acknowledge Abscond Request
        /// </summary>
        /// <remarks>
        /// Sample request :
        /// 
        ///     POST /AcknowledgeAbscond
        ///     {
        ///         "associateAbscondId": 0,
        ///         "associateId": 0,
        ///         "associateName": "string",
        ///         "absentFromDate": "2023-04-18T11:19:00.397Z",
        ///         "absentToDate": "2023-04-18T11:19:00.397Z",
        ///         "isAbscond": true,
        ///         "statusId": 0,
        ///         "tlId": 0,
        ///         "remarksByTL": "string",
        ///         "hraId": 0,
        ///         "remarksByHRA": "string",
        ///         "hrmId": 0,
        ///         "remarksByHRM": "string"
        ///     }
        ///     
        /// </remarks>
        /// <param name="abscondReq">abscondReq</param>
        /// <returns>Acknowledge the created abscond request</returns>
        /// <response code="200">Returns the details of asconded request</response>
        /// <response code="400">If the item is null</response>
        [HttpPost("AcknowledgeAbscond")]
        public async Task<ActionResult<bool>> AcknowledgeAbscond(AssociateAbscondModel abscondReq)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation($"Acknowledging Abscond Details by HRA.");

            try
            {
                var response = await m_AssociateAbscondService.AcknowledgeAbscond(abscondReq);
                if (response == null)
                {
                    m_Logger.LogInformation($"No Associate abscond details acknowledged.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute AcknowledgeAbscond() in AssociateAbscondController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation($"Associate abscond details acknowledged successfully.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute AcknowledgeAbscond() in AssociateAbscondController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while acknowledging Abscond details in AcknowledgeAbscond method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute AcknowledgeAbscond() in AssociateAbscondController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while acknowledging Abscond details in AcknowledgeAbscond method");
            }
        }
        #endregion

        #region ConfirmAbscond
        /// <summary>
        /// Confirm/Disprove Abscond Request
        /// </summary>
        /// <remarks>
        /// Sample request :
        /// 
        ///     POST /ConfirmAbscond
        ///     {
        ///         "associateAbscondId": 0,
        ///         "associateId": 0,
        ///         "associateName": "string",
        ///         "absentFromDate": "2023-04-18T11:19:00.397Z",
        ///         "absentToDate": "2023-04-18T11:19:00.397Z",
        ///         "isAbscond": true,
        ///         "statusId": 0,
        ///         "tlId": 0,
        ///         "remarksByTL": "string",
        ///         "hraId": 0,
        ///         "remarksByHRA": "string",
        ///         "hrmId": 0,
        ///         "remarksByHRM": "string"
        ///     }
        /// </remarks>
        /// 
        /// <param name="abscondReq">abscondReq</param>
        /// <returns>Confirm/Disprove the abscond request</returns>
        /// <response code="200">Returns the details of asconded request</response>
        /// <response code="400">If the item is null</response>
        [HttpPost("ConfirmAbscond")]
        public async Task<ActionResult<bool>> ConfirmAbscond(AssociateAbscondModel abscondReq)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation($"Confirm/Disprove Abscond Details by HRM.");

            try
            {
                var response = await m_AssociateAbscondService.ConfirmAbscond(abscondReq);
                if (response == null)
                {
                    m_Logger.LogInformation($"No Associate abscond details Confirmed/Disproved.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute ConfirmAbscond() in AssociateAbscondController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation($"Associate abscond Confirmed/Disproved successfully.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute ConfirmAbscond() in AssociateAbscondController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while Confirming/Disproving Abscond in ConfirmAbscond method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute ConfirmAbscond() in AssociateAbscondController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while Confirming/Disproving Abscond in ConfirmAbscond method");
            }
        }
        #endregion 

        #region AbscondClearance
        /// <summary>
        /// Provide Clearance for Abscond Request
        /// </summary>
        /// <remarks>
        /// Sample request :
        /// 
        ///     POST /AbscondClearance
        ///     {
        ///         "associateAbscondId": 0,
        ///         "associateId": 0,
        ///         "associateName": "string",
        ///         "absentFromDate": "2023-04-18T11:19:00.397Z",
        ///         "absentToDate": "2023-04-18T11:19:00.397Z",
        ///         "isAbscond": true,
        ///         "statusId": 0,
        ///         "tlId": 0,
        ///         "remarksByTL": "string",
        ///         "hraId": 0,
        ///         "remarksByHRA": "string",
        ///         "hrmId": 0,
        ///         "remarksByHRM": "string"
        ///     }
        ///     
        /// </remarks>
        /// <param name="abscondReq">abscondReq</param>
        /// <returns>Provide clearance for the abscond request</returns>
        /// <response code="200">Returns the details of asconded request</response>
        /// <response code="400">If the item is null</response>
        [HttpPost("AbscondClearance")]
        public async Task<ActionResult<bool>> AbscondClearance(AssociateAbscondModel abscondReq)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation($"Providing Clearance Abscond Details by HRM.");

            try
            {
                var response = await m_AssociateAbscondService.AbscondClearance(abscondReq);
                if (response == null)
                {
                    m_Logger.LogInformation($"Clearance not provided to associate abscond details.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute AbscondClearance() in AssociateAbscondController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation($"Associate abscond clearance provided successfully.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute AbscondClearance() in AssociateAbscondController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while providing Abscond clearance in AbscondClearance method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute AbscondClearance() in AssociateAbscondController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while providing Abscond clearance in AbscondClearance method");
            }
        }
        #endregion

        #region GetAbscondSubStatus
        /// <summary>
        /// Get Abscond SubStatus
        /// </summary>
        /// <param name="associateId"></param>
        /// <returns></returns>
        [HttpGet("GetAbscondSubStatus/{associateId}")]
        public async Task<IActionResult> GetAbscondSubStatus(int associateId)
        {
            m_Logger.LogInformation("Executing GetAbscondSubStatus Action method.");
            try
            {
                var response = await m_AssociateAbscondService.AssociateAbscondWFStatus(associateId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while fetching status and substatus for associate: " + (string)response.Message);
                    return NotFound(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully fetched status and substatus for associate.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while fetching status and substatus for associate: " + ex.StackTrace);
                return BadRequest("Error occurred while fetching status and substatus.");
            }
        }
        #endregion
    }
}
