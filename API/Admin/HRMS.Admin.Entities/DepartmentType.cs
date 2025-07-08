using System.Collections.Generic;

namespace HRMS.Admin.Entities
{

    public class DepartmentType : BaseEntity
    {
        /// <summary>
        /// DepartmentTypeId
        /// </summary>
        public int DepartmentTypeId  {get; set;}

        /// <summary>
        /// DepartmentTypeDescription
        /// </summary>
        public string DepartmentTypeDescription  {get; set;}

        public virtual ICollection<Department> Departments { get; set; }

    }
}
