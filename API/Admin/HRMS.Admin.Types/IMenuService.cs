using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IMenuService
    {
        Task<ServiceListResponse<MenuData>> GetMenuDetails(string roleName);
        Task<ServiceListResponse<Menus>> GetSourceMenuRoles(int roleId);
        Task<ServiceListResponse<Menus>> GetTargetMenuRoles(int roleId);
        Task<BaseServiceResponse> AddTargetMenuRoles(MenuRoleDetails menuRoles);
    }
}
