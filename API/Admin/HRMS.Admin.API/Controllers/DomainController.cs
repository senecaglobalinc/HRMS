using HRMS.Admin.Entities;
using HRMS.Admin.Types;
using HRMS.Admin.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace HRMS.Admin.API.Controllers
{
    [Route("admin/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class DomainController : Controller
    {
        #region Global Variables

        private readonly IDomainService m_domainService;
        private readonly ILogger<DomainController> m_Logger;

        #endregion

        #region Constructor

        public DomainController(IDomainService domainService, ILogger<DomainController> logger)
        {
            m_domainService = domainService;
            m_Logger = logger;
        }

        #endregion

        #region  Create
        /// <summary>
        /// CreateDomain
        /// </summary>
        /// <param name="domainIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Domain domainIn)
        {
            m_Logger.LogInformation("Inserting record in Domain table.");
            try
            {
                dynamic response = await m_domainService.Create(domainIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully created record in Domain table.");
                    return Ok(response.Domain);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating Domain: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("DomainName: " + domainIn.DomainName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating Domain: " + ex);
                return BadRequest("Error occurred while creating Domain");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetDomains
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from domain table.");

            try
            {
                var domains = await m_domainService.GetAll(isActive);
                if (domains == null)
                {
                    m_Logger.LogInformation("No records found in domain table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { domains.Count} domain.");
                    return Ok(domains);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Domain.");
            }
        }
        #endregion

        #region GetByDomainId
       /// <summary>
       /// Gets the Domain by Id
       /// </summary>
       /// <param name="domainId"></param>
       /// <returns></returns>
        [HttpGet("GetByDomainId/{domainId}")]
        public async Task<ActionResult<Domain>> GetByDomainId(int domainId)
        {
            m_Logger.LogInformation($"Retrieving records from Domain table by {domainId}.");

            try
            {
                var domain = await m_domainService.GetByDomainId(domainId);
                if (domain == null)
                {
                    m_Logger.LogInformation($"No records found for domain {domainId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for domain {domainId}.");
                    return Ok(domain);
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
        /// UpdateDomain
        /// </summary>
        /// <param name="domainIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(Domain domainIn)
        {
            m_Logger.LogInformation("updating record in Domain table.");
            try
            {
                dynamic response = await m_domainService.Update(domainIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully updated record in Domain table.");
                    return Ok(response.Domain);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating Domain: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("DomainName: " + domainIn.DomainName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating Domain: " + ex);
                return BadRequest("Error occurred while updating Domain");
            }
        }
        #endregion
    }
}