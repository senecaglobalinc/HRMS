using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using HRMS.Project.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMS.Project.API
{
    [Route("project/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : Controller
    {
        #region Global Variables

        private readonly IProjectService m_ProjectService;
        private readonly ILogger<ProjectController> m_Logger;

        #endregion

        #region Constructor
        public ProjectController(IProjectService projectService,
            ILogger<ProjectController> logger)
        {
            m_ProjectService = projectService;
            m_Logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// This method creates new project.
        /// </summary>
        /// <param name="projectIn"></param>
        /// <returns>Project</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProjectRequest projectIn)
        {
            m_Logger.LogInformation("Inserting record in project's table.");
            try
            {
                var response = await m_ProjectService.Create(projectIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating project: " + (string)response.Message);

                    //return BadRequest(response.Message);
                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in project's table.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Project Code: " + projectIn.ProjectCode);
                m_Logger.LogError("Project Description: " + projectIn.ProjectName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while project: " + ex);

                return BadRequest("Error occurred while creating project.");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetAll Projects
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            m_Logger.LogInformation("Retrieving records from Projects table.");

            try
            {
                var projects = await m_ProjectService.GetAll();
                if (projects.Items == null)
                {
                    m_Logger.LogInformation("No records found in Projects table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { projects.Items.Count } Projects.");
                    return Ok(projects.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }


        }
        #endregion

        #region GetById
        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Entities.Project>> GetById(int id)
        {
            m_Logger.LogInformation($"Retrieving records from Projects table by {id}.");

            try
            {
                var project = await m_ProjectService.GetById(id);
                if (project.Item == null)
                {
                    m_Logger.LogInformation($"No records found for Id {id}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for id {id}.");
                    return Ok(project.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region Update
        /// <summary>
        /// This method updates existing project.
        /// </summary>
        /// <param name="projectIn"></param>
        /// <returns>Project</returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(ProjectRequest projectIn)
        {
            m_Logger.LogInformation("Updating record in project's table.");
            try
            {
                var response = await m_ProjectService.Update(projectIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating project: " + (string)response.Message);

                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in project's table.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Project Code: " + projectIn.ProjectCode);
                m_Logger.LogError("Project Description: " + projectIn.ProjectName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating project: " + ex);

                return Ok("Error occurred while updating project.");
            }
        }
        #endregion

        #region HasActiveClientBillingRoles
        ///<summary>
        ///HasActiveClientBillingRoles
        ///</summary>
        /// /// <param name="projectId"></param>
        /// <returns></returns>

        [HttpGet("HasActiveClientBillingRoles")]
        public async Task<IActionResult> HasActiveClientBillingRoles(int projectId)
        {
            m_Logger.LogInformation("HasActiveClientBillingRoles method called.");
            try
            {
                var response = await m_ProjectService.HasActiveClientBillingRoles(projectId);

                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while getting active cbr count: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Returned client billing role count successfully.");

                    return Ok(response.Item);
                }

            }
            catch (Exception ex)
            {

                //Add exeption to logger
                m_Logger.LogError("Error occurred while getting active client billing role count: " + ex);

                return BadRequest("Error occurred while getting active client billing role count.");
            }

        }
        #endregion

        #region GetProjectsList
        /// <summary>
        /// GetAll Projects
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectsList")]
        public async Task<ActionResult<IEnumerable>> GetProjectsList(string userRole, int employeeId, string dashboard)
        {
            m_Logger.LogInformation("Retrieving records from Projects table.");

            try
            {
                var projects = await m_ProjectService.GetProjectsList(userRole, employeeId, dashboard);
                if (projects.Items == null)
                {
                    m_Logger.LogInformation("No records found in Projects table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { projects.Items.Count } Projects.");
                    return Ok(projects.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }


        }
        #endregion

        #region GetByProjectId
        /// <summary>
        /// Gets project by project id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetByProjectId/{projectId}")]
        public async Task<ActionResult<ProjectResponse>> GetByProjectId(int projectId)
        {
            m_Logger.LogInformation($"Retrieving records from Projects table by {projectId}.");

            try
            {
                var project = await m_ProjectService.GetProjectById(projectId);
                if (project.Item == null)
                {
                    m_Logger.LogInformation($"No records found for Id {projectId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for id {projectId}.");
                    return Ok(project.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region SubmitForApproval
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userRole"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("SubmitForApproval/{projectId}/{userRole}/{employeeId}")]
        public async Task<ActionResult<int>> SubmitForApproval(int projectId, string userRole, int employeeId)
        {
            try
            {
                var project_submitted = await m_ProjectService.SubmitForApproval(projectId, userRole, employeeId);
                m_Logger.LogInformation($"Submitting the project for approval.");
                if (!project_submitted.IsSuccessful)
                {
                    m_Logger.LogInformation($"{project_submitted.Message}");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"{project_submitted.Message} for Id {projectId}");
                    return Ok(project_submitted.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region ApproveOrRejectByDH
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="status"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("ApproveOrRejectByDH/{projectId}/{status}/{employeeId}")]
        public async Task<ActionResult<int>> ApproveOrRejectByDH(int projectId, string status, int employeeId)
        {
            try
            {
                var project_approved = await m_ProjectService.ApproveOrRejectByDH(projectId, status, employeeId);
                m_Logger.LogInformation($"Approving or Rejecting project by DH.");
                if (!project_approved.IsSuccessful)
                {
                    m_Logger.LogInformation($"{project_approved.Message}");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"{project_approved.Message} for Id {projectId}");
                    return Ok(project_approved.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region CloseProject
        /// <summary>
        /// This method deletes a project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Project</returns>
        [HttpPost("DeleteProject")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            m_Logger.LogInformation("Deleting Project.");
            try
            {
                var response = await m_ProjectService.DeleteProjectDetails(projectId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while deleting project: " + (string)response.Message);

                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted the project.");

                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {

                //Add exeption to logger
                m_Logger.LogError("Error occurred while deleting project: " + ex.Message + "\n" + ex.StackTrace);

                return BadRequest("Error occurred while deleting project.");
            }
        }
        #endregion

        #region GetProjectsForAllocation
        /// <summary>
        /// Get Projects ForAllocation
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectsForAllocation")]
        public async Task<ActionResult<ProjectResponse>> GetProjectsForAllocation()
        {
            m_Logger.LogInformation($"Retrieving records from Projects");

            try
            {
                var project = await m_ProjectService.GetProjectsForAllocation();
                if (project.Items == null)
                {
                    m_Logger.LogInformation($"No records found for projects.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for projects.");
                    return Ok(project.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetAssociateProjectsForRelease
        /// <summary>
        /// Get Associate Projects For Release
        /// <paramref name="employeeId"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAssociateProjectsForRelease/{employeeId}")]
        public async Task<ActionResult<ProjectResponse>> GetAssociateProjectsForRelease(int employeeId)
        {
            m_Logger.LogInformation($"Retrieving records from Projects");

            try
            {
                var project = await m_ProjectService.GetAssociateProjectsForRelease(employeeId);
                if (project.Items == null)
                {
                    m_Logger.LogInformation($"No records found for projects.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for projects.");
                    return Ok(project.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetEmpTalentPool
        /// <summary>
        /// Get Emp TalentPool
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetEmpTalentPool/{employeeId}")]
        public async Task<IActionResult> GetEmpTalentPool(int employeeId)
        {
            try
            {
                var project = await m_ProjectService.GetEmpTalentPool(employeeId);
                if (project.Item == null)
                {
                    m_Logger.LogInformation($"No records found for Id {employeeId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for id {employeeId}.");
                    return Ok(project.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetEmpTalentPool
        /// <summary>
        /// Get Emp TalentPool
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetEmpTalentPool/{employeeId}/{projectId}/{roleName}")]
        public async Task<IActionResult> GetEmpTalentPool(int employeeId, int projectId, string roleName)
        {
            try
            {
                var project = await m_ProjectService.GetEmpTalentPool(employeeId, projectId, roleName);
                if (project.Items == null)
                {
                    m_Logger.LogInformation($"No records found for Id {employeeId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for id {employeeId}.");
                    return Ok(project.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetProjectsByEmpId
        /// <summary>
        /// Gets projects by employee id 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetProjectsByEmpId/{employeeId}")]
        public async Task<ActionResult<ProjectDetails>> GetProjectsByEmpId(int employeeId)
        {
            m_Logger.LogInformation($"Retrieving records from Projects table by {employeeId}.");

            try
            {
                var project = await m_ProjectService.GetProjectsByEmpId(employeeId);
                if (project.Items == null)
                {
                    m_Logger.LogInformation($"No records found for Id {employeeId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for id {employeeId}.");
                    return Ok(project.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetProjectsForDropdown
        /// <summary>
        /// Get Projects For Dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectsForDropdown")]
        public async Task<ActionResult<GenericType>> GetProjectsForDropdown()
        {
            m_Logger.LogInformation($"Retrieving records from Projects");

            try
            {
                var project = await m_ProjectService.GetProjectsForDropdown();
                if (project.Items == null)
                {
                    m_Logger.LogInformation($"No records found for projects.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for projects.");
                    return Ok(project.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetProjectsByIds
        /// <summary>
        /// Get the projects by Ids
        /// </summary>
        /// <param name="projectIds">project Ids</param>
        /// <returns></returns>

        [HttpGet("GetProjectsByIds/{projectIds}")]
        public async Task<ActionResult<ServiceListResponse<Entities.Project>>> GetProjectsByIds(string projectIds)
        {
            m_Logger.LogInformation($"Retrieving records from Projects table by {projectIds}.");

            try
            {
                var project = await m_ProjectService.GetProjectsByIds(projectIds);
                if (project.Items == null)
                {
                    m_Logger.LogInformation($"No records found for Id {projectIds}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for id {projectIds}.");
                    return Ok(project.Items);
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
