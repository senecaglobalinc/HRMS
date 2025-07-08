using HRMS.Admin.Entities;
using HRMS.Admin.Entities.Models;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IUserRoleService
    {
        //Get Application Roles abstract method
        Task<IEnumerable<object>> GetRoles(bool? isActive);

        //Get UserRoles by UserId abstract method
        Task<IEnumerable<object>> GetUserRolesbyUserID(int userID);

        //Get Update UserRole
        Task<dynamic> UpdateUserRole(UserRole userRole);

        //Get users abstract method
        Task<List<UserRole>> GetAllUserRoles(bool? isActive = true);

        //Get users abstract method
        Task<List<Role>> GetAllRoles(bool? isActive = true);

        //Get Program Manager by UserRole and employeeId
        Task<List<ProgramManager>> GetProgramManagers(string userRole, int employeeId);

        //Get User Roles by UserName
        Task<List<Role>> GetUserRoleOnLogin(string userName);

        //Get All User Roles
        Task<List<RoleDetails>> GetUserRoles();

        //get HRA Advisors
        Task<IEnumerable<object>> GetHRAAdvisors();

        //Get Role detail by using Role Name
        Task<Role> GetRoleByRoleName(string roleName);
        Task<ServiceResponse<UserLogin>> GetUserDetailsByUserName(string userName);

        Task<bool> RemoveUserRoleOnExit(int userId);
    }
}
