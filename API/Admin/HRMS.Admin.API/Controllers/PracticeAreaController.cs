using HRMS.Admin.Entities;
using HRMS.Admin.Types;
using HRMS.Admin.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.API.Controllers
{
    [Route("admin/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PracticeAreaController : ControllerBase
    {
        #region  Global Variables

        private readonly IPracticeAreaService m_PracticeAreaService;
        private readonly ILogger<PracticeAreaController> m_Logger; 

        #endregion

        #region Constructor
        public PracticeAreaController(IPracticeAreaService practiceAreaService, ILogger<PracticeAreaController> logger)
        {
            m_PracticeAreaService = practiceAreaService;
            m_Logger = logger;
        } 
        #endregion

        #region Create
        /// <summary>
        /// Create practice area
        /// </summary>
        /// <param name="practiceAreaIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(PracticeArea practiceAreaIn)
        {
            m_Logger.LogInformation("Inserting record in practice area's table.");
            try
            {
                dynamic response = await m_PracticeAreaService.Create(practiceAreaIn);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogInformation("Error occurred while creating practice area: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in practice area's table.");
                    return Ok(response.PracticeArea);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Practice area code: " + practiceAreaIn.PracticeAreaCode);
                m_Logger.LogError("Practice area description: " + practiceAreaIn.PracticeAreaDescription);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating practice area: " + ex);

                return BadRequest("Error occurred while creating practice area.");
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// This methode update competency area's IsActive flag to false.
        /// Updation of competency is allowed only there is no active record for skill and skill group table.
        /// </summary>
        /// <param name="practiceAreaID"></param>
        /// <returns>bool</returns>
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int practiceAreaID)
        {
            m_Logger.LogInformation("Deleting record in competency area's table.");

            try
            {
                dynamic response = await m_PracticeAreaService.Delete(practiceAreaID);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while deleting practice area: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted record in practice area's table.");
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Competency area ID: " + practiceAreaID);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while deleting practice area: " + ex);

                return BadRequest("Error occurred while deleting practice area.");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets practice area's
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool? isActive=true)
        {
            m_Logger.LogInformation("Retrieving records from practice area's table.");

            try
            {
                var practiceAreas = await m_PracticeAreaService.GetAll(isActive);
                if (practiceAreas == null)
                {
                    m_Logger.LogInformation("No records found in practice area's table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { practiceAreas.Count} practice areas.");
                    return Ok(practiceAreas);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching practice area's.");
            }
        }
        #endregion

        #region GetTechnologyForDropdown
        /// <summary>
        /// GetTechnologyForDropdown
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetTechnologyForDropdown")]
        public async Task<ActionResult<IEnumerable>> GetTechnologyForDropdown()
        {
            m_Logger.LogInformation("Retrieving records from PracticeAreas table.");

            try
            {
                var technologies = await m_PracticeAreaService.GetTechnologyForDropdown();
                if (technologies == null)
                {
                    m_Logger.LogInformation("No records found in PracticeAreas table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { technologies.Count} PracticeAreas.");
                    return Ok(technologies);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetByIds
        /// <summary>
        /// Gets practice area's by ids
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetByIds")]
        public async Task<ActionResult<IEnumerable>> GetByIds([FromQuery] int[] practiceAreaIds)
        {
            m_Logger.LogInformation("Retrieving records from practice area's table.");

            try
            {
                var practiceAreas = await m_PracticeAreaService.GetPracticeAreaByIds(practiceAreaIds);
                if (practiceAreas == null)
                {
                    m_Logger.LogInformation("No records found in practice area's table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { practiceAreas.Count} practice areas.");
                    return Ok(practiceAreas);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching practice area's.");
            }
        }
        #endregion

        #region GetById
        /// <summary>
        ///  Gets practice area's by id
        /// </summary>
        /// <param name="practiceAreaId"></param>
        /// <returns></returns>
        [HttpGet("GetById")]
        public async Task<ActionResult<PracticeArea>> GetById(int practiceAreaId)
        {
            m_Logger.LogInformation("Retrieving records from practice area's table.");

            try
            {
                var practiceArea = await m_PracticeAreaService.GetPracticeAreaById(practiceAreaId);
                if (practiceArea == null)
                {
                    m_Logger.LogInformation("No records found in practice area's table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning practice area for id {practiceAreaId}.");
                    return Ok(practiceArea);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching practice area.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update practice area
        /// </summary>
        /// <param name="practiceAreaIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(PracticeArea practiceAreaIn)
        {
            m_Logger.LogInformation("Updating record in practice area's table.");

            try
            {
                var response = await m_PracticeAreaService.Update(practiceAreaIn);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogInformation("Error occurred while updating practice area: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in practice area's table.");
                    return Ok(response.PracticeArea);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Practice area code: " + practiceAreaIn.PracticeAreaCode);
                m_Logger.LogError("Practice area description: " + practiceAreaIn.PracticeAreaDescription);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating practice area: " + ex);

                return BadRequest("Error occurred while updating practice area.");
            }
        }
        #endregion

        #region GetByPracticeAreaCode
        /// <summary>
        /// GetByPracticeAreaCode
        /// </summary>
        /// <param name="practiceAreaCode"></param>
        /// <returns></returns>
        [HttpGet("GetByPracticeAreaCode/{practiceAreaCode}")]
        public async Task<ActionResult<PracticeArea>> GetByPracticeAreaCode(string practiceAreaCode)
        {
            m_Logger.LogInformation($"Retrieving records from PracticeArea table by {practiceAreaCode}.");

            try
            {
                var practiceArea = await m_PracticeAreaService.GetByPracticeAreaCode(practiceAreaCode);
                if (practiceArea == null)
                {
                    m_Logger.LogInformation($"No records found for practiceAreaCode {practiceAreaCode}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for practiceAreaCode {practiceAreaCode}.");
                    return Ok(practiceArea);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion
    }
}