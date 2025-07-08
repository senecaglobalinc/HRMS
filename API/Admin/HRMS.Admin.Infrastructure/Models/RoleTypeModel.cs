namespace HRMS.Admin.Infrastructure.Models
{
    public class RoleTypeModel
    {
        public int RoleTypeId { get; set; }
        public string RoleTypeName { get; set; }
        public string RoleTypeDescription { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public int FinancialYearId { get; set; }
        public string FinancialYearName { get; set; }
        public bool IsDeliveryDepartment { get; set; }
        public bool? IsActive { get; set; }
    }
}
