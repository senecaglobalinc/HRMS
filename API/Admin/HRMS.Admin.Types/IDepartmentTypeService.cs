using HRMS.Admin.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IDepartmentTypeService
    {
        Task<dynamic> Create(DepartmentType departmentTypeIn);
        Task<dynamic> Update(DepartmentType departmentTypeIn);
        Task<List<DepartmentType>> GetAll(bool isActive = true);
    }
}
