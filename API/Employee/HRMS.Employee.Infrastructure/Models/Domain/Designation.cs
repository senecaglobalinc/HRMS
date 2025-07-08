using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Domain
{
    public class Designation 
    {
        /// <summary>
        /// DesignationId
        /// </summary>        
        public int DesignationId { get; set; }

        /// <summary>
        /// DesignationCode
        /// </summary>
        public string DesignationCode { get; set; }

        /// <summary>
        /// DesignationName
        /// </summary>
        public string DesignationName { get; set; }

        /// <summary>
        /// GradeId
        /// </summary>
        public int? GradeId { get; set; }
    }
}
