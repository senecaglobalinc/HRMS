using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IEmployeeProjectService
    {
        Task<ServiceListResponse<EmployeeProject>> GetByEmployeeId(int employeeId);
        Task<ServiceResponse<EmployeeProject>> Create(EmployeeDetails employeeDetails);
    }
}
