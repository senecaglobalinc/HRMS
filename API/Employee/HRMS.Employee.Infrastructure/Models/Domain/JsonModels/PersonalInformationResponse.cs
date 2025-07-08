using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class PersonalInformationResponse 
    {        
        public Guid PersonalInfoId { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }     
        public string Gender { get; set; }        
        public string MaritalStatus { get; set; }                                  
        public DateTime DateofBirth { get; set; }
        public string BloodGroup { get; set; }
        public string Nationality { get; set; }
        public string MobileNumber { get; set; }
        public string PersonalEmailId { get; set; }
        public DateTime EmploymentStartDate { get; set; }
        public int CareerBreak { get; set; }
        public DateTime JoiningDate { get; set; }
        public string CurrentAddressLine1 { get; set; }
        public string CurrentAddressLine2 { get; set; }
        public string CurrentCity { get; set; }
        public string CurrentState { get; set; }
        public string CurrentCountry { get; set; }
        public string CurrentZip { get; set; }
        public string PermanentAddressLine1 { get; set; }
        public string PermanentAddressLine2 { get; set; }
        public string PermanentCity { get; set; }
        public string PermanentState { get; set; }
        public string PermanentCountry { get; set; }
        public string PermanentZip { get; set; }
        public string MiddleName { get; set; }
        public string PANNumber { get; set; }
        public string AadharNumber { get; set; }
        public string EmployeeCode { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public int? ReportingManager { get; set; }
        public int? CompetencyGroup { get; set; }
        public int? GradeId { get; set; }
        public bool IsActive { get; set; }
        public string HRAdvisor { get; set; }
        public int? EmployeeTypeId { get; set; }
    }
}
