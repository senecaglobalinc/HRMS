using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class PracticeArea
    {
        public int PracticeAreaId { get; set; }
        public string PracticeAreaCode { get; set; }
        public string PracticeAreaDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
