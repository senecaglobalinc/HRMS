using HRMS.Employee.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Domain
{
    public class UpdateActivityChecklist : BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int AssociateExitActivityDetailId { get; set; }

        /// <summary>
        /// AssociateExitId
        /// </summary>
        public int AssociateExitActivityId { get; set; }

        /// <summary>
        /// ActivityId
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// ActivityValue
        /// </summary>
        public string ActivityValue { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }
    }
}
