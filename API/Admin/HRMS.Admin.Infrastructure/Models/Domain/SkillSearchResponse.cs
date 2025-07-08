using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Infrastructure.Models.Domain
{
    public class SkillSearchResponse
    {

        public int? SkillId { get; set; }
        public string SkillName { get; set; }
        public int? SkillGroupId { get; set; }
        public string SkillGroupName { get; set; }
        public int? CompetencyAreaId { get; set; }
        public string CompetencyAreaName { get; set; }
    }
}
