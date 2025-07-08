using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class NotificationConfiguration
    {
        public int NotificationConfigurationId { get; set; }
        public int? NotificationTypeId { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string EmailCC { get; set; }
        public string EmailSubject { get; set; }
        public string EmailContent { get; set; }
        public int? SLA { get; set; }
        public int? CategoryMasterId { get; set; }
    }
}
