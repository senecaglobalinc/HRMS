using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Project.API.Controllers
{
    [Route("project/api/v1/[controller]")]
    [ApiController]
    [Authorize(Policy = "NightJobHeaderAuthPolicy")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class NightlyJobController : Controller
    {
        #region Global Variables
        private readonly IReportService m_ReportService;
        private readonly IAssociateAllocationService m_AssociateAllocationService;
        private readonly ILogger<NightlyJobController> m_Logger;
        #endregion

        #region Constructor
        public NightlyJobController(IReportService reportService, IAssociateAllocationService associateAllocationService, ILogger<NightlyJobController> logger)
        {
            m_ReportService = reportService;
            m_AssociateAllocationService = associateAllocationService;
            m_Logger = logger;
        }
        #endregion

        #region GetUtilizationReportAllocations
        /// <summary>
        /// Gets Resource Utilization Details.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("GetUtilizationReportAllocations")]
        public async Task<ActionResult<UtilizationReportAllocation>> GetUtilizationReportAllocations(UtilizationReportFilter filter)
        {
            m_Logger.LogInformation($"Getting project wise utilization of resoures.");

            try
            {
                var resource_util_details = await m_ReportService.GetUtilizationReportAllocations(filter);
                if (resource_util_details.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(resource_util_details.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetAllProjects
        /// <summary>
        /// GetAll Projects
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllProjects")]
        public async Task<ActionResult<ProjectResponse>> GetAllProjects()
        {
            m_Logger.LogInformation("Retrieving records from Projects table.");

            try
            {
                var projects = await m_ReportService.GetAllProjects(true);
                if (projects.Items == null)
                {
                    m_Logger.LogInformation("No records found in Projects table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { projects.Items.Count } Projects.");
                    return Ok(projects.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetAllAssociateAllocations
        /// <summary>
        /// GetAll Associate Allocations
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllAssociateAllocations")]
        public async Task<ActionResult<IList<AssociateAllocation>>> GetEmployeesForAllocations()
        {
            m_Logger.LogInformation("Retrieving records from AssociateAllocation table.");

            try
            {
                var associateAllocation = await m_AssociateAllocationService.GetAll();
                if (associateAllocation == null || associateAllocation.Items == null)
                {
                    m_Logger.LogInformation("No records found in AssociateAllocation table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { associateAllocation.Items.Count } associateAllocation.");
                    return Ok(associateAllocation.Items);
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
