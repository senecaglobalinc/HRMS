using System;
using System.Collections.Generic;

namespace HRMS.Project.Infrastructure.Models.Domain
{    
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
        public int? AllocationPercentageId { get; set; }
        public decimal Allocationpercentage { get; set; }

        public bool? IsFutureProjectMarked { get; set; }

        public string FutureProjectName { get; set; }
        public DateTime? FutureProjectTentativeDate { get; set; }
        public int ProjectTypeId { get; set; }

    }

    public class DomainDataCount
    {
        public int DomainID { get; set; }
        public int EmployeeID { get; set; }
    }

    public class TalentpoolDataCount
    {
        public int ResourceCount { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
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

    public class ProjectResourceData
    {
        public int EmployeeId { get; set; }
        public int DurationInDays { get; set; }
        public string FutureProjectName { get; set; }
        public DateTime? FutureProjectTentativeDate { get; set; }
    }
}
