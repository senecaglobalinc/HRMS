using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class Designation
    {
        public int DesignationId { get; set; }
        public string DesignationCode { get; set; }
        public string DesignationName { get; set; }
        public int? GradeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Grade Grade { get; set; }
    }
}
