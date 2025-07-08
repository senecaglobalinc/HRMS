using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Infrastructure.Models.Domain
{
    public class EmployeeDetails
    {
        /// <summary>
        /// EmployeeId
        /// </summary>
        public int EmployeeId { get; set; }
        public int EmpId { set; get; }
        public string EmpName { set; get; }
        /// <summary>
        /// EmployeeCode
        /// </summary>
        public string EmployeeCode { get; set; }

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
        public string EmployeeName { get; set; }

        /// <summary>
        /// Photograph
        /// </summary>
        public byte[] Photograph { get; set; }

        /// <summary>
        /// AccessCardNo
        /// </summary>
        public string AccessCardNo { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// GradeId
        /// </summary>
        public int? GradeId { get; set; }

        /// <summary>
        /// DesignationId
        /// </summary>
        public int? DesignationId { get; set; }

        /// <summary>
        /// MaritalStatus
        /// </summary>
        public string MaritalStatus { get; set; }

        /// <summary>
        /// Qualification
        /// </summary>
        public string Qualification { get; set; }

        /// <summary>
        /// TelephoneNo
        /// </summary>
        public string TelephoneNo { get; set; }

        /// <summary>
        /// MobileNo
        /// </summary>
        public string MobileNo { get; set; }

        /// <summary>
        /// WorkEmailAddress
        /// </summary>
        public string WorkEmailAddress { get; set; }

        /// <summary>
        /// PersonalEmailAddress
        /// </summary>
        public string PersonalEmailAddress { get; set; }

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
        public bool? IsActive { get; set; }

        /// <summary>
        /// PassportDateValidUpto
        /// </summary>
        public string PassportDateValidUpto { get; set; }

        /// <summary>
        /// ReportingManager
        /// </summary>
        public int? ReportingManager { get; set; }

        /// <summary>
        /// ProgramManager
        /// </summary>
        public int? ProgramManager { get; set; }

        /// <summary>
        /// DepartmentId
        /// </summary>
        public int? DepartmentId { get; set; }

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
        /// StatusId
        /// </summary>
        public int? StatusId { get; set; }

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
        /// UserId
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// ResignationDate
        /// </summary>
        public DateTime? ResignationDate { get; set; }

        /// <summary>
        /// Bgvstatus
        /// </summary>
        public string Bgvstatus { get; set; }

        /// <summary>
        /// Paid
        /// </summary>
        public int? Paid { get; set; }

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

        /// <summary>
        /// ExperienceExcludingCareerBreak
        /// </summary>
        public decimal? ExperienceExcludingCareerBreak { get; set; }
    }
}

