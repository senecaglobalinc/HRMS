using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Project.Types
{
    public interface IAllocationPercentageService
    {
        Task<ServiceListResponse<AllocationPercentage>> GetAll();
        Task<ServiceResponse<AllocationPercentage>> GetById(int id);
        Task<ServiceListResponse<GenericType>> GetAllocationPercentageForDropdown();
    }
}
