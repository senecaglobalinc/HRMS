using HRMS.Project.Entities;
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
    public class ProjectManagerController : Controller
    {
        #region Global Variables

        private readonly IProjectManagerService m_ProjectManagerService;
        private readonly ILogger<ProjectManagerController> m_Logger;

        #endregion

        #region Constructor
        public ProjectManagerController(IProjectManagerService projectManagerService,
            ILogger<ProjectManagerController> logger)
        {
            m_ProjectManagerService = projectManagerService;
            m_Logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// This method creates new project manager.
        /// </summary>
        /// <param name="projectManagerIn"></param>
        /// <returns>Project</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProjectManager projectManagerIn)
        {
            m_Logger.LogInformation("Inserting record in project manager's table.");
            try
            {
                var response = await m_ProjectManagerService.Create(projectManagerIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating project manager: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in project manager's table.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Project Id: " + projectManagerIn.ProjectId);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while project: " + ex);

                return BadRequest("Error occurred while creating project.");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetAll project managers
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool? isActive)
        {
            m_Logger.LogInformation("Retrieving records from project manager table.");

            try
            {
                var projectManagers = await m_ProjectManagerService.GetAll(isActive);
                if (projectManagers == null)
                {
                    m_Logger.LogInformation("No records found in project manager table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { projectManagers.Items.Count } project managers.");
                    return Ok(projectManagers.Items);
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
            m_Logger.LogInformation($"Retrieving records from project managers table by {id}.");

            try
            {
                var projectManager = await m_ProjectManagerService.GetById(id);
                if (projectManager.Item == null)
                {
                    m_Logger.LogInformation($"No records found for Id {id}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for id {id}.");
                    return Ok(projectManager.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetByEmployeeId
        /// <summary>
        /// Get the data of Program Manager/ Reporting Manager/ Lead by employeeId
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetByEmployeeId/{employeeId}")]
        public async Task<ActionResult<List<ProjectManager>>> GetByEmployeeId(int employeeId)
        {
            m_Logger.LogInformation($"Retrieving records from project managers table by {employeeId}.");

            try
            {
                var projectManager = await m_ProjectManagerService.GetByEmployeeId(employeeId);
                if (projectManager == null)
                {
                    m_Logger.LogInformation($"No records found for Id {employeeId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for id {employeeId}.");
                    return Ok(projectManager.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetProjectManagerByEmployeeId
        /// <summary>
        /// Get the data of Program Manager/ Reporting Manager/ Lead by list of employeeIds
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProjectManagerByEmployeeId")]
        public async Task<ActionResult<Entities.ProjectManager>> GetProjectManagerByEmployeeId([FromQuery(Name = "employeeIds")] string employeeIds)
        {
            m_Logger.LogInformation($"Retrieving records from project managers table by {employeeIds}.");

            try
            {
                var projectManager = await m_ProjectManagerService.GetProjectManagerByEmployeeId(employeeIds);
                if (projectManager == null)
                {
                    m_Logger.LogInformation($"No records found for Id {employeeIds}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for id {employeeIds}.");
                    return Ok(projectManager);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetActiveProjectManagers
        /// <summary>
        /// Get the Active project managers
        /// <param name="isActive"></param>
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetActiveProjectManagers")]
        public async Task<ActionResult<IEnumerable>> GetActiveProjectManagers(bool? isActive = true)
        {
            m_Logger.LogInformation("Retrieving Active records from project manager table.");

            try
            {
                var projectManagers = await m_ProjectManagerService.GetActiveProjectManagers(isActive);
                if (!projectManagers.IsSuccessful)
                {
                    m_Logger.LogInformation("No Active records found in project manager table.");
                    return Ok(projectManagers.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning { projectManagers.Items.Count } project managers.");
                    return Ok(projectManagers.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured while retreiving Active ProjectManagers" + ex.Message);
                return BadRequest("Error occured while retreiving Active ProjectManagers");
            }
        }
        #endregion

        #region GetReportingDetailsByProjectId
        /// <summary>
        /// Get Reporting Details By ProjectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("GetReportingDetailsByProjectId/{projectId}")]
        public async Task<IActionResult> GetReportingDetailsByProjectId(int projectId)
        {
            try
            {
                var projectManagers = await m_ProjectManagerService.GetReportingDetailsByProjectId(projectId);
                if (projectManagers == null)
                {
                    m_Logger.LogInformation("No Active records found in project manager table.");
                    return NotFound();
                }
                else
                {
                    return Ok(projectManagers.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured while retreiving Active ProjectManagers" + ex.Message);
                return BadRequest("Error occured while retreiving Active ProjectManagers");
            }
        }
        #endregion

        #region GetLeadsManagersBySearchString
        /// <summary>
        /// Gets the leads and Manager information by searchString
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [HttpGet("GetLeadsManagersBySearchString/{searchString}")]
        public async Task<ActionResult<List<EmployeeDetails>>> GetLeadsManagersBySearchString(string searchString)
        {

            m_Logger.LogInformation($"Retrieving records from Employee table by {searchString}.");

            try
            {
                var employee = await m_ProjectManagerService.GetLeadsManagersBySearchString(searchString);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found for {searchString}.");

                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found for {searchString}.");

                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetLeadsManagersBySearchString() method in ProjectManagerController:" + ex.StackTrace);

                return BadRequest("Error Occured in GetLeadsManagersBySearchString() method in ProjectManagerController:");
            }

        }
        #endregion

        #region GetLeadsManagersForDropdown
        /// <summary>
        /// Gets the leads and Manager information 
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [HttpGet("GetLeadsManagersForDropdown")]
        public async Task<ActionResult<List<GenericType>>> GetLeadsManagersForDropdown()
        {

            m_Logger.LogInformation($"Retrieving records from Employee table.");

            try
            {
                var employee = await m_ProjectManagerService.GetLeadsManagersForDropdown();
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found.");

                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found.");

                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetLeadsManagersForDropdown() method in ProjectManagerController:" + ex.StackTrace);

                return BadRequest("Error Occured in GetLeadsManagersForDropdown() method in ProjectManagerController:");
            }

        }
        #endregion

        #region GetManagerandLeadByProjectIdandEmpId
        /// <summary>
        /// Gets the leads and Manager information by projectId and EmployeeId
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetManagerandLeadByProjectIDandEmpId/{projectId}/{employeeId}")]
        public async Task<ActionResult<List<ManagersData>>> GetManagerandLeadByProjectIdandEmpId(int projectId, int employeeId)
        {

            m_Logger.LogInformation($"GetManagerandLeadByProjectIdandEmpId method in ProjectManager controller.");

            try
            {
                var employee = await m_ProjectManagerService.GetManagerandLeadByProjectIdandEmpId(projectId, employeeId);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found.");

                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found.");

                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetManagerandLeadByProjectIDandEmpId() method in ProjectManangerController:" + ex.StackTrace);

                return BadRequest("Error Occured in GetManagerandLeadByProjectIDandEmpId() method in ProjectManangerController:");
            }

        }
        #endregion

        #region UpdateReportingManagerToAssociate
        /// <summary>
        /// UpdateReportingManagerToAssociate
        /// </summary>
        /// <param name="projectData"></param>
        /// <param name="isDelivery"></param>
        /// <returns></returns>
        [HttpPost("UpdateReportingManagerToAssociate/{isDelivery}")]
        public async Task<ActionResult<bool>> UpdateReportingManagerToAssociate(ProjectRequest projectData, bool isDelivery)
        {

            m_Logger.LogInformation($"UpdateReportingManagerToAssociate method in ProjectManager controller.");

            try
            {
                var employee = await m_ProjectManagerService.UpdateReportingManagerToAssociate(projectData, isDelivery);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"Unable to update.");

                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"updated successfully.");

                    return Ok(employee.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in UpdateReportingManagerToAssociate() method in ProjectManagerController:" + ex.StackTrace);

                return BadRequest("Error Occured in UpdateReportingManagerToAssociate() method in ProjectManagerController:");
            }

        }
        #endregion

        #region SaveManagersToProject
        /// <summary>
        /// SaveManagersToProject
        /// </summary>
        /// <param name="projectData"></param>
        /// <returns></returns>
        [HttpPost("SaveManagersToProject")]
        public async Task<IActionResult> SaveManagersToProject(ProjectManagersData projectManagerIn)
        {
            try
            {
                var response = await m_ProjectManagerService.SaveManagersToProject(projectManagerIn);

                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating project manager: " + (string)response.Message);
                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in project manager's table.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Project Id: " + projectManagerIn.ProjectId);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while project manager's table: " + ex);

                return BadRequest("Error occurred while creating project manager's table.");
            }
        }
        #endregion

        #region GetProgramManagersForDropdown
        /// <summary>
        /// GetProgramManagersForDropdown
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetProgramManagersForDropdown")]
        public async Task<ActionResult<IEnumerable>> GetProgramManagersForDropdown()
        {
            m_Logger.LogInformation("Retrieving records from clients table.");

            try
            {
                var programManagers = await m_ProjectManagerService.GetProgramManagersForDropdown();
                if (programManagers == null)
                {
                    m_Logger.LogInformation("No records found in ProjectManagers table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { programManagers.Items.Count} ProjectManagers.");
                    return Ok(programManagers.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetProjectLeadData
        /// <summary>
        /// GetProjectManagersData
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetProjectLeadData/{employeeID}")]
        public async Task<ActionResult<IEnumerable>> GetProjectLeadData(int employeeID)
        {
            m_Logger.LogInformation("Retrieving records from clients table.");

            try
            {
                var programManagers = await m_ProjectManagerService.GetProjectLeadData(employeeID);
                if (programManagers == null)
                {
                    m_Logger.LogInformation("No records found in ProjectManagers table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { programManagers.Items.Count} ProjectManagers.");
                    return Ok(programManagers.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetProjectRMData
        /// <summary>
        /// GetProjectRMData
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetProjectRMData/{employeeID}")]
        public async Task<ActionResult<IEnumerable>> GetProjectRMData(int employeeID)
        {
            m_Logger.LogInformation("Retrieving records from clients table.");

            try
            {
                var programManagers = await m_ProjectManagerService.GetProjectRMData(employeeID);
                if (programManagers == null)
                {
                    m_Logger.LogInformation("No records found in ProjectManagers table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { programManagers.Items.Count} ProjectManagers.");
                    return Ok(programManagers.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetProjectManagerFromAllocations
        /// <summary>
        /// GetProjectManagerFromAllocations
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetProjectManagerFromAllocations/{employeeID}")]
        public async Task<ActionResult<bool>> GetProjectManagerFromAllocations(int employeeID)
        {
            m_Logger.LogInformation("Retrieving records from clients table.");

            try
            {
                var isTrue = await m_ProjectManagerService.GetProjectManagerFromAllocations(employeeID);
                if (!isTrue)
                {
                    m_Logger.LogInformation("No records found in AssociateAllocation table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { isTrue}.");
                    return Ok(isTrue);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetPMByPracticeArea
        /// <summary>
        /// Get ProjectManagers by practiceAreaId
        /// </summary>
        /// <param name="practiceAreaId">practiceAreaId</param>
        /// <returns></returns>

        [HttpGet("GetPMByPracticeAreaId/{practiceAreaId}")]
        public async Task<ActionResult<ProjectManager>> GetPMByPracticeAreaId(int practiceAreaId)
        {
            m_Logger.LogInformation($"Retrieving records from project managers table by {practiceAreaId}.");

            try
            {
                var projectManager = await m_ProjectManagerService.GetPMByPracticeAreaId(practiceAreaId);
                if (projectManager == null)
                {
                    m_Logger.LogInformation($"No records found for Id {practiceAreaId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for id {practiceAreaId}.");
                    return Ok(projectManager);
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
