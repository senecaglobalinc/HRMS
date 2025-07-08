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
    [ApiController]
    [Route("admin/api/v1/[controller]")]
    [Authorize]
    public class DesignationController : Controller
    {
        #region Global Variables

        private readonly IDesignationService m_DesignationService;
        private readonly ILogger<DesignationController> m_Logger;

        #endregion

        #region Constructor

        public DesignationController(IDesignationService designationService, ILogger<DesignationController> logger)
        {
            m_DesignationService = designationService;
            m_Logger = logger;
        } 

        #endregion

        #region Create
        /// <summary>
        /// Create Designation
        /// </summary>
        /// <param name="designationIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Designation designationIn)
        {

            m_Logger.LogInformation("Inserting record in designations table.");
            try
            {
                dynamic response = await m_DesignationService.Create(designationIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating designation: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in designation's table.");
                    return Ok(response.Designation);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating designation: " + ex);
                return BadRequest("Error occurred while creating designation.");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetDesignationDetails
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool? isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from Designations table.");

            try
            {
                var designations = await m_DesignationService.GetAll(isActive);
                if (designations == null)
                {
                    m_Logger.LogInformation("No records found in Designations table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { designations.Count} designations.");
                    return Ok(designations);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetDesignationsForDropdown
        /// <summary>
        /// GetDesignationsForDropdown
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetDesignationsForDropdown")]
        public async Task<ActionResult<IEnumerable>> GetDesignationsForDropdown()
        {
            m_Logger.LogInformation("Retrieving records from clients table.");

            try
            {
                var designations = await m_DesignationService.GetDesignationsForDropdown();
                if (designations == null)
                {
                    m_Logger.LogInformation("No records found in Designations table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { designations.Count} designations.");
                    return Ok(designations);
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
        /// Get Designation Details by id
        /// </summary>
        /// <param name="designationId"></param>
        /// <returns></returns>
        [HttpGet("GetById/{designationId}")]
        public async Task<ActionResult<Designation>> GetById(int designationId)
        {
            m_Logger.LogInformation("Retrieving records from Designations table.");

            try
            {
                var designations = await m_DesignationService.GetById(designationId);
                if (designations == null)
                {
                    m_Logger.LogInformation("No records found in Designations table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning designations.");
                    return Ok(designations);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetByCode
        /// <summary>
        /// Get Designation Details by code
        /// </summary>
        /// <param name="designationCode"></param>
        /// <returns></returns>
        [HttpGet("GetByCode/{designationCode}")]
        public async Task<ActionResult<Designation>> GetByCode(string designationCode)
        {
            m_Logger.LogInformation("Retrieving records from Designations table.");

            try
            {
                var designations = await m_DesignationService.GetByCode(designationCode);
                if (designations == null)
                {
                    m_Logger.LogInformation("No records found in Designations table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning designations.");
                    return Ok(designations);
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
        /// Update Designation
        /// </summary>
        /// <param name="designationIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(Designation designationIn)
        {
            m_Logger.LogInformation("Updating record in Designations table.");

            try
            {
                dynamic response = await m_DesignationService.Update(designationIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating designation: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in designation's table.");
                    return Ok(response.Designation);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating designation: " + ex);
                return BadRequest("Error occurred while updating designation.");
            }
        }
        #endregion

        #region GetBySearchString
        /// <summary>
        /// GetBySearchString
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [HttpGet("GetBySearchString/{searchString}")]
        public async Task<ActionResult<IEnumerable>> GetBySearchString(string searchString)
        {
            m_Logger.LogInformation("Retrieving records from Designations table.");

            try
            {
                var designations = await m_DesignationService.GetBySearchString(searchString);
                if (designations == null)
                {
                    m_Logger.LogInformation("No records found in Designations table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { designations.Count} designations.");
                    return Ok(designations);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }


        }
        #endregion

    }
}