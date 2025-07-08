using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
    public interface IClientBillingRoleService
    {
        Task<ServiceListResponse<ClientBillingRoles>> GetAll();
        Task<ServiceResponse<ClientBillingRoles>> GetById(int id);
        Task<ServiceListResponse<ClientBillingRole>> GetAllByProjectId(int projectId);
        Task<ServiceResponse<int>> Create(ClientBillingRoles ClientBillingRoleIn);
        Task<ServiceResponse<int>> Update(ClientBillingRoles ClientBillingRoleIn);
        Task<ServiceResponse<bool>> Delete(int clientBillingRoleId);
        Task<ServiceResponse<int>> Close(int clientBillingRoleId, DateTime endDate,string reason);


    }
}
