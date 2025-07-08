using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeEducationController : ControllerBase
    {
        #region Global Variables

        private readonly IEmployeeEducationService m_employeeEducationService;
        private readonly ILogger<EmployeeEducationController> m_Logger;

        #endregion

        #region Constructor
        public EmployeeEducationController(IEmployeeEducationService employeeEducationService,
            ILogger<EmployeeEducationController> logger)
        {
            m_employeeEducationService = employeeEducationService;
            m_Logger = logger;
        }
        #endregion

        #region GetById
        /// <summary>
        /// Gets the EmployeeEducationDetails based on empId
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{empId}")]
        public async Task<ActionResult<IEnumerable>> GetById(int empId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving records from EducationDetails table.");

            try
            {
                var educationDetails = await m_employeeEducationService.GetById(empId);
                if (!educationDetails.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in EducationDetails table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetById() in EmployeeEducationController:" + stopwatch.Elapsed);
                    return NotFound(educationDetails.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning EducationDetails.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetById() in EmployeeEducationController:" + stopwatch.Elapsed);
                    return Ok(educationDetails.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while getting Education Details in GetById method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetById() in EmployeeEducationController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while getting Education Details in GetById method");
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Save method performs both creation and updation of education details
        /// </summary>
        /// <param name="educationDetails"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        public async Task<IActionResult> Save(EmployeeDetails educationDetailsIn)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Inserting record in EducationDetails table.");
            try
            {
                var response = await m_employeeEducationService.Save(educationDetailsIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while saving EducationDetails: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Save() in EmployeeEducationController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully saved record in EducationDetails's table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Save() in EmployeeEducationController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while saving EducationDetails: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Save() in EmployeeEducationController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while saving EducationDetails in Save method.");
            }
        }
        #endregion
    }
}