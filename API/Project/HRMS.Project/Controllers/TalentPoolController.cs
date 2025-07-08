using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using HRMS.Project.API.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.Project.API.Controllers
{
    [Route("project/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class TalentPoolController : ControllerBase
    {
        #region Global Variables

        private readonly ITalentPoolService m_TalentPoolService;
        private readonly ILogger<TalentPoolController> m_Logger;

        #endregion

        #region Constructor
        public TalentPoolController(ITalentPoolService talentPoolService,
            ILogger<TalentPoolController> logger)
        {
            m_TalentPoolService = talentPoolService;
            m_Logger = logger;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get the all the TalentPool
        /// </summary>
        /// <returns></returns>
        
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<TalentPool>>> GetAll()
        {
            m_Logger.LogInformation("Get all talentpool data.");
            try
            {
                var response = await m_TalentPoolService.GetAll();
                if (!response.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found for talentpool.");
                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found for talentpool.");
                    return Ok(response.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        #endregion

        #region Create
        /// <summary>
        /// Create TalentPool
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="practiceAreaId">practiceAreaId</param>
        /// <returns></returns>

        [HttpPost("Create")]
        public async Task<IActionResult> Create(TalentPoolRequest talentPoolRequest)
        {
            m_Logger.LogInformation("Inserting record in talentpool table.");
            try
            {
                var response = await m_TalentPoolService.Create(talentPoolRequest.ProjectId, talentPoolRequest.PracticeAreaId);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogInformation("Error occurred while creating talentpool: " + (string)response.Message);
                    return Ok(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in talentpool table.");
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating talentpool: " + ex);

                return Ok("Error occurred while creating talentpool.");
            }
        }
        #endregion
    }
}
