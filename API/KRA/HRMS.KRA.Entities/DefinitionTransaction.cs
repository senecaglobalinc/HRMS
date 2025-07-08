using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Entities
{
    /// <summary>
    /// DefinitionTransaction
    /// </summary>
    public class DefinitionTransaction : BaseEntity
    {
        /// <summary>
        /// DefinitionTransactionId
        /// </summary>
        public int DefinitionTransactionId { get; set; }

        /// <summary>
        /// DefinitionId
        /// </summary>
        public Guid? DefinitionId { get; set; }

        /// <summary>
        /// FinancialYearId
        /// </summary>
        public int FinancialYearId { get; set; }

        /// <summary>
        /// RoleTypeId
        /// </summary>
        public int RoleTypeId { get; set; }

        /// <summary>
        /// AspectId
        /// </summary>
        public int AspectId { get; set; }

        /// <summary>
        /// Metric
        /// </summary>
        public string Metric { get; set; }

        /// <summary>
        /// OperatorId
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// MeasurementTypeId
        /// </summary>
        public int MeasurementTypeId { get; set; }

        /// <summary>
        /// ScaleId
        /// </summary>
        public int? ScaleId { get; set; }

        /// <summary>
        /// TargetValue
        /// </summary>
        public string TargetValue { get; set; }

        /// <summary>
        /// TargetPeriodId
        /// </summary>
        public int TargetPeriodId { get; set; }

        /// <summary>
        /// IsDeleted
        /// </summary>
        public bool? IsAccepted { get; set; }

        /// <summary>
        /// MeasurementType
        /// </summary>
        public virtual MeasurementType MeasurementType { get; set; }

        /// <summary>
        /// Scale
        /// </summary>
        public virtual Scale Scale { get; set; }

        /// <summary>
        /// Operator
        /// </summary>
        public virtual Operator Operator { get; set; }

        /// <summary>
        /// TargetPeriod
        /// </summary>
        public virtual TargetPeriod TargetPeriod { get; set; }
    }
}
