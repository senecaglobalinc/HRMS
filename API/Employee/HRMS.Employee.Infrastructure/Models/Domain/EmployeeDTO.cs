using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class EmployeeDTO
    {
        public List<GetActiveEmployeesDTO> Employee { get; set; }
    }


    public class GetActiveEmployeesDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
    }
}
