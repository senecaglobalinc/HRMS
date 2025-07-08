using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeProfessionController : Controller
    {
        #region Global Variables

        private readonly IEmployeeProfessionalService m_EmployeeProfessionalService;
        private readonly ILogger<EmployeeProfessionController> m_Logger;

        #endregion

        #region Constructor
        public EmployeeProfessionController(IEmployeeProfessionalService employeeProfessionalService, ILogger<EmployeeProfessionController> logger)
        {
            m_EmployeeProfessionalService = employeeProfessionalService;
            m_Logger = logger;
        }
        #endregion

        #region CreateCertificate
        /// <summary>
        /// Create Certificate
        /// </summary>
        /// <param name="certificationIn"></param>
        /// <returns></returns>
        [HttpPost("CreateCertificate")]
        public async Task<ActionResult<AssociateCertification>> CreateCertificate(AssociateCertification certificationIn)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Inserting record into AssociateCertification table");
            try
            {
                var certifications = await m_EmployeeProfessionalService.CreateCertificate(certificationIn);
                if (!certifications.IsSuccessful)
                {
                    m_Logger.LogError(certifications.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute CreateCertificate() in EmployeeProfessionalController:" + stopwatch.Elapsed);

                    return NotFound(certifications.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully inserted record into AssociateCertification table");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute CreateCertificate() in EmployeeProfessionalController:" + stopwatch.Elapsed);

                    return Ok(certifications.Item);
                }
            }catch(Exception ex)
            {
                m_Logger.LogError("Error occured in \"CreateCertificate()\" of EmployeeProfessionController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute CreateCertificate() in EmployeeProfessionalController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while creating certification");
            }
            
        }

        #endregion

        #region CreateMembership
        /// <summary>
        /// Create Membership
        /// </summary>
        /// <param name="membershipIn"></param>
        /// <returns></returns>
        [HttpPost("CreateMembership")]
        public async Task<ActionResult<AssociateMembership>> CreateMembership(AssociateMembership membershipIn)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Inserting record into AssociateMembership table");
            try
            {
                var memberships = await m_EmployeeProfessionalService.CreateMembership(membershipIn);
                if (!memberships.IsSuccessful)
                {
                    m_Logger.LogError(memberships.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute CreateMembership() in EmployeeProfessionalController:" + stopwatch.Elapsed);

                    return NotFound(memberships.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully inserted record into AssociateMembership table");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute CreateMembership() in EmployeeProfessionalController:" + stopwatch.Elapsed);

                    return Ok(memberships.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"CreateMembership()\" of EmployeeProfessionController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute CreateMembership() in EmployeeProfessionalController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while creating membership");
            }
            
        }

        #endregion

        #region Delete
        /// <summary>
        /// delete professional details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="programType"></param>
        /// <returns></returns>

        [HttpDelete("Delete/{id}/{programType}")]
        public async Task<ActionResult<AssociateMembership>> Delete(int id, int programType)
        {
            m_Logger.LogInformation("Deleting record in AssociateMembership or AssociateCertification table");
            try
            {
                var memberships = await m_EmployeeProfessionalService.Delete(id, programType);
                if (!memberships.IsSuccessful)
                {
                    m_Logger.LogError(memberships.Message);
                    return NotFound(memberships.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted record");
                    return Ok(memberships.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"Delete()\" of EmployeeProfessionController" + ex.StackTrace);
                return BadRequest("Error occured while deleting certification or membership");
            }
        }
        #endregion

        #region GetByEmployeeId
        /// <summary>
        /// Get professional details By EmployeeId based on Skill and SkillGroup
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetByEmployeeId/{employeeId}")]
        public async Task<ActionResult<List<ProfessionalDetails>>> GetByEmployeeId(int employeeId)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Fetching records from AssociateCertification and AssociateMembership table");
            try
            {
                var professionalDetails = await m_EmployeeProfessionalService.GetByEmployeeId(employeeId);
                if (!professionalDetails.IsSuccessful)
                {
                    m_Logger.LogError(professionalDetails.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetByEmployeeId() in EmployeeProfessionalController:" + stopwatch.Elapsed);

                    return NotFound(professionalDetails.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully fetched professional details");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetByEmployeeId() in EmployeeProfessionalController:" + stopwatch.Elapsed);

                    return Ok(professionalDetails.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"GetByEmployeeId()\" of EmployeeProfessionController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetByEmployeeId() in EmployeeProfessionalController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while fetching professional details");
            }
        }
        #endregion

        #region UpdateCertificate
        /// <summary>
        /// Update Certificate details
        /// </summary>
        /// <param name="certificationIn"></param>
        /// <returns></returns>
        [HttpPost("UpdateCertificate")]
        public async Task<ActionResult<AssociateCertification>> UpdateCertificate(AssociateCertification certificationIn)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Updating record in AssociateCertification table");

            try
            {
                var certifications = await m_EmployeeProfessionalService.UpdateCertificate(certificationIn);
                if (!certifications.IsSuccessful)
                {
                    m_Logger.LogInformation(certifications.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute UpdateCertificate() in EmployeeProfessionalController:" + stopwatch.Elapsed);

                    return NotFound(certifications.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in AssociateCertification table");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute UpdateCertificate() in EmployeeProfessionalController:" + stopwatch.Elapsed);

                    return Ok(certifications.Item);
                }
            }catch(Exception ex)
            {
                m_Logger.LogError("Error occured in \"UpdateCertificate()\" of EmployeeProfessionController" + ex.StackTrace);

                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute UpdateCertificate() in EmployeeProfessionalController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while updating certification");
            }
            
        }

        #endregion

        #region UpdateMembership
        /// <summary>
        /// Update Membership details
        /// </summary>
        /// <param name="membershipIn"></param>
        /// <returns></returns>
        [HttpPost("UpdateMembership")]
        public async Task<ActionResult<AssociateMembership>> UpdateMembership(AssociateMembership membershipIn)
        {
            //create stopwatch
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Updating record in AssociateMembership table");
            try
            {
                var memberships = await m_EmployeeProfessionalService.UpdateMembership(membershipIn);
                if (!memberships.IsSuccessful)
                {
                    m_Logger.LogError(memberships.Message);

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute UpdateMembership() in EmployeeProfessionalController:" + stopwatch.Elapsed);

                    return NotFound(memberships.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in AssociateCertification table");

                    //stop timing
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute UpdateMembership() in EmployeeProfessionalController:" + stopwatch.Elapsed);

                    return Ok(memberships.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in \"UpdateMembership()\" of EmployeeProfessionController" + ex.StackTrace);
                //Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute UpdateMembership() in EmployeeProfessionalController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while updating membership");
            }

        }

        #endregion

        
    }
}