using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Types;
using HRMS.KRA.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMS.KRA.API.Controllers
{
    [Authorize]
    [Route("kra/api/v1/[controller]")]
    [ApiController]
    public class ScaleController : ControllerBase
    {
        #region Global Variables
        private readonly IScaleService m_ScaleService;
        private readonly ILogger<ScaleController> m_Logger;
        #endregion

        #region Constructor
        public ScaleController(IScaleService ScaleService, ILogger<ScaleController> logger)
        {
            m_ScaleService = ScaleService;
            m_Logger = logger;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets list of KRA scale.
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            m_Logger.LogInformation("Retrieving records from scale table.");

            try
            {
                var lstScales = await m_ScaleService.GetAllAsync();
                if (lstScales == null)
                {
                    m_Logger.LogInformation("No records found in scale table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { lstScales.Count} scale.");
                    return Ok(lstScales);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetById
        /// <summary>
        /// Get Scale Details By ScaleID
        /// </summary>
        /// <param name="ScaleID"></param>
        /// <returns></returns>
        [HttpGet("GetScaleDetailsById/{ScaleID}")]
        public async Task<IActionResult> GetScaleDetailsById(int ScaleID)
        {
            m_Logger.LogInformation($"Retrieving records from scale details table by {ScaleID}.");

            try
            {
                var scale = await m_ScaleService.GetScaleDetailsByIdAsync(ScaleID);
                if (scale == null)
                {
                    m_Logger.LogInformation($"No records found for ScaleID {ScaleID}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for ScaleID {ScaleID}.");
                    return Ok(scale.OrderBy(x => x.ScaleValue).ToList());
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
        /// Create scale and details
        /// </summary>
        /// <param name="ScaleModel"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ScaleModel model)
        {
            m_Logger.LogInformation("Inserting record in scale and scaledetails table.");
            try
            {
                var response = await m_ScaleService.CreateAsync(model);

                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating scale and scaledetails: " + response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in scale table and scaledetails table.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating scale and scaledetails: " + ex);

                return BadRequest("Error occurred while creating scale and scaledetails.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Client
        /// </summary>
        /// <param name="scaleIn"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> Update(ScaleModel model)
        {
            m_Logger.LogInformation("Updating record in Scale and ScaleDetails table.");
            try
            {
                var response = await m_ScaleService.UpdateAsync(model);

                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating Scale and ScaleDetails: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in Scale and ScaleDetails's table.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating Scale and ScaleDetails: " + ex);

                return BadRequest("Error occurred while updating Scale and ScaleDetails.");
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete a Scale
        /// </summary>
        /// <param name="ScaleData"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int ScaleID)
        {
            m_Logger.LogInformation("Delete record in scale's table.");
            try
            {
                var response = await m_ScaleService.DeleteAsync(ScaleID);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while deleting scale: " + (string)response.Message);

                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted record in scale table.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while scale: " + ex);

                return BadRequest("Error occurred while delete a scale.");
            }
        }

        #endregion
    }
}
