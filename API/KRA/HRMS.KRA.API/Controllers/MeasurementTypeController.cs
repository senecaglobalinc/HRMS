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
    public class MeasurementTypeController : ControllerBase
    {
        #region Global Variables
        private readonly IMeasurementTypeService m_Service;
        private readonly ILogger<MeasurementTypeController> m_Logger;
        #endregion

        #region Constructor
        public MeasurementTypeController(IMeasurementTypeService service, ILogger<MeasurementTypeController> logger)
        {
            m_Service = service;
            m_Logger = logger;
        }
        #endregion

        #region Get
        /// <summary>
        /// Gets KRA Measurement Type
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            m_Logger.LogInformation($"Retrieving records.");

            try
            {
                var response = await m_Service.GetAllAsync();
                if (response == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(response);
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
        /// Create a new Measurement Type
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(MeasurementTypeModel model)
        {
            m_Logger.LogInformation("Inserting new record.");
            try
            {
                var response = await m_Service.CreateAsync(model.MeasurementType);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating record : " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating new record : " + ex);
                return BadRequest("Error occurred while creating new record.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update a Measurement Type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> Update(MeasurementTypeModel model)
        {
            m_Logger.LogInformation("Updating record.");
            try
            {
                var response = await m_Service.UpdateAsync(model);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating record : " + response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully update record.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating record : " + ex.Message);

                return BadRequest("Error occurred while updating record.");
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Measurement Type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            m_Logger.LogInformation("Deleting record.");
            try
            {
                var response = await m_Service.DeleteAsync(id);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while deleting record : " + response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted record.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while deleting record: " + ex);

                return BadRequest("Error occurred while deleting record.");
            }
        }

        #endregion
    }
}
