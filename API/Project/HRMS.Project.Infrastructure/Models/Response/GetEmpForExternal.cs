using HRMS.Project.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Response
{
    public class GetEmpForExternal
    {
        public string AssociateCode { get; set; }
        public string AssociateName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public int AssociateId { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? ReportingManagerId { get; set; }     
        public int? ProgramManagerID { get; set; }       
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
       
    }
}
