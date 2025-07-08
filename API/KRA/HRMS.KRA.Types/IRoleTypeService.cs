using HRMS.KRA.Infrastructure.Models.Domain;
using HRMS.KRA.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.KRA.Types
{
    public interface IRoleTypeService
    {
        Task<ServiceListResponse<RoleType>> GetRoleTypesByGradeIdAsync(int gradeId);
    }
}
