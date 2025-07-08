using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
    public interface ISOWService
    {
        Task<ServiceResponse<int>> Create(SOWRequest sowRequest);
        Task<ServiceResponse<int>> Delete(int id);
        Task<ServiceListResponse<SOW>> GetAllByProjectId(int projectId);
        Task<ServiceResponse<SOW>> GetByIdAndProjectId(int id, int projectId, string roleName);
        Task<ServiceResponse<int>> Update(SOWRequest sowRequest);
    }
}
