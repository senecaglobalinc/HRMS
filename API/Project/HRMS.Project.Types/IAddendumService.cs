using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
    public interface IAddendumService
    {
        Task<ServiceResponse<bool>> Create(AddendumRequest addendumRequest);
        Task<ServiceListResponse<Addendum>> GetAllBySOWIdAndProjectId(int sowId, int projectId);
        Task<ServiceResponse<Addendum>> GetByIdAndProjectId(int id, int projectId,string roleName);
        Task<ServiceResponse<bool>> Update(AddendumRequest addendumRequest);
    }
}
