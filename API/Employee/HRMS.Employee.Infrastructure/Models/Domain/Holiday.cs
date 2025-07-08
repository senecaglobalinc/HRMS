using System;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class Holiday
    {
        public int Id { get; set; }
        public string Occasion { get; set; }
        public DateTime HolidayDate { get; set; }
    }
}
