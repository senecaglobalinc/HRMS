using HRMS.Project.Infrastructure.Models.Response;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
    public interface IReportsService
    {
        Task<ServiceResponse<Entities.AllocationDetails>> GetResourceByProject(int projectId);
        Task<ServiceListResponse<ProjectReportData>> GetProjectDetailsReport();
    }
}
