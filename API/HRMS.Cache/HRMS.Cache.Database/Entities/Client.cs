using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class Client
    {
        public int ClientId { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public string ClientRegisterName { get; set; }
        public string ClientNameHash { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
