using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.KRA.Entities;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Types;
using HRMS.KRA.API.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.KRA.API.Controllers
{
    [Authorize]
    [Route("kra/api/v1/[controller]")]
    [ApiController]
    public class KRAController : ControllerBase
    {
        #region Global Variables
        private readonly IKRAService m_KraService;
        private readonly ILogger<KRAController> m_Logger;
        public readonly IWebHostEnvironment m_webHostEnvironment;
        #endregion

        #region Constructor
        public KRAController(IKRAService kraService, ILogger<KRAController> logger, IWebHostEnvironment webHostEnvironment)
        {
            m_KraService = kraService;
            m_Logger = logger;
            m_webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region GetKRAOperators
        /// <summary>
        /// Gets KRA Operator
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOperators")]
        public async Task<IActionResult> GetOperatorsAsync()
        {
            try
            {
                var Operators = await m_KraService.GetOperatorsAsync();
                if (Operators == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(Operators);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetKRATargetPeriods
        /// <summary>
        /// Gets KRA TargetPeriods
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTargetPeriods")]
        public async Task<IActionResult> GetTargetPeriodsAsync()
        {
            try
            {
                var TargetPeriods = await m_KraService.GetTargetPeriodsAsync();
                if (TargetPeriods == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(TargetPeriods);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region CreateKRADefinition
        /// <summary>
        /// creates kra definition
        /// </summary>
        /// <param name="kraDefinitionIn"></param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> CreateKRADefinition(KRADefinition kraDefinitionIn)
        //{
        //    try
        //    {
        //        var response = await m_KraService.CreateKRADefinition(kraDefinitionIn);
        //        if (!response.IsSuccessful)
        //        {
        //            //Add exeption to logger
        //            m_Logger.LogError("Error occurred while creating KRA Definition: " + response.Message);

        //            return BadRequest(response.Message);
        //        }
        //        else
        //        {
        //            m_Logger.LogInformation("Successfully created record in KRA Definition's table.");
        //            return Ok(response.IsSuccessful);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        m_Logger.LogError(ex.Message);
        //        return BadRequest();
        //    }
        //}

        #endregion

        #region GenerateKRAPdf
        /// <summary>
        /// GenerateKRAPdf
        /// </summary>
        /// <returns></returns>
        [HttpGet("GenerateKRAPdf")]
        public async Task<IActionResult> GenerateKRAPdf(string employeeCode, int financialYearId, int? departmentId = null, int? roleId = null)
        {
            try
            {
                AssociateKRA associateKRA = await m_KraService.GetKRAPdfData(employeeCode, financialYearId, departmentId, roleId);
                if (associateKRA == null)
                {
                    return NotFound();
                }
                else
                {
                    string webRootPath = m_webHostEnvironment.WebRootPath;
                    m_KraService.GeneratePdfFiles(associateKRA, webRootPath);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion     

        #region GetKRAPdfs
        /// <summary>
        /// Gets KRA Operator
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetKRAPdfs")]
        public async Task<IActionResult> GetKRAPdfsAsync(bool IsActive = true)
        {
            try
            {
                var kraPdfs = await m_KraService.GetKRAPdfsAsync(IsActive);
                if (kraPdfs == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(kraPdfs);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region DeleteKRAPdf
        /// <summary>
        /// Delete a KRA Pdf
        /// </summary>
        /// <param name="kraPdfId"></param>
        /// <returns></returns>
        [HttpPost("DeleteKRAPdf")]
        public async Task<IActionResult> DeleteKRAPdf(string kraPdfId)
        {
            m_Logger.LogInformation("Delete records in Definition tables.");

            try
            {
                var response = await m_KraService.DeleteKRAPdfAsync(new Guid(kraPdfId));
                if (response)
                {                    
                    m_Logger.LogError("Error occurred while deleting a KRA Pdf");
                    return Ok(false);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted a KRA Pdf.");
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while deleting a KRA definition: " + ex);

                return BadRequest("Error occurred while deleting a KRA definition.");
            }
        }
        #endregion
    }
}
