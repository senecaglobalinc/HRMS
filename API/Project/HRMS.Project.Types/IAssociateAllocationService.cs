using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
    public interface IAssociateAllocationService
    {
        Task<ServiceListResponse<AssociateAllocation>> GetAll();
        Task<ServiceResponse<AssociateAllocation>> GetById(int id);
        Task<ServiceListResponse<AssociateAllocation>> GetByProjectId(int projectId);
        Task<ServiceListResponse<AssociateAllocation>> GetByClientBillingRoleId(int clientBillingRoleId);
        Task<ServiceListResponse<ResourceAllocation>> GetBillableResouceInfoByProjectId(int projectId);
        Task<ServiceListResponse<AssociateAllocation>> GetByEmployeeId(int employeeId);
        Task<ServiceListResponse<AssociateAllocation>> GetByLeadId(int leadId);
        Task<List<AssociateAllocation>> GetAllocationsByEmpIds(string employeeIds);
        Task<ServiceResponse<bool>> AllocateAssociateToTalentPool(EmployeeDetails employee);
        Task<ServiceListResponse<EmployeeDetails>> GetEmployeesForAllocations();
        Task<ServiceListResponse<AssociateAllocationDetails>> GetEmpAllocationHistory(int employeeId);
        Task<ServiceResponse<int>> Create(AssociateAllocationDetails allocationIn);
        Task<ServiceResponse<AssociateAllocationDetails>> GetEmployeePrimaryAllocationProject(int employeeId);
        Task<ServiceListResponse<Associate>> GetAssociatesToRelease(int employeeId, string roleName);
        Task<BaseServiceResponse> TemporaryReleaseAssociate(AssociateAllocationDetails associateDetails);
        Task<ServiceListResponse<EmployeeDetails>> GetAllocatedAssociates();
        Task<BaseServiceResponse> ReleaseOnExit(int employeeId, string releaseDate);
        Task<ServiceListResponse<SkillSearchAssociateAllocation>> GetSkillSearchAllocations(string employeeIds);
        Task<ServiceResponse<int>> UpdateAssociateAllocation(AssociateAllocationDetails allocationIn);
        Task<ServiceResponse<AssociateAllocationDetails>> GetCurrentAllocationByEmpIdAndProjectId(int employeeId, int projectId);
        Task<ServiceResponse<bool>> ReleaseFromTalentPool(TalentPoolDetails tpDetails);
        Task<ServiceResponse<bool>> UpdatePracticeAreaOfTalentPoolProject(int EmpID, int competencyAreaId);
        Task<ServiceResponse<bool>> ReleaseFromAllocations(int EmployeeId);
        Task<ServiceResponse<bool>> AddAssociateFutureProject(AssociateFutureProjectAllocationDetails associateFutureProject);
        Task<ServiceListResponse<AssociateFutureProjectAllocationDetails>> GetAssociateFutureProjectByEmpId(int employeeId);
        Task<ServiceListResponse<bool>> DiactivateAssociateFutureProjectByEmpId(int employeeId);
        Task<ServiceListResponse<AssociateAllocationData>> GetAssociatesForAllocation();       
        Task<ServiceListResponse<EmployeeInfo>> GetActiveAllocations();
        Task<ServiceListResponse<AssociateAllocationHistory>> GetAllAllocationByEmployeeId(int employeeId);
        Task<ServiceResponse<CompetencyAreaMananagers>> GetCompetencyAreaManagersDetails(int competencyAreaID);
        Task<ServiceListResponse<ActiveAllocationDetails>> GetAllAllocationDetails();
    }
}
