using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Cache.Database.Entities
{
    public class Status
    {
     
        public int Id { get; set; }

        public int StatusId { get; set; }

        public string StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public int? CategoryMasterId { get; set; }

        public virtual Categories CategoryMaster { get; set; }
    }
}
