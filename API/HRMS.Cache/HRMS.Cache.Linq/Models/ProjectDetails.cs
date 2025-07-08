using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Cache.Linq.Models
{
    public class ProjectDetail
    {
        public int ProjectId { get; set; }
        public int ProjectManagerId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string ClientName { get; set; }
        public string PracticeAreaCode { get; set; }
        public string ProjectTypeDescription { get; set; }
        public string ProjectState { get; set; }
        public string ManagerName { get; set; }
    }
}
