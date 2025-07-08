using System;
using System.ComponentModel;
using System.Linq;

namespace HRMS.Employee.Entities
{
    public class BioMetricAttendance : BaseEntity
    {
        public Guid Id { get; set; }

        public string AsscociateId { get; set; }
        public string AsscociateName { get; set; }
        //public DateTime Date { get; set; }
        //public string Punch1_Date { get; set; }
        //public string Punch2_Date { get; set; }

        public DateTime? Punch1_Date { get; set; }
        public DateTime? Punch2_Date { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }

        public string WorkingShift { get; set; }

        public decimal? EarlyIn { get; set; }

        public string EarlyIn_HHMM { get; set; }

        public decimal? LateIn { get; set; }

        public string LateIn_HHMM { get; set; }     

        public decimal? EarlyOut { get; set; }

        public string EarlyOut_HHMM { get; set; }

        public decimal? WorkTime { get; set; }

        public string WorkTime_HHMM { get; set; }

        public string SUMMARY { get; set; }
        public string Location { get; set; }
        public int? SignedStatus { get; set; }
        public string PunchInfoLog { get; set; }
        public DateTime? ProcessDate { get; set; }
        public string Remarks { get; set; }
        public bool? IsRegularized { get; set; }



        public void FindDate()
        {           
                if (Punch1_Date == null && Punch2_Date != null)
                {               
                    Punch1_Date = Punch2_Date;
                }            
        }
    }
}
