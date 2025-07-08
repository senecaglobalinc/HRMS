using System;
using System.Collections.Generic;

namespace HRMS.Report.Infrastructure.Models.Domain
{
    public class UtilizationReport
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string GradeName { get; set; }
        public decimal Experience { get; set; }
        public decimal ExperienceExcludingCareerBreak { get; set; }
        public string Technology { get; set; }
        public DateTime? JoinDate { get; set; }
        public string ProjectName { get; set; }
        public string ClientName { get; set; }
        public bool IsBillable { get; set; }
        public bool IsCritical { get; set; }
        public decimal Allocationpercentage { get; set; }
        public string LeadName { get; set; }
        public string ReportingManagerName { get; set; }
        public string ProgramManagerName { get; set; }
        public string SkillCode { get; set; }
        public bool IsResigned { get; set; }
        public string ResignationDate { get; set; }
        public string LastWorkingDate { get; set; }
        public bool IsLongLeave { get; set; }
        public string LongLeaveStartDate { get; set; }
        public string TentativeJoinDate { get; set; }
        public bool? IsFutureProjectMarked { get; set; }

        public string FutureProjectName { get; set; }
        public string FutureProjectTentativeDate { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectTypeId { get; set; }
    }
    public class UtilizationReportAllocation
    {
        public int AssociateAllocationId { get; set; }
        public int ProjectId { get; set; }        
        public string ProjectName { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }        
        public int ProgramManagerId { get; set; }        
        public int ReportingManagerId { get; set; }        
        public int LeadId { get; set; }        
        public bool IsBillable { get; set; }
        public bool IsCritical { get; set; }       
        public decimal Allocationpercentage { get; set; }
        public bool? IsFutureProjectMarked { get; set; }

        public string FutureProjectName { get; set; }
        public string FutureProjectTentativeDate { get; set; }
        public int ProjectTypeId { get; set; }
    }

    public class UtilizationReportEmployee
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int? GradeId { get; set; }
        public int? DesignationId { get; set; }
        public DateTime? JoinDate { get; set; }
        public int? ReportingManagerId { get; set; }
        public int? ProgramManagerId { get; set; }
        public int? PracticeAreaId { get; set; }
        public int? LeadId { get; set; }
        public bool IsActive { get; set; }
        public int? DepartmentId { get; set; }
        public decimal? Experience { get; set; }
        public int? CompetencyGroup { get; set; }
        public int? EmployeeTypeId { get; set; }
        public string EmployeeType { get; set; }
        public string AadharNumber { get; set; }        
        public int? CareerBreak { get; set; }
        public decimal TotalExperience { get; set; }
        public decimal ExperienceExcludingCareerBreak { get; set; }
        public string SkillCode { get; set; }
        public string Technology { get; set; }
        public int RecordType { get; set; }
        public bool IsResigned { get; set; }
        public string ResignationDate { get; set; }
        public string LastWorkingDate { get; set; }
        public bool IsLongLeave { get; set; }
        public string LongLeaveStartDate { get; set; }
        public string TentativeJoinDate { get; set; }
    }

    public class UtilizationReportAllocationFilter
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public int ClientId { get; set; }
        public int AllocationPercentageId { get; set; }
        public int ProgramManagerId { get; set; }
        public int IsBillable { get; set; } = -1;
        public int IsCritical { get; set; } = -1;
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
        public int ExperienceId { get; set; }
        public string ExperienceRange { get; set; }
        public int Experience { get; set; }
    }

}
