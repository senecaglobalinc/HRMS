using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class TalentPoolRequest
    {
        public int ProjectId { get; set; }
        public int PracticeAreaId { get; set; }
    }
}
