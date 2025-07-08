using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Infrastructure.Models.Domain
{
    public class Grade
    {
        /// <summary>
        /// GradeId
        /// </summary>
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
