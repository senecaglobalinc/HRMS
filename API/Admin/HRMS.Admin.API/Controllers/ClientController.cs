using HRMS.Admin.Entities;
using HRMS.Admin.Types;
using HRMS.Admin.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("admin/api/v1/[controller]")]
    public class ClientController : Controller
    {
        #region Global Variables

        private readonly IClientService m_ClientService;
        private readonly ILogger<ClientController> m_Logger;

        #endregion

        #region Constructor
        public ClientController(IClientService clientService, ILogger<ClientController> logger)
        {
            m_ClientService = clientService;
            m_Logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// Create Client
        /// </summary>
        /// <param name="clientIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Client clientIn)
        {

            m_Logger.LogInformation("Inserting record in clients table.");
            try
            {
                dynamic response = await m_ClientService.Create(clientIn);

                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating client: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in client's table.");
                    return Ok(response.Client);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Client Code: " + clientIn.ClientCode);
                m_Logger.LogError("Client Name: " + clientIn.ClientName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating client: " + ex);

                return BadRequest("Error occurred while creating client.");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetClientsDetails
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from clients table.");

            try
            {
                var clients = await m_ClientService.GetAll(isActive);
                if (clients == null)
                {
                    m_Logger.LogInformation("No records found in clients table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { clients.Count} clients.");
                    return Ok(clients);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }


        }
        #endregion

        #region GetClientsForDropdown
        /// <summary>
        /// GetClientsForDropdown
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetClientsForDropdown")]
        public async Task<ActionResult<IEnumerable>> GetClientsForDropdown()
        {
            m_Logger.LogInformation("Retrieving records from clients table.");

            try
            {
                var clients = await m_ClientService.GetClientsForDropdown();
                if (clients == null)
                {
                    m_Logger.LogInformation("No records found in clients table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { clients.Count} clients.");
                    return Ok(clients);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }


        }
        #endregion

        #region GetByIds
        /// <summary>
        /// Gets Clients by Ids
        /// </summary>
        /// <param name="clientIds"></param>
        /// <returns></returns>
        [HttpGet("GetByIds")]
        public async Task<ActionResult<IEnumerable>> GetByIds([FromQuery] int[] clientIds)
        {
            m_Logger.LogInformation("Retrieving records from clients table.");

            try
            {
                var clients = await m_ClientService.GetByIds(clientIds);
                if (clients == null)
                {
                    m_Logger.LogInformation("No records found in clients table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { clients.Count} clients.");
                    return Ok(clients);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }


        }
        #endregion

        #region GetById
        /// <summary>
        /// GetClientById
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet("GetById/{clientId}")]
        public async Task<ActionResult<Client>> GetById(int clientId)
        {
            m_Logger.LogInformation($"Retrieving records from clients table by {clientId}.");

            try
            {
                var client = await m_ClientService.GetById(clientId);
                if (client == null)
                {
                    m_Logger.LogInformation($"No records found for clientID {clientId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for clientID {clientId}.");
                    return Ok(client);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region Update
        /// <summary>
        /// Update Client
        /// </summary>
        /// <param name="clientIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(Client clientIn)
        {
            m_Logger.LogInformation("Updating record in clients table.");

            try
            {
                var response = await m_ClientService.Update(clientIn);

                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating client: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in client's table.");
                    return Ok(response.Client);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Client Code: " + clientIn.ClientCode);
                m_Logger.LogError("Client Name: " + clientIn.ClientName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating client: " + ex);

                return BadRequest("Error occurred while updating client.");
            }
        }
        #endregion
    }
}