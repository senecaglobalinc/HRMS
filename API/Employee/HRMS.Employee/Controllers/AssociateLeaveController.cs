using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AssociateLeaveController : Controller
    {
        #region Global Variables

        private readonly IAssociateLeaveService m_associateLeaveService;
        private readonly ILogger<AssociateLeaveController> m_Logger;

        #endregion

        #region Constructor
        public AssociateLeaveController(IAssociateLeaveService associateLeaveService, ILogger<AssociateLeaveController> logger)
        {
            m_associateLeaveService = associateLeaveService;
            m_Logger = logger;
        }
        #endregion

        #region UploadLeaveData
        /// <summary>
        /// Upload Leave Data
        /// </summary>
        /// <returns></returns>
        [HttpPost("UploadLeaveData")]
        public async Task<ActionResult> UploadLeaveData(IFormFile file)
        {
            try
            {
                var employees = await m_associateLeaveService.UploadLeaveData(file);
                if (!employees.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found");
                    if (employees.Message == "File not found" || employees.Message== "No data to process")
                    {
                        return NotFound(employees.Message);
                    }
                    return BadRequest(employees.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning { employees } Skill Search.");
                    return Ok(employees);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in UpdateLeaveData method in AssociateLeaveController" + ex.StackTrace);
                return BadRequest("Error Occured while processing leave data");
            }


        }
        #endregion

        #region GetTemplateFile
        /// <summary>
        /// Get Template File
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTemplateFile")]
        public ActionResult GetTemplateFile()
        {
            try
            {

                var file =  m_associateLeaveService.GetTemplateFile();
                if (file==null)
                {                    
                        return NotFound(file);
                }
                else
                {                  
                    return Ok(file);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetTemplateFile method in AssociateLeaveController" + ex.StackTrace);
                return BadRequest("Error Occured while fetching Template file");
            }


        }
        #endregion


    }
}
