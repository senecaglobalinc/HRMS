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
    public class HolidayController : Controller
    {
        #region Global Variables

        private readonly IHolidayService m_HolidayService;
        private readonly ILogger<HolidayController> m_Logger;

        #endregion

        #region Constructor

        public HolidayController(IHolidayService holidayService, ILogger<HolidayController> logger)
        {
            m_HolidayService = holidayService;
            m_Logger = logger;
        }

        #endregion

        #region  Create
        /// <summary>
        /// CreateHoliday
        /// </summary>
        /// <param name="holidayIn"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Holiday holidayIn)
        {
            m_Logger.LogInformation("Inserting record in Holiday table.");
            try
            {
                dynamic response = await m_HolidayService.Create(holidayIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully created record in Holiday table.");
                    return Ok(response.Holiday);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while creating Holiday: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Holiday: " + holidayIn.Occasion);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while creating Holiday: " + ex);
                return BadRequest("Error occurred while creating Holiday");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetHolidays
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            m_Logger.LogInformation("Retrieving records from Holiday table.");

            try
            {
                var Holidays = await m_HolidayService.GetAll();
                if (Holidays == null)
                {
                    m_Logger.LogInformation("No records found in Holiday table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { Holidays.Count} Holiday.");
                    return Ok(Holidays);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Holiday.");
            }
        }
        #endregion

        #region GetByHolidayId
        /// <summary>
        /// Gets the Holiday by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<ActionResult<Holiday>> GetByHolidayId(int Id)
        {
            m_Logger.LogInformation($"Retrieving records from Holiday table by {Id}.");

            try
            {
                var Holiday = await m_HolidayService.GetByHolidayId(Id);
                if (Holiday == null)
                {
                    m_Logger.LogInformation($"No records found for Holiday {Id}.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"records found for Holiday {Id}.");
                    return Ok(Holiday);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetHolidaysForDropdown
        /// <summary>
        /// GetHolidaysForDropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHolidaysForDropdown")]
        public async Task<ActionResult<IEnumerable>> GetHolidaysForDropdown()
        {
            m_Logger.LogInformation("Retrieving records from Holiday table.");

            try
            {
                var Holidays = await m_HolidayService.GetHolidaysForDropdown();
                if (Holidays == null)
                {
                    m_Logger.LogInformation("No records found in Holiday table.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"Returning { Holidays.Count} Holiday.");
                    return Ok(Holidays);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return BadRequest("Error occurred while fetching Holiday.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// UpdateHoliday
        /// </summary>
        /// <param name="holidayIn"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update(Holiday holidayIn)
        {
            m_Logger.LogInformation("updating record in Holiday table.");
            try
            {
                dynamic response = await m_HolidayService.Update(holidayIn);
                if (response.IsSuccessful)
                {
                    m_Logger.LogInformation("Successfully updated record in Holiday table.");
                    return Ok(response.Holiday);
                }
                else
                {
                    //Add exeption to logger
                    m_Logger.LogError("Error occurred while updating Holiday: " + (string)response.Message);
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                //Extra information
                m_Logger.LogError("Holiday: " + holidayIn.Occasion);

                //Add exeption to logger
                m_Logger.LogError("Error occurred while updating Holiday: " + ex);
                return BadRequest("Error occurred while updating Holiday");
            }
        }
        #endregion

        #region GetHolidayIdByName
        /// <summary>
        /// Gets HolidayId By Name
        /// </summary>
        /// <param name="holidayName"></param>
        /// <returns></returns>
        [HttpGet("GetHolidayIdByName")]
        public async Task<ActionResult<int>> GetHolidayIdByName(string holidayName)
        {
            m_Logger.LogInformation($"Retrieving records from Holiday table by {holidayName}.");

            try
            {
                var holiday = await m_HolidayService.GetHolidayIdByName(holidayName);
                if (!holiday.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found in exit type table on {holidayName}.");

                    return NotFound(holiday.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found for {holidayName}.");

                    return Ok(holiday.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetHolidayIdByName() method in HolidayController:" + ex.StackTrace);

                return BadRequest("Error Occured in GetHolidayIdByName() method in HolidayController:");
            }

        }
        #endregion
    }
}