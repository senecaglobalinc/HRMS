using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Admin.Entities;
using HRMS.Admin.Types;
using HRMS.Admin.API.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.Admin.API.Controllers
{
    [Route("admin/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class SeniorityController : Controller
    {
        #region Global Variables

        private readonly ISeniorityService m_seniorityService;
        private readonly ILogger<SeniorityController> m_Logger;

        #endregion

        #region Constructor

        public SeniorityController(ISeniorityService seniorityService, ILogger<SeniorityController> logger)
        {
            m_seniorityService = seniorityService;
            m_Logger = logger;
        }

        #endregion

        #region  Create
        /// <summary>
        /// Create Seniority
        /// </summary>
        /// <param name="seniorityIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(SGRolePrefix seniorityIn)
        {
            m_Logger.LogInformation("Inserting record in SGRolePrefix table.");
            try
            {
                dynamic response = await m_seniorityService.Create(seniorityIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully created record in SGRolePrefix table.");
                    return Ok(response.Seniority);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating SGRolePrefix: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("PrefixName: " + seniorityIn.PrefixName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating SGRolePrefix: " + ex);
                return BadRequest("Error occurred while creating SGRolePrefix");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get Seniority List
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from  table.");

            try
            {
                var seniorityList = await m_seniorityService.GetAll(isActive);
                if (seniorityList == null)
                {
                    m_Logger.LogInformation("No records found in SGRolePrefi table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { seniorityList.Count} SGRolePrefix.");
                    return Ok(seniorityList);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching SGRolePrefix.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Seniority
        /// </summary>
        /// <param name="seniorityIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(SGRolePrefix seniorityIn)
        {
            m_Logger.LogInformation("updating record in SGRolePrefix table.");
            try
            {
                dynamic response = await m_seniorityService.Update(seniorityIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully updated record in SGRolePrefix table.");
                    return Ok(response.Seniority);
                }
                else
                {
                    //Add exception to logger
                    m_Logger.LogError("Error occurred while updating SGRolePrefix: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("PrefixName: " + seniorityIn.PrefixName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating SGRolePrefix: " + ex);
                return BadRequest("Error occurred while updating SGRolePrefix");
            }
        }
        #endregion
    }
}