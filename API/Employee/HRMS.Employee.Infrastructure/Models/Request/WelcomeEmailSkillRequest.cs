using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Request
{
    public class WelcomeEmailSkillRequest 
    {
        public int EmployeeId { get; set; }
        public string Skillname { get; set; }         

    }
}
