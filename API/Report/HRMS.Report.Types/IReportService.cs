using HRMS.Report.Infrastructure.Models.Domain;
using HRMS.Report.Infrastructure.Models.Request;
using HRMS.Report.Infrastructure.Models.Response;
using System.Threading.Tasks;

namespace HRMS.Report.Types
{
    public interface IReportService
    {        
        Task<ServiceListResponse<FinanceReport>> GetFinanceReport(FinanceReportFilter filter);
        Task<ServiceListResponse<UtilizationReport>> GetUtilizationReport(UtilizationReportFilter filter,bool isNightJob=false);
        Task<ServiceListResponse<DomainReportCount>> GetDomainReportCount();
        Task<ServiceListResponse<TalentPoolReportCount>> GetTalentPoolReportCount();
        Task<ServiceListResponse<DomainReport>> GetDomainReport(int domainId);
        Task<ServiceListResponse<TalentPoolReport>> GetTalentPoolReport(int projectId);
        Task<ServiceListResponse<AssociateSkillSearch>> GetEmployeeDetailsBySkill(AssociateSkillSearchFilter filter);
        Task<ServiceListResponse<ServiceTypeReportCount>> GetServiceTypeReport(string filter);
        Task<ServiceListResponse<ProjectRespose>> GetServiceTypeReportProject(int serviceTypeId);
        Task<ServiceListResponse<UtilizationReport>> GetResourceReportByFilter(bool IsNightJob);
      Task<ServiceListResponse<AssociateInformationReport>> GetCriticalResourceReport();
       Task<ServiceListResponse<AssociateInformationReport>> GetNonCriticalResourceReport();
       Task<ServiceListResponse<UtilizationReport>> GetTalentPoolResourceReport(bool IsNightJob);
       Task<ServiceListResponse<AssociateInformationReport>> GetAssociatesForFutureAllocation();
       Task<ServiceListResponse<AssociateInformationReport>> GetNonCriticalResourceBillingReport();
        Task<ServiceListResponse<ParkingSlotReport>> GetParkingSloteport(ParkingSearchFilter filter);
    }
}
