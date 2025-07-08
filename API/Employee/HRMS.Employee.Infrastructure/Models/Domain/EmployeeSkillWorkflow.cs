using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
   public class EmployeeSkillWorkflow
    {

        /// <summary>
        /// Id
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// EmployeeId
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// EmployeeName
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// EmployeeCode
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// ProjectName
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// SubmittedBy
        /// </summary>
        public int SubmittedBy { get; set; }

        /// <summary>
        /// SubmittedTo
        /// </summary>
        public int SubmittedTo { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// StatusName
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// RequestedId
        /// </summary>
        public int RequestedId { get; set; }

        /// <summary>
        /// Submitted Date
        /// </summary>
        public DateTime? SubmittedDate { get; set; }

        /// <summary>
        /// Approved Date
        /// </summary>
        public DateTime? ApprovedDate { get; set; }

        /// <summary>
        ///skillID
        /// </summary>
        public int EmployeeSkillId { get; set; }

        /// <summary>
        /// SubmittedRating
        /// </summary>
        public int? SubmittedRating { get; set; }


        /// <summary>
        /// SubmittedRating
        /// </summary>
        public string SubmittedRatingName { get; set; }

        /// <summary>
        /// ReportingManagerRating
        /// </summary>
        public int? ReportingManagerRating { get; set; }

        /// <summary>
        /// ReportingManagerRating
        /// </summary>
        public string ReportingManagerRatingName { get; set; }

        /// <summary>
        /// SubmittedToName
        /// </summary>
        public string SubmittedToName { get; set; }

        ///<summary>
        ///Remarks
        ///</summary>
        public string Remarks { get; set; }


        ///<summary>
        ///SkillName
        ///</summary>
        public string SkillName { get; set; }

        /// <summary>
        /// Experience
        /// </summary>
        public decimal? Experience { get; set; }

        /// <summary>
        /// LastUsed
        /// </summary>
        public int? LastUsed { get; set; }
    }
}
