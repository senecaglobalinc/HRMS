using System;
using System.Collections.Generic;

namespace HRMS.KRA.Infrastructure.Models
{
    public class AssociateKRA
    {
        public string FinancialYear { get; set; }
        public List<AssociateRoleType> AssociateRoleTypes { get; set; }
        public List<KRADefinition> KRADefinitions { get; set; }
    }
    public class KRADefinition
    {
        public int DepartmentId { get; set; }
        public int RoleTypeId { get; set; }
        public string KRAAspectName { get; set; }
        public string KRAAspectMetric { get; set; }
        public string KRAMeasurementType { get; set; }
        public string Operator { get; set; }
        public string KRATargetValue { get; set; }
        public string KRATargetText { get; set; }
        public string KRATargetPeriod { get; set; }
    }    
}
