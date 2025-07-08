using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using HRMS.Project.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMS.Project.API
{
    /// <summary>
    /// Controller for Addendum object.
    /// </summary>
    [Route("project/api/v1/[controller]")]
    [ApiController]
  //  [Authorize]
    public class HRMSExternalController : Controller
    {
        #region Global Variables

        private readonly IHRMSExternalService m_hRMSExternalServive;
        private readonly ILogger<HRMSExternalController> m_Logger;

        #endregion

        #region Constructor
        public HRMSExternalController(IHRMSExternalService hRMSExternalService,
            ILogger<HRMSExternalController> logger)
        {
            m_hRMSExternalServive = hRMSExternalService;
            m_Logger = logger;
        }
        #endregion


        #region GetEmployeeProjectDetails
        /// <summary>
        /// GetEmployeeProjectDetails
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmployeeProjectDetails")]
        public async Task<IActionResult> GetEmployeeProjectDetails()
        {
            try
            {
                var employee = await m_hRMSExternalServive.GetEmployeeProjectDetails();
                if (employee == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found");
                    var outputObject = new { Employees = employee.Items };
                    return Ok(outputObject);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion


        #region GetProjectsByEmpIdAndRole
        /// <summary>
        /// Get Projects By EmployeeId And Role
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectsByEmpIdAndRole/{employeeId}")]
        public async Task<IActionResult> GetProjectsByEmpIdAndRole(int employeeId)
        {
            try
            {
                var projectDetails = await m_hRMSExternalServive.GetProjectsByEmpIdAndRole(employeeId);
                if (!projectDetails.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound(projectDetails);
                }
                else
                {

                    return Ok(projectDetails);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

    }
}
