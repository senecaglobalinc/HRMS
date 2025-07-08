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
    public class EmployeeProjectController : Controller
    {
        #region Global Variables

        private readonly IEmployeeProjectService m_EmployeeProjectService;
        private readonly ILogger<EmployeeProjectController> m_Logger;

        #endregion

        #region Constructor
        public EmployeeProjectController(IEmployeeProjectService employeeProjectService, ILogger<EmployeeProjectController> logger)
        {
            m_EmployeeProjectService = employeeProjectService;
            m_Logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// Removes existing projects and inserts new projects based on employeeId
        /// </summary>
        /// <param name="employeeDetails"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<EmployeeProject>> Create(EmployeeDetails employeeDetails)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Inserting record into EmployeeProject table");
            try
            {
                var project = await m_EmployeeProjectService.Create(employeeDetails);
                if (!project.IsSuccessful)
                {
                    m_Logger.LogError(project.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in EmployeeProjectController:" + stopwatch.Elapsed);

                    return NotFound(project.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully inserted records into EmployeeProject table");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in EmployeeProjectController:" + stopwatch.Elapsed);

                    return Ok(project.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"Create()\" of EmployeeProjectController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Create() in EmployeeProjectController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while creating certification");
            }

        }

        #endregion

        #region GetByEmployeeId
        /// <summary>
        /// Fetches project details of an employee based on employee id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetByEmployeeId/{employeeId}")]
        public async Task<ActionResult<List<EmployeeProject>>> GetByEmployeeId(int employeeId)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Fetching records from  EmployeeProject table");
            try
            {
                var projects = await m_EmployeeProjectService.GetByEmployeeId(employeeId);
                if (!projects.IsSuccessful)
                {
                    m_Logger.LogError(projects.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetByEmployeeId() in EmployeeProjectController:" + stopwatch.Elapsed);
                    return NotFound(projects.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully fetched project details");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetByEmployeeId() in EmployeeProjectController:" + stopwatch.Elapsed);
                    return Ok(projects.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"GetByEmployeeId()\" of EmployeeProjectController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetByEmployeeId() in EmployeeProjectController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while fetching project details");
            }
        }
        #endregion
    }
}