using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class TalentPool
    {
        public int TalentPoolId { get; set; }
        public int? PracticeAreaId { get; set; }
        public int? ProjectId { get; set; }
    }
}
