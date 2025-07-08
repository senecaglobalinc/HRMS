using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Types;
using HRMS.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net.Http;
using HRMS.Admin.Infrastructure;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Dynamic;
using System;
using HRMS.Admin.Infrastructure.Models.Domain;

namespace HRMS.Admin.Service
{
    /// <summary>
    /// Service class to get the Client details
    /// </summary>
    public class ClientService : IClientService
    {
        #region Global Varibles

        private readonly ILogger<ClientService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly IMapper m_Mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;

        #endregion

        #region ClientService
        public ClientService(ILogger<ClientService> logger,
            AdminContext adminContext,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Client, Client>();
            });
            m_Mapper = config.CreateMapper();

            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints?.Value;
        }
        #endregion

        #region Create
        /// <summary>
        /// Create client
        /// </summary>
        /// <param name="clientIn">ClientDetails information</param>
        /// <returns></returns>
        public async Task<dynamic> Create(Client clientIn)
        {
            int isCreated;
            //The below code is for cross webapi communications
            //var httpClientFactory = m_clientFactory.CreateClient();
            //var response = httpClientFactory.GetStringAsync(m_apiEndPoints.AssociateEndPoint + "1").Result;
            //if (response == null)
            //{
            //    return null;
            //}
            //var employee = JsonConvert.DeserializeObject<Employee>(response.ToString());
            //employee.PersonalEmailAddress = "Test@test.com";
            //var result = await httpClientFactory.PutAsJsonAsync(m_apiEndPoints.AssociateEndPoint + "1", employee);
            //var retvalue = await result.Content.ReadAsStringAsync();

            m_Logger.LogInformation("ClientService: Calling \"Create\" method.");
            m_Logger.LogInformation("ClientService: Verifying client already exists?");

            Client clientAlreadyExits =
                await GetByCode(clientIn.ClientCode);

            Client clientAlreadyExitsByName =
                await GetByName(clientIn.ClientName);

            m_Logger.LogInformation("ClientService: Verifying client exists?");
            if (clientAlreadyExits != null || clientAlreadyExitsByName != null)
                return CreateResponse(null, false, "Client already exists.");
            else
                m_Logger.LogInformation("ClientService: Client does not already exists.");

            Client client = new Client();

            if (!clientIn.IsActive.HasValue)
                clientIn.IsActive = true;

            m_Logger.LogInformation("Calling CreateClient method in ClientService");

            m_Mapper.Map<Client, Client>(clientIn, client);

            client.ClientNameHash = Utility.SHA1HashStringForUTF8String(client.ClientName);

            m_AdminContext.Clients.Add(client);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ClientService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
            {
                m_Logger.LogInformation("Client created successfully.");
                return CreateResponse(client, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No client created.");
        }

        #endregion

        #region GetAll
        /// <summary>
        /// Gets the client details
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<List<Client>> GetAll(bool isActive = true) =>
                        await m_AdminContext.Clients.Where(cl => cl.IsActive == isActive).OrderBy(x => x.ClientName).ToListAsync();

        #endregion

        #region GetClientsForDropdown
        /// <summary>
        /// Gets the client details
        /// </summary>        
        /// <returns></returns>
        public async Task<List<GenericType>> GetClientsForDropdown() =>
                        await m_AdminContext.Clients.Where(cl => cl.IsActive == true).Select(ci => new GenericType { Id =ci.ClientId , Name = ci.ClientName }).OrderBy(x => x.Name).ToListAsync();

        #endregion

        #region GetById
        /// <summary>
        /// Get client by id
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <returns></returns>
        public async Task<Client> GetById(int clientId) =>
                        await m_AdminContext.Clients.FindAsync(clientId);

        #endregion

        #region GetByIds
        /// <summary>
        /// Get clients by ids
        /// </summary>
        /// <param name="clientIds">Client Id</param>
        /// <returns></returns>
        public async Task<List<Client>> GetByIds(int[] clientIds) =>
                        await m_AdminContext.Clients.Where(q => (clientIds.Contains(q.ClientId))).ToListAsync();

        #endregion

        #region GetByName
        /// <summary>
        /// Get client by code
        /// </summary>
        /// <param name="clientCode">Client code</param>
        /// <returns></returns>

        public async Task<Client> GetByName(string clientName) =>
            await m_AdminContext.Clients.Where(cl => cl.ClientName.ToLower().Trim() == clientName.ToLower().Trim() && cl.IsActive == true).FirstOrDefaultAsync();
        #endregion

        #region Update
        /// <summary>
        /// Updates the client information
        /// </summary>
        /// <param name="client">ClientDetails information</param>
        /// <returns></returns>
        public async Task<dynamic> Update(Client clientIn)
        {
            int isCreated;

            m_Logger.LogInformation("Calling UpdateClient method in ClientService");

            var client = m_AdminContext.Clients.Find(clientIn.ClientId);

            Client clientAlreadyExits =
                await GetByCode(clientIn.ClientCode);

            Client clientAlreadyExitsByName =
                await GetByName(clientIn.ClientName);

            m_Logger.LogInformation("ClientService: Verifying client exists?");
            if (clientAlreadyExits != null && clientAlreadyExits.ClientId != client.ClientId)
                return CreateResponse(null, false, "Client already exists.");
            else
                m_Logger.LogInformation("ClientService: Client does not already exists.");

            if (clientAlreadyExitsByName != null && clientAlreadyExitsByName.ClientId != client.ClientId)
                return CreateResponse(null, false, "Client name already exists.");
            else
                m_Logger.LogInformation("ClientService: Client does not already exists.");

            if (!clientIn.IsActive.HasValue)
                clientIn.IsActive = client.IsActive;

            clientIn.CreatedBy = client.CreatedBy;
            clientIn.CreatedDate = client.CreatedDate;

            m_Mapper.Map<Client, Client>(clientIn, client);

            client.ClientNameHash = Utility.SHA1HashStringForUTF8String(client.ClientName);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in ClientService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
            {
                m_Logger.LogInformation("Updating CreateClient record in ClientService");
                return CreateResponse(client, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record found Client table to update in ClientService");
        }
        #endregion

        //Helpers
        #region CreateResponse
        private dynamic CreateResponse(Client client, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("ClientService: Calling CreateResponse method.");

            dynamic response = new ExpandoObject();
            response.Client = client;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("ClientService: Response object created.");

            return response;
        }
        #endregion


        #region GetByCode
        /// <summary>
        /// this method fetches client based on client code.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns>CategoryMaster</returns>
        private async Task<Client> GetByCode(string clientCode) =>
            await m_AdminContext.Clients.Where(cl => cl.ClientCode.ToLower().Trim() == clientCode.ToLower().Trim() && cl.IsActive == true)
                        .FirstOrDefaultAsync();
        #endregion

    }
}
