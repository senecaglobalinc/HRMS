using System;

namespace HRMS.KRA.Entities
{
    public class KRAPdf : BaseEntity
    {
        public Guid KRAPdfId { get; set; }
        public string FinancialYear { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeEmail { get; set; }
        public string FileName { get; set; }
    }
}
