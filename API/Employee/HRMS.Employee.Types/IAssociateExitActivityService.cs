using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IAssociateExitActivityService
    {
        Task<ServiceResponse<int>> CreateActivityChecklist(int employeeId, int hraId);
        Task<ServiceResponse<int>> UpdateActivityChecklist(ActivityChecklist projectIn);
        Task<ServiceResponse<Activities>> GetDepartmentActivitiesByProjectId(int employeeId, int? departmentId = null);
        Task<ServiceListResponse<Activities>> GetDepartmentActivitiesForHRA(int employeeId);
    }
}
