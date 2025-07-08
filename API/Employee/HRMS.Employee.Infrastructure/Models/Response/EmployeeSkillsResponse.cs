using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Response
{
    /// <summary>
    /// Response class for EmployeeSkills
    /// </summary>
    public class EmployeeSkillsResponse : BaseServiceResponse
    {
        /// <summary>
        /// EmployeeSkills
        /// </summary>
        public EmployeeSkills EmployeeSkills
        {
            get;
            set;
        }
    }
}
