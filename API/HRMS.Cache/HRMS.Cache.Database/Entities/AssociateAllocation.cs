using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class AssociateAllocation
    {
        public int AssociateAllocationId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int? Trid { get; set; }
        public int? ProjectId { get; set; }
        public int? EmployeeId { get; set; }
        public int? RoleMasterId { get; set; }
        public int? AllocationPercentage { get; set; }
        public decimal? InternalBillingPercentage { get; set; }
        public bool? IsCritical { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? AllocationDate { get; set; }
        public int? ReportingManagerId { get; set; }
        public bool? IsPrimary { get; set; }
        public bool? IsBillable { get; set; }
        public int? InternalBillingRoleId { get; set; }
        public int? ClientBillingRoleId { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal? ClientBillingPercentage { get; set; }
        public int? ProgramManagerId { get; set; }
        public int? LeadId { get; set; }

        public virtual AllocationPercentage AllocationPercentageNavigation { get; set; }
        public virtual Projects Project { get; set; }
        public virtual TalentRequisition Tr { get; set; }
    }
}
