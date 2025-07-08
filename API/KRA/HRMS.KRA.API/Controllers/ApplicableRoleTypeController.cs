using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Infrastructure.Models.Request;
using HRMS.KRA.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace HRMS.KRA.API.Controllers
{
    [Route("kra/api/v1/[controller]")]
    [ApiController]
    public class ApplicableRoleTypeController : ControllerBase
    {
        #region Global Variables

        private readonly IApplicableRoleTypeService m_ApplicableRoleTypeService;
        private readonly ILogger<ApplicableRoleTypeController> m_Logger;

        #endregion

        #region Constructor
        public ApplicableRoleTypeController(IApplicableRoleTypeService ApplicableRoleTypeService, 
            ILogger<ApplicableRoleTypeController> logger)
        {
            m_ApplicableRoleTypeService = ApplicableRoleTypeService;
            m_Logger = logger;
        }
        #endregion      

        #region GetAll
        /// <summary>
        /// Gets list of ApplicableRoleType.
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(int? FinancialYearId, int? DepartmentId, int? GradeRoleTypeId, int? StatusId, int? gradeId)
        {
            m_Logger.LogInformation("Retrieving records from ApplicableRoleType table.");

            try
            {
                var applicableRoleTypes = await m_ApplicableRoleTypeService.GetAll(FinancialYearId, DepartmentId, GradeRoleTypeId, StatusId, gradeId);
                if (applicableRoleTypes == null)
                {
                    m_Logger.LogInformation("No records found in ApplicableRoleType table.");
                    return NotFound(applicableRoleTypes.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning ApplicableRoleTypes.");
                    return Ok(applicableRoleTypes.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured while getting Get ApplicableRoleTypes" + ex.StackTrace);
                return BadRequest("Error Occured while getting Get ApplicableRoleTypes");
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// Create ApplicableRoleType
        /// </summary>
        /// <param name="ApplicableRoleTypeModel"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ApplicableRoleTypeRequest model)
        {
            m_Logger.LogInformation("Inserting record in ApplicableRoleType table.");
            try
            {
                var response = await m_ApplicableRoleTypeService.Create(model);

                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating ApplicableRoleType: " + response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in ApplicableRoleType table.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating ApplicableRoleType: " + ex);

                return BadRequest("Error occurred while creating ApplicableRoleType.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update a ApplicableRoleType
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(ApplicableRoleTypeRequest model)
        {
            m_Logger.LogInformation("Updating record.");
            try
            {
                var response = await m_ApplicableRoleTypeService.Update(model);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating record : " + response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully update record.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating record : " + ex.Message);

                return BadRequest("Error occurred while updating record.");
            }
        }
        #endregion

        #region Update Role Type Status
        /// <summary>
        /// Update roletype status
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UpdateRoleTypeStatus")]
        public async Task<IActionResult> UpdateRoleTypeStatus(ApplicableRoleTypeRequest request)
        {
            m_Logger.LogInformation("Updating record.");
            try
            {
                var response = await m_ApplicableRoleTypeService.UpdateRoleTypeStatus(request);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating record : " + response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully update record.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating record : " + ex.Message);

                return BadRequest("Error occurred while updating record.");
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Measurement Type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int applicableRoleTypeId)
        {
            m_Logger.LogInformation("Deleting record.");
            try
            {
                var response = await m_ApplicableRoleTypeService.Delete(applicableRoleTypeId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while deleting record : " + response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted record.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while deleting record: " + ex);

                return BadRequest("Error occurred while deleting record.");
            }
        }

        #endregion
    }
}
