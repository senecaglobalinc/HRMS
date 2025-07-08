using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Infrastructure.Models
{
    public class KRAStatusModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentHeadName { get; set; }
        public int TotalRoleTypes { get; set; }
        public int AcceptedRoleTypes { get; set; }
        public int ForReviewRoleTypes { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public string SendtoCEOStatus { get; set; }
        public string Action { get; set; }
    }
}
