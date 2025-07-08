using HRMS.Employee.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ServiceType:BaseEntity
    {
        public int EmployeeId { get; set; }
        public int ServiceTypeId { get; set; }
       
    }
}
