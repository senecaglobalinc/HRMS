using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeSkillWorkFlowController : Controller
    {
        #region Global Variables
        private readonly IEmployeeSkillWorkFlow m_EmployeeSkillworkflow;
        private readonly ILogger<EmployeeSkillWorkFlowController> m_logger;
        #endregion

        #region Constructor
        public EmployeeSkillWorkFlowController(IEmployeeSkillWorkFlow employeeSkillService, ILogger<EmployeeSkillWorkFlowController> logger)
        {
            m_EmployeeSkillworkflow = employeeSkillService;
            m_logger = logger;
        }
        #endregion


        #region Create
        /// <summary>
        /// Create skill work flow
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpPost("Create/{employeeId}")]
        public async Task<ActionResult> Create(int employeeId)
        {

            m_logger.LogInformation("Inserting record into EmployeeSkillWorkflow table");
            try
            {
                var skill = await m_EmployeeSkillworkflow.Create(employeeId);
                if (!skill.IsSuccessful)
                {
                    m_logger.LogError(skill.Message);
                    m_logger.LogInformation("Time to execute Create() in EmployeeSkillWorkFlowController:");
                    return BadRequest(skill.Message);
                }
                else
                {
                    m_logger.LogInformation("Successfully inserted records into EmployeeSkill workflow table");

                    return Ok(skill.Message);
                }
            }
            catch (Exception ex)
            {
                m_logger.LogError("Error occured in \"Create()\" of EmployeeSkillWorkFlowController" + ex.StackTrace);

                return BadRequest("Error occured while creating Skills");
            }

        }

        #endregion

        #region SkillStatusApprovedByRM
        /// <summary>
        /// Create skill work flow
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpPost("SkillStatusApprovedByRM/{employeeId}")]
        public async Task<ActionResult> SkillStatusApprovedByRM(int employeeId)
        {

            m_logger.LogInformation("Inserting record into EmployeeSkillWorkflow table");
            try
            {
                var skill = await m_EmployeeSkillworkflow.SkillStatusApprovedByRM(employeeId);
                if (!skill.IsSuccessful)
                {
                    m_logger.LogError(skill.Message);
                    m_logger.LogInformation("Time to execute Create() in EmployeeSkillWorkFlowController:");
                    return BadRequest(skill.Message);
                }
                else
                {
                    m_logger.LogInformation("Successfully updated the record into EmployeeSkillworkflow table after approving");

                    return Ok(skill.Message);
                }
            }
            catch (Exception ex)
            {
                m_logger.LogError("Error occured in \"Create()\" of EmployeeSkillWorkFlowController" + ex.StackTrace);

                return BadRequest("Error occured while approving submitted Skills");
            }

        }

        #endregion

        #region GetSkillSubmittedByEmployee
        /// <summary>
        ///get all the skill submitted employees details 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("GetSkillSubmittedByEmployee/{reportingManager}")]
        public async Task<ActionResult> GetSkillSubmittedByEmployee(int reportingManager)
        {

            m_logger.LogInformation("fetching records from EmployeeSkillWorkflow and Employee tables");
            try
            {
                var emps = await m_EmployeeSkillworkflow.GetSkillSubmittedByEmployee(reportingManager);
                if (!emps.IsSuccessful)
                {
                    m_logger.LogError(emps.Message);
                    m_logger.LogInformation("Time to execute getSkillSubmittedEmps() in EmployeeSkillWorkFlowController:");
                    return NotFound(emps.Message);
                }
                else
                {
                    m_logger.LogInformation("Successfully fetched records from EmployeeSkillWorkflow and Employee tables");

                    return Ok(emps.Items);
                }
            }
            catch (Exception ex)
            {
                m_logger.LogError("Error occured in \"getSkillSubmittedEmps()\" of EmployeeSkillWorkFlowController" + ex.StackTrace);

                return BadRequest("Error occured while fetching getSkillSubmittedEmps");
            }

        }

        #endregion


        #region GetSubmittedSkillsByEmpid
        /// <summary>
        /// get all the skills based on empid
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetSubmittedSkillsByEmpid/{employeeId}")]
        public async Task<ActionResult> getAllSubmittedSkillsByEmpid(int employeeId)
        {

            m_logger.LogInformation("fetching records from EmployeeSkills table");
            try
            {
                var skills = await m_EmployeeSkillworkflow.GetSubmittedSkillsByEmpid(employeeId);
                if (!skills.IsSuccessful)
                {
                    m_logger.LogError(skills.Message);
                    m_logger.LogInformation("Time to execute getAllSubmittedSkillsByEmpid() in EmployeeSkillWorkFlowController:");
                    return BadRequest(skills.Message);
                }
                else
                {
                    m_logger.LogInformation("Successfully fetched records from EmployeeSkillWorkFlow table");

                    return Ok(skills.Items);
                }
            }
            catch (Exception ex)
            {
                m_logger.LogError("Error occured in \"getAllSubmittedSkillsByEmpid()\" of EmployeeSkillWorkFlowController" + ex.StackTrace);

                return BadRequest("Error occured while fetching getAllSubmittedSkillsByEmpid");
            }

        }

        #endregion

        #region UpdateEmpSkillDetails
        [HttpPost("UpdateEmpSkillDetails/{employeeId}")]
        public async Task<ActionResult> UpdateEmpSkillDetails(int employeeId)
        {
            m_logger.LogInformation("Inserting record into EmployeeSkillWorkflow table");
            try
            {
                var skill = await m_EmployeeSkillworkflow.UpdateEmpSkillDetails(employeeId);
                if (!skill.IsSuccessful)
                {
                    m_logger.LogError(skill.Message);
                    m_logger.LogInformation("Time to execute update() in EmployeeSkillWorkFlowController:");
                    return NotFound(skill.Message);
                }
                else
                {
                    m_logger.LogInformation("Successfully updated records into EmployeeSkill table.");

                    return Ok(skill.Message);
                }
            }
            catch (Exception ex)
            {
                m_logger.LogError("Error occured in \"updateEmpSkillDetailsByRM()\" of EmployeeSkillWorkFlowController" + ex.StackTrace);

                return BadRequest("Error occured while updating EmployeeSkills");
            }

        }
        #endregion

        #region GetEmployeeSkillHistory
        [HttpGet("GetEmployeeSkillHistory/{employeeId}")]
        public async Task<ActionResult> GetEmployeeSkillHistory(int employeeId,int ID)
        {
            m_logger.LogInformation("Inserting record into EmployeeSkillWorkflow table");
            try
            {
                var skill = await m_EmployeeSkillworkflow.GetEmployeeSkillHistory(employeeId,ID);
                if (!skill.IsSuccessful)
                {
                    m_logger.LogError(skill.Message);
                    m_logger.LogInformation("Time to execute update() in EmployeeSkillWorkFlowController:");
                    return NotFound(skill.Message);
                }
                else
                {
                    m_logger.LogInformation("Successfully fetched records from EmployeeSkillWorkFlow table.");

                    return Ok(skill.Items);
                }
            }
            catch (Exception ex)
            {
                m_logger.LogError("Error occured in \"updateEmpSkillDetailsByRM()\" of EmployeeSkillWorkFlowController" + ex.StackTrace);

                return BadRequest("Error occured while updating Employ eeSkills");
            }

        }
        #endregion



        #region UpdateEmpSkillProficienyByRM
        [HttpPost("UpdateEmpSkillProficienyByRM")]
        public async Task<ActionResult> UpdateEmpSkillProficienyByRM(EmployeeSkillWorkflow employeeSkills)
        {
            m_logger.LogInformation("update record into EmployeeSkills table");
            try
            {
                var skill = await m_EmployeeSkillworkflow.UpdateEmpSkillProficienyByRM(employeeSkills);
                if (!skill.IsSuccessful)
                {
                    m_logger.LogError(skill.Message);
                    m_logger.LogInformation("Time to execute update() in EmployeeSkillWorkFlowController:");
                    return NotFound(skill.Message);
                }
                else
                {
                    m_logger.LogInformation("Successfully updated records into EmployeeSkill table.");

                    return Ok(skill.Message);
                }
            }
            catch (Exception ex)
            {
                m_logger.LogError("Error occured in \"UpdateEmpSkillProficienyByRM()\" of EmployeeSkillWorkFlowController" + ex.StackTrace);

                return BadRequest("Error occured while updating Employ eeSkills");
            }

        }
        #endregion
    }
}

