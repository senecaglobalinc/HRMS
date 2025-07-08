using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class KRARoleTypes
    {
        public  int ID { get; set; }         
        public  int RoleTypeId { get; set; }         
        public  int EmployeeId { get; set; }         
        public  string RoleTypeName { get; set; }         
        public KRARoleTypes Items { get; set; }
    }
}
