using System.Collections.Generic;

namespace HRMS.KRA.Entities
{
	/// <summary>
    /// ScaleDetails 
    /// </summary>
    public class ScaleDetails : BaseEntity
    {
        /// <summary>
        /// ScaleDetailId 
        /// </summary>
        public int ScaleDetailId { get; set; }

        /// <summary>
        /// ScaleID 
        /// </summary>
        public int ScaleId { get; set; }

        /// <summary>
        /// ScaleValue 
        /// </summary>
        public int ScaleValue { get; set; }

        /// <summary>
        /// ScaleDescription 
        /// </summary>
        public string ScaleDescription { get; set; }

        /// <summary>
        /// Scale 
        /// </summary>
        public virtual Scale Scale { get; set; }
    }
}
