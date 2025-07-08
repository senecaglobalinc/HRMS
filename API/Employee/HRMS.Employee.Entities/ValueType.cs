using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class ValueType : BaseEntity
    {
        /// <summary>
        /// ValueTypeKey
        /// </summary>
        public int ValueTypeKey { get; set; }

        /// <summary>
        /// ValueTypeId
        /// </summary>
        public string ValueTypeId { get; set; }

        /// <summary>
        /// ValueTypeName
        /// </summary>
        public string ValueTypeName { get; set; }

        /// <summary>
        /// lkValue
        /// </summary>
        public virtual ICollection<lkValue> lkValue { get; set; }
    }
}
