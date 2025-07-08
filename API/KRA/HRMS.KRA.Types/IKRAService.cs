using HRMS.KRA.Entities;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.KRA.Types
{
    public interface IKRAService
    {
        Task<List<Operator>> GetOperatorsAsync();
        Task<List<TargetPeriod>> GetTargetPeriodsAsync();
        Task<AssociateKRA> GetKRAPdfData(string employeeCode, int financialYearId, int? departmentId = null, int? roleId = null);
        void GeneratePdfFiles(AssociateKRA associateKRA, string webRootPath);
        Task<List<KRAPdf>> GetKRAPdfsAsync(bool IsActive = true);
        Task<bool> DeleteKRAPdfAsync(Guid kraPdfId);        
    }
}
