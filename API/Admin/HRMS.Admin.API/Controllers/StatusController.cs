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
    public class StatusController : Controller
    {
        #region Global Variables

        private readonly IStatusService m_StatusService;
        private readonly ILogger<StatusController> m_Logger;

        #endregion

        #region Constructor

        public StatusController(IStatusService statusService, ILogger<StatusController> logger)
        {
            m_StatusService = statusService;
            m_Logger = logger;
        }

        #endregion

        #region GetAll
        /// <summary>
        /// GetStatuses
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool? isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from status table.");

            try
            {
                var statuses = await m_StatusService.GetAll(isActive);
                if (statuses == null)
                {
                    m_Logger.LogInformation("No records found in Status table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { statuses.Count} status.");
                    return Ok(statuses);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }


        }
        #endregion

        #region GetStatusById
        /// <summary>
        /// GetStatusById
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [HttpGet("GetStatusById/{statusId}")]
        public async Task<ActionResult<Status>> GetStatusByCode(int statusId)
        {
            m_Logger.LogInformation($"Retrieving records from status table by {statusId}.");

            try
            {
                var status = await m_StatusService.GetStatusById(statusId);
                if (status == null)
                {
                    m_Logger.LogInformation($"No records found for statusCode {statusId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for statusCode {statusId}.");
                    return Ok(status);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetStatusByCode
        /// <summary>
        /// GetStatusByCode
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        [HttpGet("GetStatusByCode/{statusCode}")]
        public async Task<ActionResult<Status>> GetStatusByCode(string statusCode)
        {
            m_Logger.LogInformation($"Retrieving records from status table by {statusCode}.");

            try
            {
                var status = await m_StatusService.GetStatusByCode(statusCode);
                if (status == null)
                {
                    m_Logger.LogInformation($"No records found for statusCode {statusCode}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for statusCode {statusCode}.");
                    return Ok(status);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetByCategoryAndStatusCode
        /// <summary>
        /// GetByCategoryAndStatusCode
        /// </summary>
        /// <param name="category"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        [HttpGet("GetByCategoryAndStatusCode/{category}/{statusCode}")]
        public async Task<ActionResult<Status>> GetByCategoryAndStatusCode(string category, string statusCode)
        {
            m_Logger.LogInformation($"Retrieving records from status table by {category} and {statusCode}.");

            try
            {
                var status = await m_StatusService.GetByCategoryAndStatusCode(category, statusCode);
                if (status == null)
                {
                    m_Logger.LogInformation($"No records found for statusCode {category} and {statusCode}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for statusCode {category} and {statusCode}.");
                    return Ok(status);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion
		
		#region GetAllByCategoryName
        /// <summary>
        /// GetStatuses
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetAllByCategoryName")]
        public async Task<ActionResult<IEnumerable>> GetAllByCategoryName(string category)
        {
            m_Logger.LogInformation("Retrieving records from status table.");

            try
            {
                var statuses = await m_StatusService.GetByCategory(category);
                if (statuses == null)
                {
                    m_Logger.LogInformation("No records found in Status table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { statuses.Count} status.");
                    return Ok(statuses);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }


        }
        #endregion

        #region GetByCategoryIdAndStatusCode
        /// <summary>
        /// GetByCategoryIdAndStatusCode
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        [HttpGet("GetByCategoryIdAndStatusCode/{categoryId}/{statusCode}")]
        public async Task<ActionResult<Status>> GetByCategoryIdAndStatusCode(int categoryId, string statusCode)
        {
            m_Logger.LogInformation($"Retrieving records from status table by {categoryId} and {statusCode}.");

            try
            {
                var status = await m_StatusService.GetByCategoryIdAndStatusCode(categoryId, statusCode);
                if (status == null)
                {
                    m_Logger.LogInformation($"No records found for CategoryId {categoryId} and statusCode {statusCode}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for CategoryId {categoryId} and statusCode {statusCode}.");
                    return Ok(status);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion
		
        #region GetProjectStatuses
        /// <summary>
        /// GetStatuses
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetProjectStatuses")]
        public async Task<ActionResult<IEnumerable>> GetProjectStatuses()
        {
            m_Logger.LogInformation("Retrieving records from status table.");

            try
            {
                var statuses = await m_StatusService.GetAll();
                if (statuses == null)
                {
                    m_Logger.LogInformation("No records found in Status table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { statuses.Count} status.");
                    return Ok(statuses);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }


        }
        #endregion

        #region GetStatusMasterDetails
        [HttpGet("GetStatusMasterDetails")]
        public async Task<ActionResult<IEnumerable>> GetStatusMasterDetails(bool isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from status table.");

            try
            {
                var statuses = await m_StatusService.GetStatusMasterDetails(isActive);
                if (statuses == null)
                {
                    m_Logger.LogInformation("No records found in Status table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { statuses.Count} status.");
                    return Ok(statuses);
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