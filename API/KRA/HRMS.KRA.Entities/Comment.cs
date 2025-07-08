using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Entities
{
    /// <summary>
    /// Comment
    /// </summary>
    public class Comment : BaseEntity
    {
        /// <summary>
        /// CommentID
        /// </summary>
        public int CommentID { get; set; }

        /// <summary>
        /// CommentText
        /// </summary>
        public string CommentText { get; set; }

        /// <summary>
        /// FinancialYearId
        /// </summary>
        public int FinancialYearId { get; set; }

        /// <summary>
        /// DepartmentId
        /// </summary>
        public int DepartmentId { get; set; }

        
    }
}
