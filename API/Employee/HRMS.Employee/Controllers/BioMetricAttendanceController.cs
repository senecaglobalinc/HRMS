using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
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
    public class BioMetricAttendanceController : Controller
    {
        #region Global Variables
        private readonly IBioMetricAttendanceService _bioMetricAttendanceReportService;
        private readonly ILogger<BioMetricAttendanceController> _logger;
        #endregion

        #region Constructor
        public BioMetricAttendanceController(IBioMetricAttendanceService attendanceReportService,
                                          ILogger<BioMetricAttendanceController> logger)
        {
            _bioMetricAttendanceReportService = attendanceReportService;
            _logger = logger;
        }
        #endregion

        #region GetAttendanceSummaryReport
        /// <summary>
        /// Get Biometric Attendance Summary Report
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Route("GetAttendanceSummaryReport")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> GetAttendanceSummaryReport(AttendanceReportFilter filter)
        {
            try
            {
                _logger.LogInformation("Start of GetAttendanceSummaryReport.");
                var result = await _bioMetricAttendanceReportService.GetAttendanceSummaryReport(filter);
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
        /// <summary>
        /// Get Biometric Attendance Detail Report
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Route("GetAttendanceDetailReport")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> GetAttendanceDetailReport(AttendanceReportFilter filter)
        {
            try
            {
                _logger.LogInformation("Start of GetAttendanceDetailReport.");
                var result = await _bioMetricAttendanceReportService.GetAttendanceDetailReport(filter);
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

        #region GetAdvanceAttendanceReport
        ///<summary>
        ///Get Advance Attendance Report
        ///</summary>
        ///<param name="filter"></param>
        /// <returns></returns>
        [Route("GetAdvanceAttendanceReport")]
        [HttpPost]
        public async Task<IActionResult> GetAdvanceAttendanceReport(AttendanceReportFilter filter)
        {
            try
            {
                _logger.LogInformation("Start of GetAdvanceAttendanceReport.");
                var result = await _bioMetricAttendanceReportService.GetAdvanceAttendanceReport(filter);
                return Ok(File(result, "application/vnd.ms-excel", "AttendanceReport.xls"));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An exception occurred in GetAdvanceAttendanceReport.");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            finally
            {
                _logger.LogInformation("End of GetAdvanceAttendanceReport.");
            }
        }

        #endregion

        #region GetAttendanceMaxDate

        /// <summary>
        /// Get Biometric Attendance Max Date
        /// </summary>
        /// <returns></returns>
        [Route("GetAttendanceMaxDate")]
        [HttpGet]
        public async Task<IActionResult> GetAttendanceMaxDate()
        {
            try
            {
                _logger.LogInformation("Start of GetAttendanceMaxDate.");
                var result = await _bioMetricAttendanceReportService.GetAttendanceMaxDate();
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

        /// <summary>
        /// Get is the associate is delivery department or not
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [Route("IsDeliveryDepartment/{employeeId}")]
        [HttpGet]
        public async Task<IActionResult> IsDeliveryDepartment(int employeeId)
        {
            try
            {
                _logger.LogInformation("Start of IsDeliveryDepartment.");
                var result = await _bioMetricAttendanceReportService.IsDeliveryDepartment(employeeId);
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
        /// Get Associates Details who are Reporting To Manager
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="roleName"></param> 
        /// <param name="projectId"></param> 
        /// <returns></returns>
        [HttpGet("GetAssociatesReportingToManager/{employeeId}/{roleName}/{projectId}")]
        public async Task<IActionResult> GetAssociatesReportingToManager(int employeeId, string roleName, int projectId = 0, bool isLeadership = false)
        {
            try
            {
                var employees = await _bioMetricAttendanceReportService.GetAssociatesReportingToManager(employeeId, roleName, projectId, isLeadership);
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
        /// Get Projects Details By Manager
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="roleName"></param>          
        /// <returns></returns>
        [HttpGet("GetProjectsByManager/{employeeId}/{roleName}")]
        public async Task<IActionResult> GetProjectsByManager(int employeeId, string roleName)
        {
            try
            {
                var projects = await _bioMetricAttendanceReportService.GetProjectsByManager(employeeId, roleName);
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

        #region GetAttendanceMuster
        /// <summary>
        /// Get Attendance Muster
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpGet("GetAttendanceMuster/{year}/{month}")]
        public async Task<IActionResult> GetAttendanceMuster(int year,int month)
        {
            try
            {
                var projects = await _bioMetricAttendanceReportService.GetAttendanceMuster(year,month);
                if (projects == null)
                {
                    _logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"records found");
                    return Ok(projects.Item);
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
