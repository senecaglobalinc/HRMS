using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
   public class EmployeeSkillWorkFlow :BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// SkillId
        /// </summary>
        public int EmployeeSkillId { get; set; }

        /// <summary>
        /// SubmittedRating
        /// </summary>
        public int? SubmittedRating { get; set; }

        /// <summary>
        /// ReportingManagerRating
        /// </summary>
        public int? ReportingManagerRating { get; set; }


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
        /// Approved Date
        /// </summary>
        public DateTime? ApprovedDate { get; set; }

        ///<summary>
        ///Remarks
        ///</summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Experience
        /// </summary>
        public int? Experience { get; set; }

        /// <summary>
        /// LastUsed
        /// </summary>
        public int? LastUsed { get; set; }


    }
}
