using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AssociateLongLeaveController : ControllerBase
    {
        #region Global Variables

        private readonly IAssociateLongLeaveService m_associateLongLeaveService;
        private readonly ILogger<AssociateLongLeaveController> m_Logger;

        #endregion

        #region Constructor
        public AssociateLongLeaveController(IAssociateLongLeaveService associateLongLeaveService,
            ILogger<AssociateLongLeaveController> logger)
        {
            m_associateLongLeaveService = associateLongLeaveService;
            m_Logger = logger;
        }
        #endregion

        #region CreateAssociateLongLeave
        /// <summary>
        /// this method is used to create associate long leave
        /// </summary>
        /// <param name="leaveDetails"></param>
        /// <returns></returns>
        [HttpPost("CreateAssociateLongLeave")]
        public async Task<IActionResult> CreateAssociateLongLeave(AssociateLongLeaveData leaveDetails)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Inserting record in AssociateLongLeave table.");
            try
            {
                var response = await m_associateLongLeaveService.CreateAssociateLongLeave(leaveDetails);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while saving associate long leave: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute CreateAssociateLongLeave() in AssociateLongLeaveController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully saved record in AssociateLongLeave table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute CreateAssociateLongLeave() in AssociateLongLeaveController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating long leave details: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Save() in AssociateLongLeaveController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while saving associate long leave.");
            }
        }
        #endregion

        #region CalculateMaternityPeriod
        /// <summary>
        /// CalculateMaternityPeriod
        /// </summary>
        /// <param name="longLeaveStartDate"></param>
        /// <returns></returns>
        [HttpGet("CalculateMaternityPeriod")]
        public async Task<ActionResult> CalculateMaternityPeriod(string longLeaveStartDate)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving tentative join date.");

            try
            {
                var tentativeJoinDate = await m_associateLongLeaveService.CalculateMaternityPeriod(longLeaveStartDate);
                if (!tentativeJoinDate.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute CalculateMaternityPeriod() in AssociateLongLeaveController:" + stopwatch.Elapsed);
                    return NotFound(tentativeJoinDate.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning tentative join date");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute CalculateMaternityPeriod() in AssociateLongLeaveController:" + stopwatch.Elapsed);
                    return Ok(tentativeJoinDate.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in CalculateMaternityPeriod method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute CalculateMaternityPeriod() in AssociateLongLeaveController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in CalculateMaternityPeriod method");
            }
        }
        #endregion

        #region ReJoinAssociateByID
        /// <summary>
        /// ReJoinAssociateByID
        /// </summary>
        /// <param name="empID"></param>
        /// <param name="reason"></param>
        /// <param name="rejoinedDate"></param>
        /// <returns></returns>
        [HttpGet("ReJoinAssociateByID")]
        public async Task<ActionResult> ReJoinAssociateByID(int empID, string reason, string rejoinedDate)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Rejoin associate from long leave.");

            try
            {
                var rejoin = await m_associateLongLeaveService.ReJoinAssociateByID(empID, reason, rejoinedDate);
                if (!rejoin.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute ReJoinAssociateByID() in AssociateLongLeaveController:" + stopwatch.Elapsed);
                    return NotFound(rejoin.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Rejoining associate.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute ReJoinAssociateByID() in AssociateLongLeaveController:" + stopwatch.Elapsed);
                    return Ok(rejoin.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in ReJoinAssociateByID method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute ReJoinAssociateByID() in AssociateLongLeaveController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in ReJoinAssociateByID method");
            }
        }
        #endregion
    }
}
