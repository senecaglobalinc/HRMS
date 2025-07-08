using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Common.Enums;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeStatusController : Controller
    {
        #region Global Variables

        private readonly IEmployeeStatusService m_EmployeeStatusService;
        private readonly ILogger<EmployeeStatusController> m_Logger;
        private readonly IOrganizationService m_OrganizationService;

        #endregion

        #region Constructor
        public EmployeeStatusController(IEmployeeStatusService EmployeeStatusService,
            ILogger<EmployeeStatusController> logger,
            IOrganizationService organizationService)
        {
            m_EmployeeStatusService = EmployeeStatusService;
            m_OrganizationService = organizationService;
            m_Logger = logger;
        }
        #endregion      

        #region GetEmployeeResignStatus
        /// <summary>
        /// Get the status by status code Resigned and category AssociateExit.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmployeeResignStatus")]
        public  async Task<ActionResult<Status>> GetEmployeeResignStatus()
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving records from Employee table.");

            try
            {
                var status = await m_OrganizationService.GetStatusByCategoryAndStatusCode(StatusCategory.AssociateExit.ToString(), 
                                                                            AssociateExitStatusCodesNew.Resigned.ToString());
                if (!status.IsSuccessful)
                {
                    m_Logger.LogInformation("status not found for employee");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeResignStatus() in EmployeeStatusController:" + stopwatch.Elapsed);
                    return NotFound(status.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning Employee Resign status.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeResignStatus() in EmployeeStatusController:" + stopwatch.Elapsed);
                    return Ok(status.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in GetEmployeeResignStatus() method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute UpdateEmployeeStatus() in EmployeeStatusController:" + stopwatch.Elapsed);
                return BadRequest("Error occured in GetEmployeeResignStatus() method");
            }


        }
        #endregion

        #region UpdateEmployeeStatus
        /// <summary>
        /// This method is to make employee inactive.
        /// <param name="associate"></param>
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateEmployeeStatus")]
        public async Task<ActionResult> UpdateEmployeeStatus(EmployeeDetails associate)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Update Employee Status in Employee table.");

            try
            {
                var response = await m_EmployeeStatusService.UpdateEmployeeStatus(associate);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogError("Error occurred while updating userrole: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute UpdateEmployeeStatus() in EmployeeStatusController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Successfully updated employee status");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute UpdateEmployeeStatus() in EmployeeStatusController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in UpdateEmployeeStatus() method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute UpdateEmployeeStatus() in EmployeeStatusController:" + stopwatch.Elapsed);
                return BadRequest("Error occured in UpdateEmployeeStatus() method");
            }
        }
        #endregion

    }
}