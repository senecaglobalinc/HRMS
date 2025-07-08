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
    public class EmployeeFilesController : ControllerBase
    {
        #region Global Variables

        private readonly IEmployeeFilesService m_EmployeeFilesService;
        private readonly ILogger<EmployeeFilesController> m_Logger;

        #endregion

        #region Constructor
        public EmployeeFilesController(IEmployeeFilesService employeeFilesService, ILogger<EmployeeFilesController> logger)
        {
            m_EmployeeFilesService = employeeFilesService;
            m_Logger = logger;
        }
        #endregion

        #region GetByEmployeeId
        /// <summary>
        /// Get Uploaded file details based on employee Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetByEmployeeId/{employeeId}")]
        public async Task<ActionResult<List<UploadFile>>> GetByEmployeeId(int employeeId)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Fetching records from UploadFiles table");
            try
            {
                var files = await m_EmployeeFilesService.GetByEmployeeId(employeeId);
                if (!files.IsSuccessful)
                {
                    m_Logger.LogError(files.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetByEmployeeId() in EmployeeFilesController:" + stopwatch.Elapsed);

                    return NotFound(files.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully Fetched records from UploadFile table");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetByEmployeeId() in EmployeeFilesController:" + stopwatch.Elapsed);

                    return Ok(files.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"GetByEmployeeId()\" of EmployeeFilesController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetByEmployeeId() in EmployeeFilesController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while fetching uploaded file details");
            }

        }
        #endregion

        #region Save
        /// <summary>
        /// Save uploaded file details
        /// </summary>
        /// <param name="uploadFiles"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        public async Task<ActionResult<UploadFile>> Save([FromForm] UploadFiles uploadFiles)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Inserting record into UploadFiles table");
            try
            {
                var file = await m_EmployeeFilesService.Save(uploadFiles);
                if (!file.IsSuccessful)
                {
                    m_Logger.LogError(file.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Save() in EmployeeFilesController:" + stopwatch.Elapsed);

                    return NotFound(file.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully inserted records into UploadFile  table");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Save() in EmployeeFilesController:" + stopwatch.Elapsed);

                    return Ok(file.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"Save()\" of EmployeeFilesController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Save() in EmployeeFilesController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while saving uploaded file details");
            }

        }

        #endregion

        #region Delete
        /// <summary>
        /// Delete uploded file details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}/{employeeId}")]
        public async Task<ActionResult<bool>> Delete(int id, int employeeId)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Deleting record from UploadFiles table");
            try
            {
                var response = await m_EmployeeFilesService.Delete(id, employeeId);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogError(response.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Delete() in EmployeeFilesController:" + stopwatch.Elapsed);

                    return NotFound(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted record in UploadFile table");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Delete() in EmployeeFilesController:" + stopwatch.Elapsed);

                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"Delete()\" of EmployeeFilesController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Delete() in EmployeeFilesController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while deleting uploaded file details");
            }

        }
        #endregion

        #region GeneratePDFReport
        /// <summary>
        /// GeneratePDFReport
        /// </summary>
        /// <param name="uploadFiles"></param>
        /// <returns></returns>
        [HttpGet("GeneratePDFReport/{empID}")]
        public async Task<IActionResult> GeneratePDFReport(int empID)
        {
            try
            {
                var pdf = await m_EmployeeFilesService.GeneratePDFReport(empID);
                return new FileContentResult(pdf, "application/octet-stream");
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured while generating PDF. " + ex.StackTrace);
                return BadRequest("Error occured while generating PDF.");
            }

        }

        #endregion
    }
}