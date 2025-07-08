using System;

namespace HRMS.KRA.Entities
{
    public class KRAWorkFlow : BaseEntity
    {
        public Guid KRAWorkFlowId { get; set; }
        public int FinancialYearId { get; set; }
        public int RoleTypeId { get; set; }
        public int StatusId { get; set; }
    }
}
