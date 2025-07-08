using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Types;
using HRMS.Project.API.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.Project.API.Controllers
{
    [Route("project/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectClosureActivityController : ControllerBase
    {
        #region Global Variables

        private readonly IProjectClosureActivityService m_ProjectClosureActivityService;
        private readonly ILogger<ProjectClosureActivityController> m_Logger;

        #endregion

        #region Constructor
        public ProjectClosureActivityController(IProjectClosureActivityService projectClosureActivityService,
            ILogger<ProjectClosureActivityController> logger)
        {
            m_ProjectClosureActivityService = projectClosureActivityService;
            m_Logger = logger;
        }
        #endregion

        #region CreateActivityChecklist
        /// <summary>
        /// This method creates list of activities checklist
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>ProjectClosureActivityId of Created checklist</returns>
        [HttpPost("CreateActivityChecklist")]
        public async Task<IActionResult> CreateActivityChecklist(int projectId)
        {
            m_Logger.LogInformation("Inserting record in projectClosureActivitytable.");
            try
            {
                var response = await m_ProjectClosureActivityService.CreateActivityChecklist(projectId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError($"Error occurred while creating project closure Activity: {(string)response.Message}");

                    
                    return NotFound(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in project closure Activity table.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                
                //Add exeption to logger
                m_Logger.LogError($"Error occurred while creating project closure activities: {ex}");

                return BadRequest("Error occurred while creating project closure Activity.");
            }
        }
        #endregion

        #region UpdateActivityChecklist
        /// <summary>
        /// This method updates and  submit checklist activities.
        /// </summary>
        /// <param name="projectIn"></param>
        /// <returns>Updated ProjectClosureActivityId</returns>
        [HttpPost("UpdateActivityChecklist")]
        public async Task<IActionResult> UpdateActivityChecklist(ActivityChecklist projectIn)
        {
            m_Logger.LogInformation("Updating record in projectClosureActivity and ProjectClosureActivityDetail table.");
            try
            {
                var response = await m_ProjectClosureActivityService.UpdateActivityChecklist(projectIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError($"Error occurred while Updating project closure Activity: {(string)response.Message}");

                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in project closure Activity table.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                
                //Add exeption to logger
                m_Logger.LogError($"Error occurred while updating project activities: {ex}");

                return BadRequest("Error occurred while Updating project closure Activity.");
            }
        }
        #endregion

        #region GetActivitiesByProjectIdAndDepartmentId
        /// <summary>
        /// This method fetches the closure Department activities by projectId and departmentId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Closure Department Activities</returns>
        [HttpGet("GetActivitiesByProjectIdAndDepartmentId")]
        public async Task<ActionResult<Activities>> GetActivitiesByProjectIdAndDepartmentId(int projectId, int? departmentId =null)
        {
            m_Logger.LogInformation("Retrieving records from Project Closure Activity table.");

            try
            {
                var deptact = await m_ProjectClosureActivityService.GetDepartmentActivitiesByProjectId(projectId, departmentId);
                if (deptact.Item == null)
                {
                    m_Logger.LogInformation("No records found in Project Closure Activity table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning Project Closure Activities.");
                    return Ok(deptact.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation($"ProjectId= {projectId}");
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetActivitiesByProjectIdForPM
        /// <summary>
        /// This method gets the submitted department activities in PM review tab  from projectClosureActivity table
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns> Submitted Department Activities</returns>
        [HttpGet("GetActivitiesByProjectIdForPM")]
        public async Task<ActionResult<List<Activities>>> GetActivitiesByProjectIdForPM(int projectId)
        {
            m_Logger.LogInformation("Retrieving records from Project Closure Activity table.");

            try
            {
                var deptact = await m_ProjectClosureActivityService.GetDepartmentActivitiesForPM(projectId);
                if (deptact.Items == null)
                {
                    m_Logger.LogInformation("No records found in Project Closure Activity table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning Project Closure Activities.");
                    return Ok(deptact.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation($"ProjectId= {projectId}");
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion
    }
}
