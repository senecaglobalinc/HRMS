using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class NotificationType
    {
        /// <summary>
        /// Id-originaly it was not there added for maintaining primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// NotificationTypeID
        /// </summary>
        public int NotificationTypeId { get; set; }

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
