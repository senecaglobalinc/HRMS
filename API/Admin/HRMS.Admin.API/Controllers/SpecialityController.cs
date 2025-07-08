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
    public class SpecialityController : Controller
    {
        #region Global Variables

        private readonly ISpecialityService m_specialityService;
        private readonly ILogger<SpecialityController> m_Logger;

        #endregion

        #region Constructor

        public SpecialityController(ISpecialityService specialityService, ILogger<SpecialityController> logger)
        {
            m_specialityService = specialityService;
            m_Logger = logger;
        }

        #endregion

        #region  Create
        /// <summary>
        /// Create Speciality
        /// </summary>
        /// <param name="specialityIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(SGRoleSuffix specialityIn)
        {
            m_Logger.LogInformation("Inserting record in SGRoleSuffix table.");
            try
            {
                dynamic response = await m_specialityService.Create(specialityIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully created record in SGRoleSuffix table.");
                    return Ok(response.Speciality);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating SGRoleSuffix: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("SuffixName: " + specialityIn.SuffixName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating SGRoleSuffix: " + ex);
                return BadRequest("Error occurred while creating SGRoleSuffix");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get Speciality List
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from  table.");

            try
            {
                var specialityList = await m_specialityService.GetAll(isActive);
                if (specialityList == null)
                {
                    m_Logger.LogInformation("No records found in SGRolePrefi table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { specialityList.Count} SGRoleSuffix.");
                    return Ok(specialityList);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching SGRoleSuffix.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Speciality
        /// </summary>
        /// <param name="specialityIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(SGRoleSuffix specialityIn)
        {
            m_Logger.LogInformation("updating record in SGRoleSuffix table.");
            try
            {
                dynamic response = await m_specialityService.Update(specialityIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully updated record in SGRoleSuffix table.");
                    return Ok(response.Speciality);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating SGRoleSuffix: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("SuffixName: " + specialityIn.SuffixName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating SGRoleSuffix: " + ex);
                return BadRequest("Error occurred while updating SGRoleSuffix");
            }
        }
        #endregion
    }
}