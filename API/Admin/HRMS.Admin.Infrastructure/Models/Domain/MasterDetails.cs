namespace HRMS.Admin.Infrastructure.Models.Domain
{
    public class MasterDetails
    {
        public int RecordType { get; set; }
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }      
        public int RoleTypeId { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public string GradeCode { get; set; }
        public int GradeRoleTypeId { get; set; }
        public string FinancialYearName { get; set; }
        public string RoleTypeName { get; set; }
        public string RoleTypeDescription { get; set; }

    }

}
