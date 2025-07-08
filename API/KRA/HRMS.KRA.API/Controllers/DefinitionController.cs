using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Types;
using HRMS.KRA.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.KRA.API.Controllers
{
    [Authorize]
    [Route("kra/api/v1/[controller]")]
    [ApiController]
    public class DefinitionController : ControllerBase
    {
        #region Global Variables

        private readonly IDefinitionService m_DefinitionService;
        private readonly ILogger<DefinitionController> m_Logger;

        #endregion

        #region Constructor

        /// <summary>
        /// DefinitionController
        /// </summary>
        /// <param name="definitionService"></param>
        /// <param name="logger"></param>
        public DefinitionController(IDefinitionService definitionService,
            ILogger<DefinitionController> logger)
        {
            m_DefinitionService = definitionService;
            m_Logger = logger;
        }

        #endregion

        #region Create

        /// <summary>
        /// Creates a KRA Definition record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(DefinitionModel model)
        {
            m_Logger.LogInformation("Inserting record in Definition table.");
            try
            {
                var response = await m_DefinitionService.Create(model);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating Definition record: " + response.Message);

                    return NotFound(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in Definition table.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Definition name: " + model.DefinitionId);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating a new Definition record: " + ex);

                return BadRequest("Error occurred while creating a new Definition record.");
            }
        }
        #endregion

        #region GetDefinition
        /// <summary>
        /// Gets the Definitions based on FinancialYearId and RoleTypeId
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <param name="roleTypeId"></param>
        /// <returns></returns>
        [HttpGet("GetDefinitions")]
        public async Task<ActionResult<List<KRAModel>>> GetDefinitions(int financialYearId, int roleTypeId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from Definition table.");

            try
            {
                var definitions = await m_DefinitionService.GetDefinitions(financialYearId, roleTypeId);
                if (!definitions.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetDefinitions() in DefinitionController:" + stopwatch.Elapsed);

                    return Ok(definitions.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found in Definition.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetDefinitions() in DefinitionController:" + stopwatch.Elapsed);

                    return Ok(definitions.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetDefinitions() in DefinitionController:" + ex.StackTrace);

                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetDefinitions() in DefinitionController:" + stopwatch.Elapsed);

                return BadRequest("Error Occured in GetDefinitions() in DefinitionController:");
            }
        }
        #endregion 

        #region Update Definition Status
        /// <summary>
        /// Update KRA Status
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <param name="roleTypeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost("UpdateKRAStatus")]
        public async Task<IActionResult> UpdateKRAStatus(int financialYearId, int roleTypeId, string status)
        {
            m_Logger.LogInformation("Updating record.");
            try
            {
                var response = await m_DefinitionService.UpdateKRAStatus(financialYearId, roleTypeId, status);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating record : " + response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully update record.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating record : " + ex.Message);

                return BadRequest("Error occurred while updating record.");
            }
        }
        #endregion

        #region UpdateKRA
        /// <summary>
        /// Updates a KRA Definition
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateKRA(DefinitionModel model)
        {
            m_Logger.LogInformation("Updating record.");
            try
            {
                var response = await m_DefinitionService.UpdateKRA(model);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating a kra record : " + response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully update a kra record.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating a kra record : " + ex.Message);

                return BadRequest("Error occurred while updating a kra record.");
            }
        }
        #endregion

        #region ImportKRA

        /// <summary>
        /// ImportKRA
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ImportKRA")]
        public async Task<IActionResult> ImportKRA(DefinitionModel model)
        {
            m_Logger.LogInformation("Inserting record in Definition table.");
            try
            {
                var response = await m_DefinitionService.ImportKRA(model);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating Definition record: " + response.Message);

                    return NotFound(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in Definition table.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Definition name: " + model.DefinitionId);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating a new Definition record: " + ex);

                return BadRequest("Error occurred while creating a new Definition record.");
            }
        }
        #endregion

        #region GetKRAs
        /// <summary>
        /// Gets KRAs.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetKRAs")]
        public async Task<ActionResult<List<KRAModel>>> GetKRAs(int financialYearId, int departmentId, int gradeId, int roleTypeId, bool IsHOD)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from Definition Transaction table.");

            try
            {
                var kras = await m_DefinitionService.GetKRAs(financialYearId, departmentId, gradeId, roleTypeId, IsHOD);
                if (!kras.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetKRAs() in DefinitionController:" + stopwatch.Elapsed);

                    return Ok(kras.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found in Definition Transaction.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetKRAs() in DefinitionController:" + stopwatch.Elapsed);

                    return Ok(kras.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetKRAs() in DefinitionController:" + ex.StackTrace);

                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetKRAs() in DefinitionController:" + stopwatch.Elapsed);

                return BadRequest("Error Occured in GetKRAs() in DefinitionController:");
            }
        }
        #endregion

        #region GetDefinitionDetails
        /// <summary>
        /// GetDefinitionDetails.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDefinitionDetails")]
        public async Task<ActionResult<List<DefinitionModel>>> GetDefinitionDetails(Guid Id)
        {           

            m_Logger.LogInformation($"Retrieving records from Definition Transaction table.");

            try
            {
                var kras = await m_DefinitionService.GetDefinitionDetails(Id);
                if (!kras.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found.");  
                    return NotFound(kras.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found in Definition Transaction.");
                    return Ok(kras.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetKRAs() in DefinitionController:" + ex.StackTrace);
                return BadRequest("Error Occured in GetKRAs() in DefinitionController:");
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete a KRA Definition
        /// </summary>
        /// <param name="definitionId"></param>
        /// <returns></returns>
        [HttpPost("Delete/{definitionId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid definitionId)
        {
            m_Logger.LogInformation("Delete records in Definition tables.");

            try
            {
                var response = await m_DefinitionService.Delete(definitionId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while deleting a KRA Definition: " + (string)response.Message);

                    return Ok(false);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted a KRA Definition.");
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

        #region DeleteByHOD
        /// <summary>
        /// Delete (soft delete) a KRA Definition by HOD
        /// </summary>
        /// <param name="defintionDetailId"></param>
        /// <returns></returns>
        [HttpPost("DeleteByHOD")]
        public async Task<IActionResult> DeleteByHOD(int defintionDetailId)
        {
            m_Logger.LogInformation("Calling DeleteByHOD.");
            try
            {
                var response = await m_DefinitionService.DeleteByHOD(defintionDetailId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while deleting a kra record by HOD: " + response.Message);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted a kra record.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while deleting a kra record by HOD : " + ex.Message);

                return BadRequest("Error occurred while deleting a kra record by HOD.");
            }
        }
        #endregion

        #region SetPreviousMetricValues
        /// <summary>
        /// SetPreviousMetricValues
        /// </summary>
        /// <param name="defintionDetailId"></param>
        /// <returns></returns>
        [HttpPost("SetPreviousMetricValues")]
        public async Task<IActionResult> SetPreviousMetricValues(int defintionDetailId)
        {
            m_Logger.LogInformation("Calling SetPreviousMetricValues.");
            try
            {
                var response = await m_DefinitionService.SetPreviousMetricValues(defintionDetailId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating the metric values: " + response.Message);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated a kra record.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating the metric values : " + ex.Message);

                return BadRequest("Error occurred while updating the metric values.");
            }
        }
        #endregion

        #region SetPreviousTargetValues
        /// <summary>
        /// SetPreviousTargetValues
        /// </summary>
        /// <param name="defintionDetailId"></param>
        /// <returns></returns>
        [HttpPost("SetPreviousTargetValues")]
        public async Task<IActionResult> SetPreviousTargetValues(int defintionDetailId)
        {
            m_Logger.LogInformation("Calling SetPreviousTargetValues.");
            try
            {
                var response = await m_DefinitionService.SetPreviousTargetValues(defintionDetailId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating the target values: " + response.Message);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated a kra record.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating the target values : " + ex.Message);

                return BadRequest("Error occurred while updating the target values.");
            }
        }
        #endregion
        
        #region AcceptTargetValue
        /// <summary>
        /// AcceptTargetValue
        /// </summary>
        /// <param name="defintionTransactionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("AcceptTargetValue")]
        public async Task<IActionResult> AcceptTargetValue(int defintionTransactionId, string username)
        {
            m_Logger.LogInformation("Calling AcceptTargetValue.");
            try
            {
                var response = await m_DefinitionService.AcceptTargetValue(defintionTransactionId, username);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating the metric values: " + response.Message);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated a kra record.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating the metric values : " + ex.Message);

                return BadRequest("Error occurred while updating the metric values.");
            }
        }
        #endregion

        #region AcceptMetricValue
        /// <summary>
        /// AcceptMetricValue
        /// </summary>
        /// <param name="defintionTransactionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("AcceptMetricValue")]
        public async Task<IActionResult> AcceptMetricValue(int defintionTransactionId, string username)
        {
            m_Logger.LogInformation("Calling AcceptMetricValue.");
            try
            {
                var response = await m_DefinitionService.AcceptMetricValue(defintionTransactionId,username);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating the metric values: " + response.Message);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated a metric value.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating the metric values : " + ex.Message);

                return BadRequest("Error occurred while updating the metric values.");
            }
        }
        #endregion

        #region RejectTargetValue
        /// <summary>
        /// RejectTargetValue
        /// </summary>
        /// <param name="defintionTransactionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("RejectTargetValue")]
        public async Task<IActionResult> RejectTargetValue(int defintionTransactionId, string username)
        {
            m_Logger.LogInformation("Calling RejectTargetValue.");
            try
            {
                var response = await m_DefinitionService.RejectTargetValue(defintionTransactionId,username);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating the target values: " + response.Message);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated a target value.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating the target values : " + ex.Message);

                return BadRequest("Error occurred while updating the target values.");
            }
        }
        #endregion

        #region RejectMetricValue
        /// <summary>
        /// RejectMetricValue
        /// </summary>
        /// <param name="defintionTransactionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("RejectMetricValue")]
        public async Task<IActionResult> RejectMetricValue(int defintionTransactionId, string username)
        {
            m_Logger.LogInformation("Calling RejectMetricValue.");
            try
            {
                var response = await m_DefinitionService.RejectMetricValue(defintionTransactionId,username);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating the metric values: " + response.Message);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated a kra record.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating the metric values : " + ex.Message);

                return BadRequest("Error occurred while updating the metric values.");
            }
        }
        #endregion
       
        #region AcceptDeletedKRAByHOD
        /// <summary>
        /// AcceptDeletedKRAByHOD
        /// </summary>
        /// <param name="defintionTransactionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("AcceptDeletedKRAByHOD")]
        public async Task<IActionResult> AcceptDeletedKRAByHOD(int defintionTransactionId, string username)
        {
            m_Logger.LogInformation("Calling AcceptDeletedKRAByHOD.");
            try
            {
                var response = await m_DefinitionService.AcceptDeletedKRAByHOD(defintionTransactionId, username);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating the kra record: " + response.Message);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated a kra record.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating the kra record : " + ex.Message);
                return BadRequest("Error occurred while updating the kra record.");
            }
        }
        #endregion

        #region RejectDeletedKRAByHOD
        /// <summary>
        /// RejectDeletedKRAByHOD
        /// </summary>
        /// <param name="defintionTransactionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("RejectDeletedKRAByHOD")]
        public async Task<IActionResult> RejectDeletedKRAByHOD(int defintionTransactionId, string username)
        {
            m_Logger.LogInformation("Calling RejectDeletedKRAByHOD.");
            try
            {
                var response = await m_DefinitionService.RejectDeletedKRAByHOD(defintionTransactionId, username);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating the kra record: " + response.Message);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated a kra record.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating the kra record : " + ex.Message);
                return BadRequest("Error occurred while updating the kra record.");
            }
        }
        #endregion

        #region AcceptAddedKRAByHOD
        /// <summary>
        /// AcceptAddedKRAByHOD
        /// </summary>
        /// <param name="defintionTransactionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("AcceptAddedKRAByHOD")]
        public async Task<IActionResult> AcceptAddedKRAByHOD(int defintionTransactionId, string username)
        {
            m_Logger.LogInformation("Calling AcceptAddedKRAByHOD.");
            try
            {
                var response = await m_DefinitionService.AcceptAddedKRAByHOD(defintionTransactionId, username);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating the kra record: " + response.Message);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated a kra record.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating the kra record : " + ex.Message);

                return BadRequest("Error occurred while updating the kra record.");
            }
        }
        #endregion

        #region RejectAddedKRAByHOD
        /// <summary>
        /// RejectAddedKRAByHOD
        /// </summary>
        /// <param name="defintionTransactionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("RejectAddedKRAByHOD")]
        public async Task<IActionResult> RejectAddedKRAByHOD(int defintionTransactionId, string username)
        {
            m_Logger.LogInformation("Calling RejectAddedKRAByHOD.");
            try
            {
                var response = await m_DefinitionService.RejectAddedKRAByHOD(defintionTransactionId, username);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating the kra record: " + response.Message);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated a kra record.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating the kra record : " + ex.Message);
                return BadRequest("Error occurred while updating the kra record.");
            }
        }
        #endregion

        #region DeleteByHOD- Hard-delete
        /// <summary>
        /// Delete a KRA Definition
        /// </summary>
        /// <param name="definitionDetailId"></param>
        /// <returns></returns>
        [HttpPost("DeleteKRA")]
        public async Task<IActionResult> DeleteKRA(int definitionDetailId)
        {
            m_Logger.LogInformation("Delete records in Definition tables.");

            try
            {
                var response = await m_DefinitionService.DeleteKRA(definitionDetailId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while deleting a KRA Definition: " + (string)response.Message);
                    return Ok(false);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted a KRA Definition.");
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

        #region AddKRAAgain
        /// <summary>
        /// AddKRAAgain
        /// </summary>
        /// <param name="defintionDetailId"></param>
        /// <returns></returns>
        [HttpPost("AddKRAAgain")]
        public async Task<IActionResult> AddKRAAgain(int defintionDetailId)
        {
            m_Logger.LogInformation("Calling AddKRAAgain.");
            try
            {
                var response = await m_DefinitionService.AddKRAAgain(defintionDetailId);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating the kra record: " + response.Message);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated a kra record.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating the kra record : " + ex.Message);

                return BadRequest("Error occurred while updating the kra record.");
            }
        }
        #endregion
    }

}
