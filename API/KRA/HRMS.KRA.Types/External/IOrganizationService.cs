
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Infrastructure.Models.Domain;
using HRMS.KRA.Infrastructure.Models.Response;
using HRMS.KRA.Infrastructure.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.KRA.Types.External
{
    public interface IOrganizationService
    {
        Task<ServiceListResponse<Department>> GetAllDepartmentsAsync(); 
        Task<ServiceListResponse<RoleDetails>> GetUserRolesAsync();
        Task<ServiceListResponse<Department>> GetUserDepartmentDetailsAsync();
        Task<ServiceListResponse<Grade>> GetAllGradesAsync();
        Task<ServiceListResponse<FinancialYear>> GetAllFinancialYearsAsync();
        Task<ServiceListResponse<RoleType>> GetAllRoleTypesAsync();
        Task<ServiceListResponse<GradeRoleType>> GetAllGradeRoleTypesAsync();
        Task<ServiceResponse<NotificationConfiguration>> GetByNotificationTypeAndCategoryAsync(int notificationTypeId, int categoryMasterId);
        Task<ServiceResponse<NotificationConfiguration>> GetNotificationConfigurationAsync(string notificationCode, int categoryMasterId);
        Task<ServiceResponse<bool>> SendEmailAsync(NotificationDetail notificationDetail);
        Task<Department> GetByIdAsync(int departmentId);
        Task<List<RoleTypeDepartment>> GetRoleTypesAndDepartmentsAsync(int departmentId = 0);
        Task<FinancialYear> GetFinancialYearByIdAsync(int financialYearId);
    }
}
