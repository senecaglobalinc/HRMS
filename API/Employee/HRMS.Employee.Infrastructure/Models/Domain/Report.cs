using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
   public class FinanceReportEmployee
    {
        public int EmployeeId { get; set; }       
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public bool IsActive { get; set; }
        public int? ProjectId { get; set; }
        public int? GradeId { get; set; }      
        public int? DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public string Skills { get; set; }
        public int RecordType { get; set; }
        public bool IsResigned { get; set; }
        public bool IsLongLeave { get; set; }
        public DateTime ? LastWorkingDate { get; set; }
        
    }

    public class UtilizationReportEmployee
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }        
        public int? GradeId { get; set; }
        public int? DesignationId { get; set; }
        public int? PracticeAreaId { get; set; }
        public DateTime? JoinDate { get; set; }
        public int? ReportingManagerId { get; set; }
        public int? ProgramManagerId { get; set; }
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

    public enum RecordTypeEnum {
     Associate =1 ,
     Manager = 2,
     Client =3,
     Skill = 4,
     Department = 5,
     Designation = 6,
     Grade = 7,
     PracticeArea = 8
    }
    public class DomainDataCount
    {
        public int DomainID { get; set; }
        public int EmployeeID { get; set; }
    }

    public class ServiceTypeCount
    {
        public int ServiceTypeId { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }

    public class DomainReportEmployee
    {
        public int EmployeeId { get; set; }        
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal Experience { get; set; }
        public int DesignationId { get; set; }
        public int GradeId { get; set; }
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
        public DateTime? LastWorkingDate { get; set; }

    }
    public class SkillSearchAllocation
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int EmployeeId { get; set; }
        public int ProgramManagerId { get; set; }
        public int ReportingManagerId { get; set; }
        public int LeadId { get; set; }
        public decimal Allocationpercentage { get; set; }
    }
    public class SkillSearchEmployee
    {
        public int EmployeeId { get; set; }
        public int SkillId { get; set; }
        public bool IsBillable { get; set; }
        public bool IsCritical { get; set; }       
        public decimal Experience { get; set; }       
        public int DesignationId { get; set; }
        public string EmployeeName { get; set; }
        public int GradeId { get; set; }
        public string LeadName { get; set; }
        public string ManagerName { get; set; }
        public int ProjectId { get; set; }        
        public string ProjectName { get; set; }
        public bool IsSkillPrimary { get; set; }          
        public string PrimarySkill { get; set; }
        public string SecondarySkill { get; set; }        
        public decimal Allocationpercentage { get; set; }
    }
    public class SkillSearchAssociateAllocation
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsBillable { get; set; }
        public bool IsCritical { get; set; }
        public int RoleMasterId { get; set; }
    }
    public class GenericType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? DepartmentId { get; set; }
    }
    public class ProjectResourceData
    {
        public int EmployeeId { get; set; }
        public int DurationInDays { get; set; }
        public string FutureProjectName { get; set; }
        public DateTime? FutureProjectTentativeDate { get; set; }
    }       

    public class ChartData
    {
        public string Label { get; set; }
        public int Value { get; set; }
    }    
}
