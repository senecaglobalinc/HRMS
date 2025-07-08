using System;
using HRMS.Admin.Infrastructure.Models;
using HRMS.Admin.Types;
using HRMS.Admin.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HRMS.Admin.API.Controllers
{
    [Route("admin/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationManagerController : Controller
    {
        #region Global Variables

        private readonly INotificationManagerService m_notificationManagerService;
        private readonly ILogger<NotificationManagerController> m_Logger;
        #endregion

        #region Constructor
        public NotificationManagerController(INotificationManagerService notificationManagerService, ILogger<NotificationManagerController> logger)
        {
            m_notificationManagerService = notificationManagerService;
            m_Logger = logger;
        }
        #endregion

        #region Send Email
        /// <summary>
        /// This method sends Email.
        /// </summary>
        /// <param name="notificationDetail"></param>
        /// <returns>notificationDetail</returns>
        [HttpPost("SendEmail")]
        public async Task SendEmail(NotificationDetail notificationDetail)
        {
            m_Logger.LogInformation("Sending Email notification.");

            try
            {
               await m_notificationManagerService.SendEmail(notificationDetail);
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation("Error while sending notification");
                throw ex;
            }
        }
        #endregion
    }
}
