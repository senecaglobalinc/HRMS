using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IExitTypeService
    {
        Task<dynamic> Create(ExitType exitTypeIn);
        Task<dynamic> Update(ExitType exitTypeIn);
        Task<List<ExitType>> GetAll(bool isActive = true);
        Task<ExitType> GetByExitTypeId(int exitTypeId);
        Task<List<GenericType>> GetExitTypesForDropdown();
        public Task<ServiceResponse<int>> GetExitTypeIdByName(string exitTypeName);
    }
}
