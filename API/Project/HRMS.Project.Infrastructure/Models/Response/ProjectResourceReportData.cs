using HRMS.Project.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Response
{
    public class ProjectResourceReportData 
    {
        public int ProjectId { get; set; }
        public bool IsBillable { get; set; }             
        public int Total { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public int ClientId { get; set; }
        public int PracticeAreaId { get; set; }
        public int ProgramManagerId { get; set; }
    }
}
