using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Request;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface ITransitionPlanService
    {
        Task<ServiceResponse<int>> UpdateTransitionPlan(TransitionDetail projectIn);
        Task<ServiceResponse<TransitionDetail>> GetTransitionPlanByAssociateIdandProjectId(int employeeId, int projectId);
        Task<ServiceListResponse<TransitionDetail>> GetTransitionPlanByAssociateId(int employeeId);

        Task<ServiceResponse<int>> DeleteTransitionActivity(int employeeId, int projectId, int activityId);

    }
}
