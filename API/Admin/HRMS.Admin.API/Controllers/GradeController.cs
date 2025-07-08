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
    [ApiController]
    [Route("admin/api/v1/[controller]")]
    [Authorize]
    public class GradeController : Controller
    {
        #region Global Variables

        private readonly IGradeService m_GradeService;
        private readonly ILogger<GradeController> m_Logger;

        #endregion

        #region Constructor

        public GradeController(IGradeService gradeService, ILogger<GradeController> logger)
        {
            m_GradeService = gradeService;
            m_Logger = logger;
        }

        #endregion

        #region Create
        /// <summary>
        /// Create Grade
        /// </summary>
        /// <param name="gradeIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Grade gradeIn)
        {
            m_Logger.LogInformation("Inserting record in grades table.");
            try
            {
                dynamic response = await m_GradeService.Create(gradeIn);

                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating grade: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in grade's table.");
                    return Ok(response.Grade);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetGradesDetails
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool? isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from grades table.");

            try
            {
                var grades = await m_GradeService.GetAll(isActive);
                if (grades == null)
                {
                    m_Logger.LogInformation("No records found in Designations table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { grades.Count} designations.");
                    return Ok(grades);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while getting grade: " + ex);
                return BadRequest("Error occurred while getting grade.");
            }
        }
        #endregion

        #region GetGradesForDropdown
        /// <summary>
        /// GetGradesForDropdown
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetGradesForDropdown")]
        public async Task<ActionResult<IEnumerable>> GetGradesForDropdown()
        {
            m_Logger.LogInformation("Retrieving records from grades table.");

            try
            {
                var grades = await m_GradeService.GetGradesForDropdown();
                if (grades == null)
                {
                    m_Logger.LogInformation("No records found in grades table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { grades.Count} grades.");
                    return Ok(grades);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }


        }
        #endregion

        #region GetById
        /// <summary>
        /// GetGradesById
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        [HttpGet("GetById/{gradeId}")]
        public async Task<ActionResult<Grade>> GetById(int gradeId)
        {
            m_Logger.LogInformation($"Retrieving records from grades table by {gradeId}.");

            try
            {
                var grade = await m_GradeService.GetById(gradeId);
                if (grade == null)
                {
                    m_Logger.LogInformation($"No records found for gradeId {gradeId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for gradeId {gradeId}.");
                    return Ok(grade);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region Update
        /// <summary>
        /// Update Grade
        /// </summary>
        /// <param name="gradeIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(Grade gradeIn)
        {
            m_Logger.LogInformation("Updating record in grades table.");

            try
            {
                dynamic response = await m_GradeService.Update(gradeIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating grade: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in grade's table.");
                    return Ok(response.Grade);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating grade: " + ex);
                return BadRequest("Error occurred while updating grade.");
            }
        }
        #endregion

        #region GetGradeByDesignation
        /// <summary>
        /// GetGradeByDesignation
        /// </summary>
        /// <param name="designationId"></param>
        /// <returns></returns>
        [HttpGet("GetGradeByDesignation/{designationId}")]
        public async Task<ActionResult<Grade>> GetGradeByDesignation(int designationId)
        {
            m_Logger.LogInformation($"Retrieving records from grades table by {designationId}.");

            try
            {
                var grade = await m_GradeService.GetGradeByDesignation(designationId);
                if (grade == null)
                {
                    m_Logger.LogInformation($"No records found for designationId {designationId}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for designationId {designationId}.");
                    return Ok(grade);
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