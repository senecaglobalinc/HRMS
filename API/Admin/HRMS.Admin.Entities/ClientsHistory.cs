using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities
{
    public class ClientsHistory : BaseEntity
    {
        public int ClientHistoryId { get; set; }
        public int ClientId { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public string ClientRegisterName { get; set; }
    }
}
