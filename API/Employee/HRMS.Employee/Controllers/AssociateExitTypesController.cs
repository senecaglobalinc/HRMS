using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{

    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AssociateExitTypesController : ControllerBase
    {
        #region Global Variables

        private readonly IAssociateExitTypesService m_AssociateExitTypesService;
        private readonly ILogger<AssociateExitTypesController> m_Logger;

        #endregion

        #region Constructor
        public AssociateExitTypesController(IAssociateExitTypesService associateExitTypesService,
            ILogger<AssociateExitTypesController> logger)
        {
            m_AssociateExitTypesService = associateExitTypesService;
            m_Logger = logger;
        }
        #endregion

        #region GetEmployeesByEmpIdAndRole
        /// <summary>
        /// This method gets the employee details
        /// </summary>
        /// <param name="employeetId"></param>
        /// <param name="roleName"></param>
        /// <returns> </returns>
        [HttpGet("GetEmployeesByEmpIdAndRole/{employeeId}/{roleName}")]
        public async Task<ActionResult<List<EmployeeDetails>>> GetEmployeesByEmpIdAndRole(int employeeId, string roleName)
        {
            m_Logger.LogInformation("Retrieving records");

            try
            {
                var emp = await m_AssociateExitTypesService.GetEmployeesByEmpIdAndRole(employeeId, roleName);
                if (emp.Items == null)
                {
                    m_Logger.LogInformation("No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning Employee details.");
                    return Ok(emp.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation($"EmployeeID= {employeeId}");
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetByExitTypeId
        /// <summary>
        /// GetByExitTypeId
        /// </summary>
        /// <param name="exitTypeId"></param>
        /// <returns></returns>
        [HttpGet("GetByExitTypeId/{exitTypeId}")]
        public async Task<ActionResult<AssociateExit>> GetByExitTypeId(int exitTypeId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving records from AssociateExit table.");

            try
            {
                var associateExits = await m_AssociateExitTypesService.GetByExitType(exitTypeId);
                if (!associateExits.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in AssociateExit table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAll() in AssociateExitTypesController:" + stopwatch.Elapsed);
                    return NotFound(associateExits.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning AssociateExit records.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAll() in AssociateExitTypesController:" + stopwatch.Elapsed);
                    return Ok(associateExits.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while getting AssociateExit records in GetByExitType method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetAll() in AssociateExitTypesController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while getting AssociateExit records in GetByExitType method");
            }
        }
        #endregion       
    }
}
