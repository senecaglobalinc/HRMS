using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Admin.Entities;
using HRMS.Admin.Types;
using HRMS.Admin.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.Admin.API.Controllers
{
    [Route("admin/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class FunctionalRoleController : ControllerBase
    {
        #region Global Variable
        private readonly IRoleService roleService;
        private readonly ILogger<FunctionalRoleController> m_Logger;
        #endregion

        #region Constructor
        public FunctionalRoleController(IRoleService n_roleService, ILogger<FunctionalRoleController> logger)
        {
            roleService = n_roleService;
            m_Logger = logger;
        }
        #endregion

        #region create
        /// <summary>
        /// Create FuctionalRole
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(RoleMaster roleMaster)
        {
            m_Logger.LogInformation("Inserting record in Department table.");
            try
            {
                dynamic response = await roleService.Create(roleMaster);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully created record in RoleMaster table.");
                    return Ok(response.RoleMaster);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating functional role: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("RoleDescription: " + roleMaster.RoleDescription);
                m_Logger.LogError("SGRoleID: " + roleMaster.SGRoleID);
                m_Logger.LogError("PrefixID: " + roleMaster.PrefixID);
                m_Logger.LogError("SuffixID: " + roleMaster.SuffixID);
                m_Logger.LogError("DepartmentId: " + roleMaster.DepartmentId);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating Functional role: " + ex);
                return BadRequest("Error occurred while creating Functional role");
            }
        }
        #endregion

        #region Get All Roles 
        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            m_Logger.LogInformation("Retrieving records from RoleMaster table.");

            try
            {
                var roles = await roleService.GetAll();
                if (roles == null)
                {
                    m_Logger.LogInformation("No records found in RoleMaster table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { roles.Items.Count} RoleMaster.");
                    return Ok(roles.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching from RoleMaster.");
            }
        }
        #endregion

        #region GetRoleNames 
        /// <summary>
        /// Get Roles by Role id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("GetRoleNames")]
        public async Task<IActionResult> GetRoleNames()
        {
            m_Logger.LogInformation("Retrieving records from RoleMaster table.");

            try
            {
                var roles = await roleService.GetRoleNames();
                if (roles == null)
                {
                    m_Logger.LogInformation("No records found in RoleMaster table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { roles.Items.Count} RoleMaster.");
                    return Ok(roles.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching from RoleMaster.");
            }
        }
        #endregion

        #region GetByDepartmentID 
        /// <summary>
        /// Get all roles by DepartmentID
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetByDepartmentID")]
        public async Task<ActionResult<IEnumerable>> GetByDepartmentID(int departmentId)
        {
            m_Logger.LogInformation("Retrieving records from RoleMaster table.");

            try
            {
                var roles = await roleService.GetByDepartmentID(departmentId);
                if (roles == null)
                {
                    m_Logger.LogInformation("No records found in RoleMaster table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { roles.Count} RoleMaster.");
                    return Ok(roles);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching from RoleMaster.");
            }
        }
        #endregion

        #region GetSGRoleSuffixAndPrefix 
        /// <summary>
        /// Get all records from SGRoles, SGRolePrefix, SGRoleSuffix tables.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSGRoleSuffixAndPrefix")]
        public async Task<ActionResult<IEnumerable>> GetSGRoleSuffixAndPrefix(int departmentId)
        {
            m_Logger.LogInformation("Retrieving records from SGRoles, SGRolePrefix, SGRoleSuffix tables.");

            try
            {
                var roles = await roleService.GetSGRoleSuffixAndPrefix(departmentId);
                if (roles == null)
                {
                    m_Logger.LogInformation("No records found in tables.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning SGRoles, SGRolePrefix, SGRoleSuffix data.");
                    return Ok(roles);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching.");
            }
        }
        #endregion

        #region update
        /// <summary>
        /// Update FuctionalRole
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(RoleMaster roleMaster)
        {
            m_Logger.LogInformation("Inserting record in Department table.");
            try
            {
                dynamic response = await roleService.Update(roleMaster);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully updated record in RoleMaster table.");
                    return Ok(response.RoleMaster);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating functional role: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("RoleDescription: " + roleMaster.RoleDescription);
                m_Logger.LogError("SGRoleID: " + roleMaster.SGRoleID);
                m_Logger.LogError("PrefixID: " + roleMaster.PrefixID);
                m_Logger.LogError("SuffixID: " + roleMaster.SuffixID);
                m_Logger.LogError("DepartmentId: " + roleMaster.DepartmentId);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating Functional role: " + ex);
                return BadRequest("Error occurred while updating Functional role");
            }
        }
        #endregion

        #region GetRolesAndDepartments
        /// <summary>
        /// Get all roles and mapped departments
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRolesAndDepartments")]
        public async Task<ActionResult<IEnumerable>> GetRolesAndDepartments()
        {
            m_Logger.LogInformation("Retrieving records from roles and departments table.");



            try
            {
                var rolesDepartmentsLists = await roleService.GetRolesAndDepartments();
                if (rolesDepartmentsLists == null)
                {
                    m_Logger.LogInformation("No join records found for roles and departments table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { rolesDepartmentsLists.Count} from roles and departments table.");
                    return Ok(rolesDepartmentsLists);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching from roles and departments table.");
            }
        }
        #endregion
    }
}