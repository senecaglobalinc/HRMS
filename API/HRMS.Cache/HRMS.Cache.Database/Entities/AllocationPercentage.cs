using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class AllocationPercentage
    {
        public AllocationPercentage()
        {
            AssociateAllocation = new HashSet<AssociateAllocation>();
        }

        public int AllocationPercentageId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public decimal Percentage { get; set; }

        public virtual ICollection<AssociateAllocation> AssociateAllocation { get; set; }
    }
}
