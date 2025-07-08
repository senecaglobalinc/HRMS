using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMS.Admin.Types;
using Microsoft.Extensions.Logging;
using HRMS.Admin.Entities;
using System.Diagnostics;
using HRMS.Admin.API.Auth;
using HRMS.Admin.Infrastructure.Models;

namespace HRMS.Admin.API.Controllers
{
    [Route("admin/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class FinancialYearController : Controller
    {
        #region Global Variables

        private readonly IFinancialYearService m_FinancialYearService;
        private readonly ILogger<FinancialYearController> m_Logger;

        #endregion

        #region Constructor
        public FinancialYearController(IFinancialYearService financialYearService, ILogger<FinancialYearController> logger)
        {
            m_FinancialYearService = financialYearService;
            m_Logger = logger;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get all Financial years
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<FinancialYear>>> GetAll()
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving records from FinancialYear table.");

            try
            {
                var financialYears = await m_FinancialYearService.GetAll();
                if (!financialYears.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in FinancialYear table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAll() in FinancialYearController:" + stopwatch.Elapsed);

                    return NotFound(financialYears.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning FinancialYear Details.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAll() in FinancialYearController:" + stopwatch.Elapsed);
                    return Ok(financialYears.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetAll method in FinancialYearController" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetAll() in FinancialYearController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while getting all the financial years in GetAll method");
            }
        }
        #endregion

        #region GetById
        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <returns></returns>
        [HttpGet("GetById/{financialYearId}")]
        public async Task<ActionResult<FinancialYearModel>> GetById(int financialYearId)
        {
            m_Logger.LogInformation($"Retrieving records from FinancialYear table by {financialYearId}.");

            try
            {
                var financialYear = await m_FinancialYearService.GetByIdAsync(financialYearId);
                if (financialYear == null)
                {
                    m_Logger.LogInformation($"No records found for financialYearId {financialYearId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for financialYearId {financialYearId}.");
                    return Ok(financialYear.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetCurrentFinancialYear
        /// <summary>
        /// This method gets the current financial year.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCurrentFinancialYear")]
        public async Task<ActionResult<FinancialYearModel>> GetCurrentFinancialYear()
        {
            m_Logger.LogInformation($"Retrieving records from FinancialYear table.");

            try
            {
                var financialYear = await m_FinancialYearService.GetCurrentFinancialYear();
                if (financialYear == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Current year found.");
                    return Ok(financialYear.Item);
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
