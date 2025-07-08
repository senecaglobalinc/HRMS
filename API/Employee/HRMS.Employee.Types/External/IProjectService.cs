using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Employee.Types.External
{
    /// <summary>
    /// Interface for external project api calls
    /// </summary>
    public interface IProjectService
    {
        Task<ServiceListResponse<Project>> GetProjectById(List<int> projectIds);

        Task<ServiceListResponse<AssociateAllocation>> GetAssociateAllocations(List<int> employeeIds);

        ServiceListResponse<AssociateAllocation> GetAssociateAllocationsUsingCache(List<int> employeeIds);

        Task<ServiceListResponse<ProjectManager>> GetProjectManagersByIds(List<int> projectManagerIds);
        Task<ServiceListResponse<ProjectManager>> GetActiveProjectManagers();

        Task<ServiceListResponse<ProjectManager>> GetProjectManagersByEmployeeIds(List<int> employeeIds);

        Task<ServiceListResponse<AllocationPercentage>> GetAllocationPercentage();
        Task<ServiceListResponse<ProjectManager>> GetProjectManagersByEmployeeId(int employeeId);
        Task<ServiceListResponse<AssociateAllocation>> GetAssociateAllocationsByEmployeeId(int employeeId);
        Task<ServiceListResponse<AssociateAllocation>> GetAssociateAllocationsByLeadId(int leadId);
        Task<ServiceResponse<bool>> AllocateAssociateToTalentPool(EmployeeDetails employee);
        Task<ServiceListResponse<int>> GetEmployeeByProjectId(int projectId);
        Task<ServiceListResponse<ProjectResourceData>> GetResourceByProject(int projectId);
        Task<ServiceListResponse<SkillSearchAllocation>> GetSkillSearchAllocations();
        Task<ServiceListResponse<SkillSearchAssociateAllocation>> GetSkillSearchAssociateAllocations(string employeeIds);
        Task<ServiceListResponse<AssociateAllocation>> GetAllAssociateAllocations(bool nightlyJob = false);
        Task<ServiceListResponse<Project>> GetAllProjects(bool nightlyJob = false);
        Task<ServiceResponse<Project>> GetProjectByID(int projectId);
        Task<ServiceResponse<AssociateAllocation>> GetEmployeeProjectIdByEmpId(int empId);
        Task<ServiceResponse<bool>> ReleaseFromTalentPool(TalentPoolDetails tpDetails);
        public Task<ServiceListResponse<EmployeeDetails>> GetEmployeesByEmployeeIdAndRole(int employeeId, string roleName);
        public Task<ServiceListResponse<ProjectManager>> GetProjectLeadData(int employeeId);
        public Task<ServiceListResponse<ProjectManager>> GetProjectRMData(int employeeId);
        public Task<bool> GetProjectManagerFromAllocations(int employeeId);
        public Task<ServiceResponse<bool>> UpdatePracticeAreaOfTalentPoolProject(int employeeId, int CompetenceyAreaId);
        Task<ServiceListResponse<TalentPool>> GetAllTalentPoolData();
        Task<ServiceResponse<ProjectManager>> GetPMByPracticeAreaId(int practiceAreaId);
        Task<ServiceResponse<bool>> ReleaseFromAllocations(int EmployeeId);
        Task<ServiceResponse<bool>> ReleaseOnExit(int employeeId, string releaseDate);
        Task<ServiceListResponse<UtilizationReportFilter>> GetUtilizationReportAllocations(int projectId);
        Task<ServiceListResponse<EmployeeInfo>> GetActiveAllocations();        
        Task<ServiceResponse<AssociateAllocation>> GetAllocationById(int allocationId);        
        Task<ServiceResponse<CompetencyAreaMananagers>> GetCompetencyAreaManagersDetails(int competencyAreaId);        
        Task<ServiceListResponse<AssociateAllocation>> GetAllocationsByEmpIds(string  empIds);        
        Task<ServiceListResponse<ActiveAllocationDetails>> GetAllAllocationDetails();        
        Task<ServiceResponse<ProjectsData>> GetProjectsByEmpIdAndRole(int employeeId);        
    }
}
