using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class AllEmployeeDetailsDTO
    {
        public string AssociateCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AssociateName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Email { get; set; }
        public int AssociateId { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public int? ReportingManagerId { get; set; }
        public string ReportingManagerName { get; set; }
        public int? ProgramManagerId { get; set; }
        public string ProgramManagerName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string RoleDescription { get; set; }
        public string Designation { get; set; }
        public string BloodGroup { get; set; }

    }
    public class EmployeesInfo
    {
        public List<AllEmployeeDetailsDTO> Employees { get; set; }
    }
}
