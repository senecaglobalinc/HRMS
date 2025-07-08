using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Infrastructure.Models
{
    public class ApplicableRoleTypeModel
    {        
        public int ApplicableRoleTypeId { get; set; }
        public int FinancialYearId { get; set; }
        public int DepartmentId { get; set; }
        //public int[]  GradeRoleTypeId { get; set; }    
        public int GradeRoleTypeId { get; set; }
        public int StatusId { get; set; }
        public int RoleTypeId { get; set; }
        public int GradeId { get; set; }
        //
        public string DepartmentDescription { get; set; }
        public string FinancialYearName { get; set; }      
        public string Grade { get; set; }
        public string GradeName { get; set; }
        public string RoleTypeName { get; set; }
        public string RoleTypeDescription { get; set; }

    }
}
