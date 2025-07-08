using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class Employee
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public byte[] Photograph { get; set; }
        public string AccessCardNo { get; set; }
        public string Gender { get; set; }
        public Nullable<int> GradeId { get; set; }
        public Nullable<int> Designation { get; set; }
        public string MaritalStatus { get; set; }
        public string Qualification { get; set; }
        public string TelephoneNo { get; set; }
        public string MobileNo { get; set; }
        public string WorkEmailAddress { get; set; }
        public string PersonalEmailAddress { get; set; }
        public Nullable<System.DateTime> DateofBirth { get; set; }
        public Nullable<System.DateTime> JoinDate { get; set; }
        public Nullable<System.DateTime> ConfirmationDate { get; set; }
        public Nullable<System.DateTime> RelievingDate { get; set; }
        public string BloodGroup { get; set; }
        public string Nationality { get; set; }
        public string PANNumber { get; set; }
        public string PassportNumber { get; set; }
        public string PassportIssuingOffice { get; set; }
        public string PassportDateValidUpto { get; set; }
        public Nullable<int> ReportingManager { get; set; }
        public Nullable<int> ProgramManager { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<bool> DocumentsUploadFlag { get; set; }
        public string CubicalNumber { get; set; }
        public string AlternateMobileNo { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<System.DateTime> BGVInitiatedDate { get; set; }
        public Nullable<System.DateTime> BGVCompletionDate { get; set; }
        public Nullable<int> BGVStatusId { get; set; }
        public Nullable<decimal> Experience { get; set; }
        public Nullable<int> CompetencyGroup { get; set; }
        public Nullable<System.DateTime> BGVTargetDate { get; set; }
        public Nullable<int> EmployeeTypeId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> ResignationDate { get; set; }
        public string BGVStatus { get; set; }
        public Nullable<int> PAID { get; set; }
        public string HRAdvisor { get; set; }
        public string UANNumber { get; set; }
        public string AadharNumber { get; set; }
        public string PFNumber { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> EmploymentStartDate { get; set; }
        public Nullable<int> CareerBreak { get; set; }
        public Nullable<decimal> TotalExperience { get; set; }
        public Nullable<decimal> ExperienceExcludingCareerBreak { get; set; }
        public string CurrentUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int? ProgramManagerId { get; set; }
    }
}
