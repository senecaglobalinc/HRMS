using System.Collections.Generic;

namespace HRMS.KRA.Entities
{
    /// <summary>
    /// Operator 
    /// </summary>
    public class Operator : BaseEntity
    {
        /// <summary>
        /// OperatorId 
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// OperatorValue 
        /// </summary>
        public string OperatorValue { get; set; }

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
