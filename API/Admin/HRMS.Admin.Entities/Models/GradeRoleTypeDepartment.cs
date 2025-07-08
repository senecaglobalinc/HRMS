using System.Collections.Generic;

namespace HRMS.Admin.Entities.Models
{
    public class GradeRoleTypeDepartment
    {
        public GradeRoleTypeDepartment()
        {
            GradeRoleTypeIds = new List<int>();
        }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public List<int> GradeRoleTypeIds { get; set; }
    }
}
