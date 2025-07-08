using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Types;
using HRMS.Admin.Types.External;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    public class UserService : IUserService
    {
        #region Global Variables

        private readonly ILogger<UserService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;
        private IEmployeeService m_employeeService;
        private readonly IHttpContextAccessor m_HttpContextAccessor;


        #endregion

        #region Constructor
        public UserService(ILogger<UserService> logger, AdminContext adminContext, IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints,IEmployeeService employeeService, IHttpContextAccessor httpContextAccessor)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, User>();
            });
            m_mapper = config.CreateMapper();
            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints?.Value;
            m_employeeService = employeeService;
            m_HttpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region GetByUserId
        /// <summary>
        /// Get user by userId
        /// </summary>
        /// <param name="userId">Is active or not</param>
        /// <returns></returns>
        public async Task<User> GetByUserId(int userId) =>
                        await m_AdminContext.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
        #endregion

            #region GetAllUsers
        /// <summary>
        /// GetAllUsers
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<List<User>> GetAllUsers(bool? isActive = true) =>
                        await m_AdminContext.Users.Where(us => us.IsActive == isActive).OrderBy(x => x.UserName).ToListAsync();
        #endregion

        #region GetUsers
        /// <summary>
        /// GetUsers For Allocation and Release notifications
        /// </summary>
        public async Task<IEnumerable<UserDetails>> GetUsers()
        {
            var employees = (await m_employeeService.GetAll(true)).Items;
            var users = await m_AdminContext.Users.Where(x=>x.IsActive==true).ToListAsync();
            return (from user in users
                    join emp in employees on user.UserId equals emp.UserId
                    where  emp.UserId != null
                    orderby user.UserName
                    select new UserDetails()
                    {
                        UserName = user.UserName,
                        UserId = user.UserId,
                        EmailAddress = user.EmailAddress,
                        DepartmentId = emp.DepartmentId,
                        EmployeeId = emp.EmployeeId,
                        EmployeeCode = emp.EmployeeCode,
                        FirstName = emp.FirstName
                    }).OrderBy(x => x.UserName).ToList();

        }
        #endregion

        #region GetUsersBySearchString
        /// <summary>
        /// Get Users by search string
        /// </summary>
        public async Task<List<UserDetails>> GetUsersBySearchString(string searchstring)
        {

            List<EmployeeDetails> employees = (await m_employeeService.GetEmployeeBySearchString(searchstring)).Items;
            List<int?> empids = employees.Select(x => x.UserId).ToList();
            List<User> users = m_AdminContext.Users.Where(u => empids.Contains(u.UserId) && u.IsActive == true).ToList();
            return (from user in users
                    join emp in employees on user.UserId equals emp.UserId
                    where emp.UserId != null
                    orderby user.UserName
                    select new UserDetails()
                    {
                        UserName = user.UserName,
                        UserId = user.UserId,
                        EmailAddress = user.EmailAddress,
                        DepartmentId = emp.DepartmentId,
                        EmployeeId = emp.EmpId,
                        EmployeeCode = emp.EmployeeCode,
                        FirstName = emp.FirstName
                    }).OrderBy(x => x.UserName).ToList();


        }
        #endregion

        #region GetById
        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        public async Task<User> GetById(int userId) =>
                        await m_AdminContext.Users.FindAsync(userId);

        #endregion

        #region GetUserByEmail
        /// <summary>
        /// Get user by email address
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public async Task<User> GetUserByEmail(string emailAddress) =>
                        await m_AdminContext.Users.Where(u => u.EmailAddress.ToLower() == emailAddress.ToLower()).FirstOrDefaultAsync();
        #endregion

        #region GetActivepUserByUserId
        /// <summary>
        /// Get active user by userId
        /// </summary>
        /// <param name="userId">Is active or not</param>
        /// <returns></returns>
        public async Task<User> GetActiveUserByUserId(int userId) =>
                        await m_AdminContext.Users.Where(u => u.UserId == userId && u.IsActive == true).FirstOrDefaultAsync();
        #endregion

        //Private Method

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="userRole"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(User user, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in UserService");

            dynamic response = new ExpandoObject();
            response.User = user;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in UserService");

            return response;
        }

        #endregion

        #region UpdateUser
        ///<summary>
        ///UpdateUser
        ///</summary>
        public async Task<bool> UpdateUser(int userId)
        {
            User user = m_AdminContext.Users.Find(userId);
            user.IsActive = false;
            //Deactivating Usser
            var isupdate = await m_AdminContext.SaveChangesAsync();

            if (isupdate > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region GetUsersByRoles
        /// <summary>
        /// GetUsersByRoles
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public async Task<List<UserRoleDetails>> GetUsersByRoles(string roles)
        {
            List<string> rolesList = new List<string>(roles.Split(","));
            List<Role> Roles = await m_AdminContext.Roles.ToListAsync();
            List<UserRole> UserRoles = m_AdminContext.UserRoles.Where(urole => urole.IsActive == true).ToList();
            List<UserRoleDetails> userRoles = ( from rolename in rolesList
                              join rol in Roles on rolename.ToLower() equals rol.RoleName.ToLower()
                              join ur in UserRoles on rol.RoleId equals ur.RoleId
                              select new UserRoleDetails
                              {
                                  UserId = ur.UserId,
                                  RoleId = ur.RoleId,
                                  RoleName = rol.RoleName
                              }).ToList();
            return userRoles;
        }

        #endregion

        #region GetUserEmail
        /// <summary>
        /// GetUserEmail
        /// </summary>
        /// <returns></returns>
        public string GetUserEmail()
        {
            string userEmail = m_HttpContextAccessor.HttpContext.Response.Headers["UserEmailId"].ToString();
            return userEmail;
        }

        #endregion
    }
}
