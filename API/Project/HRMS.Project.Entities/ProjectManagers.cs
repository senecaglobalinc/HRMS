using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
    public class ProjectManager : BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ProjectId
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// ReportingManagerId
        /// </summary>
        public int? ReportingManagerId { get; set; }

        /// <summary>
        /// ProgramManagerId
        /// </summary>
        public int? ProgramManagerId { get; set; }

        /// <summary>
        /// LeadId
        /// </summary>
        public int? LeadId { get; set; }

        /// <summary>
        /// Project
        /// </summary>
        public virtual Project Project { get; set; }
    }
}
