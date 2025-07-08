using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class SkillGroup
    {
        public int SkillGroupId { get; set; }
        public string SkillGroupName { get; set; }
        public string Description { get; set; }
        public int? CompetencyAreaId { get; set; }
        public CompetencyArea CompetencyArea { get; set; }
    }
}
