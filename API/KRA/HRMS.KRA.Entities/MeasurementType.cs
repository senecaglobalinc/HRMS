using System.Collections.Generic;

namespace HRMS.KRA.Entities
{
    /// <summary>
    /// MeasurementType 
    /// </summary>
    public class MeasurementType : BaseEntity
    {
        /// <summary>
        /// MeasurementTypeId
        /// </summary>
        public int MeasurementTypeId { get; set; }

        /// <summary>
        /// MeasurementTypeName 
        /// </summary>
        public string MeasurementTypeName { get; set; }

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
