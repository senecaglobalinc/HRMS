using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Request;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IReportService
    {
        Task<ServiceListResponse<FinanceReportEmployee>> GetFinanceReportAssociates(FinanceReportEmployeeFilter filter);
        Task<ServiceListResponse<UtilizationReportEmployee>> GetUtilizationReportAssociates(UtilizationReportEmployeeFilter filter);
        Task<ServiceListResponse<DomainDataCount>> GetDomainWiseResourceCount();
        Task<ServiceListResponse<DomainReportEmployee>> GetDomainReportAssociates(string employeeIds);
        Task<ServiceListResponse<TalentPoolReportEmployee>> GetTalentPoolReportAssociates(int projectId);       
        Task<ServiceListResponse<SkillSearchEmployee>> GetSkillSearchAssociates(SkillSearchFilter filter);
        Task<ServiceListResponse<GenericType>> GetActiveAssociates(List<int> employeeIds);
        Task<ServiceListResponse<ServiceTypeCount>> GetServiceTypeResourceCount();
        Task<ServiceListResponse<UtilizationReport>> GetUtilizationReport();
        Task<ServiceListResponse<UtilizationReport>> GetResourceUtilization(int? year = null, bool nightlyJob = false);
        Task<ServiceListResponse<AssociateExitReport>> GetAssociateExitReport();
        Task<ServiceListResponse<AssociateExitReport>> GetAssociateExitData(bool nightlyJob = false);
        Task<ServiceListResponse<GenericType>> GetAssociateExitReportTypes();
        Task<ServiceListResponse<ChartData>> GetAssociateExitChartReport(AssociateExitReportFilter filter);
        Task<ServiceListResponse<AssociateExitReport>> GetAssociateExitReport(AssociateExitReportFilter filter);
        Task<ServiceListResponse<ParkingSlotReport>> GetParkingSlotReport(ParkingSearchFilter filter);
    }
}
