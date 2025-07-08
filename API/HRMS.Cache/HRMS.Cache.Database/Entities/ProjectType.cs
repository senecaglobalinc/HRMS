using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class ProjectType
    {
        public int ProjectTypeId { get; set; }
        public string ProjectTypeCode { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
