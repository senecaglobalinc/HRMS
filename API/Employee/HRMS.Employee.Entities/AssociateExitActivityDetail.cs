using System;

namespace HRMS.Employee.Entities
{
    public class AssociateExitActivityDetail : BaseEntity
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
        public virtual AssociateExitActivity AssociateExitActivity { get; set; }
    }
}
