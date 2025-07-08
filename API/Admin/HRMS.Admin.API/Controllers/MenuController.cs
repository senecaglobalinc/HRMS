using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Types;
using HRMS.Admin.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HRMS.Admin.API.Controllers
{
    [Route("admin/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class MenuController : ControllerBase
    {
        #region Global Variables
        private readonly IMenuService m_MenuService;
        private readonly ILogger<MenuController> m_Logger;
        #endregion

        #region Constructor
        public MenuController(IMenuService menuService, ILogger<MenuController> logger)
        {
            m_MenuService = menuService;
            m_Logger = logger;
        }
        #endregion

        #region GetSourceMenuRoles
        /// <summary>
        /// Get Menu Details by logged in user role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpGet("GetMenuDetails/{roleName}")]
        public async Task<IActionResult> GetMenuDetails(string roleName)
        {
            try
            {
                var menuRoles = await m_MenuService.GetMenuDetails(roleName);
                return Ok(menuRoles.Items);
            }
            catch (Exception ex)
            {
                m_Logger.LogError("There is an error getting GetMenuDetails" + ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetSourceMenuRoles
        /// <summary>
        /// GetSourceMenuRoles
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpGet("GetSourceMenuRoles/{roleId}")]
        public async Task<IActionResult> GetSourceMenuRoles(int roleId)
        {
            try
            {
                var menuRoles = await m_MenuService.GetSourceMenuRoles(roleId);
                return Ok(menuRoles.Items);
            }
            catch (Exception ex)
            {
                m_Logger.LogError("There is an error getting GetSourceMenuRoles" + ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetTargetMenuRoles
        /// <summary>
        /// GetTargetMenuRoles
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpGet("GetTargetMenuRoles/{roleId}")]
        public async Task<IActionResult> GetTargetMenuRoles(int roleId)
        {
            try
            {
                var menuRoles = await m_MenuService.GetTargetMenuRoles(roleId);
                return Ok(menuRoles.Items);
            }
            catch (Exception ex)
            {
                m_Logger.LogError("There is an error getting GetTargetMenuRoles" + ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region AddTargetMenuRoles
        /// <summary>
        /// AddTargetMenuRoles
        /// </summary>
        /// <param name="menuRoles"></param>
        /// <returns></returns>
        [HttpPost("AddTargetMenuRoles")]
        public async Task<IActionResult> AddTargetMenuRoles(MenuRoleDetails menuRoles)
        {
            try
            {
                var response = await m_MenuService.AddTargetMenuRoles(menuRoles);
                return Ok(response.IsSuccessful);
            }
            catch (Exception ex)
            {
                m_Logger.LogError("There is an error getting AddTargetMenuRoles" + ex.Message);
                return BadRequest();
            }
        }
        #endregion
    }
}
