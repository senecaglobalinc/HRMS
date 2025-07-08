using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Associate = HRMS.Employee.Entities;

namespace HRMS.Employee.Types
{
    public interface IEmployeeService
    {
        Task<ServiceListResponse<Associate.Employee>> GetAll(bool? isActive);
        Task<ServiceResponse<Associate.Employee>> GetById(int id);
        Task<ServiceListResponse<EmployeeDetails>> GetByIds(string idList);
        Task<ServiceListResponse<EmployeeDetails>> GetEmployeeByUserName(string userName);
        Task<ServiceListResponse<EmployeeType>> GetEmpTypes();
        Task<ServiceListResponse<EmployeeDetails>> GetEmployeeNames();
        Task<ServiceListResponse<lkValue>> GetBusinessValues(string valueKey);
        Task<ServiceResponse<object>> GetJoinedEmployees();
        Task<ServiceListResponse<EmployeeDetails>> GetManagersAndLeads(int? departmentId = null);
        Task<ServiceListResponse<EmployeeDetails>> GetEmployeeInfo(string searchString, int pageIndex, int pageSize);
        Task<ServiceListResponse<EmployeeDetails>> GetEmployeeOnPagination(string searchString,int pageIndex, int pageSize);
        Task<ServiceResponse<int>> GetEmployeeCount(string searchString);
        List<EmployeeSkills> GetEmployeesBySkillUsingCache();
        Task<ServiceResponse<int>> GetStatusbyId(int empId);
		Task<ServiceResponse<Associate.Employee>> GetByUserId(int userId);
        Task<ServiceResponse<Associate.Employee>> GetActiveEmployeeById(int id);
        Task<ServiceListResponse<Manager>> GetProgramManagersList();
        Task<ServiceListResponse<ActiveEmployee>> GetAllActiveEmployees();
        Task<ServiceListResponse<EmployeeProfileStatus>> GetPendingProfiles();
        Task<ServiceListResponse<EmployeeProfileStatus>> GetRejectedProfiles();
        Task<ServiceListResponse<EmployeeDetails>> GetEmployeeBySearchString(string searchString);
        Task<ServiceListResponse<GenericType>> GetAssociatesForDropdown();
        Task<ServiceListResponse<GenericType>> GetEmployeesForDropdown();
        Task<ServiceListResponse<GenericType>> GetAssociatesByProjectId(int projectId);
        string GetEmployeeName(int employeeId);
        Task<ServiceListResponse<GenericType>> GetAssociatesByDepartmentId(int departmentId);
        Task<ServiceResponse<GenericType>> GetDepartmentHeadByDepartmentId(int departmentId);
        Task<ServiceListResponse<EmployeeSearchDetails>> GetEmployeeDetailsByNameString(string nameString);
        Task<ServiceListResponse<EmployeeRoleDetails>> GetListAssociatesByRoles(string roles);
        Task<ServiceResponse<string>> GetEmployeeWorkEmailAddress(int empId);
        Task<ServiceListResponse<AssociateRoleType>> GetEmployeesByRole(string employeeCode, int? departmentId = null, int? roleId = null);
        Task<ServiceListResponse<AssociateModel>> GetEmployeesByCode(List<string> employeeCodes);
        Task<ServiceListResponse<AssociateLongLeaveModel>> GetEmployeesOnLongLeave(int daysLeftToJoin);
        Task<ServiceListResponse<FinancialYearRoleTypeModel>> GetEmployeeRoleTypes(int employeeId);
        Task<ServiceResponse<FileDetail>> DownloadKRA(string employeeCode, string financialYear, string roleType);
        Task<ServiceListResponse<AssociateRM>> GetAssociateRMDetailsByDepartmentId(int departmentId);
        Task<ServiceListResponse<GenericType>> GetServiceDepartmentAssociates();
        Task<ServiceResponse<bool>> UpdateServiceDepartmentAssociateRM(AssociatesRMDetails associatesRMDetails);
    }
}
