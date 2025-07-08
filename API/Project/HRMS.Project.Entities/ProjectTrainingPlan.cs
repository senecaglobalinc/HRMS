using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
    public class ProjectTrainingPlan : BaseEntity
    {
        /// <summary>
        /// ProjectTrainingPlanId
        /// </summary>
        public int ProjectTrainingPlanId { get; set; }

        /// <summary>
        /// ProjectId
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// AssociateId
        /// </summary>
        public int AssociateId { get; set; }

        /// <summary>
        /// SkillId
        /// </summary>
        public int SkillId { get; set; }

        /// <summary>
        /// ProjectTrainingPlanned
        /// </summary>
        public string ProjectTrainingPlanned { get; set; }

        /// <summary>
        /// TrainingFromDate
        /// </summary>
        public DateTime? TrainingFromDate { get; set; }

        /// <summary>
        /// TrainingToDate
        /// </summary>
        public DateTime? TrainingToDate { get; set; }

        /// <summary>
        /// FinancialYearId
        /// </summary>
        public int FinancialYearId { get; set; }

        /// <summary>
        /// CycleId
        /// </summary>
        public int CycleId { get; set; }

        /// <summary>
        /// TrainingModeId
        /// </summary>
        public int TrainingModeId { get; set; }

        /// <summary>
        /// IsTrainingCompleted
        /// </summary>
        public bool IsTrainingCompleted { get; set; }

        /// <summary>
        /// SkillAssessedBy
        /// </summary>
        public int SkillAssessedBy { get; set; }

        /// <summary>
        /// SkillAssessmentDate
        /// </summary>
        public DateTime? SkillAssessmentDate { get; set; }

        /// <summary>
        /// SkillApplied
        /// </summary>
        public bool SkillApplied { get; set; }

        /// <summary>
        /// ProficiencyLevelAchieved
        /// </summary>
        public int ProficiencyLevelAchieved { get; set; }

        public virtual Project Project { get; set; }
        public virtual TrainingMode TrainingMode { get; set; }
    }
}
