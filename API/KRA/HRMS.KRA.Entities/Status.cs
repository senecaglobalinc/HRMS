using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Entities
{
    /// <summary>
    /// Status
    /// </summary>
    public class Status : BaseEntity
    {
        /// <summary>
        /// StatusId
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// StatusText
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// StatusDescription
        /// </summary>
        public string StatusDescription { get; set; }

        
    }
}
