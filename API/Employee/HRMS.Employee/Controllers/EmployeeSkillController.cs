using System;
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
    public class EmployeeSkillController : Controller
    {
        #region Global Variables
        private readonly IEmployeeSkillService m_EmployeeSkillService;
        private readonly ILogger<EmployeeSkillController> m_Logger;
        #endregion

        #region Constructor
        public EmployeeSkillController(IEmployeeSkillService employeeSkillService, ILogger<EmployeeSkillController> logger)
        {
            m_EmployeeSkillService = employeeSkillService;
            m_Logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// Create skill 
        /// </summary>
        /// <param name="employeeSkillDetails"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<EmployeeSkill>> Create(EmployeeSkillDetails employeeSkillDetails)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Inserting record into EmployeeSkill table");
            try
            {
                var skill = await m_EmployeeSkillService.Create(employeeSkillDetails);
                if (!skill.IsSuccessful)
                {
                    m_Logger.LogError(skill.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in EmployeeSkillController:" + stopwatch.Elapsed);

                    return NotFound(skill.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully inserted records into EmployeeSkill table");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Create() in EmployeeSkillController:" + stopwatch.Elapsed);

                    return Ok(skill.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"Create()\" of EmployeeSkillController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Create() in EmployeeSkillController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while creating Skill");
            }

        }

        #endregion

        #region GetByEmployeeId
        /// <summary>
        /// Fetches project details of an employee based on employee id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetByEmployeeId/{employeeId}/{roleName}")]
        public async Task<ActionResult<List<EmployeeSkill>>> GetByEmployeeId(int employeeId,string roleName)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Fetching records from  EmployeeSkill table");
            try
            {
                var skills = await m_EmployeeSkillService.GetByEmployeeId(employeeId,roleName);
                if (!skills.IsSuccessful)
                {
                    m_Logger.LogError(skills.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetByEmployeeId() in EmployeeSkillController:" + stopwatch.Elapsed);
                    return NotFound(skills.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully fetched project details");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetByEmployeeId() in EmployeeSkillController:" + stopwatch.Elapsed);
                    return Ok(skills.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"GetByEmployeeId()\" of EmployeeSkillController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetByEmployeeId() in EmployeeSkillController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while fetching skill details");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update skill 
        /// </summary>
        /// <param name="employeeSkillDetails"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<ActionResult<EmployeeSkill>> Update(EmployeeSkillDetails employeeSkillDetails)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Updating record in EmployeeSkill table");
            try
            {
                var skill = await m_EmployeeSkillService.Update(employeeSkillDetails);
                if (!skill.IsSuccessful)
                {
                    m_Logger.LogError(skill.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Update() in EmployeeSkillController:" + stopwatch.Elapsed);

                    return NotFound(skill.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully inserted records into EmployeeSkill table");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute Update() in EmployeeSkillController:" + stopwatch.Elapsed);

                    return Ok(skill.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"Update()\" of EmployeeSkillController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute Update() in EmployeeSkillController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while updating Skill");
            }

        }

        #endregion

        #region GetByEmployeeId
        /// <summary>
        /// Fetches project details of an employee based on employee id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteSkill/{Id}")]
        public async Task<ActionResult<bool>> DeleteSkill(int Id)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("deleting record from  Skill table");
            try
            {
                var response = await m_EmployeeSkillService.DeleteSkill(Id);
                if (!response.IsSuccessful)
                {
                    m_Logger.LogError(response.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute DeleteSkill() in EmployeeSkillController:" + stopwatch.Elapsed);
                    return NotFound(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully Deleted skill details");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute DeleteSkill() in EmployeeSkillController:" + stopwatch.Elapsed);
                    return Ok(response.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"DeleteSkill()\" of EmployeeSkillController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute DeleteSkill() in EmployeeSkillController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while deleting skill details");
            }
        }
        #endregion

    }
}