namespace HRMS.Admin.Infrastructure.Models.Domain
{
    public class DepartmentDetails
    {

        /// <summary>
        /// DepartmentId
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// DepartmentCode
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// DepartmentHeadId
        /// </summary>
        public int? DepartmentHeadId { get; set; }

        /// <summary>
        /// DepartmentTypeId
        /// </summary>
        public int DepartmentTypeId { get; set; }

        /// <summary>
        /// DepartmentHeadId
        /// </summary>
        public int? EmployeeId { get; set; }
    }
}
