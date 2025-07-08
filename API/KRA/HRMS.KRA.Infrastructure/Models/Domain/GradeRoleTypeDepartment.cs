using System.Collections.Generic;

namespace HRMS.KRA.Infrastructure.Models.Domain
{
    /// <summary>
    /// RoleTypeDepartment model
    /// </summary>
    public class RoleTypeDepartment
    {
        public RoleTypeDepartment()
        {
            RoleTypeIds = new List<int>();
        }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public List<int> RoleTypeIds { get; set; }
    }

    /// <summary>
    /// OperationHeadStatusModel model
    /// </summary>
    public class OperationHeadStatusModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int TotalRoleTypeCount { get; set; }
        public int AcceptedRoleTypesCount{ get; set; }
        public int UnderReviewRoleTypesCount { get; set; }
        public int EditedRoleTypesCount { get; set; }
        public int KRARoleTypesNotDefinedCount { get; set; }
        public bool IsEligilbeForReivew { get; set; }
        public string Status { get; set; }

    }
}
