using HRMS.Employee.Infrastructure.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Associate = HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;

namespace HRMS.Employee.Types
{
    public interface IEmployeePersonalDetailsService
    {
        Task<ServiceResponse<EmployeePersonalDetails>> GetPersonalDetailsById(int empId);
        Task<ServiceResponse<EmployeePersonalDetails>> AddPersonalDetails(EmployeePersonalDetails personalDetails);
        Task<ServiceResponse<EmployeePersonalDetails>> UpdatePersonalDetails(EmployeePersonalDetails personalDetails);
        string ValidatePersonalData(EmployeePersonalDetails personalDetails);
        Task<BaseServiceResponse> UpdateReportingManagerId(int employeeId, int reportingManagerId);
        Task<BaseServiceResponse> UpdateExternal(EmployeeDetails employeeDetails);
        Task<ServiceResponse<EmployeeDetailsDashboard>> GetEmployeeDetailsDashboard(int empId);
    }
}
