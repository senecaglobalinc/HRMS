using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class SkillSearchController : ControllerBase
    {
        #region Global Variables

        private readonly ISkillSearchService m_SkillSearchService;
        private readonly ILogger<SkillSearchController> m_Logger;

        #endregion

        #region Constructor
        public SkillSearchController(ISkillSearchService skillSearchService, 
            ILogger<SkillSearchController> logger)
        {
            m_SkillSearchService = skillSearchService;
            m_Logger = logger;
        }
        #endregion

        #region BulkInsert
        /// <summary>
        /// BulkInsert 
        /// </summary>        
        /// <returns></returns>
        [HttpPost("BulkInsert")]
        public async Task<ActionResult> BulkInsert()
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Inserting record into SkillSearch table");
            try
            {
                var skill = await m_SkillSearchService.BulkInsert();
                if (!skill.IsSuccessful)
                {
                    m_Logger.LogError(skill.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute BulkInsert() in SkillSearchController:" + stopwatch.Elapsed);

                    return NotFound(skill.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully inserted records into SkillSearch table");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute BulkInsert() in SkillSearchController:" + stopwatch.Elapsed);

                    return Ok(skill);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"BulkInsert()\" of SkillSearchController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Create() in SkillSearchController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while inserting SkillSearch");
            }

        }

        #endregion

        #region GetAll
        /// <summary>
        /// Get All Skill Search
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<SkillSearch>>> GetAll()
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving records from Skill Search table.");

            try
            {
                var skillSearch = await m_SkillSearchService.GetAll();
                if (!skillSearch.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in Skill Search table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAll() in SkillSearchController:" + stopwatch.Elapsed);
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { skillSearch.Items.Count } Skill Search.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAll() in SkillSearchController:" + stopwatch.Elapsed);
                    return Ok(skillSearch.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured while getting skill Search in GetAll() method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Error occured while getting skill Search in GetAll() method" + stopwatch.Elapsed);
                return BadRequest();
            }


        }
        #endregion

        #region GetById
        /// <summary>
        /// Get SkillSearch by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<SkillSearch>> GetById(int id)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from Skill Search table by {id}.");

            try
            {
                var skillSearch = await m_SkillSearchService.GetById(id);
                if (!skillSearch.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found for skillSearchId {id}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetById() in SkillSearchController:" + stopwatch.Elapsed);
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for skillSearchId {id}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetById() in SkillSearchController:" + stopwatch.Elapsed);
                    return Ok(skillSearch.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in GetById() method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetById() in SkillSearchController:" + stopwatch.Elapsed);
                return BadRequest("Error occured in GetById() method");
            }

        }
        #endregion

        #region GetAllSkillDetails
        /// <summary>
        /// Get All Skill Details
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllSkillDetails")]
        public async Task<ActionResult<List<EmployeeSkillSearch>>> GetAllSkillDetails(int empID)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving records from Skill Search table.");

            try
            {
                var skillSearch = await m_SkillSearchService.GetAllSkillDetails(empID);
                if (!skillSearch.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in Skill Search table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAll() in SkillSearchController:" + stopwatch.Elapsed);
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { skillSearch.Items.Count } Skill Search.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAll() in SkillSearchController:" + stopwatch.Elapsed);
                    return Ok(skillSearch.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured while getting skill Search in GetAll() method" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Error occured while getting skill Search in GetAll() method" + stopwatch.Elapsed);
                return BadRequest();
            }
        }
        #endregion       
    }
}
