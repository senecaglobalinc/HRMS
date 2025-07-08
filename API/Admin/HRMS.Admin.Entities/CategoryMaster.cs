using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Admin.Entities
{
    public class CategoryMaster : BaseEntity
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
