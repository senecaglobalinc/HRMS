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
    [Route("admin/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ExitTypeController : Controller
    {
        #region Global Variables

        private readonly IExitTypeService m_ExitTypeService;
        private readonly ILogger<ExitTypeController> m_Logger;

        #endregion

        #region Constructor

        public ExitTypeController(IExitTypeService exitTypeService, ILogger<ExitTypeController> logger)
        {
            m_ExitTypeService = exitTypeService;
            m_Logger = logger;
        }

        #endregion

        #region  Create
        /// <summary>
        /// CreateExitType
        /// </summary>
        /// <param name="exitTypeIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ExitType exitTypeIn)
        {
            m_Logger.LogInformation("Inserting record in ExitType table.");
            try
            {
                dynamic response = await m_ExitTypeService.Create(exitTypeIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully created record in ExitType table.");
                    return Ok(response.ExitType);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating ExitType: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("ExitType: " + exitTypeIn.Description);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating ExitType: " + ex);
                return BadRequest("Error occurred while creating ExitType");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetExitTypes
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from ExitType table.");

            try
            {
                var ExitTypes = await m_ExitTypeService.GetAll(isActive);
                if (ExitTypes == null)
                {
                    m_Logger.LogInformation("No records found in ExitType table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { ExitTypes.Count} ExitType.");
                    return Ok(ExitTypes);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching ExitType.");
            }
        }
        #endregion

        #region GetByExitTypeId
        /// <summary>
        /// Gets the ExitType by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<ActionResult<ExitType>> GetByExitTypeId(int Id)
        {
            m_Logger.LogInformation($"Retrieving records from ExitType table by {Id}.");

            try
            {
                var ExitType = await m_ExitTypeService.GetByExitTypeId(Id);
                if (ExitType == null)
                {
                    m_Logger.LogInformation($"No records found for ExitType {Id}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for ExitType {Id}.");
                    return Ok(ExitType);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetExitTypesForDropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetExitTypesForDropdown")]
        public async Task<ActionResult<IEnumerable>> GetExitTypesForDropdown()
        {
            m_Logger.LogInformation("Retrieving records from ExitType table.");

            try
            {
                var ExitTypes = await m_ExitTypeService.GetExitTypesForDropdown();
                if (ExitTypes == null)
                {
                    m_Logger.LogInformation("No records found in ExitType table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { ExitTypes.Count} ExitType.");
                    return Ok(ExitTypes);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching ExitType.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// UpdateExitType
        /// </summary>
        /// <param name="exitTypeIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(ExitType exitTypeIn)
        {
            m_Logger.LogInformation("updating record in ExitType table.");
            try
            {
                dynamic response = await m_ExitTypeService.Update(exitTypeIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully updated record in ExitType table.");
                    return Ok(response.ExitType);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating ExitType: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("ExitType: " + exitTypeIn.Description);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating ExitType: " + ex);
                return BadRequest("Error occurred while updating ExitType");
            }
        }
        #endregion

        #region GetExitTypeIdByName
        /// <summary>
        /// Gets ExitTypeId By Name
        /// </summary>
        /// <param name="exitTypeName"></param>
        /// <returns></returns>
        [HttpGet("GetExitTypeIdByName")]
        public async Task<ActionResult<int>> GetExitTypeIdByName(string exitTypeName)
        {
            m_Logger.LogInformation($"Retrieving records from ExitType table by {exitTypeName}.");

            try
            {
                var exitType = await m_ExitTypeService.GetExitTypeIdByName(exitTypeName);
                if (!exitType.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found in exit type table on {exitTypeName}.");

                    return NotFound(exitType.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found for {exitTypeName}.");

                    return Ok(exitType.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetExitTypeIdByName() method in ExitTypeController:" + ex.StackTrace);

                return BadRequest("Error Occured in GetExitTypeIdByName() method in ExitTypeController:");
            }

        }
        #endregion
    }
}