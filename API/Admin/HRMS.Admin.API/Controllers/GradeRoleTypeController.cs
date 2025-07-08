using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Admin.Infrastructure.Models;
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
    public class GradeRoleTypeController : ControllerBase
    {
        #region Global Variables
        private readonly IGradeRoleTypeService m_GradeRoleTypeService;
        private readonly ILogger<GradeRoleTypeController> m_Logger;
        #endregion

        #region Constructor
        public GradeRoleTypeController(IGradeRoleTypeService gradeRoleTypeService,
            ILogger<GradeRoleTypeController> logger)
        {
            m_GradeRoleTypeService = gradeRoleTypeService;
            m_Logger = logger;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets Role Type
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns>List<RoleTypesModel></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool? isActive)
        {
            try
            {
                var roleTypes = await m_GradeRoleTypeService.GetAll(isActive);
                if (roleTypes == null)
                {
                    m_Logger.LogInformation("No records found in Role Type's table.");
                    return NotFound();
                }
                else
                {
                    return Ok(roleTypes);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Role Type's.");
            }
        }
        #endregion

        #region GetById
        /// <summary>
        /// Gets Grade Role Type
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns>GradeRoleType</returns>
        [HttpGet("GetById")]
        public async Task<ActionResult<IEnumerable>> GetById(int gradeRoleTypeId)
        {
            try
            {
                var roleTypes = await m_GradeRoleTypeService.GetById(gradeRoleTypeId);
                if (roleTypes == null)
                {
                    m_Logger.LogInformation("No records found in Grade Role Types.");
                    return NotFound();
                }
                else
                {
                    return Ok(roleTypes);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Grade Role Types.");
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// This method creates new Role Type.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>GradeRoleType</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(GradeRoleTypeRequest model)
        {
            m_Logger.LogInformation("Inserting record in Role Type's table.");
            try
            {
                dynamic response = await m_GradeRoleTypeService.Create(model);
                if (!response.IsSuccessful)
                {   
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating Role Type: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in Role Type's table.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("RoleTypes: " + model.RoleTypeId.ToString());
                return BadRequest("Error occurred while creating Role Type.");
            }
        }
        #endregion

        #region GetGradesBySearchFilters
        /// <summary>
        /// Gets the Grade details by Financial Year, Department and RoleTypeId(optional)
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetGradesByDepartment")]
        public async Task<ActionResult<IEnumerable>> GetGradesByDepartment(int financialYearId, int departmentId, int roleTypeId)
        {
            m_Logger.LogInformation("Retrieving records from gradeRoleTypes table.");

            try
            {
                var roleTypes = await m_GradeRoleTypeService.GetGradesBySearchFilters(financialYearId, departmentId, roleTypeId);
                if (roleTypes == null)
                {
                    m_Logger.LogInformation("No records found in gradeRoleTypes table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { roleTypes.Count} gradeRoleTypes.");
                    return Ok(roleTypes);
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
