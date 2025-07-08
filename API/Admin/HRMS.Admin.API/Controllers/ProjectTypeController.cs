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
    public class ProjectTypeController : ControllerBase
    {
        #region Global Variable

        private readonly IProjectTypeService m_projectTypeService;
        private readonly ILogger<ProjectTypeController> m_Logger;

        #endregion

        #region Constructor
        public ProjectTypeController(IProjectTypeService projectTypeService, ILogger<ProjectTypeController> logger)
        {
            m_projectTypeService = projectTypeService;
            m_Logger = logger;
        }
        #endregion

        #region  Create
        /// <summary>
        /// CreateProjectType
        /// </summary>
        /// <param name="projectTypeIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProjectType projectTypeIn)
        {
            m_Logger.LogInformation("Inserting record in ProjectType table.");
            try
            {
                dynamic response = await m_projectTypeService.Create(projectTypeIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully created record in ProjectType table.");
                    return Ok(response.ProjectType);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating ProjectType: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("ProjectTypeCode: " + projectTypeIn.ProjectTypeCode);
                m_Logger.LogError("ProjectTypeDescription: " + projectTypeIn.Description);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating ProjectType: " + ex);
                return BadRequest("Error occurred while creating ProjectType");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetProjectType
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from ProjectType table.");

            try
            {
                var projectTypes = await m_projectTypeService.GetAll(isActive);
                if (projectTypes == null)
                {
                    m_Logger.LogInformation("No records found in ProjectType table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning {projectTypes.Count} ProjectType.");
                    return Ok(projectTypes);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching ProjectType.");
            }
        }
        #endregion

        #region GetByIds
        /// <summary>
        /// GetProjectType
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetByIds")]
        public async Task<ActionResult<IEnumerable>> GetByIds([FromQuery] int[] projectTypeIds)
        {
            m_Logger.LogInformation("Retrieving records from ProjectType table.");

            try
            {
                var projectTypes = await m_projectTypeService.GetByProjectTypeIds(projectTypeIds);
                if (projectTypes == null)
                {
                    m_Logger.LogInformation("No records found in ProjectType table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning {projectTypes.Count} ProjectTypes.");
                    return Ok(projectTypes);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching ProjectType.");
            }
        }
        #endregion

        #region GetById
        /// <summary>
        /// GetProjectType
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById")]
        public async Task<ActionResult<ProjectType>> GetById(int projectTypeId)
        {
            m_Logger.LogInformation("Retrieving records from ProjectType table.");

            try
            {
                var projectType = await m_projectTypeService.GetProjectTypeById(projectTypeId);
                if (projectType == null)
                {
                    m_Logger.LogInformation("No records found in ProjectType table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning ProjectType for id {projectTypeId}.");
                    return Ok(projectType);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching ProjectType.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// UpdateProjectType
        /// </summary>
        /// <param name="projectTypeIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(ProjectType projectTypeIn)
        {
            m_Logger.LogInformation("updating record in ProjectType table.");
            try
            {
                dynamic response = await m_projectTypeService.Update(projectTypeIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully updated record in ProjectType table.");
                    return Ok(response.ProjectType);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating ProjectType: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("ProjectTypeCode: " + projectTypeIn.ProjectTypeCode);
                m_Logger.LogError("ProjectTypeDescription: " + projectTypeIn.Description);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating ProjectType: " + ex);

                return BadRequest("Error occurred while updating ProjectType");
            }
        }
        #endregion

        #region GetProjectTypeByCode
        /// <summary>
        /// GetProjectTypeByCode
        /// </summary>
        /// <param name="projectTypeCode"></param>
        /// <returns></returns>
        [HttpGet("GetProjectTypeByCode/{projectTypeCode}")]
        public async Task<ActionResult<IEnumerable>> GetProjectTypeByCode(string projectTypeCode)
        {
            m_Logger.LogInformation("Retrieving records from ProjectType table.");

            try
            {
                var projectType = await m_projectTypeService.GetByProjectTypeCode(projectTypeCode);
                if (projectType == null)
                {
                    m_Logger.LogInformation("No records found in ProjectType table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning {projectType} ProjectType.");
                    return Ok(projectType);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching ProjectType.");
            }
        }
        #endregion
    }
}