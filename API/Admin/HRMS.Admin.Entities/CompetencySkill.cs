using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities
{
    public class CompetencySkill : BaseEntity
    {
        public int CompetencySkillId { get; set; }
        public int? RoleMasterId { get; set; }
        public int? CompetencyAreaId { get; set; }
        public int? SkillId { get; set; }
        public int? SkillGroupId { get; set; }
        public bool? IsPrimary { get; set; }
        public int ProficiencyLevelId { get; set; }
        public virtual CompetencyArea CompetencyArea { get; set; }
        public virtual ProficiencyLevel ProficiencyLevel { get; set; }
        public virtual Skill Skill { get; set; }
        public virtual SkillGroup SkillGroup { get; set; }
        public virtual RoleMaster RoleMaster { get; set; }
    }
}
