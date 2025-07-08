using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities
{
    public class Activity : BaseEntity
    {
        /// <summary>
        /// ActivityId
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// DepartmentId
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Department
        /// </summary>
        public virtual Department Department { get; set; }       

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ActivityTypeId
        /// </summary>
        public int ActivityTypeId { get; set; }

        /// <summary>
        /// ActivityType
        /// </summary>
        public virtual ActivityType ActivityType { get; set; }

        /// <summary>
        /// IsRequired
        /// </summary>
        public bool? IsRequired { get; set; }
    }
}
