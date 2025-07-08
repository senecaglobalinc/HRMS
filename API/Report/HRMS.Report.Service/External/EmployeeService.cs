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

        #region Get Finance Report Details
        public async Task<ServiceListResponse<FinanceReportEmployee>> GetFinanceReportAssociates(FinanceReportEmployeeFilter filter)
        {
            
            ServiceListResponse<FinanceReportEmployee> response = new ServiceListResponse<FinanceReportEmployee>();
            try
            {
                List<FinanceReportEmployee> associates = new List<FinanceReportEmployee>();
                //Get the Employee list from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETFINANCEREPORTASSOCIATES}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.PostAsync(httpRequestMessage.RequestUri.ToString(), filter, new JsonMediaTypeFormatter());

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        associates = JsonConvert.DeserializeObject<List<FinanceReportEmployee>>(json);
                        response.Items = associates;
                        if (associates.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No Associates data found";

                            m_Logger.LogError("No Associates data found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Associates data";

                        m_Logger.LogError("Error occured while fetching the Associates data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Associates data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region Get Utilization Report Details
        public async Task<ServiceListResponse<UtilizationReportEmployee>> GetUtilizationReportAssociates(UtilizationReportEmployeeFilter filter, bool isNightJob)
        {

            ServiceListResponse<UtilizationReportEmployee> response = new ServiceListResponse<UtilizationReportEmployee>();
            try
            {
                List<UtilizationReportEmployee> associates = new List<UtilizationReportEmployee>();
                //Get the Employee list from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                   
                    // check if the call is from Night job, then add servicepath to NightJob controller else Report Controller
                    httpRequestMessage.RequestUri = isNightJob ? new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETUTILIZATIONREPORTASSOCIATESNIGHTJOB}"): new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETUTILIZATIONREPORTASSOCIATES}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.PostAsync(httpRequestMessage.RequestUri.ToString(), filter, new JsonMediaTypeFormatter());

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        associates = JsonConvert.DeserializeObject<List<UtilizationReportEmployee>>(json);
                        response.Items = associates;
                        if (associates.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No Associates data found";

                            m_Logger.LogError("No Associates data found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Associates data";

                        m_Logger.LogError("Error occured while fetching the Associates data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Associates data";

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
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETDOMAINWISERESOURCECOUNT}");
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

        #region Get Domain Report Associates
        public async Task<ServiceListResponse<DomainReportEmployee>> GetDomainReportAssociates(string employeeIds)
        {

            ServiceListResponse<DomainReportEmployee> response = new ServiceListResponse<DomainReportEmployee>();
            try
            {
                List<DomainReportEmployee> domains = new List<DomainReportEmployee>();
                //Get the Employee list from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETDOMAINREPORTASSOCIATES}" + employeeIds);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        domains = JsonConvert.DeserializeObject<List<DomainReportEmployee>>(json);
                        response.Items = domains;
                        if (domains.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No Employee data found";

                            m_Logger.LogError("No Employee data found");
                        }
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

        #region Get TalentPool Report Associates
        public async Task<ServiceListResponse<TalentPoolReportEmployee>> GetTalentPoolReportAssociates(int projectId)
        {

            ServiceListResponse<TalentPoolReportEmployee> response = new ServiceListResponse<TalentPoolReportEmployee>();
            try
            {
                List<TalentPoolReportEmployee> domains = new List<TalentPoolReportEmployee>();
                //Get the Employee list from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETTALENTPOOLREPORTASSOCIATES}" + projectId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        domains = JsonConvert.DeserializeObject<List<TalentPoolReportEmployee>>(json);
                        response.Items = domains;
                        if (domains.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No Employee data found";

                            m_Logger.LogError("No Employee data found");
                        }
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

        #region Get Skill Search Associates
        public async Task<ServiceListResponse<SkillSearchEmployee>> GetSkillSearchAssociates(AssociateSkillSearchFilter filter)
        {

            ServiceListResponse<SkillSearchEmployee> response = new ServiceListResponse<SkillSearchEmployee>();
            try
            {
                List<SkillSearchEmployee> associates = new List<SkillSearchEmployee>();
                //Get the Employee list from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETSKILLSEARCHASSOCIATES}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.PostAsync(httpRequestMessage.RequestUri.ToString(), filter, new JsonMediaTypeFormatter());

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        associates = JsonConvert.DeserializeObject<List<SkillSearchEmployee>>(json);
                        response.Items = associates;
                        if (associates.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No Associates data found";

                            m_Logger.LogError("No Associates data found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Associates data";

                        m_Logger.LogError("Error occured while fetching the Associates data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Associates data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region Get Active Employee Details
        public async Task<ServiceListResponse<GenericType>> GetActiveAssociates(List<int> employeeIds)
        {

            ServiceListResponse<GenericType> response = new ServiceListResponse<GenericType>();
            try
            {
                List<GenericType> associates = new List<GenericType>();
                //Get the Employee list from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETACTIVEASSOCIATES}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.PostAsync(httpRequestMessage.RequestUri.ToString(), employeeIds, new JsonMediaTypeFormatter());

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        associates = JsonConvert.DeserializeObject<List<GenericType>>(json);
                        response.Items = associates;
                        if (associates.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No Associates data found";

                            m_Logger.LogError("No Associates data found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the Associates data";

                        m_Logger.LogError("Error occured while fetching the Associates data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Associates data";

                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region Get Domain Report Details
        public async Task<ServiceListResponse<ServiceTypeCount>> GetServiceTypeResourceCount()
        {

            ServiceListResponse<ServiceTypeCount> response = new ServiceListResponse<ServiceTypeCount>();
            try
            {
                List<ServiceTypeCount> domains = new List<ServiceTypeCount>();
                //Get the Employee list from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETSERVICETYPECOUNT}");
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

        #region GetParkingSloteport
        public async Task<ServiceListResponse<ParkingSlotReport>> GetParkingSlotReport(ParkingSearchFilter parkingSearchFilter)
        {

            ServiceListResponse<ParkingSlotReport> response = new ServiceListResponse<ParkingSlotReport>();
            try
            {
                List<ParkingSlotReport> parkingSlotReport = new List<ParkingSlotReport>();
                //Get the Employee list from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +$"{ServicePaths.EmployeeEndPoint.GETPARKINGSLOTREPORT}");
                    httpRequestMessage.Headers.Add("Accept", "application/json");
                    HttpResponseMessage httpResponseMessage = await httpClientFactory.PostAsync(httpRequestMessage.RequestUri.ToString(), parkingSearchFilter, new JsonMediaTypeFormatter());

                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ServiceListResponse<ParkingSlotReport>>(json);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Parking Slot Reports data";

                m_Logger.LogError("Error occured while fetching the Parking Slot Reports data  ", ex.Message);
            }
            return response;
        }
        #endregion
    }
}
