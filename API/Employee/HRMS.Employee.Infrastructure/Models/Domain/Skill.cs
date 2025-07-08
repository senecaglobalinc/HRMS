using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class Skill
    {
        public int SkillId { get; set; }
        public string SkillCode { get; set; }
        public string SkillName { get; set; }
        public string SkillDescription { get; set; }
        public bool? IsApproved { get; set; }
        public int? CompetencyAreaId { get; set; }
        public int? SkillGroupId { get; set; }
        public virtual SkillGroup SkillGroup { get; set; }
        public virtual CompetencyArea CompetencyArea { get; set; }
    }
}
