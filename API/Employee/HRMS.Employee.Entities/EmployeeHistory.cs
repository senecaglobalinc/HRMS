using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
   public class EmployeeHistory:BaseEntity
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? GradeId { get; set; }
        public int? DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public int? ReportingManagerId { get; set; }
        public int? PracticeAreaId { get; set; }

    }
}
