using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IAssociateExitInterviewService
    {
        Task<ServiceResponse<int>> CreateExitFeedback(ExitInterviewRequest interviewRequest);
        Task<ServiceResponse<ExitInterviewRequest>> GetExitInterview(int employeeId);
    }
}
