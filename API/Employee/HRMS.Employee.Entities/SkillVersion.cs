using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
   public class SkillVersion :BaseEntity
    {
        public int ID { get; set; }
        public int EmployeeSkillId { get; set; }
        public string Version  { get; set; }
        public int EmployeeId { get; set; }
    }
}
