using System;
using System.Collections.Generic;

namespace HRMS.Project.Entities
{
    public class ProjectClosure : BaseEntity
    {
        /// <summary>
        /// ProjectClosureId
        /// </summary>
        public int ProjectClosureId { get; set; }

        /// <summary>
        /// ProjectId
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// ExpectedClosureDate
        /// </summary>
        public DateTime? ExpectedClosureDate { get; set; }

        /// <summary>
        /// ActualClosureDate
        /// </summary>
        public DateTime? ActualClosureDate { get; set; }

        /// <summary>
        /// IsTransitionRequired
        /// </summary>
        public bool? IsTransitionRequired { get; set; }

        /// <summary>
        /// StatusId
        /// </summary>
        public int StatusId { get; set; }

        public Project Project { get; set; }
        public ProjectClosureReport projectClosureReport;
        public virtual ICollection<ProjectClosureWorkflow> ProjectClosureWorkflows { get; set; }
        public virtual ICollection<ProjectClosureActivity> ProjectClosureActivities { get; set; }            

    }
}
