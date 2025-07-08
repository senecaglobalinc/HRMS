using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IMapAssociateIdService
    {
        //Map Associate Id method
        Task<ServiceResponse<Entities.Employee>> MapAssociateId(EmployeeDetails employee);

        //Get UnMapped Users method
        Task<ServiceListResponse<User>> GetUnMappedUsers();
    }
}
