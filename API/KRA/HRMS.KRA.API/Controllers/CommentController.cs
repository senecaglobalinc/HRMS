using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Types;
using HRMS.KRA.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.KRA.API.Controllers
{
    [Authorize]
    [Route("kra/api/v1/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        #region Global Variables
        private readonly ICommentService m_CommentService;
        private readonly ILogger<CommentController> m_Logger;
        #endregion

        #region Constructor
        public CommentController(ICommentService commentService, ILogger<CommentController> logger)
        {
            m_CommentService = commentService;
            m_Logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CommentModel model)
        {
            m_Logger.LogInformation("Inserting record in Comment table.");
            try
            {
                var response = await m_CommentService.Create(model);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogError("Error occurred while creating Comment record: " + response.Message);
                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in Comment table.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            { 
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while creating a new Comment record.");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets all Comments 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(int financialYearId, int departmentId, int gradeId, int roleTypeId, bool IsCEO)
        {
            m_Logger.LogInformation("Retrieving records from RoleType table by {gradeId}.");
            try
            {
                var comments = await m_CommentService.GetAll(financialYearId, departmentId, gradeId, roleTypeId, IsCEO);
                if (comments == null)
                {
                    m_Logger.LogInformation("No records found");
                    return NotFound(comments.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning Comments.");
                    return Ok(comments.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Comments.");
            }
        }
        #endregion
    }
}
