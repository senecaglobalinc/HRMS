using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
    public interface IProjectClosureActivityService
    {
        Task<ServiceResponse<Activities>> GetDepartmentActivitiesByProjectId(int ProjectId, int? DepartmentId = null);
        Task<ServiceResponse<int>> CreateActivityChecklist(int ProjectId);
        Task<ServiceResponse<int>> UpdateActivityChecklist(ActivityChecklist projectIn);
        Task<ServiceListResponse<Activities>> GetDepartmentActivitiesForPM(int ProjectId);
    }
}
