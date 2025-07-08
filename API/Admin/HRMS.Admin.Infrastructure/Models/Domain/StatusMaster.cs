using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Infrastructure.Models.Domain
{
    public class StatusMaster
    {
        public int StatusId { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string IsActive { get; set; }
    }
}
