using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Infrastructure.Models.Request
{
    public class ApplicableRoleTypeRequest
    {
        public int FinancialYearId { get; set; }
        public int DepartmentId { get; set; }
        public int[] GradeRoleTypeId { get; set; }
        public int RoleTypeId { get; set; }
        public int GradeId { get; set; }
        public int ApplicableRoleTypeId { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
    }
}
