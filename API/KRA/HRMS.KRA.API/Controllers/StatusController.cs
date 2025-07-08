using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Types;
using HRMS.KRA.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.KRA.API.Controllers
{
    [Authorize]
    [Route("kra/api/v1/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        #region Global Variables
        private readonly IStatusService m_StatusService;
        private readonly ILogger<StatusController> m_Logger;
        #endregion

        #region Constructor
        public StatusController(IStatusService StatusService, ILogger<StatusController> logger)
        {
            m_StatusService = StatusService;
            m_Logger = logger;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets list of Status.
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            m_Logger.LogInformation("Retrieving records from status table.");

            try
            {
                var lstStatus = await m_StatusService.GetAllAsync();
                if (lstStatus == null)
                {
                    m_Logger.LogInformation("No records found in status table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { lstStatus.Count} status.");
                    return Ok(lstStatus);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// Create status
        /// </summary>
        /// <param name="StatusModel"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(StatusModel model)
        {
            m_Logger.LogInformation("Inserting record in status table.");
            try
            {
                var response = await m_StatusService.CreateAsync(model);

                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating status: " + response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in status table.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating status: " + ex);

                return BadRequest("Error occurred while creating status.");
            }
        }
        #endregion

    }
}
