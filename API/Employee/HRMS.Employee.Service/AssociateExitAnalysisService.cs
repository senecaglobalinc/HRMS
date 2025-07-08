using AutoMapper;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Common.Enums;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    public class AssociateExitAnalysisService : IAssociateExitAnalysisService
    {
        #region Global Varibles

        private readonly ILogger<AssociateExitAnalysisService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;
        private readonly IOrganizationService m_OrgService;
        private readonly IAssociateExitService m_AssociateExitService;
        #endregion

        #region Constructor
        public AssociateExitAnalysisService(
            ILogger<AssociateExitAnalysisService> logger,
            EmployeeDBContext employeeDBContext,
            IOrganizationService orgService,
            IAssociateExitService associateExitService
            )
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AssociateExitAnalysisService, AssociateExitAnalysisService>();

            });
            m_mapper = config.CreateMapper();

            m_OrgService = orgService;
            m_AssociateExitService = associateExitService;

        }

        #endregion

        #region CreateExitAnalysis
        /// <summary>
        /// Create an Exit Analysis Form
        /// </summary>
        /// <param name="exitAnalysis"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> CreateExitAnalysis(ExitAnalysis exitAnalysis)
        {
            var response = new ServiceResponse<int>();

            try
            {
                int resignedStatus = Convert.ToInt32(AssociateExitStatusCodesNew.Resigned);
                if (resignedStatus == 0)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"No Resigned Status found in ORG Service";
                    return response;
                }

                var employeeDetails = m_EmployeeContext.AssociateExit.Where(st => st.EmployeeId == exitAnalysis.EmployeeId && st.StatusId == resignedStatus && st.IsActive == false).FirstOrDefault();
                if (employeeDetails == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"No employee existes with employeeId {exitAnalysis.EmployeeId}";
                    return response;
                }

                int exitId = employeeDetails.AssociateExitId;
                var employee = m_EmployeeContext.Employees.Where(st => st.EmployeeId == exitAnalysis.EmployeeId && st.IsActive == false).FirstOrDefault();
                if (employee == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"No employee existes with employeeId {exitAnalysis.EmployeeId}";
                    return response;
                }

                int inProgressStatus = Convert.ToInt32(AssociateExitStatusCodesNew.DepartmentActivityInProgress);
                if (inProgressStatus == 0)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"No inProgress Status found in ORG Service";
                    return response;
                }

                int completeStatus = Convert.ToInt32(AssociateExitStatusCodesNew.DepartmentActivityCompleted);
                if (completeStatus == 0)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"No Completed Status found in ORG Service";
                    return response;
                }

                var exitRecord = m_EmployeeContext.AssociateExitAnalysis.Where(st => st.AssociateExitId == exitId).FirstOrDefault();
                if (exitRecord == null)
                {
                    if (exitAnalysis.SubmitType == "save")
                    {
                        AssociateExitAnalysis associateExitAnalysis = new AssociateExitAnalysis();
                        associateExitAnalysis.IsActive = true;
                        associateExitAnalysis.Remarks = exitAnalysis.Remarks;
                        associateExitAnalysis.Responsibility = exitAnalysis.Responsibility;
                        associateExitAnalysis.TagretDate = exitAnalysis.TagretDate;
                        associateExitAnalysis.ActualDate = exitAnalysis.ActualDate;
                        associateExitAnalysis.ActionItem = exitAnalysis.ActionItem;
                        associateExitAnalysis.RootCause = exitAnalysis.RootCause;
                        associateExitAnalysis.AssociateExitId = exitId;

                        associateExitAnalysis.StatusId = inProgressStatus;
                        m_EmployeeContext.AssociateExitAnalysis.Add(associateExitAnalysis);
                    }
                }
                else
                {
                    exitRecord.Remarks = exitAnalysis.Remarks;
                    exitRecord.Responsibility = exitAnalysis.Responsibility;
                    exitRecord.TagretDate = exitAnalysis.TagretDate;
                    exitRecord.ActualDate = exitAnalysis.ActualDate;
                    exitRecord.ActionItem = exitAnalysis.ActionItem;
                    exitRecord.RootCause = exitAnalysis.RootCause;

                    if (exitAnalysis.SubmitType == "submit")
                    {
                        exitRecord.StatusId = completeStatus;
                    }
                }
               
                //Saving Details to database
                int created = await m_EmployeeContext.SaveChangesAsync();
                if (created > 0)
                {
                    response.Item = created;
                    response.IsSuccessful = true;
                    response.Message = $"Associate Exit Analysis record saved Successfully";
                    return response;
                }
                else
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"Error Occured While Updating Database";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Item = 0;
                response.IsSuccessful = false;
                response.Message = $"Error Occured while Updating Associate Exit Analysis";
                return response;
            }
        }
        #endregion

        #region GetAssociateExitAnalysis
        /// <summary>
        /// GetAssociateExitAnalysis
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns>Exit Analysis Information</returns>
        public async Task<ServiceListResponse<GetExitAnalysis>> GetAssociateExitAnalysis(DateTime? fromDate, DateTime? toDate, int? employeeId)
        {
            ServiceListResponse<GetExitAnalysis> response;
            try
            {
                fromDate = fromDate ?? new DateTime();
                toDate = toDate ?? DateTime.Now;
                //  employeeId = employeeId ?? 0;
                List<GetExitAnalysis> getExitAnalysis = new List<GetExitAnalysis>();
                List<int> employees = new List<int>();
                if (employeeId == null)
                {
                    employees = m_EmployeeContext.AssociateExit.Where(st => st.IsActive == false && st.StatusId == Convert.ToInt32(HRMS.Common.Enums.AssociateExitStatusCodesNew.Resigned)
                                                                                 && st.ActualExitDate.Value.Date >= fromDate.Value.Date && st.ActualExitDate.Value.Date <= toDate.Value.Date).Select(st => st.EmployeeId).Distinct().ToList();
                }
                else if (employeeId != null)
                    employees.Add(employeeId ?? 0);
                if (employees.Count == 0)
                {
                    return response = new ServiceListResponse<GetExitAnalysis>()
                    {
                        Items = new List<GetExitAnalysis>(),
                        IsSuccessful = false,
                        Message = "No Employee Exits Found in this Date Range"

                    };
                }
                foreach (int emp in employees)
                {
                    GetExitAnalysis analysis = new GetExitAnalysis();
                    var employeeDetails = m_EmployeeContext.Employees.Where(st => st.EmployeeId == emp && st.IsActive == false).FirstOrDefault();
                    if (employeeDetails == null)
                    {
                        return response = new ServiceListResponse<GetExitAnalysis>()
                        {
                            Items = new List<GetExitAnalysis>(),
                            IsSuccessful = false,
                            Message = $"No Employee found with Id {emp}"

                        };
                    }
                    var employeeExitDetails = m_EmployeeContext.AssociateExit.Where(st => st.IsActive == false && st.StatusId == Convert.ToInt32(HRMS.Common.Enums.AssociateExitStatusCodesNew.Resigned)
                                                                             && st.EmployeeId == emp).FirstOrDefault();
                    if (employeeExitDetails == null)
                    {
                        return response = new ServiceListResponse<GetExitAnalysis>()
                        {
                            Items = new List<GetExitAnalysis>(),
                            IsSuccessful = false,
                            Message = $"No Employee Exit found with EmployeeId {emp}"

                        };
                    }
                    ServiceListResponse<Status> status = await m_OrgService.GetAllStatuses();
                    if (status.Items.Count == 0)
                    {
                        return response = new ServiceListResponse<GetExitAnalysis>()
                        {
                            Items = new List<GetExitAnalysis>(),
                            IsSuccessful = false,
                            Message = $"Error While Fetching Statuses"

                        };
                    }
                    string exitFeedback = m_EmployeeContext.AssociateExitInterview.Where(st => st.AssociateExitId == employeeExitDetails.AssociateExitId).Select(st => st.Remarks).FirstOrDefault();
                    //if (exitFeedback == null)
                    //{
                    //    return response = new ServiceListResponse<GetExitAnalysis>()
                    //    {
                    //        Items = new List<GetExitAnalysis>(),
                    //        IsSuccessful = false,
                    //        Message = $"Exit Feedback not Found For EmployeeId {emp}"

                    //    };
                    //}
                    var exitAnalysisDetails = m_EmployeeContext.AssociateExitAnalysis.Where(st => st.AssociateExitId == employeeExitDetails.AssociateExitId).FirstOrDefault();
                    if (exitAnalysisDetails == null)
                    {
                        analysis.EmployeeId = emp;
                        analysis.EmployeeName = $"{employeeDetails.FirstName} {employeeDetails.LastName}";
                        analysis.SummaryOfExitFeedback = exitFeedback;
                        analysis.EmployeeCode = employeeDetails.EmployeeCode;
                        analysis.ActualExitDate = employeeExitDetails.ActualExitDate;
                        analysis.ExitDate = employeeExitDetails.ExitDate;
                        analysis.ExitReasonId = employeeExitDetails.ExitReasonId;
                        analysis.ExitReasonDetail = employeeExitDetails.ExitReasonDetail;
                        analysis.ExitTypeId = employeeExitDetails.ExitTypeId;
                        ServiceResponse<ExitType> exitType = await m_OrgService.GetExitTypeById(analysis.ExitTypeId);
                        if (exitType.Item == null)
                        {
                            return response = new ServiceListResponse<GetExitAnalysis>()
                            {
                                Items = new List<GetExitAnalysis>(),
                                IsSuccessful = false,
                                Message = $"No  Exit Type found with Exit Id {analysis.ExitTypeId}"

                            };
                        }
                        analysis.ExitType = exitType.Item.Description;
                        analysis.ResignationDate = employeeExitDetails.ResignationDate;
                        analysis.StatusId = 0;

                    }
                    else
                    {
                        analysis.EmployeeId = emp;
                        analysis.EmployeeName = $"{employeeDetails.FirstName} {employeeDetails.LastName}";
                        analysis.SummaryOfExitFeedback = exitFeedback;
                        analysis.EmployeeCode = employeeDetails.EmployeeCode;
                        analysis.ActualExitDate = employeeExitDetails.ActualExitDate;
                        analysis.ExitDate = employeeExitDetails.ExitDate;
                        analysis.ExitReasonId = employeeExitDetails.ExitReasonId;
                        analysis.ExitReasonDetail = employeeExitDetails.ExitReasonDetail;
                        analysis.ExitTypeId = employeeExitDetails.ExitTypeId;
                        ServiceResponse<ExitType> exitType = await m_OrgService.GetExitTypeById(analysis.ExitTypeId);
                        if (exitType.Item == null)
                        {
                            return response = new ServiceListResponse<GetExitAnalysis>()
                            {
                                Items = new List<GetExitAnalysis>(),
                                IsSuccessful = false,
                                Message = $"No  Exit Type found with Exit Id {analysis.ExitTypeId}"

                            };
                        }
                        analysis.ExitType = exitType.Item.Description;
                        analysis.ResignationDate = employeeExitDetails.ResignationDate;
                        analysis.RootCause = exitAnalysisDetails.RootCause;
                        analysis.Responsibility = exitAnalysisDetails.Responsibility;
                        analysis.StatusId = exitAnalysisDetails.StatusId;
                        analysis.Status = status.Items.Where(st => st.StatusId == analysis.StatusId).Select(st => st.StatusCode).FirstOrDefault();
                        analysis.ActionItem = exitAnalysisDetails.ActionItem;
                        analysis.TagretDate = exitAnalysisDetails.TagretDate;
                        analysis.ActualDate = exitAnalysisDetails.ActualDate;
                        analysis.Remarks = exitAnalysisDetails.Remarks;

                    }
                    getExitAnalysis.Add(analysis);

                }
                return response = new ServiceListResponse<GetExitAnalysis>()
                {
                    Items = getExitAnalysis,
                    IsSuccessful = true,

                };
            }
            catch (Exception ex)
            {
                return response = new ServiceListResponse<GetExitAnalysis>()
                {
                    Items = new List<GetExitAnalysis>(),
                    IsSuccessful = false,
                    Message = $"Error Occured in GetExitAnalysis Method"

                };
            }

        }
        #endregion
    }
}
