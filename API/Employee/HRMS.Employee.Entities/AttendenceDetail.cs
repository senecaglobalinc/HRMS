using System;
using System.Linq;

namespace HRMS.Employee.Entities
{
    public class AttendanceDetail :BaseEntity
    {
        public int AttendanceDetailId { get; set; }
        public string AsscociateId { get; set; }
        public string AsscociateName { get; set; }
        public DateTime Date { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string TotalTime { get; set; }
        public bool IsRosterDay { get; set; }
        public void CalculateTotalTime()
        {
            if (string.IsNullOrWhiteSpace(OutTime) == false && string.IsNullOrWhiteSpace(InTime) == false)
            {
                var inTime = InTime.Split(':');
                var outTime = OutTime.Split(':');


                TotalTime = inTime.Count() > 1 && outTime.Count() > 1 ?
                                (new TimeSpan(Convert.ToInt32(outTime[0]), Convert.ToInt32(outTime[1]), 0) -
                                 new TimeSpan(Convert.ToInt32(inTime[0]), Convert.ToInt32(inTime[1]), 0)).ToString(@"hh\:mm") : "";
            }
        }
    }
}
