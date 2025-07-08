namespace HRMS.Admin.Entities
{
    public class Designation : BaseEntity
    {
        /// <summary>
        /// DesignationId
        /// </summary>        
        public int DesignationId { get; set; }

        /// <summary>
        /// DesignationCode
        /// </summary>
        public string DesignationCode { get; set; }

        /// <summary>
        /// DesignationName
        /// </summary>
        public string DesignationName { get; set; }

        /// <summary>
        /// GradeId
        /// </summary>
        public int? GradeId { get; set; }

        /// <summary>
        /// Grade
        /// </summary>
        public virtual Grade Grade { get; set; }
    }
}
