using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class Categories
    {
        public Categories()
        {
            NotificationConfiguration = new HashSet<NotificationConfiguration>();
            NotificationType = new HashSet<NotificationType>();
        }

        public int CategoryMasterId { get; set; }
        public string CategoryName { get; set; }
        public int? ParentId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<NotificationConfiguration> NotificationConfiguration { get; set; }
        public virtual ICollection<NotificationType> NotificationType { get; set; }
    }
}
