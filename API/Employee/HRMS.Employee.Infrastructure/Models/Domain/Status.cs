using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Domain
{
    public class Status 
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// StatusId
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// StatusCode
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// StatusDescription
        /// </summary>
        public string StatusDescription { get; set; }

        /// <summary>
        /// CategoryMasterId
        /// </summary>
        public int? CategoryMasterId { get; set; }
        /// <summary>
        /// CategoryMaster
        /// </summary>
        //public virtual CategoryMaster CategoryMaster { get; set; }
    }
}
