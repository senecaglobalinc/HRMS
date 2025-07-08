using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class NotificationConfiguration
    {
        public int NotificationConfigurationId { get; set; }
        public int? NotificationTypeId { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string EmailCc { get; set; }
        public string EmailSubject { get; set; }
        public string EmailContent { get; set; }
        public int? Sla { get; set; }
        public int? CategoryMasterId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Categories CategoryMaster { get; set; }
        public virtual NotificationType NotificationType { get; set; }
    }
}
