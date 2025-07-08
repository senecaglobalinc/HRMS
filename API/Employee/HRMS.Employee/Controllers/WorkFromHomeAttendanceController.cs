using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkFromHomeAttendanceController : Controller
    {
        #region Global Variables

        private readonly IWorkFromHomeAttendanceService m_workFromHomeAttendance;
        private readonly ILogger<WorkFromHomeAttendanceController> m_Logger;

        #endregion

        #region Constructor
        public WorkFromHomeAttendanceController(IWorkFromHomeAttendanceService workFromHomeAttendance,
            ILogger<WorkFromHomeAttendanceController> logger)
        {
            m_workFromHomeAttendance = workFromHomeAttendance;
            m_Logger = logger;
        }
        #endregion

        #region SaveAttendanceDetais
        /// <summary>
        ///  Save Work from home day attendance detail of an associate
        /// </summary>
        /// <param name="bioMetricAttendance"></param>
        /// <returns></returns>
        [HttpPost("SaveAttendanceDetais")]
        public async Task<IActionResult> SaveAttendanceDetais(BioMetricAttendance bioMetricAttendance)
        {
            try
            {
                var response = await m_workFromHomeAttendance.SaveAttendanceDetais(bioMetricAttendance);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError($"Error occurred while saving WorkFromHomeAttendance details: {(string)response.Message}");

                    return Ok(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {

                //Add exeption to logger
                m_Logger.LogError($"Error occurred while saving WorkFromHomeAttendance details: {ex}");

                return BadRequest("Error occurred while saving WorkFromHomeAttendance details.");
            }
        }
        #endregion

        #region GetAttendanceDetais
        /// <summary>
        /// Get Associates Work from home attendance detail of the day.
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns></returns>
        [HttpGet("GetAttendanceDetais/{employeeCode}")]
        public async Task<IActionResult> GetAttendanceDetais(string  employeeCode)
        {
            try
            {
                var response = await m_workFromHomeAttendance.GetAttendanceDetais(employeeCode);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError($"Error occurred while fetching WorkFromHomeAttendance details: {(string)response.Message}");

                    return Ok(response);
                }
                else
                {

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {

                //Add exeption to logger
                m_Logger.LogError($"Error occurred while fetching records from WorkFromHomeAttendance details: {ex}");

                return BadRequest("Error occurred while fetching records from WorkFromHomeAttendance details.");
            }
        }
        #endregion

        #region GetloginStatus
        /// <summary>
        /// Get Associates logedIn/Signin status detail of the day.
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns></returns>
        [HttpGet("GetloginStatus/{employeeCode}")]
        public async Task<IActionResult> GetloginStatus(string employeeCode)
        {
            try
            {
                var response = await m_workFromHomeAttendance.GetloginStatus(employeeCode);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError($"Error occurred while fetching Attendance details: {(string)response.Message}");

                    return NotFound(response);
                }
                else
                {

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {

                //Add exeption to logger
                m_Logger.LogError($"Error occurred while fetching records from Attendance details: {ex}");

                return BadRequest("Error occurred while fetching records from Attendance details.");
            }
        }
        #endregion
    }
}
