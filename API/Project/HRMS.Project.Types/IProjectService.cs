using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
    public interface IProjectService
    {
        Task<ServiceResponse<int>> Create(ProjectRequest projectIn);
        Task<ServiceListResponse<Entities.Project>> GetAll();
        //Task<List<Entities.Project>> GetProjectDetails(string userRole, int employeeId, string dashboard);
        Task<ServiceResponse<Entities.Project>> GetById(int id);
        //Task<Entities.Project> GetByProjectId(int id);
        Task<ServiceResponse<Entities.Project>> GetByProjectCode(string projectCode);
        Task<ServiceResponse<int>> Update(ProjectRequest projectIn);
        
        Task<ServiceResponse<int>> HasActiveClientBillingRoles(int projectId);
        Task<ServiceListResponse<ProjectResponse>> GetProjectsList(string userRole, int employeeId, string dashboard);
        Task<ServiceResponse<ProjectResponse>> GetProjectById(int projectId);
        Task<ServiceResponse<int>> SubmitForApproval(int projectId, string userRole, int employeeId);
        Task<ServiceResponse<int>> ApproveOrRejectByDH(int projectId, string status, int employeeId);
        Task<ServiceResponse<bool>> DeleteProjectDetails(int projectId);
        Task<ServiceListResponse<ProjectResponse>> GetProjectsForAllocation();
        Task<ServiceResponse<ProjectResponse>> GetEmpTalentPool(int employeeId);
        Task<ServiceListResponse<ProjectResponse>> GetEmpTalentPool(int employeeId, int projectId, string roleName);
        Task<ServiceListResponse<ProjectDetails>> GetProjectsByEmpId(int employeeId);
        Task<ServiceListResponse<GenericType>> GetProjectsForDropdown();
        Task<ServiceListResponse<Entities.Project>> GetProjectsByIds(string projectIds);
        Task<ServiceListResponse<ProjectDetails>> GetAssociateProjectsForRelease(int employeeId);
    }
}
