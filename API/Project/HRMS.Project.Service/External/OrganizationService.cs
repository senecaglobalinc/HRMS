using HRMS.Common.Enums;
using HRMS.Common.Redis;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Constants;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types.External;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using NotificationType = HRMS.Project.Infrastructure.Models.Domain.NotificationType;

namespace HRMS.Project.Service.External
{
    public class OrganizationService : IOrganizationService
    {
        #region Global Varaibles
        private readonly ILogger<OrganizationService> m_Logger;
        private IHttpClientFactory m_ClientFactory;
        private APIEndPoints m_ApiEndPoints;
        //private ICacheService m_CacheService;
        #endregion

        #region Constructor
       /// <summary>
       /// 
       /// </summary>
       /// <param name="logger"></param>
       /// <param name="clientFactory"></param>
       /// <param name="apiEndPoints"></param>
        public OrganizationService(ILogger<OrganizationService> logger,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints
            /*,ICacheService cacheService*/)
        {
            m_Logger = logger;
            m_ClientFactory = clientFactory;
            m_ApiEndPoints = apiEndPoints?.Value;
            //m_CacheService = cacheService;
        }

        #endregion

        #region GetStatusByCategoryAndStatusCode
        public async Task<ServiceResponse<Status>> GetStatusByCategoryAndStatusCode(string categoryName, string statusCode)
        {
            //Status status = new Status();
            var response = new ServiceResponse<Status>();
            try
            {
                //Get the ProjectManagers information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETSTATUSBYCATEGORYANDSTATUSCODE}{categoryName}/{statusCode}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var status = JsonConvert.DeserializeObject<Status>(json);
                        response.Item = status;
                        response.IsSuccessful = true;
                    }
                    else
                    {
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

        #region GetStatusesByCategoryName
        public async Task<ServiceListResponse<Status>> GetStatusesByCategoryName(string categoryName, bool nightlyJob = false)
        {
            var response = new ServiceListResponse<Status>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    if (nightlyJob)
                    {
                        httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETSTATUSESBYCATEGORY}" + categoryName);

                        httpRequestMessage.Headers.Add("x-Nightjob", "HRMSReportsConsole");
                    }
                    else
                    {
                        httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETSTATUSESBYCATEGORYNAME}" + categoryName);
                    }
                    
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

        #region GetUserById
        public async Task<ServiceResponse<User>> GetUserById(int userId)
        {
            var response = new ServiceResponse<User>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETUSERBYID}" + userId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<User>(json.ToString());
                        if (user == null)
                        {
                            response.Item = null;
                            response.IsSuccessful = false;
                            response.Message = "User not found..";
                        }
                        else
                        {
                            response.Item = user;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Item = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the user";

                        m_Logger.LogError("Error occured while fetching the user");
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

        #region GetClients
        public async Task<ServiceListResponse<Client>> GetClients()
        {
            var response = new ServiceListResponse<Client>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;                    
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETCLIENTS}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var clients = JsonConvert.DeserializeObject<List<Client>>(json.ToString());
                        if (clients == null || clients.Count == 0)
                        {
                            response.Items = null;
                            response.IsSuccessful = false;
                            response.Message = "Client not found..";
                        }
                        else
                        {
                            response.Items = clients;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Clients";

                        m_Logger.LogError("Error occured while fetching the Clients");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Clients";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetClientsByIds
        public async Task<ServiceListResponse<Client>> GetClientsByIds(List<int> clientIds)
        {
            var response = new ServiceListResponse<Client>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    string sclientIds = "";
                    foreach (int clientId in clientIds) {
                        sclientIds += ((sclientIds != "") ? ("&clientIds=" + clientId) : ""+clientId);
                    }
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETCLIENTBYIDS}"+ sclientIds);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                   HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);
                   
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var clients = JsonConvert.DeserializeObject<List<Client>>(json.ToString());
                        if (clients == null || clients.Count ==0)
                        {
                            response.Items = null;
                            response.IsSuccessful = false;
                            response.Message = "Client not found..";
                        }
                        else
                        {
                            response.Items = clients ;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Clients";

                        m_Logger.LogError("Error occured while fetching the Clients");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Clients";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetClientById
        public async Task<ServiceResponse<Client>> GetClientById(int clientId)
        {
            var response = new ServiceResponse<Client>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETCLIENTBYID}" + clientId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var client = JsonConvert.DeserializeObject<Client>(json.ToString());
                        if (client == null)
                        {
                            response.Item = null;
                            response.IsSuccessful = false;
                            response.Message = "Client not found..";
                        }
                        else
                        {
                            response.Item = client;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Item = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Client";

                        m_Logger.LogError("Error occured while fetching the Client");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Client";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetPracticeAreasByIds
        public async Task<ServiceListResponse<PracticeArea>> GetPracticeAreasByIds(List<int> practiceAreaIds)
        {
            var response = new ServiceListResponse<PracticeArea>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    string spracticeAreaIds = "";
                    foreach (int practiceAreaId in practiceAreaIds)
                    {
                        spracticeAreaIds += ((spracticeAreaIds != "") ? ("&practiceAreaIds=" + practiceAreaId) : "" + practiceAreaId);
                    }
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETPRACTICEAREABYIDS}" + spracticeAreaIds);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var practiceAreas = JsonConvert.DeserializeObject<List<PracticeArea>>(json.ToString());
                        if (practiceAreas == null || practiceAreas.Count == 0)
                        {
                            response.Items = null;
                            response.IsSuccessful = false;
                            response.Message = "Practice Area not found..";
                        }
                        else
                        {
                            response.Items = practiceAreas;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Practice Area";

                        m_Logger.LogError("Error occured while fetching the Practice Area");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Practice Area";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetPracticeAreaById
        public async Task<ServiceListResponse<PracticeArea>> GetAllPracticeArea(bool isActive)
        {
            var response = new ServiceListResponse<PracticeArea>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETALLPRACTICEAREA}" + isActive);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var practiceArea = JsonConvert.DeserializeObject<List<PracticeArea>>(json.ToString());
                        if (practiceArea == null)
                        {
                            response.Items = null;
                            response.IsSuccessful = false;
                            response.Message = "Practice Area not found..";
                        }
                        else
                        {
                            response.Items = practiceArea;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Practice Area";

                        m_Logger.LogError("Error occured while fetching the Practice Area");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Practice Area";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetPracticeAreaById
        public async Task<ServiceResponse<PracticeArea>> GetPracticeAreaById(int practiceAreaId)
        {
            var response = new ServiceResponse<PracticeArea>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETPRACTICEAREABYID}" + practiceAreaId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var practiceArea = JsonConvert.DeserializeObject<PracticeArea>(json.ToString());
                        if (practiceArea == null)
                        {
                            response.Item = null;
                            response.IsSuccessful = false;
                            response.Message = "Practice Area not found..";
                        }
                        else
                        {
                            response.Item = practiceArea;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Item = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Practice Area";

                        m_Logger.LogError("Error occured while fetching the Practice Area");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Practice Area";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetProjectTypesByIds
        public async Task<ServiceListResponse<ProjectType>> GetProjectTypesByIds(List<int> projectTypeIds)
        {
            var response = new ServiceListResponse<ProjectType>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    string sprojectTypeIds = "";
                    foreach (int projectTypeId in projectTypeIds)
                    {
                        sprojectTypeIds += ((sprojectTypeIds != "") ? ("&projectTypeIds=" + projectTypeId) : "" + projectTypeId);
                    }
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETPROJECTTYPEBYIDS}" + sprojectTypeIds);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var projectTypes = JsonConvert.DeserializeObject<List<ProjectType>>(json.ToString());
                        if (projectTypes == null || projectTypes.Count == 0)
                        {
                            response.Items = null;
                            response.IsSuccessful = false;
                            response.Message = "Project Types not found..";
                        }
                        else
                        {
                            response.Items = projectTypes;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Project Types";

                        m_Logger.LogError("Error occured while fetching the Project Types");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Project Types";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetProjectTypeById
        public async Task<ServiceResponse<ProjectType>> GetProjectTypeById(int projectTypeId)
        {
            var response = new ServiceResponse<ProjectType>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETPROJECTTYPEBYID}" + projectTypeId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var projectType = JsonConvert.DeserializeObject<ProjectType>(json.ToString());
                        if (projectType == null)
                        {
                            response.Item = null;
                            response.IsSuccessful = false;
                            response.Message = "Project Type not found..";
                        }
                        else
                        {
                            response.Item = projectType;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Item = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Project Type";

                        m_Logger.LogError("Error occured while fetching the Project Type");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Project Type";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetAllProjectTypes
        public async Task<ServiceListResponse<ProjectType>> GetAllProjectTypes(bool isActive)
        {           
            var response = new ServiceListResponse<ProjectType>();
            try
            {                
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETALLPROJECTTYPES}" + isActive);
                    httpRequestMessage.Headers.Add("Accept", "application/json");                    
                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var projectType = JsonConvert.DeserializeObject<List<ProjectType>>(json.ToString());
                        if (projectType == null)
                        {
                            response.Items = null;
                            response.IsSuccessful = false;
                            response.Message = "Project Type not found..";
                        }
                        else
                        {
                            response.Items = projectType;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Project Type";

                        m_Logger.LogError("Error occured while fetching the Project Type");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Project Type";

                m_Logger.LogError(ex.StackTrace);
            }

            return response;
        }
        #endregion

        #region GetDomainById
        public async Task<ServiceResponse<Domain>> GetDomainById(int domainId)
        {
            var response = new ServiceResponse<Domain>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETDOMAINBYID}" + domainId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var domain = JsonConvert.DeserializeObject<Domain>(json.ToString());
                        if (domain == null)
                        {
                            response.Item = null;
                            response.IsSuccessful = false;
                            response.Message = "Domain not found..";
                        }
                        else
                        {
                            response.Item = domain;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Item = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Domain";

                        m_Logger.LogError("Error occured while fetching the Domain");
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

        #region GetAllDepartment
        public async Task<ServiceListResponse<Department>> GetAllDepartment(bool isActive)
        {
            var response = new ServiceListResponse<Department>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GetAllDepartment}" + isActive);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var dept = JsonConvert.DeserializeObject<List<Department>>(json.ToString());
                        if (dept == null)
                        {
                            response.Items = null;
                            response.IsSuccessful = false;
                            response.Message = "Department not found..";
                        }
                        else
                        {
                            response.Items = dept;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Department";

                        m_Logger.LogError("Error occured while fetching the Department");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Department";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetDepartmentById
        public async Task<ServiceResponse<Department>> GetDepartmentById(int departmentId)
        {
            var response = new ServiceResponse<Department>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETDEPARTMENTBYID}" + departmentId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var dept = JsonConvert.DeserializeObject<Department>(json.ToString());
                        if (dept == null)
                        {
                            response.Item = null;
                            response.IsSuccessful = false;
                            response.Message = "Department not found..";
                        }
                        else
                        {
                            response.Item = dept;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Item = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Department";

                        m_Logger.LogError("Error occured while fetching the Department");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Department";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetDepartmentByCode
        public async Task<ServiceResponse<Department>> GetDepartmentByCode(string departmentCode)
        {
            var response = new ServiceResponse<Department>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETDEPARTMENTBYCODE}" + departmentCode);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var dept = JsonConvert.DeserializeObject<Department>(json.ToString());
                        if (dept == null)
                        {
                            response.Item = null;
                            response.IsSuccessful = false;
                            response.Message = "Department not found..";
                        }
                        else
                        {
                            response.Item = dept;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Item = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Department";

                        m_Logger.LogError("Error occured while fetching the Department");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Department";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetCategoryByName
        public async Task<ServiceResponse<Category>> GetCategoryByName(string categoryName)
        {
            var response = new ServiceResponse<Category>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETCATEGORYBYNAME}" + categoryName); ;
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var category = JsonConvert.DeserializeObject<Category>(json.ToString());
                        if (category == null)
                        {
                            response.Item = null;
                            response.IsSuccessful = false;
                            response.Message = "No Category found..";
                        }
                        else
                        {
                            response.Item = category;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Item = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Category";

                        m_Logger.LogError("Error occured while fetching the Category");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Category";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetUserByEmail
        public async Task<ServiceResponse<User>> GetUserByEmail(string email)
        {
            var response = new ServiceResponse<User>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETUSERBYEMAIL}" + email);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<User>(json.ToString());
                        if (user == null)
                        {
                            response.Item = null;
                            response.IsSuccessful = false;
                            response.Message = "User not found..";
                        }
                        else
                        {
                            response.Item = user;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Item = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the user";

                        m_Logger.LogError("Error occured while fetching the user");
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

        public async Task<ServiceResponse<Status>> GetStatusByCategoryIdAndStatusCode(int categoryId, string statusCode)
        {
            //Status status = new Status();
            var response = new ServiceResponse<Status>();
            try
            {
                //Get the ProjectManagers information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETSTATUSBYCATEGORYIDANDSTATUSCODE}{categoryId}/{statusCode}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var status = JsonConvert.DeserializeObject<Status>(json);
                        response.Item = status;
                        response.IsSuccessful = true;
                    }
                    else
                    {
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

        #region GetNotificationTypeByCode
        public async Task<ServiceResponse<NotificationType>> GetNotificationTypeByCode(string notificationCode)
        {
            var response = new ServiceResponse<NotificationType>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETNOTIFICATIONTYPEBYCODE}" + notificationCode);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var notificationtype = JsonConvert.DeserializeObject<NotificationType>(json.ToString());
                        if (notificationtype == null)
                        {
                            response.Item = null;
                            response.IsSuccessful = false;
                            response.Message = "Notification Types not found..";
                        }
                        else
                        {
                            response.Item = notificationtype;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Item = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Notification Type";

                        m_Logger.LogError("Error occured while fetching the Notification Type");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Notification Type";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetNotificationTypeByCategoryId
        public async Task<ServiceResponse<NotificationConfiguration>> GetByNotificationTypeAndCategoryId(int notificationTypeId, int categoryMasterId)
        {
            var response = new ServiceResponse<NotificationConfiguration>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETBYNOTIFICATIONTYPEANCATEGORYID}{ notificationTypeId }&categoryMasterId={ categoryMasterId}");
               
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var notificationConfig = JsonConvert.DeserializeObject<NotificationConfiguration>(json.ToString());
                        if (notificationConfig == null)
                        {
                            response.Item = null;
                            response.IsSuccessful = false;
                            response.Message = "Notification Configuration not found..";
                        }
                        else
                        {
                            response.Item = notificationConfig;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Item = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Notification Configuration";

                        m_Logger.LogError("Error occured while fetching the Notification Configuration");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Notification Configuration";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetProgramManagers
        public async Task<ServiceListResponse<ProgramManager>> GetProgramManagers(string userRole, int employeeId)
        {
            var response = new ServiceListResponse<ProgramManager>();
            try {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETPROGRAMMANAGERS}{userRole}/{employeeId}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var statuses = JsonConvert.DeserializeObject<List<ProgramManager>>(response.ToString());
                        if (statuses == null || statuses.Count == 0)
                        {
                            response.Items = null;
                            response.IsSuccessful = false;
                            response.Message = "No Program Managers found..";
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
                        response.Message = "Error occured while fetching the Program Managers";

                        m_Logger.LogError("Error occured while fetching the Program Managers");
                    }
                }
            }
            catch (Exception ex) {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Program Managers";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion
        
        #region GetNotificationConfigurationByNotificationTypeIdAndCategoryId

        public async Task<ServiceResponse<NotificationConfiguration>> GetNotificationConfiguration(int notificationTypeId, int categoryId)
        {
            var response = new ServiceResponse<NotificationConfiguration>();
            try
            {
                //Get the NotificationConfiguration information from Organisation Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETNOTIFICATIONCONFIGURATION}{notificationTypeId}&categoryMasterId={categoryId}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var notificationConfiguration = JsonConvert.DeserializeObject<NotificationConfiguration>(json);
                        response.Item = notificationConfiguration;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the NotificationConfiguration";

                        m_Logger.LogError("Error occured while fetching the NotificationConfiguration");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the NotificationConfiguration";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }

        #endregion

        #region GetRolesById
        public async Task<ServiceListResponse<RoleMaster>> GetRolesByDepartmentId(int departmentId)
        {
            var response = new ServiceListResponse<RoleMaster>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETROLESBYDEPARTMENTID}" + departmentId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var roles= JsonConvert.DeserializeObject<List<RoleMaster>>(json.ToString());
                        if (roles == null)
                        {
                            response.Items = null;
                            response.IsSuccessful = false;
                            response.Message = "Department not found..";
                        }
                        else
                        {
                            response.Items = roles;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Department";

                        m_Logger.LogError("Error occured while fetching the Department");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Department";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetAllRoleMasters
        public async Task<ServiceListResponse<RoleMaster>> GetAllRoleMasters()
        {
            var response = new ServiceListResponse<RoleMaster>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETALLROLEMASTERS}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var roles = JsonConvert.DeserializeObject<List<RoleMaster>>(json.ToString());
                        if (roles == null)
                        {
                            response.Items = null;
                            response.IsSuccessful = false;
                            response.Message = "RoleMaster data not found..";
                        }
                        else
                        {
                            response.Items = roles;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the RoleMaster";

                        m_Logger.LogError("Error occured while fetching the RoleMaster");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the RoleMaster";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetAllRoles
        public async Task<ServiceListResponse<Role>> GetAllRoles(bool? isActive)
        {
            var response = new ServiceListResponse<Role>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETALLROLES}" + isActive);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var roles = JsonConvert.DeserializeObject<List<Role>>(json.ToString());
                        if (roles == null)
                        {
                            response.Items = null;
                            response.IsSuccessful = false;
                            response.Message = "Roles not found..";
                        }
                        else
                        {
                            response.Items = roles;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Roles";

                        m_Logger.LogError("Error occured while fetching the Roles");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Roles";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetAllUserRoles
        public async Task<ServiceListResponse<UserRole>> GetAllUserRoles(bool? isActive)
        {
            var response = new ServiceListResponse<UserRole>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETALLUSERROLES}"+ isActive);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var roles = JsonConvert.DeserializeObject<List<UserRole>>(json.ToString());
                        if (roles == null)
                        {
                            response.Items = null;
                            response.IsSuccessful = false;
                            response.Message = "UserRoles not found..";
                        }
                        else
                        {
                            response.Items = roles;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the UserRoles";

                        m_Logger.LogError("Error occured while fetching the UserRoles");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the UserRoles";

                m_Logger.LogError(ex.Message);
            }

            return response;
        }
        #endregion

        #region GetRoleMasterNames
        public async Task<ServiceListResponse<RoleMaster>> GetRoleMasterNames()
        {
            var response = new ServiceListResponse<RoleMaster>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETROLENAMES}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var roles = JsonConvert.DeserializeObject<List<RoleMaster>>(json.ToString());
                        if (roles == null)
                        {
                            response.Items = null;
                            response.IsSuccessful = false;
                            response.Message = "Department not found..";
                        }
                        else
                        {
                            response.Items = roles;
                            response.IsSuccessful = true;
                            response.Message = "";
                        }
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Department";

                        m_Logger.LogError("Error occured while fetching the Department");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Department";

                m_Logger.LogError(ex.Message);
            }

            return response;
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

        #region SendEmail
        public async void SendEmail(NotificationDetail notificationDetail)
        {
            try
            {
                //Send Email from Admin Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;

                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.SENDEMAIL}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");
                    
                    HttpResponseMessage httpResponseMessage = await httpClientFactory.PostAsync(httpRequestMessage.RequestUri.ToString(), notificationDetail,new JsonMediaTypeFormatter());

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        m_Logger.LogError("Email sent.");
                    }
                    else
                    {
                        m_Logger.LogError("Error occured while fetching the user data");
                    }
                }
            }
            catch (Exception ex)
            {

                m_Logger.LogError(ex.Message);
            }

        }
        #endregion

        #region GetClosureActivitiesByDepartment
        public async Task<ServiceListResponse<Activity>> GetClosureActivitiesByDepartment(int? departmentId = null)
        {
            var response = new ServiceListResponse<Activity>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETCLOSUREACTIVITIESBYDEPARTMENT}"+departmentId );
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

        #region Get Associate Allocation Master Details
        public async Task<ServiceListResponse<ReportDetails>> GetAssociateAllocationMasters()
        {

            ServiceListResponse<ReportDetails> response = new ServiceListResponse<ReportDetails>();
            try
            {
                List<ReportDetails> masters = new List<ReportDetails>();
                //Get the Master list from Admin Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETASSOCIATEALLOCATIONMASTERS}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        masters = JsonConvert.DeserializeObject<List<ReportDetails>>(json);
                        response.Items = masters;
                        if (masters.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No Master data found";

                            m_Logger.LogError("No Master data found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Master data";

                        m_Logger.LogError("Error occured while fetching the Master data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Master data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetAllDepartmentDLs
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
    }

}
