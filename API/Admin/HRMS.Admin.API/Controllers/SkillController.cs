using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
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
    [ApiController]
    [Route("admin/api/v1/[controller]")]
    [Authorize]
    public class SkillController : Controller
    {
        #region Global Variables

        private readonly ISkillService m_SkillService;
        private readonly ILogger<SkillController> m_Logger;

        #endregion

        #region Constructor
        public SkillController(ISkillService skillService, ILogger<SkillController> logger)
        {
            m_SkillService = skillService;
            m_Logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// Create Skill
        /// </summary>
        /// <param name="skillIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Skill skillIn)
        {

            m_Logger.LogInformation("Inserting record in skills table.");
            try
            {
                dynamic response = await m_SkillService.Create(skillIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating skill: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in skill's table.");
                    return Ok(response.Skill);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating skill: " + ex);
                return BadRequest("Error occurred while creating skill.");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get Skills
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool? isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from skills table.");

            try
            {
                var skills = await m_SkillService.GetAll(isActive);
                if (skills == null)
                {
                    m_Logger.LogInformation("No records found in skills table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Successfully returned Skills.");
                    return Ok(skills);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetActiveSkillsForDropdown
        /// <summary>
        /// Get Active Skills For Dropdown
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetActiveSkillsForDropdown")]
        public async Task<ActionResult<List<GenericType>>> GetActiveSkillsForDropdown()
        {
            m_Logger.LogInformation("Retrieving records from skills table.");

            try
            {
                var skills = await m_SkillService.GetActiveSkillsForDropdown();
                if (skills == null)
                {
                    m_Logger.LogInformation("No records found in skills table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Successfully returned Skills.");
                    return Ok(skills.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetSkillsForDropdown
        /// <summary>
        /// Get Skills For Dropdown
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetSkillsForDropdown")]
        public async Task<ActionResult<List<GenericType>>> GetSkillsForDropdown()
        {
            m_Logger.LogInformation("Retrieving records from skills table.");

            try
            {
                var skills = await m_SkillService.GetActiveSkillsForDropdown();
                if (skills == null)
                {
                    m_Logger.LogInformation("No records found in skills table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Successfully returned Skills.");
                    return Ok(skills.Items);
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
        /// Get skills by SkillId
        /// </summary>
        /// <param name="skillIds"></param>
        /// <returns></returns>
        [HttpGet("GetById")]
        public async Task<ActionResult<List<Skill>>> GetById([FromQuery(Name = "skillIds")] string skillIds)
        {
            m_Logger.LogInformation("Retrieving records from skills table.");

            try
            {
                var skills = await m_SkillService.GetById(skillIds);
                if (skills == null)
                {
                    m_Logger.LogInformation("No records found in skills table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Successfully returned Skills.");
                    return Ok(skills);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetBySkillGroupId
        /// <summary>
        /// Get skills by SkillGroupId
        /// </summary>
        /// <param name="skillGroupIds"></param>
        /// <returns></returns>
        [HttpGet("GetBySkillGroupId")]
        public async Task<ActionResult<List<Skill>>> GetBySkillGroupId([FromQuery(Name = "skillGroupIds")] string skillGroupIds)
        {
            m_Logger.LogInformation("Retrieving records from skills table.");

            try
            {
                var skills = await m_SkillService.GetBySkillGroupId(skillGroupIds);
                if (skills == null)
                {
                    m_Logger.LogInformation("No records found in skills table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Successfully returned Skills.");
                    return Ok(skills);
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
        /// Update Skill
        /// </summary>
        /// <param name="skillIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(Skill skillIn)
        {

            m_Logger.LogInformation("Updating record in skills table.");

            try
            {
                dynamic response = await m_SkillService.Update(skillIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating skill: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in skill's table.");
                    return Ok(response.Skill);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating skill: " + ex);
                return BadRequest("Error occurred while updating skill.");
            }
        }
        #endregion


        #region GetskillsBySkillGroupId
        /// <summary>
        /// Update Skill
        /// </summary>
        /// <param name="skillgroupid"></param>
        /// <returns></returns>
        [HttpGet("GetskillsBySkillGroupId")]
        public async Task<IActionResult> GetskillsBySkillGroupId(int SkillGroupId)
        {

         //   m_Logger.LogInformation("Updating record in skills table.");

            try
            {
                var response = await m_SkillService.GetskillsBySkillGroupId(SkillGroupId);
                if (response==null)
                {
                    //Add exeption to logger
                    m_Logger.LogInformation("no skills found with this skillgroupid: " );
                    return NotFound();
                }
                else
                {
                   m_Logger.LogInformation("Successfully returned skill's list based on skillgroupid.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while fetching skills based on skillgroupid: " + ex);
                return BadRequest("Error occurred while fetching skills based on skillgroupid.");
            }
        }
        #endregion


        #region GetskillsBySearchString
        /// <summary>
        /// GetskillsBySearchString
        /// </summary>
        /// <param name="skillsearchstring"></param>
        /// <returns></returns>
        [HttpGet("GetSkillsBySearchString")]
        public async Task<IActionResult> GetSkillsBySearchString(string skillsearchstring)
        {

            try
            {
                List<SkillSearchResponse> response = await m_SkillService.GetSkillsBySearchString(skillsearchstring);
                if (response==null)
                {                  
                    return NotFound();
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
              
                m_Logger.LogError("Error occurred while fetching skills based on skillsearchstring: " + ex);
                return BadRequest("Error occurred while fetching skills based on skillsearchstring.");
            }
        }
        #endregion

    }
}