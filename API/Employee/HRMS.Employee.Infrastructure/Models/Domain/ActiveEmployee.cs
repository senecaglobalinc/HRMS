using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ActiveEmployee
    {
        public int EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Code { get; set; }
        public string Gender { get; set; }
    }
}
