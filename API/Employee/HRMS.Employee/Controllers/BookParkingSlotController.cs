using HRMS.Employee.Infrastructure;
using HRMS.Employee.Types;
using HRMS.Employee.API.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class BookParkingSlotController : Controller
    {
        #region Global Variables

        private readonly IBookParkingSlot m_BookParkingSlotService;
        private readonly ILogger<BookParkingSlotController> m_Logger;

        #endregion

        #region Constructor
        public BookParkingSlotController(IBookParkingSlot bookParkingSlot, ILogger<BookParkingSlotController> logger)
        {
            m_BookParkingSlotService = bookParkingSlot;
            m_Logger = logger;
        }
        #endregion

        #region Create       
        /// <summary>
        /// Book a parking slot of an associate for a day
        /// </summary>
        /// <param name="bookedParkingSlots"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(BookedParkingSlotDetails bookedParkingSlots)
        {
            try
            {                
                var parkingdetails = await m_BookParkingSlotService.Create(bookedParkingSlots);
                if (!parkingdetails.IsSuccessful)
                {
                    m_Logger.LogInformation("No Slots are available for the today.");
                    //return NotFound(parkingdetails.Message);
                    return StatusCode(StatusCodes.Status200OK);
                }
                else
                {
                    m_Logger.LogInformation($"Returning { parkingdetails } BookedParkingSlots details.");                    
                    return StatusCode(StatusCodes.Status201Created, parkingdetails);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Book Parking Slot - Create. Error Occured in Create method in BookParkingSlotController" + ex.InnerException.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,"Error occuered while creating a slot");
                //return BadRequest("Error Occured while Creating the BookedParkingSlots in Create method");
            }


        }
        #endregion

        #region GetSlotDetailsByEmailID
       /// <summary>
       /// Get booked parking slot details of an associate by emailid
       /// </summary>
       /// <param name="EmailID"></param>
       /// <returns></returns>
        [HttpGet("GetSlotDetailsByEmailID/{EmailID}")]
        public async Task<IActionResult> GetSlotDetailsByEmailID(string EmailID)
        {
            try
            {
                var parkingdetails = await m_BookParkingSlotService.GetSlotDetailsByEmailID(EmailID);
                if (!parkingdetails.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in BookedParkingSlots table.");
                    return NotFound(parkingdetails.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning { parkingdetails } BookedParkingSlots details.");
                    return StatusCode(StatusCodes.Status200OK, parkingdetails);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetSlotDetailsByEmailID method in BookParkingSlotController" + ex.StackTrace);
                return BadRequest("Error Occured while getting the BookedParkingSlot in GetSlotDetailsByEmailID method");
            }


        }
        #endregion

        #region GetSlotDetails
       /// <summary>
       /// Get total slot, available slot details
       /// </summary>
       /// <param name="PlaceName"></param>
       /// <returns></returns>
        [HttpGet("GetSlotDetails/{PlaceName}")]
        public async Task<IActionResult> GetSlotDetails(string PlaceName)
        {
            try
            {
                var parkingdetails = await m_BookParkingSlotService.GetSlotDetails(PlaceName);
                if (!parkingdetails.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in BookedParkingSlots table.");
                    return NotFound(parkingdetails.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning { parkingdetails } BookedParkingSlots details.");
                    return Ok(parkingdetails);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetSlotDetails method in BookParkingSlotController" + ex.StackTrace);
                return BadRequest("Error Occured while getting the BookedParkingSlots in GetSlotDetails method");
            }
        }
        #endregion

        #region
        /// <summary>
        /// Release from existing slot of associate by emailid
        /// </summary>
        /// <param name="email">email</param>
        /// <returns></returns>
        [HttpPost("ReleaseSlot")]
        public async Task<IActionResult> ReleaseSlot(string email)
        {
            try
            {
                var releseSlot = await m_BookParkingSlotService.ReleaseSlot(email);
                if (releseSlot.IsSuccessful)
                {
                    m_Logger.LogInformation($"Returning { releseSlot } BookedParkingSlots details.");
                    return StatusCode(StatusCodes.Status200OK, releseSlot);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
               
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in ReleaeSlot method in BookParkingSlotController" + ex.StackTrace);
                return BadRequest("Error Occured while getting the ReleaeSlot in GetSlotDetails method");
            }
        }
        #endregion
    }
}
