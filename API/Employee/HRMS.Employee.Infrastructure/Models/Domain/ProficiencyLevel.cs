using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ProficiencyLevel
    {
        public int ProficiencyLevelId { get; set; }
        public string ProficiencyLevelCode { get; set; }
        public string ProficiencyLevelDescription { get; set; }
    }
}
