using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Constants;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types.External;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Service.External
{
  public  class KRAService:IKRAService
    {
        #region Global Variables
        private readonly ILogger<OrganizationService> m_Logger;
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
        public KRAService(ILogger<OrganizationService> logger,
        IHttpClientFactory clientFactory,
        IOptions<APIEndPoints> apiEndPoints)
        {
            m_Logger = logger;
            m_ClientFactory = clientFactory;
            m_ApiEndPoints = apiEndPoints?.Value;
        }

        #endregion

        #region GetEmpKRARoleTypeByGrade
        /// <summary>
        /// GetAllGrades
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<KRARoleTypes>> GetEmpKRARoleTypeByGrade(int gradeId)
        {
            var Roletype = new List<KRARoleTypes>();
            var response = new ServiceListResponse<KRARoleTypes>();
            return response;

            /*
             * COMMENTED BY KALYAN PENUMUTCHU ON 25MAY2024 AS PART OF AWS CLOUD MIGRATION. THIS NEEDS TO BE OPENED UP ONCE KRA MODULE IS DEPLOYED TO CLOUD
            try
            {
                //Get the RoleTypes information from KRA Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("KRAClient"))                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.KRAEndPoint + $"{ServicePaths.KRAEndPoint.GETROLETYPEBYGRADEID}"+gradeId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        var json =  httpResponseMessage.Content.ReadAsStringAsync().Result.ToString();
                        //JArray array = JArray.Parse(json);
                        JObject obj = JObject.Parse(json);
                        Roletype = JsonConvert.DeserializeObject<List<KRARoleTypes>>(obj["Items"].ToString());
                        
                        response.Items = Roletype;
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
            */
        }

        #endregion
    }
}
