using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IAssociateLongLeaveService
    {
        Task<ServiceResponse<bool>> CreateAssociateLongLeave(AssociateLongLeaveData leaveDetails);
        Task<ServiceResponse<bool>> ReJoinAssociateByID(int empID, string reason, string rejoinedDate);
        Task<ServiceResponse<string>> CalculateMaternityPeriod(string maternityStartDate);
    }
}
