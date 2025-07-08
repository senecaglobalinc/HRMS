namespace HRMS.Employee.Infrastructure.Models.Domain
{

    public class BiometricAttendance
    {
        //public string Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Punch1_Date { get; set; } = string.Empty;
        public string Punch1_Time { get; set; } = string.Empty;
        public string Punch2_Date { get; set; } = string.Empty;
        public string Punch2_Time { get; set; } = string.Empty;
        public string? WorkingShift { get; set; }
        public decimal? EarlyIn { get; set; }
        public string? EarlyIn_HHMM { get; set; }
        public decimal? LateIn { get; set; }
        public string? LateIn_HHMM { get; set; }
        public decimal? EarlyOut { get; set; }
        public string? EarlyOut_HHMM { get; set; }
        public decimal? WorkTime { get; set; }
        public string? WorkTime_HHMM { get; set; }
        public string? SUMMARY { get; set; }

    }
}