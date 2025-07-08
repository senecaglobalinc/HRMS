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
    public class UserController : Controller
    {

        #region Global Variables

        private readonly IUserService m_userService;
        private readonly ILogger<UserController> m_Logger;
        #endregion

        #region Constructor
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            m_userService = userService;
            m_Logger = logger;
        }
        #endregion

        #region GetUsers
        /// <summary>
        /// Get users with employee table join
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUsers")]
        public async Task<ActionResult<IEnumerable>> GetUsers()
        {
            m_Logger.LogInformation("Retrieving records from Users table.");

            try
            {
                var users = await m_userService.GetUsers();
                if (users == null)
                {
                    m_Logger.LogInformation("No records found in users table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning  users.");
                    return Ok(users);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                throw;
            }

        }
        #endregion

        #region GetUsersBySearchString
        /// <summary>
        /// Get users by search string 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUsersBySearchString/{searchstring}")]
        public async Task<ActionResult<IEnumerable>> GetUsersBySearchString(string searchstring)
        {
            m_Logger.LogInformation("Retrieving records from Users table.");

            try
            {
                var users = await m_userService.GetUsersBySearchString(searchstring);
                if (users == null)
                {
                    m_Logger.LogInformation("No records found in users table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning  users.");
                    return Ok(users);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                throw;
            }

        }
        #endregion

        #region GetAllUsers
        /// <summary>
        /// GetAllUsers
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable>> GetAllUsers(bool? isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from users table.");

            try
            {
                var users = await m_userService.GetAllUsers(isActive);
                if (users == null)
                {
                    m_Logger.LogInformation("No records found in users table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { users.Count} users.");
                    return Ok(users);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while getting users: " + ex);
                return BadRequest("Error occurred while getting users.");
            }
        }
        #endregion

        #region GetUserById
        /// <summary>
        /// GetAllUsers
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetByUserId")]
        public async Task<ActionResult> GetByUserId(int userId)
        {
            m_Logger.LogInformation("Retrieving records from users table.");

            try
            {
                var user = await m_userService.GetByUserId(userId);
                if (user == null)
                {
                    m_Logger.LogInformation("No records found in users table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning user:" + userId + ".");
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while getting users: " + ex);
                return BadRequest("Error occurred while getting users.");
            }
        }
        #endregion
        
        #region GetById
        /// <summary>
        /// GetUserById
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("GetById/{userId}")]
        public async Task<ActionResult<User>> GetById(int userId)
        {
            m_Logger.LogInformation($"Retrieving records from user table by {userId}.");

            try
            {
                var user = await m_userService.GetById(userId);
                if (user == null)
                {
                    m_Logger.LogInformation($"No records found for userId {userId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for userId {userId}.");
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetActiveUserByUserId
        /// <summary>
        /// GetUserById
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("GetActiveUserByUserId/{userId}")]
        public async Task<ActionResult<User>> GetActiveUserByUserId(int userId)
        {
            m_Logger.LogInformation($"Retrieving records from user table by {userId}.");

            try
            {
                var user = await m_userService.GetActiveUserByUserId(userId);
                if (user == null)
                {
                    m_Logger.LogInformation($"No records found for userId {userId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for userId {userId}.");
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetUserByEmail
        /// <summary>
        /// GetAllUsers
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetByEmail")]
        public async Task<ActionResult> GetByEmail(string email)
        {
            m_Logger.LogInformation("Retrieving records from users table.");

            try
            {
                var user = await m_userService.GetUserByEmail(email);
                if (user == null)
                {
                    m_Logger.LogInformation("No records found in users table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning user for the emailid:" + email + ".");
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while getting users: " + ex);
                return BadRequest("Error occurred while getting users.");
            }
        }
        #endregion

        #region UpdateUser
        /// <summary>
        /// UpdateUser
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("UpdateUser/{userId}")]
        public async Task<ActionResult> UpdateUser(int userId)
        {
            m_Logger.LogInformation("Updating users table.");

            try
            {
                var user = await m_userService.UpdateUser(userId);
                if (!user)
                {
                    m_Logger.LogInformation("Update Failed in users table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"User updated successfully:");
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating user: " + ex);
                return BadRequest("Error occurred while updating user.");
            }
        }
        #endregion

        #region GetUsersByRoles
        /// <summary>
        /// GetUsersByRoles
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpGet("GetUsersByRoles/{roles}")]
        public async Task<ActionResult<UserRoleDetails>> GetUsersByRoles(string roles)
        {
            m_Logger.LogInformation($"Retrieving records from UserRoles table by {roles}.");

            try
            {
                var userIds = await m_userService.GetUsersByRoles(roles);
                if (userIds == null)
                {
                    m_Logger.LogInformation($"No records found for roles {roles}");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for roles {roles}");
                    return Ok(userIds);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion


        #region GetUserEmail
        /// <summary>
        /// Get logged-In User EmailId 
        /// </summary>
       
        /// <returns></returns>
        [HttpGet("GetUserEmail")]
        public async Task<ActionResult> GetUserEmail()
        {
            try
            {
                string userEmail = m_userService.GetUserEmail();
                if (userEmail == null || userEmail=="")
                {
                    m_Logger.LogInformation($"No user email found ");
                    return NotFound();
                }
                else
                {
                    return Ok(userEmail);
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