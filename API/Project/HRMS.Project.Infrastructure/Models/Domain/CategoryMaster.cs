using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class CategoryMaster
    {
        /// <summary>
        /// CategoryMasterId
        /// </summary>
        public int CategoryMasterId { get; set; }

        /// <summary>
        /// CategoryName
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// ParentId
        /// </summary>
        public int ParentId { get; set; }
        public string ParentCategoryName { get; set; }

        public virtual ICollection<NotificationType> NotificationTypes { get; set; }
        public virtual ICollection<NotificationConfiguration> NotificationConfigurations { get; set; }

        public virtual ICollection<Status> Status { get; set; }

    }
}
