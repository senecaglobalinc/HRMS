using HRMS.Report.Infrastructure.Models.Domain;
using HRMS.Report.Infrastructure.Models.Request;
using HRMS.Report.Infrastructure.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Report.Types.External
{
    public interface IEmployeeService
    {       
        Task<ServiceListResponse<FinanceReportEmployee>> GetFinanceReportAssociates(FinanceReportEmployeeFilter filter);
        Task<ServiceListResponse<UtilizationReportEmployee>> GetUtilizationReportAssociates(UtilizationReportEmployeeFilter filter, bool isNightJob);
        Task<ServiceListResponse<DomainDataCount>> GetDomainWiseResourceCount();
        Task<ServiceListResponse<DomainReportEmployee>> GetDomainReportAssociates(string employeeIds);
        Task<ServiceListResponse<TalentPoolReportEmployee>> GetTalentPoolReportAssociates(int projectId);
        Task<ServiceListResponse<SkillSearchEmployee>> GetSkillSearchAssociates(AssociateSkillSearchFilter filter);
        Task<ServiceListResponse<GenericType>> GetActiveAssociates(List<int> employeeIds);
        Task<ServiceListResponse<ServiceTypeCount>> GetServiceTypeResourceCount();
        Task<ServiceListResponse<ParkingSlotReport>> GetParkingSlotReport(ParkingSearchFilter parkingSearchFilter);
    }
}
