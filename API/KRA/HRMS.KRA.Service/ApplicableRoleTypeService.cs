using HRMS.KRA.Database;
using HRMS.KRA.Entities;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Infrastructure.Models.Domain;
using HRMS.KRA.Infrastructure.Models.Request;
using HRMS.KRA.Infrastructure.Models.Response;
using HRMS.KRA.Infrastructure.Response;
using HRMS.KRA.Types;
using HRMS.KRA.Types.External;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.KRA.Service
{
    public class ApplicableRoleTypeService : IApplicableRoleTypeService
    {
        #region Global Varibles

        private readonly IOrganizationService m_OrganizationService;       
        private readonly ILogger<ApplicableRoleTypeService> m_Logger;
        private readonly KRAContext m_KRAContext;
        IHttpClientFactory m_clientFactory;

        #endregion

        #region ApplicableRoleTypeService

        public ApplicableRoleTypeService(ILogger<ApplicableRoleTypeService> logger, KRAContext kraContext, 
            IOrganizationService organizationService,
             IHttpClientFactory clientFactory)
        {
            m_Logger = logger;
            m_KRAContext = kraContext;
            m_OrganizationService = organizationService;
            m_clientFactory = clientFactory;
        }

        #endregion               

        #region GetAll
        /// <summary>
        /// Get ApplicableRoleTypes By FinancialYearId or DepartmentId or GradeRoleTypeId or StatusId
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ApplicableRoleTypeModel>> GetAll(int? FinancialYearId, int? DepartmentId, 
            int? GradeRoleTypeId, int? StatusId , int? gradeId)
        {
            var response = new ServiceListResponse<ApplicableRoleTypeModel>();
            try
            {
                m_Logger.LogInformation("ApplicableRoleTypeService: Calling \"GetAll\" method.");

                List<ApplicableRoleType> lstApplicableRoleTypes = new List<ApplicableRoleType>();
                List<ApplicableRoleTypeModel> lstappRoleTypesModel = new List<ApplicableRoleTypeModel>();

                var departments = await m_OrganizationService.GetAllDepartments();
                if (!departments.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Items = null;
                    response.Message = departments.Message;
                    return response;
                }               
                var grades = await m_OrganizationService.GetAllGrades();
                if (!grades.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Items = null;
                    response.Message = grades.Message;
                    return response;
                }                
                var financialYears = await m_OrganizationService.GetAllFinancialYears();
                if (!financialYears.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Items = null;
                    response.Message = financialYears.Message;
                    return response;
                }                
                var gradeRoleTypes = await m_OrganizationService.GetAllGradeRoleTypes();
                if (!gradeRoleTypes.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Items = null;
                    response.Message = gradeRoleTypes.Message;
                    return response;
                }                
                var RoleTypes = await m_OrganizationService.GetAllRoleTypes();
                if (!RoleTypes.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Items = null;
                    response.Message = RoleTypes.Message;
                    return response;
                }

                IEnumerable<ApplicableRoleType> query = m_KRAContext.ApplicableRoleTypes;
                //
                if (FinancialYearId.HasValue) query = query.Where(c => c.FinancialYearId == FinancialYearId);
                if (DepartmentId.HasValue) query = query.Where(c => c.DepartmentId == DepartmentId);
                if (GradeRoleTypeId.HasValue) query = query.Where(c => c.GradeRoleTypeId == GradeRoleTypeId);
                if (StatusId.HasValue) query = query.Where(c => c.StatusId == StatusId);
                lstApplicableRoleTypes = query.ToList<ApplicableRoleType>();
                //
                if (lstApplicableRoleTypes != null && lstApplicableRoleTypes.Count > 0)
                {
                    lstappRoleTypesModel = (from art in lstApplicableRoleTypes

                                            join fy in financialYears.Items on art.FinancialYearId equals fy.Id 

                                            join d in departments.Items on art.DepartmentId equals d.DepartmentId

                                            join gt in gradeRoleTypes.Items on art.GradeRoleTypeId equals gt.GradeRoleTypeId

                                            join gra in grades.Items on gt.GradeId equals gra.GradeId

                                            join rt in RoleTypes.Items on gt.RoleTypeId equals rt.RoleTypeId 

                                            where rt.GradeId == gt.GradeId

                                            select new ApplicableRoleTypeModel
                                            {
                                                ApplicableRoleTypeId = art.ApplicableRoleTypeId,
                                                StatusId = art.StatusId,
                                                FinancialYearId = art.FinancialYearId,
                                                //GradeRoleTypeId =new int[1]{ art.GradeRoleTypeId },
                                                GradeRoleTypeId =art.GradeRoleTypeId,
                                                DepartmentId = art.DepartmentId,
                                                DepartmentDescription = d.Description,
                                                GradeId = gra.GradeId,
                                                GradeName = gra.GradeName,
                                                Grade = gra.GradeCode,
                                                FinancialYearName = fy.FinancialYearName,
                                                RoleTypeName = rt.RoleTypeName,
                                                RoleTypeDescription = rt.RoleTypeDescription,
                                                RoleTypeId = rt.RoleTypeId

                                            }).ToList();

                    if (gradeId.HasValue) lstappRoleTypesModel = lstappRoleTypesModel.Where(c => c.GradeId == gradeId).ToList();
                    response.Items = lstappRoleTypesModel;
                    response.IsSuccessful = true;
                    response.Message = string.Empty;
                }
                else
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "ApplicableRoleTypes not found";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching GetApplicableRoleTypes";
                m_Logger.LogError("Error occured in GetApplicableRoleTypes() method." + ex.StackTrace);
            }
            return response;
        }

        #endregion   

        #region Create
        /// <summary>
        /// Create New ApplicableRoleType
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> Create(ApplicableRoleTypeRequest model)
        {
            ServiceResponse<int> response;
            int isCreated;

            m_Logger.LogInformation("ApplicableRoleTypeService: Calling \"Create\" method.");
            try
            {
                var departments = await m_OrganizationService.GetAllDepartments();
                if (departments.Items == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Departments not found."
                    };
                }
                if (departments.Items != null)
                {
                    var department = departments.Items.Where(c => c.DepartmentId == model.DepartmentId)
                        .FirstOrDefault<Department>();
                    if (department == null)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "Department not found."
                        };
                    }
                }
                var financialYears = await m_OrganizationService.GetAllFinancialYears();
                if (financialYears != null)
                {
                    var financialYear = financialYears.Items.Where(c => c.Id == model.FinancialYearId)
                        .FirstOrDefault<FinancialYear>();
                    if (financialYear == null)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "FinancialYear not found."
                        };                        
                    }
                }
                var gradeRoleTypes = await m_OrganizationService.GetAllGradeRoleTypes();
                if (gradeRoleTypes != null)
                {
                    var gadeRoleType = gradeRoleTypes.Items.Where(c => model.GradeRoleTypeId.Contains(c.GradeRoleTypeId) && c.IsActive == true)
                        .ToList<GradeRoleType>();
                    if (gadeRoleType == null || (gadeRoleType != null && gadeRoleType.Count != model.GradeRoleTypeId.Length))
                        {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "gadeRoleType not found."
                        };
                    }
                }
                var draftStatus = m_KRAContext.Statuses.Where(c => c.StatusText == "Draft").FirstOrDefault<Status>();
                if (draftStatus == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Draft status not found."
                    };
                }
                ApplicableRoleType applicableRoleType = null;
                List<ApplicableRoleType> lstApplicableRoleTypes = new List<ApplicableRoleType>();

                Int32 totalCnt = model.GradeRoleTypeId.Length;

                IEnumerable<ApplicableRoleType> query = m_KRAContext.ApplicableRoleTypes
                                                                   .Where(x => x.DepartmentId == model.DepartmentId
                                                                    && x.FinancialYearId == model.FinancialYearId
                                                                    && model.GradeRoleTypeId.Contains(x.GradeRoleTypeId));

                lstApplicableRoleTypes = query.ToList<ApplicableRoleType>();
                if (lstApplicableRoleTypes != null && lstApplicableRoleTypes.Count > 0)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = -1,
                        IsSuccessful = false,
                        Message = "Duplicate record."
                    };
                }

                for (int i = 0; i < totalCnt; i++)
                {
                    applicableRoleType = new ApplicableRoleType()
                    {
                        DepartmentId = model.DepartmentId,
                        FinancialYearId = model.FinancialYearId,
                        GradeRoleTypeId = model.GradeRoleTypeId[i],
                        StatusId = draftStatus.StatusId
                    };
                    m_KRAContext.ApplicableRoleTypes.Add(applicableRoleType);
                }
                isCreated = await m_KRAContext.SaveChangesAsync();
                if (isCreated > 0)
                {
                    m_Logger.LogError("ApplicableRoleType created.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = 1,
                        IsSuccessful = true,
                        Message = "ApplicableRoleType created successfully."
                    };
                }
                else
                {
                    m_Logger.LogError("No ApplicableRoleType created.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = -2,
                        IsSuccessful = false,
                        Message = "No ApplicableRoleType created."
                    };
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured while creating a ApplicableRoleType record." + ex.StackTrace);
                return response = new ServiceResponse<int>()
                {
                    Item = -2,
                    IsSuccessful = false,
                    Message = "No ApplicableRoleType created."
                };
            }           
        }
        #endregion

        #region Update
        /// <summary>
        /// This method updates ApplicableRoleType.
        /// </summary>
        /// <param name="moedl">ApplicableRoleType information</param>
        /// <returns>dynamic</returns>
        public async Task<ServiceResponse<int>> Update(ApplicableRoleTypeRequest model)
        {
            throw new NotImplementedException();
            /*
            bool isUpdated = false;
            ServiceResponse<int> response;

            try
            {
                m_Logger.LogInformation("ApplicableRoleTypeService: Calling \"Update\" method.");
                var definitions = m_KRAContext.Definitions.Where(c => c.ApplicableRoleTypeId == model.ApplicableRoleTypeId);
                if (definitions.Count() > 0)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = -1,
                        IsSuccessful = false,
                        Message = "Can't update already referenced to Definitions.."
                    };
                }

                var applicableRoleType = m_KRAContext.ApplicableRoleTypes.Find(model.ApplicableRoleTypeId);

                m_Logger.LogInformation("ApplicableRoleTypeService: ApplicableRoleType exists?");

                if (applicableRoleType == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = -2,
                        IsSuccessful = false,
                        Message = "ApplicableRoleType not found for update"
                    };
                }

                var departments = await m_OrganizationService.GetAllDepartments();
                if (departments != null)
                {
                    var department = departments.Items.Where(c => c.DepartmentId == model.DepartmentId)
                        .FirstOrDefault<Department>();
                    if (department == null)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = -3,
                            IsSuccessful = false,
                            Message = "Department not found"
                        };
                    }
                    applicableRoleType.DepartmentId = model.DepartmentId;
                }
                var financialYears = await m_OrganizationService.GetAllFinancialYears();
                if (financialYears != null)
                {
                    var financialYear = financialYears.Items.Where(c => c.Id == model.FinancialYearId)
                        .FirstOrDefault<FinancialYear>();
                    if (financialYear == null)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = -3,
                            IsSuccessful = false,
                            Message = "FinancialYear not found"
                        };
                    }
                    applicableRoleType.FinancialYearId = model.FinancialYearId;
                }
                var gradeRoleTypes = await m_OrganizationService.GetAllGradeRoleTypes();
                if (gradeRoleTypes != null)
                {
                    var gadeRoleType = gradeRoleTypes.Items.Where(c => c.GradeRoleTypeId == model.GradeRoleTypeId[0])
                        .FirstOrDefault<GradeRoleType>();
                    if (gadeRoleType == null)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = -3,
                            IsSuccessful = false,
                            Message = "GadeRoleType not found"
                        };
                    }
                    applicableRoleType.GradeRoleTypeId = model.ApplicableRoleTypeId;
                }
                //if (model.StatusId == 0)
                //{
                //    var status = m_KRAContext.Statuses.Where(c => c.StatusId == model.StatusId).FirstOrDefault<Status>();
                //    if (status == null)
                //    {
                //        return response = new ServiceResponse<int>()
                //        {
                //            Item = -4,
                //            IsSuccessful = false,
                //            Message = "Draft status not found"
                //        };
                //    }
                //    applicableRoleType.StatusId = model.StatusId;
                //}
                isUpdated = await m_KRAContext.SaveChangesAsync() > 0 ? true : false;
                if (isUpdated)
                {
                    m_Logger.LogError("ApplicableRoleType updated.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = 1,
                        IsSuccessful = true,
                        Message = "ApplicableRoleType updated successfully."
                    };
                }
                else
                {
                    m_Logger.LogError("No ApplicableRoleType updated.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = -5,
                        IsSuccessful = false,
                        Message = "No ApplicableRoleType updated."
                    };
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured while updating a ApplicableRoleType record." + ex.StackTrace);
                return response = new ServiceResponse<int>()
                {
                    Item = -6,
                    IsSuccessful = false,
                    Message = "No ApplicableRoleType updated."
                };
            }*/
        }

        #endregion

        #region UpdateRoleTypeStatus
        /// <summary>
        /// This method is used to update roletype status.
        /// </summary>
        /// <returns></returns>

        public async Task<ServiceResponse<ApplicableRoleType>> UpdateRoleTypeStatus(ApplicableRoleTypeRequest request)
        {
            throw new NotImplementedException();
            /*
            var response = new ServiceResponse<ApplicableRoleType>();
            try
            {
                m_Logger.LogInformation("UpdateRoleTypeStatus: Calling \"Update\" method.");
                var gradeRoleTypes = await m_OrganizationService.GetAllGradeRoleTypes();
                GradeRoleType gadeRoleType = null;

                if (gradeRoleTypes != null)
                {
                    gadeRoleType = gradeRoleTypes.Items.Where(c => c.GradeId == request.GradeId && c.RoleTypeId == request.RoleTypeId && c.IsActive == true)
                        .FirstOrDefault<GradeRoleType>();
                }

                if (gadeRoleType == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "gradeRoleType not found for update";
                    return response;
                }
                var applicableRoleType = m_KRAContext.ApplicableRoleTypes
                                                   .Where(x => x.DepartmentId == request.DepartmentId &&
                                                          x.FinancialYearId == request.FinancialYearId
                                                          && x.GradeRoleTypeId == gadeRoleType.GradeRoleTypeId).FirstOrDefault();
                if (applicableRoleType == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "ApplicableRoleType not found for update";
                    return response;
                }
                if (request.Status == "FD")
                {
                    var finishedDraftingStatus = m_KRAContext.Statuses.Where(c => c.StatusText == "FinishedDrafting").FirstOrDefault<Status>();
                    if (finishedDraftingStatus == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "FinishedDrafting status not found";
                        return response;
                    }
                    applicableRoleType.StatusId = finishedDraftingStatus.StatusId;
                }
                else if (request.Status == "Draft")
                {
                    var draftStatus = m_KRAContext.Statuses.Where(c => c.StatusText == "Draft").FirstOrDefault<Status>();
                    if (draftStatus == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Draft status not found";
                        return response;
                    }
                    applicableRoleType.StatusId = draftStatus.StatusId;
                }
                else if (request.Status == "ApprovedbyHOD")
                {
                    var approvedbyHODStatus = m_KRAContext.Statuses.Where(c => c.StatusText == "ApprovedbyHOD").FirstOrDefault<Status>();
                    if (approvedbyHODStatus == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "ApprovedbyHOD status not found";
                        return response;
                    }
                    applicableRoleType.StatusId = approvedbyHODStatus.StatusId;

                    var definitionTransactions = m_KRAContext.DefinitionTransactions.
                                     Where(x => x.DefinitionDetails.Definition.ApplicableRoleType.ApplicableRoleTypeId == applicableRoleType.ApplicableRoleTypeId
                                     && x.IsActive==true
                                     ).ToList();

                    DefinitionDetails definitionDetail = null;

                    var definitionDetails = m_KRAContext.DefinitionDetails.
                        Where(x => x.Definition.ApplicableRoleType.ApplicableRoleTypeId == applicableRoleType.ApplicableRoleTypeId).ToList();

                    var definitions = m_KRAContext.Definitions.
                       Where(x => x.ApplicableRoleType.ApplicableRoleTypeId == applicableRoleType.ApplicableRoleTypeId).ToList();

                    foreach (Definition dt in definitions)
                    {
                        dt.IsHODApproved = true;
                    }

                    foreach (DefinitionTransaction dt in definitionTransactions)
                    {
                        definitionDetail = definitionDetails.Where(x => x.DefinitionDetailsId == dt.DefinitionDetailsId).FirstOrDefault();
                        definitionDetail.Metric = dt.Metric;
                        definitionDetail.OperatorId = dt.OperatorId;
                        definitionDetail.MeasurementTypeId = dt.MeasurementTypeId;
                        definitionDetail.ScaleId = dt.ScaleId;
                        definitionDetail.TargetPeriodId = dt.TargetPeriodId;
                        definitionDetail.TargetValue = dt.TargetValue;
                    }

                }
                else if (request.Status == "SentToHOD")
                {
                    var sentToHODStatus = m_KRAContext.Statuses.Where(c => c.StatusText == "SentToHOD").FirstOrDefault<Status>();
                    if (sentToHODStatus == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "SentToHOD status not found";
                        return response;
                    }
                    applicableRoleType.StatusId = sentToHODStatus.StatusId;
                                       
                    var definitions = m_KRAContext.Definitions.
                       Where(x => x.ApplicableRoleType.ApplicableRoleTypeId == applicableRoleType.ApplicableRoleTypeId).ToList();

                    foreach (Definition dt in definitions)
                    {
                        dt.IsHODApproved = false;
                    }
                }
                else if (request.Status == "FinishedEditByHOD")
                {
                    var finishedEditByHODStatus = m_KRAContext.Statuses.Where(c => c.StatusText == "FinishedEditByHOD").FirstOrDefault<Status>();
                    if (finishedEditByHODStatus == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "FinishedEditByHOD status not found";
                        return response;
                    }
                    applicableRoleType.StatusId = finishedEditByHODStatus.StatusId;
                }
                else if (request.Status == "AcceptOriginal")
                {
                    var approvedbyHODStatus = m_KRAContext.Statuses.Where(c => c.StatusText == "ApprovedbyHOD").FirstOrDefault<Status>();
                    if (approvedbyHODStatus == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "ApprovedbyHOD status not found";
                        return response;
                    }
                    applicableRoleType.StatusId = approvedbyHODStatus.StatusId;

                    var definitionTransactions = m_KRAContext.DefinitionTransactions.
                                     Where(x => x.DefinitionDetails.Definition.ApplicableRoleType.ApplicableRoleTypeId == applicableRoleType.ApplicableRoleTypeId
                                     && x.IsActive == true
                                     ).ToList();

                    //new
                    var userroles = await m_OrganizationService.GetUserRoles();
                    if (!userroles.IsSuccessful)
                    {
                        response.IsSuccessful = false;                       
                        response.Message = userroles.Message;
                        return response;
                    }

                    var lsDefinitionTransactions = (from dt in definitionTransactions
                                                   
                                                    join ur in userroles.Items
                                                    on dt.CreatedBy equals ur.Username into crData
                                                    from cduserrole in crData.DefaultIfEmpty()

                                                    join ur in userroles.Items
                                                    on dt.ModifiedBy equals ur.Username into mdData
                                                    from mduserrole in mdData.DefaultIfEmpty()

                                                    select new KRAModel
                                                    {
                                                        DefinitionTransactionId = dt.DefinitionTransactionId , 
                                                        CreatedByUserRole = cduserrole != null ? (cduserrole.Roles.Contains("HRM") ? "HRM" : (cduserrole.Roles.Contains("Department Head")) ? "HOD" : null) : null,
                                                        ModifiedByUserRole = mduserrole != null ? (mduserrole.Roles.Contains("HRM") ? "HRM" : (mduserrole.Roles.Contains("Department Head")) ? "HOD" : null) : null,
                                                    }).ToList();
                    //
                    DefinitionDetails definitionDetail = null;
                    KRAModel transaction = null;

                    var definitionDetails = m_KRAContext.DefinitionDetails.
                        Where(x => x.Definition.ApplicableRoleType.ApplicableRoleTypeId == applicableRoleType.ApplicableRoleTypeId).ToList();

                    foreach (DefinitionDetails dt in definitionDetails)
                    {                       
                        dt.IsDeleted = false;
                    }

                    var definitions = m_KRAContext.Definitions.
                       Where(x => x.ApplicableRoleType.ApplicableRoleTypeId == applicableRoleType.ApplicableRoleTypeId).ToList();

                    foreach (Definition dt in definitions)
                    {
                        dt.IsHODApproved = true;
                        dt.IsDeleted = false;
                    }

                    foreach (DefinitionTransaction dt in definitionTransactions)
                    {
                        transaction= lsDefinitionTransactions.Where(c=>c.DefinitionTransactionId==dt.DefinitionTransactionId).FirstOrDefault();
                        definitionDetail = definitionDetails.Where(x => x.DefinitionDetailsId == dt.DefinitionDetailsId).FirstOrDefault();
                        dt.Metric = definitionDetail.Metric;
                        dt.OperatorId = definitionDetail.OperatorId;
                        dt.MeasurementTypeId = definitionDetail.MeasurementTypeId;
                        dt.ScaleId = definitionDetail.ScaleId;
                        dt.TargetPeriodId = definitionDetail.TargetPeriodId;
                        dt.TargetValue = definitionDetail.TargetValue;
                        dt.IsDeleted = false;
                        if (transaction != null && "HOD".Equals(transaction.CreatedByUserRole))
                        {
                            dt.IsActive = false;
                            definitionDetail.IsActive = false;
                        }
                    }

                }
                if (request.Status == "EditByHR")
                {
                    var editByHRStatus = m_KRAContext.Statuses.Where(c => c.StatusText == "EditByHR").FirstOrDefault<Status>();
                    if (editByHRStatus == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "EditByHR status not found";
                        return response;
                    }
                    applicableRoleType.StatusId = editByHRStatus.StatusId;
                }
                else if(request.Status == "FinishedEditByHR")
                {
                    var finishedEditByHRStatus = m_KRAContext.Statuses.Where(c => c.StatusText == "FinishedEditByHR").FirstOrDefault<Status>();
                    if (finishedEditByHRStatus == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "FinishedEditByHR status not found";
                        return response;
                    }
                    applicableRoleType.StatusId = finishedEditByHRStatus.StatusId;
                }
                else if (request.Status == "EditedByHOD")
                {
                    var editedByHODStatus = m_KRAContext.Statuses.Where(c => c.StatusText == "EditedByHOD").FirstOrDefault<Status>();
                    if (editedByHODStatus == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "EditedByHOD status not found";
                        return response;
                    }
                    applicableRoleType.StatusId = editedByHODStatus.StatusId;

                    var definitions = m_KRAContext.Definitions.
                      Where(x => x.ApplicableRoleType.ApplicableRoleTypeId == applicableRoleType.ApplicableRoleTypeId).ToList();

                    foreach (Definition dt in definitions)
                    {
                        dt.IsHODApproved = false;
                    }
                }
                int retValue = await m_KRAContext.SaveChangesAsync();
                if (retValue > 0)
                {
                    response.IsSuccessful = true;
                    response.Message = "Record Updated Successfully";
                    return response;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No record updated";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while Update RoleTypeStatus";
                m_Logger.LogError("Error occurred while Update RoleTypeStatus" + ex.StackTrace);
            }
            return response;
            */
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete ApplicableRoleType
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool, string)> Delete(int applicableRoleTypeId)
        {
            throw new NotImplementedException();
            /*
            bool isdeleted = false;

            m_Logger.LogInformation("ApplicableRoleTypeService: Calling \"Delete\" method");

            var definitions = m_KRAContext.Definitions.Where(c => c.ApplicableRoleTypeId == applicableRoleTypeId);
            if (definitions.Count() > 0)
                return (isdeleted, "Can't delete already referenced to Definitions.");

            var applicableRoleType = m_KRAContext.ApplicableRoleTypes.Find(applicableRoleTypeId);

            m_Logger.LogInformation("ApplicableRoleTypeService: ApplicableRoleType exists?");

            if (applicableRoleType == null)
                return (isdeleted, "ApplicableRoleType not found for delete.");

            m_Logger.LogInformation("ApplicableRoleTypeService: Calling SaveChanges method on DB Context.");
            m_KRAContext.ApplicableRoleTypes.Remove(applicableRoleType);

            isdeleted = await m_KRAContext.SaveChangesAsync() > 0 ? true : false;
            return (isdeleted, isdeleted ? "Record deleted" : "Not deleted any record.");
            */
        }
        #endregion

    }
}
