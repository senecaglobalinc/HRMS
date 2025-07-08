using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Request;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
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

        #region GetFinanceReportAssociates
        /// <summary>
        /// GetFinanceReportAssociates
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("GetFinanceReportAssociates")]
        public async Task<ActionResult<FinanceReportEmployee>> GetFinanceReportAssociates(FinanceReportEmployeeFilter filter)
        {
            try
            {
                var financeReport = await m_ReportService.GetFinanceReportAssociates(filter);
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

        #region GetUtilizationReportAssociates
        /// <summary>
        /// GetUtilizationReportAssociates
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("GetUtilizationReportAssociates")]
        public async Task<ActionResult<UtilizationReportEmployee>> GetUtilizationReportAssociates(UtilizationReportEmployeeFilter filter)
        {
            try
            {
                var utilizationReport = await m_ReportService.GetUtilizationReportAssociates(filter);
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

        #region GetDomainWiseResourceCount
        /// <summary>
        /// GetDomainWiseResourceCount
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("GetDomainWiseResourceCount")]
        public async Task<ActionResult<DomainDataCount>> GetDomainWiseResourceCount()
        {
            try
            {
                var domainReport = await m_ReportService.GetDomainWiseResourceCount();
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

        #region GetDomainReportAssociates
        /// <summary>
        /// GetDomainReportAssociates
        /// </summary>
        /// <param name="domainId"></param>
        /// <returns></returns>
        [HttpGet("GetDomainReportAssociates")]
        public async Task<ActionResult<DomainReportEmployee>> GetDomainReportAssociates(string employeeIds)
        {
            try
            {
                var domainReport = await m_ReportService.GetDomainReportAssociates(employeeIds);
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

        #region GetTalentPoolReportAssociates
        /// <summary>
        /// GetTalentPoolReportAssociates
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("GetTalentPoolReportAssociates")]
        public async Task<ActionResult<DomainReportEmployee>> GetTalentPoolReportAssociates(int projectId)
        {
            try
            {
                var talentPoolReport = await m_ReportService.GetTalentPoolReportAssociates(projectId);
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

        #region GetSkillSearchAssociates
        /// <summary>
        /// GetSkillSearchAssociates
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("GetSkillSearchAssociates")]
        public async Task<ActionResult<SkillSearchEmployee>> GetSkillSearchAssociates(SkillSearchFilter filter)
        {
            try
            {
                var skillReport = await m_ReportService.GetSkillSearchAssociates(filter);
                if (skillReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(skillReport.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetActiveAssociates
        /// <summary>
        /// GetActiveAssociates
        /// </summary>
        /// <param name="employeeIds"></param>
        /// <returns></returns>
        [HttpPost("GetActiveAssociates")]
        public async Task<ActionResult<GenericType>> GetActiveAssociates(List<int> employeeIds)
        {
            try
            {
                var employeeData = await m_ReportService.GetActiveAssociates(employeeIds);
                if (employeeData.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(employeeData.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        #endregion

        #region GetServiceTypeResourceCount
        /// <summary>
        /// GetDomainWiseResourceCount
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("GetServiceTypeResourceCount")]
        public async Task<ActionResult<ServiceTypeCount>> GetServiceTypeResourceCount()
        {
            try
            {
                var domainReport = await m_ReportService.GetServiceTypeResourceCount();
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

        #region GetUtilizationReport
        /// <summary>
        /// GetUtilizationReport
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("GetUtilizationReport")]
        public async Task<ActionResult<UtilizationReport>> GetUtilizationReport()
        {
            try
            {
                var resourceReport = await m_ReportService.GetUtilizationReport();
                if (resourceReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(resourceReport.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetResourceUtilization
        /// <summary>
        /// GetResourceUtilization
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetResourceUtilization")]
        public async Task<ActionResult<UtilizationReport>> GetResourceUtilization(int? year = null)
        {
            try
            {
                var resourceReport = await m_ReportService.GetResourceUtilization(year);
                if (resourceReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(resourceReport.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetAssociateExitReport
        /// <summary>
        /// GetAssociateExitReport
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("GetAssociateExitReport")]
        public async Task<ActionResult<AssociateExitReport>> GetAssociateExitReport()
        {
            try
            {
                var resourceReport = await m_ReportService.GetAssociateExitReport();
                if (resourceReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(resourceReport.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        [HttpPost("GetAssociateExitReport")]
        public async Task<ActionResult<AssociateExitReport>> GetAssociateExitReport(AssociateExitReportFilter filter)
        {
            try
            {
                var resourceReport = await m_ReportService.GetAssociateExitReport(filter);
                if (resourceReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(resourceReport.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetAssociateExitData
        /// <summary>
        /// GetAssociateExitData
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetAssociateExitData")]
        public async Task<ActionResult<AssociateExitReport>> GetAssociateExitData()
        {
            try
            {
                var resourceReport = await m_ReportService.GetAssociateExitData();
                if (resourceReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(resourceReport.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetAssociateExitReportTypes
        /// <summary>
        /// GetAssociateExitReportTypes
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetAssociateExitReportTypes")]
        public async Task<ActionResult<AssociateExitReport>> GetAssociateExitReportTypes()
        {
            try
            {
                var resourceReport = await m_ReportService.GetAssociateExitReportTypes();
                if (resourceReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return Ok(resourceReport.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(resourceReport.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion        

        #region GetAssociateExitChartReport
        /// <summary>
        /// GetAssociateExitChartReport
        /// </summary>        
        /// <returns></returns>
        [HttpPost("GetAssociateExitChartReport")]
        public async Task<ActionResult<ChartData>> GetAssociateExitChartReport(AssociateExitReportFilter filter)
        {
            try
            {
                var resourceReport = await m_ReportService.GetAssociateExitChartReport(filter);
                if (resourceReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return Ok(resourceReport.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(resourceReport.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetParkingSloteport
        /// <summary>
        /// GetParkingSloteport
        /// </summary>        
        /// <returns></returns>
        [HttpPost("GetParkingSlotReport")]
        public async Task<ActionResult<ChartData>> GetParkingSlotReport(ParkingSearchFilter filter)
        {
            try
            {
                var resourceReport = await m_ReportService.GetParkingSlotReport(filter);
                if (!resourceReport.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound(resourceReport);
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(resourceReport);
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
