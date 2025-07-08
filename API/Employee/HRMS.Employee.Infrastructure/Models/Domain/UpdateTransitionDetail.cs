using HRMS.Employee.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class UpdateTransitionDetail : BaseEntity
    {
        /// <summary>
        /// TransitionPlanDetailId
        /// </summary>
        public int TransitionPlanDetailId { get; set; }
        /// <summary>
        /// TransitionPlanId
        /// </summary>
        public int TransitionPlanId { get; set; }
        /// <summary>
        /// ActivityId
        /// </summary>
        public int ActivityId { get; set; }
        /// <summary>
        /// StartDate
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// EndDatae
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// StatusId
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// ActivityDescription
        /// </summary>
        public string ActivityDescription { get; set; }

    }
}
