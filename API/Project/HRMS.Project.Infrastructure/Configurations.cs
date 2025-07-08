using System;

namespace HRMS.Project.Infrastructure
{
    public class APIEndPoints
    {
        public string EmployeeEndPoint { get; set; }
        public string OrgEndPoint { get; set; }
    }

    public class MiscellaneousSettings
    {
        public string Environment { get; set; }
        public string RepositoryPath { get; set; }
        public string HigherOfficialEmail { get; set; }
    }

    public class EmailConfigurations
    {
        public bool SendEmail { get; set; }
        public string FromEmail { get; set; }
        public string CCEmail { get; set; }
        public string ToEmail { get; set; }
    }
}
