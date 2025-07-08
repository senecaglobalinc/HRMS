using System.Collections.Generic;

namespace HRMS.KRA.Entities
{
    /// <summary>
    /// TargetPeriod 
    /// </summary>
    public class TargetPeriod : BaseEntity
    {
        /// <summary>
        /// TargetPeriodId 
        /// </summary>
        public int TargetPeriodId { get; set; }

        /// <summary>
        /// TargetPeriodValue 
        /// </summary>
        public string TargetPeriodValue { get; set; }

        /// <summary>
        /// DefinitionDetails 
        /// </summary>
        public virtual ICollection<DefinitionDetails> DefinitionDetails { get; set; }

        /// <summary>
        /// DefinitionTransactions 
        /// </summary>
        public virtual ICollection<DefinitionTransaction> DefinitionTransactions { get; set; }
    }
}
