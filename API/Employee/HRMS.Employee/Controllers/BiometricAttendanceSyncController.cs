using HRMS.Employee.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using HRMS.Employee.Infrastructure.Models.Domain;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    /// <summary>
    /// Controller for performing biometric attendance synchronization
    /// operations between external systems and the HRMS database.
    /// </summary>
    public class BiometricAttendanceSyncController : ControllerBase
    {
        #region Global Variables
        private readonly IBiometricAttendanceSyncService _biometricAttendanceSyncService;
        private readonly ILogger<BiometricAttendanceSyncController> _logger;
        #endregion

        #region Constructor
        public BiometricAttendanceSyncController(IBiometricAttendanceSyncService attendanceSyncService,
                                          ILogger<BiometricAttendanceSyncController> logger)
        {
            _biometricAttendanceSyncService = attendanceSyncService;
            _logger = logger;
        }
        #endregion

        #region Biometric Attendance Sync
        [HttpGet("Get/{dateToSync}/{location}")]
        /// <summary>
        /// Retrieves biometric attendance information for the specified date
        /// and location.
        /// </summary>
        /// <param name="dateToSync">Date for which data should be retrieved.</param>
        /// <param name="location">Location identifier.</param>
        /// <returns>Collection of biometric attendance records.</returns>
        public async Task<IActionResult> Get(DateTime dateToSync, string location)
        {
            var result = await _biometricAttendanceSyncService.GetBiometricAttendance(dateToSync, location);

            return Ok(result);
        }

        [HttpDelete("DeleteBiometric/{dateToSync}/{location}")]
        /// <summary>
        /// Deletes biometric attendance data for the specified date range and
        /// location.
        /// </summary>
        /// <param name="dateToSync">Start and end date for deletion.</param>
        /// <param name="location">Location identifier.</param>
        /// <returns>Result of the delete operation.</returns>
        public async Task<IActionResult> DeleteEmployee(DateTime dateToSync, string location)
        {
            var result = await _biometricAttendanceSyncService.DeleteBiometricAttendance(dateToSync, dateToSync, location);

            return Ok(result);
        }

        [HttpPost("InsertBulkBiometricAttendance")]
        /// <summary>
        /// Inserts bulk biometric attendance records into the system.
        /// </summary>
        /// <param name="dtoRequest">DTO containing the attendance data.</param>
        /// <returns>Result of the insert operation.</returns>
        public async Task<IActionResult> InsertBulkBiometricAttendance(InsertBulkBiometricAttendanceDTO dtoRequest)
        {
            //List<BiometricAttendance> _biometricAttendanceList = JsonConvert.DeserializeObject<List<BiometricAttendance>>(stringifiedjsondata);
            var result = await _biometricAttendanceSyncService.WriteBulkData(dtoRequest.dateFromSync,dtoRequest.dateToSync, dtoRequest.stringifiedjsondata);
            return Ok(result);
        }

        [HttpGet("GetExcludedAssociates")]
        /// <summary>
        /// Retrieves the list of associates excluded from biometric processing.
        /// </summary>
        /// <returns>List of excluded associates.</returns>
        public async Task<IActionResult> GetExcludedAssociates()
        {
            var result = await _biometricAttendanceSyncService.GetExcludedAssociates();

            return Ok(result);
        }
        #endregion
    }
}
