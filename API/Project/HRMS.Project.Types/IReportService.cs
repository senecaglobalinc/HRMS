using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
    public interface IReportService
    {       
        Task<ServiceListResponse<FinanceReportAllocation>> GetFinanceReportAllocations(FinanceReportFilter filter);
        Task<ServiceListResponse<UtilizationReportAllocation>> GetUtilizationReportAllocations(UtilizationReportFilter filter);
        Task<ServiceListResponse<TalentpoolDataCount>> GetTalentPoolWiseResourceCount(string projectTypeIds);
        Task<ServiceListResponse<int>> GetEmployeeByProjectId(int projectId);
        Task<ServiceListResponse<SkillSearchAllocation>> GetSkillSearchAllocations();
        Task<ServiceListResponse<DomainDataCount>> GetDomainWiseResourceCount();
        Task<ServiceListResponse<ProjectResponse>> GetAllProjects(bool nightlyJob = false);
        Task<ServiceListResponse<ServiceTypeCount>> GetServiceTypeProjectCount();
        Task<ServiceListResponse<ProjectResourceData>> GetResourceByProject(int projectId);
        Task<ServiceListResponse<AssociateInformationReport>> GetCriticalResourceReport();
        Task<ServiceListResponse<AssociateInformationReport>> GetNonCriticalResourceReport();                
        Task<ServiceListResponse<AssociateInformationReports>> GetNonCriticalResourceBillingReport();                
    }
}
