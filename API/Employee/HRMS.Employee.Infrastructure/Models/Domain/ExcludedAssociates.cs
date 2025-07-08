using System;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ExcludedAssociates
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string AssociateName { get; set; } = string.Empty;
        public Boolean IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
