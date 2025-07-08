using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Types;
using HRMS.Admin.API.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.Admin.API.Controllers
{
    [Route("admin/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        #region Global Variable
        private readonly IDepartmentService departmentService;
        private readonly ILogger<DepartmentController> m_Logger;
        #endregion

        #region Constructor
        public DepartmentController(IDepartmentService n_departmentService, ILogger<DepartmentController> logger)
        {
            departmentService = n_departmentService;
            m_Logger = logger;
        }
        #endregion

        #region create
        /// <summary>
        /// Create Department
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Department department)
        {
            m_Logger.LogInformation("Inserting record in Department table.");
            try
            {
                dynamic response = await departmentService.Create(department);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully created record in Department table.");
                    return Ok(response.Department);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating Department: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Description: " + department.Description);
                m_Logger.LogError("DepartmentCode: " + department.DepartmentCode);
                m_Logger.LogError("DepartmentHeadId: " + department.DepartmentHeadId);
                m_Logger.LogError("DepartmentTypeId: " + department.DepartmentTypeId);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating Department: " + ex);
                return BadRequest("Error occurred while creating Department");
            }
        }
        #endregion

        #region GetAll 
        /// <summary>
        /// GetAll DepartmentTypes
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from Department table.");

            try
            {
                var department = await departmentService.GetAll(isActive);
                if (department == null)
                {
                    m_Logger.LogInformation("No records found in Department table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { department.Count()} Department.");
                    return Ok(department);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Department.");
            }
        }
        #endregion

        #region GetById 
        /// <summary>
        /// Get Department By Id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpGet("GetById/{departmentId}")]
        public async Task<ActionResult<Department>> GetById(int departmentId)
        {
            m_Logger.LogInformation("Retrieving records from Department table.");

            try
            {
                var department = await departmentService.GetById(departmentId);
                if (department == null)
                {
                    m_Logger.LogInformation("No records found in Department table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning  Department.");
                    return Ok(department);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Department.");
            }
        }
        #endregion

        #region GetUserDepartmentDetails
        /// <summary>
        /// Get UserDepartment Details
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserDepartmentDetails")]
        public async Task<IActionResult> GetUserDepartmentDetails()
        {
            try
            {
                var departments = await departmentService.GetUserDepartmentDetails();
                if (departments == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(departments.Items);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region GetUserDepartmentDetailsByEmployeeID
        /// <summary>
        /// Get UserDepartmentDetails By EmployeeID
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetUserDepartmentDetailsByEmployeeID/{employeeId}")]
        public async Task<IActionResult> GetUserDepartmentDetailsByEmployeeID(int employeeId)
        {
            try
            {
                var departments = await departmentService.GetUserDepartmentDetailsByEmployeeID(employeeId);
                if (departments == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(departments.Items);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion       

        #region Update
        /// <summary>
        /// Update Department
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(Department department)
        {
            m_Logger.LogInformation("Updating record in Department table.");
            try
            {
                dynamic response = await departmentService.Update(department);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully updated record in Department table.");
                    return Ok(response.Department);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while Updating Department: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Description: " + department.Description);
                m_Logger.LogError("DepartmentCode: " + department.DepartmentCode);
                m_Logger.LogError("DepartmentHeadId: " + department.DepartmentHeadId);
                m_Logger.LogError("DepartmentTypeId: " + department.DepartmentTypeId);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while Updating Department: " + ex);
                return BadRequest("Error occurred while Updating Department");
            }
        }
        #endregion

        #region GetByDepartmentCode
        /// <summary>
        /// GetByDepartmentCode
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <returns></returns>
        [HttpGet("GetByDepartmentCode/{departmentCode}")]
        public async Task<ActionResult<Department>> GetByDepartmentCode(string departmentCode)
        {
            m_Logger.LogInformation($"Retrieving records from department table by {departmentCode}.");

            try
            {
                var department = await departmentService.GetByDepartmentCode(departmentCode);
                if (department == null)
                {
                    m_Logger.LogInformation($"No records found for departmentCode {departmentCode}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for departmentCode {departmentCode}.");
                    return Ok(department);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetByDepartmentCodes
        /// <summary>
        /// GetByDepartmentCodes
        /// </summary>
        /// <param name="departmentCodes"></param>
        /// <returns></returns>
        [HttpGet("GetByDepartmentCodes")]
        public async Task<ActionResult<Department>> GetByDepartmentCodes([FromQuery(Name = "departmentCodes")] string departmentCodes)
        {
            m_Logger.LogInformation($"Retrieving records from department table by {departmentCodes}.");

            try
            {
                var department = await departmentService.GetByDepartmentCodes(departmentCodes);
                if (department == null)
                {
                    m_Logger.LogInformation($"No records found for departmentCodes {departmentCodes}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for departmentCodes {departmentCodes}.");
                    return Ok(department);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetMasterTablesData
        /// <summary>
        /// Get GetMasterTablesData
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetMasterTablesData")]
        public async Task<ActionResult<MasterDetails>> GetMasterTablesData()
        {
            try
            {
                var data = await departmentService.GetMasterTablesData();
                if (data.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(data.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetDepartmentDLByDeptId 
        /// <summary>
        /// Get DepartmentDL By DeptId
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [HttpGet("GetDepartmentDLByDeptId/{deptId}")]
        public async Task<ActionResult<DepartmentDL>> GetDepartmentDLByDeptId(int deptId)
        {
            m_Logger.LogInformation("Retrieving records from DepartmentDL table.");

            try
            {
                var departmentdl = await departmentService.GetDepartmentDLByDeptId(deptId);
                if (departmentdl == null)
                {
                    m_Logger.LogInformation("No records found in DepartmentDL table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning  DepartmentDLs.");
                    return Ok(departmentdl);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching DepartmentDL.");
            }
        }
        #endregion

        #region GetAllDepartmentDLs
        /// <summary>
        /// Get DepartmentDL By DeptId
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet("GetAllDepartmentsWithDLs")]
        public async Task<ActionResult<DepartmentDL>> GetAllDepartmentsWithDLs()
        {
            m_Logger.LogInformation("Getting all records from DepartmentDL");

            try
            {
                var departmentdl = await departmentService.GetAllDepartmentDLs();
                if (departmentdl == null)
                {
                    m_Logger.LogInformation("No records found in DepartmentDL table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning all DepartmentDLs.");
                    return Ok(departmentdl);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching DepartmentDL list.");
            }
        }
        #endregion
    }
}