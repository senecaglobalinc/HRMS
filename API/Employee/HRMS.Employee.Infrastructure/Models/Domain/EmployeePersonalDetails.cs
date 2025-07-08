using HRMS.Employee.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class EmployeePersonalDetails : Entities.Employee
    {
        public int ID { get; set; }
        
        public string EmploymentType { get; set; }
        
        public string Designation { get; set; }
        
        public int? TechnologyId { get; set; }
        public string GradeName { get; set; }
        public string RoleTypeName { get; set; }
        public int? KRARoleId { get; set; }
        public int ServiceTypeId { get; set; }
        public EmployeeContactDetails contacts { get; set; }
        public IEnumerable<EmployeeContactDetails> contactDetails { get; set; }
        public IEnumerable<FamilyDetails> RelationsInfo { get; set; }
        //public EmergencyContactData contactDetailsOne { get; set; }
        //public EmergencyContactData contactDetailsTwo { get; set; }
        public EmergencyContactDetails contactDetailsOne { get; set; }
        public EmergencyContactDetails contactDetailsTwo { get; set; }
    }
}
