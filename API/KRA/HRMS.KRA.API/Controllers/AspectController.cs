using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Types;
using HRMS.KRA.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;


namespace HRMS.KRA.API.Controllers
{
    [Authorize]
    [Route("kra/api/v1/[controller]")]
    [ApiController]
    public class AspectController : ControllerBase
    {
        #region Global Variables

        private readonly IAspectService m_AspectService;
        private readonly ILogger<AspectController> m_Logger;

        #endregion

        #region Constructor
        public AspectController(IAspectService aspectService,
            ILogger<AspectController> logger)
        {
            m_AspectService = aspectService;
            m_Logger = logger;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets aspect records
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns>List<Aspect></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            m_Logger.LogInformation("Retrieving records from aspect table.");

            try
            {
                var aspects = await m_AspectService.GetAllAsync();
                if (aspects == null)
                {
                    m_Logger.LogInformation("No records found in aspect table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { aspects.Count} aspect records.");
                    return Ok(aspects);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching aspect records.");
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// This method creates new aspect record.
        /// </summary>
        /// <param name="aspectIn"></param>
        /// <returns>Aspect</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(AspectModel model)
        {
            m_Logger.LogInformation("Inserting record in aspect table.");
            try
            {
                var response = await m_AspectService.CreateAsync(model.AspectName);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating aspect record: " + response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in aspect table.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Aspect name: " + model.AspectName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating a new aspect record: " + ex);

                return BadRequest("Error occurred while creating a new aspect record.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Aspect
        /// </summary>
        /// <param name="clientIn"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(AspectModel model)
        {
            m_Logger.LogInformation("Updating record in aspect table.");

            try
            {
                var response = await m_AspectService.UpdateAsync(model);

                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating aspect : " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in aspect table.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Client Name: " + model.AspectName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating aspect: " + ex);

                return BadRequest("Error occurred while updating aspect.");
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// This methode deletes aspect record.
        /// </summary>
        /// <param name="aspectId"></param>
        /// <returns>bool</returns>
        [HttpDelete("Delete/{aspectId}")]
        public async Task<IActionResult> DeleteAsync(int aspectId)
        {
            m_Logger.LogInformation("Deleting record in aspect table.");

            try
            {
                var response = await m_AspectService.DeleteAsync(aspectId);
                if (!response.IsSuccessful)
                {
                    //Extra information
                    m_Logger.LogError("aspect ID: " + aspectId);

                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while deleting aspect: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted record in aspect table.");
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("aspect ID: " + aspectId);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while deleting aspect: " + ex);

                return BadRequest("Error occurred while deleting aspect.");
            }
        }
        #endregion
    }
}
