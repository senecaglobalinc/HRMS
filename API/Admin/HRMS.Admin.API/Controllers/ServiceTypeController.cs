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
    public class ServiceTypeController : ControllerBase
    {
        #region Global Variables

        private readonly IServiceTypeService m_ServiceTypeService;
        private readonly ILogger<ServiceTypeController> m_Logger;

        #endregion

        #region Constructor
        public ServiceTypeController(IServiceTypeService serviceTypeService, ILogger<ServiceTypeController> logger)
        {
            m_ServiceTypeService = serviceTypeService;
            m_Logger = logger;
        }
        #endregion

        #region  Create
        /// <summary>
        /// Create Service Type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ServiceType serviceType)
        {
            m_Logger.LogInformation("Updating record in Service Type table.");
            try
            {
                dynamic response = await m_ServiceTypeService.Create(serviceType);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully Updated record in Service Type table.");
                    return Ok(response.ServiceType);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating Service Type: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Service Type: " + serviceType.ServiceTypeName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating Service Type: " + ex);
                return BadRequest("Error occurred while creating Service Type");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Service Type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(ServiceType serviceType)
        {
            m_Logger.LogInformation("updating record in ServiceType table.");
            try
            {
                dynamic response = await m_ServiceTypeService.Update(serviceType);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully updated record in ServiceType table.");
                    return Ok(response.ServiceType);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating ServiceType: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("ServiceType: " + serviceType.ServiceTypeName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating ServiceType: " + ex);
                return BadRequest("Error occurred while updating ServiceType");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetAll ServiceTypes
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from ServiceType table.");

            try
            {
                var serviceTypes = await m_ServiceTypeService.GetAll(isActive);
                if (serviceTypes == null)
                {
                    m_Logger.LogInformation("No records found in ServiceType table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning ServiceTypes.");
                    return Ok(serviceTypes);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching ServiceTypes.");
            }
        }
        #endregion

        #region GetServiceTypeForDropdown
        /// <summary>
        /// GetServiceTypeForDropdown
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetServiceTypeForDropdown")]
        public async Task<ActionResult<IEnumerable>> GetServiceTypeForDropdown()
        {
            m_Logger.LogInformation("Retrieving records from ServiceType table.");

            try
            {
                var serviceTypes = await m_ServiceTypeService.GetServiceTypeForDropdown();
                if (serviceTypes == null)
                {
                    m_Logger.LogInformation("No records found in ServiceType table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { serviceTypes.Count} Service Types.");
                    return Ok(serviceTypes);
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
