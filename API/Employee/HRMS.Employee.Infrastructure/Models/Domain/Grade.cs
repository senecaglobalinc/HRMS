using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Domain
{
    public class Grade 
    {
        public int GradeId { get; set; }

        /// <summary>
        /// GradeCode
        /// </summary>
        public string GradeCode { get; set; }

        /// <summary>
        /// GradeName
        /// </summary>
        public string GradeName { get; set; }

    }
}
