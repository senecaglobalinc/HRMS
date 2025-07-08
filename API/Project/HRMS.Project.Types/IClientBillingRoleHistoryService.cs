using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Response;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
    public interface IClientBillingRoleHistoryService
    {
        Task<ServiceResponse<ClientBillingRolesHistory>> Create(ClientBillingRolesHistory clientBillingRolesHistoryIn);
    }
}
