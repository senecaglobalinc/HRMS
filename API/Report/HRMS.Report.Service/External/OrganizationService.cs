using HRMS.Report.Infrastructure;
using HRMS.Report.Infrastructure.Constants;
using HRMS.Report.Infrastructure.Models.Domain;
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

        #region Get Finance Report Details
        public async Task<ServiceListResponse<ReportDetails>> GetFinanceReportMasters()
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
                        $"{ServicePaths.OrgEndPoint.GETFINANCEREPORTMASTERS}");
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

        #region Get Utilization Report Details
        public async Task<ServiceListResponse<ReportDetails>> GetUtilizationReportMasters(bool isNightJob)
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

                    // check if the call is from Night job, then add servicepath to NightJob controller else Report Controller
                    httpRequestMessage.RequestUri = isNightJob ? new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETUTILIZATIONREPORTMASTERSNIGHTJOB}"): new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETUTILIZATIONREPORTMASTERS}");
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

        #region Get Domain Report Details
        public async Task<ServiceListResponse<ReportDetails>> GetDomainReportMasters()
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
                        $"{ServicePaths.OrgEndPoint.GETDOMAINREPORTMASTERS}");
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

        #region Get TalentPool Report Details
        public async Task<ServiceListResponse<ReportDetails>> GetTalentPoolReportMasters()
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
                        $"{ServicePaths.OrgEndPoint.GETTALENTPOOLREPORTMASTERS}");
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

        #region Get Finance Report Details
        public async Task<ServiceListResponse<ReportDetails>> GetSkillSearchMasters()
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
                        $"{ServicePaths.OrgEndPoint.GETSKILLSEARCHMASTERS}");
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

        #region Get ServiceTypeAll 
        public async Task<ServiceListResponse<ServiceType>> GetServiceTypetMasters()
        {

            ServiceListResponse<ServiceType> response = new ServiceListResponse<ServiceType>();
            try
            {
                List<ServiceType> masters = new List<ServiceType>();
                //Get the Master list from Admin Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETSERVICETYPEMASTERS}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        masters = JsonConvert.DeserializeObject<List<ServiceType>>(json);
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

        #region GetProjectTypeByCode
        public async Task<ServiceResponse<ServiceType>> GetProjectTypeByCode(string projectTypeCode)
        {

            ServiceResponse<ServiceType> response = new ServiceResponse<ServiceType>();
            try
            {
                ServiceType projectType = new ServiceType();
                //Get the Master list from Admin Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("AdminClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                        $"{ServicePaths.OrgEndPoint.GETPROJECTTYPEBYCODE}/{projectTypeCode}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        projectType = JsonConvert.DeserializeObject<ServiceType>(json);
                        response.Item = projectType;
                        if (projectType!= null)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No projectType data found";

                            m_Logger.LogError("No projectType data found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the projectType from Master data";

                        m_Logger.LogError("Error occured while fetching the projectType from Master data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the projectType from Master data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion
    }

}
