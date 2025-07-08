using HRMS.Admin.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    /// <summary>
    /// Interface for notification configuration
    /// </summary>
    public interface INotificationConfigurationService
    {
        //Create notification configuration abstract method 
        Task<dynamic> Create(NotificationConfiguration notificationConfigurationIn);

        //Get notification configuration details abstract method
        Task<List<NotificationConfiguration>> GetAll(bool? isActive);

        //Get notification configuration by using notification type id and category id
        Task<NotificationConfiguration> GetByNotificationTypeAndCategory(int? notificationTypeId, int? categoryMasterId);

        //Get notification configuration by using notification code and category id
        Task<NotificationConfiguration> GetNotificationConfiguration(string notificationCode, int? categoryMasterId);

        //Get notification configuration by using notification type id and email to
        Task<NotificationConfiguration> GetByNotificationTypeAndEmailTo(int? notificationTypeId, string emailTo);

        //Update notification configuration abstract method
        Task<dynamic> Update(NotificationConfiguration notificationConfigurationIn);
        Task<List<string>> GetEmployeeWorkEmails();
    }
}