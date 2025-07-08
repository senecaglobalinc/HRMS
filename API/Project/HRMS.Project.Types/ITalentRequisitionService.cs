using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
   public interface ITalentRequisitionService
    {
        Task<ServiceListResponse<TalentRequisition>> GetAll();
        Task<ServiceResponse<TalentRequisition>> GetById(int id);
    }
}
