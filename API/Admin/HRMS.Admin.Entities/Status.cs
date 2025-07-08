using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities
{
    public class Status : BaseEntity
    {
        /// <summary>
        /// Id-originaly it was not there added for maintaining primary key
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
        public virtual CategoryMaster CategoryMaster { get; set; }
    }
}
