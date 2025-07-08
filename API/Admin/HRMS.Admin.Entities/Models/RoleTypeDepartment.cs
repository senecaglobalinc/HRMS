using System.Collections.Generic;

namespace HRMS.Admin.Entities.Models
{
    public class RoleTypeDepartment
    {
        public RoleTypeDepartment()
        {
            RoleTypeIds = new List<int>();
        }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public List<int> RoleTypeIds { get; set; }
    }
}
