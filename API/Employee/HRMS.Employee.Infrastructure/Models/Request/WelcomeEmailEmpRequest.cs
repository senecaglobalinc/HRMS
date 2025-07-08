using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Request
{
    public class WelcomeEmailEmpRequest 
    {        
        public int EmpId { get; set; }
        public string EmpCode { get; set; }                
        
        /// <summary>
        /// EmpName
        /// </summary>
        public string EmpName { get; set; }               

        /// <summary>
        /// EncryptedMobileNumber
        /// </summary>
        public string EncryptedMobileNumber { get; set; }

        /// <summary>
        /// MobileNo
        /// </summary>
        public string MobileNo { get; set; }
        
        /// <summary>
        /// WorkEmail
        /// </summary>
        public string WorkEmail { get; set; }

        /// <summary>
        /// PersonalEmailAddress
        /// </summary>
        public string PersonalEmailAddress { get; set; }        

        /// <summary>
        /// DepartmentId
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Department
        /// </summary>
        public string Department { get; set; }       

        /// <summary>
        /// EmploymentType
        /// </summary>
        public string EmploymentType { get; set; }
       
        /// <summary>
        /// JoiningDate
        /// </summary>
        public DateTime? JoiningDate { get; set; }

        /// <summary>
        /// Designation
        /// </summary>
        public string Designation { get; set; }      
        
        /// <summary>
        /// Experience
        /// </summary>
        public decimal? Experience { get; set; }

        /// <summary>
        /// PrevEmployeeDetails
        /// </summary>
        public string PrevEmployeeDetails { get; set; }

        /// <summary>
        /// PrevEmploymentDesignation
        /// </summary>
        public string PrevEmploymentDesignation { get; set; }

        /// <summary>
        /// Institution
        /// </summary>
        public string Institution { get; set; }

        /// <summary>
        /// HighestQualification
        /// </summary>
        public string HighestQualification { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// SkillName
        /// </summary>
        public List<WelcomeEmailSkillRequest> SkillName { get; set; }

        public List<WelcomeEmailCertRequest> CertificationList { get; set; }
    }
}
