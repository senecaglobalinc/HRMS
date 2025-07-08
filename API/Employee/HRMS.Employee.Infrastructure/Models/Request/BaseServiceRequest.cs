using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Request
{
    /// <summary>
    /// BaseServiceRequest class
    /// </summary>
    public class BaseServiceRequest
    {
        public string CurrentUserName { get; set; }
    }
}
