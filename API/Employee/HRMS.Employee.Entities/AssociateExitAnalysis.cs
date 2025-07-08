using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class AssociateExitAnalysis : BaseEntity
    {
        public int Id { get; set; }
        public int AssociateExitId { get; set; }
        public string RootCause { get; set; }
        public string ActionItem { get; set; }
        public string Responsibility { get; set; }
        public DateTime? TagretDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public string Remarks { get; set; }
        public int StatusId { get; set; }

        public virtual AssociateExit AssociateExit { get; set; }
    }
}
