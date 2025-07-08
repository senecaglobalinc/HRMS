using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Request;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AssociateExitActivityController : ControllerBase
    {
        #region Global Variables

        private readonly IAssociateExitActivityService m_AssociateExitActivityService;
        private readonly ILogger<AssociateExitActivityController> m_Logger;

        #endregion

        #region Constructor
        public AssociateExitActivityController(IAssociateExitActivityService associateExitActivityService,
            ILogger<AssociateExitActivityController> logger)
        {
            m_AssociateExitActivityService = associateExitActivityService;
            m_Logger = logger;
        }
        #endregion

        #region CreateActivityChecklist
        /// <summary>
        /// This method creates list of activities checklist
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>AssociateExitActivityId of Created checklist</returns>
        [HttpGet("CreateActivityChecklist/{employeeId}/{hraId}")]
        public async Task<IActionResult> CreateActivityChecklist(int employeeId, int hraId)
        {
            m_Logger.LogInformation("Inserting record in associateExitActivitytable.");
            try
            {
                var response = await m_AssociateExitActivityService.CreateActivityChecklist(employeeId, hraId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError($"Error occurred while creating associate exit Activity: {(string)response.Message}");


                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in associate exit Activity table.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {

                //Add exeption to logger
                m_Logger.LogError($"Error occurred while creating associate exit activities: {ex}");

                return BadRequest("Error occurred while creating associate exit Activity.");
            }
        }
        #endregion

        #region UpdateActivityChecklist
        /// <summary>
        /// This method updates and  submit checklist activities.
        /// </summary>
        /// <param name="employeetIn"></param>
        /// <returns>Updated associateExitActivityId</returns>
        [HttpPost("UpdateActivityChecklist")]
        public async Task<IActionResult> UpdateActivityChecklist(ActivityChecklist employeeIn)
        {
            m_Logger.LogInformation("Updating record in assoctaEcitActivity and associateExitActivityDetail table.");
            try
            {
                var response = await m_AssociateExitActivityService.UpdateActivityChecklist(employeeIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError($"Error occurred while Updating associateExit Activity: {(string)response.Message}");

                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in associateExit Activity table.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {

                //Add exeption to logger
                m_Logger.LogError($"Error occurred while updating associateExit activities: {ex}");

                return BadRequest("Error occurred while Updating associateExit Activity.");
            }
        }
        #endregion

        #region GetActivitiesByemployeeIdAndDepartmentId
        /// <summary>
        /// This method fetches the associateExit Department activities by employeetId and departmentId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>associateExit Department Activities</returns>
        [HttpGet("GetActivitiesByEmployeeIdAndDepartmentId")]
        public async Task<ActionResult<Activities>> GetActivitiesByEmployeeIdAndDepartmentId(int employeeId, int? departmentId = null)
        {
            m_Logger.LogInformation("Retrieving records from associateExit Activity table.");

            try
            {
                var deptact = await m_AssociateExitActivityService.GetDepartmentActivitiesByProjectId(employeeId, departmentId);
                if (deptact.Item == null)
                {
                    m_Logger.LogInformation("No records found in associateExit Activity table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning associateExit Activities.");
                    return Ok(deptact.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation($"employeeId= {employeeId}");
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetActivitiesByemployeeIdForHRA
        /// <summary>
        /// This method gets the submitted department activities in HRA review tab  from associateExitActivity table
        /// </summary>
        /// <param name="employeetId"></param>
        /// <returns> Submitted Department Activities</returns>
        [HttpGet("GetActivitiesByemployeeIdForHRA")]
        public async Task<ActionResult<List<Activities>>> GetActivitiesByemployeeIdForHRA(int employeeId)
        {
            m_Logger.LogInformation("Retrieving records from  Associate Exit Activity table.");

            try
            {
                var deptact = await m_AssociateExitActivityService.GetDepartmentActivitiesForHRA(employeeId);
                if (deptact.Items == null)
                {
                    m_Logger.LogInformation("No records found in Associate Exit Activity table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning  Associate Exit Activities.");
                    return Ok(deptact.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation($"EmployeeID= {employeeId}");
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion
    }
}