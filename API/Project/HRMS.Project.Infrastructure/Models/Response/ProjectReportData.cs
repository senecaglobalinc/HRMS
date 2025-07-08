using HRMS.Project.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Response
{
    public class ProjectReportData 
    {
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string Technology { get; set; }
        public string ProgramManager { get; set; }
        public int Total { get; set; }
        public int Billable { get; set; }
        public int NonBillable { get; set; }
        public string ClientName { get; set; }
    }
}
