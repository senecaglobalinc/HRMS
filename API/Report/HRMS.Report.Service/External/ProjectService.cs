using HRMS.Report.Infrastructure;
using HRMS.Report.Infrastructure.Constants;
using HRMS.Report.Infrastructure.Models.Domain;
using HRMS.Report.Infrastructure.Models.Request;
using HRMS.Report.Infrastructure.Models.Response;
using HRMS.Report.Types.External;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace HRMS.Report.Service.External
{
    public class ProjectService : IProjectService
    {
        #region Global Varaibles
        private readonly ILogger<ProjectService> m_Logger;
        private IHttpClientFactory m_ClientFactory;
        private APIEndPoints m_ApiEndPoints;
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
            /*,ICacheService cacheService*/)
        {
            m_Logger = logger;
            m_ClientFactory = clientFactory;
            m_ApiEndPoints = apiEndPoints?.Value;
            //m_CacheService = cacheService;
        }

        #endregion      

        #region Gets Finance Report Details
        public async Task<ServiceListResponse<FinanceReportAllocation>> GetFinanceReportAllocations(FinanceReportFilter filter)
        {

            ServiceListResponse<FinanceReportAllocation> response = new ServiceListResponse<FinanceReportAllocation>();
            try
            {
                List<FinanceReportAllocation> allocations = new List<FinanceReportAllocation>();
                //Get the Allocation list from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETFINANCEREPORTALLOCATIONS}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.PostAsync(httpRequestMessage.RequestUri.ToString(), filter, new JsonMediaTypeFormatter());

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        allocations = JsonConvert.DeserializeObject<List<FinanceReportAllocation>>(json);
                        response.Items = allocations;
                        if (allocations.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No Allocations data found";

                            m_Logger.LogError("No Allocations data found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Allocations data";

                        m_Logger.LogError("Error occured while fetching the Allocations data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Allocations data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region Gets Utilization Report Details
        public async Task<ServiceListResponse<UtilizationReportAllocation>> GetUtilizationReportAllocations(UtilizationReportAllocationFilter filter,bool isNightJob)
        {

            ServiceListResponse<UtilizationReportAllocation> response = new ServiceListResponse<UtilizationReportAllocation>();
            try
            {
                List<UtilizationReportAllocation> allocations = new List<UtilizationReportAllocation>();
                //Get the Allocation list from Project Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    
                    // check if the call is from Night job, then add servicepath to NightJob controller else Report Controller
                    httpRequestMessage.RequestUri =isNightJob ?  new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETUTILIZATIONREPORTALLOCATIONSNIGHTJOB}"): new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETUTILIZATIONREPORTALLOCATIONS}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.PostAsync(httpRequestMessage.RequestUri.ToString(), filter, new JsonMediaTypeFormatter());

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        allocations = JsonConvert.DeserializeObject<List<UtilizationReportAllocation>>(json);
                        response.Items = allocations;
                        if (allocations.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No Allocations data found";

                            m_Logger.LogError("No Allocations data found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Allocations data";

                        m_Logger.LogError("Error occured while fetching the Allocations data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Allocations data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region Get Domain Report Details
        public async Task<ServiceListResponse<DomainDataCount>> GetDomainWiseResourceCount()
        {

            ServiceListResponse<DomainDataCount> response = new ServiceListResponse<DomainDataCount>();
            try
            {
                List<DomainDataCount> domains = new List<DomainDataCount>();
                //Get the Employee list from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETDOMAINWISERESOURCECOUNT}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        domains = JsonConvert.DeserializeObject<List<DomainDataCount>>(json);
                        response.Items = domains;
                        if (domains.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No Domains data found";

                            m_Logger.LogError("No Domains data found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Domains data";

                        m_Logger.LogError("Error occured while fetching the Domains data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Domains data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region Get TalentPool Report Details
        public async Task<ServiceListResponse<TalentPoolReportCount>> GetTalentPoolWiseResourceCount(List<int> projectTypeIds)
        {

            ServiceListResponse<TalentPoolReportCount> response = new ServiceListResponse<TalentPoolReportCount>();
            try
            {
                List<TalentPoolReportCount> list = new List<TalentPoolReportCount>();                
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETTALENTPOOLWISERESOURCECOUNT}" + string.Join(",", projectTypeIds));
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        list = JsonConvert.DeserializeObject<List<TalentPoolReportCount>>(json);
                        response.Items = list;
                        if (list.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No TalentPool data found";

                            m_Logger.LogError("No TalentPool data found");
                        }
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

        #region Get Service Type Resource Count
        public async Task<ServiceListResponse<ServiceTypeCount>> GetServiceTypeResourceCount()
        {

            ServiceListResponse<ServiceTypeCount> response = new ServiceListResponse<ServiceTypeCount>();
            try
            {
                List<ServiceTypeCount> domains = new List<ServiceTypeCount>();
                //Get the Employee list from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETSERVICETYPECOUNT}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        domains = JsonConvert.DeserializeObject<List<ServiceTypeCount>>(json);
                        response.Items = domains;
                        if (domains.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No ProjectsType data found";

                            m_Logger.LogError("No ProjectsType data found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the ProjectsType data";

                        m_Logger.LogError("Error occured while fetching the ProjectsType data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the ProjectsType data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region Get All Project Details
        public async Task<ServiceListResponse<ProjectRespose>> GetAllProjects()
        {

            ServiceListResponse<ProjectRespose> response = new ServiceListResponse<ProjectRespose>();
            try
            {
                List<ProjectRespose> domains = new List<ProjectRespose>();
                //Get the Employee list from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETALLPROJECTS}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        domains = JsonConvert.DeserializeObject<List<ProjectRespose>>(json);
                        response.Items = domains;
                        if (domains.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No Projects data found";

                            m_Logger.LogError("No Projects data found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Projects data";

                        m_Logger.LogError("Error occured while fetching the Projects data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Projects data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetCriticalResourceReport
        public async Task<ServiceListResponse<AssociateInformationReport>> GetCriticalResourceReport()
        {

            ServiceListResponse<AssociateInformationReport> response = new ServiceListResponse<AssociateInformationReport>();
            try
            {
                List<AssociateInformationReport> informationReports = new List<AssociateInformationReport>();

                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETCRITICALRESOURCEREPORT}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        informationReports = JsonConvert.DeserializeObject<List<AssociateInformationReport>>(json);
                       
                        if (informationReports.Count > 0)
                        {
                            response.IsSuccessful = true;
                            response.Items = informationReports;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No critical resource data found";

                            m_Logger.LogError("No critical resource data found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the critical resource data";

                        m_Logger.LogError("Error occured while fetching the critical resource data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the critical resource data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetNonCriticalResourceReport
        public async Task<ServiceListResponse<AssociateInformationReport>> GetNonCriticalResourceReport()
        {

            ServiceListResponse<AssociateInformationReport> response = new ServiceListResponse<AssociateInformationReport>();
            try
            {
                List<AssociateInformationReport> informationReports = new List<AssociateInformationReport>();

                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETNONCRITICALRESOURCEREPORT}" );
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        informationReports = JsonConvert.DeserializeObject<List<AssociateInformationReport>>(json);
                        if (informationReports.Count > 0)
                        {
                            response.IsSuccessful = true;
                            response.Items = informationReports;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No non_critical resource data found";

                            m_Logger.LogError("No non_critical resource data found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the non_critical resource data";

                        m_Logger.LogError("Error occured while fetching the non_critical resource data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the non_critical resource data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetNonCriticalResourceBillingReport
        public async Task<ServiceListResponse<AssociateInformationReport>> GetNonCriticalResourceBillingReport()
        {

            ServiceListResponse<AssociateInformationReport> response = new ServiceListResponse<AssociateInformationReport>();
            try
            {
                List<AssociateInformationReport> informationReports = new List<AssociateInformationReport>();

                using (var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.ProjectEndPoint +
                        $"{ServicePaths.ProjectEndPoint.GETNONCRITICALRESOURCEBILLINGREPORT}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        informationReports = JsonConvert.DeserializeObject<List<AssociateInformationReport>>(json);
                        if (informationReports.Count > 0)
                        {
                            response.IsSuccessful = true;
                            response.Items = informationReports;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No non_critical resource last billing data found";

                            m_Logger.LogError("No non_critical resource last billing data found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the non_critical resource last billing data";

                        m_Logger.LogError("Error occured while fetching the non_critical resource last billing data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the non_critical resource last billing data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

    }
}
