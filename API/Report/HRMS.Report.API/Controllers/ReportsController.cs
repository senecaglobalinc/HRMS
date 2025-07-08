using HRMS.Report.Infrastructure.Models.Domain;
using HRMS.Report.Infrastructure.Models.Request;
using HRMS.Report.Infrastructure.Models.Response;
using HRMS.Report.Types;
using HRMS.Report.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Report.API.Controllers
{
    [Route("report/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : Controller
    {
        #region Global Variables

        private readonly IReportService m_ReportService;
        private readonly ILogger<ReportsController> m_Logger;

        #endregion

        #region Constructor
        public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
        {
            m_ReportService = reportService;
            m_Logger = logger;
        }
        #endregion

        #region GetFinanceReports
        /// <summary>
        /// Get Finance Reports with filters
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Finance Report</returns>
        [HttpPost("GetFinanceReports")]
        public async Task<ActionResult<FinanceReport>> GetFinanceReports(FinanceReportFilter filter)
        {
            try
            {
                var financeReport = await m_ReportService.GetFinanceReport(filter);
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

        #region GetUtilizationReports
        /// <summary>
        /// Get Utilization Reports with filters
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("GetUtilizationReports")]
        public async Task<ActionResult<UtilizationReport>> GetUtilizationReports(UtilizationReportFilter filter)
        {
            try
            {
                // when calling from Report service -non job call
                bool isNightJob = false;
                var financeReport = await m_ReportService.GetUtilizationReport(filter,  isNightJob);
                if (financeReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return Ok(new List<UtilizationReport>());
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

        #region GetDomainReportCount
        /// <summary>
        /// Get Domain Report Count
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetDomainReportCount")]
        public async Task<ActionResult<DomainReportCount>> GetDomainReportCount()
        {
            try
            {
                var domainReport = await m_ReportService.GetDomainReportCount();
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

        #region GetTalentPoolReportCount
        /// <summary>
        /// Get Talent Pool Report Count
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetTalentPoolReportCount")]
        public async Task<ActionResult<TalentPoolReportCount>> GetTalentPoolReportCount()
        {
            try
            {
                var talentPoolReport = await m_ReportService.GetTalentPoolReportCount();
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

        #region GetDomainReport
        /// <summary>
        /// Get Domain Report by domainId
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetDomainReport")]
        public async Task<ActionResult<DomainReport>> GetDomainReport(int domainId)
        {
            try
            {
                var domainReport = await m_ReportService.GetDomainReport(domainId);
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

        #region GetTalentPoolReport
        /// <summary>
        /// Get Talent Pool Report by projectId
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetTalentPoolReport")]
        public async Task<ActionResult<DomainReport>> GetTalentPoolReport(int projectId)
        {
            try
            {
                var talentPoolReport = await m_ReportService.GetTalentPoolReport(projectId);
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

        #region GetEmployeeDetailsBySkill
        /// <summary>
        /// Get Employee Details By Skill by filters
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("GetEmployeeDetailsBySkill")]
        public async Task<ActionResult<AssociateSkillSearch>> GetEmployeeDetailsBySkill(AssociateSkillSearchFilter filter)
        {
            try
            {
                var financeReport = await m_ReportService.GetEmployeeDetailsBySkill(filter);
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

        #region GetServiceTypeReportCount
        /// <summary>
        /// Get Service Type Report with string filter
        /// </summary>  
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("GetServiceTypeReportCount")]
        public async Task<ActionResult<ServiceTypeReportCount>> GetServiceTypeReport(string filter)
        {
            try
            {
                var serviceTypeReport = await m_ReportService.GetServiceTypeReport(filter);
                if (serviceTypeReport.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(serviceTypeReport.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetServiceTypeReportEmployee
        /// <summary>
        /// Get Service Type Report by serviceTypeId
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetServiceTypeReportProject")]
        public async Task<ActionResult<ProjectRespose>> GetServiceTypeReportProject(int serviceTypeId)
        {
            try
            {
                var domainReport = await m_ReportService.GetServiceTypeReportProject(serviceTypeId);
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

        #region GetCriticalResourceReport
        /// <summary>
        /// Get Critical Resource Report
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetCriticalResourceReport")]
        public async Task<ActionResult<AssociateInformationReport>> GetCriticalResourceReport()
        {
            try
            {
                var resourceReport = await m_ReportService.GetCriticalResourceReport();

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

        #region GetNonCriticalResourceReport
        /// <summary>
        /// Get Non Critical Resource Report
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetNonCriticalResourceReport")]
        public async Task<ActionResult<AssociateInformationReport>> GetNonCriticalResourceReport()
        {
            try
            {
                var resourceReport =await m_ReportService.GetNonCriticalResourceReport();

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

        #region GetTalentPoolResourceReport
        /// <summary>
        /// Get TalentPool Resource Report
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetTalentPoolResourceReport")]
        public async Task<ActionResult<AssociateInformationReport>> GetTalentPoolResourceReport()
        {
            try
            {
                // when calling from Report service -non job call
                bool isNightJob = false;
                var resourceReport = await m_ReportService.GetTalentPoolResourceReport(isNightJob);

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

        #region GetAssociatesForFutureAllocation
        /// <summary>
        /// Get Associates For Future Allocation
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetAssociatesForFutureAllocation")]
        public async Task<ActionResult<AssociateInformationReport>> GetAssociatesForFutureAllocation()
        {
            try
            {
                // when calling from Report service -non job call
                var associateInformation = await m_ReportService.GetAssociatesForFutureAllocation();

                if (associateInformation.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(associateInformation.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetNonCriticalResourceBillingReport
        /// <summary>
        /// GetNonCriticalResourceBillingReport details
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetNonCriticalResourceBillingReport")]
        public async Task<ActionResult<AssociateInformationReport>> GetNonCriticalResourceBillingReport()
        {
            try
            {
                // when calling from Report service -non job call
                var associateInformation = await m_ReportService.GetNonCriticalResourceBillingReport();

                if (associateInformation.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(associateInformation.Items);
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

        [HttpPost("GetParkingSloteport")]
        public async Task<ActionResult> GetParkingSloteport(ParkingSearchFilter filter)
        {
            ServiceListResponse<ParkingSlotReport> response = null;
            try
            {
                response = await m_ReportService.GetParkingSloteport(filter);
                if(!response.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound(response);
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured while fetching data from Parking slot report ", ex.Message);
                return BadRequest();
            }           
        }
        #endregion
    }
}
