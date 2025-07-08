using HRMS.Report.Infrastructure.Models.Domain;
using HRMS.Report.Infrastructure.Models.Request;
using HRMS.Report.Infrastructure.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Report.Types.External
{
    public interface IProjectService
    {        
        Task<ServiceListResponse<FinanceReportAllocation>> GetFinanceReportAllocations(FinanceReportFilter filter);
        Task<ServiceListResponse<UtilizationReportAllocation>> GetUtilizationReportAllocations(UtilizationReportAllocationFilter filter, bool isNightJob);
        Task<ServiceListResponse<DomainDataCount>> GetDomainWiseResourceCount();
        Task<ServiceListResponse<TalentPoolReportCount>> GetTalentPoolWiseResourceCount(List<int> projectTypeIds);
        Task<ServiceListResponse<ServiceTypeCount>> GetServiceTypeResourceCount();
        Task<ServiceListResponse<ProjectRespose>> GetAllProjects();
        Task<ServiceListResponse<AssociateInformationReport>> GetCriticalResourceReport();
        Task<ServiceListResponse<AssociateInformationReport>> GetNonCriticalResourceReport();
        Task<ServiceListResponse<AssociateInformationReport>> GetNonCriticalResourceBillingReport();
    }
}
