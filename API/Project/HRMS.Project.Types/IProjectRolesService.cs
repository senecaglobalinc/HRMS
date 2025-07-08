using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
    public interface IProjectRolesService
    {
        Task<ServiceListResponse<ProjectRoles>> GetAll();
        Task<ServiceResponse<ProjectRoles>> GetById(int id);
    }
}
