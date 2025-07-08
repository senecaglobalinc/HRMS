using HRMS.Admin.Entities;
using HRMS.Admin.Types;
using HRMS.Admin.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace HRMS.Admin.API.Controllers
{
    [Route("admin/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentTypeController : ControllerBase
    {
        #region Global Variable

        private readonly IDepartmentTypeService departmentTypeService;
        private readonly ILogger<DepartmentTypeController> m_Logger;

        #endregion

        #region Constructor

        public DepartmentTypeController(IDepartmentTypeService n_departmentTypeService, ILogger<DepartmentTypeController> logger)
        {
            departmentTypeService = n_departmentTypeService;
            m_Logger = logger;
        } 

        #endregion

        #region create
        /// <summary>
        /// Create DepartmentType
        /// </summary>
        /// <param name="departmentTypeIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(DepartmentType departmentTypeIn)
        {
            m_Logger.LogInformation("Inserting record in DepartmentType table.");
            try
            {
                dynamic response = await departmentTypeService.Create(departmentTypeIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully created record in DepartmentType table.");
                    return Ok(response.DepartmentType);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating DepartmentType: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("DepartmentTypeDescription: " + departmentTypeIn.DepartmentTypeDescription);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating DepartmentType: " + ex);
                return BadRequest("Error occurred while creating DepartmentType");
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
            m_Logger.LogInformation("Retrieving records from DepartmentType table.");

            try
            {
                var departmentTypes = await departmentTypeService.GetAll(isActive);
                if (departmentTypes == null)
                {
                    m_Logger.LogInformation("No records found in DepartmentType table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { departmentTypes.Count} DepartmentType.");
                    return Ok(departmentTypes);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching DepartmentType.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update DepartmentType
        /// </summary>
        /// <param name="departmentTypeIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(DepartmentType departmentTypeIn)
        {
            m_Logger.LogInformation("Updating record in DepartmentType table.");
            try
            {
                dynamic response = await departmentTypeService.Update(departmentTypeIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully Updating record in DepartmentType table.");
                    return Ok(response.DepartmentType);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while Updating DepartmentType: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("DepartmentTypeDescription: " + departmentTypeIn.DepartmentTypeDescription);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while Updating DepartmentType: " + ex);
                return BadRequest("Error occurred while Updating DepartmentType");
            }
        }
        #endregion
    }
}