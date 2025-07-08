using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.KRA.Types;
using HRMS.KRA.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.KRA.API.Controllers
{
    [Authorize]
    [Route("kra/api/v1/[controller]")]
    [ApiController]
    public class RoleTypeController : Controller
    {
        #region Global Variables

        private readonly IRoleTypeService m_RoleTypeService;
        private readonly ILogger<RoleTypeController> m_Logger;

        #endregion

        #region Constructor
        public RoleTypeController(IRoleTypeService roleTypeService, ILogger<RoleTypeController> logger)
        {
            m_RoleTypeService = roleTypeService;
            m_Logger = logger;
        }
        #endregion

        #region GetRoleTypesByGradeId
        /// <summary>
        /// Gets RoleTypes By GradeId
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        [HttpGet("GetRoleTypesByGradeId/{gradeId}")]
        public async Task<IActionResult> GetRoleTypesByGradeId(int gradeId)
        {
            m_Logger.LogInformation("Retrieving records from RoleType table by {gradeId}.");

            try
            {
                var roleTypes = await m_RoleTypeService.GetRoleTypesByGradeIdAsync(gradeId);
                if (roleTypes == null)
                {
                    m_Logger.LogInformation("No records found");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning RoleType Details.");
                    return Ok(roleTypes);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching RoleTypes.");
            }
        }
        #endregion
    }
}
