using System;

namespace HRMS.Report.Infrastructure.Models.Request
{
    public class FinanceReportFilter
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? ProjectId { get; set; }
    }

    public class UtilizationReportFilter
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public int GradeId { get; set; }
        public int DesignationId { get; set; }
        public int ClientId { get; set; }
        public int AllocationPercentageId { get; set; }
        public int ProgramManagerId { get; set; }
        public int ExperienceId { get; set; }
        public string ExperienceRange { get; set; }
        public int Experience { get; set; }
        public int PracticeAreaId { get; set; }        
        public int IsBillable { get; set; } = -1;
        public int IsCritical { get; set; } = -1;
    }

    public class AssociateSkillSearchFilter
    {
        public bool IsBillable { get; set; }
        public bool IsCritical { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsnonBillable { get; set; }
        public bool IsnonCritical { get; set; }
        public bool IsSecondary { get; set; }
        public string SkillIds { get; set; }
    }

    public class ParkingSearchFilter
    {
        public string StartDate { get;set; }
        public string Enddate { get; set; }
        public string Location { get; set; }
    }


}
