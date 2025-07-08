using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class Department
    {
        public int DepartmentId { get; set; }
        public string Description { get; set; }
        public string DepartmentCode { get; set; }
        public int? DepartmentHeadId { get; set; }
        public int DepartmentTypeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual DepartmentType DepartmentType { get; set; }
    }
}
