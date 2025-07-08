using System.Collections.Generic;

namespace HRMS.Admin.Entities
{
    public class Client : BaseEntity
    {
        /// <summary>
        /// ClientId
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// ClientCode
        /// </summary>
        public string ClientCode { get; set; }

        /// <summary>
        /// ClientName
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// ClientRegisterName
        /// </summary>
        public string ClientRegisterName { get; set; }

        /// <summary>
        /// ClientNameHash
        /// </summary>
        public string ClientNameHash { get; set; }
    }
}
