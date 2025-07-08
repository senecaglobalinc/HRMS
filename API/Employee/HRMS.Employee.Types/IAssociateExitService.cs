using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Request;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IAssociateExitService
    {
        Task<ServiceListResponse<AssociateExit>> GetAll(bool? isActive = true);
        Task<ServiceResponse<AssociateExitRequest>> GetByEmployeeId(int employeeId, bool isDecryptReq = false);
        Task<ServiceResponse<AssociateExit>> Create(AssociateExitRequest associateExitIn);
        Task<BaseServiceResponse> ReviewByPM(AssociateExitPMRequest associateExitPMIn);
        Task<ServiceResponse<AssociateExit>> Approve(AssociateExitRequest associateExitIn);
        Task<ServiceListResponse<ExitDashboardResponse>> GetAssociatesForExitDashboard(string userRole, int employeeId, string dashboard, int departmentId);
        Task<ServiceResponse<int>> RevokeExit(RevokeRequest revokeRequest);
        Task<ServiceResponse<int>> ExitClearance(ClearanceRequest clearanceRequest);
        Task<ServiceResponse<int>> AssociateExitSendNotification(int employeeId, int notificationType, int? departmentId, int? projectId, [Optional] string resignationRecommendation, [Optional] string exitFeedback);
        Task<ServiceResponse<int>> RequestForWithdrawResignation(int employeeId, string resignationRecommendation);
        Task<ServiceResponse<int>> ApproveOrRejectRevoke(RevokeRequest revokeRequest);
        Task<ServiceResponse<ClearanceRequest>> GetClearanceRemarks(int employeeId);
        Task<ServiceResponse<bool>> AssociateExitDailyNotification(int departmentId);
        Task<ServiceResponse<AssociateExitWFStatus>> AssociateExitWFStatus(int employeeId);
        Task<ServiceResponse<bool>> AssociateClearanceStatus(int employeeId);
        Task<ServiceResponse<AssociateExitDetails>> GetResignedAssociateByID(int EmployeeId);
        Task<ServiceResponse<bool>> ReviewReminderNotification(int employeeId);
    }
}
