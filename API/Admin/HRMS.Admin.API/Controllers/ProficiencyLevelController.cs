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
    public class ProficiencyLevelController : ControllerBase
    {
        #region Global Variables
        
        private readonly IProficiencyLevelService m_ProficiencyLevelService;
        private readonly ILogger<ProficiencyLevelController> m_Logger;

        #endregion

        #region Constructor
        public ProficiencyLevelController(IProficiencyLevelService ProficiencyLevelService,
                                            ILogger<ProficiencyLevelController> logger)
        {
            m_ProficiencyLevelService = ProficiencyLevelService;
            m_Logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// CreateProficiencyLevel
        /// </summary>
        /// <param name="proficiencyLevelIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProficiencyLevel proficiencyLevelIn)
        {
            m_Logger.LogInformation("Inserting record in ProficiencyLevel table.");
            try
            {
                dynamic response = await m_ProficiencyLevelService.Create(proficiencyLevelIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully created record in ProficiencyLevel table.");
                    return Ok(response.ProficiencyLevel);
                    
                }
                else
                {
                    m_Logger.LogError("Error occurred while creating ProficiencyLevel: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("ProficiencyLevelCode: " + proficiencyLevelIn.ProficiencyLevelCode);
                m_Logger.LogError("ProficiencyLevelDescription: " + proficiencyLevelIn.ProficiencyLevelDescription);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating ProficiencyLevel: " + ex);

                return BadRequest("Error occurred while creating ProficiencyLevel");
            }

        }
        #endregion

        #region GetAll

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<ProficiencyLevel>>> GetAll(bool isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from ProficiencyLevel table.");

            try
            {
                var proficiencyLevels = await m_ProficiencyLevelService.GetAll(isActive);
                if (proficiencyLevels == null)
                {
                    m_Logger.LogInformation("No records found in ProficiencyLevel table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { proficiencyLevels.Count} ProficiencyLevel.");
                    return Ok(proficiencyLevels);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching ProficiencyLevel.");
            }

        }
        #endregion

        #region GetById
        
        [HttpGet("GetById")]
        public async Task<ActionResult<List<ProficiencyLevel>>> GetById([FromQuery(Name = "proficiencyLevelIds")] string proficiencyLevelIds)
        {
            m_Logger.LogInformation("Retrieving records from ProficiencyLevel table.");

            try
            {
                var proficiencyLevels = await m_ProficiencyLevelService.GetByIds(proficiencyLevelIds);
                if (proficiencyLevels == null)
                {
                    m_Logger.LogInformation("No records found in ProficiencyLevel table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { proficiencyLevels.Count} ProficiencyLevel.");
                    return Ok(proficiencyLevels);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching ProficiencyLevel.");
            }

        }
        #endregion

        #region Update
        /// <summary>
        /// UpdateProficiencyLevel
        /// </summary>
        /// <param name="proficiencyLevelIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(ProficiencyLevel proficiencyLevelIn)
        {
            m_Logger.LogInformation("Updating record in ProficiencyLevel table.");

            try
            {
                dynamic response = await m_ProficiencyLevelService.Update(proficiencyLevelIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully updated record in ProficiencyLevel table.");
                    return Ok(response.ProficiencyLevel);

                }
                else
                {
                    m_Logger.LogError("Error occurred while updating ProficiencyLevel: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("ProficiencyLevelCode: " + proficiencyLevelIn.ProficiencyLevelCode);
                m_Logger.LogError("ProficiencyLevelDescription: " + proficiencyLevelIn.ProficiencyLevelDescription);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating ProficiencyLevel: " + ex);

                return BadRequest("Error occurred while updating ProficiencyLevel");
            }
        }
        #endregion
    }
}