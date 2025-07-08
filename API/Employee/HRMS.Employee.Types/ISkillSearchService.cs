using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface ISkillSearchService
    {
        Task<ServiceListResponse<Employee.Entities.SkillSearch>> GetAll();
        Task<ServiceResponse<Employee.Entities.SkillSearch>> GetById(int id);
        Task<ServiceListResponse<EmployeeSkillSearch>> GetAllSkillDetails(int empID);
        Task<ServiceResponse<dynamic>> BulkInsert();
    }
}
