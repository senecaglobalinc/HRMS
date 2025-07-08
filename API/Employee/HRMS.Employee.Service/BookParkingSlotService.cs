using AutoMapper;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    public class BookParkingSlotService : IBookParkingSlot
    {
        #region Global Variables

        private readonly EmployeeDBContext m_employeeDBContext;
        private readonly ILogger<BookParkingSlotService> m_Logger;
        private readonly ParkingSlot m_parkingSlot;
        private readonly string CurrentDate = DateTime.Now.ToString("MM/dd/yyyy");

        #endregion

        #region Constructor
        public BookParkingSlotService(EmployeeDBContext context, ILogger<BookParkingSlotService> logger, IConfiguration configuration, IOptions<ParkingSlot> parkingSlot)
        {
            m_employeeDBContext = context;
            m_Logger = logger;
            m_parkingSlot = parkingSlot.Value;
        }
        #endregion

        #region Create
        public async Task<ServiceResponse<BookedParkingSlotDetails>> Create(BookedParkingSlotDetails bookedParkingSlot)
        {
            
            var response = new ServiceResponse<BookedParkingSlotDetails>();
            try
            {                
                BookedParkingSlots bookedParkingSlt = await m_employeeDBContext.BookedParkingSlots.Where(x => x.EmailID == bookedParkingSlot.EmailID && x.IsActive == true && x.BookedDate == CurrentDate).FirstOrDefaultAsync();
                if (bookedParkingSlt == null)
                {
                    int slotCount = (await GetTotalParkingSlotBooked(bookedParkingSlot.PlaceName));

                    if (slotCount < m_parkingSlot.GalaxyTotalParkingSlots)
                    {
                        BookedParkingSlots bookedParkingSlots = new BookedParkingSlots();
                        bookedParkingSlots.BookedDate = DateTime.Now.ToString("MM/dd/yyyy");
                        bookedParkingSlots.BookedTime = DateTime.Now.ToString("HH:mm");
                        bookedParkingSlots.EmailID = bookedParkingSlot.EmailID;
                        bookedParkingSlots.PlaceName = bookedParkingSlot.PlaceName;
                        bookedParkingSlots.VehicleNumber = bookedParkingSlot.VehicleNumber;
                        m_employeeDBContext.BookedParkingSlots.Add(bookedParkingSlots);
                        await m_employeeDBContext.SaveChangesAsync();
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                    }
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Parking slot already booked";

                }

            }
            catch (Exception e)
            {

                response.IsSuccessful = false;
                response.Message = "Error occured while booking slot " + e.Message;
            }
            return response;
        }
        #endregion
        #region ReleaseSlot
        public async Task<ServiceResponse<BookedParkingSlotDetails>> ReleaseSlot(string email)
        {
            var response = new ServiceResponse<BookedParkingSlotDetails>();
            try
            {                
                var slotDetails = await m_employeeDBContext.BookedParkingSlots.Where(x => x.EmailID == email && x.IsActive == true && x.BookedDate == CurrentDate).FirstOrDefaultAsync();
                if (slotDetails != null)
                {
                    slotDetails.IsActive = false;
                    slotDetails.ReleaseDate = CurrentDate;
                    slotDetails.ReleaseTime = DateTime.Now.ToString("HH:mm");                    
                    m_employeeDBContext.Update(slotDetails);
                    await m_employeeDBContext.SaveChangesAsync();
                    response.IsSuccessful = true;
                }
               
            }
            catch(Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching details " + ex.Message;
            }
            return response;
        }
        #endregion

        #region GetSlotDetailsByEmailID
        public async Task<ServiceResponse<BookedParkingSlotDetails>> GetSlotDetailsByEmailID(string emailId)
        {
            var response = new ServiceResponse<BookedParkingSlotDetails>();
            try
            {
                BookedParkingSlotDetails bookedParkingSlotDetails = new BookedParkingSlotDetails();
                var slotDetails = await m_employeeDBContext.BookedParkingSlots.Where(x => x.EmailID == emailId && x.IsActive==true && x.BookedDate==CurrentDate).FirstOrDefaultAsync();                
                if (slotDetails != null)
                {
                    bookedParkingSlotDetails.IsSlotBooked = true;
                    bookedParkingSlotDetails.BookingDate = slotDetails.BookedDate;
                    bookedParkingSlotDetails.BookingTime = slotDetails.BookedTime;
                    bookedParkingSlotDetails.PlaceName = slotDetails.PlaceName;
                    bookedParkingSlotDetails.VehicleNumber = slotDetails.VehicleNumber;
                    response.Message = "Slot already booked";
                    response.Item = bookedParkingSlotDetails;
                }
                else
                {
                    bookedParkingSlotDetails.IsSlotBooked = false;
                    response.Message = "Slot not booked";
                    response.Item = bookedParkingSlotDetails;
                }
                response.IsSuccessful = true;

            }
            catch (Exception e)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching details "+e.Message;
            }
            return response;
        }
        #endregion

        #region GetSlotDetails
        public async Task<ServiceResponse<AvailableSlots>> GetSlotDetails(string PlaceName)
        {
            var response = new ServiceResponse<AvailableSlots>();
            try
            {
                int bookedSlotCount = (await GetTotalParkingSlotBooked(PlaceName));
                int totalSlotCount = PlaceName.ToUpper().Trim() == "GALAXY" ? m_parkingSlot.GalaxyTotalParkingSlots : m_parkingSlot.ShilparamamTotalParkingSlots;
                AvailableSlots availabledParkingSlotDetails = new AvailableSlots
                {
                    TotalSlotCount = totalSlotCount,
                    AvailableSlotCount = totalSlotCount - bookedSlotCount
                };
                response.IsSuccessful = true;
                response.Item = availabledParkingSlotDetails;
            }
            catch(Exception e)
            {
                response.IsSuccessful = false;
                response.Message = "Error Occured while fetching details "+e.Message;
            }
            return response;
        }
        #endregion

        #region private methods
        private async Task<int>  GetTotalParkingSlotBooked()
        {
           return (await m_employeeDBContext.BookedParkingSlots.Where(x => x.IsActive == true && x.BookedDate == CurrentDate).ToListAsync()).Count;

        }
        private async Task<int> GetTotalParkingSlotBooked(string PlaceName)
        {
            return (await m_employeeDBContext.BookedParkingSlots.Where(x => x.IsActive == true && x.PlaceName.ToUpper()==PlaceName.ToUpper() && x.BookedDate == CurrentDate).ToListAsync()).Count;

        }
        #endregion
    }
}
