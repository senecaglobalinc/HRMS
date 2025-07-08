using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using HRMS.Project.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Project.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("project/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AssociateAllocationController : Controller
    {
        #region Global Variables

        private readonly IAssociateAllocationService m_AssociateAllocationService;
        private IOrganizationService m_OrgService;
        private readonly ILogger<AssociateAllocationController> m_Logger;

        #endregion

        #region Constructor
        public AssociateAllocationController(IAssociateAllocationService associateAllocationService, IOrganizationService orgService,
            ILogger<AssociateAllocationController> logger)
        {
            m_AssociateAllocationService = associateAllocationService;
            m_OrgService = orgService;
            m_Logger = logger;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetAll Associate Allocations
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IList<AssociateAllocation>>> GetAll()
        {
            m_Logger.LogInformation("Retrieving records from AssociateAllocation table.");

            try
            {
                var associateAllocation = await m_AssociateAllocationService.GetAll();
                if (associateAllocation == null || associateAllocation.Items == null)
                {
                    m_Logger.LogInformation("No records found in AssociateAllocation table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { associateAllocation.Items.Count } associateAllocation.");
                    return Ok(associateAllocation.Items);
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
        public async Task<ActionResult<AssociateAllocation>> GetById(int id)
        {
            m_Logger.LogInformation($"Retrieving records from associateAllocation table by {id}.");

            try
            {
                var employee = await m_AssociateAllocationService.GetById(id);
                if (employee == null || employee.Item == null)
                {
                    m_Logger.LogInformation($"No records found for associateAllocationId {id}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for associateAllocationId {id}.");
                    return Ok(employee.Item);
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
        /// GetById
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetByEmployeeId/{employeeId}")]
        public async Task<ActionResult<List<AssociateAllocation>>> GetByEmployeeId(int employeeId)
        {
            m_Logger.LogInformation($"Retrieving records from associateAllocation table by {employeeId}.");

            try
            {
                var employee = await m_AssociateAllocationService.GetByEmployeeId(employeeId);
                if (employee == null)
                {
                    m_Logger.LogInformation($"No records found for associateAllocationId {employeeId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for associateAllocationId {employeeId}.");
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetAllAllocationByEmployeeId
        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetAllAllocationByEmployeeId/{employeeId}")]
        public async Task<IActionResult> GetAllAllocationByEmployeeId(int employeeId)
        {
            m_Logger.LogInformation($"Retrieving records from associateAllocation table by {employeeId}.");

            try
            {
                var employee = await m_AssociateAllocationService.GetAllAllocationByEmployeeId(employeeId);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found for GetAllAllocationByEmployeeId {employeeId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for GetAllAllocationByEmployeeId {employeeId}.");
                    return Ok(employee);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetByLeadId
        /// <summary>
        /// Get Associate Allocations By LeadId
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        [HttpGet("GetByLeadId/{leadId}")]
        public async Task<ActionResult<List<AssociateAllocation>>> GetByLeadId(int leadId)
        {
            m_Logger.LogInformation($"Retrieving records from associateAllocation table by {leadId}.");

            try
            {
                var employee = await m_AssociateAllocationService.GetByLeadId(leadId);
                if (employee == null)
                {
                    m_Logger.LogInformation($"No records found for associateAllocationId {leadId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for associateAllocationId {leadId}.");
                    return Ok(employee.Items);
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
        /// <param name="employeeIds"></param>
        /// <returns></returns>
        [HttpGet("GetAllocationsByEmpIds")]
        public async Task<ActionResult<AssociateAllocation>> GetAllocationsByEmpIds([FromQuery(Name = "employeeIds")] string employeeIds)
        {
            m_Logger.LogInformation($"Retrieving records from associateAllocation table by {employeeIds}.");

            try
            {
                var employees = await m_AssociateAllocationService.GetAllocationsByEmpIds(employeeIds);
                if (employees == null)
                {
                    m_Logger.LogInformation($"No records found for associateAllocationId {employeeIds}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for associateAllocationId {employeeIds}.");
                    return Ok(employees);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region AllocateAssociateToTalentPool
        /// <summary>
        /// Allocate employee to talent pool based on employee competency group
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("AllocateAssociateToTalentPool")]
        public async Task<ActionResult<bool>> AllocateAssociateToTalentPool(EmployeeDetails employee)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Inserting record into AssociateAllocation table");
            try
            {
                var certifications = await m_AssociateAllocationService.AllocateAssociateToTalentPool(employee);
                if (!certifications.IsSuccessful)
                {
                    m_Logger.LogError(certifications.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute AllocateAssociateToTalentPool() in AssociateAllocationController:" + stopwatch.Elapsed);

                    return NotFound(certifications.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully inserted record into AssociateAllocation table");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute AllocateAssociateToTalentPool() in AssociateAllocationController:" + stopwatch.Elapsed);

                    return Ok(certifications.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"AllocateAssociateToTalentPool()\" of AssociateAllocationController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute AllocateAssociateToTalentPool() in AssociateAllocationController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while creating allocation");
            }
        }
        #endregion

        #region GetEmployeesForAllocations
        /// <summary>
        /// GetEmployeesForAllocations
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmployeesForAllocations")]
        public async Task<IActionResult> GetEmployeesForAllocations()
        {
            m_Logger.LogInformation("Retrieving records from AssociateAllocation table.");

            try
            {
                var employees = await m_AssociateAllocationService.GetEmployeesForAllocations();
                if (employees == null || employees.Items == null)
                {
                    m_Logger.LogInformation("No records found in AssociateAllocation table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { employees.Items.Count } associateAllocation.");
                    return Ok(employees.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }


        }
        #endregion

        #region GetAssociatesForAllocation
        /// <summary>
        /// GetAssociatesForAllocation
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAssociatesForAllocation")]
        public async Task<IActionResult> GetAssociatesForAllocation()
        {
            m_Logger.LogInformation("Retrieving records from AssociateAllocation table.");

            try
            {
                var employees = await m_AssociateAllocationService.GetAssociatesForAllocation();
                if (!employees.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in AssociateAllocation table.");
                    return Ok(employees.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning { employees.Items.Count } associateAllocation.");
                    return Ok(employees.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetRolesByDepartmentId
        /// <summary>
        /// GetRolesByDepartmentId
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRolesByDepartmentId/{departmentId}")]
        public async Task<IActionResult> GetRolesByDepartmentId(int departmentId)
        {
            try
            {
                var roles = await m_OrgService.GetRoleMasterNames();
                if (roles == null || roles.Items == null)
                {
                    m_Logger.LogInformation("No records found in RoleMaster table.");
                    return NotFound();
                }
                else
                {
                    var rolesByDepartment = roles.Items.Where(x => x.DepartmentId == departmentId).ToList();
                    m_Logger.LogInformation($"Returning { roles.Items.Count } associateAllocation.");
                    return Ok(rolesByDepartment);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }


        }
        #endregion

        #region GetEmployeePrimaryAllocationProject
        /// <summary>
        /// Get EmployeePrimary AllocationProject
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        [HttpGet("GetEmployeePrimaryAllocationProject/{employeeId}")]
        public async Task<IActionResult> GetEmployeePrimaryAllocationProject(int employeeId)
        {
            try
            {
                var employeePrimaryProject = await m_AssociateAllocationService.GetEmployeePrimaryAllocationProject(employeeId);
                if (employeePrimaryProject == null || employeePrimaryProject.Item == null)
                {
                    m_Logger.LogInformation("No primary allocation found in associate allocation table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning primary allocation { employeePrimaryProject.Item } from associateAllocation table.");
                    return Ok(employeePrimaryProject.Item);
                }                
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }


        }
        #endregion

        #region GetEmpAllocationHistory
        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetEmpAllocationHistory/{employeeId}")]
        public async Task<IActionResult> GetEmpAllocationHistory(int employeeId)
        {
            m_Logger.LogInformation($"Retrieving records from associateAllocation table by {employeeId}.");

            try
            {
                var employee = await m_AssociateAllocationService.GetEmpAllocationHistory(employeeId);
                if (employee == null)
                {
                    m_Logger.LogInformation($"No records found for associateAllocationId {employeeId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for associateAllocationId {employeeId}.");
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region Allocate associate to Project
        /// <summary>
        ///Allocate a associate to project
        /// </summary>
        /// <param name="associateDetails"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(AssociateAllocationDetails allocationIn)
        {
            m_Logger.LogInformation("Inserting record in AssociateAllocation's table.");
            try
            {
                var response = await m_AssociateAllocationService.Create(allocationIn);

                m_Logger.LogInformation("Successfully created record in AssociateAllocation's table.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while AssociateAllocation: " + ex);

                return BadRequest("Error occurred while creating AssociateAllocation.");
            }
        }
        #endregion

        #region GetAssociatesToRelease
        /// <summary>
        /// Get Associates To Release from Allocated projects
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpGet("GetAssociatesToRelease/{employeeId}/{roleName}")]
        public async Task<IActionResult> GetAssociatesToRelease(int employeeId, string roleName)
        {
            try
            {
                var employee = await m_AssociateAllocationService.GetAssociatesToRelease(employeeId, roleName);
                if (employee == null)
                {
                    m_Logger.LogInformation($"No records found for Associates ToRelease.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found forAssociates ToRelease");
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region TemporaryReleaseAssociate
        /// <summary>
        /// Temporary Release Associate
        /// </summary>
        /// <param name="associateDetails"></param>
        /// <returns></returns>
        [HttpPost("TemporaryReleaseAssociate")]
        public async Task<IActionResult> TemporaryReleaseAssociate(AssociateAllocationDetails associateDetails)
        {
            try
            {
                var response = await m_AssociateAllocationService.TemporaryReleaseAssociate(associateDetails);

                m_Logger.LogInformation($"records found forAssociates ToRelease");
                return Ok(response);
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetAllocatedAssociates
        /// <summary>
        /// Get Associates from Allocated projects
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllocatedAssociates")]
        public async Task<IActionResult> GetAllocatedAssociates()
        {
            try
            {
                var employee = await m_AssociateAllocationService.GetAllocatedAssociates();
                if (employee == null)
                {
                    m_Logger.LogInformation($"No records found for Allocated Associates.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for Allocated Associates");
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region ReleaseOnExit
        /// <summary>
        /// Release Associate on Exit
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="releaseDate"></param>
        /// <returns></returns>
        [HttpPost("ReleaseOnExit/{employeeId}/{releaseDate}")]
        public async Task<IActionResult> ReleaseOnExit(int employeeId, string releaseDate)
        {
            try
            {
                var response = await m_AssociateAllocationService.ReleaseOnExit(employeeId, releaseDate);
                return Ok(response);
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetSkillSearchAssociateAllocations
        /// <summary>
        /// GetSkillSearchAssociateAllocations
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("GetSkillSearchAssociateAllocations")]
        public async Task<ActionResult<SkillSearchAssociateAllocation>> GetSkillSearchAssociateAllocations(string employeeIds)
        {
            try
            {
                var skillSearch = await m_AssociateAllocationService.GetSkillSearchAllocations(employeeIds);
                if (skillSearch.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(skillSearch.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region Update Associate Allocation
        /// <summary>
        ///Update Associate Allocation(Critical to non critical and vice versa)
        /// </summary>
        /// <param name="associateDetails"></param>
        /// <returns></returns>
        [HttpPost("UpdateAssociateAllocation")]
        public async Task<IActionResult> UpdateAssociateAllocation(AssociateAllocationDetails allocationIn)
        {
            m_Logger.LogInformation("making associate changes in AssociateAllocation's table.");
            try
            {
                var response = await m_AssociateAllocationService.UpdateAssociateAllocation(allocationIn);

                m_Logger.LogInformation("Successfully created record in AssociateAllocation's table.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while AssociateAllocation: " + ex);

                return BadRequest("Error occurred while doing allocation changes.");
            }
        }
        #endregion

        #region GetCurrentAllocationByEmpIdAndProjectId
        /// <summary>
        /// Get Current Allocation By EmpId And ProjectId
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("GetCurrentAllocationByEmpIdAndProjectId")]
        public async Task<IActionResult> GetCurrentAllocationByEmpIdAndProjectId(int employeeId, int projectId)
        {
            try
            {
                var employee = await m_AssociateAllocationService.GetCurrentAllocationByEmpIdAndProjectId(employeeId, projectId);
                if (employee == null)
                {
                    m_Logger.LogInformation($"No allocation found");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"returning associate allocation");
                    return Ok(employee.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region ReleaseFromTalentPool
        /// <summary>
        /// Release From TalentPool
        /// </summary>
        /// <param name="tpDetails"></param>
        /// <returns></returns>
        [HttpPost("ReleaseFromTalentPool")]
        public async Task<ActionResult<bool>> ReleaseFromTalentPool(TalentPoolDetails tpDetails)
        {

            m_Logger.LogInformation("releasing from talent pool in AssociateAllocation table");
            try
            {
                var response = await m_AssociateAllocationService.ReleaseFromTalentPool(tpDetails);
                if (!response.IsSuccessful)
                {
                    if (response.Message.StartsWith("Invalid date. Date should be greater than or equal to"))
                    {
                        m_Logger.LogError(response.Message);
                        return BadRequest(response.Message);
                    }
                    m_Logger.LogError("Error occured while releasing from talent pool");
                    return NotFound("Error occured while releasing from talent pool");
                }
                else
                {
                    m_Logger.LogInformation("Successfully released from Talent pool");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation("Error occured while releasing from talent pool");
                return BadRequest("Error occured while releasing from talent pool");
            }
        }
        #endregion

        #region UpdatePracticeAreaOfTalentPoolProject
        /// <summary>
        ///UpdatePracticeAreaOfTalentPoolProject
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdatePracticeAreaOfTalentPoolProject/{empID}/{competencyAreaId}")]
        public async Task<ActionResult<bool>> UpdatePracticeAreaOfTalentPoolProject(int empID, int competencyAreaId)
        {

            m_Logger.LogInformation("updating talent pool project in AssociateAllocation table");
            try
            {
                var response = await m_AssociateAllocationService.UpdatePracticeAreaOfTalentPoolProject(empID, competencyAreaId);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogError("Error occured while updating talent pool project in AssociateAllocation table");
                    return NotFound("Error occured while updating talent pool project in AssociateAllocation table");
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated talent pool project in AssociateAllocation table");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation("Error occured while updating talent pool project in AssociateAllocation table", ex.StackTrace);
                return BadRequest("Error occured while updating talent pool project in AssociateAllocation table");
            }
        }
        #endregion

        #region ReleaseFromAllocations
        /// <summary>
        ///ReleaseFromAllocations
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        [HttpPost("ReleaseFromAllocations/{EmployeeId}")]
        public async Task<ActionResult<bool>> ReleaseFromAllocations(int EmployeeId)
        {

            m_Logger.LogInformation("Releasing allocations from AssociateAllocation table");
            try
            {
                var response = await m_AssociateAllocationService.ReleaseFromAllocations(EmployeeId);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogError("Error occured while releasing allocation");
                    return NotFound("Error occured while releasing allocation");
                }
                else
                {
                    m_Logger.LogInformation("Successfully released allocation");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation("Error occured while releasing allocation", ex.StackTrace);
                return BadRequest("Error occured while releasing allocation");
            }
        }
        #endregion

        #region AddAssociateFutureProject
        [HttpPost("AddAssociateFutureProject")]
        public  async Task<IActionResult> AddAssociateFutureProject(AssociateFutureProjectAllocationDetails associateFutureProject)
        {
            m_Logger.LogInformation("Add future project details into AssociateFutureProjectAllocation table");
            try{
              var responce=await  m_AssociateAllocationService.AddAssociateFutureProject(associateFutureProject);
                if (responce.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully inserted data into AssociateFutureProjectAllocation table");
                    return Ok(responce);
                }
                else
                {
                    m_Logger.LogError("Error occured while adding data into AssociateFutureProjectAllocation table");
                    return BadRequest("Error occured while adding");
                }
                
            }
            catch(Exception ex)
            {
               m_Logger.LogError(ex.Message);
               m_Logger.LogError(ex.StackTrace);
                return BadRequest("Error occurred while adding AssociateFutureProjectAllocation");
            }

        }
        #endregion

        #region GetAssociateFutureProjectByEmpId
        [HttpGet("GetAssociateFutureProjectByEmpId/{employeeId}")]
        public async Task<IActionResult> GetAssociateFutureProjectByEmpId(int employeeId)
        {
            m_Logger.LogInformation("Get future project details from AssociateFutureProjectAllocation table");
            try
            {
                var responce = await m_AssociateAllocationService.GetAssociateFutureProjectByEmpId(employeeId);
                if (responce.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully fetched AssociateFutureProjectAllocation table");
                    return Ok(responce.Items);
                }
                else
                {
                    m_Logger.LogError("Error occured while fetching data from AssociateFutureProjectAllocation table");
                    return BadRequest("Error occured while fetching");
                }

            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                m_Logger.LogError(ex.StackTrace);
                return BadRequest("Error occurred while fetching AssociateFutureProjectAllocation");
            }

        }
        #endregion

        #region DiactivateAssociateFutureProjectByEmpId
        [HttpPost("DiactivateAssociateFutureProjectByEmpId/{employeeId}")]
        public async Task<IActionResult> DiactivateAssociateFutureProjectByEmpId(int employeeId)
        {
            m_Logger.LogInformation("Deactivating project details from AssociateFutureProjectAllocation table");
            try
            {
                var responce = await m_AssociateAllocationService.DiactivateAssociateFutureProjectByEmpId(employeeId);
                if (responce.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully deactivated AssociateFutureProjectAllocation table");
                    return Ok(responce);
                }
                else
                {
                    m_Logger.LogError("Error occured while deactivating data in AssociateFutureProjectAllocation table");
                    return BadRequest("Error occured while deactivating");
                }

            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                m_Logger.LogError(ex.StackTrace);
                return BadRequest("Error occurred while deactivating AssociateFutureProjectAllocation");
            }

        }
        #endregion       

        #region GetActiveAllocations
        /// <summary>
        /// Get Active Allocations 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="roleName"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpGet("GetActiveAllocations")]
        public async Task<IActionResult> GetActiveAllocations()
        {
            try
            {
                var employee = await m_AssociateAllocationService.GetActiveAllocations();
                if (employee == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found");
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetCompetencyAreaManagersDetails
        /// <summary>
        /// GetCompetencyAreaManagersDetails
        /// </summary>
        /// <param name="competencyAreaId"></param>        
        /// <returns></returns>
        [HttpGet("GetCompetencyAreaManagersDetails/{competencyAreaId}")]
        public async Task<IActionResult> GetCompetencyAreaManagersDetails(int competencyAreaId)
        {
            try
            {
                var employee = await m_AssociateAllocationService.GetCompetencyAreaManagersDetails(competencyAreaId);
                if (employee == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found");
                    return Ok(employee);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetAllAllocationDetails
        /// <summary>
        /// GetAllAllocationDetails
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllAllocationDetails")]
        public async Task<IActionResult> GetAllAllocationDetails()
        {
            try
            {
                var employee = await m_AssociateAllocationService.GetAllAllocationDetails();
                if (employee == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found");
                    return Ok(employee);
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
