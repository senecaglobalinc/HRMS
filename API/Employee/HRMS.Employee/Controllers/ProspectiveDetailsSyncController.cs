using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using HRMS.Employee.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using HRMS.Employee.Infrastructure.Models.Request;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ProspectiveDetailsSyncController : Controller
    {
        #region Global Variables

        private readonly IProspectiveDetailsSyncService m_ProspectiveDetailsSyncService;
        private readonly ILogger<ProspectiveDetailsSyncController> m_Logger;

        #endregion

        #region Constructor
        public ProspectiveDetailsSyncController(IProspectiveDetailsSyncService prospectiveDetailsSyncService, ILogger<ProspectiveDetailsSyncController> logger)
        {
            m_ProspectiveDetailsSyncService = prospectiveDetailsSyncService;
            m_Logger = logger;
        }
        #endregion

        #region SyncDataFromRepository
        /// <summary>
        /// Gets the Prospective Associate info from the repository and sync data into respective Employee tables.
        /// </summary>
        /// <returns></returns>
        [HttpGet("ReadRepository")]
        public async Task<ActionResult<bool>> ReadRepository()
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Read repository Information and insert into tables");

            try
            {
                var employee = await m_ProspectiveDetailsSyncService.ReadRepository();
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"Error Occured while Reading Repository");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute ReadRepository() in ProspectiveDetailsSyncController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning Employees Information from Read Repository");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute ReadRepository() in ProspectiveDetailsSyncController:" + stopwatch.Elapsed);
                    return Ok(employee);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in ReadRepository() in ProspectiveDetailsSyncContrller:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute ReadRepository() in ProspectiveDetailsSyncController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in ReadRepository() in ProspectiveDetailsSyncController:");
            }
        }
        #endregion

    }
}
