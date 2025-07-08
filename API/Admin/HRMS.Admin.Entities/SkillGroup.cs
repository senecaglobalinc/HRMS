using System.Collections.Generic;

namespace HRMS.Admin.Entities
{
    public class SkillGroup : BaseEntity
    {
        /// <summary>
        /// SkillGroupId
        /// </summary>
        public int SkillGroupId { get; set; }

        /// <summary>
        /// SkillGroupName
        /// </summary>
        public string SkillGroupName { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// CompetencyAreaId
        /// </summary>
        public int? CompetencyAreaId { get; set; }

        /// <summary>
        /// CompetencyArea
        /// </summary>
        public virtual CompetencyArea CompetencyArea { get; set; }
        public virtual ICollection<CompetencySkill> CompetencySkills { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }

    }
}
