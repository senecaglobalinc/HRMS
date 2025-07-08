using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Infrastructure.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace HRMS.Project.Types.External
{
    public interface IEmployeeService
    {
        Task<ServiceListResponse<Employee>> GetEmployeesByIds(List<int> employeeIds);
        Task<ServiceResponse<Employee>> GetEmployeeById(int employeeId);
        Task<ServiceResponse<AssociateExit>> GetResignedAssociateByID(int employeeId);
        Task<ServiceResponse<User>> GetEmployeeDetails(int employeeId);
        Task<ServiceResponse<Employee>> GetEmployeeByUserId(int userId);
        Task<ServiceListResponse<Employee>> GetEmployeeByUserName(string userName);
        Task<ServiceResponse<Employee>> GetActiveEmployeeById(int employeeId);
        Task<BaseServiceResponse> UpdateReportingManagerId(int employeeId, int reportingManagerId);
        Task<ServiceListResponse<Employee>> GetAll(bool? isActive);
        Task<ServiceListResponse<GenericType>> GetAssociatesForDropdown();
        Task<BaseServiceResponse> UpdateEmployee(EmployeeExternal employeeDetails);
        Task<ServiceListResponse<AssociateExit>> GetAllExitAssociates();
    }
}
