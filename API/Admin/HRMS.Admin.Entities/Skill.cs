using System.Collections.Generic;
namespace HRMS.Admin.Entities
{
    public class Skill : BaseEntity
    {
        /// <summary>
        /// SkillId
        /// </summary>
        public int SkillId { get; set; }

        /// <summary>
        /// SkillCode
        /// </summary>
        public string SkillCode { get; set; }

        /// <summary>
        /// SkillName
        /// </summary>
        public string SkillName { get; set; }

        /// <summary>
        /// SkillDescription
        /// </summary>
        public string SkillDescription { get; set; }

        /// <summary>
        /// IsApproved
        /// </summary>
        public bool? IsApproved { get; set; }

        /// <summary>
        /// CompetencyAreaId
        /// </summary>
        public int? CompetencyAreaId { get; set; }
      
        /// <summary>
        /// SkillGroupId
        /// </summary>
        public int? SkillGroupId { get; set; }

        /// <summary>
        /// SkillGroup
        /// </summary>
        public virtual SkillGroup SkillGroup { get; set; }

        /// <summary>
        /// CompetencyArea
        /// </summary>
        public virtual CompetencyArea CompetencyArea { get; set; }
        public virtual ICollection<CompetencySkill> CompetencySkills { get; set; }
    }
}
