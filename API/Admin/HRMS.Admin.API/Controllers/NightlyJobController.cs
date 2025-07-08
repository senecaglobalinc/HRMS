using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.API.Controllers
{
    [Route("admin/api/v1/[controller]")]
    [ApiController]
    [Authorize(Policy = "NightJobHeaderAuthPolicy")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class NightlyJobController : Controller
    {
        #region Global Variables
        private readonly IReportService m_ReportService;
        private readonly IStatusService m_StatusService;
        private readonly IDepartmentService m_DepartmentService;
        private readonly IPracticeAreaService m_PracticeAreaService;
        private readonly ILogger<NightlyJobController> m_Logger;
        #endregion

        #region Constructor
        public NightlyJobController(IReportService reportService, IStatusService statusService, IDepartmentService departmentService, IPracticeAreaService practiceAreaService, ILogger<NightlyJobController> logger)
        {
            m_ReportService = reportService;
            m_StatusService = statusService;
            m_DepartmentService = departmentService;
            m_PracticeAreaService = practiceAreaService;
            m_Logger = logger;
        }
        #endregion

        #region GetUtilizationReportMasters
        /// <summary>
        /// This endpoint is used in GetUtilizationReportMasters job. 
        /// Enforces Authorization policy to check its headers for its configuration
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

        #region GetAllByCategoryName
        /// <summary>
        /// GetStatuses
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("GetAllByCategoryName")]
        public async Task<ActionResult<IEnumerable>> GetAllByCategoryName(string category)
        {
            m_Logger.LogInformation("Retrieving records from status table.");

            try
            {
                var statuses = await m_StatusService.GetByCategory(category);
                if (statuses == null)
                {
                    m_Logger.LogInformation("No records found in Status table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { statuses.Count} status.");
                    return Ok(statuses);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }


        }
        #endregion

        #region GetAllDepartments 
        /// <summary>
        /// GetAll Departments
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllDepartments")]
        public async Task<ActionResult<IEnumerable>> GetAllDepartments(bool isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from Department table.");

            try
            {
                var department = await m_DepartmentService.GetAll(isActive);
                if (department == null)
                {
                    m_Logger.LogInformation("No records found in Department table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning Department.");
                    return Ok(department);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Department.");
            }
        }
        #endregion

        #region GetAllPracticeAreas 
        /// <summary>
        /// GetAll Departments
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllPracticeAreas")]
        public async Task<ActionResult<IEnumerable>> GetAllPracticeAreas(bool isActive = true)
        {
            m_Logger.LogInformation("Retrieving records from PracticeArea table.");

            try
            {
                var practiceArea = await m_PracticeAreaService.GetAll(isActive);
                if (practiceArea == null)
                {
                    m_Logger.LogInformation("No records found in PracticeArea table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { practiceArea.Count} Department.");
                    return Ok(practiceArea);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching PracticeArea.");
            }
        }
        #endregion        
    }
}
