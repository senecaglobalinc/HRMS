using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Infrastructure.Models.Domain;
using HRMS.KRA.Infrastructure.Response;
using System;
using System.Threading.Tasks;

namespace HRMS.KRA.Types
{
    public interface IKRAWorkFlowService
    {
        Task<BaseServiceResponse> UpdateDefinitionDetails(int financialYearId, int departmentId);
        Task<BaseServiceResponse> SendToCEO(int financialYearId);
        Task<BaseServiceResponse> UpdateRoleTypeStatus(int financialYearId, int departmentId);
        Task<BaseServiceResponse> EditByHR(int financialYearId, int departmentId);        
        Task<ServiceListResponse<KRAStatusModel>> GetKRAStatusByFinancialYearId(int financialYearId);
        Task<ServiceListResponse<KRAStatusModel>> GetKRAStatus(int financialYearId, int departmentId);
        Task<ServiceListResponse<KRAStatusModel>> GetKRAStatusByFinancialYearIdForCEO(int financialYearId);
        Task<BaseServiceResponse> UpdateRoleTypeStatusForCEO(int financialYearId, int departmentId, bool isAccepted);
        Task<BaseServiceResponse> SentToHODAsync(KRAWorkFlowModel kRAWorkFlowModel);
        Task<BaseServiceResponse> ApprovedbyHODAsync(KRAWorkFlowModel kRAWorkFlowModel);
        Task<BaseServiceResponse> EditedByHODAsync(KRAWorkFlowModel kRAWorkFlowModel);
        Task<BaseServiceResponse> SentToOpHeadAsync(KRAWorkFlowModel kRAWorkFlowModel);
        Task<BaseServiceResponse> SendToCEOAsync(KRAWorkFlowModel kRAWorkFlowModel);
        Task<BaseServiceResponse> ApprovedByCEOAsync(KRAWorkFlowModel kRAWorkFlowModel);
        Task<BaseServiceResponse> HODAddAsync(DefinitionModel definitionModel);
        Task<BaseServiceResponse> HODUpdateAsync(DefinitionModel definitionModel);
        Task<BaseServiceResponse> HODDeleteAsync(DefinitionModel definitionModel);
        Task<ServiceListResponse<KRAModel>> GetHODDefinitionsAsync(int financialYearId, int gradeRoleTypeId);
        Task<ServiceListResponse<OperationHeadStatusModel>> GetOperationHeadStatusAsync(int financialYearId);
        Task<BaseServiceResponse> AcceptedByOperationHeadAsync(DefinitionModel definitionModel);
        Task<BaseServiceResponse> RejectedByOperationHeadAsync(DefinitionModel definitionModel);
        Task<ServiceListResponse<KRAModel>> GetOperationHeadDefinitionsAsync(int financialYearId, int gradeRoleTypeId);
    }
}

