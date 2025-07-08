using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Response;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
    public interface IHRMSExternalService
    {
        Task<ServiceListResponse<GetEmpForExternal>> GetEmployeeProjectDetails();
        Task<ServiceResponse<ProjectDTO>> GetProjectsByEmpIdAndRole(int employeeId);
    }
}
