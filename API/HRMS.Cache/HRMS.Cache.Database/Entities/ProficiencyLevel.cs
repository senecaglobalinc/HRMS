using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class ProficiencyLevel
    {
        public int ProficiencyLevelId { get; set; }
        public string ProficiencyLevelCode { get; set; }
        public string ProficiencyLevelDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
