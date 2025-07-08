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
        public async Task<IActionResult> Get(DateTime dateToSync, string location)
        {
            var result = await _biometricAttendanceSyncService.GetBiometricAttendance(dateToSync, location);

            return Ok(result);
        }

        [HttpDelete("DeleteBiometric/{dateToSync}/{location}")]
        public async Task<IActionResult> DeleteEmployee(DateTime dateToSync, string location)
        {
            var result = await _biometricAttendanceSyncService.DeleteBiometricAttendance(dateToSync, dateToSync, location);

            return Ok(result);
        }

        [HttpPost("InsertBulkBiometricAttendance")]
        public async Task<IActionResult> InsertBulkBiometricAttendance(InsertBulkBiometricAttendanceDTO dtoRequest)
        {
            //List<BiometricAttendance> _biometricAttendanceList = JsonConvert.DeserializeObject<List<BiometricAttendance>>(stringifiedjsondata);
            var result = await _biometricAttendanceSyncService.WriteBulkData(dtoRequest.dateFromSync,dtoRequest.dateToSync, dtoRequest.stringifiedjsondata);
            return Ok(result);
        }

        [HttpGet("GetExcludedAssociates")]
        public async Task<IActionResult> GetExcludedAssociates()
        {
            var result = await _biometricAttendanceSyncService.GetExcludedAssociates();

            return Ok(result);
        }
        #endregion
    }
}
