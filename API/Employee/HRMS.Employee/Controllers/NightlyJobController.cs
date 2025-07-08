using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Request;
using HRMS.Employee.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize(Policy = "NightJobHeaderAuthPolicy")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class NightlyJobController : Controller
    {
        #region Global Variables
        private readonly IReportService m_ReportService;
        private readonly ILogger<NightlyJobController> m_Logger;
        #endregion

        #region Constructor
        public NightlyJobController(IReportService reportService, ILogger<NightlyJobController> logger)
        {
            m_ReportService = reportService;
            m_Logger = logger;
        }
        #endregion

        #region GetUtilizationReportAssociates
        /// <summary>
        /// GetUtilizationReportAssociates
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("GetUtilizationReportAssociates")]
        public async Task<ActionResult<UtilizationReportEmployee>> GetUtilizationReportAssociates(UtilizationReportEmployeeFilter filter)
        {
            try
            {
                var utilizationReport = await m_ReportService.GetUtilizationReportAssociates(filter);
                if (utilizationReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(utilizationReport.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion       

        #region GetResourceUtilization
        /// <summary>
        /// GetResourceUtilization
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetResourceUtilization")]
        public async Task<ActionResult<UtilizationReport>> GetResourceUtilization(int? year = null)
        {
            try
            {
                var resourceReport = await m_ReportService.GetResourceUtilization(year, true);
                if (resourceReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(resourceReport.Items);
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
