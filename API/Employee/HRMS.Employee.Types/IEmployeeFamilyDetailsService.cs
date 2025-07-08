using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IEmployeeFamilyDetailsService
    {
        Task<ServiceResponse<EmployeePersonalDetails>> GetFamilyDetailsById(int empId);
        Task<ServiceResponse<EmployeePersonalDetails>> UpdateFamilyDetails(EmployeePersonalDetails details);
    }
}
