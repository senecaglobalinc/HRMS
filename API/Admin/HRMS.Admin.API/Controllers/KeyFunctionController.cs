using System;
using System.Collections;
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
    public class KeyFunctionController : Controller
    {
        #region Global Variables

        private readonly IKeyFunctionService m_keyFunctionService;
        private readonly ILogger<KeyFunctionController> m_Logger;

        #endregion

        #region Constructor

        public KeyFunctionController(IKeyFunctionService keyFunctionService, ILogger<KeyFunctionController> logger)
        {
            m_keyFunctionService = keyFunctionService;
            m_Logger = logger;
        }

        #endregion

        #region  Create
        /// <summary>
        /// Create KeyFunction
        /// </summary>
        /// <param name="keyFunctionIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(SGRole keyFunctionIn)
        {
            m_Logger.LogInformation("Inserting record in SGRole table.");
            try
            {
                dynamic response = await m_keyFunctionService.Create(keyFunctionIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully created record in SGRole table.");
                    return Ok(response.KeyFunction);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating SGRole: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("SGRoleName: " + keyFunctionIn.SGRoleName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating SGRole: " + ex);
                return BadRequest("Error occurred while creating SGRole");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get KeyFunction List
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from  table.");

            try
            {
                var keyFunctionList = await m_keyFunctionService.GetAll(isActive);
                if (keyFunctionList == null)
                {
                    m_Logger.LogInformation("No records found in SGRolePrefi table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { keyFunctionList.Count} SGRole.");
                    return Ok(keyFunctionList);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching SGRole.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update KeyFunction
        /// </summary>
        /// <param name="keyFunctionIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(SGRole keyFunctionIn)
        {
            m_Logger.LogInformation("updating record in SGRole table.");
            try
            {
                dynamic response = await m_keyFunctionService.Update(keyFunctionIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully updated record in SGRole table.");
                    return Ok(response.KeyFunction);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating SGRole: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("SGRoleName: " + keyFunctionIn.SGRoleName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating SGRole: " + ex);
                return BadRequest("Error occurred while updating SGRole");
            }
        }
        #endregion
    }
}