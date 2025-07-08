using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using HRMS.Project.API.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.Project.API.Controllers
{
    [Route("project/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectClosureController : ControllerBase
    {
        #region Global Variables

        private readonly IProjectClosureService m_ProjectClosureService;
        private readonly ILogger<ProjectClosureController> m_Logger;

        #endregion

        #region Constructor
        public ProjectClosureController(IProjectClosureService projectClosureService,
            ILogger<ProjectClosureController> logger)
        {
            m_ProjectClosureService = projectClosureService;
            m_Logger = logger;
        }
        #endregion

        #region ProjectClosureInitiation
        /// <summary>
        /// Initiates a Project Closure Process by taking projectData as paramenter
        /// </summary>
        /// <param name="projectData">Project information</param>
        /// <returns>Integer value 0-represent unsucessful response and >1-represent successful response</returns>
        [HttpPost("ProjectClosureInitiation")]
        public async Task<IActionResult> ProjectClosureInitiation(ProjectClosureInitiationResponse projectData)
        {
            m_Logger.LogInformation("Closing Project.");
            try
            {
                var response = await m_ProjectClosureService.ProjectClosureInitiation(projectData);
                if (!response.IsSuccessful)
                {
                   
                    m_Logger.LogError($"Error occurred while inititating closure: {(string)response.Message}");

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Project Closure Initiated Successfully");

                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {

                m_Logger.LogError($"Error occurred while closing project initiation: {ex}");

                return BadRequest("Error occurred while initiating closure of the project.");
            }
        }
        #endregion

        #region SubmitForClosureApproval
        /// <summary>
        /// Submits a project for closure approval By DH.
        /// </summary>
        /// <param name="submitForClosureApprovalRequest"></param>
        /// <returns>Integer value, 0-Represents Unsuccessful Response and 1-Represents Successful Response</returns>

        /// <returns></returns>
        [HttpPost("SubmitForClosureApproval")]
        public async Task<ActionResult<int>> SubmitForClosureApproval(SubmitForClosureApprovalRequest submitForClosureApprovalRequest)
        {
            try
            {
                var projectSubmitted = await m_ProjectClosureService.SubmitForClosureApproval(submitForClosureApprovalRequest);
                m_Logger.LogInformation($"Submitting the project for closure approval.");
                if (!projectSubmitted.IsSuccessful)
                {
                    m_Logger.LogInformation($"{projectSubmitted.Message}");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"{projectSubmitted.Message} for Id {submitForClosureApprovalRequest.projectId}");
                    return Ok(projectSubmitted.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region ApproveOrRejectClosureByDH
        /// <summary>
        /// Approves or Rejects a project by DH
        /// </summary>
        /// <param name="approveOrRejectClosureRequest"></param>
        /// <returns>Integer Value 0-Represents Unsuccessful Response and 1-Represents Successful Response</returns>
        [HttpPost("ApproveOrRejectClosureByDH")]
        public async Task<ActionResult<int>> ApproveOrRejectClosureByDH(ApproveOrRejectClosureRequest approveOrRejectClosureRequest)
        {
            try
            {
                var projectApproved = await m_ProjectClosureService.ApproveOrRejectClosureByDH(approveOrRejectClosureRequest);
                m_Logger.LogInformation($"Approving or Rejecting project closure by DH.");
                if (!projectApproved.IsSuccessful)
                {
                    m_Logger.LogInformation($"{projectApproved.Message}");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"{projectApproved.Message} for Id {approveOrRejectClosureRequest.projectId}");
                    return Ok(projectApproved.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region RejectClosure
        /// <summary>
        /// Reject a Closure Report sent by TL.
        /// </summary>
        /// <param name="rejectClosureReport"></param>
        /// <returns>Integer Value 0-Represents Unsuccessful Response and 1-Represents Successful Response</returns>
        [HttpPost("RejectClosure")]
        public async Task<ActionResult<int>> RejectClosure(RejectClosureReport rejectClosureReport)
        {
            try
            {
                var projectSubmitted = await m_ProjectClosureService.RejectClosure(rejectClosureReport);
                m_Logger.LogInformation($"Rejecting the Project Closure Report");
                if (!projectSubmitted.IsSuccessful)
                {
                    m_Logger.LogInformation($"{projectSubmitted.Message}");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"{projectSubmitted.Message} for Id {rejectClosureReport.ProjectId}");
                    return Ok(projectSubmitted.Item);
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
