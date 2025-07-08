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
    public class ReportController : Controller
    {
        #region Global Variables

        private readonly IReportService m_ReportService;
        private readonly ILogger<ReportController> m_Logger;
        #endregion

        #region Constructor
        public ReportController(IReportService reportService, ILogger<ReportController> logger)
        {
            m_ReportService = reportService;
            m_Logger = logger;
        }
        #endregion

        #region GetFinanceReportMasters
        /// <summary>
        /// Get FinanceReportMasters
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetFinanceReportMasters")]
        public async Task<ActionResult<ReportDetails>> GetFinanceReportMasters()
        {
            try
            {
                var financeReport = await m_ReportService.GetFinanceReportMasters();
                if (financeReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(financeReport.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetUtilizationReportMasters
        /// <summary>
        /// Get UtilizationReportMasters
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetUtilizationReportMasters")]
        public async Task<ActionResult<ReportDetails>> GetUtilizationReportMasters()
        {
            try
            {
                var utilizationReport = await m_ReportService.GetUtilizationReportMasters();
                if (utilizationReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(utilizationReport.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion      

        #region GetDomainReportMasters
        /// <summary>
        /// Get DomainReportMasters
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetDomainReportMasters")]
        public async Task<ActionResult<ReportDetails>> GetDomainReportMasters()
        {
            try
            {
                var domainReport = await m_ReportService.GetDomainReportMasters();
                if (domainReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(domainReport.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetTalentPoolReportMasters
        /// <summary>
        /// Get TalentPool Report Masters
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetTalentPoolReportMasters")]
        public async Task<ActionResult<ReportDetails>> GetTalentPoolReportMasters()
        {
            try
            {
                var talentPoolReport = await m_ReportService.GetTalentPoolReportMasters();
                if (talentPoolReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(talentPoolReport.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetSkillSearchMasters
        /// <summary>
        /// GetSkillSearchMasters
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetSkillSearchMasters")]
        public async Task<ActionResult<ReportDetails>> GetSkillSearchMasters()
        {
            try
            {
                var skills = await m_ReportService.GetSkillSearchMasters();
                if (skills.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(skills.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetSkillsBySearchString
        /// <summary>
        /// GetSkillsBySearchString
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetSkillsBySearchString")]
        public async Task<ActionResult<GenericType>> GetSkillsBySearchString(string searchString)
        {
            try
            {
                var skills = await m_ReportService.GetSkillsBySearchString(searchString);
                if (skills.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(skills.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetAssociateAllocationMasters
        /// <summary>
        /// Get GetAssociateAllocationMasters
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetAssociateAllocationMasters")]
        public async Task<ActionResult<ReportDetails>> GetAssociateAllocationMasters()
        {
            try
            {
                var associateAllocationMasters = await m_ReportService.GetAssociateAllocationMasters();
                if (associateAllocationMasters.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(associateAllocationMasters.Items);
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