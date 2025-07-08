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
    public class SkillGroupController : Controller
    {
        #region Global Variables

        private readonly ISkillGroupService m_SkillGroupService;
        private readonly ILogger<SkillGroupController> m_Logger;

        #endregion

        #region Constructor
        public SkillGroupController(ISkillGroupService skillGroupService, ILogger<SkillGroupController> logger)
        {
            m_SkillGroupService = skillGroupService;
            m_Logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// Create SkillGroup
        /// </summary>
        /// <param name="skillGroupIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(SkillGroup skillGroupIn)
        {

            m_Logger.LogInformation("Inserting record in skillGroup table.");
            try
            {
                dynamic response = await m_SkillGroupService.Create(skillGroupIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating skillGroup: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in skillGroup's table.");
                    return Ok(response.SkillGroup);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating skillGroup: " + ex);
                return BadRequest("Error occurred while creating skillGroup.");
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete skillGroup.
        /// </summary>
        /// <param name="skillGroupId"></param>
        /// <returns>bool</returns>
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int skillGroupId)
        {
            m_Logger.LogInformation("Deleting record in SkillGroup table.");

            try
            {
                dynamic response = await m_SkillGroupService.Delete(skillGroupId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while deleting SkillGroup: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted record in SkillGroup's table.");
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while deleting SkillGroup: " + ex);
                return BadRequest("Error occurred while deleting SkillGroup.");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetSkillGroups
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool? isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from SkillGroup table.");

            try
            {
                var skillGroups = await m_SkillGroupService.GetAll(isActive);
                if (skillGroups == null)
                {
                    m_Logger.LogInformation("No records found in SkillGroup table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { skillGroups.Count} SkillGroups.");
                    return Ok(skillGroups);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region Update
        /// <summary>
        /// Update skillGroup
        /// </summary>
        /// <param name="skillGroupIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(SkillGroup skillGroupIn)
        {

            m_Logger.LogInformation("Updating record in skillGroup table.");

            try
            {
                dynamic response = await m_SkillGroupService.Update(skillGroupIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating skillGroup: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in skillGroup's table.");
                    return Ok(response.SkillGroup);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating skillGroup: " + ex);
                return BadRequest("Error occurred while updating skillGroup.");
            }
        }
        #endregion

        #region GetByCompetencyAreaId
        /// <summary>
        /// GetByCompetencyAreaId
        /// </summary>
        /// <param name="competencyAreaID"></param>
        /// <returns></returns>
        [HttpGet("GetByCompetencyAreaId/{competencyAreaID}")]
        public async Task<ActionResult<SkillGroup>> GetByCompetencyAreaId(int competencyAreaID)
        {
            m_Logger.LogInformation($"Retrieving records from skillGroup table by {competencyAreaID}.");

            try
            {
                var skillGroup = await m_SkillGroupService.GetByCompetencyAreaId(competencyAreaID);
                if (skillGroup == null)
                {
                    m_Logger.LogInformation($"No records found for competencyAreaID {competencyAreaID}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for competencyAreaID {competencyAreaID}.");
                    return Ok(skillGroup);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetByCompetencyAreaCode
        /// <summary>
        /// GetByCompetencyAreaCode
        /// </summary>
        /// <param name="competencyAreaCode"></param>
        /// <returns></returns>
        [HttpGet("GetByCompetencyAreaCode/{competencyAreaCode}")]
        public async Task<ActionResult<List<SkillGroup>>> GetByCompetencyAreaCode(string competencyAreaCode)
        {
            m_Logger.LogInformation($"Retrieving records from skillGroup table by {competencyAreaCode}.");

            try
            {
                var skillGroup = await m_SkillGroupService.GetByCompetencyAreaCode(competencyAreaCode);
                if (skillGroup == null)
                {
                    m_Logger.LogInformation($"No records found for competencyAreaCode {competencyAreaCode}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for competencyAreaCode {competencyAreaCode}.");
                    return Ok(skillGroup);
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