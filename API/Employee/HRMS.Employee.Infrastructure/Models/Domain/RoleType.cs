namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class RoleType
        {
            public int RoleTypeId { get; set; }
            public string RoleTypeName { get; set; }
            public string RoleTypeDescription { get; set; }
            public string Grade { get; set; }
            public int GradeId { get; set; }
            public string Department { get; set; }
            public int DepartmentId { get; set; }
            public int GradeRoleTypeId { get; set; }
            public bool? IsActive { get; set; }
        }    
}
