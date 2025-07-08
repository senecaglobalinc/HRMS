using HRMS.Employee.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class EmployeeDetails : BaseEntity
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public int EmployeeId { get; set; }
        /// <summary>
        /// EmpCode
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// EmpName
        /// </summary>
        public string EmpName { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// MiddleName
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// MaritalStatus
        /// </summary>
        public string MaritalStatus { get; set; }

        /// <summary>
        /// BgvStatus
        /// </summary>
        public string BgvStatus { get; set; }

        /// <summary>
        /// EncryptedMobileNumber
        /// </summary>
        public string EncryptedMobileNumber { get; set; }

        /// <summary>
        /// MobileNo
        /// </summary>
        public string MobileNo   { get; set; }

        /// <summary>
        /// RoleName
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// WorkEmail
        /// </summary>
        public string WorkEmail { get; set; }

        /// <summary>
        /// PersonalEmailAddress
        /// </summary>
        public string PersonalEmailAddress { get; set; }

        /// <summary>
        /// Dob
        /// </summary>
        public Nullable<System.DateTime> Dob { get; set; }

        /// <summary>
        /// DepartmentId
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Department
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Department
        /// </summary>
        public string DepartmentDesc { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// ProjectId
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// joiningStatusID
        /// </summary>
        public Nullable<int> joiningStatusID { get; set; }

        /// <summary>
        /// ProjectName
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// LeadId
        /// </summary>
        public int? LeadId { get; set; }

        /// <summary>
        /// ManagerId
        /// </summary>
        public int? ManagerId { get; set; }

        /// <summary>
        /// ManagerFirstName
        /// </summary>
        public string ManagerFirstName { get; set; }

        /// <summary>
        /// ManagerLastName
        /// </summary>
        public string ManagerLastName { get; set; }

        /// <summary>
        /// LeadFirstName
        /// </summary>
        public string LeadFirstName { get; set; }

        /// <summary>
        /// LeadLastName
        /// </summary>
        public string LeadLastName { get; set; }

        /// <summary>
        /// ManagerName
        /// </summary>
        public string ManagerName { get; set; }

        /// <summary>
        /// LeadName
        /// </summary>
        public string LeadName { get; set; }

        /// <summary>
        /// EmailAddress
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// StatusId
        /// </summary>
        public Nullable<int> StatusId { get; set; }

        /// <summary>
        /// Year
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// RoleId
        /// </summary>
        public int? RoleId { get; set; }

        /// <summary>
        /// GradeID
        /// </summary>
        public int? GradeId { get; set; }

        /// <summary>
        /// TechnologyID
        /// </summary>
        public int? TechnologyID { get; set; }

        /// <summary>
        /// DesignationId
        /// </summary>
        public int? DesignationId { get; set; }

        /// <summary>
        /// EmploymentType
        /// </summary>
        public string EmploymentType { get; set; }

        /// <summary>
        /// GradeName
        /// </summary>
        public string GradeName { get; set; }

        /// <summary>
        /// LastWorkingDate
        /// </summary>
        public DateTime? LastWorkingDate { get; set; }

        /// <summary>
        /// JoiningDate
        /// </summary>
        public string JoiningDate { get; set; }

        /// <summary>
        /// Designation
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// HRAdvisorName
        /// </summary>
        public string HRAdvisorName { get; set; }

        /// <summary>
        /// RecruitedBy
        /// </summary>
        public string RecruitedBy { get; set; }

        /// <summary>
        /// BgvStatusId
        /// </summary>
        public Nullable<int> BgvStatusId { get; set; }

        /// <summary>
        /// ReportingManager
        /// </summary>
        public string ReportingManager { get; set; }

        /// <summary>
        /// ReportingManagerId
        /// </summary>
        public int? ReportingManagerId { get; set; }

        /// <summary>
        /// Technology
        /// </summary>
        public string Technology { get; set; }

        /// <summary>
        /// DateOfJoining
        /// </summary>
        public DateTime? DateOfJoining { get; set; }

        /// <summary>
        /// DropOutReason
        /// </summary>
        public string DropOutReason { get; set; }
        public string DepartmentName { get; set; }

        /// <summary>
        /// Photograph
        /// </summary>
        //public byte[] Photograph { get; set; }

        /// <summary>
        /// AccessCardNo
        /// </summary>
        //public string AccessCardNo { get; set; }

        /// <summary>
        /// Qualification
        /// </summary>
        //public string Qualification { get; set; }

        /// <summary>
        /// TelephoneNo
        /// </summary>
        //public string TelephoneNo { get; set; }

        /// <summary>
        /// WorkEmailAddress
        /// </summary>
        public string WorkEmailAddress { get; set; }

        /// <summary>
        /// DateofBirth
        /// </summary>
        public DateTime? DateofBirth { get; set; }

        /// <summary>
        /// JoinDate
        /// </summary>
        public DateTime? JoinDate { get; set; }

        /// <summary>
        /// ConfirmationDate
        /// </summary>
        public DateTime? ConfirmationDate { get; set; }

        /// <summary>
        /// RelievingDate
        /// </summary>
        public DateTime? RelievingDate { get; set; }

        /// <summary>
        /// BloodGroup
        /// </summary>
        public string BloodGroup { get; set; }

        /// <summary>
        /// Nationality
        /// </summary>
        public string Nationality { get; set; }

        /// <summary>
        /// Pannumber
        /// </summary>
        public string Pannumber { get; set; }

        /// <summary>
        /// PassportNumber
        /// </summary>
        public string PassportNumber { get; set; }

        /// <summary>
        /// PassportIssuingOffice
        /// </summary>
        public string PassportIssuingOffice { get; set; }

        /// <summary>
        /// PassportDateValidUpto
        /// </summary>
        public string PassportDateValidUpto { get; set; }

        /// <summary>
        /// ProgramManager
        /// </summary>
        public int? ProgramManager { get; set; }

        /// <summary>
        /// DocumentsUploadFlag
        /// </summary>
        public bool? DocumentsUploadFlag { get; set; }

        /// <summary>
        /// CubicalNumber
        /// </summary>
        public string CubicalNumber { get; set; }

        /// <summary>
        /// AlternateMobileNo
        /// </summary>
        public string AlternateMobileNo { get; set; }

        /// <summary>
        /// BgvinitiatedDate
        /// </summary>
        public DateTime? BgvinitiatedDate { get; set; }

        /// <summary>
        /// BgvcompletionDate
        /// </summary>
        public DateTime? BgvcompletionDate { get; set; }

        /// <summary>
        /// BgvstatusId
        /// </summary>
        public int? BgvstatusId { get; set; }

        /// <summary>
        /// Experience
        /// </summary>
        public decimal? Experience { get; set; }

        /// <summary>
        /// CompetencyGroup
        /// </summary>
        public int? CompetencyGroup { get; set; }

        /// <summary>
        /// BgvtargetDate
        /// </summary>
        public DateTime? BgvtargetDate { get; set; }

        /// <summary>
        /// EmployeeTypeId
        /// </summary>
        public int? EmployeeTypeId { get; set; }
        /// <summary>
        /// ResignationDate
        /// </summary>
        public DateTime? ResignationDate { get; set; }

        /// <summary>
        /// Bgvstatus
        /// </summary>
        public string Bgvstatus { get; set; }

        /// <summary>
        /// Hradvisor
        /// </summary>
        public string Hradvisor { get; set; }

        /// <summary>
        /// Uannumber
        /// </summary>
        public string Uannumber { get; set; }

        /// <summary>
        /// AadharNumber
        /// </summary>
        public string AadharNumber { get; set; }

        /// <summary>
        /// Pfnumber
        /// </summary>
        public string Pfnumber { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// EmploymentStartDate
        /// </summary>
        public DateTime? EmploymentStartDate { get; set; }

        /// <summary>
        /// CareerBreak
        /// </summary>
        public int? CareerBreak { get; set; }

        /// <summary>
        /// TotalExperience
        /// </summary>
        public decimal? TotalExperience { get; set; }

        public DateTime? SubmittedDate { get; set; }
        public bool ?IsDepartmentChange { get; set; }
        /// <summary>
        /// ExperienceExcludingCareerBreak
        /// </summary>
        public decimal? ExperienceExcludingCareerBreak { get; set; }
        public string External { get; set; }
        public string ProgramManagerName { get; set; }
        public List<EducationDetails> Qualifications { get; set; }
        public IEnumerable<PreviousEmploymentDetails> PrevEmploymentDetails { get; set; }
        public IEnumerable<ProfessionalReferences> ProfReferences { get; set; }       
        public IEnumerable<EmployeeProject> Projects { get; set; }
    }
}
