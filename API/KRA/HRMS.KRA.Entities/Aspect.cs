using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Entities
{
	/// <summary>
    /// Aspect
    /// </summary>
    public class Aspect : BaseEntity
    {
        /// <summary>
        /// AspectId
        /// </summary>
        public int AspectId { get; set; }

        /// <summary>
        /// AspectName
        /// </summary>
        public string AspectName { get; set; }

        /// <summary>
        /// Definitions
        /// </summary>
        public virtual ICollection<Definition> Definitions { get; set; }
    }
}
