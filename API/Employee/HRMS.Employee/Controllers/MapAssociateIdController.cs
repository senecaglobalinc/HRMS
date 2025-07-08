using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class MapAssociateIdController : Controller
    {
        #region Global Variables

        private readonly IMapAssociateIdService m_MapAssociateIdService;
        private readonly ILogger<MapAssociateIdController> m_Logger;
        private readonly IOrganizationService m_OrganizationService;
        #endregion

        #region Constructor
        public MapAssociateIdController(IMapAssociateIdService mapAssociateIdService,
            ILogger<MapAssociateIdController> logger, IOrganizationService organizationService)
        {
            m_OrganizationService = organizationService;
            m_MapAssociateIdService = mapAssociateIdService;
            m_Logger = logger;
        }
        #endregion

        #region GetUnMappedUsers
        /// <summary>
        /// Retrieves active users where the UserId is not mapped to employee.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUnMappedUsers")]
        public async Task<ActionResult<IEnumerable>> GetUnMappedUsers()
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving records from Users table.");

            try
            {
                var users = await m_MapAssociateIdService.GetUnMappedUsers();
                if (!users.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in users table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetUnMappedUsers() in MapAssociateIdController:" + stopwatch.Elapsed);
                    return NotFound(users.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning unMapped users.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetUnMappedUsers() in MapAssociateIdController:" + stopwatch.Elapsed);
                    return Ok(users.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured while getting UnMappedUsers in GetUnMappedUsers method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetUnMappedUsers() in MapAssociateIdController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while getting UnMappedUsers in GetUnMappedUsers method");
            }
        }
        #endregion


        #region MapAssociateId
        /// <summary>
        /// Maps the UserId to the UserId in Employee
        /// <param name="associate"></param>
        /// </summary>
        /// <returns></returns>
        [HttpPost("MapAssociateId")]
        public async Task<ActionResult> MapAssociateId(EmployeeDetails employee)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Map Associate Id to User Id.");

            try
            {
                dynamic response = await m_MapAssociateIdService.MapAssociateId(employee);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogError("Error occurred while mapping associate Id: " + (string)response.Message);
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute MapAssociateId() in MapAssociateIdController:" + stopwatch.Elapsed);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Successfully Mapped Associate Id");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute MapAssociateId() in MapAssociateIdController:" + stopwatch.Elapsed);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured while mapping user Id in MapAssociateId method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute MapAssociateId() in MapAssociateIdController:" + stopwatch.Elapsed);
                return BadRequest("Error occurred while mapping user Id in MapAssociateId method");
            }
        }
        #endregion
    }
}