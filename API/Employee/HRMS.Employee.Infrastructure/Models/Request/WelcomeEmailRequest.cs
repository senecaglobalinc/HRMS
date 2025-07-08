using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Request
{
    public class WelcomeEmailRequest 
    {
        public string FormMailContent { get; set; }
        public string FormMailSubject { get; set; }
        public string FormMailCC { get; set; }
        public int FormMailEmpId { get; set; }

    }
}
