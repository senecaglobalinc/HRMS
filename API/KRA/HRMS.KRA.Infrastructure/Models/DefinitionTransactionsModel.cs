using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Infrastructure.Models
{
    public class DefinitionTransactionsModel
    {       
        public int DefinitionTransactionId { get; set; }
        public Guid? DefinitionId { get; set; }
        public int FinancialYearId { get; set; }
        public int RoleTypeId { get; set; }
        public string AspectName { get; set; }       
        public string Metric { get; set; }
        public int OperatorId { get; set; }
        public int MeasurementTypeId { get; set; }
        public int? ScaleId { get; set; }
        public string TargetValue { get; set; }
        public int TargetPeriodId { get; set; }
        public bool IsAccepted { get; set; }
        public string CurrentUser { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
