using System;
using System.Collections.Generic;

namespace HRMS.Employee.Infrastructure.Domain
{
    public class AllocationPercentage 
    {
        /// <summary>
        /// AllocationPercentage
        /// </summary>
        public AllocationPercentage()
        {
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
        
    }
}
