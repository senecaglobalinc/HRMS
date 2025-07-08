using System;
using System.Collections.Generic;

namespace HRMS.Report.Infrastructure.Models.Domain
{
    public class FinanceReport
    {
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int AssociateAllocationId { get; set; }
        public int ProgramManagerId { get; set; }
        public string ProgramManagerName { get; set; }
        public int ReportingManagerId { get; set; }
        public string ReportingManagerName { get; set; }
        public int LeadId { get; set; }
        public string LeadName { get; set; }
        public bool IsBillable { get; set; }            
        public bool IsCritical { get; set; }        
        public decimal ClientBillingPercentage { get; set; }
        public decimal Allocationpercentage { get; set; }
        public decimal InternalBillingPercentage { get; set; }
        public int InternalBillingRoleId { get; set; }
        public string ClientBillingRoleCode { get; set; }
        public string InternalBillingRoleCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public bool IsActive { get; set; }
        public int? GradeId { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public string GradeName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }        
        public string RoleName { get; set; }
        public string SkillCode { get; set; }
        public bool IsResigned { get; set; }
        public bool IsLongLeave { get; set; }
    }
    public class FinanceReportAllocation
    {
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int AssociateAllocationId { get; set; }
        public int ProgramManagerId { get; set; }        
        public int ReportingManagerId { get; set; }        
        public int LeadId { get; set; }        
        public bool IsBillable { get; set; }
        public bool IsCritical { get; set; }
        public decimal ClientBillingPercentage { get; set; }
        public decimal Allocationpercentage { get; set; }
        public decimal InternalBillingPercentage { get; set; }
        public int InternalBillingRoleId { get; set; }
        public string ClientBillingRoleCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

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
    }   
    public class FinanceReportEmployeeFilter
    {
        public List<int> EmployeeIdList { get; set; }
        public List<int> LeadIdList { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
