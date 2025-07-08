using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Constants;
using HRMS.Project.Types.External;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HRMS.Project.Infrastructure.Models.Domain;
using System.Linq;
using System.Net.Http.Formatting;

namespace HRMS.Project.Service.External
{
    public class EmployeeService : IEmployeeService
    {
        #region Global Varaibles
        private readonly ILogger<EmployeeService> m_Logger;
        private IHttpClientFactory m_ClientFactory;
        private APIEndPoints m_ApiEndPoints;
        private readonly IOrganizationService m_OrgService;
        //private ICacheService m_CacheService;
        #endregion

        #region Constructor
        /// <summary>
        /// ProjectService
        /// <paramref name="apiEndPoints"/>
        /// <paramref name="clientFactory"/>
        /// <paramref name="logger"/>
        /// </summary>
        public EmployeeService(ILogger<EmployeeService> logger,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints,
            IOrganizationService orgService
            /*,ICacheService cacheService*/)
        {
            m_Logger = logger;
            m_ClientFactory = clientFactory;
            m_ApiEndPoints = apiEndPoints?.Value;
            m_OrgService = orgService;
            //m_CacheService = cacheService;
        }

        #endregion

        #region GetAllEmployees
        public async Task<ServiceListResponse<Employee>> GetAll(bool? isActive)
        {
            List<Employee> employeeList = null;
            var response = new ServiceListResponse<Employee>();
            try
            {
                //Get the Employees information from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETALL}" + isActive);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        employeeList = JsonConvert.DeserializeObject<List<Employee>>(json);
                        response.Items = employeeList;
                        if (employeeList.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No employees found";

                            m_Logger.LogError("No employees found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the employees data";

                        m_Logger.LogError("Error occured while fething the employees data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the employees data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion

        #region GetEmployeesByIds
        public async Task<ServiceListResponse<Employee>> GetEmployeesByIds(List<int> employeeIds)
        {
            List<Employee> employeeList = null;
            var response = new ServiceListResponse<Employee>();
            try
            {
                //Get the Employees information from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETEMPLOYEEBYIDS}" + string.Join(",", employeeIds));
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        employeeList = JsonConvert.DeserializeObject<List<Employee>>(json);
                        response.Items = employeeList;
                        if (employeeList.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else {
                            response.IsSuccessful = false;
                            response.Message = "No employees found";

                            m_Logger.LogError("No employees found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the employees data";

                        m_Logger.LogError("Error occured while fething the employees data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the employees data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion

        #region GetEmployeeById
        public async Task<ServiceResponse<Employee>> GetEmployeeById(int employeeId)
        {
            Employee employee = null;
            var response = new ServiceResponse<Employee>();
            try
            {
                //Get the Employee information from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETEMPLOYEEBYID}" + employeeId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        employee = JsonConvert.DeserializeObject<Employee>(json);
                        response.Item = employee;
                        if (employee != null)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No employee found";

                            m_Logger.LogError("No employee found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the employee data";

                        m_Logger.LogError("Error occured while fething the employee data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the employees data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }

        public async Task<ServiceResponse<Employee>> GetEmployeeByUserId(int userId)
        {
            Employee employee = null;
            var response = new ServiceResponse<Employee>();
            try
            {
                //Get the ProjectManagers information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETEMPLOYEEBYUSERID}" + userId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        employee = JsonConvert.DeserializeObject<Employee>(json);
                        response.Item = employee;
                        if (employee != null)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No employee found";

                            m_Logger.LogError("No employee found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the employee data";

                        m_Logger.LogError("Error occured while fething the employee data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the employees data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion

        #region GetEmployeeDetails
        public async Task<ServiceResponse<User>> GetEmployeeDetails(int employeeId)
        {
            var response = new ServiceResponse<User>();
            var users = await m_OrgService.GetUsers();
            if(users.Items != null)
            {
                var employee = users.Items.Where(x => x.EmployeeId == employeeId).FirstOrDefault();
                response.Item = employee;
                response.IsSuccessful = true;
            }

            return response;
        }
        #endregion

        #region GetEmployeeByUserName

        public async Task<ServiceListResponse<Employee>> GetEmployeeByUserName(string userName)
        {
            var response = new ServiceListResponse<Employee>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETEMPLOYEEBYUSERNAME}" + userName);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var employee = JsonConvert.DeserializeObject<List<Employee>>(json);
                        response.Items = employee;
                        if (employee != null)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No employee found";

                            m_Logger.LogError("No employee found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the employee data";

                        m_Logger.LogError("Error occured while fething the employee data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the employees data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion

        #region UpdateReportingManagerId
        public async Task<BaseServiceResponse> UpdateReportingManagerId(int employeeId, int reportingManagerId)
        {
            var response = new BaseServiceResponse();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;

                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.OrgEndPoint.UPDATEREPORTINGMANAGERID}" + employeeId +"/" + reportingManagerId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.PostAsync(httpRequestMessage.RequestUri.ToString(), "", new JsonMediaTypeFormatter());

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<BaseServiceResponse>(json);
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while updating the employee data";

                        m_Logger.LogError("Error occured while fething the employee data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while updating the employees data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region UpdateEmployee
        public async Task<BaseServiceResponse> UpdateEmployee(EmployeeExternal employeeDetails)
        {
            var response = new BaseServiceResponse();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;

                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.OrgEndPoint.UPDATEEMPLOYEE}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.PostAsync(httpRequestMessage.RequestUri.ToString(), employeeDetails, new JsonMediaTypeFormatter());

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<BaseServiceResponse>(json);
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while updating the employee data";

                        m_Logger.LogError("Error occured while fething the employee data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while updating the employees data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetAssociatesForDropdown
        public async Task<ServiceListResponse<GenericType>> GetAssociatesForDropdown()
        {
            List<GenericType> employeeList = null;
            var response = new ServiceListResponse<GenericType>();
            try
            {
                //Get the Employees information from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETASSOCIATESFORDROPDOWN}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        employeeList = JsonConvert.DeserializeObject<List<GenericType>>(json);
                        response.Items = employeeList;
                        if (employeeList.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No employees found";

                            m_Logger.LogError("No employees found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the employees data";

                        m_Logger.LogError("Error occured while fething the employees data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the employees data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion

        public async Task<ServiceResponse<Employee>> GetActiveEmployeeById(int employeeId)
        {
            Employee employee = null;
            var response = new ServiceResponse<Employee>();
            try
            {
                //Get the ProjectManagers information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETACTIVEEMPLOYEEBYID}" + employeeId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        employee = JsonConvert.DeserializeObject<Employee>(json);
                        response.Item = employee;
                        if (employee != null)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No employee found";

                            m_Logger.LogError("No employee found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the employee data";

                        m_Logger.LogError("Error occured while fething the employee data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the employees data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }

        public async Task<ServiceResponse<AssociateExit>> GetResignedAssociateByID(int employeeId)
        {
            AssociateExit associateExit = null;
            var response = new ServiceResponse<AssociateExit>();
            try
            {
                //Get the ProjectManagers information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GetResignedAssociateByID}" + employeeId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        associateExit = JsonConvert.DeserializeObject<AssociateExit>(json);
                        response.Item = associateExit;
                        if (associateExit != null)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No associateExit record found";

                            m_Logger.LogError("No associateExit record found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the associateExit data";

                        m_Logger.LogError("Error occured while fething the associateExit data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the associateExit data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }

        #region GetAllExitAssociates
        public async Task<ServiceListResponse<AssociateExit>> GetAllExitAssociates()
        {
            List<AssociateExit> employeeList = null;
            var response = new ServiceListResponse<AssociateExit>();
            try
            {
                //Get the Employees information from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETALLEXITASSOCIATES}" );
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        employeeList = JsonConvert.DeserializeObject<List<AssociateExit>>(json);
                        response.Items = employeeList;
                        if (employeeList.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No employees found";

                            m_Logger.LogError("No employees found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the employees data";

                        m_Logger.LogError("Error occured while fething the employees data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the employees data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion
    }
}
