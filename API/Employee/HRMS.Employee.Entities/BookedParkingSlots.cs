using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
   public class BookedParkingSlots:BaseEntity
    {
        public int ID { get; set; }
        public string EmailID { get; set; }
        public string BookedDate { get; set; }
        public string BookedTime { get; set; }
        public string ReleaseDate { get; set; }
        public string ReleaseTime { get; set; }
        private string placeName = string.Empty;
        public string PlaceName
        {
            get
            {
                return placeName.ToUpper().Trim();
            }
            set
            {
                placeName = value;
            }
        }
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
    }
}
