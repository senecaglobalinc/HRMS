using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
    public class BaseEntity
    {
        public string CurrentUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
