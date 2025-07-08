using HRMS.Admin.Entities;
using HRMS.Admin.Entities.Models;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types.External
{
    public interface IEmployeeService
    {
        Task<ServiceListResponse<EmployeeDetails>> GetAll(bool? isActive);
        Task<ServiceListResponse<EmployeeDetails>> GetEmployeeBySearchString(string text);
        Task<ServiceResponse<Employee>> GetEmployeeByUserId(int userid);
        Task<ServiceListResponse<Employee>> GetEmployeesByIds(List<int?> employeeIds);
        
    }
}
