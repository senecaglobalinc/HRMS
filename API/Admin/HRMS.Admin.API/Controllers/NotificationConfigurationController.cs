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
    public class NotificationConfigurationController : ControllerBase
    {
        #region  Global Variables

        private readonly INotificationConfigurationService m_NotificationConfigurationService;
        private readonly ILogger<NotificationConfigurationController> m_Logger; 

        #endregion

        #region Constructor
        public NotificationConfigurationController(INotificationConfigurationService notificationConfigurationService, 
            ILogger<NotificationConfigurationController> logger)
        {
            m_NotificationConfigurationService = notificationConfigurationService;
            m_Logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// This method creates new notification configuration.
        /// </summary>
        /// <param name="notificationConfigurationIn"></param>
        /// <returns>NotificationConfiguration</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(NotificationConfiguration notificationConfigurationIn)
        {
            m_Logger.LogInformation("Inserting record in notification configuration's table.");
            try
            {
                dynamic response = await m_NotificationConfigurationService.Create(notificationConfigurationIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating notification configuration: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in notification configuration's table.");
                    return Ok(response.NotificationConfiguration);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Notification type id: " + notificationConfigurationIn.NotificationTypeId);
                m_Logger.LogError("Category id: " + notificationConfigurationIn.CategoryMasterId);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while notification configuration: " + ex);

                return BadRequest("Error occurred while creating notification configuration.");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets notification configuration's
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns>List<NotificationConfiguration></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool? isActive)
        {
            m_Logger.LogInformation("Retrieving records from notification configuration's table.");

            try
            {
                var notificationTypes = await m_NotificationConfigurationService.GetAll(isActive);
                if (notificationTypes == null)
                {
                    m_Logger.LogInformation("No records found in notification configuration's table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { notificationTypes.Count} notification configuration.");
                    return Ok(notificationTypes);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching notification configuration's.");
            }
        }
        #endregion

        #region GetByNotificationTypeAndCategory
        /// <summary>
        /// Gets notification configuration's
        /// </summary>
        /// <param name="notificationTypeId"></param>
        /// <param name="categoryMasterId"></param>
        /// <returns>NotificationConfiguration</returns>
        [HttpGet("GetByNotificationTypeAndCategory")]
        public async Task<ActionResult<NotificationConfiguration>> GetByNotificationTypeAndCategory(int? notificationTypeId, int? categoryMasterId)
        {
            m_Logger.LogInformation("Retrieving records from notification configuration table by using notification type id and category id.");

            try
            {
                var notificationConfiguration = 
                    await m_NotificationConfigurationService.GetByNotificationTypeAndCategory(notificationTypeId, categoryMasterId);

                    m_Logger.LogInformation($"Returning notification configuration for passed notification type id and category id.");
                    return Ok(notificationConfiguration);
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching notification configuration by using notification type id and category id.");
            }
        }
        #endregion

        #region GetByNotificationTypeAndEmailTo
        /// <summary>
        /// Gets notification configuration's
        /// </summary>
        /// <param name="notificationTypeId"></param>
        /// <param name="emailTo"></param>
        /// <returns>NotificationConfiguration</returns>
        [HttpGet("GetByNotificationTypeAndEmailTo")]
        public async Task<ActionResult<IEnumerable>> GetByNotificationTypeAndEmailTo(int? notificationTypeId, string emailTo)
        {
            m_Logger.LogInformation("Retrieving records from notification configuration table by using notification type id and email To.");

            try
            {
                var notificationConfiguration =
                    await m_NotificationConfigurationService.GetByNotificationTypeAndEmailTo(notificationTypeId, emailTo);

                m_Logger.LogInformation($"Returning notification configuration for passed notification type id and email To.");
                return Ok(notificationConfiguration);
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching notification configuration by using notification type id and email To.");
            }
        }
        #endregion

        #region GetEmployeeWorkEmails
        /// <summary>
        /// Gets notification configuration's
        /// </summary>      
        /// <returns>String</returns>
        [HttpGet("GetEmployeeWorkEmails")]
        public async Task<ActionResult<IEnumerable>> GetEmployeeWorkEmails()
        {
            m_Logger.LogInformation("Retrieving records from Work Emails.");

            try
            {
                var notificationConfiguration =
                    await m_NotificationConfigurationService.GetEmployeeWorkEmails();

                m_Logger.LogInformation($"Returning Work Emails.");
                return Ok(notificationConfiguration);
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Work Emails.");
            }
        }
        #endregion

        #region GetNotificationConfiguration
        /// <summary>
        /// Get Notification Configuration
        /// </summary>
        /// <param name="notificationCode"></param>
        /// <param name="categoryMasterId"></param>
        /// <returns>NotificationConfiguration</returns>
        [HttpGet("GetNotificationConfiguration")]
        public async Task<ActionResult<NotificationConfiguration>> GetNotificationConfiguration(string notificationCode, int? categoryMasterId)
        {
            m_Logger.LogInformation("Retrieving records from notification configuration table by using notification code and category id.");

            try
            {
                var notificationConfiguration =
                    await m_NotificationConfigurationService.GetNotificationConfiguration(notificationCode, categoryMasterId);

                m_Logger.LogInformation($"Returning notification configuration for passed notification code and category id.");
                return Ok(notificationConfiguration);
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching notification configuration by using notification code and category id.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// This method updates notification configuration details.
        /// </summary>
        /// <param name="notificationConfigurationIn"></param>
        /// <returns>NotificationConfiguration</returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(NotificationConfiguration notificationConfigurationIn)
        {
            m_Logger.LogInformation("Updating record in notification configuration's table.");

            try
            {
                dynamic response = await m_NotificationConfigurationService.Update(notificationConfigurationIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating notification configuration: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in notification configuration table.");
                    return Ok(response.NotificationConfiguration);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Notification type id: " + notificationConfigurationIn.NotificationTypeId);
                m_Logger.LogError("Category id: " + notificationConfigurationIn.CategoryMasterId);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating notification configuration: " + ex);

                return BadRequest("Error occurred while updating notification configuration.");
            }
        }
        #endregion
    }
}