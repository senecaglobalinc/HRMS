using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Entities.Models;
using HRMS.Admin.Infrastructure;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using HRMS.Admin.Types;
using HRMS.Admin.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    public class UserRoleService : IUserRoleService
    {
        #region Global Variables

        private readonly ILogger<UserRoleService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;
        private IEmployeeService m_employeeService;
        private IConfiguration m_configuration;

        #endregion

        #region Constructor
        public UserRoleService(ILogger<UserRoleService> logger, AdminContext adminContext, IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints, IEmployeeService employeeService,IConfiguration configuration)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserRole, UserRole>();
            });
            m_mapper = config.CreateMapper();
            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints?.Value;
            m_employeeService = employeeService;
            m_configuration = configuration;
        }
        #endregion

        private async Task<List<ProjectManager>> GetProjectManagersList()
        {
            var httpClientFactory = m_clientFactory.CreateClient("ProjectClient");
            httpClientFactory.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClientFactory.GetStringAsync(m_apiEndPoints.ProjectEndPoint + "ProjectManager/GetAll?isActive=true");

            if (response == null)
                return CreateResponse(null, false, "Error occured while fetching existing project managers.");

            return JsonConvert.DeserializeObject<List<ProjectManager>>(response.ToString());
        }

        #region GetRoles
        /// <summary>
        /// Gets the Roles
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> GetRoles(bool? isActive)
        {
            var roles = await (from role in m_AdminContext.Roles.Where(i => i.IsActive == true)
                               select new
                               {
                                   ID = role.RoleId,
                                   Name = role.RoleName,
                                   IsActive = role.IsActive
                               }).OrderBy(r => r.Name).ToListAsync();

            return roles;
        }
        #endregion

        #region GetUserRoleOnLogin
        /// <summary>
        /// GetUserRoleOnLogin
        /// </summary>
        /// <param name="userName">userName</param>
        /// <returns></returns>
        public async Task<List<Role>> GetUserRoleOnLogin(string userName)
        {
            return await (from role in m_AdminContext.Roles
                          join usrrole in m_AdminContext.UserRoles on role.RoleId equals usrrole.RoleId
                          join usr in m_AdminContext.Users on usrrole.UserId equals usr.UserId
                          where usr.EmailAddress == userName && usrrole.IsActive == true
                          select new Role
                          {
                              RoleId = role.RoleId,
                              RoleName = role.RoleName
                          }).Distinct().OrderBy(st => st.RoleName).ToListAsync();

        }

        #endregion

        #region GetUserRoles
        /// <summary>
        /// Get all UserRoles
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleDetails>> GetUserRoles()
        {
            var userRoles = await (from usrrole in m_AdminContext.UserRoles
                                   join role in m_AdminContext.Roles on usrrole.RoleId equals role.RoleId
                                   join usr in m_AdminContext.Users on usrrole.UserId equals usr.UserId
                                   where usrrole.IsActive == true && role.IsActive == true
                                   select new
                                   {
                                       Username = usr.EmailAddress,
                                       RoleName = role.RoleName
                                   }).ToListAsync();

            return (from ur in userRoles
                    group ur by ur.Username into g
                    select new RoleDetails { Username = g.Key, Roles = string.Join(";", g.Select(c => c.RoleName)) })
                    .ToList();

        }

        #endregion

        #region GetAllRoles
        /// <summary>
        /// GetAllRoles
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<List<Role>> GetAllRoles(bool? isActive = true) =>
                        await m_AdminContext.Roles.Where(rs => rs.IsActive == isActive).OrderBy(x => x.RoleName).ToListAsync();
        #endregion

        #region GetAllUserRoles
        /// <summary>
        /// GetAllUserRoles
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<List<UserRole>> GetAllUserRoles(bool? isActive = true) =>
                        await m_AdminContext.UserRoles.Where(us => us.IsActive == isActive).ToListAsync();
        #endregion

        #region GetUserRolesbyUserID
        /// <summary>
        /// GetUserRolesbyUserID
        /// </summary>
        /// <param name="userID">userID</param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> GetUserRolesbyUserID(int userID)
        {
            Employee employee = (await m_employeeService.GetEmployeeByUserId(userID)).Item;

            return (from usr in m_AdminContext.Users
                    join ur in m_AdminContext.UserRoles on usr.UserId equals ur.UserId
                    join r in m_AdminContext.Roles on ur.RoleId equals r.RoleId
                    where usr.UserId == userID && usr.IsActive == true && ur.IsActive == true
                    select new
                    {
                        UserRoleID = ur.UserRoleID,
                        UserId = usr.UserId,
                        RoleId = r.RoleId,
                        IsActive = ur.IsActive,
                        IsPrimary = ur.IsPrimary == null ? false : ur.IsPrimary,
                        RoleName = r.RoleName,
                        UserName = usr.UserName,
                        EmployeeId = employee.EmployeeId,
                        EmployeeName = employee.FirstName + "" + employee.LastName

                    }).Distinct().ToList();

        }

        #endregion

        #region GetProgramManagers
        /// <summary>
        /// GetProgramManagers
        /// </summary>
        /// <param name="userRole"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<List<ProgramManager>> GetProgramManagers(string userRole, int employeeId)
        {
            List<EmployeeDetails> employees = (await (m_employeeService.GetAll(true))).Items;
            List<UserRole> userRoles = await m_AdminContext.UserRoles.ToListAsync();
            List<Role> roles = await m_AdminContext.Roles.ToListAsync();

            if ("Program Manager".ToLower().Trim().Equals(userRole.ToLower().Trim()))
                return (from ur in userRoles
                        join r in roles on ur.RoleId equals r.RoleId
                        join emp in employees on ur.UserId equals emp.UserId
                        where ur.IsActive == true && r.IsActive == true
                              && "Program Manager".ToLower().Trim().Equals(r.RoleName.ToLower().Trim()) &&
                              emp.EmployeeId == employeeId
                        select new ProgramManager
                        {
                            UserRolesId = ur.UserRoleID,
                            EmployeeId = emp.EmployeeId,
                            ManagerName = $"{emp.FirstName} {emp.LastName}"
                        }).ToList();
            //Fetch all projects
            else if ("Department Head".ToLower().Trim().Equals(userRole.ToLower().Trim()))
                return (from ur in userRoles
                        join r in roles on ur.RoleId equals r.RoleId
                        join emp in employees on ur.UserId equals emp.UserId
                        where ur.IsActive == true && r.IsActive == true
                              && "Program Manager".ToLower().Trim().Equals(r.RoleName.ToLower().Trim())
                        select new ProgramManager
                        {
                            UserRolesId = ur.UserRoleID,
                            EmployeeId = emp.EmployeeId,
                            ManagerName = $"{emp.FirstName} {emp.LastName}"
                        }).ToList();
            else
                return null;

        }

        #endregion

        #region UpdateUserRole
        /// <summary>
        /// UpdateUserRole
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
        public async Task<dynamic> UpdateUserRole(UserRole userRole)
        {
            int assignedRolesCount = 0;
            m_Logger.LogInformation("Calling UpdateUserRole method in UserRoleService");
            IQueryable<UserRole> userRoleQuery = m_AdminContext.UserRoles.Where(
                                                ur => ur.UserId == userRole.UserId &&
                                                ur.RoleId == userRole.RoleId &&
                                                ur.IsActive == true);
            Employee employee = (await m_employeeService.GetEmployeeByUserId((int)userRole.UserId)).Item;
            assignedRolesCount = userRoleQuery.Count();

            if (assignedRolesCount <= 0)
            {
                var departmentDetails = await m_AdminContext.Departments.FirstOrDefaultAsync(w => w.DepartmentId == employee.DepartmentId);

                if (departmentDetails != null)
                {
                    if (departmentDetails.DepartmentTypeId == 1)
                        return await DeliveryDepartmentRoles(userRole, employee.EmployeeId);
                    else
                        return await NonDeliveryDepartmentRoles(userRole, employee.EmployeeId);
                }

                return CreateResponse(null, false, "Department not found for User.");
            }
            else
            {
                UserRole UserRole = userRoleQuery.FirstOrDefault();

                if (UserRole.IsActive == userRole.IsActive)
                    return CreateResponse(userRoleQuery.FirstOrDefault(), false, "Role already exist for the User.");

                UserRole.IsActive = userRole.IsActive;
                int retValue = await m_AdminContext.SaveChangesAsync();

                if (retValue > 0)
                    return CreateResponse(UserRole, true, string.Empty);
                else
                    return CreateResponse(null, false, "No record updated.");
            }
        }
        #endregion

        #region AssignRole
        private async Task<dynamic> AssignUserRole(int? RoleId, int? UserId, bool? IsActive)
        {
            int isCreated;

            UserRole userRole = new UserRole();
            userRole.RoleId = RoleId;
            userRole.IsActive = IsActive;
            userRole.UserId = UserId;
            m_AdminContext.UserRoles.Add(userRole);
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
                return CreateResponse(userRole, true, string.Empty);
            else
                return CreateResponse(null, false, "failed to assign userRole to the user.");
        }

        private async Task<dynamic> DeliveryDepartmentRoles(UserRole userRole, int EmployeeId)
        {
            dynamic retValue;
            List<ProjectManager> ProjectManagers = await GetProjectManagersList();
            if (userRole.Role.RoleName.Trim().ToLower() == "Lead".Trim().ToLower())
            {
                int leadId = (from projectManager in ProjectManagers
                              where projectManager.LeadId == EmployeeId
                              select projectManager.Id).FirstOrDefault();

                if (leadId > 0)
                    retValue = await AssignUserRole(userRole.RoleId, userRole.UserId, userRole.IsActive);
                else
                    return CreateResponse(null, false, "Cannot assign lead role to associate.");

            }
            else if (userRole.Role.RoleName.Trim().ToLower() == "Reporting Manager".Trim().ToLower())
            {
                int projectManagerId = (from projectManager in ProjectManagers
                                        where projectManager.ReportingManagerId == EmployeeId && projectManager.IsActive == true
                                        select projectManager.Id).FirstOrDefault();

                if (projectManagerId > 0)
                    retValue = await AssignUserRole(userRole.RoleId, userRole.UserId, userRole.IsActive);
                else
                    return CreateResponse(null, false, "Cannot assign reporting manager role to associate.");

            }
            else if (userRole.Role.RoleName.Trim().ToLower() == "Program Manager".Trim().ToLower())
            {
                int projectManagerId = (from projectManager in ProjectManagers
                                        where projectManager.ProgramManagerId == EmployeeId && projectManager.IsActive == true
                                        select projectManager.Id).FirstOrDefault();

                if (projectManagerId > 0)
                    retValue = await AssignUserRole(userRole.RoleId, userRole.UserId, userRole.IsActive);
                else
                    return CreateResponse(null, false, "Cannot assign program manager role to associate.");

            }
            else if (userRole.Role.RoleName.Trim().ToLower() == "Department Head".Trim().ToLower())
            {
                retValue = await AssignDepartmentHeadRole(userRole, EmployeeId);
            }
            else
            {
                retValue = await AssignUserRole(userRole.RoleId, userRole.UserId, userRole.IsActive);
            }
            return retValue;
        }

        private async Task<dynamic> NonDeliveryDepartmentRoles(UserRole userRole, int EmployeeId)
        {
            dynamic retValue;
            List<EmployeeDetails> employees = (await m_employeeService.GetAll(true)).Items;
            if (userRole.Role.RoleName.Trim().ToLower() == "Lead".Trim().ToLower() || userRole.Role.RoleName.Trim().ToLower() == "Program Manager".Trim().ToLower()
                || userRole.Role.RoleName.Trim().ToLower() == "Reporting Manager".Trim().ToLower())
            {

                int leadId = (from employee in employees
                              where employee.ReportingManager == EmployeeId && employee.IsActive == true
                              select employee.EmployeeId).FirstOrDefault();

                if (leadId > 0)
                    retValue = await AssignUserRole(userRole.RoleId, userRole.UserId, userRole.IsActive);
                else
                    return CreateResponse(null, false, "Cannot assign lead role to associate.");

            }
            else if (userRole.Role.RoleName.Trim().ToLower() == "Department Head".Trim().ToLower())
            {
                retValue = await AssignDepartmentHeadRole(userRole, EmployeeId);
            }
            else
            {
                retValue = await AssignUserRole(userRole.RoleId, userRole.UserId, userRole.IsActive);
            }

            return retValue;
        }

        private async Task<dynamic> AssignDepartmentHeadRole(UserRole userRoleData, int EmployeeId)
        {
            dynamic retValue;

            int departmentID = (from department in m_AdminContext.Departments
                                where department.DepartmentHeadId == EmployeeId && department.IsActive == true
                                select department.DepartmentId).FirstOrDefault();

            if (departmentID > 0)
                retValue = await AssignUserRole(userRoleData.RoleId, userRoleData.UserId, userRoleData.IsActive);
            else
                return CreateResponse(null, false, "Cannot assign department head role to associate.");


            return retValue;
        }
        #endregion

        #region GetHRAAdvisors
        /// <summary>
        /// Get the all the HRA Advisors
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> GetHRAAdvisors()
        {
            return await (from r in m_AdminContext.Roles
                          join usrrole in m_AdminContext.UserRoles on r.RoleId equals usrrole.RoleId
                          join usr in m_AdminContext.Users on usrrole.UserId equals usr.UserId
                          where r.RoleName == "HRA" && usrrole.IsActive == true
                          select new
                          {
                              ID = usr.UserId,
                              Name = usr.UserName
                          }).OrderBy(x => x.Name).ToListAsync();
        }
        #endregion

        #region RemoveUserRoleOnExit
        /// <summary>
        /// RemoveUserRoleOnExit
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveUserRoleOnExit(int userId)
        {
            m_Logger.LogInformation("Calling RemoveUserRoleOnExit method in UserRoleService");

            List<UserRole> userRoles = await m_AdminContext.UserRoles.Where(ur => ur.UserId == userId && ur.IsActive == true).ToListAsync();

            if (userRoles.Count() > 0)
            {
                foreach (UserRole userRole in userRoles)
                {
                    userRole.IsActive = false;
                }

                int retValue = await m_AdminContext.SaveChangesAsync();

                if (retValue > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        #endregion

        //Private Method
        #region GetRoleByRoleName
        /// <summary>
        /// Get the Role by role name
        /// </summary>
        /// <param name="roleName">roleName</param>
        /// <returns>RoleDetails</returns>
        public async Task<Role> GetRoleByRoleName(string roleName) =>
                        await m_AdminContext.Roles.Where(r => r.RoleName == roleName && r.IsActive == true)
                        .FirstOrDefaultAsync();

        #endregion

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="userRole"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(UserRole userRole, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in UserRoleService");

            dynamic response = new ExpandoObject();
            response.UserRole = userRole;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in UserRoleService");

            return response;
        }

        #endregion

        #region GetUserDetailsByUserName
        /// <summary>
        /// Get employee by id
        /// </summary>
        /// <param name="username">employee Id</param>
        /// <returns></returns>
        public async Task<ServiceResponse<UserLogin>> GetUserDetailsByUserName(string userName)
        {
            var response = new ServiceResponse<UserLogin>();
            try
            {
                StringBuilder roles = new StringBuilder();
                //List<Employee> employees = await GetEmployeesList();

                var user = await m_AdminContext.Users.Where(st => st.EmailAddress.ToLower().Trim() == userName.ToLower().Trim() && st.IsActive == true).FirstOrDefaultAsync();
                if (user == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "Invalid Username";
                }
                else
                {
                    Employee employee = (await m_employeeService.GetEmployeeByUserId(user.UserId)).Item;
                    //int employeeId = employees.Where(st => st.UserId == user.UserId).Select(st => st.EmployeeId).FirstOrDefault();
                    List<Role> userRole = await GetUserRoleOnLogin(userName);
                    if (userRole.Count == 0)
                    {
                        response.IsSuccessful = false;
                        response.Message = "User roles not found ";
                    }
                    foreach (Role role in userRole)
                    {
                        roles.Append(role.RoleName);
                        roles.Append(",");
                    }
                    string roleStr = roles.ToString();
                    string trimmedRoles = roleStr.TrimEnd(',');

                    string? allowedWfoAssociates = m_configuration.GetSection("AssociateCodesAllowedWFOInHrms").Value;
                    List<string> allowedWfoAssociatesList = allowedWfoAssociates?.Split(",").ToList() ?? new List<string>();

                    bool isExceptional = allowedWfoAssociatesList.Contains(employee.EmployeeCode);

                    UserLogin userLogin = new UserLogin
                    {
                        EmployeeId = employee.EmployeeId,
                        EmployeeCode = employee.EmployeeCode,
                        roles = trimmedRoles,
                        username = userName,
                        EmployeeDepartmentId = employee.DepartmentId,
                        allowedWfoInHrms = isExceptional
                    };

                    response.IsSuccessful = true;
                    response.Item = userLogin;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching employee by Id";
                m_Logger.LogError("Error occured in GetById() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion
    }
}
