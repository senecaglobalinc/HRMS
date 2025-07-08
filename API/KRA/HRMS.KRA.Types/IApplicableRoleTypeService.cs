using HRMS.KRA.Entities;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Infrastructure.Models.Request;
using HRMS.KRA.Infrastructure.Models.Response;
using HRMS.KRA.Infrastructure.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.KRA.Types
{
    public interface IApplicableRoleTypeService
    {
        Task<ServiceListResponse<ApplicableRoleTypeModel>> GetAll(int? FinancialYearId = null, int? DepartmentId = null, int? GradeRoleTypeId = null,
            int? StatusId = null, int? gradeId = null);
      
        Task<ServiceResponse<int>> Create(ApplicableRoleTypeRequest model);
        Task<ServiceResponse<int>> Update(ApplicableRoleTypeRequest model);
        Task<ServiceResponse<ApplicableRoleType>> UpdateRoleTypeStatus(ApplicableRoleTypeRequest request);
        Task<(bool IsSuccessful, string Message)> Delete(int ApplicableRoleTypeId);
    }
}
