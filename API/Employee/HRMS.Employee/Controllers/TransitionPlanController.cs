using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Request;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class TransitionPlanController : ControllerBase
    {
        #region Global Variables

        private readonly ITransitionPlanService m_TransitionPlanService;
        private readonly ILogger<TransitionPlanController> m_Logger;

        #endregion

        #region Constructor
        public TransitionPlanController(ITransitionPlanService transitionPlanService,
            ILogger<TransitionPlanController> logger)
        {
            m_TransitionPlanService = transitionPlanService;
            m_Logger = logger;
        }
        #endregion

        #region UpdateTransitionPlan
        /// <summary>
        /// This method updates and  submit checklist activities.
        /// </summary>
        /// <param name="projectIn"></param>
        /// <returns>Updated ProjectClosureActivityId</returns>
        [HttpPost("UpdateTransitionPlan")]
        public async Task<IActionResult> UpdateTransitionPlan(TransitionDetail projectIn)
        {
            m_Logger.LogInformation("Updating record in UpdateTransitionPlan and TransitionPlan table.");
            try
            {
                var response = await m_TransitionPlanService.UpdateTransitionPlan(projectIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError($"Error occurred while Updating transition plan Activity: {(string)response.Message}");

                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in transition plan Activity table.");

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {

                //Add exeption to logger
                m_Logger.LogError($"Error occurred while updating transition plan activities: {ex}");

                return BadRequest("Error occurred while Updating transition plan Activity.");
            }
        }
        #endregion

        #region GetTransitionPlanByAssociateIdandProjectId
        /// <summary>
        /// This method fetches the closure Department activities by employeeId and projectId
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>Closure Department Activities</returns>
        [HttpGet("GetTransitionPlanByAssociateIdandProjectId")]
        public async Task<ActionResult<TransitionDetail>> GetTransitionPlanByAssociateIdandProjectId(int employeeId, int? projectId)
        {
            m_Logger.LogInformation("Retrieving records from transition plan table.");

            try
            {
                int proj = projectId ?? 0;
                var deptact = await m_TransitionPlanService.GetTransitionPlanByAssociateIdandProjectId(employeeId, proj);
                if (deptact.Item == null)
                {
                    m_Logger.LogInformation("No records found in transition plan table.");
                    return Ok(null);
                }
                else
                {
                    m_Logger.LogInformation($"Returning transition plan Activities.");
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

        #region GetTransitionPlanByAssociateId
        /// <summary>
        /// This method fetches the Transition Plan  activities by employeeId
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>Transition Activities</returns>
        [HttpGet("GetTransitionPlanByAssociateId")]
        public async Task<ActionResult<TransitionDetail>> GetTransitionPlanByAssociateId(int employeeId)
        {
            m_Logger.LogInformation("Retrieving records from Transition Plan table.");

            try
            {
                var deptact = await m_TransitionPlanService.GetTransitionPlanByAssociateId(employeeId);
                if (deptact.IsSuccessful == false)
                {
                    m_Logger.LogInformation("No records found in Transition Plan table.");
                    return NotFound(deptact.IsSuccessful);
                }
                else
                {
                    m_Logger.LogInformation($"Returning Transition Plan Activities.");
                    return Ok(deptact.Items);
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

        #region DeleteTransitionActivity
        /// <summary>
        /// This method Deletes the Transition Activities
        /// </summary> 
        /// <param name="employeeId">employeeId</param>
        /// <param name="projectId">projectId</param>
        /// <param name="activityId">activityId</param>
        /// <returns>Integer value</returns>
        [HttpDelete("DeleteTransitionActivity")]
        public async Task<ActionResult<int>> DeleteTransitionActivity(int employeeId, int? projectId, int activityId)
        {
            m_Logger.LogInformation("Retrieving records from transition plan table.");

            try
            {
                int proj = projectId ?? 0;
                var deptact = await m_TransitionPlanService.DeleteTransitionActivity(employeeId, proj, activityId);
                if (deptact.Item == 0)
                {
                    m_Logger.LogInformation("No records found in transition plan table.");
                    return Ok(deptact.Item);
                }
                else
                {
                    m_Logger.LogInformation($"Deleted Successfully");
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
    }
}