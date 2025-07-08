using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ExitEmployeeResponse
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AssociateExitId { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public int? ProjectId { get; set; }
        public int StatusId { get; set; }
        public int RevokeStatusId { get; set; }
        public string RevokeComment { get; set; }
        public DateTime ExitDate { get; set; }
        public int SubStatusId { get; set; }
        public bool TransitionRequired { get; set; } = true;
        public int? AssociateAllocationId { get; set; }
    }
}
