using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Infrastructure.Models
{
    public class CommentModel
    {
        public int CommentID { get; set; }
        public string CommentText { get; set; }
        public int FinancialYearId { get; set; }
        public int DepartmentId { get; set; }
        public int? ApplicableRoleTypeId { get; set; }
        public int? RoleTypeId { get; set; }
        public int? GradeId { get; set; }
        public string Username { get; set; }
        public bool IsCEO { get; set; }
        public string CreatedBy { get; set; }
        //public DateTime? CreatedDate { get; set; }
    }
}
