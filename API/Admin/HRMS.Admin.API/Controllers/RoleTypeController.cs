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
    public class RoleTypeController : ControllerBase
    {
        #region Global Variables
        private readonly IRoleTypeService m_RoleTypeService;
        private readonly ILogger<RoleTypeController> m_Logger;
        #endregion

        #region Constructor
        public RoleTypeController(IRoleTypeService roleTypeService,
            ILogger<RoleTypeController> logger)
        {
            m_RoleTypeService = roleTypeService;
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
        public async Task<ActionResult<IEnumerable>> GetAll(bool? isActive=true)
        {
            try
            {
                var roleTypes = await m_RoleTypeService.GetAll(isActive);
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
                var roleTypes = await m_RoleTypeService.GetById(gradeRoleTypeId);
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
        /// <returns>RoleType</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(RoleTypeModel model)
        {
            m_Logger.LogInformation("Inserting record in Role Type's table.");
            try
            {
                var response = await m_RoleTypeService.Create(model);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating Role Type: " + response.Message);

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
                m_Logger.LogError("RoleTypes name: " + model.RoleTypeName);
                return BadRequest("Error occurred while creating Role Type.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// This method updates Role Type details.
        /// </summary>
        /// <param name="RoleTypesMasterIn"></param>
        /// <returns>NotificationType</returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(RoleTypeModel model)
        {
            m_Logger.LogInformation("Updating record in Role Type's table.");

            try
            {
                var response = await m_RoleTypeService.Update(model);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating Role Type: " + response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in Role Type table.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating Role Type: " + ex);

                return BadRequest("Error occurred while updating Role Type.");
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// This methode deletes Role Type.
        /// </summary>
        /// <param name="RoleTypesMasterID"></param>
        /// <returns>bool</returns>
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int RoleTypesMasterID)
        {
            m_Logger.LogInformation("Deleting record in Role Type's table.");

            try
            {
                dynamic response = await m_RoleTypeService.Delete(RoleTypesMasterID);
                if (!response.IsSuccessful)
                {
                    //Extra information
                    m_Logger.LogError("RoleTypes ID: " + RoleTypesMasterID);

                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while deleting Role Type: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted record in Role Type's table.");
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("RoleTypes ID: " + RoleTypesMasterID);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while deleting Role Type: " + ex);

                return BadRequest("Error occurred while deleting Role Type.");
            }
        }
        #endregion

        #region GetRoleTypesForDropdown
        /// <summary>
        /// GetRoleTypesForDropdown
        /// </summary>        
        /// <returns>List<GenericType></returns>
        [HttpGet("GetRoleTypesForDropdown")]
        public async Task<ActionResult<IEnumerable>> GetRoleTypesForDropdown(int financialYearId=0, int departmentId=0)
        {
            try
            {
                var roleTypes = await m_RoleTypeService.GetRoleTypesForDropdown(financialYearId, departmentId);
                if (roleTypes == null)
                {
                    m_Logger.LogInformation("No records found in Role Type's table with " + financialYearId + " and " + departmentId);
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

        /// <summary>
        /// Gets Department info and Role Types for the passed in Department Id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpGet("GetRoleTypesAndDepartments/{departmentId}")]
        public async Task<ActionResult<IEnumerable>> GetRoleTypesAndDepartmentsAsync(int departmentId = 0)
        {
            m_Logger.LogInformation("Retrieving records from RoleType table.");

            try
            {
                var result = await m_RoleTypeService.GetRoleTypesAndDepartmentsAsync(departmentId);

                if (result.Count == 0)
                {
                    m_Logger.LogInformation("No records found in RoleType table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { result.Count} RoleTypes.");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
