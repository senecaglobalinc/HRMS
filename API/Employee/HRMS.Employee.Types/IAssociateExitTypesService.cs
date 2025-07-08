using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IAssociateExitTypesService
    {
        Task<ServiceListResponse<EmployeeDetails>> GetEmployeesByEmpIdAndRole(int employeeId, string roleName);
        Task<ServiceListResponse<AssociateExit>> GetByExitType(int exitTypeId);
    }
}
