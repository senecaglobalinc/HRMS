using HRMS.Admin.Infrastructure.Models;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface INotificationManagerService
    {
        public Task SendEmail(NotificationDetail notificationDetail);
    }
}
