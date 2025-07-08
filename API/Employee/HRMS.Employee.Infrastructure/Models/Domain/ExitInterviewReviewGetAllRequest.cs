using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ExitInterviewReviewGetAllRequest
    {
        /// <summary>
        /// FromDate
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// ToDate
        /// </summary>
        public DateTime ToDate { get; set; }
    }
}
