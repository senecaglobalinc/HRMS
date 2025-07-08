using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class InsertBulkBiometricAttendanceDTO
    {
        public DateTime dateFromSync { get; set; }
        public DateTime dateToSync { get; set; } = DateTime.Now;
        public List<BiometricAttendance> stringifiedjsondata { get; set; }
    }
}
