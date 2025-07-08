namespace HRMS.Admin.Entities
{
    public class ProjectType : BaseEntity
    {
        /// <summary>
        /// ProjectTypeId
        /// </summary>
        public int ProjectTypeId { get; set; }

        /// <summary>
        /// ProjectTypeCode
        /// </summary>
        public string ProjectTypeCode { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
    }
}
