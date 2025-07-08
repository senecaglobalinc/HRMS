using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities
{
    public class TransitionPlanDetails
    {
        public int ActivityTypeId { get; set; }
        public string ActivityType { get; set; }
        public List<TransitionDetail> TransitionActivityDetails { get; set; }
    }

    public class TransitionDetail
    {
        public int ActivityId { get; set; }
        public string Description { get; set; }
    }
}
