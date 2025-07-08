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
    public class NotificationTypeController : ControllerBase
    {
        #region  Global Variables

        private readonly INotificationTypeService m_NotificationTypeService;
        private readonly ILogger<NotificationTypeController> m_Logger; 

        #endregion

        #region Constructor
        public NotificationTypeController(INotificationTypeService notificationTypeService, 
            ILogger<NotificationTypeController> logger)
        {
            m_NotificationTypeService = notificationTypeService;
            m_Logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// This method creates new notification type.
        /// </summary>
        /// <param name="notificationTypeIn"></param>
        /// <returns>CompetencyArea</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(NotificationType notificationTypeIn)
        {
            m_Logger.LogInformation("Inserting record in notification type's table.");
            try
            {
                dynamic response = await m_NotificationTypeService.Create(notificationTypeIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating notification type: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in notification type's table.");
                    return Ok(response.NotificationType);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Notification code: " + notificationTypeIn.NotificationCode);
                m_Logger.LogError("Notification description: " + notificationTypeIn.NotificationDescription);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while notification type: " + ex);

                return BadRequest("Error occurred while creating notification type.");
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// This methode deletes notification type.
        /// Before deleting notification type, associated notification configuration will be deleted
        /// </summary>
        /// <param name="notificationTypeID"></param>
        /// <returns>bool</returns>
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int notificationTypeID)
        {
            m_Logger.LogInformation("Deleting record in notification type's table.");

            try
            {
                dynamic response = await m_NotificationTypeService.Delete(notificationTypeID);
                if (!response.IsSuccessful)
                {
                    //Extra information
                    m_Logger.LogError("Notification type Id: " + notificationTypeID);

                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while deleting notification type: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted record in notification type's table.");
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Notification type Id: " + notificationTypeID);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while deleting notification type: " + ex);

                return BadRequest("Error occurred while deleting notification type.");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets notification type's
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns>List<NotificationType></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool? isActive)
        {
            m_Logger.LogInformation("Retrieving records from notification type's table.");

            try
            {
                var notificationTypes = await m_NotificationTypeService.GetAll(isActive);
                if (notificationTypes == null)
                {
                    m_Logger.LogInformation("No records found in notification type's table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { notificationTypes.Count} notification type.");
                    return Ok(notificationTypes);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching notification type's.");
            }
        }
        #endregion

        #region GetByNotificationCode
        /// <summary>
        /// Gets notification type's
        /// </summary>
        /// <param name="notificationCode"></param>
        /// <returns>NotificationType</returns>
        [HttpGet("GetByNotificationCode/{notificationCode}")]
        public async Task<ActionResult<NotificationType>> GetByNotificationCode(string notificationCode)
        {
            m_Logger.LogInformation("Retrieving records from notification type's table by using notification Type.");

            try
            {
                var notificationType = await m_NotificationTypeService.GetByNotificationCode(notificationCode);
                if (notificationType == null)
                {
                    m_Logger.LogInformation("No records found in notification type's table by using notification Type.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning notification type by using notification Type.");
                    return Ok(notificationType);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching notification type by using notification Type.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// This method updates notification type details.
        /// </summary>
        /// <param name="notificationTypeIn"></param>
        /// <returns>NotificationType</returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(NotificationType notificationTypeIn)
        {
            m_Logger.LogInformation("Updating record in notification type's table.");

            try
            {
                dynamic response = await m_NotificationTypeService.Update(notificationTypeIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating notification type: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in notification type table.");
                    return Ok(response.NotificationType);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Notification code: " + notificationTypeIn.NotificationCode);
                m_Logger.LogError("Notification description: " + notificationTypeIn.NotificationDescription);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating notification type: " + ex);

                return BadRequest("Error occurred while updating notification type.");
            }
        }
        #endregion
    }
}