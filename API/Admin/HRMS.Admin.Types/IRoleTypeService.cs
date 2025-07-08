using HRMS.Admin.Entities;
using HRMS.Admin.Entities.Models;
using HRMS.Admin.Infrastructure.Models;
using HRMS.Admin.Infrastructure.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IRoleTypeService
    {
        Task<List<RoleTypeModel>> GetAll(bool? isActive);
        Task<RoleTypeModel> GetById(int gradeRoleTypeId);
        Task<(bool IsSuccessful, string Message)> Create(RoleTypeModel model);
        Task<(bool IsSuccessful, string Message)> Update(RoleTypeModel model);
        Task<(bool IsSuccessful, string Message)> Delete(int roleTypeId);
        Task<List<GenericType>> GetRoleTypesForDropdown(int financialYearId, int departmentId);
        Task<List<RoleTypeDepartment>> GetRoleTypesAndDepartmentsAsync(int departmentId = 0);
    }
}
