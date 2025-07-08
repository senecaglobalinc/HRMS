using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure
{
    public abstract class Common
    {
        public string EmailID { get; set; }
        public string BookingDate { get; set; }
    }
   public class BookedParkingSlotDetails:Common
    {
        public int ID { get; set; }
        public string BookingTime { get; set; }
        public bool IsSlotBooked { get; set; }
        private string vehicleNumber = string.Empty;
        public string VehicleNumber
        {
            get
            {
                return vehicleNumber.ToUpper().Trim();
            }
            set
            {
                vehicleNumber = value;
            }
        }
        private string placeName = string.Empty;
        public string PlaceName
        {
            get
            {
                return placeName.ToUpper();
            }
            set
            {
                placeName = value;
            }
        }
    }
    public class AvailableSlots
    {
        public int TotalSlotCount { get; set; }
        public int AvailableSlotCount { get; set; }       
    }
    public class ReleaseSlot : Common
    {
        public string ReleaseDate { get; set; }
        public string ReleaseTime { get; set; }
    }

}
