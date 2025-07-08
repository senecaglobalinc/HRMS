using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ProspectiveAssociateController : ControllerBase
    {
        #region Global Variables

        private readonly IProspectiveAssociateService m_ProspectiveAssociateService;
        private readonly ILogger<ProspectiveAssociateController> m_Logger;

        #endregion

        #region Constructor
        public ProspectiveAssociateController(IProspectiveAssociateService prospectiveAssociateService,
            ILogger<ProspectiveAssociateController> logger)
        {
            m_ProspectiveAssociateService = prospectiveAssociateService;
            m_Logger = logger;
        }
        #endregion

        #region GetProspectiveAssociates
        /// <summary>
        /// Get Active Prospective Associates based on Departments, Designations and PracticeArea.
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetProspectiveAssociates")]
        public async Task<ActionResult<IEnumerable>> GetProspectiveAssociates(bool? isActive = true)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving records from ProspectiveAssociate table.");

            try
            {
                var prospectiveAssociates = await m_ProspectiveAssociateService.GetProspectiveAssociates(isActive);
                if (!prospectiveAssociates.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in prospective associate table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetProspectiveAssociates() in ProspectiveAssociateController:" + stopwatch.Elapsed);
                    return NotFound(prospectiveAssociates.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning prospective associates.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetProspectiveAssociates() in ProspectiveAssociateController:" + stopwatch.Elapsed);
                    return Ok(prospectiveAssociates.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while getting prospective associates in GetProspectiveAssociates method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetProspectiveAssociates() in ProspectiveAssociateController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while getting prospective associates in GetProspectiveAssociates method");
            }
        }
        #endregion

        #region GetbyId
        /// <summary>
        /// Gets the prospectiveAssociate data based on Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetbyId/{Id}")]
        public async Task<ActionResult<EmployeeDetails>> GetbyId(int Id)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation($"Retrieving records from Prospective Associates table by {Id}.");

            try
            {
                var prospectiveAssociate = await m_ProspectiveAssociateService.GetbyId(Id);
                if (!prospectiveAssociate.IsSuccessful)
                {
                    m_Logger.LogInformation($"No record found in  Prospective Associates table for Id {Id}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetbyId() in ProspectiveAssociateController:" + stopwatch.Elapsed);
                    return NotFound(prospectiveAssociate.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found in  Prospective Associates table for Id {Id}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetbyId() in ProspectiveAssociateController:" + stopwatch.Elapsed);
                    return Ok(prospectiveAssociate.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while getting ProspectiveAssociates by Id in GetbyId method" +ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetbyId() in ProspectiveAssociateController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while getting ProspectiveAssociates by Id in GetbyId method");
            }

        }
        #endregion

        #region Update
        /// <summary>
        /// Update ProspectiveAssociate
        /// </summary>
        /// <param name="prospectiveAssociateIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(ProspectiveAssociate prospectiveAssociateIn)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Updating record in prospectiveAssociates table.");

            try
            {
                var response = await m_ProspectiveAssociateService.Update(prospectiveAssociateIn);
                if (!response.IsSuccessful)
                {
                    //Add exception to logger
                    m_Logger.LogError("Error occurred while updating prospectiveAssociate: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Update() in ProspectiveAssociateController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in prospectiveAssociate's table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Update() in ProspectiveAssociateController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exception to logger
                m_Logger.LogError("Error occurred while updating prospectiveAssociate: " + ex.StackTrace);

                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Update() in ProspectiveAssociateController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while updating prospectiveAssociate in Update() method.");
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// Create ProspectiveAssociate
        /// </summary>
        /// <param name="prospectiveAssociateIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProspectiveAssociate prospectiveAssociateIn)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Inserting record in ProspectiveAssociate table.");
            try
            {
                var response = await m_ProspectiveAssociateService.Create(prospectiveAssociateIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating ProspectiveAssociate: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in ProspectiveAssociateController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in ProspectiveAssociate's table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in ProspectiveAssociateController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while creating ProspectiveAssociate: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Create() in ProspectiveAssociateController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while creating ProspectiveAssociate in Create method.");
            }
        }
        #endregion

        #region UpdateEmployeeStatusToPending
        /// <summary>
        /// This method is to update employee status to pending and send email to HRM for approval.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpPost("UpdateEmployeeStatusToPending")]
        public async Task<ActionResult<bool>> UpdateEmployeeStatusToPending(EmployeeProfileStatus employeeProfileStatus)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Update Employee Status in Employee table.");

            try
            {
                var response = await m_ProspectiveAssociateService.UpdateEmployeeStatusToPending(employeeProfileStatus.EmpId);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogError("Error occurred while updating employee status: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute UpdateEmployeeStatusToPending() in EmployeeStatusController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Successfully updated employee status");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute UpdateEmployeeStatusToPending() in EmployeeStatusController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in UpdateEmployeeStatusToPending() method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute UpdateEmployeeStatusToPending() in EmployeeStatusController:" + stopwatch.Elapsed);
                return BadRequest("Error occured in UpdateEmployeeStatusToPending() method");
            }
        }
        #endregion

        #region UpdateEmployeeProfileStatus
        /// <summary>
        /// This method is to Update employee status and send respective email either approval or rejection notification to HRM 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpPost("UpdateEmployeeProfileStatus")]
        public async Task<ActionResult<bool>> UpdateEmployeeProfileStatus(EmployeeProfileStatus employeeProfileStatus)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Update Employee Status in Employee table.");

            try
            {
                var response = await m_ProspectiveAssociateService.UpdateEmployeeProfileStatus(employeeProfileStatus);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogError("Error occurred while updating employee status: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute UpdateEmployeeStatusToPending() in EmployeeStatusController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Successfully updated employee status");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute UpdateEmployeeStatusToPending() in EmployeeStatusController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in UpdateEmployeeStatusToPending() method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute UpdateEmployeeStatusToPending() in EmployeeStatusController:" + stopwatch.Elapsed);
                return BadRequest("Error occured in UpdateEmployeeStatusToPending() method");
            }
        }
        #endregion
    }
}