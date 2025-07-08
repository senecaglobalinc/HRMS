namespace HRMS.Admin.Infrastructure.Models.Domain
{
    public class ActivityDetails
    {
        /// <summary>
        /// ActivityId
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// Department
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// DepartmentId
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// ActivityType
        /// </summary>
        public string ActivityType { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// IsRequired
        /// </summary>
        public bool? IsRequired { get; set; }
    }
}
