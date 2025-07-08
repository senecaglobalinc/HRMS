using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ParkingSlotReport
    {
            public string Email { get; set; }
            public string VehicleNumber { get; set; }
            public string BookedDate { get; set; }
            public string BookedTime { get; set; }
            public string Location { get; set; }
        
    }
    public class ParkingSearchFilter
    {
        public string StartDate { get; set; }
        public string Enddate { get; set; }
        public string Location { get; set; }
    }

}
