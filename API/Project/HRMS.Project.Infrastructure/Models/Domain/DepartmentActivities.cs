using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class DepartmentActivities
    {
        /// <summary>
        /// ActivityId
        /// </summary>
        public int ActivityId { get; set; }
        /// <summary>
        /// ActivityDescription
        /// </summary>
        public string ActivityDescription { get; set; }
        /// <summary>
        /// DepartmentId
        /// </summary>
        public int DepartmentId { get; set; }
        /// <summary>
        /// DepartmentDescription
        /// </summary>
        public string DepartmentDescription { get; set; }
        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// value
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// StatusId
        /// </summary>
        public int StatusId { get; set; }
        /// <summary>
        /// StatusDescription
        /// </summary>
        public string StatusDescription { get; set; }
    }
}
