using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
    public interface IProjectClosureService
    {
        
        Task<ServiceResponse<int>> SubmitForClosureApproval(SubmitForClosureApprovalRequest submitForClosureApproval);
        Task<ServiceResponse<int>> ApproveOrRejectClosureByDH(ApproveOrRejectClosureRequest approveOrRejectClosureRequest);
        Task<ServiceResponse<int>> RejectClosure(RejectClosureReport rejectClosureReport);
        Task<ServiceResponse<int>> NotifyAll(int projectId);
        Task<ServiceResponse<int>> ProjectClosureInitiation(ProjectClosureInitiationResponse projectData);
        Task<ServiceResponse<int>> ApproveOrRejectProjectClosureByDH(int projectId, int employeeId);
    }
}
