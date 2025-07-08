using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class Skill
    {
        public int SkillId { get; set; }
        public string SkillCode { get; set; }
        public string SkillName { get; set; }
        public string SkillDescription { get; set; }
        public bool? IsApproved { get; set; }
        public int? CompetencyAreaId { get; set; }
        public int? SkillGroupId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual CompetencyArea CompetencyArea { get; set; }
        public virtual SkillGroup SkillGroup { get; set; }
    }
}
