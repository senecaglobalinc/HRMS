using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Request;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AssociateExitController : ControllerBase
    {
        #region Global Variables

        private readonly IAssociateExitService m_AssociateExitService;
        private readonly ILogger<AssociateExitController> m_Logger;

        #endregion

        #region Constructor
        public AssociateExitController(IAssociateExitService associateExitService,
            ILogger<AssociateExitController> logger)
        {
            m_AssociateExitService = associateExitService;
            m_Logger = logger;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get Active Prospective Associates based on Departments, Designations and PracticeArea.
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool? isActive = true)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving records from AssociateExit table.");

            try
            {
                var associateExits = await m_AssociateExitService.GetAll(isActive);
                if (!associateExits.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in AssociateExit table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAll() in AssociateExitController:" + stopwatch.Elapsed);
                    return NotFound(associateExits.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning AssociateExit records.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAll() in AssociateExitController:" + stopwatch.Elapsed);
                    return Ok(associateExits.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while getting AssociateExit records in GetAll method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetAll() in AssociateExitController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while getting AssociateExit records in GetAll method");
            }
        }
        #endregion

        #region GetByEmployeeId
        /// <summary>
        /// GetByEmployeeId
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        [HttpGet("GetByEmployeeId/{EmployeeId}")]
        public async Task<ActionResult<AssociateExitRequest>> GetByEmployeeId(int EmployeeId, bool isDecryptReq = false)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation($"Retrieving records from Prospective Associates table by {EmployeeId}.");

            try
            {
                var associateExit = await m_AssociateExitService.GetByEmployeeId(EmployeeId, isDecryptReq);
                if (!associateExit.IsSuccessful)
                {
                    m_Logger.LogInformation($"No record found in  AssociatesExit Associates table for EmployeeId {EmployeeId}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetByEmployeeId() in AssociateExitController:" + stopwatch.Elapsed);
                    return NotFound(associateExit.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found in  AssociatesExit table for EmployeeId {EmployeeId}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetByEmployeeId() in AssociateExitController:" + stopwatch.Elapsed);
                    return Ok(associateExit.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while getting AssociateExits by Id in GetByEmployeeId method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetByEmployeeId() in AssociateExitController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while getting AssociateExit by EmployeeId in GetByEmployeeId method");
            }

        }
        #endregion       

        #region Create
        /// <summary>
        /// Create AssociateExit
        /// </summary>
        /// <param name="AssociateExitIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(AssociateExitRequest associateExitIn)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Inserting record in AssociateExit table.");
            try
            {
                var response = await m_AssociateExitService.Create(associateExitIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating AssociateExit: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in AssociateExitController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in AssociateExit's table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in AssociateExitController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while creating AssociateExit: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Create() in AssociateExitController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while creating AssociateExit in Create method.");
            }
        }
        #endregion

        #region ReviewByPM
        /// <summary>
        /// Review AssociateExit By PM
        /// </summary>
        /// <param name="associateExitPMIn">associateExitPMIn</param>
        /// <returns></returns>
        [HttpPost("ReviewByPM")]
        public async Task<IActionResult> ReviewByPM(AssociateExitPMRequest associateExitPMIn)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Updating record in AssociateExit table.");
            try
            {
                var response = await m_AssociateExitService.ReviewByPM(associateExitPMIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while approve AssociateExit: " + response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute ReviewByPM() in AssociateExitController:" + stopwatch.Elapsed);
                    return BadRequest(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in AssociateExit's table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute ReviewByPM() in AssociateExitController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while creating AssociateExit: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute ReviewByPM() in AssociateExitController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while creating AssociateExit in AccepteByPM method.");
            }
        }
        #endregion      

        #region Approve
        /// <summary>
        /// Approve AssociateExit
        /// </summary>
        /// <param name="AssociateExitIn"></param>
        /// <returns></returns>
        [HttpPost("Approve")]
        public async Task<IActionResult> Approve(AssociateExitRequest associateExitIn)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Updating record in AssociateExit table.");
            try
            {
                var response = await m_AssociateExitService.Approve(associateExitIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while approve AssociateExit: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Approve() in AssociateExitController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in AssociateExit's table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Approve() in AssociateExitController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while creating AssociateExit: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Create() in AssociateExitController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while creating AssociateExit in Create method.");
            }
        }
        #endregion      

        #region GetAssociatesForExitDashboard
        /// <summary>
        /// GetByEmployeeId
        /// </summary>
        /// <param name="userRole"></param>
        /// <param name="employeeId"></param>
        /// <param name="dashboard"></param>
        /// <returns></returns>
        [HttpGet("GetAssociatesForExitDashboard/{userRole}/{employeeId}/{dashboard}/{departmentId}")]
        public async Task<ActionResult<ExitDashboardResponse>> GetAssociatesForExitDashboard(string userRole, int employeeId, string dashboard, int departmentId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation($"Retrieving records from Associate Exit Table table for {employeeId}.");

            try
            {
                var response = await m_AssociateExitService.GetAssociatesForExitDashboard(userRole, employeeId, dashboard, departmentId);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogInformation($"No record found in  AssociatesExit  table for EmployeeId {employeeId}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAssociatesForExitDashboard() in AssociateExitController:" + stopwatch.Elapsed);
                    return NotFound(response.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found in  AssociatesExit table for EmployeeId {employeeId}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAssociatesForExitDashboard() in AssociateExitController:" + stopwatch.Elapsed);
                    return Ok(response.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while getting AssociateExits by Id in GetByEmployeeId method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetAssociatesForExitDashboard() in AssociateExitController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while getting AssociateExit in GetAssociatesForExitDashboard method");
            }

        }
        #endregion       

        #region RevokeExit
        /// <summary>
        /// Revoke Exit by Associate
        /// </summary>
        /// <param name="revokeRequest"></param>
        /// <returns>Integer value 0-represents UnSuccessful response and >0- represents Successful response</returns>
        [HttpPost("RevokeExit")]
        public async Task<IActionResult> RevokeExit(RevokeRequest revokeRequest)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Updating record in AssociateExit table.");
            try
            {
                var response = await m_AssociateExitService.RevokeExit(revokeRequest);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while Revoking AssociateExit: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute RevokeExit() in AssociateExitController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in AssociateExit's table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute RevokeExit() in AssociateExitController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while revoking AssociateExit: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute RevokeExit() in AssociateExitController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while revoking AssociateExit in RevokeExit method.");
            }
        }
        #endregion

        #region ExitClearance
        /// <summary>
        /// Provide Exit Clearance By HRA
        /// </summary>
        /// <param name="clearanceRequest"></param>
        /// <returns></returns>
        [HttpPost("ExitClearance")]
        public async Task<IActionResult> ExitClearance(ClearanceRequest clearanceRequest)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Updating record in AssociateExit table.");
            try
            {
                var response = await m_AssociateExitService.ExitClearance(clearanceRequest);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while giving clearance for AssociateExit: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute ExitClearance() in AssociateExitController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in AssociateExit's table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute ExitClearance() in AssociateExitController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while giving clearance for AssociateExit: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute ExitClearance() in AssociateExitController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while giving clearance for AssociateExit in ExitClearance method.");
            }
        }
        #endregion

        #region ApproveOrRejectRevoke
        /// <summary>
        /// Approve or Reject ExitRevoke by ProgramManager/DepartmentHead
        /// </summary>
        /// <param name="revokeRequest"></param>
        /// <returns>Integer value 0-represents UnSuccessful response and >0- represents Successful response</returns>
        [HttpPost("ApproveOrRejectRevoke")]
        public async Task<IActionResult> ApproveOrRejectRevoke(RevokeRequest revokeRequest)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Updating record in AssociateExit table.");
            try
            {
                var response = await m_AssociateExitService.ApproveOrRejectRevoke(revokeRequest);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while Revoking AssociateExit: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute ApproveOrRejectRevoke() in AssociateExitController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in AssociateExit's table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute ApproveOrRejectRevoke() in AssociateExitController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while Approving or Rejecting Revoke of Associate: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute ApproveOrRejectRevoke() in AssociateExitController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while revoking AssociateExit in ApproveOrRejectRevoke method.");
            }
        }
        #endregion

        #region RequestForWithdrawResignation
        /// <summary>
        /// Program Manager Request Associate to Withdraw Resignation
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="resignationRecommendation"></param>
        /// <returns></returns>
        [HttpGet("RequestForWithdrawResignation")]
        public async Task<IActionResult> RequestForWithdrawResignation(int employeeId, string resignationRecommendation)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Sending Notification and Updating in AssociateExit table.");
            try
            {
                var response = await m_AssociateExitService.RequestForWithdrawResignation(employeeId, resignationRecommendation);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while Request for AssociateExit Withdrawl: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute RequestForWithdrawResignation() in AssociateExitController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in AssociateExit's table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute RequestForWithdrawResignation() in AssociateExitController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while Request for AssociateExit Withdrawl: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute RequestForWithdrawResignation() in AssociateExitController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while Request for AssociateExit Withdrawl in RequestForWithdrawResignation method");
            }
        }
        #endregion

        #region GetClearanceRemarks
        /// <summary>
        ///Get Clearance Remarks
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>Clearance Remarks of Associate</returns>
        [HttpGet("GetClearanceRemarks/{employeeId}")]
        public async Task<IActionResult> GetClearanceRemarks(int employeeId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Fetching record from Remarks table.");
            try
            {
                var response = await m_AssociateExitService.GetClearanceRemarks(employeeId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while fetching Remarks: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetClearanceRemarks() in AssociateExitController:" + stopwatch.Elapsed);
                    return NotFound(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully fetched record from Remarks table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetClearanceRemarks() in AssociateExitController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while fetching Remarks: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetClearanceRemarks() in AssociateExitController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while fetching Remarks in GetClearanceRemarks method.");
            }
        }
        #endregion

        #region GetAssociateExitDailyNotification
        /// <summary>
        ///GetAssociateExitDailyNotification
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns>GetAssociateExitDailyNotification</returns>
        [HttpGet("GetAssociateExitDailyNotification/{departmentId}")]
        public async Task<IActionResult> GetAssociateExitDailyNotification(int departmentId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Fetching record from GetAssociateExitDailyNotification.");
            try
            {
                var response = await m_AssociateExitService.AssociateExitDailyNotification(departmentId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while fetching : " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAssociateExitDailyNotification in AssociateExitController:" + stopwatch.Elapsed);
                    return NotFound(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully fetched record .");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAssociateExitDailyNotification in AssociateExitController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while fetching : " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetAssociateExitDailyNotification in AssociateExitController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while fetching  in GetAssociateExitDailyNotification method.");
            }
        }
        #endregion

        #region GetResignationSubStatus
        /// <summary>
        ///Get Clearance Remarks
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetResignationSubStatus/{employeeId}")]
        public async Task<IActionResult> GetResignationSubStatus(int employeeId)
        {
            m_Logger.LogInformation("Executing GetResignationSubStatus Action method.");
            try
            {
                var response = await m_AssociateExitService.AssociateExitWFStatus(employeeId);
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
                return BadRequest("Error occurred while fetching status and substatus in GetResignationSubStatus method.");
            }
        }
        #endregion

        #region GetResignedAssociateByID
        /// <summary>
        /// GetResignedAssociateByID
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetResignedAssociateByID/{employeeId}")]
        public async Task<IActionResult> GetResignedAssociateByID(int employeeId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("fetching the record from AssociateExit table.");
            try
            {
                var response = await m_AssociateExitService.GetResignedAssociateByID(employeeId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while fetching AssociateExit: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute AssociateExitCheck() in AssociateExitController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully fetched the record from AssociateExit's table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute AssociateExitCheck() in AssociateExitController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while fetching AssociateExit: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute AssociateExitCheck() in AssociateExitController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while fetching AssociateExit in AssociateExitCheck method.");
            }
        }
        #endregion

        #region ReviewReminderNotification
        /// <summary>
        /// ReviewReminderNotification
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("ReviewReminderNotification/{employeeId}")]
        public async Task<IActionResult> ReviewReminderNotification(int employeeId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            try
            {
                var response = await m_AssociateExitService.ReviewReminderNotification(employeeId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while sending Review Reminder: " + response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute ReviewReminderNotification() in AssociateExitController:" + stopwatch.Elapsed);
                    return BadRequest(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully sent notification for Review Reminder.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute ReviewReminderNotification() in AssociateExitController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while sending Review Reminder: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute ReviewReminderNotification() in AssociateExitController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while sending ReviewReminderNotification() method.");
            }
        }
        #endregion
    }
}