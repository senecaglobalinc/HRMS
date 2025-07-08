using System;

namespace HRMS.Employee.Infrastructure
{
    public class APIEndPoints
    {
        public string ProjectEndPoint { get; set; }
        public string OrgEndPoint { get; set; }
        public string KRAEndPoint { get; set; }
    }

    public class MiscellaneousSettings
    {
        public string Environment { get; set; }        
        public string RepositoryPath { get; set; }
        public string PdfPassword { get; set; }
        public string WFHAssociates { get; set; }
        public string ExcludeAssociates { get; set; }
        public int NoticePeriodInDays { get; set; }
    }

    public class EmailConfigurations
    {
        public bool SendEmail { get; set; }
        public string ToEmail { get; set; }
        public string CcEmail { get; set; }
        public string FromEmail { get; set; }
        public string DeptCodes { get; set; }
        public string HRManagerRole { get; set; }
        public string HigherOfficialEmail { get; set; }
        public string FeedbackNotificationEmail { get; set; }
        public string FinanceCCEmail { get; set; }
        public string DepartmentHeadEmail { get; set; }
    }

    public class WelcomeEmailConfigurations
    {
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
    }

    public class MigrationEmailConfigurations
    {
        public string SendEmail { get; set; }
        public string ToEmail { get; set; }
        public string CcEmail { get; set; }
        public string FromEmail { get; set; }
        public string ProcessedRepoPath { get; set; }
        public string TobeprocessedRepoPath { get; set; }
    }
    public class ParkingSlot
    {
        public int TotalParkingSlots { get; set; }
        public int GalaxyTotalParkingSlots { get; set; }
        public int ShilparamamTotalParkingSlots { get; set; }

    }
}
