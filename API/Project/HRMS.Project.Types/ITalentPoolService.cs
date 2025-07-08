using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
    public interface ITalentPoolService
    {
        Task<ServiceListResponse<TalentPool>> GetAll();
        Task<ServiceResponse<TalentPool>> GetById(int id);
        Task<ServiceResponse<int>> Create(int projectId, int practiceAreaId);
    }
}
