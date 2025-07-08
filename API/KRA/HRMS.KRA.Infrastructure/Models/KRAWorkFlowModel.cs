using System.Collections.Generic;

namespace HRMS.KRA.Infrastructure.Models
{
    public class KRAWorkFlowModel
    {
        public int FinancialYearId { get; set; }
        public int DepartmentId { get; set; }
        public List<int> RoleTypeIds { get; set; }
        public string CurrentUser { get; set; }
    }
}
