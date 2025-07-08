using HRMS.Admin.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IProficiencyLevelService
    {
        Task<dynamic> Create(ProficiencyLevel proficiencyLevelIn);
        Task<List<ProficiencyLevel>> GetByIds(string skillIds);
        Task<List<ProficiencyLevel>> GetAll(bool isActive = true);
        Task<dynamic> Update(ProficiencyLevel proficiencyLevelIn);
    }
}
