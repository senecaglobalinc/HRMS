using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class AssociateModel
    {
        public int AssociateId { get; set; }
        public string AssociateCode { get; set; }
        public string AssociateName { get; set; }
        public string AssociateEmail { get; set; }
        public string AssociateRole { get; set; }
        public bool AssociateExitFlag { get; set; }
        public string ReportingManagerEmail { get; set; }
        public string ProgramManagerEmail { get; set; }
    }  
}
