using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace HRMS.Admin.Types
{
    /// <summary>
    /// Interface for role service
    /// </summary>
    public interface IRoleService
    {
        //Create functional role abstract method 
        Task<dynamic> Create(RoleMaster roleMasterIn);

        //get functional roles by DepartmentId abstract method 
        Task<List<RoleMaster>> GetByDepartmentID(int departmentId);
        Task<ServiceListResponse<RoleMaster>> GetAll();
        Task<ServiceListResponse<RoleMasterDetails>> GetRoleNames();

        //get SGRoles, SGRolePrefix and SGRoleSuffix by DepartmentId abstract method 
        Task<dynamic> GetSGRoleSuffixAndPrefix(int departmentId);

        //Update functional role abstract method 
        Task<dynamic> Update(RoleMaster roleMasterIn);

        Task<List<RolesDepartmentsList>> GetRolesAndDepartments();
    }
}
