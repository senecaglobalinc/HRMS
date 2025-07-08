using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Project.Types.External
{
    public interface IOrganizationService
    {
        Task<ServiceResponse<Status>> GetStatusByCategoryAndStatusCode(string categoryName, string statusCode);
        Task<ServiceListResponse<Status>> GetStatusesByCategoryName(string categoryName, bool nightlyJob = false);
        Task<ServiceResponse<User>> GetUserById(int userId);
        Task<ServiceListResponse<Client>> GetClients();
        Task<ServiceListResponse<Client>> GetClientsByIds(List<int> clientIds);
        Task<ServiceResponse<Client>> GetClientById(int clientId);
        Task<ServiceListResponse<PracticeArea>> GetPracticeAreasByIds(List<int> practiceAreaids);
        Task<ServiceListResponse<PracticeArea>> GetAllPracticeArea(bool isActive);
        Task<ServiceResponse<PracticeArea>> GetPracticeAreaById(int practiceAreaId);
        Task<ServiceListResponse<ProjectType>> GetProjectTypesByIds(List<int> projectTypeIds);
        Task<ServiceResponse<ProjectType>> GetProjectTypeById(int projectTypeId);
        Task<ServiceListResponse<ProjectType>> GetAllProjectTypes(bool isActive);
        Task<ServiceResponse<Domain>> GetDomainById(int domainId);
        Task<ServiceListResponse<Department>> GetAllDepartment(bool isActive);
        Task<ServiceResponse<Department>> GetDepartmentById(int departmentId);
        Task<ServiceResponse<Department>> GetDepartmentByCode(string departmentCode);
        Task<ServiceResponse<Category>> GetCategoryByName(string categoryName);
        Task<ServiceResponse<User>> GetUserByEmail(string email);
        Task<ServiceResponse<Status>> GetStatusByCategoryIdAndStatusCode(int categoryId, string statusCode);
        Task<ServiceResponse<NotificationType>> GetNotificationTypeByCode(string notificationCode);
        Task<ServiceResponse<NotificationConfiguration>> GetByNotificationTypeAndCategoryId(int notificationTypeId, int categoryMasterId);
        Task<ServiceListResponse<ProgramManager>> GetProgramManagers(string userRole, int employeeId);
        Task<ServiceResponse<NotificationConfiguration>> GetNotificationConfiguration(int notificationTypeId, int categoryId);
        Task<ServiceListResponse<RoleMaster>> GetRolesByDepartmentId(int departmentId);
        Task<ServiceListResponse<RoleMaster>> GetAllRoleMasters();
        Task<ServiceListResponse<Role>> GetAllRoles(bool? isActive);
        Task<ServiceListResponse<UserRole>> GetAllUserRoles(bool? isActive);
        Task<ServiceListResponse<RoleMaster>> GetRoleMasterNames();
        Task<ServiceListResponse<User>> GetUsers();
        Task<ServiceListResponse<Activity>> GetClosureActivitiesByDepartment(int? departmentId = null);
        void SendEmail(NotificationDetail notificationDetail);
        Task<ServiceListResponse<ReportDetails>> GetAssociateAllocationMasters();
        Task<ServiceListResponse<DepartmentWithDLAddress>> GetAllDepartmentsWithDLs();
    }
}
