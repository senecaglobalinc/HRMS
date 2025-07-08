using System;

namespace HRMS.KRA.Infrastructure.Models
{
    public class KRAModel
    {
        public Guid? DefinitionId { get; set; }
        public int DefinitionTransactionId { get; set; }
        public string AspectName { get; set; }
        public string Metric { get; set; }
        public string PreviousMetric { get; set; }
        public string Status { get; set; }
        public bool? IsDeleted { get; set; }
        public string Date { get; set; }
        public string Target { get; set; }
        public string previousration { get; set; }
        public int? ScaleId { get; set; }
        public int ModifiedTargetCount { get; set; }
        public int ModifiedMetricCount { get; set; }
        public int DeleteCount { get; set; }
        public int CreateCount { get; set; }
        public DateTime? DefinitionDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Username { get; set; }
        public string CreatedByUserRole { get; set; }
        public string ModifiedByUserRole { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsAdded { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAccepted { get; set; }
        public int AspectId { get; set; }
        public int OperatorId { get; set; }
        public int MeasurementTypeId { get; set; }
        public int TargetPeriodId { get; set; }
        public string TargetValue { get; set; }
        public int StatusId { get; set; }
        public bool IsUpdatedByHOD { get; set; }
        public string MeasurementType { get; set; }

    }
}
