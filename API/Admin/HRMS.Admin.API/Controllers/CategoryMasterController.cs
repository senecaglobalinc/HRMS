using HRMS.Admin.Entities;
using HRMS.Admin.Types;
using HRMS.Admin.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace HRMS.Admin.API.Controllers
{
    [Route("admin/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryMasterController : ControllerBase
    {
        #region Global Variables

        private readonly ICategoryMasterService m_CategoryMasterService;
        private readonly ILogger<CategoryMasterController> m_Logger; 

        #endregion

        #region Constructor
        public CategoryMasterController(ICategoryMasterService categoryMasterService,
            ILogger<CategoryMasterController> logger)
        {
            m_CategoryMasterService = categoryMasterService;
            m_Logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// This method creates new category master.
        /// </summary>
        /// <param name="categoryMasterIn"></param>
        /// <returns>CategoryMaster</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CategoryMaster categoryMasterIn)
        {
            m_Logger.LogInformation("Inserting record in category master's table.");
            try
            {
                dynamic response = await m_CategoryMasterService.Create(categoryMasterIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating category master: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully created record in category master's table.");
                    return Ok(response.CategoryMaster);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Category name: " + categoryMasterIn.CategoryName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while category master: " + ex);

                return BadRequest("Error occurred while creating category master.");
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// This methode deletes category master.
        /// </summary>
        /// <param name="categoryMasterID"></param>
        /// <returns>bool</returns>
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int categoryMasterID)
        {
            m_Logger.LogInformation("Deleting record in category master's table.");

            try
            {
                dynamic response = await m_CategoryMasterService.Delete(categoryMasterID);
                if (!response.IsSuccessful)
                {
                    //Extra information
                    m_Logger.LogError("category ID: " + categoryMasterID);

                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while deleting category master: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully deleted record in category master's table.");
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("category ID: " + categoryMasterID);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while deleting category master: " + ex);

                return BadRequest("Error occurred while deleting category master.");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets category master's
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns>List<CategoryMaster></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll(bool? isActive)
        {
            m_Logger.LogInformation("Retrieving records from category master's table.");

            try
            {
                var categories = await m_CategoryMasterService.GetAll(isActive);
                if (categories == null)
                {
                    m_Logger.LogInformation("No records found in category master's table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { categories.Count} category master.");
                    return Ok(categories);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching category master's.");
            }
        }
        #endregion

        #region GetByCategoryName
        /// <summary>
        /// Gets category master's
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns>CategoryMaster</returns>
        [HttpGet("GetByCategoryName")]
        public async Task<ActionResult<IEnumerable>> GetByCategoryName(string categoryName)
        {
            m_Logger.LogInformation("Retrieving records from category master's table by using category name.");

            try
            {
                var categories = await m_CategoryMasterService.GetByCategoryName(categoryName);
                if (categories == null)
                {
                    m_Logger.LogInformation("No records found in category master's table by using category name.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning category master by using category name.");
                    return Ok(categories);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching category master's by using category name.");
            }
        }
        #endregion

        #region GetParentCategoies
        /// <summary>
        /// Gets category master's
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns>List<CategoryMaster></returns>
        [HttpGet("GetParentCategoies")]
        public async Task<ActionResult<IEnumerable>> GetParentCategoies()
        {
            m_Logger.LogInformation("Retrieving parent categories records from category master's table.");

            try
            {
                var categories = await m_CategoryMasterService.GetParentCategoies();
                if (categories == null)
                {
                    m_Logger.LogInformation("No records found in parent categories.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { categories.Count} parent categories.");
                    return Ok(categories);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching parent categories.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// This method updates category master details.
        /// </summary>
        /// <param name="categoryMasterIn"></param>
        /// <returns>NotificationType</returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(CategoryMaster categoryMasterIn)
        {
            m_Logger.LogInformation("Updating record in category master's table.");

            try
            {
                dynamic response = await m_CategoryMasterService.Update(categoryMasterIn);
                if (!response.IsSuccessful)
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating category master: " + (string)response.Message);

                    return BadRequest(response.Message);
                }
                else
                {
                    m_Logger.LogInformation("Successfully updated record in category master table.");
                    return Ok(response.CategoryMaster);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Category name: " + categoryMasterIn.CategoryName);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating category master: " + ex);

                return BadRequest("Error occurred while updating category master.");
            }
        }
        #endregion
    }
}