using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HRMS.Employee.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendanceReportController : ControllerBase
    {
        #region Global Variables
        private readonly IAttendanceReportService _attendanceReportService;
        private readonly ILogger<AttendanceReportController> _logger;
        #endregion

        #region Constructor
        public AttendanceReportController(IAttendanceReportService attendanceReportService,
                                          ILogger<AttendanceReportController> logger)
        {
            _attendanceReportService = attendanceReportService;
            _logger = logger;
        }
        #endregion

        #region GetAttendanceSummaryReport
        [Route("GetAttendanceSummaryReport")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> GetAttendanceSummaryReport(AttendanceReportFilter filter)
        {
            try
            {
                _logger.LogInformation("Start of GetAttendanceSummaryReport.");
                var result = await _attendanceReportService.GetAttendanceSummaryReport(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred in GetAttendanceSummaryReport.");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            finally
            {
                _logger.LogInformation("End of GetAttendanceSummaryReport.");
            }
        }
        #endregion

        #region GetAttendanceDetailReport
        [Route("GetAttendanceDetailReport")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> GetAttendanceDetailReport(AttendanceReportFilter filter)
        {
            try
            {
                _logger.LogInformation("Start of GetAttendanceDetailReport.");
                var result = await _attendanceReportService.GetAttendanceDetailReport(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred in GetAttendanceDetailReport.");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            finally
            {
                _logger.LogInformation("End of GetAttendanceDetailReport.");
            }
        }
        #endregion

        #region GetAttendanceMaxDate
        [Route("GetAttendanceMaxDate")]
        [HttpGet]
        public async Task<IActionResult> GetAttendanceMaxDate()
        {
            try
            {
                _logger.LogInformation("Start of GetAttendanceMaxDate.");
                var result = await _attendanceReportService.GetAttendanceMaxDate();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred in GetAttendanceMaxDate.");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            finally
            {
                _logger.LogInformation("End of GetAttendanceMaxDate.");
            }
        }
        #endregion

        #region IsDeliveryDepartment
        [Route("IsDeliveryDepartment/{employeeId}")]
        [HttpGet]
        public async Task<IActionResult> IsDeliveryDepartment(int employeeId)
        {
            try
            {
                _logger.LogInformation("Start of IsDeliveryDepartment.");
                var result = await _attendanceReportService.IsDeliveryDepartment(employeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred in IsDeliveryDepartment.");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            finally
            {
                _logger.LogInformation("End of IsDeliveryDepartment.");
            }
        }
        #endregion

        #region GetAssociatesReportingToManager
        /// <summary>
        /// GetAssociatesReportingToManager
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="roleName"></param> 
        /// <param name="projectId"></param> 
        /// <returns></returns>
        [HttpGet("GetAssociatesReportingToManager/{employeeId}/{roleName}/{projectId}")]
        public async Task<IActionResult> GetAssociatesReportingToManager(int employeeId, string roleName,int projectId = 0, bool isLeadership = false)
        {
            try
            {
                var employees = await _attendanceReportService.GetAssociatesReportingToManager(employeeId, roleName, projectId, isLeadership);
                if (employees == null)
                {
                    _logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"records found");
                    return Ok(employees.Items);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetProjectsByManager
        /// <summary>
        /// Get Projects By Manager
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="roleName"></param>          
        /// <returns></returns>
        [HttpGet("GetProjectsByManager/{employeeId}/{roleName}")]
        public async Task<IActionResult> GetProjectsByManager(int employeeId, string roleName)
        {
            try
            {
                var projects = await _attendanceReportService.GetProjectsByManager(employeeId, roleName);
                if (projects == null)
                {
                    _logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"records found");
                    return Ok(projects.Items);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion
    }
}
