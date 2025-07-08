using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities
{
    public class ServiceType : BaseEntity
    {
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName {get;set;}
        public string ServiceDescription { get; set; }
    }
}
