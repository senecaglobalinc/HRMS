using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Types.External
{
    public interface IKRAService
    {
        Task<ServiceListResponse<KRARoleTypes>> GetEmpKRARoleTypeByGrade(int gradeId);
    }
}
