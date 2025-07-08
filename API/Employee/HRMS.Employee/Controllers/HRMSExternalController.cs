using HRMS.Employee.API.Auth;
using HRMS.Employee.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class HRMSExternalController : ControllerBase
    {
        private readonly IHRMSExternalService m_HRMSExternalService;
        private readonly ILogger<HRMSExternalController> m_Logger;


        public HRMSExternalController(IHRMSExternalService hRMSExternalService,ILogger<HRMSExternalController> logger)
        {
            m_HRMSExternalService = hRMSExternalService;
            m_Logger = logger;
        }

        #region GetProjects
        /// <summary>
        /// Get all Project details
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjects")]
        public async Task<IActionResult> GetProjects()
        {
            try
            {
                var projectDetails = await m_HRMSExternalService.GetProjects();
                if (projectDetails == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {

                    return Ok(projectDetails.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetProjectById
        /// <summary>
        /// Get Project By Project Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectById/{projectId}")]
        public async Task<IActionResult> GetProjectById(int projectId)
        {
            try
            {
                var projectDetails = await m_HRMSExternalService.GetProjectById(projectId);
                if (projectDetails.Item.Projects == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {

                    return Ok(projectDetails.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetActiveEmployees
        /// <summary>
        /// Get Active Employees
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetActiveEmployees")]
        public async Task<IActionResult> GetActiveEmployees()
        {          

            try
            {
                var employeesList = await m_HRMSExternalService.GetActiveEmployeeNamesAsync(Request.HttpContext.RequestAborted);

                if (employeesList==null)
                {
                      return NotFound("Failed to fetch data");
                }
                else
                {
                       return Ok(employeesList);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occured while fetching employee details");
            }
        }
        #endregion

        #region GetAllEmployeeDetailsForExternal
        /// <summary>
        /// GetAllEmployeeDetailsForExternal
        /// </summary>
        /// <returns></returns>
        [Route("GetAllEmployeeDetailsForExternal")]
        [HttpGet]
        public async Task<IActionResult> GetEmpDataForExternalProjects()
        {
            var allEmployees = await m_HRMSExternalService.GetAllEmployeeDetailsForExternalAsync();
            if (allEmployees==null)
            {
                return NotFound();
            }
            return Ok(allEmployees);

        }
        #endregion

        #region GetDepartmentsList
        /// <summary>
        /// GetDepartmentsList
        /// </summary>
        /// <returns></returns>
        [Route("GetDepartmentsList")]
        [HttpGet]
        public async Task<IActionResult> GetDepartmentsList()
        {
            try
            {
                var deptDtls = (await m_HRMSExternalService.GetDepartmentsList());
                if(!deptDtls.IsSuccessful)
                {
                    return NotFound();
                }
                return Ok(deptDtls.Item);
               
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occured while fetching department details");
            }
        }
        #endregion

        #region GetProjectsBasedOnEmailAndRole
        /// <summary>
        /// Get Projects Based On Email and Role
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectsByEmailAndRole/{emailId}")]
        public async Task<IActionResult> GetProjectsByEmailAndRole(string emailId)
        {
            try
            {
                var projectDetails = await m_HRMSExternalService.GetProjectsByEmailAndRole(emailId);
                if (!projectDetails.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound(projectDetails);
                }
                else
                {

                    return Ok(projectDetails.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetProgramManagersList
        /// <summary>
        /// GetProgramManagersList
        /// </summary>
        /// <returns></returns>
        [Route("GetProgramManagersList")]
        [HttpGet]
        public async Task<IActionResult> GetProgramManagersList()
        {
            try
            {
                var pmList = await m_HRMSExternalService.GetProgramManagersList();
                if (pmList == null)
                {
                    return NotFound();
                }
                return Ok(pmList);
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occured while fetching program managers list");
            }
        }
        #endregion
    }
}
