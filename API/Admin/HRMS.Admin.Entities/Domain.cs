using System.Collections.Generic;

namespace HRMS.Admin.Entities
{
    public class Domain : BaseEntity
    {
        /// <summary>
        /// DomainID
        /// </summary>
        public int DomainID { get; set; }

        /// <summary>
        /// DomainName
        /// </summary>
        public string DomainName { get; set; }
    }
}
