using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Infrastructure.Models.Domain;
using HRMS.KRA.Infrastructure.Models.Response;
using HRMS.KRA.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.KRA.Types.External
{
    public interface IEmployeeService
    {
        Task<ServiceListResponse<EmployeeDetails>> GetEmployeeNames();
        Task<ServiceResponse<string>> GetEmployeeWorkEmailAddress(int employeeId); 
        Task<ServiceListResponse<AssociateRoleType>> GetEmployeesByRole(string employeeCode, int? departmentId, int? roleId);
    }
}
