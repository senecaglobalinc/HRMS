using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace HRMS.Admin.Infrastructure.Models
{
    public class NotificationDetail
    {
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string CcEmail { get; set; }
        public string Subject { get; set; }
        public string EmailBody { get; set; }
        public List<string> Attachments { get; set; } = new List<string>();
        public string InlineFilePath { get; set; } 
    }
}
