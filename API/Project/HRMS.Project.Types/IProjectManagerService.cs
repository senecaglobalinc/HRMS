using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
    public interface IProjectManagerService
    {
        Task<ServiceResponse<ProjectManager>> Create(ProjectManager projectIn); 
        Task<ServiceListResponse<ProjectManager>> GetAll(bool? isActive);
        Task<ServiceResponse<ProjectManager>> GetById(int id);
        Task<ServiceListResponse<ProjectManager>> GetByEmployeeId(int employeeId);
        Task<List<ProjectManager>> GetProjectManagerByEmployeeId(string employeeIds);
        Task<ServiceListResponse<ProjectManager>> GetActiveProjectManagers(bool? isActive = true);
        Task<ServiceResponse<ProjectManagersData>> GetReportingDetailsByProjectId(int projectId);
        Task<ServiceListResponse<EmployeeDetails>> GetLeadsManagersBySearchString(string searchString);
        Task<ServiceListResponse<ManagersData>> GetManagerandLeadByProjectIdandEmpId(int projectId, int employeeId);
        Task<ServiceResponse<bool>> UpdateReportingManagerToAssociate(ProjectRequest projectData, bool isDelivery);
        Task<BaseServiceResponse> SaveManagersToProject(ProjectManagersData projectManagerIn);
        Task<ServiceListResponse<GenericType>> GetProgramManagersForDropdown();
        Task<ServiceListResponse<GenericType>> GetLeadsManagersForDropdown();
        Task<ServiceListResponse<ProjectManagersData>> GetProjectLeadData(int employeeID);
        Task<ServiceListResponse<ProjectManagersData>> GetProjectRMData(int employeeID);
        Task<bool> GetProjectManagerFromAllocations(int employeeID);
        Task<ProjectManager> GetPMByPracticeAreaId(int practiceAreaId);
    }
}
