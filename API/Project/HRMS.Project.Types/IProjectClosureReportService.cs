using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
    public interface IProjectClosureReportService
    {
        Task<ServiceResponse<int>> Create(int projectId);
        
        Task<ServiceResponse<int>> Update(ProjectClosureReportRequest projectIn);
        Task<ServiceListResponse<ProjectClosureReportDetails>> GetClosureReportByProjectId(int ProjectId);
       
        Task<ServiceResponse<int>> NotificationConfiguration(int projectId, int notificationTypeId, int? DepartmentId);
        Task<ServiceResponse<int>> Save(UploadFiles uploadFilesIn);
        Task<ServiceResponse<String>> Download(string Filetype, int projectId);
        Task<ServiceResponse<bool>> Delete(string Filetype, int projectId);


    }
}
