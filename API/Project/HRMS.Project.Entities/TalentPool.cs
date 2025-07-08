using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
    public class TalentPool : BaseEntity
    {
        public int TalentPoolId { get; set; }
        public int? PracticeAreaId { get; set; }
        public int? ProjectId { get; set; }
    }
}
