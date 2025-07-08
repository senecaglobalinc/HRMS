using System.Collections.Generic;

namespace HRMS.Admin.Entities
{
    public class RoleType : BaseEntity
    {
        public int RoleTypeId { get; set; }
        public string RoleTypeName { get; set; }
        public string RoleTypeDescription { get; set; }
        public int FinancialYearId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsDeliveryDepartment { get; set; }

        public virtual ICollection<GradeRoleType> GradeRoleTypes { get; set; }
    }
}
