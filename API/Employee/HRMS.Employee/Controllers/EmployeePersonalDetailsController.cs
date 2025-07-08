using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
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
    public class EmployeePersonalDetailsController : Controller
    {
        #region Global Variables

        private readonly IEmployeePersonalDetailsService m_AssociatePersonalDetailsService;
        private readonly ILogger<EmployeePersonalDetailsController> m_Logger;

        #endregion

        #region Constructor
        public EmployeePersonalDetailsController(IEmployeePersonalDetailsService associatePersonalDetailsService, 
                                                 ILogger<EmployeePersonalDetailsController> logger)
        {
            m_AssociatePersonalDetailsService = associatePersonalDetailsService;
            m_Logger = logger;
        }
        #endregion

        #region AddPersonalDetails
        /// <summary>
        /// Adds personal details and contacts of an employee and updates the data in prospective associate table.
        /// </summary>
        /// <param name="personalDetails"></param>
        /// <returns></returns>
        [HttpPost("AddPersonalDetails")]
        public async Task<IActionResult> AddPersonalDetails(EmployeePersonalDetails personalDetails)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Inserting record in Employee table.");
            try
            {
                //Checks whether the duplicate data exists or not.
                string duplicateFields = m_AssociatePersonalDetailsService.ValidatePersonalData(personalDetails);
                if (duplicateFields == null)
                {
                    dynamic response = await m_AssociatePersonalDetailsService.AddPersonalDetails(personalDetails);
                    if (!response.IsSuccessful)
                    {
                        m_Logger.LogError("Error occurred while saving personal details: " + (string)response.Message);
                        return BadRequest(response.Message);
                    }
                    else
                    {
                        m_Logger.LogInformation("Successfully saved personal details of the associate.");
                        return Ok(response.Item);
                    }
                }
                else
                {
                    return BadRequest(duplicateFields);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error Occured in AddPersonalDetails() method in AssociatePersonalDetailsController:" + ex.StackTrace);
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute AddPersonalDetails() in AssociatePersonalDetailsController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in AddPersonalDetails() method in AssociatePersonalDetailsController:");
            }
        }
        #endregion

        #region UpdatePersonalDetails
        /// <summary>
        /// Updates personal details and contacts of an employee.
        /// </summary>
        /// <param name="personalDetails"></param>
        /// <returns></returns>
        [HttpPost("UpdatePersonalDetails")]
        public async Task<IActionResult> UpdatePersonalDetails(EmployeePersonalDetails personalDetails)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Updating record in Employee table.");
            try
            {
                //Checks whether the duplicate data exists or not
                string duplicateFields = m_AssociatePersonalDetailsService.ValidatePersonalData(personalDetails);
                if (duplicateFields ==null)
                {
                    dynamic response = await m_AssociatePersonalDetailsService.UpdatePersonalDetails(personalDetails);
                    if (!response.IsSuccessful)
                    {
                        m_Logger.LogError("Error occurred while updating personal details: " + (string)response.Message);
                        return BadRequest(response.Message);
                    }
                    else
                    {
                        m_Logger.LogInformation("Successfully updated personal details of the associate.");
                        return Ok(response);
                    }
                }
                else
                {
                    return BadRequest(duplicateFields);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error Occured in UpdatePersonalDetails() method in AssociatePersonalDetailsController:" + ex.StackTrace);
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute UpdatePersonalDetails() in AssociatePersonalDetailsController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in UpdatePersonalDetails() method in AssociatePersonalDetailsController:");
            }
        }
        #endregion

        #region GetPersonalDetailsById
        /// <summary>
        /// Gets personal details by given employee Id.
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpGet("GetPersonalDetailsById/{empId}")]
        public async Task<ActionResult<EmployeePersonalDetails>> GetPersonalDetailsById(int empId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from Employee table by {empId}.");

            try
            {
                dynamic employee = await m_AssociatePersonalDetailsService.GetPersonalDetailsById(empId);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found for employeeId {empId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Records found for associate {empId}.");
                    return Ok(employee.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetPersonalDetailsByID() method in AssociatePersonalDetailsController:" + ex.StackTrace);
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetPersonalDetailsByID() in AssociatePersonalDetailsController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetPersonalDetailsByID() method in AssociatePersonalDetailsController:");
            }

        }
        #endregion

        #region UpdateReporting ManagerId
        /// <summary>
        /// Update Reporting Manager after project allocation.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="reportingManagerId"></param>
        /// <returns></returns>
        [HttpPost("UpdateReportingManagerId/{employeeId}/{reportingManagerId}")]
        public async Task<IActionResult> UpdateReportingManagerId(int employeeId, int reportingManagerId)
        {
            try
            {
                var response = await m_AssociatePersonalDetailsService.UpdateReportingManagerId(employeeId, reportingManagerId);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogError("Error occurred while updating reporting managerId: " + response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated reporting managerId of the associate.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error Occured in UpdateReportingManagerId() method in AssociatePersonalDetailsController:" + ex.StackTrace);
                return BadRequest("Error Occured in UpdateReportingManagerId() method in AssociatePersonalDetailsController:");
            }
        }
        #endregion

        #region Update External
        /// <summary>
        /// Update Employee External services
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="reportingManagerId"></param>
        /// <returns></returns>
        [HttpPost("UpdateExternal")]
        public async Task<IActionResult> UpdateExternal(EmployeeDetails employeeDetails)
        {
            try
            {
                var response = await m_AssociatePersonalDetailsService.UpdateExternal(employeeDetails);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogError("Error occurred while updating reporting managerId: " + response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated reporting managerId of the associate.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error Occured in UpdateReportingManagerId() method in AssociatePersonalDetailsController:" + ex.StackTrace);
                return BadRequest("Error Occured in UpdateReportingManagerId() method in AssociatePersonalDetailsController:");
            }
        }
        #endregion

        #region GetEmployeeDetailsDashboard
        /// <summary>
        /// Gets personal details by given employee Id.
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpGet("GetEmployeeDetailsDashboard/{empId}")]
        public async Task<ActionResult<EmployeeDetailsDashboard>> GetEmployeeDetailsDashboard(int empId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving all details for employee dashboard by {empId}.");

            try
            {
                dynamic employee = await m_AssociatePersonalDetailsService.GetEmployeeDetailsDashboard(empId);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found for employeeId {empId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Records found for associate {empId}.");
                    return Ok(employee.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetEmployeeDetailsDashboard(empId) method in AssociatePersonalDetailsController:" + ex.StackTrace);
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmployeeDetailsDashboard(empId) in AssociatePersonalDetailsController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetEmployeeDetailsDashboard(empId) method in AssociatePersonalDetailsController:");
            }
        }
        #endregion
    }
}
