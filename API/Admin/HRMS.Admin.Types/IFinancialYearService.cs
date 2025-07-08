using HRMS.Admin.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HRMS.Admin.Infrastructure.Models.Response;
using HRMS.Admin.Infrastructure.Models;

namespace HRMS.Admin.Types
{
    public interface IFinancialYearService
    {
        Task<ServiceListResponse<FinancialYearModel>> GetAll();
        Task<ServiceResponse<FinancialYearModel>> GetByIdAsync(int financialYearId);
        Task<ServiceResponse<FinancialYearModel>> GetCurrentFinancialYear();
    }
}
