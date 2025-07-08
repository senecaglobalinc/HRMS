using HRMS.Admin.Entities;
using HRMS.Admin.Types;
using HRMS.Admin.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace HRMS.Admin.API.Controllers
{
    [Route("admin/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ReasonController : Controller
    {
        #region Global Variables

        private readonly IReasonService m_ReasonService;
        private readonly ILogger<ReasonController> m_Logger;

        #endregion

        #region Constructor

        public ReasonController(IReasonService reasonService, ILogger<ReasonController> logger)
        {
            m_ReasonService = reasonService;
            m_Logger = logger;
        }

        #endregion

        #region  Create
        /// <summary>
        /// CreateReason
        /// </summary>
        /// <param name="reasonIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Reason reasonIn)
        {
            m_Logger.LogInformation("Inserting record in Reason table.");
            try
            {
                dynamic response = await m_ReasonService.Create(reasonIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully created record in Reason table.");
                    return Ok(response.Reason);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating Reason: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("ReasonName: " + reasonIn.Description);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating Reason: " + ex);
                return BadRequest("Error occurred while creating Reason");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetReasons
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from Reason table.");

            try
            {
                var Reasons = await m_ReasonService.GetAll(isActive);
                if (Reasons == null)
                {
                    m_Logger.LogInformation("No records found in Reason table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { Reasons.Count} Reason.");
                    return Ok(Reasons);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Reason.");
            }
        }
        #endregion

        #region GetByReasonId
        /// <summary>
        /// Gets the Reason by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<ActionResult<Reason>> GetByReasonId(int Id)
        {
            m_Logger.LogInformation($"Retrieving records from Reason table by {Id}.");

            try
            {
                var Reason = await m_ReasonService.GetByReasonId(Id);
                if (Reason == null)
                {
                    m_Logger.LogInformation($"No records found for Reason {Id}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for Reason {Id}.");
                    return Ok(Reason);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetReasonsForDropdown
        /// <summary>
        /// GetReasonsForDropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetReasonsForDropdown")]
        public async Task<ActionResult<IEnumerable>> GetReasonsForDropdown()
        {
            m_Logger.LogInformation("Retrieving records from Reason table.");

            try
            {
                var Reasons = await m_ReasonService.GetReasonsForDropdown();
                if (Reasons == null)
                {
                    m_Logger.LogInformation("No records found in Reason table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { Reasons.Count} Reason.");
                    return Ok(Reasons);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Reason.");
            }
        }
        #endregion

        #region GetVoluntaryExitReasons
        /// <summary>
        /// GetVoluntaryExitReasons
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetVoluntaryExitReasons")]
        public async Task<ActionResult<IEnumerable>> GetVoluntaryExitReasons()
        {
            m_Logger.LogInformation("Retrieving records from Reason table.");

            try
            {
                var Reasons = await m_ReasonService.GetVoluntaryExitReasons();
                if (Reasons == null)
                {
                    m_Logger.LogInformation("No records found in Reason table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { Reasons.Count} Reason.");
                    return Ok(Reasons);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Reason.");
            }
        }
        #endregion

        #region GetOtherExitReasons
        /// <summary>
        /// GetOtherExitReasons
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOtherExitReasons")]
        public async Task<ActionResult<IEnumerable>> GetOtherExitReasons()
        {
            m_Logger.LogInformation("Retrieving records from Reason table.");

            try
            {
                var Reasons = await m_ReasonService.GetOtherExitReasons();
                if (Reasons == null)
                {
                    m_Logger.LogInformation("No records found in Reason table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { Reasons.Count} Reason.");
                    return Ok(Reasons);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Reason.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// UpdateReason
        /// </summary>
        /// <param name="reasonIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(Reason reasonIn)
        {
            m_Logger.LogInformation("updating record in Reason table.");
            try
            {
                dynamic response = await m_ReasonService.Update(reasonIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully updated record in Reason table.");
                    return Ok(response.Reason);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating Reason: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Reason: " + reasonIn.Description);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating Reason: " + ex);
                return BadRequest("Error occurred while updating Reason");
            }
        }
        #endregion
    }
}