using System;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class EmpEducationDetails
    {
        public int ID { get; set; }
        public string qualificationName { get; set; }       
        public int? empID { get; set; }       
        public DateTime? yearCompleted { get; set; }
        public string institution { get; set; }
        public string specialization { get; set; }
        public string programType { get; set; }
        public string grade { get; set; }
        public string marks { get; set; }        
        public string completedYear { get; set; }
        public int programTypeID { get; set; }
    }
}
