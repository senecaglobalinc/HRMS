using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities
{
    public class FinancialYear : BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// FromYear
        /// </summary>
        public int FromYear { get; set; }

        /// <summary>
        /// ToYear
        /// </summary>
        public int ToYear { get; set; }
    }
}
