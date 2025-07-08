using HRMS.Employee.Infrastructure.Domain;
using System.Collections.Generic;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ActivityChecklist
    {
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public string Remarks { get; set; }
        public int StatusId { get; set; }
        public bool? IsActive { get; set; }
        public string Type { get; set; }
        public List<UpdateActivityChecklist> ActivityDetails { get; set; }
    }
}
