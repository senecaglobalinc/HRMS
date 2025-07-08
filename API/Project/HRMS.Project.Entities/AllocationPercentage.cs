using System;
using System.Collections.Generic;

namespace HRMS.Project.Entities
{
    public class AllocationPercentage : BaseEntity
    {
        /// <summary>
        /// AllocationPercentage
        /// </summary>
        public AllocationPercentage()
        {
            AssociateAllocation = new HashSet<AssociateAllocation>();
        }

        /// <summary>
        /// AllocationPercentageId
        /// </summary>
        public int AllocationPercentageId
        {
            get;
            set;
        }

        /// <summary>
        /// Percentage
        /// </summary>
        public decimal Percentage
        {
            get;
            set;
        }

        /// <summary>
        /// AssociateAllocation
        /// </summary>
        public virtual ICollection<AssociateAllocation> AssociateAllocation
        {
            get;
            set;
        }
    }
}
