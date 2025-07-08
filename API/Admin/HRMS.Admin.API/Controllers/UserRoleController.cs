using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Types;
using HRMS.Admin.API.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HRMS.Admin.API.Controllers
{
    [Route("admin/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class UserRoleController : Controller
    {
        #region Global Variables

        private readonly IUserRoleService m_userRoleService;
        private readonly ILogger<UserRoleController> m_Logger;
        #endregion

        #region Constructor
        public UserRoleController(IUserRoleService userRoleService, ILogger<UserRoleController> logger)
        {
            m_userRoleService = userRoleService;
            m_Logger = logger;
        }
        #endregion

        #region GetProgramManagers
        /// <summary>
        /// GetProgramManagers
        /// </summary>
        /// <param name="userRole"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetProgramManagers")]
        public async Task<ActionResult<IEnumerable>> GetProgramManagers(string userRole, int employeeId)
        {
            m_Logger.LogInformation("Retrieving records for program manager.");

            try
            {
                var programManagers = await m_userRoleService.GetProgramManagers(userRole, employeeId);
                if (programManagers == null)
                {
                    m_Logger.LogInformation("No records found for program manager.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning program managers.");
                    return Ok(programManagers);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetRoles
        /// <summary>
        /// GetRoles
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetRoles")]
        public async Task<ActionResult<IEnumerable>> GetRoles(bool? isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from  table.");

            try
            {
                var roles = await m_userRoleService.GetRoles(isActive);
                if (roles == null)
                {
                    m_Logger.LogInformation("No records found in roles table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning  roles.");
                    return Ok(roles);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetUserRolesbyUserID
        /// <summary>
        /// GetUserRolesbyUserID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet("GetUserRolesbyUserID/{userID}")]
        public async Task<ActionResult<UserRole>> GetUserRolesbyUserID(int userID)
        {
            m_Logger.LogInformation($"Retrieving records from UserRoles table by {userID}.");

            try
            {
                var UserRole = await m_userRoleService.GetUserRolesbyUserID(userID);
                if (UserRole == null)
                {
                    m_Logger.LogInformation($"No records found for userID {userID}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for userID {userID}.");
                    return Ok(UserRole);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion      

        #region UpdateUserRole
        /// <summary>
        /// UpdateUserRole
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
        [HttpPost("UpdateUserRole")]
        public async Task<ActionResult<UserRole>> UpdateUserRole(UserRole userRole)
        {

            m_Logger.LogInformation("Updating record in UserRole table.");

            try
            {
                dynamic response = await m_userRoleService.UpdateUserRole(userRole);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating userrole: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in UserRole's table.");
                    return Ok(response.UserRole);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating UserRole: " + ex);
                return BadRequest("Error occurred while updating UserRole.");
            }
        }
        #endregion      

        #region GetUserRoleOnLogin
        /// <summary>
        /// GetUserRoleOnLogin
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("GetUserRoleOnLogin/{userName}")]
        public async Task<ActionResult<IEnumerable>> GetUserRoleOnLogin(string userName)
        {
            m_Logger.LogInformation($"Retrieving records from Roles table by {userName}.");

            try
            {
                var roles = await m_userRoleService.GetUserRoleOnLogin(userName);
                if (roles == null)
                {
                    m_Logger.LogInformation($"No records found for username {userName}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for username {userName}.");
                    return Ok(roles);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetUserRoles
        /// <summary>
        /// GetUserRoles
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetUserRoles")]
        public async Task<ActionResult<IEnumerable>> GetUserRoles()
        {
            m_Logger.LogInformation($"Retrieving records from Roles table by");

            try
            {
                var roles = await m_userRoleService.GetUserRoles();
                if (roles == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(roles);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetAllRoles
        /// <summary>
        /// GetAllRoles
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetAllRoles")]
        public async Task<ActionResult<IEnumerable>> GetAllRoles(bool? isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from roles table.");

            try
            {
                var roles = await m_userRoleService.GetAllRoles(isActive);
                if (roles == null)
                {
                    m_Logger.LogInformation("No records found in roles table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { roles.Count} roles.");
                    return Ok(roles);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while getting roles: " + ex);
                return BadRequest("Error occurred while getting roles.");
            }
        }
        #endregion

        #region GetAllUserRoles
        /// <summary>
        /// GetAllUserRoles
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetAllUserRoles")]
        public async Task<ActionResult<IEnumerable>> GetAllUserRoles(bool? isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from userRoles table.");

            try
            {
                var userRoles = await m_userRoleService.GetAllUserRoles(isActive);
                if (userRoles == null)
                {
                    m_Logger.LogInformation("No records found in userRoles table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { userRoles.Count} userRoles.");
                    return Ok(userRoles);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while getting userRoles: " + ex);
                return BadRequest("Error occurred while getting userRoles.");
            }
        }
        #endregion

        #region GetHRAAdvisors
        /// <summary>
        /// GetHRAAdvisors
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHRAAdvisors")]
        public async Task<ActionResult<IEnumerable>> GetHRAAdvisors()
        {
            m_Logger.LogInformation("Retrieving HRA Advisors.");

            try
            {
                var hraAdvisors = await m_userRoleService.GetHRAAdvisors();
                if (hraAdvisors == null)
                {
                    m_Logger.LogInformation("No HRAAdvisors found");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning HRA Advisors.");
                    return Ok(hraAdvisors);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetRoleByRoleName
        /// <summary>
        /// Get the Role by role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpGet("GetRoleByRoleName/{roleName}")]
        public async Task<ActionResult<Role>> GetRoleByRoleName(string roleName)
        {
            m_Logger.LogInformation($"Retrieving records from Role table by {roleName}.");

            try
            {
                var role = await m_userRoleService.GetRoleByRoleName(roleName);
                if (role == null)
                {
                    m_Logger.LogInformation($"No records found for roleName {roleName}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for roleName {roleName}.");
                    return Ok(role);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetUserDetailsByUserName
        /// <summary>
        /// Get the Role by role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpGet("GetUserDetailsByUserName/{userName}")]
        public async Task<ActionResult<UserLogin>> GetUserDetailsByUserName(string userName)
        {
            m_Logger.LogInformation($"Retrieving records from Role table by {userName}.");

            try
            {
                var userDetails = await m_userRoleService.GetUserDetailsByUserName(userName);
                if (userDetails.IsSuccessful == false)
                {
                    m_Logger.LogInformation($"No records found for roleName {userName}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for roleName {userName}.");
                    return Ok(userDetails.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region RemoveUserRoleOnExit
        /// <summary>
        /// RemoveUserRoleOnExit
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("RemoveUserRoleOnExit/{userId}")]
        public async Task<IActionResult> RemoveUserRoleOnExit(int userId)
        {
            m_Logger.LogInformation("Updating record in UserRole table.");

            try
            {
                var response = await m_userRoleService.RemoveUserRoleOnExit(userId);
                if (!response)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating userrole: " + userId);
                    return BadRequest(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in UserRole's table.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating UserRole: " + ex);
                return BadRequest("Error occurred while updating UserRole.");
            }
        }
        #endregion
    }
}