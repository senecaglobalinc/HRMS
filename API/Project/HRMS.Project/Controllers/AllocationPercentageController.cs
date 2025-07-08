using HRMS.Project.Entities;
using HRMS.Project.Types;
using HRMS.Project.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace HRMS.Project.API.Controllers
{
    [Route("project/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AllocationPercentageController : Controller
    {
        #region Global Variables

        private readonly IAllocationPercentageService m_AllocationPercentageService;
        private readonly ILogger<AllocationPercentageController> m_Logger;

        #endregion

        #region Constructor
        public AllocationPercentageController(IAllocationPercentageService allocationPercentageService,
            ILogger<AllocationPercentageController> logger)
        {
            m_AllocationPercentageService = allocationPercentageService;
            m_Logger = logger;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetAll Associate Percentage
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            m_Logger.LogInformation("Retrieving records from AssociatePercentage table.");

            try
            {
                var associatePercentage = await m_AllocationPercentageService.GetAll();
                if (!associatePercentage.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in AssociatePercentage table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { associatePercentage.Items.Count } associatePercentage.");
                    return Ok(associatePercentage.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }


        }
        #endregion

        #region GetAllocationPercentageForDropdown
        /// <summary>
        /// GetAllocationPercentageForDropdown
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetAllocationPercentageForDropdown")]
        public async Task<ActionResult<IEnumerable>> GetAllocationPercentageForDropdown()
        {
            m_Logger.LogInformation("Retrieving records from clients table.");

            try
            {
                var percentages = await m_AllocationPercentageService.GetAllocationPercentageForDropdown();
                if (percentages == null)
                {
                    m_Logger.LogInformation("No records found in AllocationPercentage table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { percentages.Items.Count} AllocationPercentage.");
                    return Ok(percentages.Items);
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
        /// GetById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<AllocationPercentage>> GetById(int id)
        {
            m_Logger.LogInformation($"Retrieving records from AllocationPercentage table by {id}.");

            try
            {
                var employee = await m_AllocationPercentageService.GetById(id);
                if (employee.Item == null)
                {
                    m_Logger.LogInformation($"No records found for Id {id}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for id {id}.");
                    return Ok(id);
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