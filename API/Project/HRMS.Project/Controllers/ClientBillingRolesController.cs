using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using HRMS.Project.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMS.Project.API
{
    [Route("project/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientBillingRolesController : Controller
    {
        #region Global Variables

        private readonly IClientBillingRoleService m_ClientBillingRoleService;
        private readonly ILogger<ClientBillingRolesController> m_Logger;

        #endregion

        #region Constructor
        public ClientBillingRolesController(IClientBillingRoleService clientBillingRoleService,
            ILogger<ClientBillingRolesController> logger)
        {
            m_ClientBillingRoleService = clientBillingRoleService;
            m_Logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// Create Client billing role
        /// </summary>
        /// <param name="clientBillingRoleIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ClientBillingRoles clientBillingRoleIn)
        {
            m_Logger.LogInformation("Inserting record in Client Billing Role's table.");
            try
            {
                var response = await m_ClientBillingRoleService.Create(clientBillingRoleIn);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogInformation("Error occurred while creating Client Billing Role: " + (string)response.Message);
                    //return BadRequest(response.Message);
                    return Ok(response.Item);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in Client Billing Role's table.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Project Id: " + clientBillingRoleIn.ProjectId);
                m_Logger.LogError("Client Billing Percentage: " + clientBillingRoleIn.ClientBillingPercentage);
                m_Logger.LogError("No Of Positions: " + clientBillingRoleIn.NoOfPositions);
                m_Logger.LogError("Start Date: " + clientBillingRoleIn.StartDate);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating Client Billing Role: " + ex);

                return BadRequest("Error occurred while creating Client Billing Role.");
            }
        }
        #endregion

        #region GetAllByProjectId
        /// <summary>
        /// GetAllByProjectId 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllByProjectId")]
        public async Task<ActionResult<IEnumerable>> GetAllByProjectId(int projectId)
        {
            m_Logger.LogInformation("Retrieving records from CBR table.");

            try
            {
                var response = await m_ClientBillingRoleService.GetAllByProjectId(projectId);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogInformation("Error occured while fetching client billing role.");
                    return Ok(response.Message);
                }
                else if (response.Items != null
                    && response.Items.Count <= 0)
                {
                    m_Logger.LogInformation("No records found.");
                    return Ok(response.Items);
                }
                else
                {
                    m_Logger.LogInformation($"Returning { response.Items.Count } " +
                        $"Client billing roles.");
                    return Ok(response.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation($"ProjectId= {projectId}");
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates Client billing role
        /// </summary>
        /// <param name="clientBillingRoleIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(ClientBillingRoles clientBillingRoleIn)
        {
            m_Logger.LogInformation("Updating record in client billing role's table.");
            try
            {
                var response = await m_ClientBillingRoleService.Update(clientBillingRoleIn);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogInformation("Error occurred while updating client billing role: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in client billing role's table.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Project Id: " + clientBillingRoleIn.ProjectId);
                m_Logger.LogError("Client Billing Percentage: " + clientBillingRoleIn.ClientBillingPercentage);
                m_Logger.LogError("No Of Positions: " + clientBillingRoleIn.NoOfPositions);
                m_Logger.LogError("Start Date: " + clientBillingRoleIn.StartDate);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating client billing role: " + ex);

                return BadRequest("Error occurred while updating client billing role.");
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Deletes Client billing role
        /// </summary>
        /// <param name="clientBillingRoleId"></param>
        /// <returns></returns>
        [HttpPost("Delete/{clientBillingRoleId}")]
        public async Task<IActionResult> Delete([FromRoute] int clientBillingRoleId)
        {
            m_Logger.LogInformation("Deleting record in client billing role's table.");
            try
            {
                var response = await m_ClientBillingRoleService.Delete(clientBillingRoleId);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogInformation("Error occurred while deleting client billing role: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted record in client billing role's table.");
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError($"Client Billing Role Id: {clientBillingRoleId}");

                //Add exeption to logger
                m_Logger.LogError("Error occurred while deleting client billing role: " + ex);

                return BadRequest("Error occurred while deleting client billing role.");
            }
        }
        #endregion

        #region Close
        /// <summary>
        /// Close Client billing role
        /// </summary>
        /// <param name="clientBillingRoleId"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("Close/{clientBillingRoleId}/{endDate}/{reason}")]
        public async Task<IActionResult> Close([FromRoute] int clientBillingRoleId,[FromRoute] DateTime endDate,string reason)
        {
            m_Logger.LogInformation("Closing record in client billing role's table.");
            try
            {
                var response = await m_ClientBillingRoleService.Close(clientBillingRoleId, endDate,reason);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogInformation("Error occurred while closing client billing role: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully closed record in client billing role's table.");
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError($"Client Billing Role Id: {clientBillingRoleId}");
                m_Logger.LogError($"End date: {endDate}");

                //Add exeption to logger
                m_Logger.LogError("Error occurred while closing client billing role: " + ex);

                return BadRequest("Error occurred while closing client billing role.");
            }
        }
        #endregion

    }
}
