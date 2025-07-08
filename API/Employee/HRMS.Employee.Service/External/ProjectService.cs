using HRMS.Common.Extensions;
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
//using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace HRMS.Employee.Service.External
{
    /// <summary>
    /// Service for Project Microservice calls
    /// </summary>
    public class ProjectService : IProjectService
    {
        #region Global Varaibles
        private readonly ILogger<ProjectService> m_Logger;
        private IHttpClientFactory m_ClientFactory;
        private APIEndPoints m_ApiEndPoints;
        //private ICacheService m_CacheService;

        #endregion

        #region Constructor
        /// <summary>
        /// ProjectService
        /// <paramref name="apiEndPoints"/>
        /// <paramref name="clientFactory"/>
        /// <paramref name="logger"/>
        /// </summary>
        public ProjectService(ILogger<ProjectService> logger,
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

        #region GetProjectByIds
        /// <summary>
        /// GetProjectById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<Project>> GetProjectById(List<int> projectIds)
        {
            //var projectResponse = new ServiceListResponse<Project>();
            //List<Project> projects = new List<Project>();
            //try
            //{
            //    ///Get the project records for cache for the selected projectid
            //    foreach (var projectId in projectIds)
            //    {
            //        var project = await m_CacheService.GetCacheValueAsync<Project>("Projects_ID_" + projectId);
            //        projects.Add(project);
            //    }

            //    if (projects.Count == 0)
            //    {
            //        projectResponse.IsSuccessful = false;
            //        projectResponse.Message = "Error occured while fetching project by Id.";

            //        m_Logger.LogError("Error occured while fetching project by Id.");
            //        return projectResponse;
            //    }

            //    projectResponse.IsSuccessful = true;
            //    projectResponse.Items = projects;
            //    return projectResponse;
            //}
            //catch (Exception ex)
            //{
            //    projectResponse.IsSuccessful = false;
            //    projectResponse.Message = "Error occured while fetching project by Id.";

            //    m_Logger.LogError(ex.Message);
            //    return projectResponse;
            //}

            //To get data from Project MicroService
            var response = new ServiceListResponse<Project>();

            try
            {
                //Get the Associate Allocation information from Project Microservice
                using var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient");
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                     $"{ServicePaths.ProjectEndPoint.GETPROJECTSBYIDS}" + string.Join(",", projectIds))
                };
                httpRequestMessage.Headers.Add("Accept", "application/json");

                HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    response.Items = JsonConvert.DeserializeObject<List<Project>>(json);
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occured while fething the Project data";
                    m_Logger.LogError("Error occured while fething the Project data.");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the Project data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetProjectByID
        /// <summary>
        /// GetAssociateAllocations
        /// </summary>
        /// <param name="employeeIds">list of employeeIds</param>
        /// <returns></returns>

        public async Task<ServiceResponse<Project>> GetProjectByID(int projectId)
        {
            Project associateAllocations = null;
            var response = new ServiceResponse<Project>();

            try
            {
                //Get the Associate Allocation information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                         $"{ServicePaths.ProjectEndPoint.GETPROJECTBYID}" + projectId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        associateAllocations = JsonConvert.DeserializeObject<Project>(json);
                        response.Item = associateAllocations;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the Project data";
                        m_Logger.LogError("Error occured while fething the Project data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the Project data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }

        #endregion

        #region GetAssociateAllocations
        /// <summary>
        /// GetAssociateAllocations
        /// </summary>
        /// <param name="employeeIds">list of employeeIds</param>
        /// <returns></returns>

        public async Task<ServiceListResponse<AssociateAllocation>> GetAssociateAllocations(List<int> employeeIds)
        {
            List<AssociateAllocation> associateAllocations = null;
            var response = new ServiceListResponse<AssociateAllocation>();

            try
            {
                //Get the Associate Allocation information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                         $"{ServicePaths.ProjectEndPoint.GETASSOCIATEALLOCATIONSBYIDS}" + string.Join(",", employeeIds));
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        associateAllocations = JsonConvert.DeserializeObject<List<AssociateAllocation>>(json);
                        response.Items = associateAllocations;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the AssociateAllocation data";
                        m_Logger.LogError("Error occured while fething the AssociateAllocation data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the AssociateAllocation data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }

        #endregion

        #region GetAssociateAllocationsUsingCache
        /// <summary>
        /// GetAssociateAllocationsUsingCache
        /// </summary>
        /// <param name="employeeIds">list of employeeIds</param>
        /// <returns></returns>

        public ServiceListResponse<AssociateAllocation> GetAssociateAllocationsUsingCache(List<int> employeeIds)
        {
            var response = new ServiceListResponse<AssociateAllocation>();
            List<AssociateAllocation> associateAllocations = new List<AssociateAllocation>();

            //try
            //{
            //    //Get the Associate Allocation information from Cache
            //    foreach (var employeeId in employeeIds)
            //    {
            //        RedisValue[] associateAllocationEmployeeIds = m_CacheService.SetMembers("AssociateAllocation:Emp:ID:" + employeeId);

            //        foreach (RedisValue item in associateAllocationEmployeeIds)
            //        {
            //            var associateAllocationEntries = m_CacheService.HashGetAll(item.ToString());
            //            if (associateAllocationEntries != null)
            //            {
            //                var associateAllocation = associateAllocationEntries.ConvertHashToEntity<AssociateAllocation>();
            //                associateAllocations.Add(associateAllocation);
            //            }
            //        }
            //    }

            //    if (associateAllocations.Count > 0)
            //    {
            //        response.Items = associateAllocations;
            //        response.IsSuccessful = true;
            //    }
            //    else
            //    {
            //        response.IsSuccessful = false;
            //        response.Message = "There are no matching records for the employee ids";
            //    }

            //}
            //catch (Exception ex)
            //{
            //    response.IsSuccessful = false;
            //    response.Message = "Error occured while fething the AssociateAllocation data";

            //    m_Logger.LogError(ex.Message);
            //}
            return response;
        }

        #endregion

        #region GetProjectManagersByIds
        /// <summary>
        /// GetProjectManagers
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectManager>> GetProjectManagersByIds(List<int> projectManagerIds)
        {

            List<ProjectManager> projectManagers = null;
            var response = new ServiceListResponse<ProjectManager>();
            try
            {
                //Get the ProjectManagers information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETPROJECTMANAGERSBYID}" + projectManagerIds.ToArray());
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        projectManagers = JsonConvert.DeserializeObject<List<ProjectManager>>(json);
                        response.Items = projectManagers;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the project managers data";

                        m_Logger.LogError("Error occured while fething the project managers data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the project managers data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion

        #region GetProjectManagersByEmployeeIds
        /// <summary>
        /// GetProjectManagers
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectManager>> GetProjectManagersByEmployeeIds(List<int> employeeIds)
        {

            List<ProjectManager> projectManagers = null;
            var response = new ServiceListResponse<ProjectManager>();
            try
            {
                //Get the ProjectManagers information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETPROJECTMANAGERSBYEMPLOYEEIDS}" + string.Join(",", employeeIds));
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        projectManagers = JsonConvert.DeserializeObject<List<ProjectManager>>(json);
                        response.Items = projectManagers;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the project managers data";

                        m_Logger.LogError("Error occured while fething the project managers data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the project managers data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion

        #region GetAllocationPercentage
        /// <summary>
        /// GetAllocationPercentage
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<AllocationPercentage>> GetAllocationPercentage()
        {

            List<AllocationPercentage> allocationPercentage = null;
            var response = new ServiceListResponse<AllocationPercentage>();
            try
            {
                //Get the AllocationPercentage information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETALLOCATIONPERCENTAGE}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        allocationPercentage = JsonConvert.DeserializeObject<List<AllocationPercentage>>(json);
                        response.Items = allocationPercentage;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the allocationPercentage data";

                        m_Logger.LogError("Error occured while fething the allocationPercentage data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the allocationPercentage data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion

        #region GetAssociateAllocationsByEmployeeId
        /// <summary>
        /// GetAssociateAllocationsByEmployeeId
        /// </summary>
        /// <param name="employeeId">employeeIds</param>
        /// <returns></returns>

        public async Task<ServiceListResponse<AssociateAllocation>> GetAssociateAllocationsByEmployeeId(int employeeId)
        {
            List<AssociateAllocation> associateAllocations = null;
            var response = new ServiceListResponse<AssociateAllocation>();

            try
            {
                //Get the Associate Allocation information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                         $"{ServicePaths.ProjectEndPoint.GETASSOCIATEALLOCATIONSBYEMPLOYEEID}" + employeeId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        associateAllocations = JsonConvert.DeserializeObject<List<AssociateAllocation>>(json);
                        response.Items = associateAllocations;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the AssociateAllocation data";
                        m_Logger.LogError("Error occured while fething the AssociateAllocation data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the AssociateAllocation data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }

        #endregion

        #region GetAssociateAllocationsByLeadId
        /// <summary>
        /// GetAssociateAllocationsByLeadId
        /// </summary>
        /// <param name="leadId">employeeIds</param>
        /// <returns></returns>

        public async Task<ServiceListResponse<AssociateAllocation>> GetAssociateAllocationsByLeadId(int leadId)
        {
            List<AssociateAllocation> associateAllocations = null;
            var response = new ServiceListResponse<AssociateAllocation>();

            try
            {
                //Get the Associate Allocation information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                         $"{ServicePaths.ProjectEndPoint.GETASSOCIATEALLOCATIONSBYLEADID}" + leadId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        associateAllocations = JsonConvert.DeserializeObject<List<AssociateAllocation>>(json);
                        response.Items = associateAllocations;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the AssociateAllocation data";
                        m_Logger.LogError("Error occured while fething the AssociateAllocation data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the AssociateAllocation data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }

        #endregion

        #region GetProjectManagersByEmployeeId
        /// <summary>
        /// GetProjectManagers
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectManager>> GetProjectManagersByEmployeeId(int employeeId)
        {

            List<ProjectManager> projectManagers = null;
            var response = new ServiceListResponse<ProjectManager>();
            try
            {
                //Get the ProjectManagers information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETPROJECTMANAGERSBYEMPLOYEEID}" + employeeId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        projectManagers = JsonConvert.DeserializeObject<List<ProjectManager>>(json);
                        response.Items = projectManagers;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the project managers data";

                        m_Logger.LogError("Error occured while fething the project managers data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the project managers data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion

        #region GetActiveProjectManagers
        /// <summary>
        /// GetActiveProjectManagers
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectManager>> GetActiveProjectManagers()
        {

            List<ProjectManager> projectManagers = null;
            var response = new ServiceListResponse<ProjectManager>();
            try
            {
                //Get the ProjectManagers information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETACTIVEPROJECTMANAGERS}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        projectManagers = JsonConvert.DeserializeObject<List<ProjectManager>>(json);
                        response.Items = projectManagers;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the project managers data";

                        m_Logger.LogError("Error occured while fething the project managers data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the project managers data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion

        #region AllocateAssociateToTalentPool
        /// <summary>
        /// allocate associate to talent pool
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> AllocateAssociateToTalentPool(EmployeeDetails employee)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                //Get the Designation information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {

                    var uri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.ALLOCATEASSOCIATETOTALENTPOOL}");
                    var httpResponse = await httpClientFactory.PostAsync(uri, employee, new JsonMediaTypeFormatter());
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        string json = await httpResponse.Content.ReadAsStringAsync();
                        response.Item = true;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while allocating employee";
                        m_Logger.LogError("Error occured while allocating employee");
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

        #region GetEmployeeByProjectId
        /// <summary>
        /// GetEmployeeByProjectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<int>> GetEmployeeByProjectId(int projectId)
        {
            var response = new ServiceListResponse<int>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETEMPLOYEEBYPROJECTID}" + projectId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        List<int> list = JsonConvert.DeserializeObject<List<int>>(json);
                        response.Items = list;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the Employee data";

                        m_Logger.LogError("Error occured while fething the Employee data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the Employee data";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetResourceByProject
        /// <summary>
        /// GetResourceByProject
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectResourceData>> GetResourceByProject(int projectId)
        {
            var response = new ServiceListResponse<ProjectResourceData>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETRESOURCEBYPROJECT}" + projectId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        List<ProjectResourceData> list = JsonConvert.DeserializeObject<List<ProjectResourceData>>(json);
                        response.Items = list;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the Employee data";

                        m_Logger.LogError("Error occured while fething the Employee data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the Employee data";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetSkillSearchAllocations
        /// <summary>
        /// GetSkillSearchAllocations
        /// </summary>       
        /// <returns></returns>
        public async Task<ServiceListResponse<SkillSearchAllocation>> GetSkillSearchAllocations()
        {
            var response = new ServiceListResponse<SkillSearchAllocation>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETSKILLSEARCHALLOCATIONS}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        List<SkillSearchAllocation> list = JsonConvert.DeserializeObject<List<SkillSearchAllocation>>(json);
                        response.Items = list;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the Allocation data";

                        m_Logger.LogError("Error occured while fething the Allocation data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the Allocation data";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetSkillSearchAssociateAllocations
        /// <summary>
        /// GetSkillSearchAssociateAllocations
        /// </summary>       
        /// <returns></returns>
        public async Task<ServiceListResponse<SkillSearchAssociateAllocation>> GetSkillSearchAssociateAllocations(string employeeIds)
        {
            var response = new ServiceListResponse<SkillSearchAssociateAllocation>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETSKILLSEARCHASSOCIATEALLOCATIONS}" + employeeIds);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        List<SkillSearchAssociateAllocation> list = JsonConvert.DeserializeObject<List<SkillSearchAssociateAllocation>>(json);
                        response.Items = list;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the Allocation data";

                        m_Logger.LogError("Error occured while fething the Allocation data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the Allocation data";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetAllAssociateAllocations
        /// <summary>
        /// GetAllAssociateAllocations
        /// </summary>
        /// <param ></param>
        /// <returns></returns>

        public async Task<ServiceListResponse<AssociateAllocation>> GetAllAssociateAllocations(bool nightlyJob = false)
        {
            List<AssociateAllocation> associateAllocations = null;
            var response = new ServiceListResponse<AssociateAllocation>();

            try
            {
                //Get the Associate Allocation information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    
                    if (nightlyJob)
                    {
                        httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                             $"{ServicePaths.ProjectEndPoint.GETALLASSOCIATEALLOCATIONS}");

                        httpRequestMessage.Headers.Add("x-Nightjob", "HRMSUtilizationReportConsole");
                    }
                    else
                    {
                        httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                             $"{ServicePaths.ProjectEndPoint.GETASSOCIATEALLOCATIONS}");
                    }

                    httpRequestMessage.Headers.Add("Accept", "application/json");
                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        associateAllocations = JsonConvert.DeserializeObject<List<AssociateAllocation>>(json);
                        response.Items = associateAllocations;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the AssociateAllocation data";
                        m_Logger.LogError("Error occured while fething the AssociateAllocation data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the AssociateAllocation data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetAllProjects
        /// <summary>
        /// GetAllProjects
        /// </summary>
        /// <returns></returns>

        public async Task<ServiceListResponse<Project>> GetAllProjects(bool nightlyJob = false)
        {
            List<Project> associateAllocations = null;
            var response = new ServiceListResponse<Project>();

            try
            {
                //Get the Associate Allocation information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;                   

                    if (nightlyJob)
                    {
                        httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                         $"{ServicePaths.ProjectEndPoint.GETPROJECTS}");

                        httpRequestMessage.Headers.Add("x-Nightjob", "HRMSUtilizationReportConsole");
                    }
                    else
                    {
                        httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                         $"{ServicePaths.ProjectEndPoint.GETALLPROJECTS}");
                    }
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        associateAllocations = JsonConvert.DeserializeObject<List<Project>>(json);
                        response.Items = associateAllocations;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the Project data";
                        m_Logger.LogError("Error occured while fething the Project data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the Project data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetEmployeeProjectIdByEmpId
        public async Task<ServiceResponse<AssociateAllocation>> GetEmployeeProjectIdByEmpId(int empId)
        {
            var response = new ServiceResponse<AssociateAllocation>();
            try
            {
                //List<AssociateAllocation> AssociateAllocations = new List<AssociateAllocation>();
                //Cross communication call to get associate allocations by employee id
                var allocationsResponse = await GetAssociateAllocationsByEmployeeId(empId);
                if (allocationsResponse.Items != null)
                {
                    //AssociateAllocations = allocationsResponse.Items;

                    //Check is there any primary project. First priority could be primary project
                    var associateProject = (from associateAllocation in allocationsResponse.Items
                                            where associateAllocation.IsPrimary.Value == true && associateAllocation.ReleaseDate == null
                                            select associateAllocation).FirstOrDefault();

                    // Get non primary Or Talent pool
                    if (associateProject == null)
                    {
                        associateProject = (from associateAllocation in allocationsResponse.Items
                                            where associateAllocation.IsPrimary.Value == false && associateAllocation.ReleaseDate == null
                                            select associateAllocation).FirstOrDefault();
                    }

                    response.IsSuccessful = true;
                    response.Item = associateProject;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching active employees";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region ReleaseFromTalentPool
        /// <summary>
        /// ReleaseFromTalentPool
        /// </summary>
        /// <param name="tpDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> ReleaseFromTalentPool(TalentPoolDetails tpDetails)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {

                    var uri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.UPDATETALENTPOOL}");
                    var httpResponse = await httpClientFactory.PostAsync(uri, tpDetails, new JsonMediaTypeFormatter());
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        string json = await httpResponse.Content.ReadAsStringAsync();
                        response.Item = true;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        string json = await httpResponse.Content.ReadAsStringAsync();
                        response.IsSuccessful = false;
                        response.Message = json;
                        m_Logger.LogError("Error occured while releasing from talent pool");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while releasing from talent pool";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetEmployeeByEmployeeIdAndRole
        /// <summary>
        /// GetEmployeeByEmployeeIdAndRole
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeDetails>> GetEmployeesByEmployeeIdAndRole(int employeeId, string roleName)
        {
            var response = new ServiceListResponse<EmployeeDetails>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETEMPLOYEESBYEMPLOYEEIDANDROLE}" + employeeId + "/" + roleName);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        List<EmployeeDetails> list = JsonConvert.DeserializeObject<List<EmployeeDetails>>(json);
                        response.Items = list;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Employee data";

                        m_Logger.LogError("Error occured while fetching the Employee data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetProjectLeadData
        /// <summary>
        /// GetProjectLeadData
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectManager>> GetProjectLeadData(int employeeId)
        {
            var response = new ServiceListResponse<ProjectManager>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETPROJECTLEADDATA}" + employeeId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        List<ProjectManager> list = JsonConvert.DeserializeObject<List<ProjectManager>>(json);
                        response.Items = list;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the managers data";

                        m_Logger.LogError("Error occured while fetching the managers data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetProjectRMData
        /// <summary>
        /// GetProjectRMData
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectManager>> GetProjectRMData(int employeeId)
        {
            var response = new ServiceListResponse<ProjectManager>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETPROJECTRMDATA}" + employeeId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        List<ProjectManager> list = JsonConvert.DeserializeObject<List<ProjectManager>>(json);
                        response.Items = list;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the managers data";

                        m_Logger.LogError("Error occured while fetching the managers data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetProjectManagerFromAllocations
        /// <summary>
        /// GetProjectManagerFromAllocations
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<bool> GetProjectManagerFromAllocations(int employeeId)
        {
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETPROJECTMANAGERFROMALLOCATIONS}" + employeeId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        m_Logger.LogError("Error occured while fetching the managers data");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return false;
            }

        }
        #endregion

        #region UpdatePracticeAreaOfTalentPoolProject
        /// <summary>
        /// UpdatePracticeAreaOfTalentPoolProject
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> UpdatePracticeAreaOfTalentPoolProject(int employeeid, int competencyAreaId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                //Get the Designation information from Org Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.UPDATETP_PROJECTINALLOCATIONS}{employeeid}{'/'}{competencyAreaId}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponse = await httpClientFactory.SendAsync(httpRequestMessage);
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        string json = await httpResponse.Content.ReadAsStringAsync();
                        response.Item = true;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while updating talent-pool project in associate allocation";
                        m_Logger.LogError("Error occured while updating talent-pool project in associate allocation");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while updating talent-pool project in associate allocation";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetAllTalentPoolData
        /// <summary>
        /// GetAllTalentPoolData
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<TalentPool>> GetAllTalentPoolData()
        {
            var response = new ServiceListResponse<TalentPool>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETALLTALENTPOOLDATA}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        List<TalentPool> list = JsonConvert.DeserializeObject<List<TalentPool>>(json);
                        response.Items = list;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the TalentPool data";

                        m_Logger.LogError("Error occured while fetching the TalentPool data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the TalentPool data";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetPMByPracticeAreaId
        /// <summary>
        /// GetPMByPracticeAreaId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<ProjectManager>> GetPMByPracticeAreaId(int practiceAreaId)
        {
            var response = new ServiceResponse<ProjectManager>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETPMBYPRACTICEAREAID}" + practiceAreaId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        ProjectManager res = JsonConvert.DeserializeObject<ProjectManager>(json);
                        response.Item = res;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the managers data";

                        m_Logger.LogError("Error occured while fetching the managers data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the managers data";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region ReleaseFromAllocations
        /// <summary>
        /// ReleaseFromAllocations
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> ReleaseFromAllocations(int EmployeeId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.RELEASEALLOCATIONS}" + EmployeeId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while releasing allocation";

                        m_Logger.LogError("Error occured while releasing allocation");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while releasing allocation";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region ReleaseOnExit
        /// <summary>
        /// ReleaseOnExit
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="releaseDate"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> ReleaseOnExit(int employeeId, string releaseDate)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                using var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient");

                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                    $"{ServicePaths.ProjectEndPoint.RELEASEONEXIT}{employeeId}{'/'}{releaseDate}")
                };

                httpRequestMessage.Headers.Add("Accept", "application/json");

                HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occured while releasing allocation";

                    m_Logger.LogError("Error occured while releasing allocation");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while releasing allocation";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion


        #region GetUtilizationReportAllocations
        /// <summary>
        /// GetUtilizationReportAllocations
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<UtilizationReportFilter>> GetUtilizationReportAllocations(int projectId)
        {
            var response = new ServiceListResponse<UtilizationReportFilter>();
            try
            {
                var utilizationfilter = new UtilizationReportFilter();
                utilizationfilter.ProjectId = projectId;
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {

                    var uri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETALLOCATIONSBYPROJECT}");
                    var httpResponse = await httpClientFactory.PostAsync(uri, utilizationfilter, new JsonMediaTypeFormatter());
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        string json = await httpResponse.Content.ReadAsStringAsync();
                        var list= JsonConvert.DeserializeObject<List<UtilizationReportFilter>>(json);
                        response.Items = list;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        string json = await httpResponse.Content.ReadAsStringAsync();
                        response.IsSuccessful = false;
                        response.Message = json;
                        m_Logger.LogError("Error occured while fetching data from AssociateAllocation");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching data from AssociateAllocation";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

    	#region GetActiveAllocations
        /// <summary>
        /// GetActiveAllocations
        /// </summary>
        /// <returns></returns>

        public async Task<ServiceListResponse<EmployeeInfo>> GetActiveAllocations()
        {
            List<EmployeeInfo> associateAllocations = null;
            var response = new ServiceListResponse<EmployeeInfo>();

            try
            {
                //Get the Associate Allocation information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                    $"{ServicePaths.ProjectEndPoint.GETACTIVEALLOCATIONS}");
                    
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        associateAllocations = JsonConvert.DeserializeObject<List<EmployeeInfo>>(json);
                        response.Items = associateAllocations;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the Project data";
                        m_Logger.LogError("Error occured while fething the Project data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the Project data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetAllocationByID
        /// <summary>
        /// GetAllocationByID
        /// </summary>
        /// <returns></returns>

        public async Task<ServiceResponse<AssociateAllocation>> GetAllocationById(int allocationId)
        {
            AssociateAllocation associateAllocation = null;
            var response = new ServiceResponse<AssociateAllocation>();

            try
            {
                //Get the projectManagers information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                    $"{ServicePaths.ProjectEndPoint.GETMANAGERSDETAILSBYALLOCATIONID + allocationId}");

                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        associateAllocation = JsonConvert.DeserializeObject<AssociateAllocation>(json);
                        response.Item = associateAllocation;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the AssociateAllocation data";
                        m_Logger.LogError("Error occured while fething the AssociateAllocation data.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the AssociateAllocation data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion



        #region GetCompetencyAreaManagersDetails
        /// <summary>
        /// GetCompetencyAreaManagersDetails
        /// </summary>
        /// <returns></returns>

        public async Task<ServiceResponse<CompetencyAreaMananagers>> GetCompetencyAreaManagersDetails(int competencyAreaId)
        {
            var response = new ServiceResponse<CompetencyAreaMananagers>();

            try
            {
                //Get the projectManagers information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                    $"{ServicePaths.ProjectEndPoint.GETPRACTIVEAREAMANAGERS + competencyAreaId}");

                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ServiceResponse<CompetencyAreaMananagers>>(json);                   
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message =response.Message;

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetAllocationsByEmpIds
        /// <summary>
        /// GetCompetencyAreaManagersDetails
        /// </summary>
        /// <returns></returns>

        public async Task<ServiceListResponse<AssociateAllocation>> GetAllocationsByEmpIds(string empIds)
        {
            var response = new ServiceListResponse<AssociateAllocation>();

            try
            {
                //Get the AssociateAllocation information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                    $"{ServicePaths.ProjectEndPoint.GETALLOCATIONBYEMPIDS + empIds}");

                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    response.Items = JsonConvert.DeserializeObject<List<AssociateAllocation>>(json);
                    
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = response.Message;

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion


        #region GetAllAllocationDetails
        /// <summary>
        /// GetAllAllocationDetails
        /// </summary>
        /// <returns></returns>

        public async Task<ServiceListResponse<ActiveAllocationDetails>> GetAllAllocationDetails()
        {
            var response = new ServiceListResponse<ActiveAllocationDetails>();

            try
            {
                //Get the AssociateAllocation information from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                    $"{ServicePaths.ProjectEndPoint.GETALLACTIVEALLOCATIONDETAILS}");

                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ServiceListResponse<ActiveAllocationDetails>>(json);
                    

                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = response.Message;

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion


        #region GetProjectsByEmpIdAndRole
        /// <summary>
        /// GetProjectsByEmpIdAndRole
        /// </summary>
        /// <returns></returns>

        public async Task<ServiceResponse<ProjectsData>> GetProjectsByEmpIdAndRole(int employeeId)
        {
            var response = new ServiceResponse<ProjectsData>();

            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                    $"{ServicePaths.ProjectEndPoint.GETPROJECTBYEMPIDANDROLE + employeeId}");

                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ServiceResponse<ProjectsData>>(json);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = response.Message;

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion
    }

}
