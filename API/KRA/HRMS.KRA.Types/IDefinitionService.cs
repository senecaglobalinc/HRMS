using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Infrastructure.Models.Response;
using HRMS.KRA.Infrastructure.Response;

namespace HRMS.KRA.Types
{
    public interface IDefinitionService
    {
        Task<BaseServiceResponse> Create(DefinitionModel model);
        Task<ServiceListResponse<KRAModel>> GetDefinitions(int financialYearId, int roleTypeId);
        Task<BaseServiceResponse> UpdateKRA(DefinitionModel model);
        Task<BaseServiceResponse> ImportKRA(DefinitionModel model);
        Task<ServiceListResponse<KRAModel>> GetKRADefinitionTransactions(int financialYearId, int departmentId, int graderoleTypeId, int? statusid = null);
        Task<ServiceListResponse<KRAModel>> GetKRAFromDefinitionDetails(int financialYearId, int graderoleTypeId, int? statusid = null);
        Task<ServiceListResponse<KRAModel>> GetKRAs(int financialYearId, int departmentId, int gradeId, int roleTypeId, bool IsHOD);
        Task<ServiceListResponse<DefinitionModel>> GetDefinitionDetails(Guid Id);
        Task<ServiceResponse<bool>> Delete(Guid definitionId);
        Task<BaseServiceResponse> DeleteByHOD(int definitionDetailId);
        Task<ServiceResponse<bool>> DeleteKRA(int definitionDetailId);
        Task<BaseServiceResponse> SetPreviousMetricValues(int definitionDetailId);
        Task<BaseServiceResponse> SetPreviousTargetValues(int definitionDetailId);
        Task<BaseServiceResponse> AcceptTargetValue(int defintionTransactionId, string username);
        Task<BaseServiceResponse> AcceptMetricValue(int defintionTransactionId, string username);
        Task<BaseServiceResponse> RejectTargetValue(int defintionTransactionId, string username);
        Task<BaseServiceResponse> RejectMetricValue(int defintionTransactionId, string username);
        Task<BaseServiceResponse> AddKRAAgain(int definitionDetailId);
        Task<BaseServiceResponse> AcceptDeletedKRAByHOD(int defintionTransactionId, string username);
        Task<BaseServiceResponse> RejectDeletedKRAByHOD(int defintionTransactionId, string username);
        Task<BaseServiceResponse> AcceptAddedKRAByHOD(int defintionTransactionId, string username);
        Task<BaseServiceResponse> RejectAddedKRAByHOD(int defintionTransactionId, string username);
        Task<BaseServiceResponse> UpdateKRAStatus(int financialYearId, int gradeRoleTypeId, string status);
    }

}
