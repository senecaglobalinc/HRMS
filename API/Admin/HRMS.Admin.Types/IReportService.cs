using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IReportService
    {
        Task<ServiceListResponse<ReportDetails>> GetFinanceReportMasters();
        Task<ServiceListResponse<ReportDetails>> GetUtilizationReportMasters();
        Task<ServiceListResponse<ReportDetails>> GetDomainReportMasters();
        Task<ServiceListResponse<ReportDetails>> GetTalentPoolReportMasters();
        Task<ServiceListResponse<ReportDetails>> GetSkillSearchMasters();
        Task<ServiceListResponse<GenericType>> GetSkillsBySearchString(string searchString);
        Task<ServiceListResponse<ReportDetails>> GetAssociateAllocationMasters();
    }
}
