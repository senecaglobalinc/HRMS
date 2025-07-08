using System;

namespace HRMS.Report.Infrastructure.Models.Domain
{
    public class TalentPoolReportCount
    {
        public int ResourceCount { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
    }   

    public class TalentPoolReportEmployee
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal Experience { get; set; }
        public int DesignationId { get; set; }
        public int GradeId { get; set; }
        public bool IsResigned { get; set; }
        public bool IsLongLeave { get; set; }
        public int DurationInDays { get; set; }
        public string FutureProjectName { get; set; }
        public DateTime? FutureProjectTentativeDate { get; set; }
    }

    public class TalentPoolReport
    {       
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal Experience { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public bool IsResigned { get; set; }
        public bool IsLongLeave { get; set; }
        public int DurationInDays { get; set; }
        public string FutureProjectName { get; set; }
        public DateTime? FutureProjectTentativeDate { get; set; }
    }

}
