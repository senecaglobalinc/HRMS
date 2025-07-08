using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IAssociateAbscondService
    {
        Task<ServiceListResponse<AssociateModel>> GetAssociateByLead(int leadId, int deptId);
        Task<ServiceListResponse<AbscondDashboardResponse>> GetAssociatesAbscondDashboard(string userRole, int employeeId, int departmentId);
        Task<ServiceResponse<AssociateAbscondModel>> GetAbscondDetailByAssociateId(int associateId);
        Task<ServiceResponse<bool>> CreateAbscond(AssociateAbscondModel abscondReq);
        Task<ServiceResponse<bool>> AcknowledgeAbscond(AssociateAbscondModel abscondReq);
        Task<ServiceResponse<bool>> ConfirmAbscond(AssociateAbscondModel abscondReq);
        Task<ServiceResponse<bool>> AbscondClearance(AssociateAbscondModel abscondReq);
        Task<ServiceResponse<AssociateAbscondWFStatus>> AssociateAbscondWFStatus(int associateId);
    }
}
