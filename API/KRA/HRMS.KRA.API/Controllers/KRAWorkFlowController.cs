using System;
using System.Collections.Generic;
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
    public class KRAWorkFlowController : ControllerBase
    {
        #region Global Variables

        private readonly IKRAWorkFlowService m_kraWorkFlowService;
        private readonly ILogger<KRAWorkFlowController> m_Logger;

        #endregion

        #region Constructor
        public KRAWorkFlowController(IKRAWorkFlowService userStatusService,
            ILogger<KRAWorkFlowController> logger)
        {
            m_kraWorkFlowService = userStatusService;
            m_Logger = logger;
        }
        #endregion

        #region UpdateDefinitionDetails 
        /// <summary>
        /// Update DefinitionDetails -- SEND TO HOD
        /// </summary>
        ///  <param name="financialYearId"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpPost("UpdateDefinitionDetails/{financialYearId}/{departmentId}")]
        public async Task<IActionResult> UpdateDefinitionDetails(int financialYearId, int departmentId)
        {
            m_Logger.LogInformation("Updating records in DefinitionDetails table.");

            try
            {
                var response = await m_kraWorkFlowService.UpdateDefinitionDetails(financialYearId, departmentId);

                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating definition details : " + response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated records in DefinitionDetails table.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating Definition details: " + ex);

                return BadRequest("Error occurred while updating Definition details.");
            }
        }
        #endregion

        #region KRAWorkFlow
        /// <summary>
        /// "Send To HOD" by Operations Head
        /// </summary>
        /// <param name="kRAWorkFlowModel"></param>
        /// <returns></returns>
        [HttpPost("SendTOHod")]
        public async Task<IActionResult> SentToHODAsync(KRAWorkFlowModel kRAWorkFlowModel)
        {
            m_Logger.LogInformation($"KRAWorkFlowController: Calling {nameof(SentToHODAsync)} method.");

            var response = await m_kraWorkFlowService.SentToHODAsync(kRAWorkFlowModel);

            if (!response.IsSuccessful)
            {
                m_Logger.LogError($"{nameof(SentToHODAsync)}: Error occurred while calling KRAWorkFlow : {response.Message}");

                return BadRequest(response.Message);
            }
            else
            {
                m_Logger.LogInformation($"{nameof(SentToHODAsync)}: Successfully added record in KRAWorkFlow table.");
                
                return Ok(response.Message);
            }
        }

        /// <summary>
        /// "Approved by HOD" KRA Definitions
        /// </summary>
        /// <param name="kRAWorkFlowModel"></param>
        /// <returns></returns>
        [HttpPost("ApprovedbyHOD")]
        public async Task<IActionResult> ApprovedbyHODAsync(KRAWorkFlowModel kRAWorkFlowModel)
        {
            m_Logger.LogInformation($"KRAWorkFlowController: Calling {nameof(ApprovedbyHODAsync)} method.");

            var response = await m_kraWorkFlowService.ApprovedbyHODAsync(kRAWorkFlowModel);

            if (!response.IsSuccessful)
            {
                m_Logger.LogError($"{nameof(ApprovedbyHODAsync)}: Error occurred while calling KRAWorkFlow : {response.Message}");

                return BadRequest(response.Message);
            }
            else
            {
                m_Logger.LogInformation($"{nameof(ApprovedbyHODAsync)}: Successfully added record in KRAWorkFlow table.");
                
                return Ok(response.Message);
            }
        }

        /// <summary>
        /// "Edited by HOD" KRA Definitions
        /// </summary>
        /// <param name="kRAWorkFlowModel"></param>
        /// <returns></returns>
        [HttpPost("EditedByHOD")]
        public async Task<IActionResult> EditedByHODAsync(KRAWorkFlowModel kRAWorkFlowModel)
        {
            m_Logger.LogInformation($"KRAWorkFlowController: Calling {nameof(EditedByHODAsync)} method.");

            var response = await m_kraWorkFlowService.EditedByHODAsync(kRAWorkFlowModel);

            if (!response.IsSuccessful)
            {
                m_Logger.LogError($"{nameof(EditedByHODAsync)}: Error occurred while calling KRAWorkFlow : {response.Message}");

                return BadRequest(response.Message);
            }
            else
            {
                m_Logger.LogInformation($"{nameof(EditedByHODAsync)}: Successfully added record in KRAWorkFlow table.");
                
                return Ok(response.Message);
            }
        }

        /// <summary>
        /// "Send To OpHead" KRA Definitions
        /// </summary>
        /// <param name="kRAWorkFlowModel"></param>
        /// <returns></returns>
        [HttpPost("SentToOpHead")]
        public async Task<IActionResult> SentToOpHeadAsync(KRAWorkFlowModel kRAWorkFlowModel)
        {
            m_Logger.LogInformation($"KRAWorkFlowController: Calling {nameof(SentToOpHeadAsync)} method.");

            var response = await m_kraWorkFlowService.SentToOpHeadAsync(kRAWorkFlowModel);

            if (!response.IsSuccessful)
            {
                m_Logger.LogError($"{nameof(SentToOpHeadAsync)}: Error occurred while calling KRAWorkFlow : {response.Message}");

                return BadRequest(response.Message);
            }
            else
            {
                m_Logger.LogInformation($"{nameof(SentToOpHeadAsync)}: Successfully added record in KRAWorkFlow table.");
                
                return Ok(response.Message);
            }
        }

        /// <summary>
        /// "Send To CEO" KRA Definitions
        /// </summary>
        /// <param name="kRAWorkFlowModel"></param>
        /// <returns></returns>
        [HttpPost("SendToCEO")]
        public async Task<IActionResult> SendToCEOAsync(KRAWorkFlowModel kRAWorkFlowModel)
        {
            m_Logger.LogInformation($"KRAWorkFlowController: Calling {nameof(SendToCEOAsync)} method.");

            var response = await m_kraWorkFlowService.SendToCEOAsync(kRAWorkFlowModel);

            if (!response.IsSuccessful)
            {
                m_Logger.LogError($"{nameof(SendToCEOAsync)}: Error occurred while calling KRAWorkFlow : {response.Message}");

                return BadRequest(response.Message);
            }
            else
            {
                m_Logger.LogInformation($"{nameof(SendToCEOAsync)}: Successfully added record in KRAWorkFlow table.");
                
                return Ok(response.Message);
            }
        }

        /// <summary>
        /// "Approved By CEO" KRA Definitions
        /// </summary>
        /// <param name="kRAWorkFlowModel"></param>
        /// <returns></returns>
        [HttpPost("ApprovedByCEO")]
        public async Task<IActionResult> ApprovedByCEOAsync(KRAWorkFlowModel kRAWorkFlowModel)
        {
            m_Logger.LogInformation($"KRAWorkFlowController: Calling {nameof(ApprovedByCEOAsync)} method.");

            var response = await m_kraWorkFlowService.ApprovedByCEOAsync(kRAWorkFlowModel);

            if (!response.IsSuccessful)
            {
                m_Logger.LogError($"{nameof(ApprovedByCEOAsync)}: Error occurred while calling KRAWorkFlow : {response.Message}");

                return BadRequest(response.Message);
            }
            else
            {
                m_Logger.LogInformation($"{nameof(ApprovedByCEOAsync)}: Successfully added record in KRAWorkFlow table.");
                
                return Ok(response.IsSuccessful);
            }
        }

        /// <summary>
        /// HOD Adds KRA Definitions
        /// </summary>
        /// <param name="definitionModel"></param>
        /// <returns></returns>
        [HttpPost("HODAdd")]
        public async Task<IActionResult> HODAddAsync(DefinitionModel definitionModel)
        {
            m_Logger.LogInformation($"KRAWorkFlowController: Calling {nameof(HODAddAsync)} method.");

            var response = await m_kraWorkFlowService.HODAddAsync(definitionModel);

            if (!response.IsSuccessful)
            {
                m_Logger.LogError($"{nameof(HODAddAsync)}: Error occurred while adding definition in DefinitionTransaction table : {response.Message}");

                return BadRequest(response.Message);
            }
            else
            {
                m_Logger.LogInformation($"{nameof(HODAddAsync)}: Successfully added definition in DefinitionTransaction table.");

                return Ok(response.Message);
            }
        }

        /// <summary>
        /// HOD Updates KRA Definitions
        /// </summary>
        /// <param name="definitionModel"></param>
        /// <returns></returns>
        [HttpPost("HODUpdate")]
        public async Task<IActionResult> HODUpdateAsync(DefinitionModel definitionModel)
        {
            m_Logger.LogInformation($"KRAWorkFlowController: Calling {nameof(HODUpdateAsync)} method.");

            var response = await m_kraWorkFlowService.HODUpdateAsync(definitionModel);

            if (!response.IsSuccessful)
            {
                m_Logger.LogError($"{nameof(HODUpdateAsync)}: Error occurred while updating definition in DefinitionTransaction table : {response.Message}");

                return BadRequest(response.Message);
            }
            else
            {
                m_Logger.LogInformation($"{nameof(HODUpdateAsync)}: Successfully updated definition in DefinitionTransaction table.");

                return Ok(response.Message);
            }
        }

        /// <summary>
        /// HOD Deletes KRA Definitions
        /// </summary>
        /// <param name="definitionModel"></param>
        /// <returns></returns>
        [HttpPost("HODDelete")]
        public async Task<IActionResult> HODDeleteAsync(DefinitionModel definitionModel)
        {
            m_Logger.LogInformation($"KRAWorkFlowController: Calling {nameof(HODDeleteAsync)} method.");

            var response = await m_kraWorkFlowService.HODDeleteAsync(definitionModel);

            if (!response.IsSuccessful)
            {
                m_Logger.LogError($"{nameof(HODDeleteAsync)}: Error occurred while deleting definition in DefinitionTransaction table : {response.Message}");

                return BadRequest(response.Message);
            }
            else
            {
                m_Logger.LogInformation($"{nameof(HODDeleteAsync)}: Successfully deleted definition in DefinitionTransaction table.");

                return Ok(response.Message);
            }
        }

        /// <summary>
        /// Get KRA Definitions for HOD
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <param name="roleTypeId"></param>
        /// <returns></returns>
        [HttpGet("GetHODDefinitions")]
        public async Task<IActionResult> GetHODDefinitionsAsync(int financialYearId, int roleTypeId)
        {
            m_Logger.LogInformation($"KRAWorkFlowController: Calling {nameof(GetHODDefinitionsAsync)} method.");

            var response = await m_kraWorkFlowService.GetHODDefinitionsAsync(financialYearId, roleTypeId);

            if (!response.IsSuccessful)
            {
                m_Logger.LogError($"{nameof(GetHODDefinitionsAsync)}: Error occurred while getting KRA definitions : {response.Message}");

                return BadRequest(response.Message);
            }
            else
            {
                m_Logger.LogInformation($"{nameof(GetHODDefinitionsAsync)}: KRA definition call success.");

                return Ok(response.Items);
            }
        }

        /// <summary>
        /// GetOperationHeadStatus
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <returns></returns>
        [HttpGet("GetOperationHeadStatus")]
        public async Task<IActionResult> GetOperationHeadStatusAsync(int financialYearId)
        {
            m_Logger.LogInformation($"KRAWorkFlowController: Calling {nameof(GetOperationHeadStatusAsync)} method.");

            var response = await m_kraWorkFlowService.GetOperationHeadStatusAsync(financialYearId);

            if (!response.IsSuccessful)
            {
                m_Logger.LogError($"{nameof(GetOperationHeadStatusAsync)}: Error occurred while getting KRA status : {response.Message}");

                return BadRequest(response.Message);
            }
            else
            {
                m_Logger.LogInformation($"{nameof(GetOperationHeadStatusAsync)}: KRA status call success");

                return Ok(response.Items);
            }
        }

        /// <summary>
        /// "Accepted By OperationHead" KRA Definitions
        /// </summary>
        /// <param name="definitionModel"></param>
        /// <returns></returns>
        [HttpPost("AcceptedByOperationHead")]
        public async Task<IActionResult> AcceptedByOperationHeadAsync(DefinitionModel definitionModel)
        {
            m_Logger.LogInformation($"KRAWorkFlowController: Calling {nameof(AcceptedByOperationHeadAsync)} method.");

            var response = await m_kraWorkFlowService.AcceptedByOperationHeadAsync(definitionModel);

            if (!response.IsSuccessful)
            {
                m_Logger.LogError($"{nameof(AcceptedByOperationHeadAsync)}: Error occurred while accpeting KRA(s) by head of operations : {response.Message}");

                return BadRequest(response.Message);
            }
            else
            {
                m_Logger.LogInformation($"{nameof(AcceptedByOperationHeadAsync)}: Successfully accepted  KRA(s) by head of operations.");

                return Ok(response.Message);
            }
        }

        /// <summary>
        /// "Rejected By OperationHead" KRA Definitions
        /// </summary>
        /// <param name="definitionModel"></param>
        /// <returns></returns>
        [HttpPost("RejectedByOperationHead")]
        public async Task<IActionResult> RejectedByOperationHeadAsync(DefinitionModel definitionModel)
        {
            m_Logger.LogInformation($"KRAWorkFlowController: Calling {nameof(RejectedByOperationHeadAsync)} method.");

            var response = await m_kraWorkFlowService.RejectedByOperationHeadAsync(definitionModel);

            if (!response.IsSuccessful)
            {
                m_Logger.LogError($"{nameof(RejectedByOperationHeadAsync)}: Error occurred while rejecting KRA(s) by head of operations : {response.Message}");

                return BadRequest(response.Message);
            }
            else
            {
                m_Logger.LogInformation($"{nameof(RejectedByOperationHeadAsync)}: Successfully rejected KRA(s) by head of operations.");

                return Ok(response.Message);
            }
        }

        /// <summary>
        /// Get the KRA Definitions for Operations Head
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <param name="roleTypeId"></param>
        /// <returns></returns>
        [HttpGet("GetOperationHeadDefinitions")]
        public async Task<IActionResult> GetOperationHeadDefinitionsAsync(int financialYearId, int roleTypeId)
        {
            m_Logger.LogInformation($"KRAWorkFlowController: Calling {nameof(GetOperationHeadDefinitionsAsync)} method.");

            var response = await m_kraWorkFlowService.GetOperationHeadDefinitionsAsync(financialYearId, roleTypeId);

            if (!response.IsSuccessful)
            {
                m_Logger.LogError($"{nameof(GetHODDefinitionsAsync)}: Error occurred while getting KRA definitions : {response.Message}");

                return BadRequest(response.Message);
            }
            else
            {
                m_Logger.LogInformation($"{nameof(GetOperationHeadDefinitionsAsync)}: KRA definition call success.");

                return Ok(response.Items);
            }
        }
        #endregion

        #region SendToCEO 
        /// <summary>
        /// SendToCEO
        /// </summary>
        ///  <param name="financialYearId"></param>
        /// <returns></returns>
        [HttpPost("SendToCEO/{financialYearId}")]
        public async Task<IActionResult> SendToCEO(int financialYearId)
        {
            m_Logger.LogInformation("Updating records in applicableroletype table.");
            try
            {
                var response = await m_kraWorkFlowService.SendToCEO(financialYearId);

                if (!response.IsSuccessful)
                {                   
                    m_Logger.LogError("Error occurred while updating applicableroletype table : " + response.Message);
                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated records in applicableroletype table .");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating applicableroletype table : " + ex);

                return BadRequest("Error occurred while updating applicableroletype table .");
            }
        }
        #endregion

        #region UpdateRoleTypeStatus 
        /// <summary>
        /// UpdateRoleTypeStatus -- SEND TO HR
        /// </summary>
        ///  <param name="financialYearId"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpPost("UpdateRoleTypeStatus/{financialYearId}/{departmentId}")]
        public async Task<IActionResult> UpdateRoleTypeStatus(int financialYearId, int departmentId)
        {
            m_Logger.LogInformation("Updating records in DefinitionDetails table.");

            try
            {
                var response = await m_kraWorkFlowService.UpdateRoleTypeStatus(financialYearId, departmentId);

                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating definition details : " + response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated records in DefinitionDetails table.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating Definition details: " + ex);

                return BadRequest("Error occurred while updating Definition details.");
            }
        }
        #endregion

        #region EditByHR 
        /// <summary>
        /// EditByHR
        /// </summary>
        ///  <param name="financialYearId"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpPost("EditByHR/{financialYearId}/{departmentId}")]
        public async Task<IActionResult> EditByHR(int financialYearId, int departmentId)
        {
            m_Logger.LogInformation("Updating records in approletypes table.");

            try
            {
                var response = await m_kraWorkFlowService.EditByHR(financialYearId, departmentId);

                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating in  EditByHR method: " + response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated records in roletypes table.");
                    return Ok(response.IsSuccessful);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while EditByHR method: " + ex);

                return BadRequest("Error occurred while updating EditByHR method.");
            }
        }
        #endregion


        #region GetKRAStatusByFinancialYearId
        /// <summary>
        /// Gets KRAStatus by FinancialYearId.
        /// </summary>
        ///  <param name="financialYearId"></param>
        /// <returns></returns>
        [HttpGet("GetKRAStatusByFinancialYearId/{financialYearId}")]
        public async Task<ActionResult<List<KRAStatusModel>>> GetKRAStatusByFinancialYearId(int financialYearId)
        {

            m_Logger.LogInformation($"Retrieving records from ApplicableRoleType table.");

            try
            {
                var krastatus = await m_kraWorkFlowService.GetKRAStatusByFinancialYearId(financialYearId);
                if (krastatus == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound(krastatus.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found in ApplicableRoleType table.");
                    return Ok(krastatus.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetStatusByFinancialYearId() in KRAStatusController:" + ex.StackTrace);
                return BadRequest("Error Occured in GetStatusByFinancialYearId() in KRAStatusController:");
            }
        }
        #endregion

        #region GetKRAStatus
        /// <summary>
        /// Gets KRAStatus by FinancialYearId.
        /// </summary>
        ///  <param name="financialYearId"></param>
        ///  /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpGet("GetKRAStatus/{financialYearId}/{departmentId}")]
        public async Task<ActionResult<List<KRAStatusModel>>> GetKRAStatus(int financialYearId, int departmentId)
        {

            m_Logger.LogInformation($"Retrieving records from ApplicableRoleType table.");

            try
            {
                var krastatus = await m_kraWorkFlowService.GetKRAStatus(financialYearId, departmentId);
                if (!krastatus.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found.");
                    return Ok(krastatus.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found in ApplicableRoleType table.");
                    return Ok(krastatus.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetKRAStatus() in KRAStatusController:" + ex.StackTrace);
                return BadRequest("Error Occured in GetKRAStatus() in KRAStatusController:");
            }
        }
        #endregion

        #region GetKRAStatusByFinancialYearIdForCEO
        /// <summary>
        /// Gets KRAStatus by FinancialYearId for CEO.
        /// </summary>
        ///  <param name="financialYearId"></param>
        /// <returns></returns>
        [HttpGet("GetKRAStatusByFinancialYearIdForCEO/{financialYearId}")]
        public async Task<ActionResult<List<KRAStatusModel>>> GetKRAStatusByFinancialYearIdForCEO(int financialYearId)
        {
            m_Logger.LogInformation($"Retrieving records from ApplicableRoleType table.");

            try
            {
                var krastatus = await m_kraWorkFlowService.GetKRAStatusByFinancialYearIdForCEO(financialYearId);
                if (krastatus == null)
                {
                    m_Logger.LogInformation($"No records found.");
                    return NotFound(krastatus.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found in ApplicableRoleType table.");
                    return Ok(krastatus.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetStatusByFinancialYearId() in KRAStatusController:" + ex.StackTrace);
                return BadRequest("Error Occured in GetStatusByFinancialYearId() in KRAStatusController:");
            }
        }
        #endregion

        #region UpdateRoleTypeStatusForCEO 
        /// <summary>
        /// UpdateRoleTypeStatusForCEO -- CEO Accepts/Rejects
        /// </summary>
        ///  <param name="financialYearId"></param>
        /// <param name="departmentId"></param>
        /// <param name="isAccepted"></param>
        /// <returns></returns>
        [HttpPost("UpdateRoleTypeStatusForCEO/{financialYearId}/{departmentId}/{isAccepted}")]
        public async Task<IActionResult> UpdateRoleTypeStatusForCEO(int financialYearId, int departmentId, bool isAccepted)
        {
            m_Logger.LogInformation("Updating records in Definition table.");

            try
            {
                var response = await m_kraWorkFlowService.UpdateRoleTypeStatusForCEO(financialYearId, departmentId, isAccepted);

                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating definition table : " + response.Message);
                    return Ok(response);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated records in Definition table.");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating Definition table: " + ex);

                return BadRequest("Error occurred while updating Definition table.");
            }
        }
        #endregion
    }
}

 