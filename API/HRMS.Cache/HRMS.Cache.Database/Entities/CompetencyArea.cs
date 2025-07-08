using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class CompetencyArea
    {
        public CompetencyArea()
        {
            Skill = new HashSet<Skill>();
            SkillGroup = new HashSet<SkillGroup>();
        }

        public int CompetencyAreaId { get; set; }
        public string CompetencyAreaCode { get; set; }
        public string CompetencyAreaDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<Skill> Skill { get; set; }
        public virtual ICollection<SkillGroup> SkillGroup { get; set; }
    }
}
