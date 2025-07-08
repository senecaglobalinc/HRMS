using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Infrastructure
{
    public class APIEndPoints
    {
        public string OrgEndPoint { get; set; }
        public string EmployeeEndPoint { get; set; }
    }

    public class MiscellaneousSettings
    {
        public string Environment { get; set; }
        public string RepositoryPath { get; set; }
    }

    public class EmailConfigurations
    {
        public string FromEmail { get; set; }
        public string HRMEmail { get; set; }
        public string HODEmail { get; set; }
        public string CEOEmail { get; set; }
        public string TestEmail { get; set; }

    }
}
