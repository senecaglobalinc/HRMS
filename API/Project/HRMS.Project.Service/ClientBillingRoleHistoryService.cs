using AutoMapper;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Project.Service
{
    public class ClientBillingRoleHistoryService : IClientBillingRoleHistoryService
    {
        #region Global Varibles

        private readonly ILogger<ClientBillingRoleHistoryService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_Mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;

        #endregion

        #region ClientBillingRolesService
        public ClientBillingRoleHistoryService(ILogger<ClientBillingRoleHistoryService> logger,
            ProjectDBContext projectContext,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints, IMapper mapper)
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;
            
            m_Mapper = mapper;

            m_clientFactory = clientFactory;
            //m_apiEndPoints = apiEndPoints?.Value;
            m_apiEndPoints = apiEndPoints != null ? apiEndPoints.Value : null;
        }
        #endregion

        #region Create
        /// <summary>
        /// This method create Client Billing Role. 
        /// </summary>
        /// <param name="clientBillingRolesHistoryIn"></param>
        /// <returns>ClientBillingRoleResponse</returns>
        public async Task<ServiceResponse<ClientBillingRolesHistory>> Create(ClientBillingRolesHistory clientBillingRolesHistoryIn)
        {
            int isCreated;
            ServiceResponse<ClientBillingRolesHistory> response;
            m_Logger.LogInformation("Calling \"Create\" method in ClientBillingRoleService");

            ClientBillingRolesHistory clientBillingRolesHistory = new ClientBillingRolesHistory();

            if (!clientBillingRolesHistory.IsActive.HasValue)
                clientBillingRolesHistory.IsActive = true;

            m_Logger.LogInformation("Assigning to automapper.");

            clientBillingRolesHistory =  m_Mapper.Map<ClientBillingRolesHistory, ClientBillingRolesHistory>(clientBillingRolesHistoryIn);

            m_ProjectContext.ClientBillingRoleHistory.Add(clientBillingRolesHistory);
            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ClientBillingRoleService");
            isCreated = await m_ProjectContext.SaveChangesAsync();

            if (isCreated > 0)
            {
               return response = new ServiceResponse<ClientBillingRolesHistory>() { 
                    Item = clientBillingRolesHistory,
                    IsSuccessful = true,
                    Message = string.Empty
                };
            }
            else
            {
                return response = new ServiceResponse<ClientBillingRolesHistory>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "No Client Billing Role created."
                };
            }
        }

        #endregion

        //Private Methods
        #region GetExistingClientBillingRole
        /// <summary>
        /// This method gets resource allocation for billable resources
        /// </summary>
        /// <param name="noOfPositions"></param>
        /// <param name="clientBillingPercentage"></param>
        /// <param name="startDate"></param>
        /// <param name="projectId"></param>
        /// <returns>ResourceAllocationResponse</returns>
        private async Task<ClientBillingRoles> GetExistingClientBillingRole(int? noOfPositions, int? clientBillingPercentage,
            DateTime? startDate, int? projectId)
        {
            return await m_ProjectContext.ClientBillingRoles
                        .Where(
                                cbr => cbr.NoOfPositions == noOfPositions
                                      && cbr.ClientBillingPercentage == clientBillingPercentage
                                      && cbr.StartDate == startDate
                                      && cbr.ProjectId == projectId
                              ).FirstOrDefaultAsync();
        }
        #endregion

        #region GetExistingClientBillingRole
        /// <summary>
        /// This method gets resource allocation for billable resources
        /// </summary>
        /// <param name="noOfPositions"></param>
        /// <param name="clientBillingPercentage"></param>
        /// <param name="startDate"></param>
        /// <param name="projectId"></param>
        /// <param name="clientBillingRoleId"></param>
        /// <returns>ClientBillingRoles</returns>
        private async Task<ClientBillingRoles> GetExistingClientBillingRole(int? noOfPositions, int? clientBillingPercentage,
                                                                 DateTime? startDate, int? projectId, int clientBillingRoleId)
        {
            return await m_ProjectContext.ClientBillingRoles
                        .Where(
                                cbr => cbr.NoOfPositions == noOfPositions
                                      && cbr.ClientBillingPercentage == clientBillingPercentage
                                      && cbr.StartDate == startDate
                                      && cbr.ProjectId == projectId
                                      && cbr.ClientBillingRoleId != clientBillingRoleId
                              ).FirstOrDefaultAsync();
        }
        #endregion

        #region GetStatusesByCategoryName
        private async Task<List<Status>> GetStatusesByCategoryName(HttpClient httpClientFactory, string categoryName)
        {
            httpClientFactory.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClientFactory.GetStringAsync(m_apiEndPoints.OrgEndPoint +
                "Status/GetAllByCategoryName?categoryName=" + categoryName);

            if (response == null)
                return null;

            return JsonConvert.DeserializeObject<List<Status>>(response.ToString());
        }
        #endregion
    }
}
