using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IServiceTypeToEmployeeService
    {
        Task<ServiceResponse<ServiceType>> GetServiceTypeByEmployeeId(int employeeId);
       
        Task<ServiceResponse<int>> Create(ServiceType employeeDetails);
        Task<ServiceResponse<int>> Update(ServiceType employeeDetails);

    }
}
