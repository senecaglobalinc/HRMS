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
    public class CompetencyAreaController : ControllerBase
    {
        #region Global Variables

        private readonly ICompetencyAreaService m_CompetencyAreaService;
        private readonly ILogger<CompetencyAreaController> m_Logger; 

        #endregion

        #region Constructor
        public CompetencyAreaController(ICompetencyAreaService competencyAreaService, ILogger<CompetencyAreaController> logger)
        {
            m_CompetencyAreaService = competencyAreaService;
            m_Logger = logger;
        } 
        #endregion

        #region Create
        /// <summary>
        /// This method creates new competency area.
        /// </summary>
        /// <param name="competencyAreaIn"></param>
        /// <returns>CompetencyArea</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CompetencyArea competencyAreaIn)
        {
            m_Logger.LogInformation("Inserting record in competency area's table.");
            try
            {
                dynamic response = await m_CompetencyAreaService.Create(competencyAreaIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating competency area: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in competency area's table.");
                    return Ok(response.CompetencyArea);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Competency area code: " + competencyAreaIn.CompetencyAreaCode);
                m_Logger.LogError("Competency area description: " + competencyAreaIn.CompetencyAreaDescription);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating competency area: " + ex);

                return BadRequest("Error occurred while creating competency area.");
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// This methode update competency area's IsActive flag to false.
        /// Updation of competency is allowed only there is no active record for skill and skill group table.
        /// </summary>
        /// <param name="competencyAreaID"></param>
        /// <returns>bool</returns>
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int competencyAreaID)
        {
            m_Logger.LogInformation("Deleting record in competency area's table.");

            try
            {
                dynamic response = await m_CompetencyAreaService.Delete(competencyAreaID);
                if (!response.IsSuccessful)
                {
                    //Extra information
                    m_Logger.LogError("Competency area ID: " + competencyAreaID);

                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while deleting competency area: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted record in competency area's table.");
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Competency area ID: " + competencyAreaID);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while deleting competency area: " + ex);

                return BadRequest("Error occurred while deleting competency area.");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets competency area's
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns>List<CompetencyArea></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool? isActive)
        {
            m_Logger.LogInformation("Retrieving records from competency area's table.");

            try
            {
                var practiceAreas = await m_CompetencyAreaService.GetAll(isActive);
                if (practiceAreas == null)
                {
                    m_Logger.LogInformation("No records found in competency area's table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { practiceAreas.Count} competency areas.");
                    return Ok(practiceAreas);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching competency area's.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// This method updates competency area details.
        /// Updation of competency is allowed only there is no active record for skill and skill group table.
        /// </summary>
        /// <param name="competencyArea"></param>
        /// <returns>CompetencyArea</returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(CompetencyArea competencyArea)
        {
            m_Logger.LogInformation("Updating record in competency area's table.");

            try
            {
                dynamic response = await m_CompetencyAreaService.Update(competencyArea);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating competency area: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in competency area's table.");
                    return Ok(response.CompetencyArea);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Competency area code: " + competencyArea.CompetencyAreaCode);
                m_Logger.LogError("Competency area description: " + competencyArea.CompetencyAreaDescription);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating competency area: " + ex);

                return BadRequest("Error occurred while updating competency area.");
            }
        }
        #endregion
    }
}