using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using HRMS.Project.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMS.Project.API
{
    [Route("project/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class SOWController : Controller
    {
        #region Global Variables

        private readonly ISOWService m_SOWService;
        private readonly ILogger<SOWController> m_Logger;

        #endregion

        #region Constructor
        public SOWController(ISOWService sowService,
            ILogger<SOWController> logger)
        {
            m_SOWService = sowService;
            m_Logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// Create SOW
        /// </summary>
        /// <param name="sowRequest"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(SOWRequest sowRequest)
        {
            m_Logger.LogInformation("Inserting record in sow's table.");
            try
            {
                ServiceResponse<int> response = await m_SOWService.Create(sowRequest);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogInformation("Error occurred while creating sow: " + (string)response.Message);
                    return Ok(response.Item);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in sow's table.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Project Id: " + sowRequest.ProjectId);
                m_Logger.LogError("SOW Id: " + sowRequest.SOWId);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating sow: " + ex);

                return BadRequest("Error occurred while creating sow.");
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// This methode deletes SOW.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            m_Logger.LogInformation("Deleting record in SOW's table.");

            try
            {
                ServiceResponse<int> response = await m_SOWService.Delete(id);
                if (!response.IsSuccessful)
                {
                    //Extra information
                    m_Logger.LogError("SOW Id: " + id);

                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while deleting SOW: " + (string)response.Message);

                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted record in SOW's table.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("SOW Id: " + id);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while deleting SOW: " + ex);

                return BadRequest("Error occurred while deleting SOW.");
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
            m_Logger.LogInformation("Retrieving records from SOW table.");

            try
            {
                ServiceListResponse<SOW> response = await m_SOWService.GetAllByProjectId(projectId);
                if (response.Items == null)
                {
                    m_Logger.LogInformation("No records found in SOW table.");
                    return Ok(response.Items);
                }
                else
                {
                    m_Logger.LogInformation($"Returning { response.Items.Count } SOWs.");
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

        #region GetByIdAndProjectId
        /// <summary>
        /// GetByIdAndProjectId Projects
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetByIdAndProjectId")]
        public async Task<ActionResult<IEnumerable>> GetByIdAndProjectId(int id, int projectId, string roleName)
        {
            m_Logger.LogInformation("Retrieving record from SOW table.");

            try
            {
                ServiceResponse<SOW> response = await m_SOWService.GetByIdAndProjectId(id, projectId, roleName);
                if (response.Item == null)
                {
                    m_Logger.LogInformation("No record found in SOW table.");
                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning SOW.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation($"Id= {id}");
                m_Logger.LogInformation($"ProjectId= {projectId}");
                m_Logger.LogInformation($"roleName= {roleName}");
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Create SOW
        /// </summary>
        /// <param name="sowRequest"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(SOWRequest sowRequest)
        {
            m_Logger.LogInformation("Updating record in sow's table.");
            try
            {
                ServiceResponse<int> response = await m_SOWService.Update(sowRequest);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogInformation("Error occurred while updating sow: " + (string)response.Message);
                    return Ok(response.Item);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in sow's table.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Project Id: " + sowRequest.ProjectId);
                m_Logger.LogError("SOW Id: " + sowRequest.SOWId);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating sow: " + ex);

                return BadRequest("Error occurred while updating sow.");
            }
        }
        #endregion
    }
}
