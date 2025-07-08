using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Types.External
{
    public interface IOrganizationService
    {
        Task<ServiceListResponse<User>> GetUsers();
        Task<ServiceListResponse<User>> GetActiveUsersById(int userId);
        Task<ServiceResponse<Status>> GetStatusById(int statusId);
        Task<ServiceResponse<Status>> GetStatusByCode(string statusCode);
        Task<ServiceListResponse<Status>> GetStatusesByCategoryName(string categoryName);
        Task<ServiceListResponse<Department>> GetAllDepartments(bool nightlyJob = false);
        Task<ServiceListResponse<PracticeArea>> GetAllPracticeAreas(bool nightlyJob = false);
        Task<ServiceListResponse<Designation>> GetAllDesignations();
        Task<ServiceResponse<Designation>> GetDesignationById(int designationId);
        Task<ServiceResponse<Designation>> GetDesignationByCode(string designationCode);
        Task<ServiceResponse<Department>> GetDepartmentByCode(string departmentCode);
        Task<ServiceListResponse<Department>> GetDepartmentByCodes(List<string> departmentCodes);
        Task<ServiceResponse<Department>> GetDepartmentById(int departmentId);
        Task<ServiceResponse<PracticeArea>> GetPracticeAreaByCode(string practiceAreaCode);
        Task<ServiceListResponse<Grade>> GetAllGrades();
        Task<ServiceListResponse<UserRole>> GetAllUserRoles();
        Task<ServiceListResponse<Role>> GetAllRoles();
        Task<ServiceResponse<Role>> GetRoleByRoleName(string roleName);
        Task<ServiceListResponse<Status>> GetAllStatuses();
        Task<ServiceResponse<Status>> GetStatusByCategoryAndStatusCode(string category, string statusCode);
        Task<ServiceListResponse<Skill>> GetSkillsBySkillGroupId(List<int> certificationSkillGroupIds);
        Task<ServiceListResponse<Skill>> GetSkillsBySkillId(List<int> skillIds);
        Task<ServiceListResponse<Skill>> GetAllSkills(bool isActive);
        Task<ServiceListResponse<Domain>> GetAllDomains();
        Task<ServiceListResponse<ProficiencyLevel>> GetAllProficiencyLevels(bool isActive);
        Task<ServiceListResponse<CompetencyArea>> GetCompetencyAreas(bool isActive);
        Task<ServiceResponse<NotificationConfiguration>> GetByNotificationTypeAndCategory(int notificationTypeId, int categoryMasterId);
        Task<ServiceListResponse<ProficiencyLevel>> GetProficiencyLevelsByProficiencyLevelId(List<int> proficiencyLevelIds);
        Task<ServiceResponse<bool>> SendEmail(NotificationDetail notificationDetail);
        Task<ServiceListResponse<Activity>> GetExitActivitiesByDepartment(int? departmentId = null);
        public Task<ServiceResponse<ExitType>> GetExitTypeById(int exitTypeId);
        Task<ServiceListResponse<GenericType>> GetAllExitTypes();
        Task<ServiceListResponse<GenericType>> GetAllExitReasons();
        Task<bool> UpdateUser(int userId);
        Task<bool> RemoveUserRoleOnExit(int userId);
        Task<ServiceResponse<DepartmentDL>> GetDepartmentDLByDeptId(int deptIds);
        Task<ServiceListResponse<EmployeeRoleDetails>> GetUsersByRoles(string roles);
        Task<ServiceListResponse<RoleType>> GetAllRoleTypes();
        Task<ServiceResponse<RoleType>> GetRoleTypeById(int roleTypeId);
        Task<ServiceListResponse<DepartmentWithDLAddress>> GetAllDepartmentsWithDLs();
        Task<ServiceListResponse<GenericType>> GetRoleTypesForDropdown();
        Task<ServiceListResponse<FinancialYear>> GetAllFinancialYearsAsync();
        Task<ServiceListResponse<Holiday>> GetAllHolidays();
        Task<ServiceResponse<Client>> GetClientById(int clientId);
        Task<ServiceListResponse<Client>> GetAllClients();
        Task<ServiceListResponse<RoleMasterDetails>> GetRoleMasterDetails();
    }
}
