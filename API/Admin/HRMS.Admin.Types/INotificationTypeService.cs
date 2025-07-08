using HRMS.Admin.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    /// <summary>
    /// Interface for notification type service
    /// </summary>
    public interface INotificationTypeService
    {
        //Create notification type abstract method 
        Task<dynamic> Create(NotificationType notificationTypeIn);

        //Get notification type details abstract method
        Task<List<NotificationType>> GetAll(bool? isActive);

        //Get notification type by notication type id abstract method
        Task<NotificationType> GetByNotificationTypeId(int? notificationTypeId);

        //Get notification type by notication type id abstract method
        Task<NotificationType> GetByNotificationCode(string notificationCode);

        //Update notification type abstract method
        Task<dynamic> Update(NotificationType notificationTypeIn);

        //Deletes notification type abstract method
        Task<dynamic> Delete(int notificationTypeID);
    }
}
