using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public byte[] Photograph { get; set; }
        public string AccessCardNo { get; set; }
        public string Gender { get; set; }
        public int? GradeId { get; set; }
        public int? DesignationId { get; set; }
        public string MaritalStatus { get; set; }
        public string Qualification { get; set; }
        public string TelephoneNo { get; set; }
        public string MobileNo { get; set; }
        public string WorkEmailAddress { get; set; }
        public string PersonalEmailAddress { get; set; }
        public DateTime? DateofBirth { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? RelievingDate { get; set; }
        public string BloodGroup { get; set; }
        public string Nationality { get; set; }
        public string Pannumber { get; set; }
        public string PassportNumber { get; set; }
        public string PassportIssuingOffice { get; set; }
        public string PassportDateValidUpto { get; set; }
        public int? ReportingManager { get; set; }
        public int? ProgramManager { get; set; }
        public int? DepartmentId { get; set; }
        //public string CreatedUser { get; set; }
        //public string ModifiedUser { get; set; }
        public bool? DocumentsUploadFlag { get; set; }
        public string CubicalNumber { get; set; }
        public string AlternateMobileNo { get; set; }
        public int? StatusId { get; set; }
        public DateTime? BgvinitiatedDate { get; set; }
        public DateTime? BgvcompletionDate { get; set; }
        public int? BgvstatusId { get; set; }
        public decimal? Experience { get; set; }
        public int? CompetencyGroup { get; set; }
        public DateTime? BgvtargetDate { get; set; }
        public int? EmployeeTypeId { get; set; }
        public int? Userid { get; set; }
        public DateTime? ResignationDate { get; set; }
        public string Bgvstatus { get; set; }
        public int? Paid { get; set; }
        public string Hradvisor { get; set; }
        public string Uannumber { get; set; }
        public string AadharNumber { get; set; }
        public string Pfnumber { get; set; }
        public byte[] Remarks { get; set; }
        public DateTime? EmploymentStartDate { get; set; }
        public int? CareerBreak { get; set; }
        public decimal? TotalExperience { get; set; }
        public decimal? ExperienceExcludingCareerBreak { get; set; }
    }
}
