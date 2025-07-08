using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class lkValue : BaseEntity
    {
        /// <summary>
        /// ValueKey
        /// </summary>
        public int ValueKey { get; set; }

        /// <summary>
        /// ValueId
        /// </summary>
        public string ValueId { get; set; }

        /// <summary>
        /// ValueName
        /// </summary>
        public string ValueName { get; set; }

        /// <summary>
        /// ValueTypeKey
        /// </summary>
        public int ValueTypeKey { get; set; }

        /// <summary>
        /// ValueType
        /// </summary>
        public virtual ValueType ValueType { get; set; }
    }
}
