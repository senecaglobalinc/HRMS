using HRMS.Common.Enums;
//using HRMS.Common.Redis;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Constants;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types.External;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Service.External
{
    public class OrganizationService : IOrganizationService
    {
        #region Global Variables
        private readonly ILogger<OrganizationService> m_Logger;
        private IHttpClientFactory m_ClientFactory;
        private APIEndPoints m_ApiEndPoints;
       // private ICacheService m_CacheService;
        #endregion

        #region Constructor
        /// <summary>
        /// ProjectService
        /// <paramref name="apiEndPoints"/>
        /// <paramref name="clientFactory"/>
        /// <paramref name="logger"/>
        /// </summary>
        public OrganizationService(ILogger<OrganizationService> logger,
        IHttpClientFactory clientFactory,
        IOptions<APIEndPoints> apiEndPoints
        //,ICacheService cacheService
            )
        {
            m_Logger = logger;
            m_ClientFactory = clientFactory;
            m_ApiEndPoints = apiEndPoints?.Value;
            //m_CacheService = cacheService;
        }

        #endregion

        #region GetUsers
        /// <summary>
        /// GetUsers
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<User>> GetUsers()
        {
            List<User> users = null;
            var response = new ServiceListResponse<User>();

            try
            {
                //Get the User information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                     $"{ServicePaths.OrgEndPoint.GETALLUSERS}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        users = JsonConvert.DeserializeObject<List<User>>(json);
                        response.Items = users;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the User data";
                        m_Logger.LogError("Error occured while fetching the User data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the User data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetActiveUsersById
        /// <summary>
        /// GetUsers
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<User>> GetActiveUsersById(int userId)
        {
            List<User> users = null;
            var response = new ServiceListResponse<User>();

            try
            {
                //Get the User information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                     $"{ServicePaths.OrgEndPoint.GETACTIVEUSERSBYID}{userId}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        users = JsonConvert.DeserializeObject<List<User>>(json);
                        response.Items = users;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the User data";
                        m_Logger.LogError("Error occured while fetching the User data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the User data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetAllDepartments

        /// <summary>
        /// GetAllDepartments
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<Department>> GetAllDepartments(bool nightlyJob =  false)
        {
            List<Department> departments = null;
            var response = new ServiceListResponse<Department>();

            try
            {
                //Get the Department information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;

                    if(nightlyJob)
                    {
                        httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETALLDEPARTMENTSFORJOB}");
                        httpRequestMessage.Headers.Add("x-Nightjob", "HRMSUtilizationReportConsole");
                    }
                    else
                    {
                        httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETALLDEPARTMENTS}");
                    }
                    
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        departments = JsonConvert.DeserializeObject<List<Department>>(json);
                        response.Items = departments;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the departments data";
                        m_Logger.LogError("Error occured while fetching the departments data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the departments data";

                m_Logger.LogError(ex.Message);
            }
            return response;

        }
        #endregion

        #region GetDepartmentByCode
        /// <summary>
        /// GetDepartmentByCode
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<Department>> GetDepartmentByCode(string departmentCode)
        {
            Department department = null;
            var response = new ServiceResponse<Department>();

            try
            {
                //Get the Department information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETDEPARTMENTBYCODE}{departmentCode}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        department = JsonConvert.DeserializeObject<Department>(json);
                        response.Item = department;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the department data";
                        m_Logger.LogError("Error occured while fetching the department data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the department data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetDepartmentByCodes
        /// <summary>
        /// GetDepartmentByCodes
        /// </summary>
        /// <param name="departmentCodes"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<Department>> GetDepartmentByCodes(List<string> departmentCodes)
        {
            List<Department> departments = null;
            var response = new ServiceListResponse<Department>();

            try
            {
                //Get the Department information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETDEPARTMENTBYCODES}" + string.Join(",", departmentCodes));
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        departments = JsonConvert.DeserializeObject<List<Department>>(json);
                        response.Items = departments;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the department data";
                        m_Logger.LogError("Error occured while fetching the department data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the department data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetDesignationById
        /// <summary>
        /// Get designation details by Id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<Designation>> GetDesignationById(int designationId)
        {
            Designation designation = null;
            var response = new ServiceResponse<Designation>();

            try
            {
                //Get the Designation information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETDESIGNATIONBYID}{designationId}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        designation = JsonConvert.DeserializeObject<Designation>(json);
                        response.Item = designation;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the designation data";
                        m_Logger.LogError("Error occured while fetching the designation data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the designation data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetDesignationByCode
        /// <summary>
        /// Get designation details by Id
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<Designation>> GetDesignationByCode(string designationCode)
        {
            Designation designation = null;
            var response = new ServiceResponse<Designation>();

            try
            {
                //Get the Designation information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETDESIGNATIONBYCODE}{designationCode}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        designation = JsonConvert.DeserializeObject<Designation>(json);
                        response.Item = designation;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the designation data";
                        m_Logger.LogError("Error occured while fetching the designation data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the designation data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetAllDesignations
        /// <summary>
        /// GetAllDesignations
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<Designation>> GetAllDesignations()
        {
            List<Designation> designations = null;
            var response = new ServiceListResponse<Designation>();

            try
            {
                //Get the Designation information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETALLDESIGNATIONS}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        designations = JsonConvert.DeserializeObject<List<Designation>>(json);
                        response.Items = designations;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Designation data";
                        m_Logger.LogError("Error occured while fetching the Designation data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Designation data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetDepartmentById
        /// <summary>
        /// Get department details by Id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<Department>> GetDepartmentById(int departmentId)
        {
            Department department = null;
            var response = new ServiceResponse<Department>();

            try
            {
                //Get the Department information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETDEPARTMENTBYID}{departmentId}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        department = JsonConvert.DeserializeObject<Department>(json);
                        response.Item = department;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the department data";
                        m_Logger.LogError("Error occured while fetching the department data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the department data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetAllStatuses

        /// <summary>
        /// GetAllStatuses
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<Status>> GetAllStatuses()
        {
            List<Status> statuses = null;
            var response = new ServiceListResponse<Status>();

            try
            {
                //Get the status information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETALLSTATUSES}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        statuses = JsonConvert.DeserializeObject<List<Status>>(json);
                        response.Items = statuses;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the status data";
                        m_Logger.LogError("Error occured while fetching the status data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the status data";

                m_Logger.LogError(ex.Message);
            }
            return response;

        }
        #endregion

        #region GetAllPracticeAreas
        /// <summary>
        /// GetAllPracticeAreas
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<PracticeArea>> GetAllPracticeAreas(bool nightlyJob = false)
        {
            List<PracticeArea> practiceAreas = null;
            var response = new ServiceListResponse<PracticeArea>();

            try
            {
                //Get the PracticeArea information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;

                    if (nightlyJob)
                    {
                        httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETALLPRACTICEAREASFORJOB}");
                        httpRequestMessage.Headers.Add("x-Nightjob", "HRMSUtilizationReportConsole");
                    }
                    else
                    {
                        httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETALLPRACTICEAREAS}");
                    }
                    
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        practiceAreas = JsonConvert.DeserializeObject<List<PracticeArea>>(json);
                        response.Items = practiceAreas;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the PracticeArea data";
                        m_Logger.LogError("Error occured while fetching the PracticeArea data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the PracticeArea data";

                m_Logger.LogError(ex.Message);
            }
            return response;

        }

        #endregion

        #region GetPracticeAreaByCode
        /// <summary>
        /// GetPracticeAreaByCode
        /// </summary>
        /// <param name="GetPracticeAreaByCode"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<PracticeArea>> GetPracticeAreaByCode(string practiceAreaCode)
        {
            PracticeArea practiceArea = null;
            var response = new ServiceResponse<PracticeArea>();

            try
            {
                //Get the practiceArea information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETPRACTICEAREABYCODE}{practiceAreaCode}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        practiceArea = JsonConvert.DeserializeObject<PracticeArea>(json);
                        response.Item = practiceArea;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the practiceArea data";
                        m_Logger.LogError("Error occured while fetching the practiceArea data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the practiceArea data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetAllGrades
        /// <summary>
        /// GetAllGrades
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<Grade>> GetAllGrades()
        {
            List<Grade> grades = null;
            var response = new ServiceListResponse<Grade>();

            try
            {
                //Get the Grade information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETALLGRADES}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        grades = JsonConvert.DeserializeObject<List<Grade>>(json);
                        response.Items = grades;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Grade data";
                        m_Logger.LogError("Error occured while fetching the Grade data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Grade data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }

        #endregion

        #region GetAllDomains
        /// <summary>
        /// GetAllDomains
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<Domain>> GetAllDomains()
        {
            List<Domain> domains = null;
            var response = new ServiceListResponse<Domain>();

            try
            {
                //Get the Grade information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETALLDOMAINS}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        domains = JsonConvert.DeserializeObject<List<Domain>>(json);
                        response.Items = domains;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Domain data";
                        m_Logger.LogError("Error occured while fetching the Domain data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Domain data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }

        #endregion

        #region GetAllUserRoles
        /// <summary>
        /// GetAllUserRoles
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<UserRole>> GetAllUserRoles()
        {
            List<UserRole> userRoles = null;
            var response = new ServiceListResponse<UserRole>();

            try
            {
                //Get the UserRole information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETALLUSERROLES}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        userRoles = JsonConvert.DeserializeObject<List<UserRole>>(json);
                        response.Items = userRoles;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the UserRole data";
                        m_Logger.LogError("Error occured while fetching the UserRole data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the UserRole data";

                m_Logger.LogError(ex.Message);
            }
            return response;

        }
        #endregion

        #region GetRoleTypesForDropdown

        /// <summary>
        /// GetRoleTypesForDropdown
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<GenericType>> GetRoleTypesForDropdown()
        {
            List<GenericType> roles = null;
            var response = new ServiceListResponse<GenericType>();

            try
            {
                //Get the RoleType information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETROLETYPESFORDROPDOWN}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        roles = JsonConvert.DeserializeObject<List<GenericType>>(json);
                        response.Items = roles;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the RoleType data";
                        m_Logger.LogError("Error occured while fetching the RoleType data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the RoleType data";

                m_Logger.LogError(ex.Message);
            }
            return response;

        }
        #endregion

        #region GetAllFinancialYears
        /// <summary>
        /// GetFinancialYears
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<FinancialYear>> GetAllFinancialYearsAsync()
        {
            var response = new ServiceListResponse<FinancialYear>();

            try
            {
                //Get the Financial year information from Org Microservice
                using var httpClientFactory = m_ClientFactory.CreateClient("OrgClient");
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                 $"{ServicePaths.OrgEndPoint.GetAllFinancialYears}")
                };

                httpRequestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    var financialYears = JsonConvert.DeserializeObject<List<FinancialYear>>(json);
                    response.Items = financialYears;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occured while fetching the Financial Year data";
                    m_Logger.LogError($"{nameof(GetAllFinancialYearsAsync)} - Error occured while fetching the Financial Year data.");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Financial Year data";

                m_Logger.LogError($"{nameof(GetAllFinancialYearsAsync)} - {ex?.Message}");
            }
            return response;
        }
        #endregion

        #region GetAllRoles

        /// <summary>
        /// GetAllRoles
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<Role>> GetAllRoles()
        {
            List<Role> roles = null;
            var response = new ServiceListResponse<Role>();

            try
            {
                //Get the Role information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETALLROLES}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        roles = JsonConvert.DeserializeObject<List<Role>>(json);
                        response.Items = roles;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Role data";
                        m_Logger.LogError("Error occured while fetching the Role data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Role data";

                m_Logger.LogError(ex.Message);
            }
            return response;

        }
        #endregion

        #region GetAllRoleTypes

        /// <summary>
        /// GetAllRoleTypes
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<RoleType>> GetAllRoleTypes()
        {
            List<RoleType> roles = null;
            var response = new ServiceListResponse<RoleType>();

            try
            {
                //Get the RoleType information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient())
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETALLROLETYPES}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        roles = JsonConvert.DeserializeObject<List<RoleType>>(json);
                        response.Items = roles;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the RoleType data";
                        m_Logger.LogError("Error occured while fetching the RoleType data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the RoleType data";

                m_Logger.LogError(ex.Message);
            }
            return response;

        }
        #endregion

        #region GetRoleTypeById
        /// <summary>
        /// GetRoleTypeById
        /// </summary>
        /// <param name="roleTypeId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<RoleType>> GetRoleTypeById(int roleTypeId)
        {
            RoleType roleType = null;
            var response = new ServiceResponse<RoleType>();

            try
            {
                //Get the RoleType information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient())
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETROLETYPEBYID}{roleTypeId}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        roleType = JsonConvert.DeserializeObject<RoleType>(json);
                        response.Item = roleType;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the RoleType data";
                        m_Logger.LogError("Error occured while fetching the RoleType data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the RoleType data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetStatusById
        /// <summary>
        /// GetStatusById
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<Status>> GetStatusById(int statusId)
        {
            Status status = null;
            var response = new ServiceResponse<Status>();

            try
            {
                //Get the Role information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETSTATUSBYID}{statusId}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        status = JsonConvert.DeserializeObject<Status>(json);
                        response.Item = status;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Status data";
                        m_Logger.LogError("Error occured while fetching the Status data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Status data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetStatusByCode
        /// <summary>
        /// GetStatusByCode
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<Status>> GetStatusByCode(string statusCode)
        {
            Status status = null;
            var response = new ServiceResponse<Status>();

            try
            {
                //Get the Role information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETSTATUSBYCODE}{statusCode}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        status = JsonConvert.DeserializeObject<Status>(json);
                        response.Item = status;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Status data";
                        m_Logger.LogError("Error occured while fetching the Status data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Status data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetStatusByCategoryAndStatusCode
        /// <summary>
        /// Get Status By CategoryAndStatusCode
        /// </summary>
        /// <param name="category"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<Status>> GetStatusByCategoryAndStatusCode(string category, string statusCode)
        {
            Status status = null;
            var response = new ServiceResponse<Status>();
            try
            {
                //Get the Role information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETSTATUSBYCATEGORYANDSTATUSCODE}{category}{'/'}{statusCode}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        status = JsonConvert.DeserializeObject<Status>(json);
                        response.Item = status;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Status data";
                        m_Logger.LogError("Error occured while fetching the Status data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Status data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetSkillsBySkillGroupId
        /// <summary>
        /// Get All Skills
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<Skill>> GetSkillsBySkillGroupId(List<int> certificationSkillGroupIds)
        {
            List<Skill> skill = null;
            var response = new ServiceListResponse<Skill>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETSKILLSBYSKILLGROUPID}" + string.Join(",", certificationSkillGroupIds));
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        skill = JsonConvert.DeserializeObject<List<Skill>>(json);
                        response.Items = skill;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Skill data";
                        m_Logger.LogError("Error occured while fetching the Skill data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Skill data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetRoleByRoleName
        /// <summary>
        /// Get Role By RoleName
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<Role>> GetRoleByRoleName(string roleName)
        {
            Role role = null;
            var response = new ServiceResponse<Role>();

            try
            {
                //Get the Role information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETROLEBYROLENAME}{roleName}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        role = JsonConvert.DeserializeObject<Role>(json);
                        response.Item = role;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the role data";
                        m_Logger.LogError("Error occured while fetching the role data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the role data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetSkillsBySkillId
        /// <summary>
        /// Get Skills By SkillId
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<Skill>> GetSkillsBySkillId(List<int> skillIds)
        {
            List<Skill> skill = null;
            var response = new ServiceListResponse<Skill>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETSKILLSBYSKILLID}" + string.Join(",", skillIds));
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        skill = JsonConvert.DeserializeObject<List<Skill>>(json);
                        response.Items = skill;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Skill data";
                        m_Logger.LogError("Error occured while fetching the Skill data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Skill data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetNotificationByNotificationTypeAndCategory
        public async Task<ServiceResponse<NotificationConfiguration>> GetByNotificationTypeAndCategory(int notificationTypeId, int categoryMasterId)
        {
            NotificationConfiguration notificationConfiguration = null;
            var response = new ServiceResponse<NotificationConfiguration>();
            try
            {
                //Get the NoificationConfiguration information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETNOTIFICATIONBYNOTIFICATIONTYPEANDCATEGORY}{notificationTypeId}{"&categoryMasterId="}{categoryMasterId}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        notificationConfiguration = JsonConvert.DeserializeObject<NotificationConfiguration>(json);
                        response.Item = notificationConfiguration;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the NoificationConfiguration data";
                        m_Logger.LogError("Error occured while fetching the NoificationConfiguration data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Status data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetProficiencyLevelsByProficiencyLevelId
        /// <summary>
        /// Get ProficiencyLevels By ProficiencyLevelId
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProficiencyLevel>> GetProficiencyLevelsByProficiencyLevelId(List<int> proficiencyLevelIds)
        {
            List<ProficiencyLevel> proficiencyLevels = null;
            var response = new ServiceListResponse<ProficiencyLevel>();

            try
            {
                //Get the proficiency information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETPROFICIENCEYLEVELSBYPROFICIENCEYLEVELID}" + string.Join(",", proficiencyLevelIds));
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        proficiencyLevels = JsonConvert.DeserializeObject<List<ProficiencyLevel>>(json);
                        response.Items = proficiencyLevels;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the proficiency level data";
                        m_Logger.LogError("Error occured while fetching the proficiency level data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the proficiency level data";

                m_Logger.LogError(ex.Message);
            }
            return response;

        }
        #endregion

        #region GetAllSkills
        /// <summary>
        /// GetAllSkills
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<Skill>> GetAllSkills(bool isActive)
        {
            List<Skill> skills = null;
            var response = new ServiceListResponse<Skill>();

            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETALLSKILLS}" + isActive);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        skills = JsonConvert.DeserializeObject<List<Skill>>(json);
                        response.Items = skills;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Skills data";
                        m_Logger.LogError("Error occured while fetching the Skills data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Skills data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region Proficiency levels
        /// <summary>
        /// Gets all Proficiency levels
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProficiencyLevel>> GetAllProficiencyLevels(bool isActive)
        {
            List<ProficiencyLevel> proficiencyLevels = null;
            var response = new ServiceListResponse<ProficiencyLevel>();

            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETPROFICIENCEYLEVELS}" + isActive);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        proficiencyLevels = JsonConvert.DeserializeObject<List<ProficiencyLevel>>(json);
                        response.Items = proficiencyLevels;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the ProficiencyLevel data";
                        m_Logger.LogError("Error occured while fetching the ProficiencyLevel data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the ProficiencyLevel data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region CompetencyAreas
        /// <summary>
        /// CompetencyAreas
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<CompetencyArea>> GetCompetencyAreas(bool isActive)
        {
            List<CompetencyArea> competencyArea = null;
            var response = new ServiceListResponse<CompetencyArea>();

            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETCOMPETENCYAREAS}" + isActive);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        competencyArea = JsonConvert.DeserializeObject<List<CompetencyArea>>(json);
                        response.Items = competencyArea;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the CompetencyArea data";
                        m_Logger.LogError("Error occured while fetching the CompetencyArea data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the CompetencyArea data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region SendEmail
        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> SendEmail(NotificationDetail notificationDetail)
        {
            var response = new ServiceResponse<bool>();

            response.Item = true;
            response.IsSuccessful = true;            
            try
            {
                //send email from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {

                    var uri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.SENDEMAIL}");
                    var httpResponse = await httpClientFactory.PostAsync(uri, notificationDetail, new JsonMediaTypeFormatter());
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        string json = await httpResponse.Content.ReadAsStringAsync();
                        response.Item = true;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while sending email";
                        m_Logger.LogError("Error occured while sending email");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while sending email";
                m_Logger.LogError(ex.Message);
            }
            return response; 
        }
        #endregion

        #region GetStatusesByCategoryName
        public async Task<ServiceListResponse<Status>> GetStatusesByCategoryName(string categoryName)
        {
            var response = new ServiceListResponse<Status>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETSTATUSESBYCATEGORYNAME}" + categoryName);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var statuses = JsonConvert.DeserializeObject<List<Status>>(json.ToString());
                        if (statuses == null || statuses.Count == 0)
                        {
                            response.Items = null;
                            response.IsSuccessful = false;
                            response.Message = "No Statues found..";
                        }
                        else
                        {
                            response.Items = statuses;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the status";

                        m_Logger.LogError("Error occured while fetching the status");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the status";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetClosureActivitiesByDepartment
        public async Task<ServiceListResponse<Activity>> GetExitActivitiesByDepartment(int? departmentId = null)
        {
            var response = new ServiceListResponse<Activity>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETEXITACTIVITIESBYDEPARTMENT}" + departmentId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var activity = JsonConvert.DeserializeObject<List<Activity>>(json.ToString());
                        if (activity == null)
                        {
                            response.Items = null;
                            response.IsSuccessful = false;
                            response.Message = "Activities not found..";
                        }
                        else
                        {
                            response.Items = activity;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Activities";

                        m_Logger.LogError("Error occured while fetching the Activities");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Domain";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetExitTypeById
        public async Task<ServiceResponse<ExitType>> GetExitTypeById(int exitTypeId)
        {
            var response = new ServiceResponse<ExitType>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETEXITTYPEBYID}/" + exitTypeId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var activity = JsonConvert.DeserializeObject<ExitType>(json.ToString());
                        if (activity == null)
                        {
                            response.Item = null;
                            response.IsSuccessful = false;
                            response.Message = "Exit Type not found..";
                        }
                        else
                        {
                            response.Item = activity;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Item = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Exit Type";

                        m_Logger.LogError("Error occured while fetching the Exit Type");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the ExitType";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetAllExitTypes
        /// <summary>
        /// GetAllExitTypes
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<GenericType>> GetAllExitTypes()
        {
            List<GenericType> userRoles = null;
            var response = new ServiceListResponse<GenericType>();

            try
            {
                //Get the UserRole information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETALLEXITTYPES}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        userRoles = JsonConvert.DeserializeObject<List<GenericType>>(json);
                        response.Items = userRoles;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the ExitTypes data";
                        m_Logger.LogError("Error occured while fetching the ExitTypes data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the ExitTypes data";

                m_Logger.LogError(ex.Message);
            }
            return response;

        }
        #endregion

        #region GetAllExitReasons
        /// <summary>
        /// GetAllExitReasons
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<GenericType>> GetAllExitReasons()
        {
            List<GenericType> userRoles = null;
            var response = new ServiceListResponse<GenericType>();

            try
            {
                //Get the UserRole information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETALLEXITREASONS}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        userRoles = JsonConvert.DeserializeObject<List<GenericType>>(json);
                        response.Items = userRoles;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the ExitReasons data";
                        m_Logger.LogError("Error occured while fetching the ExitReasons data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the ExitReasons data";

                m_Logger.LogError(ex.Message);
            }
            return response;

        }
        #endregion

        #region UpdateUsers
        /// <summary>
        /// UpdateUsers
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUser(int userId)
        {
            var response = true;
            try
            {
                //Get the User information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                     $"{ServicePaths.OrgEndPoint.UPDATEUSER + userId}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        response = true;
                    }
                    else
                    {
                        m_Logger.LogError("Error occured while updating the User data.");
                        response = false;
                    }
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
            }
            return response;
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
            var response = true;
            try
            {
                //Get the User information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint + $"{ServicePaths.OrgEndPoint.REMOVEUSERROLEONEXIT + userId}")
                    };
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        response = true;
                    }
                    else
                    {
                        m_Logger.LogError("Error occured while updating the User data.");
                        response = false;
                    }
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetDepartmentDLByDeptId
        /// <summary>
        /// Get department dl by dept Id
        /// </summary>
        /// <param name="deptIds"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<DepartmentDL>> GetDepartmentDLByDeptId(int deptId)
        {
            var response = new ServiceResponse<DepartmentDL>();

            try
            {
                //Get the Department information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                       $"{ServicePaths.OrgEndPoint.GETDEPARTMENTDLBYDEPTID}{deptId}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        DepartmentDL departmentdl = JsonConvert.DeserializeObject<DepartmentDL>(json);
                        response.Item = departmentdl;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the department dl data";
                        m_Logger.LogError("Error occured while fetching the department dl data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the department dl data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetUsersByRoles
        /// <summary>
        /// Get Users By Roles
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeRoleDetails>> GetUsersByRoles(string roles)
        {
            List<EmployeeRoleDetails> userIds = new List<EmployeeRoleDetails>();
            var response = new ServiceListResponse<EmployeeRoleDetails>();

            try
            {
                //Get the Department information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETUSERBYROLES}{roles}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        userIds = JsonConvert.DeserializeObject<List<EmployeeRoleDetails>>(json);
                        response.Items = userIds;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the UserIds from UserRoles table by roles data";
                        m_Logger.LogError("Error occured while fetching the UserIds from UserRoles table by roles data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the UserIds from UserRoles table by roles data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetAllDepartmentsWithDLs
        /// <summary>
        /// GetAllDepartmentsWithDLs
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<DepartmentWithDLAddress>> GetAllDepartmentsWithDLs()
        {
            var response = new ServiceListResponse<DepartmentWithDLAddress>();

            try
            {
                //Get the User information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                     $"{ServicePaths.OrgEndPoint.GETALLDEPARTMENTSWITHDLS}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        List<DepartmentWithDLAddress> departmentWithDLAddresses = JsonConvert.DeserializeObject<List<DepartmentWithDLAddress>>(json);
                        response.Items = departmentWithDLAddresses;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching department distribution lists";
                        m_Logger.LogError("Error occured while fetching department distribution lists.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching department distribution lists";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetAllHolidays
        /// <summary>
        /// GetAllHolidays
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<Holiday>> GetAllHolidays()
        {
            List<Holiday> holidays = null;
            var response = new ServiceListResponse<Holiday>();

            try
            {
                //Get the Grade information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETALLHOLIDAYS}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        holidays = JsonConvert.DeserializeObject<List<Holiday>>(json);
                        response.Items = holidays;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Holiday data";
                        m_Logger.LogError("Error occured while fetching the Holiday data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Holiday data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }

        #endregion

        #region GetClientById
        /// <summary>
        /// GetClientById
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<Client>> GetClientById(int clientId)
        {
            Client status = null;
            var response = new ServiceResponse<Client>();

            try
            {
                //Get the Role information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETCLIENTBYID}{clientId}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        status = JsonConvert.DeserializeObject<Client>(json);
                        response.Item = status;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Client data";
                        m_Logger.LogError("Error occured while fetching the Client data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Client data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetAllClients
        /// <summary>
        /// GetAllClients
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<Client>> GetAllClients()
        {
            List<Client> clients = null;
            var response = new ServiceListResponse<Client>();

            try
            {
                //Get the Role information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETAllCLIENTS}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        clients = JsonConvert.DeserializeObject<List<Client>>(json);
                        response.Items = clients;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Client data";
                        m_Logger.LogError("Error occured while fetching the Client data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Client data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetAllRoleMaster
        /// <summary>
        /// GetAllRoleMaster
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<RoleMasterDetails>> GetRoleMasterDetails()
        {
            List<RoleMasterDetails> roleMaster = null;
            var response = new ServiceListResponse<RoleMasterDetails>();

            try
            {
                //Get the Role information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETAllROLEMASTER}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        roleMaster = JsonConvert.DeserializeObject<List<RoleMasterDetails>>(json);
                        response.Items = roleMaster;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the RoleMaster data";
                        m_Logger.LogError("Error occured while fetching the RoleMaster data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the RoleMaster data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

    }
}
