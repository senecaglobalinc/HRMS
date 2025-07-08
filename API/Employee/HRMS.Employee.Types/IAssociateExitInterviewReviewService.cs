using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IAssociateExitInterviewReviewService
    {
        Task<ServiceListResponse<ExitInterviewReviewGetAllResponse>> GetAll(ExitInterviewReviewGetAllRequest exitInterviewReviewRequest);
        Task<ServiceResponse<int>> Create(ExitInterviewReviewCreateRequest exitInterviewReviewRequest);
    }
}
