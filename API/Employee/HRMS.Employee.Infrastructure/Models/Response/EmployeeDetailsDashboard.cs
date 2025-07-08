using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Response
{
    public class EmployeeDetailsDashboard
    {
        public EmployeePersonalDetails EmployeePersonalDetails { get; set; }
        public IList<AssociateAllocation> EmployeeAllocationDetails { get; set; }
        public IList<EmployeeSkillDetails> EmployeeSkillDetails { get; set; }
        public EmployeePersonalDetails EmployeeFileDetails { get; set; }
    }
}
