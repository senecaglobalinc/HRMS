using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IHRMSExternalService
    {
        Task<ServiceResponse<ProjectDTO>> GetProjects();
        Task<ServiceResponse<ProjectDTO>> GetProjectById(int projectId);
        public Task<EmployeeDTO> GetActiveEmployeeNamesAsync(CancellationToken cancellationToken);
        public Task<EmployeesInfo> GetAllEmployeeDetailsForExternalAsync();
        public Task<ServiceResponse<DepartmentDTO>> GetDepartmentsList();
        public Task<ServiceResponse<ProjectsData>> GetProjectsByEmailAndRole(string emailId);
        public Task<ProgramManagersData> GetProgramManagersList();

    }
}
