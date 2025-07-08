using System;
using System.Collections.Generic;


namespace HRMS.Project.Entities

{
    public class AllocationDetails : BaseEntity
    {
        public List<ResourceAllocationDetails> lstBillableResources { get; set; }
        public List<ResourceAllocationDetails> lstNonBillableCriticalResources { get; set; }
        public List<ResourceAllocationDetails> lstNonBillableNonCriticalResources { get; set; }
        public AllocationCount AllocationCount = new AllocationCount();
    }

    public class AllocationCount
    {
        public string ProjectName { get; set; }
        public int ResourceCount { get; set; }
        public int BillableCount { get; set; }
        public int NonBillableCriticalCount { get; set; }
        public int NonBillableNonCriticalCount { get; set; }
    }

    public class ResourceAllocationDetails
    {
        public string AssociateCode { get; set; }
        public string AssociateName { get; set; }
        public decimal AllocationPercentage { get; set; }
        public string ClientBillingRoleName { get; set; }
        public int ClientBillingRoleId { get; set; }
        public string IsPrimaryProject { get; set; }
        public string IsCriticalResource { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
