using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Infrastructure.Models.Domain
{
    public class RoleType
    {
        /// <summary>
        /// RoleTypeId
        /// </summary>
        public int RoleTypeId { get; set; }

        /// <summary>
        /// RoleTypeName
        /// </summary>
        public string RoleTypeName { get; set; }

        /// <summary>
        /// RoleTypeDescription
        /// </summary>
        public string RoleTypeDescription { get; set; }

        /// <summary>
        /// FinancialYearId
        /// </summary>
        public int FinancialYearId { get; set; }

        /// <summary>
        /// DepartmentId
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// IsDeliveryDepartment
        /// </summary>
        public bool IsDeliveryDepartment { get; set; }
    }
}