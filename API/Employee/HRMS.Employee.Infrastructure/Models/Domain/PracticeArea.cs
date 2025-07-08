using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Domain
{
    public class PracticeArea 
    {
        /// <summary>
        /// PracticeAreaId
        /// </summary>
        public int PracticeAreaId { get; set; }

        /// <summary>
        /// PracticeAreaCode
        /// </summary>
        public string PracticeAreaCode { get; set; }

        /// <summary>
        /// PracticeAreaDescription
        /// </summary>
        public string PracticeAreaDescription { get; set; }
    }
}
