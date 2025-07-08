using HRMS.Project.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class AssociateAllocationDetails
    {
        public int AssociateAllocationId { get; set; }
        public int? TalentRequisitionId { get; set; }
        public int? ProjectId { get; set; }
        public int? ProjectTypeId { get; set; }
        public int? EmployeeId { get; set; }
        public string AssociateName { get; set; }
        public string RoleName { get; set; }
        public string Project { get; set; }
        public int? RoleMasterId { get; set; }
        public int? AllocationPercentage { get; set; }
        public decimal? InternalBillingPercentage { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? AllocationDate { get; set; }
        public int? ReportingManagerId { get; set; }
        public bool? IsPrimary { get; set; }
        public bool? IsBillable { get; set; }
        public bool? Billable { get; set; }
        public int? InternalBillingRoleId { get; set; }
        public int? ClientBillingRoleId { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal? ClientBillingPercentage { get; set; }
        public int? ProgramManagerId { get; set; }
        public int? LeadId { get; set; }
        public bool NotifyAll { get; set; }
        public int RAId { get; set; }
        public int? RequisitionRoleDetailID { get; set; }
        public string DepartmentName { get; set; }
        public string ProjectType { get; set; }
        public int? RoleRequisitionID { get; set; }
        public int? TotalNoOfPosition { get; set; }
        public int? NoOfBillablePosition { get; set; }
        public int? NoOfNonBillablePosition { get; set; }
        public int? NoOfAllocatedBillablePosition { get; set; }
        public int? NoOfAllocatedNonBillablePosition { get; set; }
        public int? NoOfRemainingBillablePosition { get; set; }
        public int? NoOfRemainingNonBillablePosition { get; set; }
        public string EmployeeNameWithGrade { get; set; }
        public decimal ResourceAvailability { get; set; }
        public Nullable<bool> isCritical { get; set; }
        public string Status { get; set; }
        public string ReportingManager { get; set; }
        public string Lead { get; set; }
        public decimal? Availability { get; set; }
        public int? CompetencyAreaId { get; set; }
        public bool IsFullyMatched { get; set; } = false;
        public int ReleaseProjectId { get; set; }
        public decimal? Percentage { get; set; }
        public decimal ReleasingPercentage { get; set; }
        public int MakePrimaryProjectId { get; set; }
        public List<int> remainingProjects { get; set; }
    }

    //public class AssociateProjectAllocation
    //{
    //    public int? projectId;
    //    public decimal allocationPercentage;
    //    public bool? isCritical;
    //    public int? AllocatedTimeSpan;
    //}

    public class CompetencyAreaMananagers
    {
        public int? ReportingManagerId { get; set; }
        public int? ProgramManagerId { get; set; }
        public int ?LeadId { get; set; }      
        public int ?ProjectId { get; set; }      
    }
}
