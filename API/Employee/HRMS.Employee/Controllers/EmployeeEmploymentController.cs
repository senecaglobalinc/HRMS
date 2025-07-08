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
    public class EmployeeEmploymentController : ControllerBase
    {
        #region Global Variables

        private readonly IEmployeeEmploymentService m_employeeEmploymentService;
        private readonly ILogger<EmployeeEmploymentController> m_Logger;

        #endregion

        #region Constructor
        public EmployeeEmploymentController(IEmployeeEmploymentService employeeEmploymentService,
            ILogger<EmployeeEmploymentController> logger)
        {
            m_employeeEmploymentService = employeeEmploymentService;
            m_Logger = logger;
        }
        #endregion

        #region GetPrevEmploymentDetailsById
        /// <summary>
        /// Gets the Previous Employment details based on empId
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPrevEmploymentDetailsById/{empId}")]
        public async Task<ActionResult<IEnumerable>> GetPrevEmploymentDetailsById(int empId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving records from PreviousEmploymentDetails table.");

            try
            {
                var employmentDetails = await m_employeeEmploymentService.GetPrevEmploymentDetailsById(empId);
                if (!employmentDetails.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in PreviousEmploymentDetails table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetPrevEmploymentDetailsById() in EmployeeEmploymentController:" + stopwatch.Elapsed);
                    return NotFound(employmentDetails.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning Previous EmploymentDetails.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetPrevEmploymentDetailsById() in EmployeeEmploymentController:" + stopwatch.Elapsed);
                    return Ok(employmentDetails.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while getting Previous Employment Details in GetPrevEmploymentDetailsById() method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetPrevEmploymentDetailsById() in EmployeeEmploymentController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while getting Employment Details in GetPrevEmploymentDetailsById() method");
            }
        }
        #endregion

        #region GetProfReferencesById
        /// <summary>
        /// Gets the Professional References based on empId
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProfReferencesById/{empId}")]
        public async Task<ActionResult<IEnumerable>> GetProfReferencesById(int empId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving records from ProfessionalReferences table.");

            try
            {
                var profReferences = await m_employeeEmploymentService.GetProfReferencesById(empId);
                if (!profReferences.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in ProfessionalReferences table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetProfReferencesById() in EmployeeEmploymentController:" + stopwatch.Elapsed);
                    return NotFound(profReferences.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning ProfessionalReferences.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetProfReferencesById() in EmployeeEmploymentController:" + stopwatch.Elapsed);
                    return Ok(profReferences.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while getting ProfessionalReferences in GetProfReferencesById() method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetProfReferencesById() in EmployeeEmploymentController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured while getting ProfessionalReferences in GetProfReferencesById() method");
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Save method performs both creation and updation of employment details
        /// </summary>
        /// <param name="employmentDetailsIn"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        public async Task<IActionResult> Save(EmployeeDetails employmentDetailsIn)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Inserting record in EmploymentDetails table.");
            try
            {
                var response = await m_employeeEmploymentService.Save(employmentDetailsIn);
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