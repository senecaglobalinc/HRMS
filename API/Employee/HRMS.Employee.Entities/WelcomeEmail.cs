using System;
using System.Collections.Generic;

namespace HRMS.Employee.Entities
{
    public class WelcomeEmail : BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// IsWelcome
        /// </summary>
        public bool IsWelcome { get; set; }


    }
}
