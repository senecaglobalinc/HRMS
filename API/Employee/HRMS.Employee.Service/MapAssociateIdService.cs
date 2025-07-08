using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Net.Http.Headers;
using HRMS.Employee.Service.External;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Types.External;
using HRMS.Employee.Infrastructure.Models.Domain;

namespace HRMS.Employee.Service
{
    /// <summary>
    /// Service class to map Associate to user.
    /// </summary>
    public class MapAssociateIdService : IMapAssociateIdService
    {
        #region Global Varibles

        private readonly ILogger<MapAssociateIdService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;
        private readonly IOrganizationService m_OrgService;

        #endregion

        #region Constructor
        public MapAssociateIdService(EmployeeDBContext employeeDBContext, ILogger<MapAssociateIdService> logger, 
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints,
            IOrganizationService orgService)
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints?.Value;
            m_OrgService = orgService;
        }

        #endregion

        #region GetUsers
        /// <summary>
        /// Retrieves active users where the UserId is not mapped to employee.
        /// </summary>
        public async Task<ServiceListResponse<User>> GetUnMappedUsers()
        {
            var response = new ServiceListResponse<User>();
            var users = await m_OrgService.GetUsers();
            var employees = m_EmployeeContext.Employees.ToList();
            try
            {
                if (users.IsSuccessful == true)
                {
                    var result = (from user in users.Items
                                  join emp in employees on user.UserId equals emp.UserId into ur
                                  from dep in ur.DefaultIfEmpty()
                                  where user.IsActive == true && dep == null
                                  orderby user.UserName
                                  select new User
                                  {
                                      UserName = user.UserName,
                                      UserId = user.UserId,
                                      EmailAddress = user.EmailAddress
                                  }).ToList();
                    response.Items = result;
                    response.IsSuccessful = true;
                }
                else {
                    response.IsSuccessful = false;
                    response.Message = users.Message;
                }               
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching UnMappedUsers";

                m_Logger.LogError("Error occured while in GetUnMappedUsers() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region MapAssociateId
        /// <summary>
        /// Maps the UserId to the UserId in Employee
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<Entities.Employee>> MapAssociateId(EmployeeDetails employee)
        {
            Employee.Entities.Employee employeeDetails = m_EmployeeContext.Employees.
                                    Where(emp => emp.EmployeeId == employee.EmpId).FirstOrDefault();

            var response = new ServiceResponse<Entities.Employee>();
            try
            {
                if (employeeDetails != null)
                {
                    employeeDetails.UserId = employee.UserId;
                    employeeDetails.SystemInfo = employee.SystemInfo;
                    int mapped = await m_EmployeeContext.SaveChangesAsync();

                    if (mapped > 0)
                    {
                        response.Item = employeeDetails;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Unable to map Associate Id.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to map Associate Id.";

                m_Logger.LogError("Error occured in MapAssociateId() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion
    }
}
