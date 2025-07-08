using System;

namespace HRMS.Admin.Infrastructure
{
    public class APIEndPoints
    {
        public string ProjectEndPoint { get; set; }
        public string AssociateEndPoint { get; set; }
        public string OrgHealthCheckEndPoint { get; set; }
    }

    public class EmailConfigurations
    {
        public string SMTPClient { get; set; }
        public bool IsSendMail { get; set; }
        public string LogoPath { get; set; }
    }

    public class MiscellaneousSettings
    {
        public string Environment { get; set; }
    }
}
