using HRMS.Admin.Entities;
using HRMS.Admin.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using HRMS.Admin.API.Auth;

namespace HRMS.Admin.API.Controllers
{
    [Authorize]
    [Route("admin/api/v1/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        #region Global Variables

        private readonly IActivityService m_ActivityService;
        private readonly ILogger<ActivityController> m_Logger;

        #endregion

        #region Constructor

        public ActivityController(IActivityService activityService, ILogger<ActivityController> logger)
        {
            m_ActivityService = activityService;
            m_Logger = logger;
        }

        #endregion

        #region  Create
        /// <summary>
        /// CreateActivity
        /// </summary>
        /// <param name="activityIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Activity activityIn)
        {
            m_Logger.LogInformation("Inserting record in Activity table.");
            try
            {
                dynamic response = await m_ActivityService.Create(activityIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully created record in Activity table.");
                    return Ok(response.Activity);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating Activity: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Activity: " + activityIn.Description);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating Activity: " + ex);
                return BadRequest("Error occurred while creating Activity");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// UpdateActivity
        /// </summary>
        /// <param name="activityIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(Activity activityIn)
        {
            m_Logger.LogInformation("updating record in Activity table.");
            try
            {
                dynamic response = await m_ActivityService.Update(activityIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully updated record in Activity table.");
                    return Ok(response.Activity);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating Activity: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Activity: " + activityIn.Description);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating Activity: " + ex);
                return BadRequest("Error occurred while updating Activity");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetActivitys
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from Activity table.");

            try
            {
                var Activitys = await m_ActivityService.GetAll(isActive);
                if (Activitys == null)
                {
                    m_Logger.LogInformation("No records found in Activity table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { Activitys.Count} Activity.");
                    return Ok(Activitys);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Activity.");
            }
        }
        #endregion

        #region GetByActivityId
        /// <summary>
        /// Gets the Activity by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<ActionResult<Activity>> GetByActivityId(int Id)
        {
            m_Logger.LogInformation($"Retrieving records from Activity table by {Id}.");

            try
            {
                var Activity = await m_ActivityService.GetByActivityId(Id);
                if (Activity == null)
                {
                    m_Logger.LogInformation($"No records found for Activity {Id}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for Activity {Id}.");
                    return Ok(Activity);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetActivitysForDropdown
        /// <summary>
        /// GetActivitiesForDropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetActivitiesForDropdown")]
        public async Task<ActionResult<IEnumerable>> GetActivitiesForDropdown()
        {
            m_Logger.LogInformation("Retrieving records from Activity table.");

            try
            {
                var Activitys = await m_ActivityService.GetActivitiesForDropdown();
                if (Activitys == null)
                {
                    m_Logger.LogInformation("No records found in Activity table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { Activitys.Count} Activity.");
                    return Ok(Activitys);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Activity.");
            }
        }
        #endregion

        #region GetExitActivitiesByDepartment
        /// <summary>
        /// GetExitActivitiesByDepartment
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetExitActivitiesByDepartment")]
        public async Task<ActionResult<IEnumerable>> GetExitActivitiesByDepartment(int? departmentId = null)
        {
            m_Logger.LogInformation("Retrieving records from Activity table.");

            try
            {
                var Activitys = await m_ActivityService.GetExitActivitiesByDepartment(departmentId);
                if (Activitys == null)
                {
                    m_Logger.LogInformation("No records found in Activity table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { Activitys.Count} Activity.");
                    return Ok(Activitys);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Activity.");
            }
        }
        #endregion

        #region GetClosureActivitiesByDepartment
        /// <summary>
        /// GetClosureActivitiesByDepartment
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetClosureActivitiesByDepartment")]
        public async Task<ActionResult<IEnumerable>> GetClosureActivitiesByDepartment(int? departmentId = null)
        {
            m_Logger.LogInformation("Retrieving records from Activity table.");

            try
            {
                var Activitys = await m_ActivityService.GetClosureActivitiesByDepartment(departmentId);
                if (Activitys == null)
                {
                    m_Logger.LogInformation("No records found in Activity table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { Activitys.Count} Activity.");
                    return Ok(Activitys);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Activity.");
            }
        }
        #endregion

        #region GetTransitionPlanActivities
        /// <summary>
        /// GetTransitionPlanActivities
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTransitionPlanActivities")]
        public async Task<ActionResult<IEnumerable>> GetTransitionPlanActivities()
        {
            m_Logger.LogInformation("Retrieving records from Activity table.");

            try
            {
                var Activitys = await m_ActivityService.GetTransitionPlanActivities();
                if (Activitys == null)
                {
                    m_Logger.LogInformation("No records found in Activity table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { Activitys.Count} Activity.");
                    return Ok(Activitys);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Activity.");
            }
        }
        #endregion

        //#region GetAllByCategoryName
        ///// <summary>
        ///// GetActivitiesByCategoryName
        ///// </summary>
        ///// <param name="CategoryName"></param>
        ///// <returns></returns>
        //[HttpGet("GetAllByCategoryName")]
        //public async Task<ActionResult<IEnumerable>> GetAllByCategoryName(string CategoryName)
        //{
        //    m_Logger.LogInformation("Retrieving records from Activity table.");

        //    try
        //    {
        //        var activity = await m_ActivityService.GetActivitiesByCategoryName(CategoryName);
        //        if (activity == null)
        //        {
        //            m_Logger.LogInformation("No records found in Activity table.");
        //            return NotFound();
        //        }
        //        else
        //        {
        //            m_Logger.LogInformation($"Returning { activity.Count} Activities.");
        //            return Ok(activity);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        m_Logger.LogError(ex.Message);
        //        return BadRequest();
        //    }


        //}
        //#endregion
    }

}