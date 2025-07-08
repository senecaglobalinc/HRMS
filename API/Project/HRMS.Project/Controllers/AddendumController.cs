using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using HRMS.Project.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMS.Project.API
{
    /// <summary>
    /// Controller for Addendum object.
    /// </summary>
    [Route("project/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AddendumController : Controller
    {
        #region Global Variables

        private readonly IAddendumService m_AddendumService;
        private readonly ILogger<AddendumController> m_Logger;

        #endregion

        #region Constructor
        public AddendumController(IAddendumService addendumService,
            ILogger<AddendumController> logger)
        {
            m_AddendumService = addendumService;
            m_Logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates new Addendum
        /// </summary>
        /// <param name="addendumRequest"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(AddendumRequest addendumRequest)
        {
            m_Logger.LogInformation("Inserting record in addendum's table.");
            try
            {
                var response = await m_AddendumService.Create(addendumRequest);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogInformation("Error occurred while creating addendum: " + (string)response.Message);
                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in addendum's table.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Project Id: " + addendumRequest.ProjectId);
                m_Logger.LogError("SOW Id: " + addendumRequest.SOWId);
                m_Logger.LogError("Addendum No: " + addendumRequest.AddendumNo);
                m_Logger.LogError("Role: " + addendumRequest.RoleName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating addendum: " + ex);

                return Ok("Error occurred while creating addendum.");
            }
        }
        #endregion

        #region GetByIdAndProjectId
        /// <summary>
        /// GetByIdAndProjectId 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectId"></param>
        /// <param name="roleName"></param>
        /// <returns>Addendum</returns>
        [HttpGet("GetByIdAndProjectId")]
        public async Task<ActionResult<Addendum>> GetByIdAndProjectId(int projectId, int id, string roleName)
        {
            m_Logger.LogInformation("Retrieving records from addendum table.");

            try
            {
                var addendum = await m_AddendumService.GetByIdAndProjectId(id, projectId, roleName);
                if (addendum.Item == null)
                {
                    m_Logger.LogInformation("No records found in addendum table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning addendum.");
                    return Ok(addendum.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation($"Id= {id}");
                m_Logger.LogInformation($"ProjectId= {projectId}");
                m_Logger.LogInformation($"RoleName= {roleName}");
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetAllBySOWIdAndProjectId
        /// <summary>
        /// GetAllBySOWIdAndProjectId 
        /// </summary>
        /// <returns></returns>
        /// <param name="sowId"></param>
        /// <param name="projectId"></param>
        [HttpGet("GetAllBySOWIdAndProjectId")]
        public async Task<ActionResult<IList<Addendum>>> GetAllBySOWIdAndProjectId(int sowId, int projectId)
        {
            m_Logger.LogInformation("Retrieving record from addendum table.");

            try
            {
                var addendums = await m_AddendumService.GetAllBySOWIdAndProjectId(sowId, projectId);
                if (addendums == null)
                {
                    m_Logger.LogInformation("No record found in addendum table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning {addendums.Items.Count}  addendum's.");
                    return Ok(addendums.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation($"Id= {sowId}");
                m_Logger.LogInformation($"ProjectId= {projectId}");
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Create Addendum
        /// </summary>
        /// <param name="addendumRequest"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(AddendumRequest addendumRequest)
        {
            m_Logger.LogInformation("Updating record in addendum's table.");
            try
            {
                var response = await m_AddendumService.Update(addendumRequest);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogInformation("Error occurred while updating Addendum: " + (string)response.Message);
                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in Addendum's table.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Project Id: " + addendumRequest.ProjectId);
                m_Logger.LogError("SOW Id: " + addendumRequest.SOWId);
                m_Logger.LogError("Addendum No: " + addendumRequest.AddendumNo);
                m_Logger.LogError("Role: " + addendumRequest.RoleName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating Addendum: " + ex);

                return Ok("Error occurred while updating Addendum.");
            }
        }
        #endregion
    }
}
