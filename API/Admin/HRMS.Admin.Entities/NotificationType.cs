using System.Collections.Generic;
namespace HRMS.Admin.Entities
{
    public class NotificationType : BaseEntity
    {
        /// <summary>
        /// NotificationTypeID-originaly it was not there added for maintaining primary key
        /// </summary>
        public int? NotificationTypeId { get; set; }

        /// <summary>
        /// NotificationCode
        /// </summary>
        public string NotificationCode { get; set; }

        /// <summary>
        /// NotificationDesc
        /// </summary>
        public string NotificationDescription { get; set; }

        /// <summary>
        /// CategoryMasterId
        /// </summary>
        public int? CategoryMasterId { get; set; }
        public virtual CategoryMaster CategoryMaster { get; set; }

        public virtual ICollection<NotificationConfiguration> NotificationConfigurations { get; set; }

    }

}

