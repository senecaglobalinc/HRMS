using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Request
{
    public class FinanceReportEmployeeFilter
    {
        public List<int> EmployeeIdList { get; set; }
        public List<int> LeadIdList { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class UtilizationReportEmployeeFilter
    {
        public List<int> EmployeeIdList { get; set; }
        public List<int> LeadIdList { get; set; }
        public int EmployeeId { get; set; }
        public int GradeId { get; set; }
        public int DesignationId { get; set; }
        public int PracticeAreaId { get; set; }
        public int MinExperience { get; set; } = -1;
        public int MaxExperience { get; set; } = -1;        
    }
    public class SkillSearchFilter
    {
        public bool IsBillable { get; set; }
        public bool IsCritical { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsnonBillable { get; set; }
        public bool IsnonCritical { get; set; }
        public bool IsSecondary { get; set; }
        public string SkillIds { get; set; }
    }

    public class AssociateExitReportFilter
    {
        public int ReportType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }        
    }
}
