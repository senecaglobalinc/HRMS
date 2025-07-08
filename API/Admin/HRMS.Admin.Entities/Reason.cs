namespace HRMS.Admin.Entities
{
    public class Reason : BaseEntity
    {
        /// <summary>
        /// ReasonId
        /// </summary>
        public int ReasonId { get; set; }

        /// <summary>
        /// ReasonCode
        /// </summary>
        public string ReasonCode { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ReasonTypeId
        /// </summary>
        public int ReasonTypeId { get; set; }

        /// <summary>
        /// ReasonType
        /// </summary>
        public virtual ReasonType ReasonType { get; set; }
    }
}
