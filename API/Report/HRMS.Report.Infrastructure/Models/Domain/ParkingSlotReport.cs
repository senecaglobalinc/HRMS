using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Report.Infrastructure.Models.Domain
{
    public class ParkingSlotReport
    {
        public string Email { get; set; }
        public string VehicleNumber { get; set; }
        public DateTime BookedDate { get; set; }
        public string BookedTime { get; set; }
        public string Location { get; set; }
    }
}
