using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Domain
{
    public class ProjectManager
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ProjectId
        /// </summary>
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }

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
        /// IsActive
        /// </summary>
        public bool IsActive { get; set; }

        public string ReportingManagerName { get; set; }
        public string LeadName { get; set; }
        public string ProgramManagerName { get; set; }       

    }
}
