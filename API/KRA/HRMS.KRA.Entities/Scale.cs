using System.Collections.Generic;

namespace HRMS.KRA.Entities
{
    /// <summary>
    /// Scale
    /// </summary>
    public class Scale : BaseEntity
    {
        /// <summary>
        /// ScaleId
        /// </summary>
        public int ScaleId { get; set; }

        /// <summary>
        /// MinimumScale 
        /// </summary>
        public int MinimumScale { get; set; }

        /// <summary>
        /// MaximumScale 
        /// </summary>
        public int MaximumScale { get; set; }

        /// <summary>
        /// ScaleTitle 
        /// </summary>
        public string ScaleTitle { get; set; }

        /// <summary>
        /// ScaleDetails 
        /// </summary>
        public virtual List<ScaleDetails> ScaleDetails { get; set; }

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
