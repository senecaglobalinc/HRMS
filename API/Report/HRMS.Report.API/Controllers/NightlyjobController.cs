using HRMS.Report.Infrastructure.Models.Domain;
using HRMS.Report.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Report.API.Controllers
{
    [Route("report/api/v1/[controller]")]
    [ApiController]
    [Authorize(Policy = "NightJobHeaderAuthPolicy")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class NightlyjobController : Controller
    {
        #region Global Variables

        private readonly IReportService m_ReportService;
        private readonly ILogger<ReportsController> m_Logger;

        #endregion

        #region Constructor
        public NightlyjobController(IReportService reportService, ILogger<ReportsController> logger)
        {
            m_ReportService = reportService;
            m_Logger = logger;
        }
        #endregion

        #region GetResourceReportByFilter
        /// <summary>
        /// This endpoint is used in HRMSReportsConsole job. 
        /// Enforces Authorization policy to check its headers for its configuration
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetResourceReportByFilter")]
        public async Task<ActionResult<UtilizationReport>> GetResourceReportByFilter()
        {
            try
            {
                // when calling the utilization report from NightJob
                bool isNightJob = true;
                var resourcReport = await m_ReportService.GetResourceReportByFilter(isNightJob);

                if (resourcReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(resourcReport.Items);
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
