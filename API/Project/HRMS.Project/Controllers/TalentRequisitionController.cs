using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Project.Entities;
using HRMS.Project.Types;
using HRMS.Project.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMS.Project.API
{
    [Route("project/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class TalentRequisitionController : Controller
    {

        #region Global Variables

        private readonly ITalentRequisitionService m_TalentRequisitionService;
        private readonly ILogger<TalentRequisitionController> m_Logger;

        #endregion

        #region Constructor
        /// <summary>
        /// TalentRequisitionController
        /// </summary>
        /// <param name="talentRequisitionService"></param>
        /// <param name="logger"></param>
        public TalentRequisitionController(ITalentRequisitionService talentRequisitionService,
            ILogger<TalentRequisitionController> logger)
        {
            m_TalentRequisitionService = talentRequisitionService;
            m_Logger = logger;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetAll TalentRequisition
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            m_Logger.LogInformation("Retrieving records from TalentRequisition table.");

            try
            {
                var talentRequisition = await m_TalentRequisitionService.GetAll();
                if (talentRequisition == null)
                {
                    m_Logger.LogInformation("No records found in TalentRequisition table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { talentRequisition.Items.Count } TalentRequisitions.");
                    return Ok(talentRequisition.Items);
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
        /// GetById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<TalentRequisition>> GetById(int id)
        {
            m_Logger.LogInformation($"Retrieving records from TalentRequisition table by {id}.");

            try
            {
                var talent_requisition = await m_TalentRequisitionService.GetById(id);
                if (talent_requisition.Item == null)
                {
                    m_Logger.LogInformation($"No records found for Id {id}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for id {id}.");
                    return Ok(id);
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
