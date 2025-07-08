using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using HRMS.Employee.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using HRMS.Employee.Infrastructure.Models.Request;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class WelcomeEmailController : Controller
    {
        #region Global Variables

        private readonly IWelcomeEmailService m_WelcomeEmailService;
        private readonly ILogger<WelcomeEmailController> m_Logger;

        #endregion

        #region Constructor
        public WelcomeEmailController(IWelcomeEmailService welcomeEmailService, ILogger<WelcomeEmailController> logger)
        {
            m_WelcomeEmailService = welcomeEmailService;
            m_Logger = logger;
        }
        #endregion

        #region GetWelcomeEmployeeInfo
        /// <summary>
        /// Gets the Employee info based on departments and approved status.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetWelcomeEmployeeInfo")]
        public async Task<ActionResult<List<WelcomeEmailEmpRequest>>> GetWelcomeEmployeeInfo()
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving Employees Information");

            try
            {
                var employee = await m_WelcomeEmailService.GetWelcomeEmployeeInfo();
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"employees information not found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeInfo() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning Employees Information");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeInfo() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetEmployeeInfo() method in EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmployeeInfo() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetEmployeeInfo() method in EmployeeController");
            }
        }
        #endregion

        #region CreateWelcomeEmailInfo
        /// <summary>
        /// Create WelcomeEmail
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateWelcomeEmailInfo(int employeeId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Inserting record in WelcomeEmail table.");
            try
            {
                var response = await m_WelcomeEmailService.CreateWelcomeEmailInfo(employeeId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating WelcomeEmail: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in WelcomeEmailController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in WelcomeEmail's table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in WelcomeEmailController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while creating WelcomeEmail: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Create() in WelcomeEmailController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while creating WelcomeEmail in Create method.");
            }
        }
        #endregion

        [HttpPost("SendWelcomeEmail")]
        public async Task<IActionResult> SendWelcomeEmail([FromForm(Name = "file")] IFormFileCollection files)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            m_Logger.LogInformation("Inserting record in WelcomeEmail table.");

            try
            {
                var jsonStringData = Request.Form["data"];

                WelcomeEmailRequest welcomeEmailRequest = JsonConvert.DeserializeObject<WelcomeEmailRequest>(jsonStringData);

                m_Logger.LogDebug("SendWelcomeEmail() Called");
                var response = await m_WelcomeEmailService.SendWelcomeEmail(files, welcomeEmailRequest);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogError("Error occurred while creating WelcomeEmail: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in WelcomeEmailController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in WelcomeEmail's table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in WelcomeEmailController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occured while creating WelcomeEmail: " + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Create() in WelcomeEmailController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while creating WelcomeEmail in WelcomeEmail method.");
            }
            
        }

    }
}
