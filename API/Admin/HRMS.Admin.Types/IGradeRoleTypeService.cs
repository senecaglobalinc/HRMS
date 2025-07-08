using HRMS.Admin.Entities;
using HRMS.Admin.Entities.Models;
using HRMS.Admin.Infrastructure.Models;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IGradeRoleTypeService
    {
        //Get all GradeRoleType detail
        Task<List<GradeRoleType>> GetAll(bool? isActive = true);
        //Get GradeRoleType detail by id abstract method
        Task<GradeRoleType> GetById(int gradeRoleTypeId);
        //Create GradeRoleType abstract method 
        Task<ServiceResponse<RoleType>> Create(GradeRoleTypeRequest model);
        //Get GradeRoleType details abstract method
        Task<List<GradeRoleTypeModel>> GetGradesBySearchFilters(int financialYear, int departmentId = 0, int roleTypeId = 0);            
    }
}
