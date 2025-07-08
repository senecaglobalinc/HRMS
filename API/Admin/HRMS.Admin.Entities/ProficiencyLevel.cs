using System.Collections.Generic;

namespace HRMS.Admin.Entities
{
    public class ProficiencyLevel : BaseEntity
    {
        /// <summary>
        /// ProficiencyLevelId
        /// </summary>
        public int ProficiencyLevelId { get; set; }

        /// <summary>
        /// ProficiencyLevelCode
        /// </summary>
        public string ProficiencyLevelCode { get; set; }

        /// <summary>
        /// ProficiencyLevelDescription
        /// </summary>
        public string ProficiencyLevelDescription { get; set; }
        public virtual ICollection<CompetencySkill> CompetencySkills { get; set; }
    }
}
