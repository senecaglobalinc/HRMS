using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using HRMS.Project.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace HRMS.Project.API
{
    [Route("project/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController: Controller
    {
        #region Global Variables

        private readonly IReportsService m_ReportsService;
        private readonly ILogger<ReportsController> m_Logger;

        #endregion

        #region Constructor
        public ReportsController(IReportsService reportsService,
            ILogger<ReportsController> logger)
        {
            m_ReportsService = reportsService;
            m_Logger = logger;
        }
        #endregion

        #region GetResourceByProject
        /// <summary>
        /// Returns total number of resource, billable and non billable resource details.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("GetResourceByProject/{projectId}")]
        public async Task<ActionResult<Entities.AllocationDetails>> GetResourceByProject(int projectId)
        {
            m_Logger.LogInformation($"Getting total number of resource, billable and non billable resource details by Project {projectId}.");

            try
            {
                var resourceAllocation = await m_ReportsService.GetResourceByProject(projectId);
                if (resourceAllocation.Item == null)
                {
                    m_Logger.LogInformation($"No records found for Id {projectId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for id {projectId}.");
                    return Ok(resourceAllocation.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetProjectDetailsReport
        /// <summary>
        /// Returns projects detailed information.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectDetailsReport")]
        public async Task<ActionResult<IEnumerable>> GetProjectDetailsReport()
        {
            m_Logger.LogInformation($"Getting projects detailed information for report");

            try
            {
                var projects = await m_ReportsService.GetProjectDetailsReport();
                if (projects.Items == null)
                {
                    m_Logger.LogInformation($"No records found");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"returning projects detailed info");
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
    }
}
