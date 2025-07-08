using HRMS.Admin.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IProjectTypeService
    {
        Task<dynamic> Create(ProjectType projectTypeIn);
        Task<List<ProjectType>> GetAll(bool isActive = true);
        Task<ProjectType> GetByProjectTypeCode(string projectTypeCode);
        Task<List<ProjectType>> GetByProjectTypeIds(int[] projectTypeIds);
        Task<ProjectType> GetProjectTypeById(int projectTypeId);
        Task<dynamic> Update(ProjectType projectTypeIn);
    }
}
