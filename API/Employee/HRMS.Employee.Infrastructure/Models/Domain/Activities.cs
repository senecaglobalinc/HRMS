using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class Activities
    {

        public int EmployeeId { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public string Remarks { get; set; }

        public int StatusId { get; set; }

        public string StatusCode { get; set; }
        public List<GetActivityChecklist> ActivityDetails { get; set; }

    }
}
