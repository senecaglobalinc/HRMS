using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Infrastructure.Models
{
    public class GradeRoleTypeModel
    {
        public int GradeRoleTypeId { get; set; }
        public int FinancialYearId { get; set; }
        public int DepartmentId { get; set; }
        public int RoleTypeId { get; set; }
        public List<int> GradeId { get; set; }
        public int Grade { get; set; }        
        public string DepartmentDescription { get; set; }
        public string GradeIds { get; set; }
        public string GradeName { get; set; }
        public string RoleTypeName { get; set; }
        public string RoleTypeDescription { get; set; }
    }

    public class GradeRoleTypeRequest
    {
        public int GradeRoleTypeId { get; set; }
        public int FinancialYearId { get; set; }
        public int DepartmentId { get; set; }
        public int RoleTypeId { get; set; }        
        public string GradeIds { get; set; }
    }
}
