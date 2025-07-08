using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class SkillGroup
    {
        public SkillGroup()
        {
            Skill = new HashSet<Skill>();
        }

        public int SkillGroupId { get; set; }
        public string SkillGroupName { get; set; }
        public string Description { get; set; }
        public int? CompetencyAreaId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual CompetencyArea CompetencyArea { get; set; }
        public virtual ICollection<Skill> Skill { get; set; }
    }
}
