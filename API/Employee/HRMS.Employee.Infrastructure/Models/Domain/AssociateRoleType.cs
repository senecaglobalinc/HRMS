using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class AssociateRoleType
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeEmail { get; set; }
        public int EmployeeRoleId { get; set; }
        public string EmployeeRole { get; set; }
        public int DepartmentId { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentHeadId { get; set; }        
        public string DepartmentHeadName { get; set; }
    }  
}
