using System;

namespace HRMS.Admin.Entities
{
    public class Holiday 
    {
        public int Id { get; set; }
        public string Occasion { get; set; }
        public DateTime HolidayDate { get; set; }
    }
}
