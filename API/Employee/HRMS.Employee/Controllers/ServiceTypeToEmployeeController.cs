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
    public class ServiceTypeToEmployeeController : Controller
    {
        #region Global Variables

        private readonly IServiceTypeToEmployeeService m_ServiceTypeToEmployeeService;
        private readonly ILogger<ServiceTypeToEmployeeController> m_Logger;

        #endregion

        #region Constructor
        public ServiceTypeToEmployeeController(IServiceTypeToEmployeeService employeeService, ILogger<ServiceTypeToEmployeeController> logger)
        {
            m_ServiceTypeToEmployeeService = employeeService;
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
        public async Task<ActionResult<int>> Create(ServiceType employeeDetails)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Inserting record into ServiceTypeToEmployeeController table");
            try
            {
                var project = await m_ServiceTypeToEmployeeService.Create(employeeDetails);
                if (!project.IsSuccessful)
                {
                    m_Logger.LogError(project.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in ServiceTypeToEmployeeController:" + stopwatch.Elapsed);

                    return NotFound(project.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully inserted records into ServiceTypeToEmployee table");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in ServiceTypeToEmployeeController:" + stopwatch.Elapsed);

                    return Ok(project.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"Create()\" of ServiceTypeToEmployeeController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Create() in ServiceTypeToEmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while creating certification");
            }

        }

        #endregion

        #region Update
        /// <summary>
        /// Removes existing projects and inserts new projects based on employeeId
        /// </summary>
        /// <param name="employeeDetails"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<ActionResult<int>> Update(ServiceType employeeDetails)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Updating record into ServiceTypeToEmployee table");
            try
            {
                var project = await m_ServiceTypeToEmployeeService.Create(employeeDetails);
                if (!project.IsSuccessful)
                {
                    m_Logger.LogError(project.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in ServiceTypeToEmployeeController:" + stopwatch.Elapsed);

                    return NotFound(project.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully inserted records into ServiceTypeToEmployee table");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in ServiceTypeToEmployeeController:" + stopwatch.Elapsed);

                    return Ok(project.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"Update()\" of ServiceTypeToEmployeeController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Update() in ServiceTypeToEmployeeController:" + stopwatch.Elapsed);
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
        public async Task<ActionResult<ServiceType>> GetServiceTypeByEmployeeId(int employeeId)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Fetching records from  EmployeeProject table");
            try
            {
                var projects = await m_ServiceTypeToEmployeeService.GetServiceTypeByEmployeeId(employeeId);
                if (!projects.IsSuccessful)
                {
                    m_Logger.LogError(projects.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetServiceTypeByEmployeeId() in ServiceTypeToEmployeeController:" + stopwatch.Elapsed);
                    return NotFound(projects.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully fetched project details");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetServiceTypeByEmployeeId() in ServiceTypeToEmployeeController:" + stopwatch.Elapsed);
                    return Ok(projects.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"GetServiceTypeByEmployeeId()\" of ServiceTypeToEmployeeController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetServiceTypeByEmployeeId() in ServiceTypeToEmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while fetching details");
            }
        }
        #endregion
    }
}