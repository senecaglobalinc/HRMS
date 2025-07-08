using System.Collections.Generic;

namespace HRMS.Admin.Entities
{
    public class CompetencyArea : BaseEntity
    {
        /// <summary>
        /// CompetencyAreaId
        /// </summary>
        public int CompetencyAreaId { get; set; }

        /// <summary>
        /// CompetencyAreaCode
        /// </summary>
        public string CompetencyAreaCode { get; set; }

        /// <summary>
        /// CompetencyAreaDescription
        /// </summary>
        public string CompetencyAreaDescription { get; set; }

        public virtual ICollection<CompetencySkill> CompetencySkills { get; set; }
        public virtual ICollection<SkillGroup> SkillGroups { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
    }
}
