using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{

        public class DepartmentWithDLAddress
        {
            public int DepartmentId { get; set; }
            public string DepartmentCode { get; set; }
            public string DepartmentDescription { get; set; }
            public int? DepartmentHeadId { get; set; }
            public string DepartmentDLAddress { get; set; }
        
    }
}
