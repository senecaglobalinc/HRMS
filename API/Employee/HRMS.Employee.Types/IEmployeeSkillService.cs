using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IEmployeeSkillService
    {
        Task<ServiceResponse<EmployeeSkill>> Create(EmployeeSkillDetails employeeSkill);
        Task<ServiceListResponse<EmployeeSkillDetails>> GetByEmployeeId(int employeeId,string roleName=null);
        Task<ServiceResponse<EmployeeSkill>> Update(EmployeeSkillDetails employeeSkill);
        Task<ServiceResponse<bool>> DeleteSkill(int id);
    }
}
