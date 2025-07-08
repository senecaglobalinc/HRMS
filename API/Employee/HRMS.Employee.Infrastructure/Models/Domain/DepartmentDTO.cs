using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class DepartmentDTO
    {
        public List<GetDepartmentsDTO> Departments { get; set; }
    }

    public class GetDepartmentsDTO
    {
        public int DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string Description { get; set; }
    }
}
