using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.KRA.Database;
using HRMS.KRA.Entities;
using HRMS.KRA.Infrastructure;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Infrastructure.Models.Domain;
using HRMS.KRA.Infrastructure.Models.Response;
using HRMS.KRA.Infrastructure.Response;
using HRMS.KRA.Types;
using HRMS.KRA.Types.External;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HRMS.KRA.Service
{
    public class DefinitionService : IDefinitionService
    {
        #region Global Varibles
        private readonly ILogger<DefinitionService> m_Logger;
        private readonly KRAContext m_kraContext;
        private readonly IConfiguration m_configuration;
        private readonly IOrganizationService m_OrganizationService;
        private readonly IKRAService m_kraService;
        private readonly IAspectService m_aspectService;
        private readonly IScaleService m_scaleService;
        private readonly IMeasurementTypeService m_measurementTypeService;
        private readonly IStatusService m_statusService;
        
        #endregion

        #region DefinitionService
        /// <summary>
        /// DefinitionService
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="kraContext"></param>
        public DefinitionService(ILogger<DefinitionService> logger, KRAContext kraContext,
                                IConfiguration configuration, IKRAService kraService,
                                IOrganizationService organizationService, IAspectService aspectService,
                                IScaleService scaleService, IMeasurementTypeService measurementTypeService,
                                IStatusService statusService)
        {
            m_Logger = logger;
            m_kraContext = kraContext;
            m_configuration = configuration;
            m_kraService = kraService;
            m_OrganizationService = organizationService;
            m_aspectService = aspectService;
            m_scaleService = scaleService;
            m_measurementTypeService = measurementTypeService;
            m_statusService = statusService;
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a KRA Definition record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> Create(DefinitionModel model)
        {
            var response = new BaseServiceResponse();
            bool isCreated = false;
            Definition definition = null;
            m_Logger.LogInformation("DefinitionService: Calling \"Create\" method.");

            var definitionExists = m_kraContext.Definitions.SingleOrDefault(x => x.FinancialYearId == model.FinancialYearId && x.RoleTypeId == model.RoleTypeId &&  x.AspectId == model.AspectId && x.Metric == model.Metric);
            if (definitionExists != null)
            {
                response.IsSuccessful = false;
                response.Message = "KRA already defined for this Aspect and Metric with the Role Type.";
            }
            else
            {
                try
                {
                    // This is when Measurement Type is not Scale
                    if (model.ScaleId == 0)
                        model.ScaleId = null;

                    definition = new Definition()
                    {
                        FinancialYearId = model.FinancialYearId,
                        RoleTypeId = model.RoleTypeId,
                        AspectId = model.AspectId,
                        MeasurementTypeId = model.MeasurementTypeId,
                        ScaleId = model.ScaleId,
                        OperatorId = model.OperatorId,
                        Metric = model.Metric,
                        TargetValue = model.TargetValue,
                        TargetPeriodId = model.TargetPeriodId,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = model.CurrentUser,
                        ModifiedBy = model.CurrentUser
                    };

                    m_kraContext.Definitions.Add(definition);
                    isCreated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;

                    if (isCreated)
                    {
                        response.IsSuccessful = true;
                        response.Message = "Definition Record created successfully!.";
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while creating a new Definition record.";
                    }
                }
                catch (Exception ex)
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while creating a new Definition records.";
                    m_Logger.LogError($"Error occured in Create() method in DefinitionService {ex?.StackTrace}");
                }
            }
            return response;
        }
        #endregion

        #region GetDefinitions
        /// <summary>
        /// Gets Definitions based on FinancialYearId and RoleTypeId
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <param name="roleTypeId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<KRAModel>> GetDefinitions(int financialYearId, int roleTypeId)
        {
            var response = new ServiceListResponse<KRAModel>();

            try
            {
                var lstDefinition = await m_kraContext.Definitions.Where(x => x.FinancialYearId == financialYearId 
                                                                    && x.RoleTypeId == roleTypeId)
                                                                    .AsNoTracking().ToListAsync();                             
                
                if (lstDefinition != null && lstDefinition.Count > 0)
                {
                    var lstOperators = await m_kraService.GetOperatorsAsync();
                    var lstMeasurementTypes = await m_measurementTypeService.GetAllAsync();
                    var lstTargetPeriods = await m_kraService.GetTargetPeriodsAsync();
                    var lstAspects = await m_aspectService.GetAllAsync();
                    var lstScale = await m_scaleService.GetAllAsync();
                    var lstScaleDetails = await m_scaleService.GetScaleDetailsAsync();

                    var lstDefinitionModels = (from d in lstDefinition

                                               join aspList in lstAspects
                                               on d.AspectId equals aspList.AspectId into aspectData
                                               from asp in aspectData.DefaultIfEmpty()

                                               join opList in lstOperators
                                               on d.OperatorId equals opList.OperatorId into opData
                                               from op in opData.DefaultIfEmpty()

                                               join mtList in lstMeasurementTypes
                                               on d.MeasurementTypeId equals mtList.Id into mtype
                                               from mt in mtype.DefaultIfEmpty()

                                               join tpList in lstTargetPeriods
                                               on d.TargetPeriodId equals tpList.TargetPeriodId into tperiod
                                               from tp in tperiod.DefaultIfEmpty()

                                               join scList in lstScale
                                               on d.ScaleId equals scList.ScaleID into scale
                                               from sc in scale.DefaultIfEmpty()

                                               orderby asp.AspectName

                                               select new KRAModel
                                               {
                                                   DefinitionId = d.DefinitionId,
                                                   AspectName = asp.AspectName,
                                                   Date = d.ModifiedDate.HasValue ? "Modified on " + d.ModifiedDate.Value.ToString("dddd, dd MMMM yyyy") : "Added on " + d.CreatedDate.Value.ToString("dddd, dd MMMM yyyy"),
                                                   //
                                                   Metric = d.Metric + (sc == null ? "" : "(Scale: " + sc.MinimumScale + "-" + sc.MaximumScale + "; "
                                                   + string.Join("; ",
                                                   (
                                                   from sd in lstScaleDetails
                                                   where sc.ScaleID == sd.ScaleID
                                                   select sd.ScaleValue + "-" + sd.ScaleDescription).ToList()) + ")"),
                                                   //
                                                   Target = op.OperatorValue + " " + d.TargetValue + (mt.MeasurementType == "Percentage" ? "%" : "") + " (" + tp.TargetPeriodValue + ")" ,
                                                   ScaleId = d.ScaleId,
                                                   IsActive = d.IsActive,
                                                   Status = "Draft"
                                               }
                                              ).ToList();

                    //Get the status of RoleTypeId
                    var statusId = await m_kraContext.KRAWorkFlows.Where(wf => wf.FinancialYearId == financialYearId && wf.RoleTypeId == roleTypeId).Select(wf => wf.StatusId).FirstOrDefaultAsync();

                    if (statusId != 0)
                    {
                        string status = "Draft";
                        if (statusId == KRAWorkFlowStatusConstants.SentToHOD) status = "SentToHOD";
                        else if (statusId == KRAWorkFlowStatusConstants.ApprovedbyHOD) status = "ApprovedbyHOD";
                        else if (statusId == KRAWorkFlowStatusConstants.EditedByHOD) status = "EditedByHOD";
                        else if (statusId == KRAWorkFlowStatusConstants.SentToOpHead) status = "SentToOpHead";
                        else if (statusId == KRAWorkFlowStatusConstants.SendToCEO) status = "SendToCEO";
                        else if (statusId == KRAWorkFlowStatusConstants.ApprovedByCEO) status = "ApprovedByCEO";
                        else if (statusId == KRAWorkFlowStatusConstants.SentToAssociates) status = "SentToAssociates";

                        foreach (var definition in lstDefinitionModels)
                        {
                            definition.StatusId = statusId;
                            definition.Status = status;
                        }
                    }

                    response.Items = lstDefinitionModels;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "KRAs not found from Definition table!";

                    int? status = m_kraContext.KRAWorkFlows.Where(x => x.FinancialYearId == financialYearId)
                                                                   .Select(k => k.StatusId).OrderBy(x => x).FirstOrDefault();
                    List<int> statusList = new List<int>(){0,1,2,3,4};                    
                    if (status.HasValue == false || (status.HasValue && status.Value == 0) || (status.HasValue && status.Value == 6))
                    {
                        response.IsSuccessful = false;
                        response.Message = "";
                    }
                    else if (status.HasValue && statusList.Contains(status.Value))
                    {
                        var lstRoleTypesAndDepartments = await m_OrganizationService.GetRoleTypesAndDepartmentsAsync();
                        List<int> roleTypes = lstRoleTypesAndDepartments.Where(c => c.RoleTypeIds.Contains(roleTypeId)).Select(c => c.RoleTypeIds).FirstOrDefault();
                        if (roleTypes != null && roleTypes.Count > 0)
                        {
                            int? departmentStatus = m_kraContext.KRAWorkFlows.Where(x => x.FinancialYearId == financialYearId
                             && roleTypes.Contains(x.RoleTypeId)).Select(k => k.StatusId).OrderBy(x => x).FirstOrDefault();

                            if (departmentStatus.HasValue == false || (departmentStatus.HasValue && departmentStatus.Value == 0) || (departmentStatus.HasValue && departmentStatus.Value == 6))
                            {
                                response.IsSuccessful = false;
                                response.Message = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;

                response.Message = "Error occured while fetching KRAs from Definition table";
                m_Logger.LogError($"{nameof(GetDefinitions)} - Error occured while fetching KRAs from Definition table {ex?.StackTrace}");
            }
            return response;
        }
        #endregion 

        #region Update Definition
        /// <summary>
        /// Update Definition
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> UpdateAsync(DefinitionModel model)
        {
            var response = new BaseServiceResponse();

            var definition = m_kraContext.Definitions.
                                Where(d => d.DefinitionId == model.DefinitionId).FirstOrDefault();

            if (definition is null)
            {
                response.IsSuccessful = false;
                response.Message = "Definition not found for update.";
                return response;
            }

            definition.ScaleId = model.ScaleId;
            definition.MeasurementTypeId = model.MeasurementTypeId;
            definition.OperatorId = model.OperatorId;
            definition.TargetPeriodId = model.TargetPeriodId;
            definition.TargetValue = model.TargetValue;

            var isUpdated = await m_kraContext.SaveChangesAsync() > 0;
            
            if (isUpdated)
            {
                response.IsSuccessful = true;
                response.Message = "Definition record updated successfully!.";
                return response;
            }
            else
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while updating a Definition record.";
                return response;
            }
        }
        #endregion

        #region Delete Definition
        public async Task<ServiceResponse<bool>> DeleteAsync(Guid definitionId)
        {
            ServiceResponse<bool> response;

            m_Logger.LogInformation("DefinitionService: Calling \"Delete\" method");

            var definition = m_kraContext.Definitions.Find(definitionId);
            if (definition == null)
            {
                response = new ServiceResponse<bool>
                {
                    Item = false,
                    IsSuccessful = false,
                    Message = "Definition not found for delete."
                };
                return response;
            }

            m_kraContext.Definitions.Remove(definition);

           var isDeleted = await m_kraContext.SaveChangesAsync() > 0;

            if (isDeleted)
            {
                response = new ServiceResponse<bool>
                {
                    Item = true,
                    IsSuccessful = true,
                    Message = "Definition record deleted."
                };
                return response;
            }
            else
            {
                response = new ServiceResponse<bool>
                {
                    Item = false,
                    IsSuccessful = false,
                    Message = "Definition record could not be deleted."
                };
                return response;
            }
        }
        #endregion 

        #region Create_OLD
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> Create_OLD(DefinitionModel model)
        {
            throw new NotImplementedException();
            /*
            var response = new BaseServiceResponse();
            bool isCreated = false;
            Definition definition = null;
            m_Logger.LogInformation("DefinitionService: Calling \"Create\" method.");

            var definitionExists = m_kraContext.Definitions.SingleOrDefault(x => x.ApplicableRoleTypeId == model.ApplicableRoleTypeId && x.AspectId == model.AspectId);
            if (definitionExists != null)
            {
                var definitionDetailExists = m_kraContext.DefinitionDetails.SingleOrDefault(x => x.Metric == model.Metric && x.DefinitionId == definitionExists.DefinitionId);

                if (definitionDetailExists != null)
                {
                    response.IsSuccessful = false;
                    response.Message = "KRA already defined for this Aspect and Metric with the Applicable Role Type.";
                }
                else
                {
                    var definitionTransactionExists = m_kraContext.DefinitionTransactions.SingleOrDefault(x => x.Metric == model.Metric && x.IsActive == true && x.DefinitionDetails.DefinitionId == definitionExists.DefinitionId);

                    if (definitionTransactionExists != null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "KRA already defined for this Aspect and Metric with the Applicable Role Type.";
                    }
                    else
                    {
                        using (var transaction = m_kraContext.Database.BeginTransaction())
                        {
                            try
                            {
                                // This is when Measurement Type is not Scale
                                if (model.ScaleId == 0)
                                    model.ScaleId = null;

                                // If Definition record is created, go on creating the DefinitionDetail record
                                DefinitionDetails definitionDetails = new DefinitionDetails()
                                {
                                    DefinitionId = definitionExists.DefinitionId,
                                    Metric = model.Metric,
                                    OperatorId = model.OperatorId,
                                    MeasurementTypeId = model.MeasurementTypeId,
                                    ScaleId = model.ScaleId,
                                    TargetValue = model.TargetValue,
                                    TargetPeriodId = model.TargetPeriodId,
                                    IsDeleted = null,
                                };

                                m_kraContext.DefinitionDetails.Add(definitionDetails);
                                isCreated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;

                                if (isCreated)
                                {
                                    // If DefinitionDetail record is created, go on creating the DefinitionTransaction record
                                    DefinitionTransaction definitionTransaction = new DefinitionTransaction()
                                    {
                                        DefinitionDetailsId = definitionDetails.DefinitionDetailsId,
                                        Metric = model.Metric,
                                        OperatorId = model.OperatorId,
                                        MeasurementTypeId = model.MeasurementTypeId,
                                        ScaleId = model.ScaleId,
                                        TargetValue = model.TargetValue,
                                        TargetPeriodId = model.TargetPeriodId,
                                        IsActive = true,
                                        IsDeleted = null,
                                        CreatedBy = model.Username,
                                    };

                                    m_kraContext.DefinitionTransactions.Add(definitionTransaction);
                                    isCreated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;

                                    if (isCreated)
                                    {
                                        if (model.IsHOD.HasValue && model.IsHOD.Value)
                                        {
                                            var appplicableroletype = m_kraContext.ApplicableRoleTypes.FirstOrDefault(x => x.ApplicableRoleTypeId == model.ApplicableRoleTypeId);
                                            appplicableroletype.StatusId = StatusConstants.EditedByHOD;
                                            isCreated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                                        }
                                        if (isCreated)
                                        {
                                            transaction.Commit();
                                            response.IsSuccessful = true;
                                            response.Message = "Definition Records created successfully!.";
                                        }
                                    }
                                }


                                // If one of the record creation failed, rollback the transaction
                                if (!isCreated)
                                {
                                    transaction.Rollback();
                                    response.IsSuccessful = false;
                                    response.Message = "Error occurred while creating a new Definition records.";
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                response.IsSuccessful = false;
                                response.Message = "Error occurred while creating a new Definition records.";
                                m_Logger.LogError("Error occured in Create() method in DefinitionService " + ex.StackTrace);
                            }
                        }
                    }
                }
            }
            else
            {
                using (var transaction = m_kraContext.Database.BeginTransaction())
                {
                    try
                    {
                        // Creation of new Definition record
                        definition = new Definition()
                        {
                            ApplicableRoleTypeId = model.ApplicableRoleTypeId,
                            AspectId = model.AspectId,
                            IsActive = true,
                            IsHODApproved = null,
                            IsCEOApproved = null,
                            IsDeleted = false,
                            SourceDefinitionId = null,
                            CreatedBy=model.Username,
                        };

                        m_kraContext.Definitions.Add(definition);
                        isCreated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;

                        // This is when Measurement Type is not Scale
                        if (model.ScaleId == 0)
                            model.ScaleId = null;

                        if (isCreated)
                        {
                            // If Definition record is created, go on creating the DefinitionDetail record
                            DefinitionDetails definitionDetails = new DefinitionDetails()
                            {
                                DefinitionId = definition.DefinitionId,
                                Metric = model.Metric,
                                OperatorId = model.OperatorId,
                                MeasurementTypeId = model.MeasurementTypeId,
                                ScaleId = model.ScaleId,
                                TargetValue = model.TargetValue,
                                TargetPeriodId = model.TargetPeriodId,
                                IsDeleted = null,                               
                            };

                            m_kraContext.DefinitionDetails.Add(definitionDetails);
                            isCreated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;

                            if (isCreated)
                            {
                                // If DefinitionDetail record is created, go on creating the DefinitionTransaction record
                                DefinitionTransaction definitionTransaction = new DefinitionTransaction()
                                {
                                    DefinitionDetailsId = definitionDetails.DefinitionDetailsId,
                                    Metric = model.Metric,
                                    OperatorId = model.OperatorId,
                                    MeasurementTypeId = model.MeasurementTypeId,
                                    ScaleId = model.ScaleId,
                                    TargetValue = model.TargetValue,
                                    TargetPeriodId = model.TargetPeriodId,
                                    IsActive = true,
                                    IsDeleted = null,
                                    CreatedBy = model.Username,
                                };

                                m_kraContext.DefinitionTransactions.Add(definitionTransaction);
                                isCreated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;

                                if (isCreated)
                                {
                                    if (model.IsHOD.HasValue && model.IsHOD.Value)
                                    {
                                        var appplicableroletype = m_kraContext.ApplicableRoleTypes.FirstOrDefault(x => x.ApplicableRoleTypeId == model.ApplicableRoleTypeId);
                                        appplicableroletype.StatusId = StatusConstants.EditedByHOD;
                                        isCreated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                                    }
                                    if (isCreated)
                                    {
                                        transaction.Commit();
                                        response.IsSuccessful = true;
                                        response.Message = "Definition Records created successfully!.";
                                    }
                                }
                            }
                        }

                        // If one of the record creation failed, rollback the transaction
                        if (!isCreated)
                        {
                            transaction.Rollback();
                            response.IsSuccessful = false;
                            response.Message = "Error occurred while creating a new Definition records.";
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while creating a new Definition records.";
                        m_Logger.LogError("Error occured in Create() method in DefinitionService " + ex.StackTrace);
                    }
                }
            }
            return response;*/
        }
        #endregion
        

        #region ImportKRA
        /// <summary>
        /// ImportKRA
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> ImportKRA(DefinitionModel model)
        {
            throw new NotImplementedException();
            /*
            var response = new BaseServiceResponse();
            bool isCreated = false;
            Definition definition = null;
            m_Logger.LogInformation("DefinitionService: Calling \"Create\" method.");

            var details = await (from sm in m_kraContext.DefinitionDetails
                                 where model.definitionDetailsIds.Contains(sm.DefinitionDetailsId)
                                 select new DefinitionDetails
                                            {
                                     DefinitionId = sm.DefinitionId,
                                     Metric = sm.Metric,                                  
                                     OperatorId = sm.OperatorId,
                                     MeasurementTypeId = sm.MeasurementTypeId,
                                     ScaleId = sm.ScaleId,
                                     TargetValue = sm.TargetValue,
                                     TargetPeriodId = sm.TargetPeriodId,
                                 }).ToListAsync();
           
            List<int> definitionIds = details.GroupBy(c => c.DefinitionId).Select(c => c.Key).ToList<int>();

            var lstDefinitions = await (from sm in m_kraContext.Definitions
                                        where definitionIds.Contains(sm.DefinitionId)
                                        select new Definition
                                        {
                                            DefinitionId = sm.DefinitionId,
                                            AspectId = sm.AspectId,

                                        }).ToListAsync();                    

            Definition existingdefinition = null;
            using (var transaction = m_kraContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (DefinitionDetails detail in details)
                    {
                        existingdefinition = lstDefinitions.Where(c => c.DefinitionId == detail.DefinitionId).FirstOrDefault();
                        // Creation of new Definition record
                        definition = new Definition()
                        {
                            ApplicableRoleTypeId = model.ApplicableRoleTypeId,
                            AspectId = existingdefinition.AspectId,
                            IsActive = true,
                            IsHODApproved = null,
                            IsCEOApproved = null,
                            IsDeleted = false,
                            SourceDefinitionId = null,
                            CreatedBy = model.Username,
                        };

                        m_kraContext.Definitions.Add(definition);
                        isCreated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;

                        if (isCreated)
                        {
                            // If Definition record is created, go on creating the DefinitionDetail record
                            DefinitionDetails definitionDetails = new DefinitionDetails()
                            {
                                DefinitionId = definition.DefinitionId,
                                Metric = detail.Metric,
                                OperatorId = detail.OperatorId,
                                MeasurementTypeId = detail.MeasurementTypeId,
                                ScaleId = detail.ScaleId,
                                TargetValue = detail.TargetValue,
                                TargetPeriodId = detail.TargetPeriodId,
                                IsDeleted = null,
                            };

                            m_kraContext.DefinitionDetails.Add(definitionDetails);
                            isCreated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;

                            if (isCreated)
                            {
                                // If DefinitionDetail record is created, go on creating the DefinitionTransaction record
                                DefinitionTransaction definitionTransaction = new DefinitionTransaction()
                                {
                                    DefinitionDetailsId = definitionDetails.DefinitionDetailsId,
                                    Metric = detail.Metric,
                                    OperatorId = detail.OperatorId,
                                    MeasurementTypeId = detail.MeasurementTypeId,
                                    ScaleId = detail.ScaleId,
                                    TargetValue = detail.TargetValue,
                                    TargetPeriodId = detail.TargetPeriodId,
                                    IsActive = true,
                                    IsDeleted = null,
                                    CreatedBy = model.Username,
                                };

                                m_kraContext.DefinitionTransactions.Add(definitionTransaction);
                                isCreated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                            }
                        }                        
                    }
                    if (isCreated)
                    {
                        transaction.Commit();
                        response.IsSuccessful = true;
                        response.Message = "Definition Records created successfully!.";
                    }
                    // If one of the record creation failed, rollback the transaction
                    if (!isCreated)
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while creating a new Definition records.";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while creating a new Definition records.";
                    m_Logger.LogError("Error occured in Create() method in DefinitionService " + ex.StackTrace);
                }
            }

            return response;
            */
        }
        #endregion

        #region UpdateKRA
        /// <summary>
        /// Update KRA
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> UpdateKRA(DefinitionModel model)
        {
            var response = new BaseServiceResponse();
            
            bool isUpdated = false;

            var existingdefinition = await (from sm in m_kraContext.Definitions
                                            where sm.DefinitionId == model.DefinitionId
                                            select new Definition
                                            {
                                                AspectId = sm.AspectId,
                                                RoleTypeId = sm.RoleTypeId,
                                            }).FirstOrDefaultAsync();


            if (existingdefinition == null)
            {
                response.IsSuccessful = false;
                response.Message = "definitions not found for update.";
                return response;
            }

            var existingDefinitionDetail = await (from sm in m_kraContext.Definitions
                                                  where sm.DefinitionId != model.DefinitionId                                                  
                                                  && sm.Metric == model.Metric
                                                  && sm.AspectId == model.AspectId
                                                  && sm.RoleTypeId == model.RoleTypeId
                                                  && sm.FinancialYearId == model.FinancialYearId
                                                  select new
                                                  {
                                                      DefinitionId = sm.DefinitionId
                                                  }).ToListAsync();

            if (existingDefinitionDetail != null && existingDefinitionDetail.Count > 0)
            {
                response.IsSuccessful = false;
                response.Message = "KRA already defined for this Aspect and Metric with the Role Type.";
                return response;
            }

            using (var transaction = m_kraContext.Database.BeginTransaction())
            {
                try
                {
                    var definition = m_kraContext.Definitions.Find(model.DefinitionId);  

                    // This is when Measurement Type is not Scale
                    if (model.ScaleId == 0)
                        model.ScaleId = null;

                    definition.AspectId = model.AspectId;
                    definition.MeasurementTypeId = model.MeasurementTypeId;
                    definition.ScaleId = model.ScaleId;
                    definition.OperatorId = model.OperatorId;
                    definition.Metric = model.Metric;
                    definition.TargetValue = model.TargetValue;
                    definition.TargetPeriodId = model.TargetPeriodId;
                    definition.IsActive = true;

                    m_kraContext.Definitions.Update(definition);
                    isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;


                    if (isUpdated)
                    {
                        transaction.Commit();
                        response.IsSuccessful = true;
                        response.Message = "Records updated successfully!.";
                        return response;
                    }
                    else
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while updating a kra record.";
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating a kra record.";
                    m_Logger.LogError("Error occured in upateRA() method in DefinitionService " + ex.StackTrace);
                    return response;
                }
            }
        }
        #endregion


        #region UpdateKRAStatus
        /// <summary>
        /// UpdateKRAStatus
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <param name="roleTypeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> UpdateKRAStatus(int financialYearId, int roleTypeId,  string status)
        {
            var response = new BaseServiceResponse();
            
            bool isUpdated = false;

            List<Definition> definitions = m_kraContext.Definitions.Where(c => c.FinancialYearId == financialYearId && c.RoleTypeId == roleTypeId).ToList();

            if (definitions == null || definitions.Count == 0)
            {
                response.IsSuccessful = false;
                response.Message = "definitions not found for update.";
                return response;
            }
            if (status == "FD")
            {
                var finishedDraftingStatus = m_kraContext.Statuses.Where(c => c.StatusText == "FinishedDrafting").FirstOrDefault<Status>();
                if (finishedDraftingStatus == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "FinishedDrafting status not found";
                    return response;
                }
                //foreach(Definition definition in definitions)
                //definition.StatusId = finishedDraftingStatus.StatusId;
            }

                     

            using (var transaction = m_kraContext.Database.BeginTransaction())
            {
                try
                {                    
                    
                    m_kraContext.Definitions.UpdateRange(definitions);
                    isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;


                    if (isUpdated)
                    {
                        transaction.Commit();
                        response.IsSuccessful = true;
                        response.Message = "Records updated successfully!.";
                        return response;
                    }
                    else
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while updating a kra record.";
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating a kra record.";
                    m_Logger.LogError("Error occured in upateRA() method in DefinitionService " + ex.StackTrace);
                    return response;                    
                }
            }
        }
        #endregion

        #region GetKRADefinitionTransactions
        /// <summary>
        /// Gets KRAs from Definition Transaction table.
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<KRAModel>> GetKRADefinitionTransactions(int financialYearId, int departmentId, int roleTypeId,int ?statusid=null)
        {
          //  throw new NotImplementedException();
            
            var response = new ServiceListResponse<KRAModel>();
            try
            {                

                var lstDefinitions = await m_kraContext.Definitions
                                                     .Where(x => x.IsActive == true &&
                                                     x.FinancialYearId == financialYearId &&
                                                     x.RoleTypeId == roleTypeId).ToListAsync();

                var lstOperators = await m_kraService.GetOperatorsAsync();
                var lstMeasurementTypes = await m_measurementTypeService.GetAllAsync();
                var lstTargetPeriods = await m_kraService.GetTargetPeriodsAsync();
                var lstAspects = await m_aspectService.GetAllAsync();
                var lstScale = await m_scaleService.GetAllAsync();
                var lstScaleDetails = await m_scaleService.GetScaleDetailsAsync();
                var lstStatus = await m_statusService.GetAllAsync();
                //string status = string.Empty;
                //if (statusid == StatusConstants.FinishedDrafting) status = "FD";
                //else if (statusid == StatusConstants.Draft) status = "Draft";
                //else if (statusid == StatusConstants.EditedByHOD) status = "EditedByHOD";
                //else if (statusid == StatusConstants.FinishedEditByHOD) status = "FinishedEditByHOD";
                //else if (statusid == StatusConstants.ApprovedbyHOD) status = "ApprovedbyHOD";
                //else if (statusid == StatusConstants.SentToHOD) status = "SentToHOD";
                //else if (statusid == StatusConstants.SentToHR) status = "SentToHR";
                //else if (statusid == StatusConstants.ApprovedByCEO) status = "ApprovedByCEO";
                //else if (statusid == StatusConstants.FinishedEditByHR) status = "FinishedEditByHR";
                //else if (statusid == StatusConstants.EditByHR) status = "EditByHR";
                if (lstDefinitions != null && lstDefinitions.Count > 0 &&
                    statusid != StatusConstants.EditedByHOD)
                {
                    var lsDefinitionTransactions = (from dt in lstDefinitions

                                                   
                                                    join aspList in lstAspects
                                                    on dt.AspectId equals aspList.AspectId into aspectData
                                                    from asp in aspectData.DefaultIfEmpty()

                                                    join opList in lstOperators
                                                    on dt.OperatorId equals opList.OperatorId into opData
                                                    from op in opData.DefaultIfEmpty()

                                                    join mtList in lstMeasurementTypes
                                                    on dt.MeasurementTypeId equals mtList.Id into mtype
                                                    from mt in mtype.DefaultIfEmpty()

                                                    join tpList in lstTargetPeriods
                                                    on dt.TargetPeriodId equals tpList.TargetPeriodId into tperiod
                                                    from tp in tperiod.DefaultIfEmpty()

                                                    join scList in lstScale
                                                    on dt.ScaleId equals scList.ScaleID into scale
                                                    from sc in scale.DefaultIfEmpty()

                                                    //join scStatus in lstStatus
                                                    //on dt.StatusId equals scStatus.StatusId into status
                                                    //from st in status.DefaultIfEmpty()

                                                    select new KRAModel
                                                    {
                                                        DefinitionId = dt.DefinitionId,
                                                        AspectName = asp.AspectName,
                                                        Date = dt.ModifiedDate.HasValue ? "Modified on " + dt.ModifiedDate.Value.ToString("dddd, dd MMMM yyyy") : "Added on " + dt.CreatedDate.Value.ToString("dddd, dd MMMM yyyy"),
                                                        //
                                                        Metric = dt.Metric + (sc == null ? "" : "(Scale: " + sc.MinimumScale + "-" + sc.MaximumScale + "; "
                                                        + string.Join("; ",
                                                        (
                                                        from sd in lstScaleDetails
                                                        where sc.ScaleID == sd.ScaleID
                                                        select sd.ScaleValue + "-" + sd.ScaleDescription).ToList()) + ")"),
                                                        //
                                                        Target =(mt.MeasurementType== "Percentage")?(op.OperatorValue + " " + dt.TargetValue + "% (" + tp.TargetPeriodValue + ")"):
                                                        (op.OperatorValue + " " + dt.TargetValue + " (" + tp.TargetPeriodValue + ")"),
                                                        ScaleId = dt.ScaleId,
                                                        IsActive = dt.IsActive,
                                                        //Status = st.StatusText
                                                    }).ToList();

                    response.Items = lsDefinitionTransactions;
                    response.IsSuccessful = true;
                }
                else if (lstDefinitions != null && lstDefinitions.Count > 0 &&
                     statusid == StatusConstants.EditedByHOD)
                {
                    var userroles = await m_OrganizationService.GetUserRolesAsync();
                    if (!userroles.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Items = null;
                        response.Message = userroles.Message;
                        return response;
                    }                                    
                   
                    var lsDefinitionTransactions = (from def in lstDefinitions

                                                  

                                                    join aspList in lstAspects
                                                    on def.AspectId equals aspList.AspectId into aspectData
                                                    from asp in aspectData.DefaultIfEmpty()

                                                 
                                                    //getting data from definationdetails table
                                                    join opList in lstOperators
                                                    on def.OperatorId equals opList.OperatorId into defopData
                                                    from defop in defopData.DefaultIfEmpty()

                                                    join mtList in lstMeasurementTypes
                                                    on def.MeasurementTypeId equals mtList.Id into defmtype
                                                    from defmt in defmtype.DefaultIfEmpty()

                                                    join tpList in lstTargetPeriods
                                                    on def.TargetPeriodId equals tpList.TargetPeriodId into deftperiod
                                                    from deftp in deftperiod.DefaultIfEmpty()

                                                    join scList in lstScale
                                                    on def.ScaleId equals scList.ScaleID into defscale
                                                    from defsc in defscale.DefaultIfEmpty()
                                                        //
                                                    join opList in lstOperators
                                                    on def.OperatorId equals opList.OperatorId into opData
                                                    from op in opData.DefaultIfEmpty()

                                                    join mtList in lstMeasurementTypes
                                                    on def.MeasurementTypeId equals mtList.Id into mtype
                                                    from mt in mtype.DefaultIfEmpty()

                                                    join tpList in lstTargetPeriods
                                                    on def.TargetPeriodId equals tpList.TargetPeriodId into tperiod
                                                    from tp in tperiod.DefaultIfEmpty()

                                                    join scList in lstScale
                                                    on def.ScaleId equals scList.ScaleID into scale
                                                    from sc in scale.DefaultIfEmpty()

                                                    //join scStatus in lstStatus
                                                    //on def.StatusId equals scStatus.StatusId into status
                                                    //from st in status.DefaultIfEmpty()

                                                    join ur in userroles.Items
                                                    on def.CreatedBy equals ur.Username into crData
                                                    from cduserrole in crData.DefaultIfEmpty()

                                                    join ur in userroles.Items
                                                    on def.ModifiedBy equals ur.Username into mdData
                                                    from mduserrole in mdData.DefaultIfEmpty()

                                                    select new KRAModel
                                                    {
                                                        DefinitionId = def.DefinitionId,                                                       
                                                        AspectName = asp.AspectName,
                                                        Date = def.ModifiedDate.HasValue ? "Modified on " + def.ModifiedDate.Value.ToString("dddd, dd MMMM yyyy") : "Added on " + def.CreatedDate.Value.ToString("dddd, dd MMMM yyyy"),
                                                        DefinitionDate = def.ModifiedDate,
                                                        //
                                                        Metric = def.Metric + (sc == null ? "" : "(Scale: " + sc.MinimumScale + "-" + sc.MaximumScale + "; "
                                                        + string.Join("; ",
                                                        (
                                                        from sd in lstScaleDetails
                                                        where sc.ScaleID == sd.ScaleID
                                                        select sd.ScaleValue + "-" + sd.ScaleDescription).ToList()) + ")"),

                                                        PreviousMetric = def.Metric + (defsc == null ? "" : "(Scale: " + defsc.MinimumScale + "-" + defsc.MaximumScale + "; "
                                                        + string.Join("; ",
                                                        (
                                                        from sd in lstScaleDetails
                                                        where defsc.ScaleID == sd.ScaleID
                                                        select sd.ScaleValue + "-" + sd.ScaleDescription).ToList()) + ")"),                                                                                                                
                                                        Target = op.OperatorValue + " " + def.TargetValue + " " + tp.TargetPeriodValue,
                                                        previousration = (op.OperatorValue != defop.OperatorValue ||
                                                        tp.TargetPeriodValue != deftp.TargetPeriodValue
                                                        ||
                                                        def.TargetValue != def.TargetValue) ? (defop.OperatorValue + " " + def.TargetValue + " " + deftp.TargetPeriodValue) : null,
                                                        ScaleId = def.ScaleId,
                                                        //Status = st.StatusText,
                                                        IsActive = def.IsActive,
                                                        CreatedBy = def.CreatedBy,
                                                        ModifiedBy = def.ModifiedBy,
                                                        ModifiedDate= def.ModifiedDate,
                                                        CreatedDate = def.CreatedDate,
                                                        CreatedByUserRole = cduserrole!=null ?(cduserrole.Roles.Contains("HRM") ? "HRM" : (cduserrole.Roles.Contains("Department Head")) ? "HOD" : null): null,
                                                        ModifiedByUserRole = mduserrole!=null ?(mduserrole.Roles.Contains("HRM") ? "HRM" : (mduserrole.Roles.Contains("Department Head")) ? "HOD" : null) : null,
                                                    }).ToList();
                    #region Bubble calculation
                    int previousCnt = 0;                    
                    lsDefinitionTransactions = lsDefinitionTransactions.OrderBy(c => c.ModifiedDate).OrderBy(c=>c.CreatedDate).ToList();
                    foreach (KRAModel model in lsDefinitionTransactions)
                    {
                        if (model.Metric == model.PreviousMetric) model.PreviousMetric = null;

                        if (model.ModifiedByUserRole == "HOD" || model.CreatedByUserRole=="HOD")
                        {
                            if (model.previousration != null)
                            {
                                if (previousCnt > 0)
                                    model.ModifiedTargetCount = previousCnt = previousCnt + 1;
                                else model.ModifiedTargetCount = previousCnt = 1;
                            }
                            if (model.PreviousMetric != null)
                            {
                                if (previousCnt > 0)
                                    model.ModifiedMetricCount = previousCnt = previousCnt + 1;
                                else model.ModifiedMetricCount = previousCnt = 1;
                            }                            
                            if (model.CreatedByUserRole == "HOD" && (model.ModifiedByUserRole==null))
                            {
                              //  model.isAdded = true;
                                if (previousCnt > 0)
                                    model.CreateCount = previousCnt = previousCnt + 1;
                                else model.CreateCount = previousCnt = 1;
                            }
                        }
                        //if (model.isDeleted.HasValue)
                        //{
                        //    if (previousCnt > 0)
                        //        model.DeleteCount = previousCnt = 1 + previousCnt;
                        //    else model.DeleteCount = previousCnt = 1;
                        //}
                    }

                    #endregion

                    response.Items = lsDefinitionTransactions;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "KRAs not found from Definition Transaction table!";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Items = null;
                response.Message = "Error occured while fetching KRAs from Definition Transaction table";
                m_Logger.LogError("Error occured while fetching KRAs from Definition Transaction table" + ex.StackTrace);
            }
            return response;
            
        }
        #endregion

        #region GetKRAFromDefinitionDetails
        /// <summary>
        /// Gets KRAs from Definition Details.
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<KRAModel>> GetKRAFromDefinitionDetails(int financialYearId, int roleTypeId, int? statusid = null)
        {           
            
            var response = new ServiceListResponse<KRAModel>();
            try
            {                
                var lstDefinition = await m_kraContext.Definitions.Where(x => x.FinancialYearId == financialYearId &&                                                  
                                                  x.RoleTypeId == roleTypeId
                                                  && x.IsActive == true).ToListAsync();

                var lstOperators = await m_kraService.GetOperatorsAsync();
                var lstMeasurementTypes = await m_measurementTypeService.GetAllAsync();
                var lstTargetPeriods = await m_kraService.GetTargetPeriodsAsync();
                var lstAspects = await m_aspectService.GetAllAsync();
                var lstScale = await m_scaleService.GetAllAsync();
                var lstScaleDetails = await m_scaleService.GetScaleDetailsAsync();
                var lstStatus = await m_statusService.GetAllAsync();

                //string status = string.Empty;
                //if (statusid == StatusConstants.FinishedDrafting) status = "FD";
                //else if (statusid == StatusConstants.Draft) status = "Draft";
                //else if (statusid == StatusConstants.SentToHOD) status = "SentToHOD";
                //else if (statusid == StatusConstants.SentToHR) status = "SentToHR";
                //else if (statusid == StatusConstants.EditedByHOD) status = "EditedByHOD";
                //else if (statusid == StatusConstants.FinishedEditByHOD) status = "FinishedEditByHOD";
                //else if (statusid == StatusConstants.ApprovedbyHOD) status = "ApprovedbyHOD";
                //else if (statusid == StatusConstants.ApprovedByCEO) status = "ApprovedByCEO";
                //else if (statusid == StatusConstants.FinishedEditByHR) status = "FinishedEditByHR";
                //else if (statusid == StatusConstants.EditByHR) status = "EditByHR";

                if (lstDefinition != null && lstDefinition.Count > 0)
                {
                    var lstKRAs = (from dd in lstDefinition

                                   join aspList in lstAspects
                                   on dd.AspectId equals aspList.AspectId into aspectData
                                   from asp in aspectData.DefaultIfEmpty()

                                   join opList in lstOperators
                                   on dd.OperatorId equals opList.OperatorId into opData
                                   from op in opData.DefaultIfEmpty()

                                   join mtList in lstMeasurementTypes
                                   on dd.MeasurementTypeId equals mtList.Id into mtype
                                   from mt in mtype.DefaultIfEmpty()

                                   join tpList in lstTargetPeriods
                                   on dd.TargetPeriodId equals tpList.TargetPeriodId into tperiod
                                   from tp in tperiod.DefaultIfEmpty()

                                   join scList in lstScale
                                   on dd.ScaleId equals scList.ScaleID into scale
                                   from sc in scale.DefaultIfEmpty()

                                   //join scStatus in lstStatus
                                   //on dd.StatusId equals scStatus.StatusId into status
                                   //from st in status.DefaultIfEmpty()

                                   select new KRAModel
                                   {
                                       DefinitionId = dd.DefinitionId,
                                       AspectName = asp.AspectName,
                                       Date = dd.ModifiedDate.HasValue ? "Modified on " + dd.ModifiedDate.Value.ToString("dddd, dd MMMM yyyy") : "Added on " + dd.CreatedDate.Value.ToString("dddd, dd MMMM yyyy"),
                                       //
                                       Metric = dd.Metric + (sc == null ? "" : "(Scale: " + sc.MinimumScale + "-" + sc.MaximumScale + "; "
                                       + string.Join("; ",
                                       (
                                       from sd in lstScaleDetails
                                       where sc.ScaleID == sd.ScaleID
                                       select sd.ScaleValue + "-" + sd.ScaleDescription).ToList()) + ")"),
                                       //
                                       Target = op.OperatorValue + " " + dd.TargetValue + " " + tp.TargetPeriodValue,
                                       ScaleId = dd.ScaleId,
                                       //Status= st.StatusText,
                                       IsActive = dd.IsActive
                                   }).ToList();

                    response.Items = lstKRAs;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "KRAs not found from Definition Details table!";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;

                response.Message = "Error occured while fetching KRAs from Definition Details table";
                m_Logger.LogError("Error occured while fetching KRAs from Definition Details table" + ex.StackTrace);
            }
            return response;
            
        }
        #endregion      

        #region GetKRAFromDefinitionAndTransactions
        /// <summary>
        /// Gets KRAs from Definitions and Definition Transactions.
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<KRAModel>> GetKRAFromDefinition(int financialYearId, int departmentId, int graderoleTypeId, int? statusid = null)
        {
            throw new NotImplementedException();
            /*
            var response = new ServiceListResponse<KRAModel>();
            List<KRAModel> lstKRAs = null;
            try
            {
                var lstDefinitions = await m_kraContext.Definitions
                                                 .Where(x => x.ApplicableRoleType.FinancialYearId == financialYearId &&
                                                  x.ApplicableRoleType.DepartmentId == departmentId &&
                                                  x.ApplicableRoleType.GradeRoleTypeId == graderoleTypeId
                                                  && x.IsActive == true).ToListAsync();

               
                List<int> hodApproveddefinitionIds = lstDefinitions.Where(c=>c.IsHODApproved.HasValue &&
                                                                          c.IsHODApproved.Value==true).GroupBy(c => c.DefinitionId).Select(c => c.Key).ToList<int>();

                List<int> hodnotApproveddefinitionIds = lstDefinitions.Where(c =>(!c.IsHODApproved.HasValue)
                || (c.IsHODApproved.HasValue && c.IsHODApproved.Value==false)).GroupBy(c => c.DefinitionId).Select(c => c.Key).ToList<int>();

                var lstOperators = await m_kraService.GetOperators();
                var lstMeasurementTypes = await m_measurementTypeService.GetAll();
                var lstTargetPeriods = await m_kraService.GetTargetPeriods();
                var lstAspects = await m_aspectService.GetAll();
                var lstScale = await m_scaleService.GetAll();
                var lstScaleDetails = await m_scaleService.GetScaleDetails();
                string status = string.Empty;
                if (statusid == StatusConstants.FinishedDrafting) status = "FD";
                else if (statusid == StatusConstants.Draft) status = "Draft";
                else if (statusid == StatusConstants.SentToHOD) status = "SentToHOD";
                else if (statusid == StatusConstants.SentToHR) status = "SentToHR";
                else if (statusid == StatusConstants.EditedByHOD) status = "EditedByHOD";
                else if (statusid == StatusConstants.FinishedEditByHOD) status = "FinishedEditByHOD";
                else if (statusid == StatusConstants.ApprovedbyHOD) status = "ApprovedbyHOD";
                else if (statusid == StatusConstants.ApprovedByCEO) status = "ApprovedByCEO";
                else if (statusid == StatusConstants.FinishedEditByHR) status = "FinishedEditByHR";
                else if (statusid == StatusConstants.EditByHR) status = "EditByHR";

                if (hodApproveddefinitionIds != null && hodApproveddefinitionIds.Count > 0)
                {                    
                    var lstDefinitionDetails = await m_kraContext.DefinitionDetails
                                                     .Where(x => hodApproveddefinitionIds.Contains(x.DefinitionId)).ToListAsync();
                    status = "ApprovedbyHOD"; 
                    if (lstDefinitionDetails != null && lstDefinitionDetails.Count > 0)
                    {
                         lstKRAs = (from dd in lstDefinitionDetails

                                       join aspList in lstAspects
                                       on dd.Definition.AspectId equals aspList.AspectId into aspectData
                                       from asp in aspectData.DefaultIfEmpty()

                                       join opList in lstOperators
                                       on dd.OperatorId equals opList.OperatorId into opData
                                       from op in opData.DefaultIfEmpty()

                                       join mtList in lstMeasurementTypes
                                       on dd.MeasurementTypeId equals mtList.Id into mtype
                                       from mt in mtype.DefaultIfEmpty()

                                       join tpList in lstTargetPeriods
                                       on dd.TargetPeriodId equals tpList.TargetPeriodId into tperiod
                                       from tp in tperiod.DefaultIfEmpty()

                                       join scList in lstScale
                                       on dd.ScaleId equals scList.ScaleID into scale
                                       from sc in scale.DefaultIfEmpty()

                                       select new KRAModel
                                       {
                                           DefinitionDetailsId = dd.DefinitionDetailsId,
                                           DefinitionId = dd.DefinitionId,
                                           AspectName = asp.AspectName,
                                           Date = dd.ModifiedDate.HasValue ? "Modified on " + dd.ModifiedDate.Value.ToString("dddd, dd MMMM yyyy") : "Added on " + dd.CreatedDate.Value.ToString("dddd, dd MMMM yyyy"),
                                           //
                                           Metric = dd.Metric + (sc == null ? "" : "(Scale: " + sc.MinimumScale + "-" + sc.MaximumScale + "; "
                                           + string.Join("; ",
                                           (
                                           from sd in lstScaleDetails
                                           where sc.ScaleID == sd.ScaleID
                                           select sd.ScaleValue + "-" + sd.ScaleDescription).ToList()) + ")"),
                                           //
                                           ration = op.OperatorValue + " " + dd.TargetValue + " " + tp.TargetPeriodValue,
                                           ScaleId = dd.ScaleId,
                                           Status = status
                                       }).ToList();
                    }
                }
                else if (hodnotApproveddefinitionIds != null && hodnotApproveddefinitionIds.Count > 0)
                {
                    status = "EditByHR";

                    var lstDetails = await m_kraContext.DefinitionDetails
                                                .Where(x => hodnotApproveddefinitionIds.Contains(x.DefinitionId))
                                                .ToListAsync();

                    List<int> definitiondetailIds = lstDetails.GroupBy(c => c.DefinitionDetailsId).Select(c => c.Key).ToList<int>();

                    var lstDefinitionTransaction = await m_kraContext.DefinitionTransactions
                                                    .Where(x => x.IsActive == true &&
                                                  definitiondetailIds.Contains(x.DefinitionDetails.DefinitionDetailsId)).ToListAsync();

                    var lstDefinitionDetails = await m_kraContext.DefinitionDetails
                                               .Where(x => definitiondetailIds.Contains(x.DefinitionDetailsId)).ToListAsync();

                    var userroles = await m_OrganizationService.GetUserRoles();
                    if (!userroles.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Items = null;
                        response.Message = userroles.Message;
                        return response;
                    }

                    if (lstDefinitionTransaction != null && lstDefinitionTransaction.Count > 0)
                    {
                        var lsDefinitionTransactions = (from dt in lstDefinitionTransaction

                                                        join aspList in lstAspects
                                                        on dt.DefinitionDetails.Definition.AspectId equals aspList.AspectId into aspectData
                                                        from asp in aspectData.DefaultIfEmpty()

                                                        join defdetail in lstDefinitionDetails
                                                        on dt.DefinitionDetailsId equals defdetail.DefinitionDetailsId into defdetailData
                                                        from def in defdetailData.DefaultIfEmpty()

                                                        join opList in lstOperators
                                                        on def.OperatorId equals opList.OperatorId into defopData
                                                        from defop in defopData.DefaultIfEmpty()

                                                        join mtList in lstMeasurementTypes
                                                        on def.MeasurementTypeId equals mtList.Id into defmtype
                                                        from defmt in defmtype.DefaultIfEmpty()

                                                        join tpList in lstTargetPeriods
                                                        on def.TargetPeriodId equals tpList.TargetPeriodId into deftperiod
                                                        from deftp in deftperiod.DefaultIfEmpty()

                                                        join scList in lstScale
                                                        on def.ScaleId equals scList.ScaleID into defscale
                                                        from defsc in defscale.DefaultIfEmpty()

                                                        join opList in lstOperators
                                                         on dt.OperatorId equals opList.OperatorId into opData
                                                        from op in opData.DefaultIfEmpty()

                                                        join mtList in lstMeasurementTypes
                                                        on dt.MeasurementTypeId equals mtList.Id into mtype
                                                        from mt in mtype.DefaultIfEmpty()

                                                        join tpList in lstTargetPeriods
                                                        on dt.TargetPeriodId equals tpList.TargetPeriodId into tperiod
                                                        from tp in tperiod.DefaultIfEmpty()

                                                        join scList in lstScale
                                                        on dt.ScaleId equals scList.ScaleID into scale
                                                        from sc in scale.DefaultIfEmpty()

                                                        join ur in userroles.Items
                                                        on dt.CreatedBy equals ur.Username into crData
                                                        from cduserrole in crData.DefaultIfEmpty()

                                                        join ur in userroles.Items
                                                        on dt.ModifiedBy equals ur.Username into mdData
                                                        from mduserrole in mdData.DefaultIfEmpty()

                                                        select new KRAModel
                                                        {
                                                            DefinitionDetailsId = dt.DefinitionDetails.DefinitionDetailsId,
                                                            DefinitionTransactionId = dt.DefinitionTransactionId,
                                                            AspectName = asp.AspectName,
                                                            Date = dt.ModifiedDate.HasValue ? "Modified on " + dt.ModifiedDate.Value.ToString("dddd, dd MMMM yyyy") : "Added on " + dt.CreatedDate.Value.ToString("dddd, dd MMMM yyyy"),
                                                            //
                                                            Metric = dt.Metric + (sc == null ? "" : "(Scale: " + sc.MinimumScale + "-" + sc.MaximumScale + "; "
                                                             + string.Join("; ",
                                                             (
                                                             from sd in lstScaleDetails
                                                             where sc.ScaleID == sd.ScaleID
                                                             select sd.ScaleValue + "-" + sd.ScaleDescription).ToList()) + ")"),
                                                            //
                                                            ration = op.OperatorValue + " " + dt.TargetValue + " " + tp.TargetPeriodValue,
                                                            //previousration = defop.OperatorValue + " " + def.TargetValue + " " + deftp.TargetPeriodValue,
                                                            ScaleId = dt.ScaleId,
                                                            Status = status,
                                                            isDeleted=dt.IsDeleted,
                                                            ModifiedDate = dt.ModifiedDate,
                                                            CreatedByUserRole = cduserrole != null ? (cduserrole.Roles.Contains("HRM") ? "HRM" : (cduserrole.Roles.Contains("Department Head")) ? "HOD" : null) : null,
                                                            ModifiedByUserRole = mduserrole != null ? (mduserrole.Roles.Contains("HRM") ? "HRM" : (mduserrole.Roles.Contains("Department Head")) ? "HOD" : null) : null,

                                                        }).ToList();

                        if (lsDefinitionTransactions != null && lsDefinitionTransactions.Count > 0)
                        {

                            //fetching DefinitionTransaction to compare each column with the last IsActive = 0 + ModifiedBy = HR                            

                            var lstInactiveTransactions = await m_kraContext.DefinitionTransactions
                                                .Where(x => x.IsActive == false &&
                                              definitiondetailIds.Contains(x.DefinitionDetailsId)).ToListAsync();

                            var lsttransactions = (from dt in lstInactiveTransactions
                                                   join ur in userroles.Items
                                                   on dt.CreatedBy equals ur.Username into crData
                                                   from cduserrole in crData.DefaultIfEmpty()

                                                   join ur in userroles.Items
                                                   on dt.ModifiedBy equals ur.Username into mdData
                                                   from mduserrole in mdData.DefaultIfEmpty()

                                                   select new KRAModel
                                                   {
                                                       DefinitionTransactionId = dt.DefinitionTransactionId,
                                                       DefinitionDetailsId = dt.DefinitionDetailsId,
                                                       isDeleted = dt.IsDeleted,
                                                       ModifiedDate = dt.ModifiedDate,
                                                       CreatedByUserRole = cduserrole != null ? (cduserrole.Roles.Contains("HRM") ? "HRM" : (cduserrole.Roles.Contains("Department Head")) ? "HOD" : null) : null,
                                                       ModifiedByUserRole = mduserrole != null ? (mduserrole.Roles.Contains("HRM") ? "HRM" : (mduserrole.Roles.Contains("Department Head")) ? "HOD" : null) : null,
                                                   }).ToList();

                            lsttransactions = lsttransactions.Where(c => 
                            c.ModifiedByUserRole == "HRM" || (c.CreatedByUserRole=="HRM" && c.ModifiedByUserRole==null)
                            ).ToList();

                            List<int> Ids = lsttransactions.GroupBy(c => c.DefinitionTransactionId).Select(c => c.Key).ToList<int>();
                            lstInactiveTransactions = lstInactiveTransactions.Where(c => Ids.Contains(c.DefinitionTransactionId)).ToList();
                            
                            var transactions = (from dt in lstInactiveTransactions

                                                join aspList in lstAspects
                                                on dt.DefinitionDetails.Definition.AspectId equals aspList.AspectId into aspectData
                                                from asp in aspectData.DefaultIfEmpty()

                                                join defdetail in lstDefinitionDetails
                                                on dt.DefinitionDetailsId equals defdetail.DefinitionDetailsId into defdetailData
                                                from def in defdetailData.DefaultIfEmpty()

                                                join opList in lstOperators
                                                on def.OperatorId equals opList.OperatorId into defopData
                                                from defop in defopData.DefaultIfEmpty()

                                                join mtList in lstMeasurementTypes
                                                on def.MeasurementTypeId equals mtList.Id into defmtype
                                                from defmt in defmtype.DefaultIfEmpty()

                                                join tpList in lstTargetPeriods
                                                on def.TargetPeriodId equals tpList.TargetPeriodId into deftperiod
                                                from deftp in deftperiod.DefaultIfEmpty()

                                                join scList in lstScale
                                                on def.ScaleId equals scList.ScaleID into defscale
                                                from defsc in defscale.DefaultIfEmpty()

                                                join opList in lstOperators
                                                 on dt.OperatorId equals opList.OperatorId into opData
                                                from op in opData.DefaultIfEmpty()

                                                join mtList in lstMeasurementTypes
                                                on dt.MeasurementTypeId equals mtList.Id into mtype
                                                from mt in mtype.DefaultIfEmpty()

                                                join tpList in lstTargetPeriods
                                                on dt.TargetPeriodId equals tpList.TargetPeriodId into tperiod
                                                from tp in tperiod.DefaultIfEmpty()

                                                join scList in lstScale
                                                on dt.ScaleId equals scList.ScaleID into scale
                                                from sc in scale.DefaultIfEmpty()

                                                select new KRAModel
                                                {
                                                    DefinitionDetailsId = dt.DefinitionDetails.DefinitionDetailsId,
                                                    DefinitionTransactionId = dt.DefinitionTransactionId,
                                                    AspectName = asp.AspectName,
                                                    isDeleted = dt.IsDeleted,
                                                    ModifiedDate=dt.ModifiedDate,
                                                    Metric = dt.Metric + (sc == null ? "" : "(Scale: " + sc.MinimumScale + "-" + sc.MaximumScale + "; "
                                                    + string.Join("; ",
                                                    (
                                                    from sd in lstScaleDetails
                                                    where sc.ScaleID == sd.ScaleID
                                                    select sd.ScaleValue + "-" + sd.ScaleDescription).ToList()) + ")"),                                                   
                                                    ration = op.OperatorValue + " " + dt.TargetValue + " " + tp.TargetPeriodValue,

                                                }).ToList();

                            #region Bubble count calculation
                            int previousCnt = 0;
                            lsDefinitionTransactions = lsDefinitionTransactions.OrderBy(c => c.ModifiedDate).OrderBy(c => c.CreatedDate).ToList();
                            KRAModel modifiedbyhrTransaction = null;

                            foreach (KRAModel model in lsDefinitionTransactions)
                            {
                                modifiedbyhrTransaction = transactions.Where(c => c.DefinitionDetailsId == model.DefinitionDetailsId)
                                    .OrderByDescending(c => c.ModifiedDate).FirstOrDefault<KRAModel>();

                                if (modifiedbyhrTransaction != null)
                                {
                                    if (model.Metric == modifiedbyhrTransaction.Metric)
                                        model.PreviousMetric = null;
                                    else model.PreviousMetric = modifiedbyhrTransaction.Metric;
                                    if (model.ration == modifiedbyhrTransaction.ration) model.previousration = null;
                                    else model.previousration = modifiedbyhrTransaction.ration;

                                    if (model.previousration != null)
                                    {
                                        if (previousCnt > 0)
                                            model.ModifiedTargetCount = previousCnt = previousCnt + 1;
                                        else model.ModifiedTargetCount = previousCnt = 1;
                                        model.DefinitionTransactionId = modifiedbyhrTransaction.DefinitionTransactionId;
                                    }
                                    if (model.PreviousMetric != null)
                                    {
                                        if (previousCnt > 0)
                                            model.ModifiedMetricCount = previousCnt = previousCnt + 1;
                                        else model.ModifiedMetricCount = previousCnt = 1;
                                        model.DefinitionTransactionId = modifiedbyhrTransaction.DefinitionTransactionId;
                                    }                                    
                                }
                                if (model.CreatedByUserRole == "HOD" && (model.ModifiedByUserRole == null ||
                                    model.ModifiedByUserRole == "HOD"))
                                {
                                    model.isAdded = true;
                                    if (previousCnt > 0)
                                        model.CreateCount = previousCnt = previousCnt + 1;
                                    else model.CreateCount = previousCnt = 1;
                                    //model.DefinitionTransactionId = modifiedbyhrTransaction.DefinitionTransactionId;
                                }
                                if (model.CreatedByUserRole == "HRM" && (model.ModifiedByUserRole == null ||
                                    model.ModifiedByUserRole == "HOD") && model.isDeleted.HasValue && model.isDeleted==true)
                                {
                                    if (previousCnt > 0)
                                        model.DeleteCount = previousCnt = 1 + previousCnt;
                                    else model.DeleteCount = previousCnt = 1;
                                }
                            }

                            #endregion

                            if (lstKRAs == null) lstKRAs = new List<KRAModel>();
                            lstKRAs.AddRange(lsDefinitionTransactions);
                        }
                      
                    }
                }
                if (lstKRAs != null && lstKRAs.Count > 0)
                {
                    response.Items = lstKRAs;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "KRAs not found!";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;

                response.Message = "Error occured while fetching KRAs from Definition Details table";
                m_Logger.LogError("Error occured while fetching KRAs from Definition Details table" + ex.StackTrace);
            }
            return response;
            */
        }
        #endregion    

        #region GetKRAs
        /// <summary>
        /// Gets KRAs list.
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<KRAModel>> GetKRAs(int financialYearId, int departmentId, int gradeId, int roleTypeId, bool IsHOD = false)
        {
            //throw new NotImplementedException();

            var response = new ServiceListResponse<KRAModel>();
            int CurrentYear = DateTime.Now.Year;
            GradeRoleType gradeRoleType = null;

            try
            {
                var gradeRoleTypes = await m_OrganizationService.GetAllGradeRoleTypesAsync();

                if (!gradeRoleTypes.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = gradeRoleTypes.Message;
                    return response;
                }
                else
                {
                    gradeRoleType = (from gr in gradeRoleTypes.Items
                                     where gr.GradeId == gradeId && gr.DepartmentId == departmentId && gr.GradeRoleTypeId == roleTypeId
                                     select gr).FirstOrDefault();

                    if (gradeRoleType == null)
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "GradeRoleType not found!";
                        return response;
                    }
                }
                //var lstApplicableRoleType = m_kraContext.ApplicableRoleTypes.Where(x => x.FinancialYearId == financialYearId
                //                                && x.DepartmentId == departmentId && x.GradeRoleTypeId == gradeRoleType.GradeRoleTypeId).ToList();
                //if (lstApplicableRoleType == null || lstApplicableRoleType.Count == 0)
                //{
                //    response.Items = null;
                //    response.Message = "No applicable roletypes found!";
                //    return response;
                //}

                //int status = (from art in lstApplicableRoleType
                //              select art.StatusId).SingleOrDefault();

                

                var lstfinancialYears = await m_OrganizationService.GetAllFinancialYearsAsync();
                string financialYearName = (from fy in lstfinancialYears.Items
                                            where fy.Id == financialYearId
                                            select fy.FinancialYearName).SingleOrDefault();

                string[] splitString = financialYearName == null ? null : financialYearName.Split('-');
                int fromYear = splitString == null ? 0 : Convert.ToInt32(splitString[0].Trim());
                int toYear = splitString == null ? 0 : Convert.ToInt32(splitString[1].Trim());

            /*    int status = m_kraContext.Definitions.Where(x => x.FinancialYearId == financialYearId &&
                                                  x.GradeRoleTypeId == roleTypeId
                                                  && x.IsActive == true)
                                            .Select(c => c.StatusId).FirstOrDefault(); ;
                 
                if ((fromYear > 0) && (fromYear < CurrentYear && toYear == CurrentYear) && status == StatusConstants.ApprovedByCEO)
                {
                    var definitionDetailsKRAs = await GetKRAFromDefinitionDetails(financialYearId, gradeRoleType.GradeRoleTypeId, status);
                    if (!definitionDetailsKRAs.IsSuccessful)
                    {
                        response.Items = null;
                        response.Message = "No KRAs found!";
                        return response;
                    }
                    else
                    {
                        response.Items = definitionDetailsKRAs.Items;
                        response.IsSuccessful = true;
                    }
                }
                else if ((fromYear == CurrentYear && CurrentYear < toYear) || fromYear > CurrentYear)
                {
                    if ((IsHOD == false && (status == StatusConstants.Draft ||
                        status == StatusConstants.FinishedDrafting))

                        || (IsHOD == true && status == StatusConstants.SentToHOD ||
                            status == StatusConstants.ApprovedbyHOD ||
                            status == StatusConstants.EditedByHOD ||
                            status == StatusConstants.FinishedEditByHOD
                            || status == StatusConstants.FinishedEditByHR
                            ))
                    {
                        var definitionTransactionKRAs = await GetKRADefinitionTransactions(financialYearId, departmentId, gradeRoleType.GradeRoleTypeId, status);

                        if (!definitionTransactionKRAs.IsSuccessful)
                        {
                            response.Items = null;
                            if (IsHOD == false) response.Message = "No KRAs defined!";
                            else response.Message = "No KRAs sent for review!";
                            return response;
                        }
                        else
                        {
                            response.Items = definitionTransactionKRAs.Items;
                            response.IsSuccessful = true;
                        }
                    }
                    else if ((IsHOD == true && (status == StatusConstants.Draft ||
                        status == StatusConstants.FinishedDrafting)))
                    {
                        response.Items = null;
                        response.Message = "No KRAs sent for review!";
                        return response;
                    }
                    else
                    {
                        if ((IsHOD == false && status == StatusConstants.ApprovedbyHOD ||
                            status == StatusConstants.ApprovedByCEO
                            || status == StatusConstants.SentToHOD ||
                            status == StatusConstants.EditedByHOD ||
                            status == StatusConstants.FinishedEditByHOD)
                            || (IsHOD == true && (status == StatusConstants.SentToHR
                            || status == StatusConstants.EditByHR
                            || status == StatusConstants.FinishedEditByHR
                            || status == StatusConstants.ApprovedByCEO)))
                        {
                            var definitionDetailsKRAs = await GetKRAFromDefinitionDetails(financialYearId, gradeRoleType.GradeRoleTypeId, status);
                            if (!definitionDetailsKRAs.IsSuccessful)
                            {
                                response.Items = null;
                                response.Message = "No KRAs found!";
                                return response;
                            }
                            else
                            {
                                response.Items = definitionDetailsKRAs.Items;
                                response.IsSuccessful = true;
                            }
                        }
                        else if (IsHOD == false && (status == StatusConstants.EditByHR || status == StatusConstants.SentToHR
                            || status == StatusConstants.SendToCEO))
                        {
                            //Check the Definition Table, if IsHODApproved = 1, then fetch that record else 
                            //fetch the IsActive record from the DefinitionTransaction table. 
                            var definitionTransactionKRAs = await GetKRAFromDefinition(financialYearId, departmentId, gradeRoleType.GradeRoleTypeId, status);
                            if (!definitionTransactionKRAs.IsSuccessful)
                            {
                                response.Items = null;
                                if (IsHOD == false)
                                {
                                    if (status == StatusConstants.EditByHR) response.Message = "No records found!";
                                    else response.Message = "No KRAs found!";
                                }
                                else if (IsHOD == true) response.Message = "No KRAs sent for review!";
                                return response;
                            }
                            else
                            {
                                response.Items = definitionTransactionKRAs.Items;
                                response.IsSuccessful = true;
                            }
                        }
                    }
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No KRAs found!";
                    return response;
                }*/
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;

                response.Message = "Error occured while fetching KRAs.";
                m_Logger.LogError("Error occured while fetching KRAs." + ex.StackTrace);
            }
            return response;

        }
        #endregion

        #region GetDefinitionDetails
        /// <summary>
        /// Get DefinationDetais By DefinationDetaisId
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<ServiceListResponse<DefinitionModel>> GetDefinitionDetails(Guid Id)
        {
            var response = new ServiceListResponse<DefinitionModel>();           
            try
            {
                m_Logger.LogInformation("GetDefinitionService: Calling \"GetDefinition\" method.");

               
                    var definitions = await (from d in m_kraContext.Definitions 
                                                   where d.DefinitionId == Id
                                                   select new DefinitionModel
                                                   {
                                                       AspectId = d.AspectId,
                                                         Metric = d.Metric,
                                                       OperatorId = d.OperatorId,
                                                       MeasurementTypeId = d.MeasurementTypeId,
                                                       ScaleId = d.ScaleId,
                                                       TargetValue = d.TargetValue,
                                                       TargetPeriodId = d.TargetPeriodId
                                                   }).ToListAsync();

                    if (definitions != null && definitions.Count > 0)
                    {
                        response.Items = definitions;
                        response.IsSuccessful = true;
                        response.Message = string.Empty;
                    }
                    else
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Definitions not found";
                    }
                
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching GetDefinitions";
                m_Logger.LogError("Error occured in GetDefinitions() method." + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region Delete
        /// <summary>
        /// Delete a KRA Definition record
        /// </summary>
        /// <param name="definitionDetailId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> Delete(Guid definitionId)
        {
            bool isDeleted = false;
            ServiceResponse<bool> response;

            m_Logger.LogInformation("DefinitionService: Calling \"Delete\" method");

            var definition = m_kraContext.Definitions.Find(definitionId);

            if (definition != null)
            {
                m_kraContext.Definitions.Remove(definition);

                isDeleted = await m_kraContext.SaveChangesAsync() > 0 ? true : false;

                if (isDeleted)
                {
                    response = new ServiceResponse<bool>();
                    response.Item = true;
                    response.IsSuccessful = true;
                    response.Message = "KRA Definition records deleted.";
                    return response;
                }
                else
                {
                    response = new ServiceResponse<bool>();
                    response.Item = false;
                    response.IsSuccessful = false;
                    response.Message = "KRA Definition could not be deleted.";
                    return response;
                }
            }
            else
            {
                response = new ServiceResponse<bool>();
                response.Item = false;
                response.IsSuccessful = false;
                response.Message = "KRA Definition could not be deleted, as it is not in Draft status.";
                return response;
            }

            //  throw new NotImplementedException();
            /*
            bool isDeleted = false;
            ServiceResponse<bool> response;

            m_Logger.LogInformation("DefinitionService: Calling \"Delete\" method");

            var definitionDetail = m_kraContext.DefinitionDetails.Find(definitionDetailId);

            if (definitionDetail == null)
            {
                response = new ServiceResponse<bool>();
                response.Item = false;
                response.IsSuccessful = false;
                response.Message = "Definition Detail not found for delete.";
                return response;
            }

            var definitionTransactions = m_kraContext.DefinitionTransactions.Where(d => d.DefinitionDetailsId == definitionDetailId).ToList();

            var definition = m_kraContext.Definitions.Where(d => d.DefinitionId == definitionDetail.DefinitionId).FirstOrDefault<Definition>();

            var artInDraftStatus = m_kraContext.ApplicableRoleTypes.Where(a => a.ApplicableRoleTypeId == definition.ApplicableRoleTypeId 
                                                                                && a.StatusId ==StatusConstants.Draft).FirstOrDefault<ApplicableRoleType>();

            if (artInDraftStatus != null)
            {
                int definitionDetailCount = 0;

                if (definition != null)
                {
                    definitionDetailCount = m_kraContext.DefinitionDetails.Where(d => d.DefinitionId == definitionDetail.DefinitionId
                                          && d.DefinitionDetailsId != definitionDetailId).Count();
                }

                if (definitionTransactions != null)
                    m_kraContext.DefinitionTransactions.RemoveRange(definitionTransactions);

                m_kraContext.DefinitionDetails.Remove(definitionDetail);

                if (definition != null)
                {
                    if (definitionDetailCount == 0)
                        m_kraContext.Definitions.Remove(definition);
                }

                isDeleted = await m_kraContext.SaveChangesAsync() > 0 ? true : false;

                if (isDeleted)
                {
                    response = new ServiceResponse<bool>();
                    response.Item = true;
                    response.IsSuccessful = true;
                    response.Message = "KRA Definition records deleted.";
                    return response;
                }
                else
                {
                    response = new ServiceResponse<bool>();
                    response.Item = false;
                    response.IsSuccessful = false;
                    response.Message = "KRA Definition could not be deleted.";
                    return response;
                }
            }
            else
            {
                response = new ServiceResponse<bool>();
                response.Item = false;
                response.IsSuccessful = false;
                response.Message = "KRA Definition could not be deleted, as it is not in Draft status.";
                return response;
            }
           */
        }
        
            #endregion

        #region DeleteByHOD
        /// <summary>
        /// Delete (soft delete) a KRA Definition by HOD
        /// </summary>
        /// <param name="defintionDetailId"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> DeleteByHOD(int defintionDetailId)
        {
            throw new NotImplementedException();
            /*
            bool isCreated = false;
            bool isUpdated = false;
            var response = new BaseServiceResponse();

            m_Logger.LogInformation("DefinitionService: Calling \"DeleteByHOD\" method");

            var defintionDetail = m_kraContext.DefinitionDetails.Find(defintionDetailId);

            if (defintionDetail == null)
            {
                response.IsSuccessful = false;
                response.Message = "Definition Detail not found for delete.";
                return response;
            }

            var existingDefTransaction = m_kraContext.DefinitionTransactions.
                                                    Where(d => d.DefinitionDetailsId == defintionDetailId && d.IsActive == true).
                                                    FirstOrDefault<DefinitionTransaction>();

            using (var transaction = m_kraContext.Database.BeginTransaction())
            {
                try
                {
                    DefinitionTransaction newDefTransaction = new DefinitionTransaction()
                    {
                        DefinitionDetailsId = existingDefTransaction.DefinitionDetailsId,
                        Metric = existingDefTransaction.Metric,
                        OperatorId = existingDefTransaction.OperatorId,
                        MeasurementTypeId = existingDefTransaction.MeasurementTypeId,
                        ScaleId = existingDefTransaction.ScaleId,
                        TargetValue = existingDefTransaction.TargetValue,
                        TargetPeriodId = existingDefTransaction.TargetPeriodId,
                        CreatedBy=existingDefTransaction.CreatedBy,
                        CreatedDate=existingDefTransaction.CreatedDate,
                        IsActive = true,
                        IsDeleted = true,
                    };

                    m_kraContext.DefinitionTransactions.Add(newDefTransaction);
                    isCreated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;

                    if (isCreated)
                    {
                        existingDefTransaction.IsActive = false;
                        isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                        if (isUpdated)
                        {
                            var definition = m_kraContext.Definitions.FirstOrDefault(x => x.DefinitionId == defintionDetail.DefinitionId);
                            var appplicableroletype = m_kraContext.ApplicableRoleTypes.FirstOrDefault(x => x.ApplicableRoleTypeId == definition.ApplicableRoleTypeId);
                            appplicableroletype.StatusId = StatusConstants.EditedByHOD;
                            isCreated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                            if (isCreated)
                            {
                                transaction.Commit();
                                response.IsSuccessful = true;
                                response.Message = "Records updated successfully.";
                            }                           
                        }
                    }

                    if (!isCreated)
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while updating a kra record.";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating a kra record.";
                    m_Logger.LogError("Error occured in DeleteByHOD() method in DefinitionService " + ex.StackTrace);
                }
            }
            return response;
            */
        }
        #endregion

        #region SetPreviousMetricValues
        /// <summary>
        /// SetPreviousMetricValues
        /// </summary>
        /// <param name="defintionDetailId"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> SetPreviousMetricValues(int defintionDetailId)
        {
            throw new NotImplementedException();
            /*
            bool isUpdated = false;
            var response = new BaseServiceResponse();

            m_Logger.LogInformation("DefinitionService: Calling \"SetPreviousMetricValues\" method");

            var defintionDetail = m_kraContext.DefinitionDetails.Find(defintionDetailId);

            if (defintionDetail == null)
            {
                response.IsSuccessful = false;
                response.Message = "Definition Detail not found for delete.";
                return response;
            }

            var existingDefTransaction = m_kraContext.DefinitionTransactions.
                                                      Where(d => d.DefinitionDetailsId == defintionDetailId && d.IsActive == true).
                                                      FirstOrDefault<DefinitionTransaction>();

            using (var transaction = m_kraContext.Database.BeginTransaction())
            {
                try
                {
                    existingDefTransaction.Metric = defintionDetail.Metric;
                    existingDefTransaction.MeasurementTypeId = defintionDetail.MeasurementTypeId;
                    existingDefTransaction.ScaleId = defintionDetail.ScaleId;
                    isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;

                    if (isUpdated)
                    {
                        transaction.Commit();
                        response.IsSuccessful = true;
                        response.Message = "Records updated successfully.";
                    }
                    else
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while updating a kra record.";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating a kra record.";
                    m_Logger.LogError("Error occured in SetPreviousMetricValues() method in DefinitionService " + ex.StackTrace);
                }
            }
            return response;*/
        }
        #endregion

        #region SetPreviousTargetValues
        /// <summary>
        /// SetPreviousTargetValues
        /// </summary>
        /// <param name="defintionDetailId"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> SetPreviousTargetValues(int defintionDetailId)
        {
            throw new NotImplementedException();
            /*
            bool isUpdated = false;
            var response = new BaseServiceResponse();

            m_Logger.LogInformation("DefinitionService: Calling \"SetPreviousTargetValues\" method");

            var defintionDetail = m_kraContext.DefinitionDetails.Find(defintionDetailId);

            if (defintionDetail == null)
            {
                response.IsSuccessful = false;
                response.Message = "Definition Detail not found for delete.";
                return response;
            }

            var existingDefTransaction = m_kraContext.DefinitionTransactions.
                                                      Where(d => d.DefinitionDetailsId == defintionDetailId && d.IsActive == true).
                                                      FirstOrDefault<DefinitionTransaction>();

            using (var transaction = m_kraContext.Database.BeginTransaction())
            {
                try
                {
                    existingDefTransaction.OperatorId = defintionDetail.OperatorId;
                    existingDefTransaction.TargetValue = defintionDetail.TargetValue;
                    existingDefTransaction.TargetPeriodId = defintionDetail.TargetPeriodId;
                    isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                    if (isUpdated)
                    {
                        transaction.Commit();
                        response.IsSuccessful = true;
                        response.Message = "Records updated successfully.";
                    }
                    else
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while updating a kra record.";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating a kra record.";
                    m_Logger.LogError("Error occured in SetPreviousTargetValues() method in DefinitionService " + ex.StackTrace);
                }
            }
            return response;*/
        }
        #endregion

        #region AcceptTargetValue
        /// <summary>
        /// AcceptTargetValue
        /// </summary>
        /// <param name="defintionTransactionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> AcceptTargetValue(int defintionTransactionId, string username)
        {
            throw new NotImplementedException();
            /*
            bool isUpdated = false;
            var response = new BaseServiceResponse();

            m_Logger.LogInformation("DefinitionService: Calling \"AcceptTargetValue\" method");
                     
            var existingDefTransaction = m_kraContext.DefinitionTransactions.
                                                      Where(d => d.DefinitionTransactionId == defintionTransactionId).
                                                      FirstOrDefault<DefinitionTransaction>();

            var defintionDetail = m_kraContext.DefinitionDetails.Find(existingDefTransaction.DefinitionDetailsId);

            if (defintionDetail == null)
            {
                response.IsSuccessful = false;
                response.Message = "Definition Detail not found";
                return response;
            }

            using (var transaction = m_kraContext.Database.BeginTransaction())
            {
                try
                {
                    existingDefTransaction.OperatorId = defintionDetail.OperatorId;
                    existingDefTransaction.TargetValue = defintionDetail.TargetValue;
                    existingDefTransaction.TargetPeriodId = defintionDetail.TargetPeriodId;
                    existingDefTransaction.ModifiedBy = username;
                    existingDefTransaction.ModifiedDate= DateTime.UtcNow;
                    isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                    if (isUpdated)
                    {
                        transaction.Commit();
                        response.IsSuccessful = true;
                        response.Message = "Records updated successfully.";
                    }
                    else
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while updating a kra record.";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating a kra record.";
                    m_Logger.LogError("Error occured in AcceptTargetValue() method in DefinitionService " + ex.StackTrace);
                }
            }
            return response;*/
        }
        #endregion

        #region AcceptMetricValue
        /// <summary>
        /// AcceptMetricValue
        /// </summary>
        /// <param name="defintionTransactionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> AcceptMetricValue(int defintionTransactionId, string username)
        {
            throw new NotImplementedException();
            /*
            bool isUpdated = false;
            var response = new BaseServiceResponse();

            m_Logger.LogInformation("DefinitionService: Calling \"AcceptMetricValue\" method");
                      

            var existingDefTransaction = m_kraContext.DefinitionTransactions.
                                                      Where(d => d.DefinitionTransactionId == defintionTransactionId).
                                                      FirstOrDefault<DefinitionTransaction>();

            var defintionDetail = m_kraContext.DefinitionDetails.Find(existingDefTransaction.DefinitionDetailsId);

            if (defintionDetail == null)
            {
                response.IsSuccessful = false;
                response.Message = "Definition Detail not found.";
                return response;
            }

            using (var transaction = m_kraContext.Database.BeginTransaction())
            {
                try
                {
                    existingDefTransaction.Metric = defintionDetail.Metric;
                    existingDefTransaction.MeasurementTypeId = defintionDetail.MeasurementTypeId;
                    existingDefTransaction.ScaleId = defintionDetail.ScaleId;
                    existingDefTransaction.ModifiedBy = username;
                    existingDefTransaction.ModifiedDate = DateTime.UtcNow;
                    isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                    if (isUpdated)
                    {
                        transaction.Commit();
                        response.IsSuccessful = true;
                        response.Message = "Records updated successfully.";
                    }
                    else
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while updating a kra record.";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating a kra record.";
                    m_Logger.LogError("Error occured in AcceptMetricValue() method in DefinitionService " + ex.StackTrace);
                }
            }
            return response;*/
        }
        #endregion

        #region RejectTargetValue
        /// <summary>
        /// RejectTargetValue
        /// </summary>
        /// <param name="defintionTransactionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> RejectTargetValue(int defintionTransactionId, string username)
        {
            throw new NotImplementedException();
            /*
            bool isUpdated = false;
            bool isCreated = false;
            var response = new BaseServiceResponse();

            m_Logger.LogInformation("DefinitionService: Calling \"RejectTargetValue\" method");

            var existingDefTransaction = m_kraContext.DefinitionTransactions.
                                                      Where(d => d.DefinitionTransactionId == defintionTransactionId).
                                                      FirstOrDefault<DefinitionTransaction>();

            var defintionDetail = m_kraContext.DefinitionDetails.Find(existingDefTransaction.DefinitionDetailsId);

            var activeDefTransaction = m_kraContext.DefinitionTransactions.
                                                      Where(d => d.DefinitionDetailsId == defintionDetail.DefinitionDetailsId
                                                      && d.IsActive==true).
                                                      FirstOrDefault<DefinitionTransaction>();

            if (defintionDetail == null)
            {
                response.IsSuccessful = false;
                response.Message = "Definition Detail not found";
                return response;
            }

            using (var transaction = m_kraContext.Database.BeginTransaction())
            {
                try
                {
                    defintionDetail.OperatorId= existingDefTransaction.OperatorId;
                    defintionDetail.TargetValue = existingDefTransaction.TargetValue;
                    defintionDetail.TargetPeriodId=existingDefTransaction.TargetPeriodId;
                    defintionDetail.ModifiedBy = username;                  

                    isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                    if (isUpdated)
                    {
                        DefinitionTransaction definitionTransaction = new DefinitionTransaction()
                        {
                            DefinitionDetailsId = existingDefTransaction.DefinitionDetailsId,
                            Metric = existingDefTransaction.Metric,
                            OperatorId = existingDefTransaction.OperatorId,
                            MeasurementTypeId = existingDefTransaction.MeasurementTypeId,
                            ScaleId = existingDefTransaction.ScaleId,
                            TargetValue = existingDefTransaction.TargetValue,
                            TargetPeriodId = existingDefTransaction.TargetPeriodId,
                            IsActive = true,
                            IsDeleted = null,
                            CreatedBy = username,
                        };
                        m_kraContext.DefinitionTransactions.Add(definitionTransaction);
                        isCreated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                        if (isCreated)
                        {
                            activeDefTransaction.IsActive = false;
                            isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                            if (isUpdated)
                            {
                                transaction.Commit();
                                response.IsSuccessful = true;
                                response.Message = "Records updated successfully.";
                            }
                            else
                            {
                                transaction.Rollback();
                                response.IsSuccessful = false;
                                response.Message = "Error occurred while updating a kra record.";
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            response.IsSuccessful = false;
                            response.Message = "Error occurred while updating a kra record.";
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while updating a kra record.";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating a kra record.";
                    m_Logger.LogError("Error occured in RejectTargetValue() method in DefinitionService " + ex.StackTrace);
                }
            }
            return response;*/
        }
        #endregion

        #region RejectMetricValue
        /// <summary>
        /// RejectMetricValue
        /// </summary>
        /// <param name="defintionTransactionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> RejectMetricValue(int defintionTransactionId, string username)
        {
            throw new NotImplementedException();
            /*
            bool isUpdated = false;
            bool isCreated = false;
            var response = new BaseServiceResponse();

            m_Logger.LogInformation("DefinitionService: Calling \"RejectMetricValue\" method");


            var existingDefTransaction = m_kraContext.DefinitionTransactions.
                                                      Where(d => d.DefinitionTransactionId == defintionTransactionId).
                                                      FirstOrDefault<DefinitionTransaction>();

            var defintionDetail = m_kraContext.DefinitionDetails.Find(existingDefTransaction.DefinitionDetailsId);

            if (defintionDetail == null)
            {
                response.IsSuccessful = false;
                response.Message = "Definition Detail not found.";
                return response;
            }

            var activeDefTransaction = m_kraContext.DefinitionTransactions.
                                                     Where(d => d.DefinitionDetailsId == defintionDetail.DefinitionDetailsId
                                                     && d.IsActive == true).
                                                     FirstOrDefault<DefinitionTransaction>();

            using (var transaction = m_kraContext.Database.BeginTransaction())
            {
                try
                {
                    defintionDetail.Metric = existingDefTransaction.Metric;
                    defintionDetail.MeasurementTypeId = existingDefTransaction.MeasurementTypeId;
                    defintionDetail.ScaleId = existingDefTransaction.ScaleId;
                    defintionDetail.ModifiedBy = username;
                    isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                    if (isUpdated)
                    {
                        DefinitionTransaction definitionTransaction = new DefinitionTransaction()
                        {
                            DefinitionDetailsId = existingDefTransaction.DefinitionDetailsId,
                            Metric = existingDefTransaction.Metric,
                            OperatorId = existingDefTransaction.OperatorId,
                            MeasurementTypeId = existingDefTransaction.MeasurementTypeId,
                            ScaleId = existingDefTransaction.ScaleId,
                            TargetValue = existingDefTransaction.TargetValue,
                            TargetPeriodId = existingDefTransaction.TargetPeriodId,
                            IsActive = true,
                            IsDeleted = null,
                            CreatedBy = username,
                        };
                        m_kraContext.DefinitionTransactions.Add(definitionTransaction);
                        isCreated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                        if (isCreated)
                        {
                            activeDefTransaction.IsActive = false;
                            isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                            if (isUpdated)
                            {
                                transaction.Commit();
                                response.IsSuccessful = true;
                                response.Message = "Records updated successfully.";
                            }
                            else
                            {
                                transaction.Rollback();
                                response.IsSuccessful = false;
                                response.Message = "Error occurred while updating a kra record.";
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            response.IsSuccessful = false;
                            response.Message = "Error occurred while updating a kra record.";
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while updating a kra record.";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating a kra record.";
                    m_Logger.LogError("Error occured in RejectMetricValue() method in DefinitionService " + ex.StackTrace);
                }
            }
            return response;*/
        }
        #endregion

        #region AddKRAAgain
        /// <summary>
        /// AddKRAAgain
        /// </summary>
        /// <param name="defintionDetailId"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> AddKRAAgain(int defintionDetailId)
        {
            throw new NotImplementedException();
            /*
            bool isUpdated = false;
            var response = new BaseServiceResponse();

            m_Logger.LogInformation("DefinitionService: Calling \"AddKRAAgain\" method");

            var defintionDetail = m_kraContext.DefinitionDetails.Find(defintionDetailId);

            if (defintionDetail == null)
            {
                response.IsSuccessful = false;
                response.Message = "Definition Detail not found.";
                return response;
            }

            var existingDefTransaction = m_kraContext.DefinitionTransactions.
                                                      Where(d => d.DefinitionDetailsId == defintionDetailId && d.IsActive == true).
                                                      FirstOrDefault<DefinitionTransaction>();

            using (var transaction = m_kraContext.Database.BeginTransaction())
            {
                try
                {
                    existingDefTransaction.IsDeleted = null;                   
                    isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                    if (isUpdated)
                    {
                        transaction.Commit();
                        response.IsSuccessful = true;
                        response.Message = "Records updated successfully.";
                    }

                    if (!isUpdated)
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while updating a kra record.";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating a kra record.";
                    m_Logger.LogError("Error occured in AddKRAAgain() method in DefinitionService " + ex.StackTrace);
                }
            }
            return response;*/
        }
        #endregion

        #region DeleteByHOD-Hard-Delete
        /// <summary>
        /// Delete a KRA Definition record
        /// </summary>
        /// <param name="definitionDetailId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> DeleteKRA(int definitionDetailId)
        {
            throw new NotImplementedException();
            /*
            bool isDeleted = false;
            ServiceResponse<bool> response;

            m_Logger.LogInformation("DefinitionService: Calling \"Delete\" method");

            var definitionDetail = m_kraContext.DefinitionDetails.Find(definitionDetailId);

            if (definitionDetail == null)
            {
                response = new ServiceResponse<bool>();
                response.Item = false;
                response.IsSuccessful = false;
                response.Message = "Definition Detail not found for delete.";
                return response;
            }

            var definitionTransactions = m_kraContext.DefinitionTransactions.Where(d => d.DefinitionDetailsId == definitionDetailId).ToList();

            var definition = m_kraContext.Definitions.Where(d => d.DefinitionId == definitionDetail.DefinitionId).FirstOrDefault<Definition>();

            var artInDraftStatus = m_kraContext.ApplicableRoleTypes.Where(a => a.ApplicableRoleTypeId == definition.ApplicableRoleTypeId
                                                                                && a.StatusId == StatusConstants.EditedByHOD).FirstOrDefault<ApplicableRoleType>();

            if (artInDraftStatus != null)
            {
                if (definitionTransactions != null)
                    m_kraContext.DefinitionTransactions.RemoveRange(definitionTransactions);

                m_kraContext.DefinitionDetails.Remove(definitionDetail);

                if (definition != null)
                    m_kraContext.Definitions.Remove(definition);

                isDeleted = await m_kraContext.SaveChangesAsync() > 0 ? true : false;

                if (isDeleted)
                {
                    response = new ServiceResponse<bool>();
                    response.Item = true;
                    response.IsSuccessful = true;
                    response.Message = "KRA Definition records deleted.";
                    return response;
                }
                else
                {
                    response = new ServiceResponse<bool>();
                    response.Item = false;
                    response.IsSuccessful = false;
                    response.Message = "KRA Definition could not be deleted.";
                    return response;
                }
            }
            else
            {
                response = new ServiceResponse<bool>();
                response.Item = false;
                response.IsSuccessful = false;
                response.Message = "KRA Definition could not be deleted, as it is not in Draft status.";
                return response;
            }
            */
        }

        #endregion

        #region AcceptDeletedKRAByHOD
        /// <summary>
        /// AcceptDeletedKRAByHOD
        /// </summary>
        /// <param name="defintionTransactionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> AcceptDeletedKRAByHOD(int defintionTransactionId, string username)
        {
            throw new NotImplementedException();
            /*
            bool isUpdated = false;            
            var response = new BaseServiceResponse();

            m_Logger.LogInformation("DefinitionService: Calling \"AcceptDeletedKRAByHOD\" method");


            var existingDefTransaction = m_kraContext.DefinitionTransactions.
                                                      Where(d => d.DefinitionTransactionId == defintionTransactionId
                                                      && d.IsActive==true).
                                                      FirstOrDefault<DefinitionTransaction>();

            var defintionDetail = m_kraContext.DefinitionDetails.Find(existingDefTransaction.DefinitionDetailsId);

            if (defintionDetail == null)
            {
                response.IsSuccessful = false;
                response.Message = "Definition Detail not found.";
                return response;
            }
            var defintion= m_kraContext.Definitions.Find(defintionDetail.DefinitionId);
            if (defintion == null)
            {
                response.IsSuccessful = false;
                response.Message = "Definition not found.";
                return response;
            }
            using (var transaction = m_kraContext.Database.BeginTransaction())
            {
                try
                {

                    defintion.ModifiedBy = username;
                    defintion.ModifiedDate = DateTime.UtcNow;
                    defintion.IsActive = false;
                    isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                    if (isUpdated)
                    {
                        DefinitionTransaction definitionTransaction = new DefinitionTransaction()
                        {
                            DefinitionDetailsId = existingDefTransaction.DefinitionDetailsId,
                            Metric = existingDefTransaction.Metric,
                            OperatorId = existingDefTransaction.OperatorId,
                            MeasurementTypeId = existingDefTransaction.MeasurementTypeId,
                            ScaleId = existingDefTransaction.ScaleId,
                            TargetValue = existingDefTransaction.TargetValue,
                            TargetPeriodId = existingDefTransaction.TargetPeriodId,
                            IsActive = false,
                            IsDeleted = true,
                            CreatedBy = existingDefTransaction.CreatedBy,
                            ModifiedBy = username,
                            ModifiedDate = DateTime.UtcNow,
                        };
                        m_kraContext.DefinitionTransactions.Add(definitionTransaction);
                        isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                        if(isUpdated)
                        {
                            existingDefTransaction.IsActive = false;
                            isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                        }

                    }
                    if (isUpdated)
                    {
                        transaction.Commit();
                        response.IsSuccessful = true;
                        response.Message = "Records updated successfully.";
                    }
                    else
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while updating a kra record.";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating a kra record.";
                    m_Logger.LogError("Error occured in AcceptDeletedKRAByHOD() method in DefinitionService " + ex.StackTrace);
                }
            }
            return response;*/
        }
        #endregion

        #region RejectDeletedKRAByHOD
        /// <summary>
        /// RejectDeletedKRAByHOD
        /// </summary>
        /// <param name="defintionTransactionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> RejectDeletedKRAByHOD(int defintionTransactionId, string username)
        {
            throw new NotImplementedException();
            /*
            bool isUpdated = false;
            var response = new BaseServiceResponse();

            m_Logger.LogInformation("DefinitionService: Calling \"RejectDeletedKRAByHOD\" method");


            var existingDefTransaction = m_kraContext.DefinitionTransactions.
                                                      Where(d => d.DefinitionTransactionId == defintionTransactionId
                                                      && d.IsActive == true).
                                                      FirstOrDefault<DefinitionTransaction>();

            var defintionDetail = m_kraContext.DefinitionDetails.Find(existingDefTransaction.DefinitionDetailsId);

            if (defintionDetail == null)
            {
                response.IsSuccessful = false;
                response.Message = "Definition Detail not found.";
                return response;
            }

            using (var transaction = m_kraContext.Database.BeginTransaction())
            {
                try
                {
                    defintionDetail.ModifiedBy = username;
                    defintionDetail.ModifiedDate = DateTime.UtcNow;
                    defintionDetail.IsDeleted = false;
                    isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                    if (isUpdated)
                    {
                        DefinitionTransaction definitionTransaction = new DefinitionTransaction()
                        {
                            DefinitionDetailsId = existingDefTransaction.DefinitionDetailsId,
                            Metric = existingDefTransaction.Metric,
                            OperatorId = existingDefTransaction.OperatorId,
                            MeasurementTypeId = existingDefTransaction.MeasurementTypeId,
                            ScaleId = existingDefTransaction.ScaleId,
                            TargetValue = existingDefTransaction.TargetValue,
                            TargetPeriodId = existingDefTransaction.TargetPeriodId,
                            IsActive = true,
                            IsDeleted = null,
                            CreatedBy = existingDefTransaction.CreatedBy,
                            ModifiedBy = username,
                            ModifiedDate = DateTime.UtcNow,
                        };
                        m_kraContext.DefinitionTransactions.Add(definitionTransaction);
                        isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                        if (isUpdated)
                        {
                            existingDefTransaction.IsActive = false;
                            isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                        }

                    }
                    if (isUpdated)
                    {
                        transaction.Commit();
                        response.IsSuccessful = true;
                        response.Message = "Records updated successfully.";
                    }
                    else
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while updating a kra record.";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating a kra record.";
                    m_Logger.LogError("Error occured in RejectDeletedKRAByHOD() method in DefinitionService " + ex.StackTrace);
                }
            }
            return response;*/
        }
        #endregion

        #region AcceptAddedKRAByHOD
        /// <summary>
        /// AcceptAddedKRAByHOD
        /// </summary>
        /// <param name="defintionTransactionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> AcceptAddedKRAByHOD(int defintionTransactionId, string username)
        {
            throw new NotImplementedException();
            /*
            bool isUpdated = false;
            var response = new BaseServiceResponse();

            m_Logger.LogInformation("DefinitionService: Calling \"AcceptAddedKRAByHOD\" method");

            var existingDefTransaction = m_kraContext.DefinitionTransactions.
                                                      Where(d => d.DefinitionTransactionId == defintionTransactionId
                                                      && d.IsActive == true).
                                                      FirstOrDefault<DefinitionTransaction>();

            var defintionDetail = m_kraContext.DefinitionDetails.Find(existingDefTransaction.DefinitionDetailsId);

            if (defintionDetail == null)
            {
                response.IsSuccessful = false;
                response.Message = "Definition Detail not found.";
                return response;
            }

            using (var transaction = m_kraContext.Database.BeginTransaction())
            {
                try
                {
                    DefinitionTransaction definitionTransaction = new DefinitionTransaction()
                    {
                        DefinitionDetailsId = existingDefTransaction.DefinitionDetailsId,
                        Metric = existingDefTransaction.Metric,
                        OperatorId = existingDefTransaction.OperatorId,
                        MeasurementTypeId = existingDefTransaction.MeasurementTypeId,
                        ScaleId = existingDefTransaction.ScaleId,
                        TargetValue = existingDefTransaction.TargetValue,
                        TargetPeriodId = existingDefTransaction.TargetPeriodId,
                        IsActive = true,
                        IsDeleted = null,
                        CreatedBy = existingDefTransaction.CreatedBy,
                        ModifiedBy = username,
                        ModifiedDate = DateTime.UtcNow,
                    };
                    m_kraContext.DefinitionTransactions.Add(definitionTransaction);
                    isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                    if (isUpdated)
                    {
                        existingDefTransaction.IsActive = false;
                        existingDefTransaction.ModifiedBy = username;
                        isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                    }
                    if (isUpdated)
                    {
                        transaction.Commit();
                        response.IsSuccessful = true;
                        response.Message = "Records updated successfully.";
                    }
                    else
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while updating a kra record.";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating a kra record.";
                    m_Logger.LogError("Error occured in AcceptAddedKRAByHOD() method in DefinitionService " + ex.StackTrace);
                }
            }
            return response;*/
        }
        #endregion

        #region RejectAddedKRAByHOD
        /// <summary>
        /// RejectAddedKRAByHOD
        /// </summary>
        /// <param name="defintionTransactionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> RejectAddedKRAByHOD(int defintionTransactionId, string username)
        {
            throw new NotImplementedException();
            /*
            bool isUpdated = false;
            var response = new BaseServiceResponse();

            m_Logger.LogInformation("DefinitionService: Calling \"RejectAddedKRAByHOD\" method");


            var existingDefTransaction = m_kraContext.DefinitionTransactions.
                                                      Where(d => d.DefinitionTransactionId == defintionTransactionId
                                                      && d.IsActive == true).
                                                      FirstOrDefault<DefinitionTransaction>();

            var defintionDetail = m_kraContext.DefinitionDetails.Find(existingDefTransaction.DefinitionDetailsId);

            if (defintionDetail == null)
            {
                response.IsSuccessful = false;
                response.Message = "Definition Detail not found.";
                return response;
            }
            var defintion = m_kraContext.Definitions.Find(defintionDetail.DefinitionId);
            if (defintion == null)
            {
                response.IsSuccessful = false;
                response.Message = "Definition not found.";
                return response;
            }
            using (var transaction = m_kraContext.Database.BeginTransaction())
            {
                try
                {
                    defintion.ModifiedBy = username;
                    defintion.ModifiedDate = DateTime.UtcNow;
                    defintion.IsActive = false;
                    isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                    if (isUpdated)
                    {
                        DefinitionTransaction definitionTransaction = new DefinitionTransaction()
                        {
                            DefinitionDetailsId = existingDefTransaction.DefinitionDetailsId,
                            Metric = existingDefTransaction.Metric,
                            OperatorId = existingDefTransaction.OperatorId,
                            MeasurementTypeId = existingDefTransaction.MeasurementTypeId,
                            ScaleId = existingDefTransaction.ScaleId,
                            TargetValue = existingDefTransaction.TargetValue,
                            TargetPeriodId = existingDefTransaction.TargetPeriodId,
                            IsActive = false,
                            IsDeleted = null,
                            CreatedBy = existingDefTransaction.CreatedBy,
                            ModifiedBy = username,
                            ModifiedDate = DateTime.UtcNow,
                        };
                        m_kraContext.DefinitionTransactions.Add(definitionTransaction);
                        isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                        if (isUpdated)
                        {
                            existingDefTransaction.IsActive = false;
                            isUpdated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                        }

                    }
                    if (isUpdated)
                    {
                        transaction.Commit();
                        response.IsSuccessful = true;
                        response.Message = "Records updated successfully.";
                    }
                    else
                    {
                        transaction.Rollback();
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while updating a kra record.";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating a kra record.";
                    m_Logger.LogError("Error occured in RejectAddedKRAByHOD() method in DefinitionService " + ex.StackTrace);
                }
            }
            return response;*/
        }
        #endregion
    }
}
