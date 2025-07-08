using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeFamilyDetailsController : Controller
    {
        #region Global Variables

        private readonly IEmployeeFamilyDetailsService m_EmployeeFamilyDetailsService;
        private readonly ILogger<EmployeeFamilyDetailsController> m_Logger;

        #endregion

        #region Constructor
        public EmployeeFamilyDetailsController(IEmployeeFamilyDetailsService employeeFamilyDetailsService, ILogger<EmployeeFamilyDetailsController> logger)
        {
            m_EmployeeFamilyDetailsService = employeeFamilyDetailsService;
            m_Logger = logger;
        }
        #endregion

        #region UpdateFamilyDetails
        /// <summary>
        /// Updates family details of an employee
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        [HttpPost("UpdateFamilyDetails")]
        public async Task<IActionResult> UpdateFamilyDetails(EmployeePersonalDetails details)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Updating record in FamilyDetails and EmergencyContact table.");
            try
            {
                    dynamic response = await m_EmployeeFamilyDetailsService.UpdateFamilyDetails(details);
                    if (!response.IsSuccessful)
                    {
                        //Add exeption to logger
                        m_Logger.LogError("Error occurred while updating family details: " + (string)response.Message);
                        return BadRequest(response.Message);
                    }
                    else
                    {
                        m_Logger.LogInformation("Successfully updated family details of the employee.");
                        return Ok(response);
                    }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in UpdateFamilyDetails() method in EmployeeFamilyDetailsController:" + ex.StackTrace);
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute UpdateFamilyDetails() in EmployeeFamilyDetailsController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in UpdateFamilyDetails() method in EmployeeFamilyDetailsController:");
            }
        }
        #endregion

        #region GetFamilyDetailsById
        /// <summary>
        /// Gets the family details of an employee by the given employee Id
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpGet("GetFamilyDetailsById/{empId}")]
        public async Task<ActionResult<EmployeePersonalDetails>> GetFamilyDetailsById(int empId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from FamilyDetails and EmergencyContact table by {empId}.");

            try
            {
                var familyDetails = await m_EmployeeFamilyDetailsService.GetFamilyDetailsById(empId);
                if (familyDetails == null)
                {
                    m_Logger.LogInformation($"No records found for employeeId {empId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Records found for employee {empId}.");
                    return Ok(familyDetails);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetFamilyDetailsById() method in EmployeeFamilyDetailsController:" + ex.StackTrace);
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetFamilyDetailsById() in EmployeeFamilyDetailsController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetFamilyDetailsById() method in EmployeeFamilyDetailsController:");
            }

        }
        #endregion
    }
}
