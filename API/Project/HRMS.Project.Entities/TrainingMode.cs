using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
    public class TrainingMode : BaseEntity
    {
        /// <summary>
        /// TrainingModeId
        /// </summary>
        public int TrainingModeId { get; set; }

        /// <summary>
        /// TrainingModeCode
        /// </summary>
        public string TrainingModeCode { get; set; }

        public virtual ICollection<ProjectTrainingPlan> ProjectTrainingPlan { get; set; }
    }
}
