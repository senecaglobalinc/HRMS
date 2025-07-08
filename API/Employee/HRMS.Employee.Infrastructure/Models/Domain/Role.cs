using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Domain
{
    public class Role 
    {
        /// <summary>
        /// RoleId
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// RoleName
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// RoleDescription
        /// </summary>
        public string RoleDescription { get; set; }

        /// <summary>
        /// DepartmentId
        /// </summary>
        public Nullable<int> DepartmentId { get; set; }

        /// <summary>
        /// KeyResponsibilities
        /// </summary>
        public string KeyResponsibilities { get; set; }

        /// <summary>
        /// EducationQualification
        /// </summary>
        public string EducationQualification { get; set; }
    }
}
