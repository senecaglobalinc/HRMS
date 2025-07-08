using HRMS.Project.Types;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Entities;
using HRMS.Project.API.Auth;

namespace HRMS.Project.API
{
    [Route("project/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController: Controller
    {
        #region Global Variables

        private readonly IReportService m_ReportService;
        private readonly ILogger<ReportController> m_Logger;

        #endregion

        #region Constructor
        public ReportController(IReportService reportService,
            ILogger<ReportController> logger)
        {
            m_ReportService = reportService;
            m_Logger = logger;
        }
        #endregion      

        #region GetFinanceReportAllocations
        /// <summary>
        /// Get FinanceReportAllocations
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("GetFinanceReportAllocations")]
        public async Task<ActionResult<FinanceReportAllocation>> GetFinanceReportAllocations(FinanceReportFilter filter)
        { 
            try
            {
                var financeReport = await m_ReportService.GetFinanceReportAllocations(filter);
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

        #region GetUtilizationReportAllocations
        /// <summary>
        /// Gets Resource Utilization Details.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("GetUtilizationReportAllocations")]
        public async Task<ActionResult<UtilizationReportAllocation>> GetUtilizationReportAllocations(UtilizationReportFilter filter)
        {
            m_Logger.LogInformation($"Getting project wise utilization of resoures.");

            try
            {
                var resource_util_details = await m_ReportService.GetUtilizationReportAllocations(filter);
                if (resource_util_details.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(resource_util_details.Items);
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

        #region Get TalentPool Wise Resource Count
        /// <summary>
        /// Get TalentPool Wise Resource Count
        /// </summary>
        /// <param name="projectTypeId"></param>
        /// <returns></returns>
        [HttpGet("GetTalentPoolWiseResourceCount")]
        public async Task<ActionResult<TalentpoolDataCount>> GetTalentPoolWiseResourceCount(string projectTypeIds)
        {
            m_Logger.LogInformation($"Getting TalentPool Wise Resource Count");

            try
            {
                var talentPoolDataCount = await m_ReportService.GetTalentPoolWiseResourceCount(projectTypeIds);
                if (talentPoolDataCount.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(talentPoolDataCount.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetEmployeeByProjectId
        /// <summary>
        /// GetEmployeeByProjectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("GetEmployeeByProjectId")]
        public async Task<ActionResult<int>> GetEmployeeByProjectId(int projectId)
        {
            m_Logger.LogInformation($"Getting TalentPool Wise Resource Count");

            try
            {
                var talentPoolData = await m_ReportService.GetEmployeeByProjectId(projectId);
                if (talentPoolData.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(talentPoolData.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetResourceByProject
        /// <summary>
        /// GetResourceByProject
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("GetResourceByProject")]
        public async Task<ActionResult<ProjectResourceData>> GetResourceByProject(int projectId)
        {
            m_Logger.LogInformation($"Getting Resource By Project");

            try
            {
                var projectResourceData = await m_ReportService.GetResourceByProject(projectId);
                if (projectResourceData.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(projectResourceData.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetSkillSearchAllocations
        /// <summary>
        /// Get GetSkillSearchAllocations
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetSkillSearchAllocations")]
        public async Task<ActionResult<SkillSearchAllocation>> GetSkillSearchAllocations()
        {
            try
            {
                var skillSearchAllocation = await m_ReportService.GetSkillSearchAllocations();
                if (skillSearchAllocation.Items == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found.");
                    return Ok(skillSearchAllocation.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetAllProjects
        /// <summary>
        /// GetAll Projects
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllProjects")]
        public async Task<ActionResult<ProjectResponse>> GetAll()
        {
            m_Logger.LogInformation("Retrieving records from Projects table.");



            try
            {
                var projects = await m_ReportService.GetAllProjects();
                if (projects.Items == null)
                {
                    m_Logger.LogInformation("No records found in Projects table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { projects.Items.Count } Projects.");
                    return Ok(projects.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }




        }
        #endregion

        #region GetServiceTypeProjectCount
        /// <summary>
        /// GetServiceTypeProjectCount
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetServiceTypeProjectCount")]
        public async Task<ActionResult<ServiceTypeCount>> GetServiceTypeProjectCount()
        {
            m_Logger.LogInformation("Retrieving records from Projects table.");



            try
            {
                var projects = await m_ReportService.GetServiceTypeProjectCount();
                if (projects.Items == null)
                {
                    m_Logger.LogInformation("No records found in Projects table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { projects.Items.Count } Projects.");
                    return Ok(projects.Items);
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
        /// GetCriticalResourceReport
        /// </summary>
        /// <param name=""></param>
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
        /// GetNonCriticalResourceReport
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("GetNonCriticalResourceReport")]
        public async Task<ActionResult<AssociateInformationReport>> GetNonCriticalResourceReport()
        {
            try
            {
                var resourceReport = await m_ReportService.GetNonCriticalResourceReport();
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

        #region GetNonCriticalResourceBillingReport
        /// <summary>
        /// GetNonCriticalResourceBillingReport
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("GetNonCriticalResourceBillingReport")]
        public async Task<ActionResult<AssociateInformationReport>> GetNonCriticalResourceBillingReport()
        {
            try
            {
                var resourceReport = await m_ReportService.GetNonCriticalResourceBillingReport();
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


    }
}
