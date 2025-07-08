using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class NotificationConfiguration
    {
        /// <summary>
        /// NotificationConfigurationId
        /// </summary>
        public int NotificationConfigurationId { get; set; }

        /// <summary>
        /// NotificationTypeId
        /// </summary>
        public int? NotificationTypeId { get; set; }
        public virtual NotificationType NotificationType { get; set; }

        /// <summary>
        /// EmailFrom
        /// </summary>
        public string EmailFrom { get; set; }

        /// <summary>
        /// EmailTo
        /// </summary>
        public string EmailTo { get; set; }

        /// <summary>
        /// EmailCC
        /// </summary>
        public string EmailCC { get; set; }

        /// <summary>
        /// EmailSubject
        /// </summary>
        public string EmailSubject { get; set; }

        /// <summary>
        /// EmailContent
        /// </summary>
        public string EmailContent { get; set; }

        /// <summary>
        /// SLA
        /// </summary>
        public int? SLA { get; set; }

        /// <summary>
        /// CategoryMasterId
        /// </summary>
        public int? CategoryMasterId { get; set; }
        public virtual CategoryMaster CategoryMaster { get; set; }
    }
}
