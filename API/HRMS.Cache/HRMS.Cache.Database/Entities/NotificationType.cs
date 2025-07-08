using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class NotificationType
    {
        public NotificationType()
        {
            NotificationConfiguration = new HashSet<NotificationConfiguration>();
        }

        public int NotificationTypeId { get; set; }
        public string NotificationCode { get; set; }
        public string NotificationDescription { get; set; }
        public int? CategoryMasterId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Categories CategoryMaster { get; set; }
        public virtual ICollection<NotificationConfiguration> NotificationConfiguration { get; set; }
    }
}
