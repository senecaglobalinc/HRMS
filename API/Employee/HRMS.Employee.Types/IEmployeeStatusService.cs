using System;
using System.Collections.Generic;
using HRMS.Employee.Entities;
using System.Text;
using System.Threading.Tasks;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Models.Domain;

namespace HRMS.Employee.Types
{
    public interface IEmployeeStatusService
    {
        Task<ServiceResponse<Employee.Entities.Employee>> UpdateEmployeeStatus(EmployeeDetails employee);
        
    }
}
