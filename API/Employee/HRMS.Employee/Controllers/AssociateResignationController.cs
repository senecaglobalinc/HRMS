using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AssociateResignationController : ControllerBase
    {
        #region Global Variables

        private readonly IAssociateResignationService m_associateResignationService;
        private readonly ILogger<AssociateResignationController> m_Logger;

        #endregion

        #region Constructor
        public AssociateResignationController(IAssociateResignationService associateResignationService,
            ILogger<AssociateResignationController> logger)
        {
            m_associateResignationService = associateResignationService;
            m_Logger = logger;
        }
        #endregion

        #region CreateAssociateResignation
        /// <summary>
        /// this method is used to create associate resignation
        /// </summary>
        /// <param name="resignationDetails"></param>
        /// <returns></returns>
        [HttpPost("CreateAssociateResignation")]
        public async Task<IActionResult> CreateAssociateResignation(AssociateResignationData resignationDetails)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Inserting record in Associate resignation table.");
            try
            {
                var response = await m_associateResignationService.CreateAssociateResignation(resignationDetails);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while saving associate resignation: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Save() in AssociateResignationController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully saved record in AssociateResignation table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Save() in AssociateResignationController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while saving resignation details: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Save() in AssociateResignationController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while saving associate resignation.");
            }
        }
        #endregion

        #region GetAssociatesById
        /// <summary>
        /// GetAssociatesById
        /// </summary>
        /// <param name="resignEmployeeId"></param>
        /// <returns></returns>
        [HttpGet("GetAssociatesById")]
        public async Task<ActionResult> GetAssociatesById(int resignEmployeeId, int employeeID)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving records from AssociateResignation table.");

            try
            {
                var associates = await m_associateResignationService.GetAssociatesBySearchString(resignEmployeeId, employeeID);
                if (!associates.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAssociatesBySearchString() in AssociateResignationController:" + stopwatch.Elapsed);
                    return NotFound(associates.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning associates.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAssociatesBySearchString() in AssociateResignationController:" + stopwatch.Elapsed);
                    return Ok(associates.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while getting associates in GetAssociatesBySearchString method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetAssociatesBySearchString() in AssociateResignationController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while getting associates in GetAssociatesBySearchString method");
            }
        }
        #endregion

        #region CalculateNoticePeriod
        /// <summary>
        /// CalculateNoticePeriod
        /// </summary>
        /// <param name="resignationDate"></param>
        /// <returns></returns>
        [HttpGet("CalculateNoticePeriod")]
        public async Task<ActionResult> CalculateNoticePeriod(string resignationDate)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving last working date.");

            try
            {
                var lastWorkingDate = await m_associateResignationService.CalculateNoticePeriod(resignationDate);
                if (!lastWorkingDate.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute CalculateNoticePeriod() in AssociateResignationController:" + stopwatch.Elapsed);
                    return NotFound(lastWorkingDate.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning last working date.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute CalculateNoticePeriod() in AssociateResignationController:" + stopwatch.Elapsed);
                    return Ok(lastWorkingDate.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while getting associates in CalculateNoticePeriod method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute CalculateNoticePeriod() in AssociateResignationController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while getting associates in CalculateNoticePeriod method");
            }
        }
        #endregion

        #region RevokeResignationByID
        /// <summary>
        /// RevokeResignationByID
        /// </summary>
        /// <param name="empID"></param>
        /// <param name="reason"></param>
        /// <param name="revokedDate"></param>
        /// <returns></returns>
        [HttpGet("RevokeResignationByID")]
        public async Task<ActionResult> RevokeResignationByID(int empID, string reason, string revokedDate)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Revoking resignation.");

            try
            {
                var resignation = await m_associateResignationService.RevokeResignationByID(empID, reason, revokedDate);
                if (!resignation.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute RevokeResignationByID() in AssociateResignationController:" + stopwatch.Elapsed);
                    return NotFound(resignation.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Revoking resignation.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute RevokeResignationByID() in AssociateResignationController:" + stopwatch.Elapsed);
                    return Ok(resignation.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in RevokeResignationByID method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute RevokeResignationByID() in AssociateResignationController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in RevokeResignationByID method");
            }
        }
        #endregion
    }
}
