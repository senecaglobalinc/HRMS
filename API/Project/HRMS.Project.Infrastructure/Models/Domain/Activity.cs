using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class Activity
    {
        /// <summary>
        /// ActivityId
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// Department
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// DepartmentId
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// ActivityType
        /// </summary>
        public string ActivityType { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
    }
}
