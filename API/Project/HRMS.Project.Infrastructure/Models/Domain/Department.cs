using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Description { get; set; }
        public string DepartmentCode { get; set; }
        public int? DepartmentHeadId { get; set; }
        public int DepartmentTypeId { get; set; }
    }
}
