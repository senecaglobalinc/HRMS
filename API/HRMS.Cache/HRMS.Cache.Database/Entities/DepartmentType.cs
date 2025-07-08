using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class DepartmentType
    {
        public DepartmentType()
        {
            Department = new HashSet<Department>();
        }

        public int DepartmentTypeId { get; set; }
        public string DepartmentTypeDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<Department> Department { get; set; }
    }
}
