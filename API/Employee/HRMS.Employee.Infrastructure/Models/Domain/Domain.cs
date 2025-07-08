using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Domain
{
    public class Domain 
    {
        public int DomainId { get; set; }

        /// <summary>
        /// DomainCode
        /// </summary>
        public string DomainCode { get; set; }

        /// <summary>
        /// DomainName
        /// </summary>
        public string DomainName { get; set; }

    }
}
