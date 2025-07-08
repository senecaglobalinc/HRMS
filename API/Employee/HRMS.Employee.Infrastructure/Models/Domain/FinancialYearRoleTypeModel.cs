using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class FinancialYearRoleTypeModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public int FinancialYearId { get; set; }
        public string FinancialYearName { get; set; }
        public int RoleTypeId { get; set; }
        public string RoleTypeName { get; set; }
        public bool DownloadKRA { get; set; }
    }
}
