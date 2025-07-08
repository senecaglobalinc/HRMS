using HRMS.Report.Infrastructure.Models.Domain;
using HRMS.Report.Infrastructure.Models.Response;
using System.Threading.Tasks;

namespace HRMS.Report.Types.External
{
    public interface IOrganizationService
    {
        Task<ServiceListResponse<ReportDetails>> GetFinanceReportMasters();
        Task<ServiceListResponse<ReportDetails>> GetUtilizationReportMasters(bool isNightJob);
        Task<ServiceListResponse<ReportDetails>> GetDomainReportMasters();
        Task<ServiceListResponse<ReportDetails>> GetTalentPoolReportMasters();
        Task<ServiceListResponse<ReportDetails>> GetSkillSearchMasters();
        Task<ServiceListResponse<ServiceType>> GetServiceTypetMasters();
        Task<ServiceResponse<ServiceType>> GetProjectTypeByCode(string projectTypeCode);
    }
}
