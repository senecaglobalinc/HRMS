using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Models.Domain;
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
    public class AttendanceRegularizationController : ControllerBase
    {

        #region Global Variables

        private readonly IAttendanceRegularizationService m_AttendanceRegularization;
        private readonly ILogger<EmployeeController> m_Logger;

        #endregion

        #region Constructor
        public AttendanceRegularizationController(IAttendanceRegularizationService  attendanceRegularization, ILogger<EmployeeController> logger)
        {
            m_AttendanceRegularization = attendanceRegularization;
            m_Logger = logger;
        }
        #endregion

        #region GetNotPunchInDates
        /// <summary>
        /// Get Not Punch-In Dates from the given dates of an associate
        /// </summary>
        /// <param name="attendanceRegularizationFilter"></param>
        /// <returns></returns>
        [HttpPost("GetNotPunchInDates")]
        public async Task<ActionResult> GetNotPunchInDates(AttendanceRegularizationFilter attendanceRegularizationFilter)
        {
            try
            {
                var attendanceRegularization = await m_AttendanceRegularization.GetNotPunchInDates(attendanceRegularizationFilter);
                if (!attendanceRegularization.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in Attendance table.");
                    return NotFound(attendanceRegularization.Message);
                }
                else
                {
                    return Ok(attendanceRegularization.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetNotPunchInDates method in AttendanceRegularizationController" + ex.StackTrace);
                return BadRequest("Error Occured while getting GetNotPunchInDates");
            }


        }
        #endregion

        #region SaveAttendanceRegularizationDetails
        /// <summary>
        ///Save attendance regularization details of an associate
        /// </summary>
        /// <param name="attendanceRegularizationWorkFlow"></param>
        /// <returns></returns>
        [HttpPost("SaveAttendanceRegularizationDetails")]
        public async Task<ActionResult> SaveAttendanceRegularizationDetails(List<AttendanceRegularizationWorkFlow> attendanceRegularizationWorkFlow)
        {
            try
            {
                var attendanceRegularization = await m_AttendanceRegularization.SaveAttendanceRegularizationDetails(attendanceRegularizationWorkFlow);
                if (!attendanceRegularization.IsSuccessful)
                {
                    m_Logger.LogInformation("Failed to save regularization details in AttendanceRegularization table.");
                    return NotFound(attendanceRegularization.Message);
                }
                else
                {
                    return Ok(attendanceRegularization);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in SaveAttendanceRegularizationDetails method in AttendanceRegularizationController" + ex.StackTrace);
                return BadRequest("Error Occured while Saving Attendance Regularization Details");
            }


        }
        #endregion

        #region ApproveOrRejectAttendanceRegularizationDetails
        /// <summary>
        /// Approve Attendance Regularization Details By RM
        /// </summary>
        /// <param name="regularizationWorkFlowDetails"></param>      
        /// <returns></returns>
        [HttpPost("ApproveOrRejectAttendanceRegularizationDetails")]
        public async Task<ActionResult> ApproveOrRejectAttendanceRegularizationDetails(AttendanceRegularizationWorkFlowDetails regularizationWorkFlowDetails)
        {
            try
            {
                var attendanceRegularization = await m_AttendanceRegularization.ApproveOrRejectAttendanceRegularizationDetails( regularizationWorkFlowDetails);
                if (!attendanceRegularization.IsSuccessful)
                {
                    m_Logger.LogInformation("Failed to Approve Regularization details in AttendanceRegularizationWorkFlow table.");
                    return NotFound(attendanceRegularization.Message);
                }
                else
                {
                    return Ok(attendanceRegularization);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in ApproveAttendanceRegularizationDetails method in AttendanceRegularizationController" + ex.StackTrace);
                return BadRequest("Error Occured while Approving Attendance Regularization Details");
            }


        }
        #endregion

        #region GetAllAssociateSubmittedAttendanceRegularization
        /// <summary>
        ///Get All Associate Submitted Attendance Regularization details
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpGet("GetAllAssociateSubmittedAttendanceRegularization/{managerId}/{roleName}")]
        public async Task<ActionResult> GetAllAssociateSubmittedAttendanceRegularization(int managerId, string roleName)
        {
            try
            {
                var attendanceRegularization = await m_AttendanceRegularization.GetAllAssociateSubmittedAttendanceRegularization(managerId, roleName);
                if (!attendanceRegularization.IsSuccessful)
                {
                    m_Logger.LogInformation("Failed to fetch Regularization details in AttendanceRegularizationWorkFlow table.");
                    return NotFound(attendanceRegularization.Message);
                }
                else
                {
                    return Ok(attendanceRegularization.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetAllAssociateSubmittedAttendanceRegularization method in AttendanceRegularizationController" + ex.StackTrace);
                return BadRequest("Error Occured while fetching the Associates Attendance Regularization Details");
            }


        }
        #endregion

        #region GetAssociateSubmittedAttendanceRegularization
        /// <summary>
        ///Get Associate Submitted Attendance Regularization
        /// </summary>
        /// <param name="AssociateId"></param>
        /// <returns></returns>
        [HttpGet("GetAssociateSubmittedAttendanceRegularization/{AssociateId}/{roleName}")]
        public async Task<ActionResult> GetAssociateSubmittedAttendanceRegularization(string AssociateId,string roleName)
        {
            try
            {
                var attendanceRegularization = await m_AttendanceRegularization.GetAssociateSubmittedAttendanceRegularization(AssociateId, roleName);
                if (!attendanceRegularization.IsSuccessful)
                {
                    m_Logger.LogInformation("Failed to fetch Regularization details in AttendanceRegularizationWorkFlow table.");
                    return NotFound(attendanceRegularization.Message);
                }
                else
                {
                    return Ok(attendanceRegularization.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetAssociateSubmittedAttendanceRegularization method in AttendanceRegularizationController" + ex.StackTrace);
                return BadRequest("Error Occured while fetching Submitted Attendance Regularization Details");
            }


        }
        #endregion
    }
}
