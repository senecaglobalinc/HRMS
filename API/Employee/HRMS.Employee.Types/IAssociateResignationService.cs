using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IAssociateResignationService
    {
        Task<ServiceResponse<AssociateResignationData>> GetAssociatesBySearchString(int resignEmployeeId, int employeeID);
        Task<ServiceResponse<bool>> CreateAssociateResignation(AssociateResignationData resignationDetails);
        Task<ServiceResponse<string>> CalculateNoticePeriod(string resignationDate);
        Task<ServiceResponse<bool>> RevokeResignationByID(int empID, string reason, string revokedDate);
    }
}
