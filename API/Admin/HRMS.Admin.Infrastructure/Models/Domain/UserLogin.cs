using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Infrastructure.Models.Domain
{
    public class UserLogin
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string username { get; set; }
        public string roles { get; set; }
        public int? EmployeeDepartmentId { get; set; }
        public bool allowedWfoInHrms { get; set; }
    }
}
