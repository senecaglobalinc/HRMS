using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.IO;
using HRMS.Common.Enums;
using HRMS.Common.Extensions;
using HRMS.Project.API.Auth;

namespace HRMS.Project.API.Controllers
{
    [Route("project/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectClosureReportController : ControllerBase
    {
        #region Global Variables

        private readonly IProjectClosureReportService m_ProjectClosureReportService;
        private readonly ILogger<ProjectClosureReportController> m_Logger;

        #endregion

        #region Constructor
        public ProjectClosureReportController(IProjectClosureReportService ProjectClosureReportService,
            ILogger<ProjectClosureReportController> logger)
        {
            m_ProjectClosureReportService = ProjectClosureReportService;
            m_Logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// This method creates new projectClosureReport by taking ProjectId as parameter .
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>integer if reteun is"0" then the report is not created 
        /// if return response is Project Clsoure Id then the report is created sucessfully</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(int projectId)
        {
            m_Logger.LogInformation("Inserting record in projectClosureReport's table.");
            try
            {
                var response = await m_ProjectClosureReportService.Create(projectId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError($"Error occurred while creating project closure report: {(string)response.Message}");

                    
                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in project closure report's table.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError($"Error occurred while project: {ex}");

                return BadRequest("Error occurred while creating project closure report.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// This method updates existing project closure report data by taking ProjectClosureReportRequest as parameter .
        /// </summary>
        /// <param name="projectIn"></param>
        /// <returns>integer if reteun is"0" then the report is not created 
        /// if return response is Project Clsoure Id then the report is created sucessfully</returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(ProjectClosureReportRequest projectIn)
        {
            m_Logger.LogInformation("Updating record in project closure report's table.");
            try
            {
                var response = await m_ProjectClosureReportService.Update(projectIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError($"Error occurred while updating project closure report : {(string)response.Message}");

                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in project closure report's table.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
               
                //Add exeption to logger
                m_Logger.LogError($"Error occurred while updating project closure report data: {ex}");

                return Ok("Error occurred while updating project closure report.");
            }
        }
        #endregion

        #region GetClosureReportByProjectId
        /// <summary>
        /// Get Project Closure Report By ProjectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Project Closure Report data</returns>
        [HttpGet("GetClosureReportByProjectId/{projectId}")]
        public async Task<ActionResult<IEnumerable>> GetClosureReportByProjectId(int projectId)
        {
            m_Logger.LogInformation($"Retrieving records from ProjectClosureReport table by {projectId}.");

            try
            {
                var reportDetails = await m_ProjectClosureReportService.GetClosureReportByProjectId(projectId);
                if (reportDetails.Items == null)
                {
                    m_Logger.LogInformation($"No records found for ProjectId {projectId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for ProjectId {projectId}.");
                    return Ok(reportDetails.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region NotificationConfiguration
        /// <summary>
        /// This method creates new projectClosureReport.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Integer value 0-represents unsuccessful response and 1-represents successful response</returns>
        [HttpPost("NotificationConfiguration")]
        public async Task<IActionResult> NotificationConfiguration(int projectId, int notificationTypeId, int? departmentId)
        {
            m_Logger.LogInformation("Sending Notification.");
            try
            {
                var response = await m_ProjectClosureReportService.NotificationConfiguration(projectId, notificationTypeId, departmentId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError($"Error occurred while sending the notification: {(string)response.Message}");

                    //return BadRequest(response.Message);
                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully sent Notification");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                
                //Add exeption to logger
                m_Logger.LogError($"Error occurred while project: {ex}");

                return BadRequest("Error occurred while creating project closure report.");
            }
        }
        #endregion

        #region SaveFile
        /// <summary>
        /// Save uploaded file details
        /// </summary>
        /// <param name="uploadFiles"></param>
        /// <returns>>method returns integer "0" when files stored sucessfuly
        /// else returns "0" fails to save the files</returns>
        [HttpPost("Save")]
        public async Task<ActionResult<int>> Save([FromForm] UploadFiles uploadFiles)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Inserting record into UploadFiles table");
            try
            {
                var file = await m_ProjectClosureReportService.Save(uploadFiles);
                if (!file.IsSuccessful)
                {
                    m_Logger.LogError(file.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation($"Time to execute Save() in EmployeeFilesController: {stopwatch.Elapsed}");

                    return NotFound(file.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully inserted records into UploadFile  table");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation($"Time to execute Save() in EmployeeFilesController: {stopwatch.Elapsed}");

                    return Ok(file.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError($"Error occured in \"Save()\" of EmployeeFilesController {ex.StackTrace}");

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation($"Time to execute Save() in EmployeeFilesController: {stopwatch.Elapsed}");
                return BadRequest("Error occured while saving uploaded file details");
            }

        }

        #endregion

        #region Download
        /// <summary>
        /// This downloads the file by taking its fileType and projectId as parameter
        /// </summary>
        /// <param name="fileType "></param>
        /// <param name="projectId"></param>
        /// <returns>File</returns>
        [HttpGet("Download")]
        public async Task<IActionResult> Download(String fileType, int projectId)
        {
            m_Logger.LogInformation("Submitting record in project closure report's table.");
            try
            {
                var response = await m_ProjectClosureReportService.Download(fileType, projectId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError($"Error occurred while submitting project closure report : {(string)response.Message}");

                    return Ok(response.Message);
                }

                else
                {
                    string path = response.Item;
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                    memory.Position = 0;
                    var ext = Path.GetExtension(path).ToLowerInvariant();
                    m_Logger.LogInformation("Download successful");
                    return File(memory, HRMS.Common.Extensions.Extensions.GetMimeType()[ext], Path.GetFileName(path));


                }
            }
            catch (Exception ex)
            {
                

                //Add exeption to logger
                m_Logger.LogError($"Error occurred while submitting project closure report data: {ex}");

                return Ok("Error occurred while submitting project closure report.");
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete uploded file details
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="projectId"></param>
        /// <returns>boolean if successfully deleted returns true else returns false </returns>
        [HttpDelete("Delete/{fileType}/{projectId}")]
        public async Task<ActionResult<bool>> Delete(string fileType, int projectId)
        {

            m_Logger.LogInformation("Deleting file from ProjectClosureReport table");
            try
            {
                var response = await m_ProjectClosureReportService.Delete(fileType, projectId);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogError(response.Message);
                    return NotFound(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted filename in ProjectClosureReport table");

                   

                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError($"Error occured in \"Delete()\" of ProjectClosureReportController {ex.StackTrace}");

                return BadRequest("Error occured while deleting uploaded file details");
            }

        }
        #endregion


    }
}
