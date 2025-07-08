using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Request
{
    public class WelcomeEmailCertRequest 
    {
        public int EmployeeId { get; set; }              
        public string Specialization { get; set; }
        public string CertificationName { get; set; }
        public string Institution { get; set; }
    }
}
