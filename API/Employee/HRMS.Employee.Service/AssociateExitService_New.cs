using AutoMapper;
using HRMS.Common;
using HRMS.Common.Enums;
using HRMS.Common.Extensions;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Constants;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Request;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    /// <summary>
    /// Service class to Maintain Associate Exits
    /// To Create Prospective Associate Exits
    /// To Update prospective Associate Exits
    /// </summary>
    public class AssociateExitService_New : IAssociateExitService
    {
        #region Global Varibles

        private readonly ILogger<AssociateExitService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IOrganizationService m_OrgService;
        private readonly IProjectService m_ProjectService;
        private readonly IConfiguration m_configuration;
        private readonly AssociateExitMailSubjects m_exitMailSubjects;
        private readonly MiscellaneousSettings m_MiscellaneousSettings;
        private readonly EmailConfigurations m_EmailConfigurations;

        #endregion

        #region Constructor
        public AssociateExitService_New(EmployeeDBContext employeeDBContext,
            ILogger<AssociateExitService> logger,
            IOrganizationService orgService,
            IProjectService projectService,
            IConfiguration configuration,
            IOptions<AssociateExitMailSubjects> exitMailSubjects,
            IOptions<MiscellaneousSettings> miscellaneousSettings,
            IOptions<EmailConfigurations> emailConfigurations)

        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            m_OrgService = orgService;
            m_ProjectService = projectService;
            m_configuration = configuration;
            m_exitMailSubjects = exitMailSubjects.Value;
            m_MiscellaneousSettings = miscellaneousSettings.Value;
            m_EmailConfigurations = emailConfigurations.Value;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateExit>> GetAll(bool? isActive = true)
        {
            var response = new ServiceListResponse<AssociateExit>();
            try
            {
                m_Logger.LogInformation("Calling GetAll method in AssociateExitService");
                var associateExits = await m_EmployeeContext.AssociateExit.Where(pa => pa.IsActive == isActive).ToListAsync();

                response.Items = associateExits;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching AssociateExits";

                m_Logger.LogError("Error occured in GetAssociateExits() method." + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region GetByEmployeeId
        /// <summary>
        /// Get AssociateExit by EmployeeId
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<AssociateExitRequest>> GetByEmployeeId(int employeeId, bool isDecryptReq = false)
        {
            var response = new ServiceResponse<AssociateExitRequest>();
            try
            {
                int? projectId;
                int? associateAllocationId;
                AssociateExitRequest associateExitRequest = new AssociateExitRequest();
                m_Logger.LogInformation("Calling GetAssociatebyEmployeeId method in AssociateExitService");

                int noticePeriodInDays = m_MiscellaneousSettings.NoticePeriodInDays;

                var designations = await m_OrgService.GetAllDesignations();
                if (!designations.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = designations.Message;
                    return response;
                }

                var departments = await m_OrgService.GetAllDepartments();
                if (!departments.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = departments.Message;
                    return response;
                }

                var grades = await m_OrgService.GetAllGrades();
                if (!grades.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = grades.Message;
                    return response;
                }

                var statuses = await m_OrgService.GetStatusesByCategoryName(CategoryMaster.AssociateExit.ToString());
                if (!statuses.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = statuses.Message;
                    return response;
                }

                int resignedStatus = Convert.ToInt32(AssociateExitStatusCodesNew.Resigned);

                //var competencyAreas = await m_OrgService.GetCompetencyAreas(true);
                var associate = await m_EmployeeContext.Employees.Where(pa => pa.EmployeeId == employeeId && pa.IsActive == true).Select(x => x).ToListAsync();
                var associateExit = await m_EmployeeContext.AssociateExit.Where(pa => pa.EmployeeId == employeeId
                                    && ((pa.IsActive == true) || (pa.StatusId == resignedStatus && pa.IsActive == false))).Select(x => x).ToListAsync();
                projectId = associateExit.Count>0? associateExit.FirstOrDefault().ProjectId:null;
                associateAllocationId = associateExit.Count>0? associateExit.FirstOrDefault().AssociateAllocationId:null;
                var associateAbscond = await m_EmployeeContext.AssociateAbscond.Where(pa => pa.AssociateId == employeeId && pa.IsActive == true).Select(x => x).ToListAsync();

                //fetching project and project managers details from associate allocations based on the associateAllocationId 
                var projectManagersDetailsResponse = await GetManagersInfo(associateAllocationId, associate?.FirstOrDefault()).ConfigureAwait(false);
                if (!projectManagersDetailsResponse.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = projectManagersDetailsResponse.Message;
                    return response;
                }
                var projectManagerDetails = projectManagersDetailsResponse.Item;
              

                if (associateExit != null && associateExit.Count > 0)
                {
                    int statusId = associateExit[0].StatusId;
                    var status = statuses.Items.FirstOrDefault(st => st.StatusId == statusId);
                    associateExitRequest = (from pa in associate
                                            join ae in associateExit on pa.EmployeeId equals ae.EmployeeId
                                            join d in departments.Items on pa.DepartmentId equals d.DepartmentId into g1
                                            from dep in g1.DefaultIfEmpty()
                                            join des in designations.Items on pa.DesignationId equals des.DesignationId into g2
                                            from designation in g2.DefaultIfEmpty()
                                            join grade in grades.Items on pa.GradeId equals grade.GradeId into Grades
                                            from gr in Grades.DefaultIfEmpty()
                                                //join technology in competencyAreas.Items on pa.CompetencyGroup equals technology.CompetencyAreaId into technologies
                                                //from tn in technologies.DefaultIfEmpty()
                                            select new AssociateExitRequest
                                            {
                                                AssociateExitId = ae.AssociateExitId,
                                                CalculatedExitDate = Convert.ToDateTime(ae.CalculatedExitDate),
                                                DateOfJoin = Convert.ToDateTime(pa.JoinDate),
                                                Department = dep.DepartmentCode,
                                                DepartmentId = dep.DepartmentId,
                                                Designation = designation.DesignationName,
                                                EmployeeCode = pa.EmployeeCode,
                                                EmployeeId = pa.EmployeeId,
                                                EmployeeName = pa.FirstName + " " + pa.LastName,
                                                ExitDate = Convert.ToDateTime(ae.ExitDate),
                                                ExitTypeId = ae.ExitTypeId,
                                                TotalExperience = pa.TotalExperience ?? 0,
                                                Gender = pa.Gender,
                                                Grade = gr.GradeName,
                                                ImpactOnClientDelivery = ae.ImpactOnClientDelivery,
                                                ImpactOnClientDeliveryDetail = ae.ImpactOnClientDeliveryDetail,
                                                LegalExit = ae.LegalExit,
                                                ProgramManagerId = projectManagerDetails.ProgramManagerId??0,
                                                ReasonId = ae.ExitReasonId,
                                                ReasonDetail = (string.IsNullOrEmpty(ae.ExitReasonDetail) || !isDecryptReq)
                                                               ? null : Utility.DecryptStringAES(ae.ExitReasonDetail),
                                                AssociateRemarks = (string.IsNullOrEmpty(ae.ExitReasonDetail) || !isDecryptReq)
                                                               ? null : Utility.DecryptStringAES(ae.AssociateRemarks),
                                                RehireEligibility = ae.RehireEligibility,
                                                RehireEligibilityDetail = ae.RehireEligibilityDetail,
                                                ReportingManager = projectManagerDetails.ReportingManagerName,
                                                ReportingManagerId = projectManagerDetails.ReportingManagerId??0,
                                                ResignationDate = Convert.ToDateTime(ae.ResignationDate),
                                                ResignationRecommendation = ae.ResignationRecommendation,
                                                //Technology = tn.CompetencyAreaDescription,
                                                ExperiencePriorSG = pa.Experience ?? 0,
                                                TransitionRequired = ae.TransitionRequired,
                                                Status = status.StatusCode,
                                                StatusDesc = status.StatusDescription,
                                                ProgramManager = projectManagerDetails.ProgramManagerName,
                                                ProjectId = projectManagerDetails.ProjectId,
                                                ProjectName = projectManagerDetails.ProjectName,
                                                WithdrawReason = ae.WithdrawReason,
                                                ServiceWithSG = (pa.TotalExperience ?? 0) - (pa.Experience ?? 0),
                                                Tenure = GetAssociateTenure(pa.JoinDate, ae.ExitDate),
                                                NoticePeriodInDays = noticePeriodInDays,
                                                LeadId = projectManagerDetails.LeadId??0,
                                                Lead = projectManagerDetails.LeadName
                                            }).FirstOrDefault();
                }
                else if (associateAbscond != null && associateAbscond.Count > 0)
                {
                    int statusId = associateAbscond[0].StatusId;
                    var status = statuses.Items.SingleOrDefault(st => st.StatusId == statusId);
                    associateExitRequest = (from pa in associate
                                            join aa in associateAbscond on pa.EmployeeId equals aa.AssociateId
                                            join d in departments.Items on pa.DepartmentId equals d.DepartmentId into g1
                                            from dep in g1.DefaultIfEmpty()
                                            join des in designations.Items on pa.DesignationId equals des.DesignationId into g2
                                            from designation in g2.DefaultIfEmpty()
                                            join grade in grades.Items on pa.GradeId equals grade.GradeId into Grades
                                            from gr in Grades.DefaultIfEmpty()
                                            select new AssociateExitRequest
                                            {
                                                AssociateAbscondId = aa.AssociateAbscondId,
                                                CalculatedExitDate = Convert.ToDateTime(aa.AbsentFromDate),
                                                DateOfJoin = Convert.ToDateTime(pa.JoinDate),
                                                Department = dep.DepartmentCode,
                                                DepartmentId = dep.DepartmentId,
                                                Designation = designation.DesignationName,
                                                EmployeeCode = pa.EmployeeCode,
                                                EmployeeId = pa.EmployeeId,
                                                EmployeeName = pa.FirstName + " " + pa.LastName,
                                                ExitDate = Convert.ToDateTime(aa.AbsentFromDate),
                                                TotalExperience = pa.TotalExperience ?? 0,
                                                Gender = pa.Gender,
                                                Grade = gr.GradeName,
                                                ProgramManagerId = projectManagerDetails.ProgramManagerId??0,
                                                ReportingManager = projectManagerDetails.ReportingManagerName,
                                                ReportingManagerId = projectManagerDetails.ReportingManagerId??0,
                                                ExperiencePriorSG = pa.Experience ?? 0,
                                                Status = status.StatusCode,
                                                StatusDesc = status.StatusDescription,
                                                ProgramManager = projectManagerDetails.ProgramManagerName,
                                                ProjectId = projectManagerDetails.ProjectId,
                                                ProjectName = projectManagerDetails.ProjectName,
                                                NoticePeriodInDays = noticePeriodInDays,
                                                BloodGroup = pa.BloodGroup,
                                                LeadId = projectManagerDetails.LeadId??0,
                                                Lead = projectManagerDetails.LeadName,
                                                ServiceWithSG = (pa.TotalExperience ?? 0) - (pa.Experience ?? 0),
                                            }).FirstOrDefault();
                }
                else
                {
                    associateExitRequest = (from pa in associate
                                            join d in departments.Items on pa.DepartmentId equals d.DepartmentId into g1
                                            from dep in g1.DefaultIfEmpty()
                                            join des in designations.Items on pa.DesignationId equals des.DesignationId into g2
                                            from designation in g2.DefaultIfEmpty()
                                            join grade in grades.Items on pa.GradeId equals grade.GradeId into Grades
                                            from gr in Grades.DefaultIfEmpty()
                                                //join technology in competencyAreas.Items on pa.CompetencyGroup equals technology.CompetencyAreaId into technologies
                                                //from tn in technologies.DefaultIfEmpty()
                                            select new AssociateExitRequest
                                            {
                                                AssociateExitId = 0,
                                                DateOfJoin = Convert.ToDateTime(pa.JoinDate),
                                                Department = dep.DepartmentCode,
                                                DepartmentId = dep.DepartmentId,
                                                Designation = designation.DesignationName,
                                                EmployeeCode = pa.EmployeeCode,
                                                EmployeeId = pa.EmployeeId,
                                                EmployeeName = pa.FirstName + " " + pa.LastName,
                                                TotalExperience = pa.TotalExperience ?? 0,
                                                Gender = pa.Gender,
                                                Grade = gr.GradeName,
                                                ProgramManagerId = projectManagerDetails.ProgramManagerId??0,
                                                ReportingManager = projectManagerDetails.ReportingManagerName,
                                                ReportingManagerId = projectManagerDetails.ReportingManagerId??0,
                                                // Technology = tn.CompetencyAreaDescription,
                                                ExperiencePriorSG = pa.Experience ?? 0,
                                                ServiceWithSG = (pa.TotalExperience ?? 0) - (pa.Experience ?? 0),
                                                ProgramManager = projectManagerDetails.ProgramManagerName,
                                                ProjectId = projectManagerDetails.ProjectId,
                                                AssociateAllocationId = projectManagerDetails.AssociateAllocationId,
                                                ProjectName = projectManagerDetails.ProjectName,
                                                NoticePeriodInDays = noticePeriodInDays,
                                                BloodGroup = pa.BloodGroup,
                                                LeadId = projectManagerDetails.LeadId??0,
                                                Lead = projectManagerDetails.LeadName
                                            }).FirstOrDefault();

                }

                response.Item = associateExitRequest;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
                {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching Associate";

                m_Logger.LogError("Error occured in GetAssociatebyEmployeeId() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetAssociateTenure
        //private method for calculating tenure of an associate
        private decimal GetAssociateTenure(DateTime? joindate, DateTime? exitDate)
        {
            int monthDiff = ((Convert.ToDateTime(exitDate).Year - Convert.ToDateTime(joindate).Year) * 12) + Convert.ToDateTime(exitDate).Month - Convert.ToDateTime(joindate).Month;
            int dr = Math.DivRem(monthDiff, 12, out int outd);
            decimal tenuture = outd < 12 ? Convert.ToDecimal(string.Format("{0}.{1}", dr, outd)) : Convert.ToDecimal(dr);
            return tenuture;
        }
        #endregion

        #region Create
        /// <summary>
        /// Create AssociateExit
        /// </summary>
        /// <param name="associateExitIn">associateExitIn</param>
        /// <returns>AssociateExitActivitites</returns>
        public async Task<ServiceResponse<AssociateExit>> Create(AssociateExitRequest associateExitIn)
        {
            int isCreated = 0;
            var response = new ServiceResponse<AssociateExit>();
            try
            {
                m_Logger.LogInformation("Calling Create method in AssociateExitService");

                var empRes = await m_EmployeeContext.Employees.FirstOrDefaultAsync(n => n.EmployeeId == associateExitIn.EmployeeId && n.IsActive == true);
                AssociateExit associateExit = await m_EmployeeContext.AssociateExit.FirstOrDefaultAsync(n => n.EmployeeId == associateExitIn.EmployeeId && n.IsActive == true);

                if (associateExit == null && empRes != null)
                {
                    associateExit = new AssociateExit();
                    associateExit.IsActive = true;
                    associateExit.EmployeeId = associateExitIn.EmployeeId;
                    if (associateExitIn.ExitTypeId == 0)
                        associateExit.ExitTypeId = 1;
                    else
                        associateExit.ExitTypeId = Convert.ToInt32(associateExitIn.ExitTypeId);
                    associateExit.ExitReasonId = associateExitIn.ReasonId;
                    associateExit.ExitReasonDetail = string.IsNullOrEmpty(associateExitIn.ReasonDetail)
                                                    ? null : Utility.EncryptStringAES(associateExitIn.ReasonDetail);
                    associateExit.AssociateRemarks = string.IsNullOrEmpty(associateExitIn.AssociateRemarks)
                                                    ? null : Utility.EncryptStringAES(associateExitIn.AssociateRemarks);
                    associateExit.ExitDate = Convert.ToDateTime(associateExitIn.ExitDate); //from UI
                    associateExit.CalculatedExitDate = Convert.ToDateTime(associateExitIn.ExitDate); // calculated exit date is equals to exit date
                    associateExit.ResignationDate = Convert.ToDateTime(associateExitIn.ResignationDate);  // from UI
                    associateExit.ProjectId = associateExitIn.ProjectId!=null?associateExitIn.ProjectId:null;  // from UI                    
                    associateExit.AssociateAllocationId = associateExitIn.AssociateAllocationId!=null?associateExitIn.AssociateAllocationId:null;  // from UI

                    //For Voluntary Resignation
                    if (associateExitIn.ExitTypeId == 1)
                    {
                        var status = await m_OrgService.GetStatusByCategoryAndStatusCode(StatusCategory.AssociateExit.ToString(),
                                                                           AssociateExitStatusCodesNew.ResignationSubmitted.ToString());
                        associateExit.StatusId = status.Item.StatusId;
                    }

                    m_EmployeeContext.AssociateExit.Add(associateExit);
                    isCreated = await m_EmployeeContext.SaveChangesAsync();

                    if (isCreated > 0)
                    {
                        AssociateExitWorkflow workflow = new AssociateExitWorkflow()
                        {
                            SubmittedBy = associateExitIn.EmployeeId,
                            SubmittedTo = associateExitIn.ProgramManagerId,
                            SubmittedDate = DateTime.Now,
                            WorkflowStatus = associateExit.StatusId,
                            AssociateExitId = associateExit.AssociateExitId,
                            Comments = null
                        };

                        m_EmployeeContext.AssociateExitWorkflow.Add(workflow);
                        isCreated += await m_EmployeeContext.SaveChangesAsync();
                    }
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Associate Exit Request already exist.";
                    return response;
                }

                if (isCreated > 0)
                {
                    ServiceResponse<int> notification = await AssociateExitSendNotification(associateExit.EmployeeId, Convert.ToInt32(NotificationType.ResignationSubmitted), null, null);
                    if (!notification.IsSuccessful)
                    {
                        response.Item = null;
                        response.IsSuccessful = false;
                        response.Message = notification.Message;
                        return response;
                    }
                    response.Item = associateExit;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No Associate Exit created.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to create Associate Exit.";

                m_Logger.LogError("Error occured in Create method." + ex.StackTrace);
            }
            return response;
        }
        #endregion       

        #region ReviewByPM
        /// <summary>
        /// Review AssociateExit By PM
        /// </summary>
        /// <param name="associateExitPMIn">associateExitPMIn</param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> ReviewByPM(AssociateExitPMRequest associateExitPMIn)
        {
            int updated = 0;
            var response = new BaseServiceResponse();
            try
            {
                m_Logger.LogInformation("Calling ReviewByPM method in AssociateExitService");

                var associateExit = await m_EmployeeContext.AssociateExit.FirstOrDefaultAsync(p => p.EmployeeId == associateExitPMIn.EmployeeId && p.IsActive == true);
                ServiceResponse<Status> statusRes = new ServiceResponse<Status>();

                if (associateExit != null)
                {
                    associateExit.RehireEligibility = associateExitPMIn.RehireEligibility ?? false;
                    associateExit.RehireEligibilityDetail = associateExitPMIn.RehireEligibilityDetail;
                    associateExit.ImpactOnClientDelivery = associateExitPMIn.ImpactOnClientDelivery ?? false;
                    associateExit.ImpactOnClientDeliveryDetail = associateExitPMIn.ImpactOnClientDeliveryDetail;

                    if (associateExitPMIn.ExitTypeId == 1)
                    {
                        statusRes = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.AssociateExit.ToString(),
                                                                          AssociateExitStatusCodesNew.ResignationReviewed.ToString());
                        associateExit.StatusId = statusRes.Item.StatusId;
                    }

                    m_Logger.LogInformation("Calling SaveChanges method on DB Context in AssociateExitService");

                    updated = m_EmployeeContext.SaveChanges();

                    var (hrmId, hrmEmail) = await GetHRMId();

                    AssociateExitWorkflow workflow = new AssociateExitWorkflow()
                    {
                        SubmittedBy = associateExitPMIn.ProgramManagerId,
                        SubmittedTo = hrmId,
                        SubmittedDate = DateTime.Now,
                        WorkflowStatus = statusRes.Item.StatusId,
                        AssociateExitId = associateExit.AssociateExitId,
                        Comments = null
                    };

                    m_EmployeeContext.AssociateExitWorkflow.Add(workflow);
                    updated = await m_EmployeeContext.SaveChangesAsync();

                    if (updated > 0)
                    {
                        ServiceResponse<int> notification = await AssociateExitSendNotification(associateExit.EmployeeId, Convert.ToInt32(NotificationType.ResignationReviewed), null, null);
                        if (!notification.IsSuccessful)
                        {
                            response.IsSuccessful = false;
                            response.Message = notification.Message;
                            return response;
                        }

                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "No record updated.";
                    }
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No record found.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to Update AssociateExit.";

                m_Logger.LogError("Error occured in ReviewByPM() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region Approve
        /// <summary>
        /// Approve AssociateExit
        /// </summary>
        /// <param name="associateExitIn">associateExitIn</param>
        /// <returns></returns>
        public async Task<ServiceResponse<AssociateExit>> Approve(AssociateExitRequest associateExitIn)
        {
            int updated = 0;
            var response = new ServiceResponse<AssociateExit>();
            try
            {
                m_Logger.LogInformation("Checking whether the AssociateExit exists with same associate");

                var associateExit = await m_EmployeeContext.AssociateExit.FirstOrDefaultAsync(p => p.EmployeeId == associateExitIn.EmployeeId && p.IsActive == true);

                if (associateExit == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "AssociateExit already exists with Associate.";
                    return response;
                }

                var employee = await m_EmployeeContext.Employees.FirstOrDefaultAsync(st => st.EmployeeId == associateExitIn.EmployeeId && st.IsActive == true);
                ServiceResponse<Status> statusRes = new ServiceResponse<Status>();

                if (associateExitIn.SubmitType == "Approve")
                {
                    associateExit.ResignationRecommendation = associateExitIn.ResignationRecommendation;
                    associateExit.LegalExit = associateExitIn.LegalExit ?? false;
                    associateExit.ExitDate = Convert.ToDateTime(associateExitIn.ExitDate);
                    associateExit.CalculatedExitDate = Convert.ToDateTime(associateExitIn.ExitDate);

                    if (associateExitIn.ExitTypeId == 1)
                    {
                        statusRes = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.AssociateExit.ToString(),
                                                                          AssociateExitStatusCodesNew.ResignationApproved.ToString());
                        associateExit.StatusId = statusRes.Item.StatusId;
                    }
                    if (associateExitIn.ExitTypeId == 2)
                    {
                        statusRes = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.AssociateExit.ToString(),
                                                                          AssociateExitStatusCodesNew.AbscondAcknowledged.ToString());
                        associateExit.StatusId = statusRes.Item.StatusId;
                    }
                    if (associateExitIn.ExitTypeId == 3)
                    {
                        statusRes = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.AssociateExit.ToString(),
                                                                          AssociateExitStatusCodesNew.TerminationApproved.ToString());
                        associateExit.StatusId = statusRes.Item.StatusId;
                    }
                    if (associateExitIn.ExitTypeId == 4)
                    {
                        statusRes = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.AssociateExit.ToString(),
                                                                          AssociateExitStatusCodesNew.SeperationByHRApproved.ToString());
                        associateExit.StatusId = statusRes.Item.StatusId;
                    }

                    if (employee.DepartmentId != 1 && employee.DepartmentId != null)
                    {
                        var associateActivity = m_EmployeeContext.TransitionPlan.Where(prj => prj.AssociateExitId == associateExit.AssociateExitId).FirstOrDefault();
                        if (associateActivity == null)
                        {
                            TransitionPlan Activity = new TransitionPlan
                            {
                                AssociateExitId = associateExit.AssociateExitId,
                                IsActive = true
                            };
                            await m_EmployeeContext.TransitionPlan.AddAsync(Activity);
                        }
                    }

                    var (hrmId, hrmEmail) = await GetHRMId();

                    AssociateExitWorkflow workflow = new AssociateExitWorkflow()
                    {
                        SubmittedBy = hrmId,
                        SubmittedDate = DateTime.Now,
                        WorkflowStatus = statusRes.Item.StatusId,
                        AssociateExitId = associateExit.AssociateExitId,
                        Comments = null
                    };

                    await m_EmployeeContext.AssociateExitWorkflow.AddAsync(workflow);
                }
                else
                {
                    TransitionPlan tpDetail = await m_EmployeeContext.TransitionPlan.FirstOrDefaultAsync(x => x.AssociateExitId == associateExit.AssociateExitId);
                    if (tpDetail != null)
                    {
                        if (Convert.ToDateTime(tpDetail.EndDate).Date > Convert.ToDateTime(associateExitIn.ExitDate).Date)
                        {
                            response.IsSuccessful = true;
                            response.Message = "Last working date must be greater than Transition plan end date!";
                            return response;
                        }
                    }

                    associateExit.ExitDate = Convert.ToDateTime(associateExitIn.ExitDate);
                    associateExit.CalculatedExitDate = Convert.ToDateTime(associateExitIn.ExitDate);
                }

                m_Logger.LogInformation("Calling SaveChanges method on DB Context in AssociateExitService");

                updated = await m_EmployeeContext.SaveChangesAsync();

                if (updated > 0)
                {
                    if (associateExitIn.SubmitType == "Approve")
                    {
                        ServiceResponse<int> notification = await AssociateExitSendNotification(associateExit.EmployeeId, Convert.ToInt32(NotificationType.ResignationApproved), null, null, null);
                        if (!notification.IsSuccessful)
                        {
                            response.Item = null;
                            response.IsSuccessful = false;
                            response.Message = notification.Message;
                            return response;
                        }
                    }
                    else
                    {
                        ServiceResponse<int> notification = await AssociateExitSendNotification(associateExit.EmployeeId, Convert.ToInt32(NotificationType.ExitDateUpdated), null, null, null);
                        if (!notification.IsSuccessful)
                        {
                            response.Item = null;
                            response.IsSuccessful = false;
                            response.Message = notification.Message;
                            return response;
                        }
                    }

                    response.Item = associateExit;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No records updated.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to Update AssociateExit.";

                m_Logger.LogError("Error occured in Update() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetAssociatesForExitDashboard
        /// <summary>
        /// Gets employees List by user role, employeeId and dashboard
        /// </summary>
        /// <param name="userRole"></param>
        /// <param name="employeeId"></param>
        /// <param name="dashboard"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<ExitDashboardResponse>> GetAssociatesForExitDashboard(string userRole, int employeeId, string dashboard, int departmentId)
        {
            ServiceListResponse<ExitDashboardResponse> response = new ServiceListResponse<ExitDashboardResponse>();

            if (string.IsNullOrEmpty(userRole) || string.IsNullOrEmpty(dashboard) || employeeId == 0)
            {
                return response = new ServiceListResponse<ExitDashboardResponse>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "Invalid Request."
                };
            }

            int resignedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.Resigned);
            int reviewedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.ResignationReviewed);
            int resignationApprovedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.ResignationApproved);
            int ktPlanInProgressStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.KTPlanInProgress);
            int ktPlanSubmittedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.KTPlanSubmitted);
            int ktPlanCompletedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.KTPlanCompleted);
            int resignationInProgressStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.ResignationInProgress);
            int deptActivityInprogress = Convert.ToInt32(AssociateExitStatusCodesNew.DepartmentActivityInProgress);
            int completedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.ExitInterviewCompleted);
            int resignationSubmittedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.ResignationSubmitted);
            int abscondApprovedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.AbscondAcknowledged);
            int terminationApprovedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.TerminationApproved);
            int seperationByHRApprovedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.SeperationByHRApproved);
            int revokeInitiatedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.RevokeInitiated);
            int resignationRevokedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.ResignationRevoked);
            int deptActivityCompleted = Convert.ToInt32(AssociateExitStatusCodesNew.DepartmentActivityCompleted);
            int readyForClearance = Convert.ToInt32(AssociateExitStatusCodesNew.ReadyForClearance);
            int[] ktStatusIds = new int[] { resignationSubmittedStatusId, reviewedStatusId, resignedStatusId };

            ServiceListResponse<Status> statusesforAssociateExit = await m_OrgService.GetStatusesByCategoryName(CategoryMaster.AssociateExit.ToString());
            if (statusesforAssociateExit.Items == null || (statusesforAssociateExit.Items != null && statusesforAssociateExit.Items.Count <= 0))
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "Status for Associate Exit not found.";
                return response;
            }

            var designations = await m_OrgService.GetAllDesignations();
            if (!designations.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.Message = designations.Message;
                return response;
            }

            var departments = await m_OrgService.GetAllDepartments();
            if (!departments.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.Message = departments.Message;
                return response;
            }

            List<ExitEmployeeResponse> exitEmployees = null;
            ServiceListResponse<AssociateAllocation> associateAllocation = null;
            List<AssociateAllocation> allocationDtls = null;

            if (Roles.Associate.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()))
            {
                exitEmployees = await (from emp in m_EmployeeContext.Employees
                                       join associateExit in m_EmployeeContext.AssociateExit.Where(w => w.IsActive == true) on emp.EmployeeId equals associateExit.EmployeeId
                                       join transitionPlan in m_EmployeeContext.TransitionPlan.Where(w => w.StatusId != ktPlanCompletedStatusId)
                                       on associateExit.AssociateExitId equals transitionPlan.AssociateExitId into tp
                                       from emptp in tp.DefaultIfEmpty()
                                       where associateExit.StatusId != resignedStatusId && associateExit.EmployeeId == employeeId
                                       orderby associateExit.ExitDate
                                       select new ExitEmployeeResponse
                                       {
                                           EmployeeId = emp.EmployeeId,
                                           EmployeeCode = emp.EmployeeCode,
                                           FirstName = emp.FirstName,
                                           LastName = emp.LastName,
                                           DepartmentId = emp.DepartmentId.Value,
                                           DesignationId = emp.DesignationId.Value,
                                           AssociateExitId = associateExit.AssociateExitId,
                                           StatusId = associateExit.StatusId,
                                           ExitDate = Convert.ToDateTime(associateExit.ExitDate),
                                           SubStatusId = emptp != null ? emptp.StatusId : 0
                                       }).Distinct().ToListAsync();

                if (dashboard.ToLower() == StringConstants.DeliveryDashboard.ToLower())
                {

                    if (allocationDtls != null && allocationDtls.Count > 0)
                    {
                        exitEmployees = (from emp in exitEmployees
                                         join all in allocationDtls on emp.EmployeeId equals all.EmployeeId
                                         select new ExitEmployeeResponse
                                         {
                                             EmployeeId = emp.EmployeeId,
                                             EmployeeCode = emp.EmployeeCode,
                                             FirstName = emp.FirstName,
                                             LastName = emp.LastName,
                                             DepartmentId = emp.DepartmentId,
                                             DesignationId = emp.DesignationId,
                                             AssociateExitId = emp.AssociateExitId,
                                             StatusId = emp.StatusId,
                                             RevokeStatusId = emp.RevokeStatusId,
                                             ExitDate = emp.ExitDate,
                                             ProjectId = all.ProjectId,
                                             SubStatusId = emp.SubStatusId
                                         }).Distinct().ToList();
                    }
                }
            }
            else if (Roles.ProgramManager.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()))
            {
                exitEmployees = await (from emp in m_EmployeeContext.Employees
                                       join associateExit in m_EmployeeContext.AssociateExit.Where(w => w.IsActive == true) on emp.EmployeeId equals associateExit.EmployeeId
                                       join exitwf in m_EmployeeContext.AssociateExitWorkflow on associateExit.AssociateExitId equals exitwf.AssociateExitId
                                       where exitwf.SubmittedTo == employeeId && associateExit.StatusId != resignedStatusId
                                       orderby associateExit.ExitDate
                                       select new ExitEmployeeResponse
                                       {
                                           EmployeeId = emp.EmployeeId,
                                           EmployeeCode = emp.EmployeeCode,
                                           FirstName = emp.FirstName,
                                           LastName = emp.LastName,
                                           DepartmentId = emp.DepartmentId.Value,
                                           DesignationId = emp.DesignationId.Value,
                                           AssociateExitId = associateExit.AssociateExitId,
                                           StatusId = associateExit.StatusId,
                                           ExitDate = Convert.ToDateTime(associateExit.ExitDate)
                                       }).Distinct().ToListAsync();
            }
            else if (Roles.HRM.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()))
            {
                if (dashboard == "ActiveResignationsDashboard")
                {
                    exitEmployees = await (from emp in m_EmployeeContext.Employees
                                           join associateExit in m_EmployeeContext.AssociateExit.Where(w => w.IsActive == true) on emp.EmployeeId equals associateExit.EmployeeId
                                           join erw in m_EmployeeContext.AssociateExitRevokeWorkflow.Where(w => w.IsActive == true)
                                           on associateExit.AssociateExitId equals erw.AssociateExitId into emperw
                                           from workflow in emperw.DefaultIfEmpty()
                                           where (associateExit.StatusId != resignedStatusId && associateExit.StatusId != resignationRevokedStatusId)
                                           || workflow.RevokeStatusId == revokeInitiatedStatusId
                                           orderby associateExit.ExitDate
                                           select new ExitEmployeeResponse
                                           {
                                               EmployeeId = emp.EmployeeId,
                                               EmployeeCode = emp.EmployeeCode,
                                               FirstName = emp.FirstName,
                                               LastName = emp.LastName,
                                               DepartmentId = emp.DepartmentId.Value,
                                               DesignationId = emp.DesignationId.Value,
                                               AssociateExitId = associateExit.AssociateExitId,
                                               RevokeStatusId = workflow.RevokeStatusId,
                                               StatusId = associateExit.StatusId,
                                               ExitDate = Convert.ToDateTime(associateExit.ExitDate)
                                           }).Distinct().ToListAsync();
                }
                else
                {
                    exitEmployees = await GetExitEmployeesForChecklist(departmentId);
                }
            }
            else if (Roles.HRA.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()))
            {
                exitEmployees = await (from emp in m_EmployeeContext.Employees
                                       join associateExit in m_EmployeeContext.AssociateExit.Where(w => w.IsActive == true) on emp.EmployeeId equals associateExit.EmployeeId
                                       where (associateExit.StatusId == resignationInProgressStatusId || associateExit.StatusId == resignationApprovedStatusId || associateExit.StatusId == readyForClearance)
                                       orderby associateExit.ExitDate
                                       select new ExitEmployeeResponse
                                       {
                                           EmployeeId = emp.EmployeeId,
                                           EmployeeCode = emp.EmployeeCode,
                                           FirstName = emp.FirstName,
                                           LastName = emp.LastName,
                                           DepartmentId = emp.DepartmentId.Value,
                                           DesignationId = emp.DesignationId.Value,
                                           AssociateExitId = associateExit.AssociateExitId,
                                           StatusId = associateExit.StatusId,
                                           ExitDate = Convert.ToDateTime(associateExit.ExitDate)
                                       }).Distinct().ToListAsync();

                var exitEmployeeActivityJoin = (from exitEmp in exitEmployees
                                                join exitActivity in m_EmployeeContext.AssociateExitActivity on exitEmp.AssociateExitId equals exitActivity.AssociateExitId
                                                into exitEmpActivityJoin
                                                orderby exitEmp.AssociateExitId
                                                select new
                                                {
                                                    ExitEmployee = exitEmp,
                                                    DeptActivityCount = exitEmpActivityJoin.Count(),
                                                    SubStatusId = exitEmpActivityJoin.Count() > 0 ? (exitEmpActivityJoin.All(x => x.StatusId == deptActivityCompleted) ? deptActivityCompleted : deptActivityInprogress) : 0
                                                }).ToList();

                foreach (var item in exitEmployeeActivityJoin)
                {
                    exitEmployees.FirstOrDefault(x => x.AssociateExitId == item.ExitEmployee.AssociateExitId).SubStatusId = item.DeptActivityCount > 0 ? item.SubStatusId : 0;
                }
            }
            else if (Roles.Corporate.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()))
            {
                exitEmployees = await (from emp in m_EmployeeContext.Employees
                                       join associateExit in m_EmployeeContext.AssociateExit.Where(w => w.IsActive == true) on emp.EmployeeId equals associateExit.EmployeeId
                                       join associateExitInterview in m_EmployeeContext.AssociateExitInterview on associateExit.AssociateExitId equals associateExitInterview.AssociateExitId
                                       into ei
                                       from exitInterview in ei.DefaultIfEmpty()
                                       where (associateExit.StatusId == resignationApprovedStatusId || associateExit.StatusId == resignationInProgressStatusId || associateExit.StatusId == readyForClearance)
                                       orderby associateExit.ExitDate
                                       select new ExitEmployeeResponse
                                       {
                                           EmployeeId = emp.EmployeeId,
                                           EmployeeCode = emp.EmployeeCode,
                                           FirstName = emp.FirstName,
                                           LastName = emp.LastName,
                                           DepartmentId = emp.DepartmentId.Value,
                                           DesignationId = emp.DesignationId.Value,
                                           AssociateExitId = associateExit.AssociateExitId,
                                           StatusId = associateExit.StatusId,
                                           ExitDate = Convert.ToDateTime(associateExit.ExitDate),
                                           SubStatusId = exitInterview != null ? Convert.ToInt32(AssociateExitStatusCodesNew.ExitInterviewCompleted) : 0
                                       }).Distinct().ToListAsync();
            }
            else if (Roles.TeamLead.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()) && dashboard.ToLower() == StringConstants.DeliveryDashboard.ToLower())
            {
                exitEmployees = await (from emp in m_EmployeeContext.Employees
                                       join associateExit in m_EmployeeContext.AssociateExit.Where(w => w.IsActive == true) on emp.EmployeeId equals associateExit.EmployeeId
                                       join transitionPlan in m_EmployeeContext.TransitionPlan on associateExit.AssociateExitId equals transitionPlan.AssociateExitId
                                       into tp
                                       from emptp in tp.DefaultIfEmpty()
                                       where !ktStatusIds.Contains(associateExit.StatusId) && associateExit.TransitionRequired != false && emp.DepartmentId == departmentId
                                       orderby associateExit.ExitDate
                                       select new ExitEmployeeResponse
                                       {
                                           EmployeeId = emp.EmployeeId,
                                           EmployeeCode = emp.EmployeeCode,
                                           FirstName = emp.FirstName,
                                           LastName = emp.LastName,
                                           DepartmentId = emp.DepartmentId.Value,
                                           DesignationId = emp.DesignationId.Value,
                                           AssociateExitId = associateExit.AssociateExitId,
                                           StatusId = associateExit.StatusId,
                                           ExitDate = Convert.ToDateTime(associateExit.ExitDate),
                                           SubStatusId = emptp != null ? emptp.StatusId : 0,
                                           TransitionRequired = associateExit.TransitionRequired ?? false,
                                           ProjectId=associateExit.ProjectId,
                                           AssociateAllocationId=associateExit.AssociateAllocationId
                                       }).Distinct().ToListAsync();
                if(exitEmployees.Count>0)
                {
                    var ids = exitEmployees.Select(x => x.EmployeeId).ToList();
                    var allocations = (await m_ProjectService.GetAssociateAllocations(ids));
                    if(!allocations.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching allocation details";
                        return response;
                    }
                    exitEmployees = (from emp in exitEmployees
                                     join all in allocations.Items on emp.AssociateAllocationId equals all.AssociateAllocationId where all.LeadId==employeeId
                                     select emp).ToList();                                   
                                   
                }
                else
                {
                    exitEmployees = new List<ExitEmployeeResponse>();
                }

            }
            else if (Roles.TeamLead.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()) && dashboard.ToLower() == StringConstants.ServiceDashboard.ToLower())
            {
                exitEmployees = await (from emp in m_EmployeeContext.Employees
                                       join associateExit in m_EmployeeContext.AssociateExit.Where(w => w.IsActive == true) on emp.EmployeeId equals associateExit.EmployeeId
                                       join exitwf in m_EmployeeContext.AssociateExitWorkflow on associateExit.AssociateExitId equals exitwf.AssociateExitId
                                       join transitionPlan in m_EmployeeContext.TransitionPlan on associateExit.AssociateExitId equals transitionPlan.AssociateExitId
                                       into tp
                                       from emptp in tp.DefaultIfEmpty()
                                       where exitwf.SubmittedTo == employeeId && associateExit.StatusId != resignedStatusId
                                       orderby associateExit.ExitDate
                                       select new ExitEmployeeResponse
                                       {
                                           EmployeeId = emp.EmployeeId,
                                           EmployeeCode = emp.EmployeeCode,
                                           FirstName = emp.FirstName,
                                           LastName = emp.LastName,
                                           DepartmentId = emp.DepartmentId.Value,
                                           DesignationId = emp.DesignationId.Value,
                                           AssociateExitId = associateExit.AssociateExitId,
                                           StatusId = associateExit.StatusId,
                                           ExitDate = Convert.ToDateTime(associateExit.ExitDate),
                                           SubStatusId = emptp != null ? emptp.StatusId : 0,
                                           TransitionRequired = associateExit.TransitionRequired ?? false
                                       }).Distinct().ToListAsync();
            }
            else if (Roles.ITManager.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim())
                || Roles.FinanceManager.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim())
                || Roles.AdminManager.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim())
                || Roles.TrainingDepartmentHead.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()))
            {
                exitEmployees = await GetExitEmployeesForChecklist(departmentId);
            }

            List<ExitDashboardResponse> dashboardResponses = new List<ExitDashboardResponse>();

            if (exitEmployees != null && exitEmployees.Count > 0)
            {
                exitEmployees = (from emp in exitEmployees
                                 join erw in m_EmployeeContext.AssociateExitRevokeWorkflow.Where(w => w.IsActive == true)
                                 on emp.AssociateExitId equals erw.AssociateExitId into emperw
                                 from workflow in emperw.DefaultIfEmpty()
                                 select new ExitEmployeeResponse
                                 {
                                     EmployeeId = emp.EmployeeId,
                                     EmployeeCode = emp.EmployeeCode,
                                     FirstName = emp.FirstName,
                                     LastName = emp.LastName,
                                     DepartmentId = emp.DepartmentId,
                                     DesignationId = emp.DesignationId,
                                     AssociateExitId = emp.AssociateExitId,
                                     StatusId = emp.StatusId,
                                     RevokeStatusId = workflow != null ? workflow.RevokeStatusId : 0,
                                     RevokeComment = workflow != null ? workflow.RevokeComment : "",
                                     ExitDate = emp.ExitDate,
                                     ProjectId = emp.ProjectId,
                                     SubStatusId = emp.SubStatusId,
                                     TransitionRequired = emp.TransitionRequired
                                 }).Distinct().ToList();

                foreach (var employee in exitEmployees)
                {
                    ExitDashboardResponse dashboardResponse = new ExitDashboardResponse();
                    var designation = designations.Items.FirstOrDefault(st => st.DesignationId == employee.DesignationId);
                    var dep = departments.Items.FirstOrDefault(st => st.DepartmentId == employee.DepartmentId);
                    dashboardResponse.EmployeeId = employee.EmployeeId;
                    dashboardResponse.Designation = designation.DesignationName;
                    dashboardResponse.DepartmentName = dep.Description;
                    dashboardResponse.DepartmentId = dep.DepartmentId;
                    dashboardResponse.EmployeeCode = employee.EmployeeCode;
                    dashboardResponse.ExitDate = employee.ExitDate;
                    dashboardResponse.EmployeeName = $"{employee.FirstName} {employee.LastName}";
                    dashboardResponse.ProjectId = employee.ProjectId ?? 0;
                    dashboardResponse.RevokeComment = employee.RevokeComment;
                    dashboardResponse.TransitionRequired = employee.TransitionRequired;

                    var status = statusesforAssociateExit.Items.FirstOrDefault(st => st.StatusId == employee?.StatusId);
                    dashboardResponse.StatusCode = status?.StatusCode;
                    dashboardResponse.StatusDesc = status?.StatusDescription;

                    if (employee.RevokeStatusId != 0)
                    {
                        var revokeStatus = statusesforAssociateExit.Items.FirstOrDefault(st => st.StatusId == employee.RevokeStatusId);
                        dashboardResponse.RevokeStatusCode = revokeStatus?.StatusCode;
                        dashboardResponse.RevokeStatusDesc = revokeStatus?.StatusDescription;
                    }

                    if (employee.SubStatusId != 0)
                    {
                        var subStatus = statusesforAssociateExit.Items.FirstOrDefault(st => st.StatusId == employee.SubStatusId);
                        dashboardResponse.SubStatusCode = subStatus?.StatusCode;
                        dashboardResponse.SubStatusDesc = subStatus?.StatusDescription;
                    }
                    else if (!employee.TransitionRequired)
                    {
                        dashboardResponse.SubStatusCode = StringConstants.KTNotRequired.Replace(" ", "");
                        dashboardResponse.SubStatusDesc = StringConstants.KTNotRequired;
                    }

                    if (employee.ProjectId != null && employee.ProjectId != 0)
                    {
                        ServiceResponse<Project> projectDetails = await m_ProjectService.GetProjectByID(employee.ProjectId ?? 0);
                        dashboardResponse.ProjectName = projectDetails?.Item?.ProjectName;
                    }

                    dashboardResponses.Add(dashboardResponse);
                }
            }

            response.Items = dashboardResponses;
            response.IsSuccessful = true;
            response.Message = string.Empty;
            return response;
        }

        #endregion

        #region RevokeExitInitiation
        /// <summary>
        /// Initiate Revoke of Exit by Associate
        /// </summary>
        /// <param name="revokeRequest"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> RevokeExit(RevokeRequest revokeRequest)
        {
            var response = new ServiceResponse<int>();
            ServiceResponse<Status> revokeInitiatonStatus = new ServiceResponse<Status>();
            try
            {
                var exitDetails = await m_EmployeeContext.AssociateExit.FirstOrDefaultAsync(st => st.EmployeeId == revokeRequest.EmployeeId && st.IsActive == true);
                if (exitDetails == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"No employee existes with employeeId {revokeRequest.EmployeeId}";
                    return response;
                }

                var employee = await m_EmployeeContext.Employees.FirstOrDefaultAsync(st => st.EmployeeId == revokeRequest.EmployeeId && st.IsActive == true);
                if (employee == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"No employee found with employeeId {revokeRequest.EmployeeId}";
                    return response;
                }

                exitDetails.WithdrawReason = revokeRequest.RevokeReason;

                revokeInitiatonStatus = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.AssociateExit.ToString(), AssociateExitStatusCodesNew.RevokeInitiated.ToString());
                if (revokeInitiatonStatus.Item == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"No Revoke Initiated Status found in ORG Service";
                    return response;
                }

                AssociateExitRevokeWorkflow revokeWorkflow = new AssociateExitRevokeWorkflow()
                {
                    AssociateExitId = exitDetails.AssociateExitId,
                    RevokeStatusId = revokeInitiatonStatus.Item.StatusId,
                    RevokeComment = revokeRequest.RevokeReason,
                    IsActive = true
                };

                await m_EmployeeContext.AssociateExitRevokeWorkflow.AddAsync(revokeWorkflow);

                var (hrmId, hrmEmail) = await GetHRMId();

                AssociateExitWorkflow workflow = new AssociateExitWorkflow()
                {
                    SubmittedBy = exitDetails.EmployeeId,
                    SubmittedTo = hrmId,
                    SubmittedDate = DateTime.Now,
                    WorkflowStatus = revokeInitiatonStatus.Item.StatusId,
                    AssociateExitId = exitDetails.AssociateExitId,
                    Comments = revokeRequest.Comment,
                    IsActive = true
                };

                await m_EmployeeContext.AssociateExitWorkflow.AddAsync(workflow);
                //Saving Details to database
                int created = await m_EmployeeContext.SaveChangesAsync();

                if (created > 0)
                {
                    ServiceResponse<int> notification = await AssociateExitSendNotification(exitDetails.EmployeeId, Convert.ToInt32(NotificationType.RevokeInitiated), null, null, null);
                    if (!notification.IsSuccessful)
                    {
                        response.Item = 0;
                        response.IsSuccessful = false;
                        response.Message = notification.Message;
                        return response;
                    }
                    response.Item = created;
                    response.IsSuccessful = true;
                    response.Message = $"Associate Revoked Successfully";
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
                response.Message = $"Error Occured while Revoking Associate Resignation";
                return response;
            }
        }
        #endregion

        #region ExitClearance
        /// <summary>
        /// Provide Exit Clearance By HRM
        /// </summary>
        /// <param name="clearanceRequest"></param>
        /// <returns>Integer</returns>
        public async Task<ServiceResponse<int>> ExitClearance(ClearanceRequest clearanceRequest)
        {
            var response = new ServiceResponse<int>();
            try
            {
                //ServiceResponse<Status> resignedStatus = new ServiceResponse<Status>();
                var statusApiCall = await m_OrgService.GetAllStatuses();
                Status resignedStatus = null;

                if (statusApiCall.IsSuccessful)
                {
                    resignedStatus = statusApiCall.Items.Where(x => x.CategoryMasterId == Convert.ToInt32(CategoryMaster.AssociateExit) && x.StatusId == Convert.ToInt32(AssociateExitStatusCodesNew.Resigned)).FirstOrDefault();
                }

                if (resignedStatus == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"No Resigned Status found in ORG Service";
                    return response;
                }

                var exitDetails = m_EmployeeContext.AssociateExit.Where(st => st.EmployeeId == clearanceRequest.employeeId && st.IsActive == true).FirstOrDefault();
                if (exitDetails == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"Exit Details for EmployeeId {clearanceRequest.employeeId} Not Found";
                    return response;
                }

                var employee = m_EmployeeContext.Employees.Where(st => st.EmployeeId == clearanceRequest.employeeId && st.IsActive == true).FirstOrDefault();
                if (employee == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"Employee Details for EmployeeId {clearanceRequest.employeeId} Not Found";
                    return response;
                }

                if (Roles.HRM.GetEnumDescription().TrimLowerCase().Equals(clearanceRequest.UserRole.ToLower().Trim()))
                {
                    exitDetails.StatusId = resignedStatus.StatusId;

                    var hrmComment = await m_EmployeeContext.Remarks.FirstOrDefaultAsync(st => st.AssociateExitId == exitDetails.AssociateExitId && st.RoleId == Convert.ToInt32(Roles.HRM));
                    if (hrmComment == null)
                    {
                        Remarks hrmRemarks = new Remarks
                        {
                            AssociateExitId = exitDetails.AssociateExitId,
                            RoleId = Convert.ToInt32(Roles.HRM),
                            Comment = clearanceRequest.RemarksByHRM
                        };

                        m_EmployeeContext.Remarks.Add(hrmRemarks);
                    }
                    else
                    {
                        hrmComment.Comment = clearanceRequest.RemarksByHRM;
                    }
                }
                else if (Roles.HRA.GetEnumDescription().TrimLowerCase().Equals(clearanceRequest.UserRole.ToLower().Trim()))
                {
                    var hraComment = await m_EmployeeContext.Remarks.FirstOrDefaultAsync(st => st.AssociateExitId == exitDetails.AssociateExitId && st.RoleId == Convert.ToInt32(Roles.HRA));
                    if (hraComment == null)
                    {
                        Remarks hraRemarks = new Remarks
                        {
                            AssociateExitId = exitDetails.AssociateExitId,
                            RoleId = Convert.ToInt32(Roles.HRA),
                            Comment = clearanceRequest.RemarksByHRA
                        };

                        m_EmployeeContext.Remarks.Add(hraRemarks);
                    }
                    else
                    {
                        hraComment.Comment = clearanceRequest.RemarksByHRA;
                    }
                }

                bool hasUnsavedChanges = m_EmployeeContext.ChangeTracker.Entries<Remarks>().Any(e => e.State == EntityState.Added || e.State == EntityState.Modified);
                int created = hasUnsavedChanges ? m_EmployeeContext.SaveChanges() : 0;
                if (created > 0)
                {
                    if (exitDetails.StatusId == resignedStatus.StatusId && Roles.HRM.GetEnumDescription().TrimLowerCase().Equals(clearanceRequest.UserRole.ToLower().Trim()))
                    {
                        ServiceResponse<int> notification = await AssociateExitSendNotification(exitDetails.EmployeeId, Convert.ToInt32(NotificationType.Resigned), null, null);
                        if (!notification.IsSuccessful)
                        {
                            response.Item = 0;
                            response.IsSuccessful = false;
                            response.Message = notification.Message;
                            return response;
                        }

                        //Updating Exit date in AssociateExit and Employee table
                        exitDetails.ActualExitDate = clearanceRequest.UpdateExitDateRequired ? exitDetails.ExitDate : DateTime.Today;
                        employee.RelievingDate = clearanceRequest.UpdateExitDateRequired ? exitDetails.ExitDate : DateTime.Today;

                        //Update allocation table
                        string exitDateToUpdate = clearanceRequest.UpdateExitDateRequired ? exitDetails.ExitDate.Value.ToShortDateString() : DateTime.Today.ToShortDateString();
                        ServiceResponse<bool> releaseOnExit = await m_ProjectService.ReleaseOnExit(employee.EmployeeId, exitDateToUpdate);

                        //Remove UserRoles
                        var removeUserRole = await m_OrgService.RemoveUserRoleOnExit(employee.UserId.Value);

                        //Inactivate the user
                        var inactiveUser = await m_OrgService.UpdateUser(employee.UserId.Value);

                        //Update exit
                        exitDetails.IsActive = false;

                        //Update employee
                        employee.IsActive = false;
                        employee.StatusId = Convert.ToInt32(AssociateExitStatusCodesNew.Resigned);
                        employee.UserId = null;

                        int isUpdated = await m_EmployeeContext.SaveChangesAsync();

                        if (isUpdated == 0 && !releaseOnExit.IsSuccessful && !removeUserRole && !inactiveUser)
                        {
                            response.IsSuccessful = false;
                            response.Item = 0;
                            response.Message = "Error while updating exit details";
                            return response;
                        }
                    }

                    response.Item = created;
                    response.IsSuccessful = true;
                    response.Message = $"Clearance Submitted successfully";
                    return response;
                }
                else
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"Error Occured While submitting Clearance Form";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Item = 0;
                response.IsSuccessful = false;
                response.Message = $"Error Occured While submitting Clearance Form";
                return response;
            }
        }
        #endregion

        #region RequestForWithdrawResignation
        /// <summary>
        /// Request Associate to Withdraw Resignation
        /// </summary>
        /// <param name = "employeeId" > employeeId </ param >
        /// <param name="resignationRecommendation">notificationType</param>
        /// < returns ></ returns >
        public async Task<ServiceResponse<int>> RequestForWithdrawResignation(int employeeId, string resignationRecommendation)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            try
            {
                m_Logger.LogInformation("Calling \"RequestForWithdrawResignation\" method in AssociateExit Service");
                Entities.Employee employee = m_EmployeeContext.Employees.Where(e => e.EmployeeId == employeeId && e.IsActive == true).FirstOrDefault();
                if (employee == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "Employee Not Found";
                    return response;
                }

                var exitDetails = m_EmployeeContext.AssociateExit.Where(e => e.EmployeeId == employeeId && e.IsActive == true).FirstOrDefault();
                if (exitDetails == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "Employee Exit Details Not Found";
                    return response;
                }

                if (resignationRecommendation != null)
                {
                    //TODO: status needs to be updated in AssociateExit and new entry to be added in AssociateExitWorkflow with new status
                    ServiceResponse<Status> revokeStatus = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.AssociateExit.ToString(), AssociateExitStatusCodesNew.RevokeRequested.ToString());

                    exitDetails.ResignationRecommendation = resignationRecommendation;
                    exitDetails.StatusId = revokeStatus.Item.StatusId;                   

                    var (hrmId, hrmEmail) = await GetHRMId();

                    AssociateExitWorkflow workflow = new AssociateExitWorkflow()
                    {
                        SubmittedBy = hrmId,
                        SubmittedTo = exitDetails.EmployeeId,
                        SubmittedDate = DateTime.Now,
                        WorkflowStatus = revokeStatus.Item.StatusId,
                        AssociateExitId = exitDetails.AssociateExitId,
                        Comments = null
                    };

                    int isUpdated = await m_EmployeeContext.SaveChangesAsync();

                    if (isUpdated > 0)
                    {
                        ServiceResponse<int> notification = await AssociateExitSendNotification(employeeId, Convert.ToInt32(NotificationType.WithdrawResignation), null, null, resignationRecommendation);

                        if (notification.IsSuccessful)
                        {
                            response.IsSuccessful = true;
                            response.Item = 1;
                            response.Message = "Notification Sent Successfully";
                            return response;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "Error occurred while sending email";
                            return response;
                        }
                    }
                    response.IsSuccessful = false;
                    response.Item = 0;
                    response.Message = "Error While Updating Database";
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Item = 0;
                response.Message = "Error Occured While Sending Notification";
                return response;
            }
        }
        #endregion

        #region Approve/RejectRevoke
        /// <summary>
        /// Accept or Reject ExitRevoke by Program Manager/DepartmentHead
        /// </summary>
        /// <param name="revokeRequest"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> ApproveOrRejectRevoke(RevokeRequest revokeRequest)
        {
            var response = new ServiceResponse<int>();
            ServiceResponse<Status> revokeStatus = new ServiceResponse<Status>();
            try
            {
                var exitDetails = await m_EmployeeContext.AssociateExit.FirstOrDefaultAsync(st => st.EmployeeId == revokeRequest.EmployeeId && st.IsActive == true);
                if (exitDetails == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"No employee existes with employeeId {revokeRequest.EmployeeId}";
                    return response;
                }

                var employee = await m_EmployeeContext.Employees.FirstOrDefaultAsync(st => st.EmployeeId == revokeRequest.EmployeeId && st.IsActive == true);
                if (employee == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = $"No employee found with employeeId {revokeRequest.EmployeeId}";
                    return response;
                }

                if (revokeRequest.SubmitType == "Approve")
                {
                    revokeStatus = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.AssociateExit.ToString(), AssociateExitStatusCodesNew.ResignationRevoked.ToString());
                    if (revokeStatus.Item == null)
                    {
                        response.Item = 0;
                        response.IsSuccessful = false;
                        response.Message = $"No Revoke Initiated Status found in ORG Service";
                        return response;
                    }

                    exitDetails.StatusId = revokeStatus.Item.StatusId;
                    exitDetails.Retained = true;
                    exitDetails.ResignationWithdrawn = true;
                }

                if (revokeRequest.SubmitType == "Reject")
                {
                    revokeStatus = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.AssociateExit.ToString(), AssociateExitStatusCodesNew.RevokeRejected.ToString());
                    if (revokeStatus.Item == null)
                    {
                        response.Item = 0;
                        response.IsSuccessful = false;
                        response.Message = $"No Revoke Initiated Status found in ORG Service";
                        return response;
                    }

                    exitDetails.Retained = false;
                    exitDetails.ResignationWithdrawn = false;
                }

                var revokeWorkflowDtls = await m_EmployeeContext.AssociateExitRevokeWorkflow
                                        .FirstOrDefaultAsync(x => x.AssociateExitId == exitDetails.AssociateExitId && x.IsActive == true);

                if (revokeWorkflowDtls != null)
                {
                    revokeWorkflowDtls.IsActive = false;
                }

                AssociateExitRevokeWorkflow revokeWorkflow = new AssociateExitRevokeWorkflow()
                {
                    AssociateExitId = exitDetails.AssociateExitId,
                    RevokeStatusId = revokeStatus.Item.StatusId,
                    RevokeComment = revokeRequest.RevokeReason,
                    IsActive = true
                };

                await m_EmployeeContext.AssociateExitRevokeWorkflow.AddAsync(revokeWorkflow);

                var (hrmId, hrmEmail) = await GetHRMId();

                AssociateExitWorkflow workflow = new AssociateExitWorkflow()
                {
                    SubmittedBy = hrmId,
                    SubmittedTo = exitDetails.EmployeeId,
                    SubmittedDate = DateTime.Now,
                    WorkflowStatus = revokeStatus.Item.StatusId,
                    AssociateExitId = exitDetails.AssociateExitId,
                    Comments = revokeRequest.Comment,
                    IsActive = true
                };

                await m_EmployeeContext.AssociateExitWorkflow.AddAsync(workflow);

                //Saving Details to database
                int created = await m_EmployeeContext.SaveChangesAsync();
                if (created > 0)
                {
                    ServiceResponse<int> notification = await AssociateExitSendNotification(exitDetails.EmployeeId,
                        revokeRequest.SubmitType == "Approve" ? Convert.ToInt32(NotificationType.ResignationRevoked) : Convert.ToInt32(NotificationType.RevokeRejected), null, null);
                    if (!notification.IsSuccessful)
                    {
                        response.Item = 0;
                        response.IsSuccessful = false;
                        response.Message = notification.Message;
                        return response;
                    }

                    //Code shouldn't be moved
                    //Deactivating the exit details in case of Approve
                    if (revokeRequest.SubmitType == "Approve")
                    {
                        exitDetails.IsActive = false;
                        await m_EmployeeContext.SaveChangesAsync();
                    }

                    response.Item = created;
                    response.IsSuccessful = true;
                    response.Message = revokeRequest.SubmitType == "Approve" ? "Resignation Revoked Successfully" : "Resignation Revoke Rejected Successfully";

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
                response.Message = $"Error Occured while Revoking Associate Resignation";
                return response;
            }
        }

        #endregion

        #region GetClearanceRemarks
        /// <summary>
        /// GetClearanceRemarks
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<ClearanceRequest>> GetClearanceRemarks(int employeeId)
        {
            var response = new ServiceResponse<ClearanceRequest>();

            try
            {
                int resignedStatus = Convert.ToInt32(AssociateExitStatusCodesNew.Resigned);
                var employeeDetails = m_EmployeeContext.AssociateExit.Where(pa => pa.EmployeeId == employeeId && ((pa.IsActive == true) || (pa.StatusId == resignedStatus && pa.IsActive == false))).FirstOrDefault();
                if (employeeDetails == null)
                {
                    response.Item = null;
                    response.IsSuccessful = false;
                    response.Message = $"No employee exit existes with employeeId {employeeId}";
                    return response;
                }
                int exitId = employeeDetails.AssociateExitId;
                var clearanceRemarks = m_EmployeeContext.Remarks.Where(st => st.AssociateExitId == exitId).ToList();
                ClearanceRequest clearanceRequest = new ClearanceRequest();
                if (clearanceRemarks == null)
                {
                    response.Item = clearanceRequest;
                    response.IsSuccessful = true;
                    response.Message = $"No record exists in Remarks Table for EmployeeId {employeeId}";
                    return response;
                }

                clearanceRequest.RemarksByHRA = clearanceRemarks.Where(st => st.RoleId == Convert.ToInt32(Roles.HRA)).Select(st => st.Comment).FirstOrDefault();
                clearanceRequest.RemarksByHRM = clearanceRemarks.Where(st => st.RoleId == Convert.ToInt32(Roles.HRM)).Select(st => st.Comment).FirstOrDefault();
                clearanceRequest.RemarksByOperationsHead = clearanceRemarks.Where(st => st.RoleId == Convert.ToInt32(Roles.OperationsHead)).Select(st => st.Comment).FirstOrDefault();
                clearanceRequest.employeeId = employeeId;
                response.Item = clearanceRequest;
                response.IsSuccessful = true;
                response.Message = $"Associate Clearance Remarks Fetched Successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Item = null;
                response.IsSuccessful = false;
                response.Message = $"Error Occured while Fetching Associate Clearance Remarks";
                return response;
            }
        }
        #endregion

        #region AssociateExitSendNotification
        /// <summary>
        /// AssociateExitSendNotification
        /// </summary>
        /// <param name = "employeeId" > employeeId </ param >
        /// <param name="notificationType">notificationType</param>
        /// <param name="departmentId">departmentId</param>
        /// <param name="projectId">projectId</param>
        /// <optionalparam name="resignationRecommendation">resignationRecommendation</param>
        /// < returns ></ returns >
        public async Task<ServiceResponse<int>> AssociateExitSendNotification(int employeeId, int notificationType, int? departmentId, int? projectId, [Optional] string resignationRecommendation, [Optional] string exitFeedback)
        {
            #region Variables
            ServiceResponse<int> response = new ServiceResponse<int>();
            string empStatus = string.Empty;
            string empDesignation = string.Empty;
            string empDepartment = string.Empty;
            string associateMail = string.Empty;
            string tlEmail = string.Empty;
            string ktAssociateMail = string.Empty;
            string programManagerMail = string.Empty;
            string departmentCodes = string.Empty;
            string projectName = string.Empty;
            string reportingManager = string.Empty;
            string programManager = string.Empty;
            string critical = string.Empty;
            string clientName = string.Empty;

            string hraEmail = string.Empty;
            string higherOfficialEmail = string.Empty;
            string hrDLEmailAddress = string.Empty;
            string feedbackNotificationEmail = string.Empty;

            string fromEmailAddress = string.Empty;
            string toEmailAddress = string.Empty;
            string ccEmailAddress = string.Empty;
            string mailSubject = string.Empty;
            string currentDirectory = Directory.GetCurrentDirectory();
            string templateFilePath = null;
            string templateBody = null;
            string departmentHeadEmail = string.Empty;                                  

            NotificationDetail financeMailObj = null;
            NotificationDetail feedbackMailObj = null;
            #endregion

            try
            {
                m_Logger.LogInformation("Calling \"AssociateExitSendNotification\" method in AssociateExit Service");

                bool isSendEmail = m_EmailConfigurations.SendEmail;

                if (!isSendEmail)
                {
                    m_Logger.LogInformation("SendEmail configured to false. Returning back without sending any notification");

                    response.Item = 0;
                    response.IsSuccessful = true;
                    response.Message = "Notification not sent as per SendEmail configuration";
                    return response;
                }

                #region ReadConfigSection

                fromEmailAddress = m_EmailConfigurations.FromEmail;
                higherOfficialEmail = m_EmailConfigurations.HigherOfficialEmail;
                feedbackNotificationEmail = m_EmailConfigurations.FeedbackNotificationEmail;
                departmentCodes = m_EmailConfigurations.DeptCodes;
                departmentHeadEmail = m_EmailConfigurations.DepartmentHeadEmail;

                #endregion

                #region GettingMailDetails                

                //Get Program Manager email address
                Entities.Employee employee = m_EmployeeContext.Employees.Where(e => e.EmployeeId == employeeId && e.IsActive == true).FirstOrDefault();
                if (employee == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "Employee Not Found";
                    return response;
                }

                var exitDetails = m_EmployeeContext.AssociateExit.Where(e => e.EmployeeId == employeeId && e.IsActive == true).FirstOrDefault();
                if (exitDetails == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "Employee Exit Details Not Found";
                    return response;
                }

                associateMail = employee.WorkEmailAddress;
                if (associateMail == null)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "Associate Email Details Not Found";
                    return response;
                }

                var exitWorkFlowDetails = await m_EmployeeContext.AssociateExitWorkflow.FirstOrDefaultAsync(x => x.AssociateExitId == exitDetails.AssociateExitId && x.WorkflowStatus == Convert.ToInt32(AssociateExitStatusCodesNew.ResignationApproved));
                if (exitWorkFlowDetails?.SubmittedTo > 0)
                {
                    hraEmail = await GetWorkMailbyEmpId(exitWorkFlowDetails.SubmittedTo);
                }

                var designationDetails = m_OrgService.GetDesignationById(employee.DesignationId ?? 0);
                var empDepartmentDetails = m_OrgService.GetDepartmentById(employee.DepartmentId ?? 0);

                await Task.WhenAll(designationDetails, empDepartmentDetails).ConfigureAwait(false);

                var designationsResult = designationDetails.GetAwaiter().GetResult();
                var empDepartmentsResult = empDepartmentDetails.GetAwaiter().GetResult();

                if (!designationsResult.IsSuccessful)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "Designation Details Not Found";
                    return response;
                }

                empDesignation = designationsResult.Item.DesignationName;

                if (!empDepartmentsResult.IsSuccessful)
                {
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "Department Details Not Found";
                    return response;
                }

                empDepartment = empDepartmentsResult.Item.DepartmentCode;

                //HRM Mail
                var (hrmId, hrmEmail) = await GetHRMId();
                empStatus = await GetStatusDesc(notificationType, exitDetails.AssociateExitId, exitDetails.StatusId);

                ServiceResponse<AssociateAllocation> allocation = new ServiceResponse<AssociateAllocation>();
                //Getting Team Lead Email
                if (empDepartment == "Delivery")
                {
                    allocation = await m_ProjectService.GetAllocationById(exitDetails.AssociateAllocationId??0);

                    if (allocation.Item != null && allocation.IsSuccessful)
                    {
                        var associateAllocation = allocation.Item;
                        tlEmail = await GetWorkMailbyEmpId(associateAllocation.LeadId ?? 0);
                        programManagerMail = await GetWorkMailbyEmpId(associateAllocation.ProgramManagerId ?? 0);
                        reportingManager = await GetManagerNameByEmpId(associateAllocation.ReportingManagerId ?? 0);
                        //Project details
                        var projectDetails = (await m_ProjectService.GetProjectByID((int)associateAllocation.ProjectId)).Item;
                        //Client details
                        var ClientDetails = (await m_OrgService.GetClientById((int)projectDetails.ClientId)).Item;
                        projectName = projectDetails.ProjectName;
                        critical = associateAllocation.IsCritical == true ? "Yes" : "No";
                        clientName = ClientDetails.ClientName;
                        programManager = await GetManagerNameByEmpId(associateAllocation.ProgramManagerId ?? 0);

                    }
                }
                else
                {
                    programManagerMail = await GetWorkMailbyEmpId(employee.ReportingManager ?? 0); //TODO: validate in non-delivery dept and for program manager and above associates
                    tlEmail = programManagerMail;
                    reportingManager = await GetManagerNameByEmpId(employee.ReportingManager ?? 0);
                    programManager = reportingManager;
                }

                //Approval Dept Details
                var departmentDLs = await m_OrgService.GetAllDepartmentsWithDLs();
                if (departmentDLs.IsSuccessful && departmentDLs.Items != null)
                {
                    hrDLEmailAddress = departmentDLs.Items?.FirstOrDefault(x => x.DepartmentCode == "HR")?.DepartmentDLAddress;
                }
                #endregion

                #region EmailConfig                

                templateFilePath = Utility.GetNotificationTemplatePath(currentDirectory, NotificationTemplatePaths.subDirectories_AssociateExit_ResignationStatus);
                templateBody = GetNotificationTemplate(templateFilePath);
                //Replace email content with current employee details
                templateBody = templateBody.Replace("{AssociateName}", $"{employee.FirstName} {employee.LastName}")
                                   .Replace("{Designation}", empDesignation)
                                   .Replace("{Department}", empDepartment)
                                   .Replace("{AssociateCode}", employee.EmployeeCode)
                                   .Replace("{LastWorkingDate}", exitDetails.ExitDate.Value.ToShortDateString())
                                   .Replace("{ResignationDate}", exitDetails.ResignationDate.Value.ToShortDateString())
                                   .Replace("{ResignationStatus}", empStatus)
                                   .Replace("{ReportingManager}", reportingManager)
                                   .Replace("{ProgramManager}", programManager);
                 templateBody = projectName != "" ? templateBody.Replace("{ProjectName}", "<td>" + projectName + "</td>") : templateBody.Replace("{ProjectName}", "<td>NA</td>");
                 templateBody = clientName != "" ? templateBody.Replace("{ClientName}", "<td>" + clientName + "</td>") : templateBody.Replace("{ClientName}", "<td>NA</td>");
                //}

                #endregion

                #region ResignationSubmitNotification

                if (notificationType == Convert.ToInt32(NotificationType.ResignationSubmitted))
                {
                    toEmailAddress = $"{hrmEmail};{programManagerMail};{tlEmail}";
                    ccEmailAddress = $"{hrDLEmailAddress};{associateMail};{departmentHeadEmail}";
                    mailSubject = m_exitMailSubjects.ResignationSubmittedNotification.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region ResignationReviewedNotification

                if (notificationType == Convert.ToInt32(NotificationType.ResignationReviewed))
                {
                    toEmailAddress = $"{hrmEmail}";
                    ccEmailAddress = $"{programManagerMail};{tlEmail};{hrDLEmailAddress};{associateMail}";
                    mailSubject = m_exitMailSubjects.ResignationReviewedByPMNotification.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region ResignationApprovedNotification

                if (notificationType == Convert.ToInt32(NotificationType.ResignationApproved))
                {
                    string financeHeadMail = string.Empty;
                    string tagHeadMail = string.Empty;

                    templateBody = templateBody.Replace("{ActivitiesInitiationToHRA}", $"<p>Please initiate the exit activities.</p>");

                    if (allocation.Item != null && allocation.IsSuccessful)
                        templateBody = templateBody.Replace("{KTInitiationToTL}", $"<p>Please initiate the KT plan for the associate or ignore if not required.</p>");

                    if (departmentDLs.IsSuccessful && departmentDLs.Items != null)
                    {
                        var financeDeptDtls = departmentCodes.Contains("FD") ? departmentDLs.Items.FirstOrDefault(x => x.DepartmentCode == "FD") : null;
                        var tagDeptDtls = departmentCodes.Contains("HR TAG") ? departmentDLs.Items.FirstOrDefault(x => x.DepartmentCode == "HR TAG") : null;

                        if (financeDeptDtls != null)
                        {
                            financeHeadMail = await GetWorkMailbyEmpId(financeDeptDtls.DepartmentHeadId ?? 0);
                        }

                        if (tagDeptDtls != null)
                        {
                            tagHeadMail = await GetWorkMailbyEmpId(tagDeptDtls.DepartmentHeadId ?? 0);
                        }
                    }

                    toEmailAddress = $"{programManagerMail};{tlEmail};{financeHeadMail};{tagHeadMail}";
                    ccEmailAddress = $"{associateMail};{hrmEmail};{hrDLEmailAddress};{higherOfficialEmail}";
                    mailSubject = m_exitMailSubjects.ResignationApprovedNotification.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);

                }

                #endregion

                #region KTPlanSubmittedNotification

                if (notificationType == Convert.ToInt32(NotificationType.KTPlanSubmitted))
                {

                    templateFilePath = Utility.GetNotificationTemplatePath(currentDirectory,NotificationTemplatePaths.subDirectories_AssociateExit_GenericMail);
                    templateBody = GetNotificationTemplate(templateFilePath);
                    ktAssociateMail = await GetKTAssociateEmailAsync(exitDetails.AssociateExitId);

                    //Replace email content with current employee details
                    templateBody = templateBody.Replace("{EmailBody}", "KT plan is submitted by Associate.");
                    toEmailAddress = $"{tlEmail}";
                    ccEmailAddress = $"{programManagerMail};{ktAssociateMail};{associateMail}";
                    mailSubject = m_exitMailSubjects.KTPlanSubmittedNotification.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region KTPlanInProgressNotification

                if (notificationType == Convert.ToInt32(NotificationType.KTPlanInProgress))
                {                    
                    templateFilePath = Utility.GetNotificationTemplatePath(currentDirectory, NotificationTemplatePaths.subDirectories_AssociateExit_GenericMail);
                    templateBody = GetNotificationTemplate(templateFilePath);
                    ktAssociateMail = await GetKTAssociateEmailAsync(exitDetails.AssociateExitId);

                    //Replace email content with current employee details
                    templateBody = templateBody.Replace("{EmailBody}", "Associate's KT plan is defined and submitted by TL.");
                    toEmailAddress = $"{associateMail}";
                    ccEmailAddress = $"{programManagerMail};{tlEmail};{ktAssociateMail}";
                    mailSubject = m_exitMailSubjects.KTPlanInProgressNotification.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region KTPlanCompletedNotification

                if (notificationType == Convert.ToInt32(NotificationType.KTPlanCompleted))
                {
                    
                    templateFilePath = Utility.GetNotificationTemplatePath(currentDirectory, NotificationTemplatePaths.subDirectories_AssociateExit_GenericMail);
                    templateBody = GetNotificationTemplate(templateFilePath);
                    ktAssociateMail = await GetKTAssociateEmailAsync(exitDetails.AssociateExitId);

                    //Replace email content with current employee details
                    templateBody = templateBody.Replace("{EmailBody}", "Associate's KT plan is verified and completed by TL.");

                    toEmailAddress = $"{programManagerMail}";
                    ccEmailAddress = $"{tlEmail};{ktAssociateMail};{hrDLEmailAddress};{associateMail}";
                    mailSubject = m_exitMailSubjects.KTPlanCompletedNotification.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region ActivitiesInProgressNotification

                if (notificationType == Convert.ToInt32(NotificationType.ActivitiesInProgress))
                {
                    string deptDLEmail = string.Empty;

                    var activityDeptDetails = await m_EmployeeContext.AssociateExitActivity.Where(w => w.AssociateExitId == exitDetails.AssociateExitId && w.IsActive == true).Select(x => x).ToListAsync();
                    if (activityDeptDetails == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Email To cannot be blank";
                        return response;
                    }

                    foreach (AssociateExitActivity dept in activityDeptDetails)
                    {
                        deptDLEmail += $"{departmentDLs.Items?.FirstOrDefault(x => x.DepartmentId == dept.DepartmentId)?.DepartmentDLAddress};";
                    }

                    // HR Talent Acquisition 
                    var hrTagTeamEmail = departmentDLs.Items?.FirstOrDefault(x => x.DepartmentCode == "HR TAG")?.DepartmentDLAddress;

                    var financeDLEmail = departmentDLs.Items?.FirstOrDefault(x => x.DepartmentCode == "FD")?.DepartmentDLAddress;

                    toEmailAddress = $"{deptDLEmail};{hrTagTeamEmail}";
                    ccEmailAddress = $"{hraEmail};{associateMail}";
                    mailSubject = m_exitMailSubjects.ActivitiesInProgressNotification.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                    templateBody = templateBody.Replace("{loginURL}", "<p>Please login to <a href='https://hrms.senecaglobal.com'>https://hrms.senecaglobal.com</a> and complete assigned activities.</p>");

                    // mail body html template for IT Returns                    
                    var financeTemplatePath = Utility.GetNotificationTemplatePath(currentDirectory, NotificationTemplatePaths.subDirectories_ITReturnsNotification);
                    var financeTemplateBody = GetNotificationTemplate(financeTemplatePath);
                    financeTemplateBody = financeTemplateBody.Replace("{Associate}", $"{employee.FirstName} {employee.LastName}");

                    //Send an email from finance team to an associate on it submission
                    financeMailObj = new NotificationDetail
                    {
                        FromEmail = fromEmailAddress,
                        ToEmail = associateMail,
                        CcEmail = financeDLEmail,
                        Subject = m_exitMailSubjects.ITReturnNotification,
                        EmailBody = financeTemplateBody
                    };
                    //adding files to mail attachment
                    foreach (string file in Directory.EnumerateFiles(currentDirectory + "/NotificationTemplate/Finance"))
                    {
                        financeMailObj.Attachments.Add(file);
                    }                                        
                }

                #endregion

                #region ActivitiesCompletedByDepartmentNotification

                if (notificationType == Convert.ToInt32(NotificationType.Completed))
                {
                    DepartmentWithDLAddress completedDept = null;

                    if (departmentId.HasValue)
                    {
                        completedDept = departmentDLs.Items?.FirstOrDefault(x => x.DepartmentId == departmentId.Value);
                    }
                  
                    templateFilePath = Utility.GetNotificationTemplatePath(currentDirectory, NotificationTemplatePaths.subDirectories_AssociateExit_GenericMail);
                    templateBody = GetNotificationTemplate(templateFilePath);

                    //Replace email content with current employee details
                    templateBody = templateBody.Replace("{EmailBody}", "Associate's exit activities are completed by " + completedDept?.DepartmentDescription + " department.");

                    toEmailAddress = $"{hrDLEmailAddress}";
                    ccEmailAddress = $"{hrmEmail};{hraEmail};{programManagerMail};{associateMail};{completedDept?.DepartmentDLAddress}";
                    mailSubject = m_exitMailSubjects.ActivitiesCompletedByDepartmentNotification.Replace("{EmployeeFirstName}", employee.FirstName)
                                                                                                               .Replace("{EmployeeLastName}", employee.LastName)
                                                                                                               .Replace("{Dept}", completedDept?.DepartmentDescription);
                }

                #endregion

                #region ClearanceGivenNotification

                if (notificationType == Convert.ToInt32(NotificationType.ClearanceGiven))
                {
                    //string mailPath = m_configuration.GetValue<string>("DepartmentHead");
                    //notificationDetail.ToEmail = $"{notificationDetail.ToEmail};{associateMail};{mailPath}";   //to-do. Replace with above line in production
                    mailSubject = m_exitMailSubjects.ClearanceGivenNotification.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region ResignedNotification

                if (notificationType == Convert.ToInt32(NotificationType.Resigned))
                {
                    string deptDLEmail = string.Empty;
                    var activityDeptDetails = await m_EmployeeContext.AssociateExitActivity.Where(w => w.AssociateExitId == exitDetails.AssociateExitId && w.IsActive == true).Select(x => x).ToListAsync();
                    if (activityDeptDetails == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Email To cannot be blank";
                        return response;
                    }

                    foreach (AssociateExitActivity dept in activityDeptDetails)
                    {
                        deptDLEmail += $"{departmentDLs.Items?.FirstOrDefault(x => x.DepartmentId == dept.DepartmentId)?.DepartmentDLAddress};";
                    }

                    toEmailAddress = $"{hrmEmail};{hraEmail};{programManagerMail};{deptDLEmail}";
                    ccEmailAddress = $"{associateMail}";
                    mailSubject = m_exitMailSubjects.ResignedNotification.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region FeedbackGivenNotification

                if (notificationType == Convert.ToInt32(NotificationType.ExitInterviewCompleted))
                {
                    toEmailAddress = $"{hrmEmail}";
                    ccEmailAddress = $"{programManagerMail};{hrDLEmailAddress}";
                    mailSubject = m_exitMailSubjects.FeedbackGivenNotification.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);

                    //Sending exit feedback to Higher Official
                    if (!string.IsNullOrEmpty(exitFeedback))
                    {                        
                        string secondTemplatePath = Utility.GetNotificationTemplatePath(currentDirectory, NotificationTemplatePaths.subDirectories_AssociateExit_GenericMail);
                        string secondTemplateBody = GetNotificationTemplate(secondTemplatePath);

                        feedbackMailObj = new NotificationDetail
                        {
                            ToEmail = feedbackNotificationEmail,
                            FromEmail = fromEmailAddress,
                            Subject = mailSubject,
                            EmailBody = secondTemplateBody.Replace("{EmailBody}", $"<p>Hello All,<br><br> Greetings of the day! <br><br> Please find the exit feedback of { employee.FirstName }.<br><br> {StripHTML(exitFeedback)}")
                        };
                    }
                }

                #endregion

                #region ResignationRevokeInitiated

                if (notificationType == Convert.ToInt32(NotificationType.RevokeInitiated))
                {
                    toEmailAddress = $"{hrmEmail}";
                    ccEmailAddress = $"{programManagerMail};{tlEmail};{associateMail};{hrDLEmailAddress}";
                    mailSubject = m_exitMailSubjects.ResignationRevokeInitiated.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region ResignationRevokedNotification

                if (notificationType == Convert.ToInt32(NotificationType.ResignationRevoked))
                {
                    toEmailAddress = $"{associateMail}";
                    ccEmailAddress = $"{hrmEmail};{programManagerMail};{tlEmail};{hrDLEmailAddress};{higherOfficialEmail}";
                    mailSubject = m_exitMailSubjects.ResignationRevokedNotification.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region ResignationRevokeRejected

                if (notificationType == Convert.ToInt32(NotificationType.RevokeRejected))
                {
                    toEmailAddress = $"{associateMail}";
                    ccEmailAddress = $"{hrmEmail};{programManagerMail};{tlEmail};{hrDLEmailAddress};{higherOfficialEmail}";
                    mailSubject = m_exitMailSubjects.ResignationRevokeRejected.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region ExitDateUpdatedNotification

                if (notificationType == Convert.ToInt32(NotificationType.ExitDateUpdated))
                {
                    
                    templateFilePath = Utility.GetNotificationTemplatePath(currentDirectory, NotificationTemplatePaths.subDirectories_AssociateExit_GenericMail);
                    templateBody = GetNotificationTemplate(templateFilePath);

                    //Replace email content with current employee details
                    templateBody = templateBody.Replace("{EmailBody}", $"Associate's last working date is updated to {exitDetails.ExitDate.Value.ToShortDateString()}");
                    toEmailAddress = $"{programManagerMail};{hraEmail};{associateMail}";
                    ccEmailAddress = $"{hrmEmail};{hrDLEmailAddress}";
                    mailSubject = m_exitMailSubjects.ExitDateUpdated.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region ReviewReminderNotification

                if (notificationType == Convert.ToInt32(NotificationType.ReviewReminder))
                {                    
                    templateFilePath = Utility.GetNotificationTemplatePath(currentDirectory, NotificationTemplatePaths.subDirectories_AssociateExit_GenericMail);
                    templateBody = GetNotificationTemplate(templateFilePath);

                    //Replace email content with current employee details
                    templateBody = templateBody.Replace("{EmailBody}", $"Please review {employee.FirstName} {employee.LastName}'s resignation.");
                    toEmailAddress = $"{programManagerMail}";
                    ccEmailAddress = $"{hrmEmail};{hrDLEmailAddress}";
                    mailSubject = m_exitMailSubjects.ReviewReminder.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region RequestForWithdrawResignation

                // Dead-Code - As per new flow, revoke will happen from Associate
                if (notificationType == Convert.ToInt32(NotificationType.WithdrawResignation))
                {
                    templateFilePath = Utility.GetNotificationTemplatePath(currentDirectory, NotificationTemplatePaths.subDirectories_AssociateExit_ResignationStatusWithReason); 
                    templateBody = GetNotificationTemplate(templateFilePath);

                    //Replace email content with current employee details
                    templateBody = templateBody.Replace("{AssociateName}", $"{employee.FirstName} {employee.LastName}")
                                       .Replace("{Designation}", empDesignation)
                                       .Replace("{Department}", empDepartment)
                                       .Replace("{AssociateCode}", employee.EmployeeCode)
                                       .Replace("{LastWorkingDate}", exitDetails.ExitDate.Value.ToShortDateString())
                                       .Replace("{ResignationStatus}", empStatus);

                    if (!string.IsNullOrEmpty(resignationRecommendation) && !string.IsNullOrWhiteSpace(resignationRecommendation))
                    {
                        templateBody = templateBody.Replace("{ResignationRecommendation}", $"<tr><td><strong>Resignation Recommendation</strong></td><td>{resignationRecommendation}</td></tr>");
                    }
                    else
                    {
                        templateBody = templateBody.Replace("{ResignationRecommendation}", "");
                    }

                    toEmailAddress = $"{associateMail}";
                    ccEmailAddress = $"{hrmEmail};{programManagerMail};{hrDLEmailAddress}";
                    mailSubject = m_exitMailSubjects.RequestForWithdrawResignation.Replace("{EmployeeFirstName}", employee.FirstName).Replace("{EmployeeLastName}", employee.LastName);
                }

                #endregion

                #region RemoveUnNecessaryPlaceHolders

                //These fields will be replaced in respective NotificationType section. If these placeholders survived till this point, which means they can be cleared from mail body.
                if (templateBody.Contains("{ActivitiesInitiationToHRA}") || templateBody.Contains("{KTInitiationToTL}") || templateBody.Contains("{loginURL}") || templateBody.Contains("{ProjectNameLable}") || templateBody.Contains("{CriticalLable}") || templateBody.Contains("{ClientNameLable}") || templateBody.Contains("{ProjectName}") || templateBody.Contains("{Critical}") || templateBody.Contains("{ClientName}"))
                {
                    templateBody = templateBody.Replace("{ActivitiesInitiationToHRA}", "").Replace("{KTInitiationToTL}", "").Replace("{loginURL}", "").Replace("{ProjectName}", "").Replace("{Critical}", "").Replace("{ProjectNameLable}", "").Replace("{CriticalLable}", "").Replace("{ClientNameLable}", "").Replace("{ClientName}", "");
                }

                #endregion

                // Validate email Notification details
                if (string.IsNullOrEmpty(fromEmailAddress))
                {
                    response.IsSuccessful = false;
                    response.Message = "Email From cannot be blank";
                    return response;
                }

                // Validate email Notification details
                if (string.IsNullOrEmpty(toEmailAddress))
                {
                    response.IsSuccessful = false;
                    response.Message = "Email To cannot be blank";
                    return response;
                }

                NotificationDetail mailObj = new NotificationDetail
                {
                    FromEmail = fromEmailAddress,
                    ToEmail = toEmailAddress,
                    CcEmail = string.IsNullOrEmpty(ccEmailAddress) ? string.Empty : ccEmailAddress,
                    Subject = mailSubject,
                    EmailBody = templateBody
                };

                var emailStatus = await m_OrgService.SendEmail(mailObj);

                //sending mail to associate for Income Tax forms only when Activites are initiated
                if (emailStatus.IsSuccessful && financeMailObj != null)
                {
                    emailStatus = await m_OrgService.SendEmail(financeMailObj);
                }

                //sending exit interview feedback to higher officials over mail based on checkbox
                if (feedbackMailObj != null)
                {
                    _ = await m_OrgService.SendEmail(feedbackMailObj);
                }

                if (!emailStatus.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = $"Error occurred while sending email. Message: {emailStatus.Message}";
                    return response;
                }

                response.IsSuccessful = true;
                response.Item = 1;
                return response;
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in AssociateExitSendNotification.");
                m_Logger.LogError(ex, ex.GetBaseException().Message);
                m_Logger.LogError(ex, ex.GetBaseException().StackTrace);

                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Exception occured in AssociateExitSendNotification."
                };
            }
        }
        #endregion

        #region AssociateExitDailyNotification
        /// <summary>
        /// AssociateExitDailyNotification
        /// </summary>
        /// <param name="departmentId">departmentId</param>
        /// < returns ></ returns >
        public async Task<ServiceResponse<bool>> AssociateExitDailyNotification(int departmentId)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                string deptDLEmail = string.Empty;
                NotificationDetail notificationDetail = new NotificationDetail();
                List<Entities.Employee> employees = null;

                var activitiesInProgress = Convert.ToInt32(AssociateExitStatusCodesNew.ResignationInProgress);
                var inProgressStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.DepartmentActivityInProgress);
                var completedStatusId = Convert.ToInt32(AssociateExitStatusCodesNew.ExitInterviewCompleted);

                employees = (from emp in m_EmployeeContext.Employees
                             join associateExit in m_EmployeeContext.AssociateExit
                             on emp.EmployeeId equals associateExit.EmployeeId
                             join activity in m_EmployeeContext.AssociateExitActivity on associateExit.AssociateExitId equals activity.AssociateExitId
                             where ((associateExit.StatusId == activitiesInProgress || associateExit.StatusId == completedStatusId) &&
                             (activity.StatusId == inProgressStatusId))
                             && activity.DepartmentId == departmentId
                             orderby associateExit.ExitDate
                             select emp).Distinct().ToList();


                var deptDLDtl = await m_OrgService.GetDepartmentDLByDeptId(departmentId);
                if (deptDLDtl.IsSuccessful && deptDLDtl.Item != null)
                    deptDLEmail = deptDLDtl.Item.DLEmailAdress;


                employees?.ForEach(async employee =>
                {
                    notificationDetail.ToEmail = $"{deptDLEmail};";
                    notificationDetail.FromEmail = m_EmailConfigurations.FromEmail;

                    notificationDetail.Subject = m_exitMailSubjects.ActivitiesInProgressNotification.Replace("{EmployeeFirstName}", employee.FirstName)
                                                                                                    .Replace("{EmployeeLastName}", employee.LastName);
                    notificationDetail.EmailBody = $"<p>Hello Team,<br><br> Please complete the Inprogress Exit Activities for the associate.<br><br> Thank you!";

                    var emailStatus = await m_OrgService.SendEmail(notificationDetail);

                });

                response.IsSuccessful = true;
                response.Item = true;
                return response;
            }
            catch
            {
                response.IsSuccessful = false;
                response.Item = false;
                return response;
            }
        }
        #endregion

        #region AssociateExitWFStatus
        /// <summary>
        /// AssociateExitWFStatus
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<AssociateExitWFStatus>> AssociateExitWFStatus(int employeeId)
        {
            var response = new ServiceResponse<AssociateExitWFStatus>();
            AssociateExitWFStatus exitWFStatus = new AssociateExitWFStatus();
            ResignationSubStatus ktPlanSubStatus;
            int associateExitId;
            Status statusItem;

            try
            {
                var associateExit = await m_EmployeeContext.AssociateExit.Where(pa => pa.EmployeeId == employeeId && pa.IsActive == true).FirstOrDefaultAsync();
                //ServiceListResponse<Status> exitStatuses = await m_OrgService.GetStatusesByCategoryName(CategoryMaster.AssociateExit.ToString());

                if (associateExit != null)
                {
                    associateExitId = associateExit.AssociateExitId;

                    var exitStatusMaster = await m_OrgService.GetStatusesByCategoryName(CategoryMaster.AssociateExit.ToString());
                    statusItem = exitStatusMaster.Items.Where(x => x.StatusId == associateExit.StatusId).Select(y => y).FirstOrDefault();

                    exitWFStatus.AssociateExitId = associateExit.AssociateExitId;
                    exitWFStatus.EmployeeId = associateExit.EmployeeId;
                    exitWFStatus.AssociateExitStatusCode = statusItem.StatusCode;
                    exitWFStatus.AssociateExitStatusDesc = statusItem.StatusDescription;

                    //var deptActivities = await m_EmployeeContext.AssociateExitActivity.Where(x => x.AssociateExitId == associateExitId).ToListAsync();
                    if (associateExit.StatusId == Convert.ToInt32(AssociateExitStatusCodesNew.ResignationInProgress))
                    {
                        List<ActivityStatus> activityStatuses = new List<ActivityStatus>();

                        var deptActivitesResult = await m_EmployeeContext.AssociateExitActivity.Where(x => x.AssociateExitId == associateExitId).ToListAsync();
                        var exitInterviewResult = await m_EmployeeContext.AssociateExitInterview.Where(x => x.AssociateExitId == associateExitId).FirstOrDefaultAsync();

                        if (deptActivitesResult?.Count > 0)
                        {
                            foreach (var activity in deptActivitesResult)
                            {
                                statusItem = exitStatusMaster.Items.Where(x => x.StatusId == activity.StatusId).Select(y => y).FirstOrDefault();

                                activityStatuses.Add(new ActivityStatus
                                {
                                    DepartmentId = activity.DepartmentId,
                                    ActivityStatusCode = statusItem.StatusCode,
                                    ActivityStatusDesc = statusItem.StatusDescription,
                                    IsCompleted = activity.StatusId == Convert.ToInt32(AssociateExitStatusCodesNew.DepartmentActivityCompleted)
                                });
                            }
                        }
                        else
                        {
                            var activites = await m_OrgService.GetExitActivitiesByDepartment();
                            var departments = activites?.Items.Select(st => st.DepartmentId).Distinct().ToList();

                            if (departments != null)
                            {
                                foreach (var departmentId in departments)
                                {
                                    activityStatuses.Add(new ActivityStatus
                                    {
                                        DepartmentId = departmentId,
                                        ActivityStatusCode = "ActivityNotYetStarted",
                                        ActivityStatusDesc = "Activity Not Yet Started",
                                        IsCompleted = false
                                    });
                                }
                            }
                        }

                        exitWFStatus.ActivitiesSubStatus = activityStatuses;

                        //Exit Interview
                        //var exitInterview = await m_EmployeeContext.AssociateExitInterview.Where(x => x.AssociateExitId == associateExitId).FirstOrDefaultAsync();
                        if (exitInterviewResult != null && exitInterviewResult.AssociateExitInterviewId != 0)
                        {
                            statusItem = exitStatusMaster.Items.Where(x => x.StatusId == Convert.ToInt32(AssociateExitStatusCodesNew.ExitInterviewCompleted)).Select(y => y).FirstOrDefault();
                            ResignationSubStatus interviewSubStatus = new ResignationSubStatus
                            {
                                IsCompleted = true,
                                SubStatusCode = statusItem.StatusCode,
                                SubStatusDesc = statusItem.StatusDescription
                            };

                            exitWFStatus.ExitInterviewSubStatus = interviewSubStatus;
                        }

                        //KT Plan
                        if (associateExit.TransitionRequired.HasValue && associateExit.TransitionRequired.Value)
                        {
                            var transistionPlan = await m_EmployeeContext.TransitionPlan.Where(x => x.AssociateExitId == associateExitId).FirstOrDefaultAsync();

                            if (transistionPlan != null && transistionPlan.StatusId > 0)
                            {
                                statusItem = exitStatusMaster.Items.Where(x => x.StatusId == transistionPlan.StatusId).Select(y => y).FirstOrDefault();
                                ktPlanSubStatus = new ResignationSubStatus
                                {
                                    SubStatusCode = statusItem.StatusCode,
                                    SubStatusDesc = statusItem.StatusDescription,
                                    IsCompleted = transistionPlan.StatusId == Convert.ToInt32(AssociateExitStatusCodesNew.KTPlanCompleted)
                                };
                            }
                            else
                            {
                                ktPlanSubStatus = new ResignationSubStatus
                                {
                                    SubStatusCode = "KTPlanNotDefined",
                                    SubStatusDesc = "KT Plan Required but not yet Defined",
                                    IsCompleted = false
                                };
                            }

                            exitWFStatus.KTPlanSubStatus = ktPlanSubStatus;
                        }
                        else
                        {
                            ktPlanSubStatus = new ResignationSubStatus
                            {
                                SubStatusCode = "KTPlanNotRequired",
                                SubStatusDesc = "KT Plan not Required",
                                IsCompleted = true
                            };

                            exitWFStatus.KTPlanSubStatus = ktPlanSubStatus;
                        }
                    }

                    response.Item = exitWFStatus;
                    response.IsSuccessful = true;
                    response.Message = "Associate Exit Status and Sub-Status fetched successfully";
                }
                else
                {
                    response.Item = null;
                    response.IsSuccessful = false;
                    response.Message = "Associate Exit not found";
                }
            }
            catch (Exception e)
            {
                response.Item = null;
                response.IsSuccessful = false;
                response.Message = $"Error Occured while Fetching Associate Exit Status and Sub-Status";
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> AssociateClearanceStatus(int employeeId)
        {
            var response = new ServiceResponse<bool>();
            bool validClearance = false;

            try
            {
                ServiceResponse<AssociateExitWFStatus> exitWFStatusResponse = await AssociateExitWFStatus(employeeId);

                if (exitWFStatusResponse.IsSuccessful)
                {
                    AssociateExitWFStatus exitWFStatus = exitWFStatusResponse.Item;

                    if (exitWFStatus.ActivitiesSubStatus != null && exitWFStatus.ExitInterviewSubStatus != null && exitWFStatus.KTPlanSubStatus != null &&
                        exitWFStatus.ActivitiesSubStatus.All(x => x.IsCompleted) && exitWFStatus.ExitInterviewSubStatus.IsCompleted && exitWFStatus.KTPlanSubStatus.IsCompleted)
                    {
                        validClearance = true;
                    }
                }

                response.Item = validClearance;
                response.IsSuccessful = validClearance;
                response.Message = validClearance ? "Associate Clearance Status fetched successfully" : exitWFStatusResponse.Message;
            }
            catch (Exception)
            {
                response.Item = false;
                response.IsSuccessful = false;
                response.Message = $"Error Occured while Fetching Associate Exit Clearance Status";
            }

            return response;
        }

        #endregion

        #region Private Methods

        private async Task<ServiceResponse<AssociateAllocation>> GetManagersInfo(int? associateAllocationId, Entities.Employee employeeInfo)
        {
            var isDeliveryDepartment = false;
            ServiceResponse<AssociateAllocation> response = new ServiceResponse<AssociateAllocation>();
            try
            {
                AssociateAllocation associateAllocation = null;
                ServiceResponse<Department> department = (await m_OrgService.GetDepartmentById(employeeInfo.DepartmentId??0));
                if (!department.IsSuccessful)
                {
                    response.IsSuccessful = true;
                    response.Message = "Error occured while fetching departmentdetails";
                    return response;
                }
                 isDeliveryDepartment = department.Item.DepartmentCode.ToLowerInvariant() == "delivery" ? true : false;

                    if (isDeliveryDepartment)
                    {
                    // associateAllocationId null when not resigned -- on initial resignation.
                  
                    if (associateAllocationId == null)
                        {
                            var allocation = (await m_ProjectService.GetAssociateAllocationsByEmployeeId(employeeInfo.EmployeeId));
                            if (!allocation.IsSuccessful)
                            {
                                response.IsSuccessful = false;
                                response.Message = "Error occured while fetching allocation details";
                                return response;
                            }
                        associateAllocation = allocation.Items.Where(x => x.IsPrimary == true).First();                           

                        }
                        else
                        {
                        // After resignation 
                        associateAllocation = (await m_ProjectService.GetAllocationById((int)associateAllocationId)).Item;
                            if (associateAllocation == null)
                            {
                                response.IsSuccessful = false;
                                response.Message = "Error occured while fetching allocation details by id";
                                return response;
                            }
                        }

                    if (associateAllocation.ProjectId != 0)
                    {
                        var projectDetails = await m_ProjectService.GetProjectByID((int)associateAllocation.ProjectId);
                        if(!projectDetails.IsSuccessful)
                        {
                            response.IsSuccessful = false;
                            response.Message = "Error occured while fetching project details";
                            return response;
                        }
                        associateAllocation.ProjectName = projectDetails.Item.ProjectName;
                    }
                   
                    associateAllocation.ProgramManagerName = (await GetManagerNameByEmpId((int)associateAllocation.ProgramManagerId));
                    associateAllocation.ReportingManagerName = (await GetManagerNameByEmpId((int)associateAllocation.ReportingManagerId));
                    associateAllocation.LeadName = (await GetManagerNameByEmpId((int)associateAllocation.LeadId));

                    }
                    else                
                    {
                    //For non-delivery departments, assiging the same reportingManagerId for Program Manager so that the UI flow will have no impact.
                    //For non-delivery departments, in general there is no program manager and it will be null
                    associateAllocation = new AssociateAllocation();
                    associateAllocation.ProgramManagerId = employeeInfo?.ReportingManager;
                    associateAllocation.ReportingManagerId = employeeInfo?.ReportingManager;
                    associateAllocation.ProgramManagerName = (await GetManagerNameByEmpId((int)employeeInfo.ReportingManager));
                    associateAllocation.ReportingManagerName = associateAllocation.ProgramManagerName;
                    }
                
                response.IsSuccessful = true;
                response.Item = associateAllocation;

            }
            catch (Exception e)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching managers details";
                return response;
            }

            return response;
        }

        private async Task<(int hrmId, string hrmEmail)> GetHRMId()
        {
            var hrDesgDetails = await m_OrgService.GetDesignationByCode(m_EmailConfigurations.HRManagerRole);

            var hrmDtls = await m_EmployeeContext.Employees.Where(e => e.DesignationId == hrDesgDetails.Item.DesignationId)
                          .Select(x => new { x.EmployeeId, x.WorkEmailAddress }).FirstOrDefaultAsync();

            return (hrmDtls.EmployeeId, hrmDtls?.WorkEmailAddress);
        }

        private async Task<string> GetWorkMailbyEmpId(int empId)
        {
            return (empId > 0) ? await m_EmployeeContext.Employees.Where(e => e.EmployeeId == empId && e.IsActive == true).Select(x => x.WorkEmailAddress)?.FirstOrDefaultAsync() : string.Empty;
        }
        private async Task<string> GetManagerNameByEmpId(int empId)
        {
            return (empId > 0) ? await m_EmployeeContext.Employees.Where(e => e.EmployeeId == empId && e.IsActive == true).Select(x => x.FirstName + " " + x.LastName)?.FirstOrDefaultAsync() : string.Empty;
        }

        private async Task<string> GetStatusDesc(int notificationType, int exitId, int statusId)
        {
            var statusDetails = await m_OrgService.GetStatusesByCategoryName(CategoryMaster.AssociateExit.ToString());
            string statusDesc = string.Empty;

            if (statusDetails != null)
            {
                if (notificationType == (int)NotificationType.RevokeInitiated
                    || notificationType == (int)NotificationType.ResignationRevoked || notificationType == (int)NotificationType.RevokeRejected)
                {
                    var revokeWorkflowDtls = await m_EmployeeContext.AssociateExitRevokeWorkflow
                                       .FirstOrDefaultAsync(x => x.AssociateExitId == exitId && x.IsActive == true);

                    if (revokeWorkflowDtls != null)
                    {
                        statusDesc = statusDetails?.Items.FirstOrDefault(x => x.StatusId == revokeWorkflowDtls.RevokeStatusId).StatusDescription;
                    }

                }
                else
                {
                    statusDesc = statusDetails?.Items.FirstOrDefault(x => x.StatusId == statusId).StatusDescription;
                }
            }

            return statusDesc;
        }

        //Get Activity Checklist for each Dept
        private async Task<List<ExitEmployeeResponse>> GetExitEmployeesForChecklist(int departmentId)
        {
            List<ExitEmployeeResponse> exitEmployees = new List<ExitEmployeeResponse>();

            exitEmployees = await (from emp in m_EmployeeContext.Employees
                                   join associateExit in m_EmployeeContext.AssociateExit.Where(w => w.IsActive == true) on emp.EmployeeId equals associateExit.EmployeeId
                                   join activity in m_EmployeeContext.AssociateExitActivity on associateExit.AssociateExitId equals activity.AssociateExitId
                                   where (associateExit.StatusId == Convert.ToInt32(AssociateExitStatusCodesNew.ResignationInProgress)
                                         || associateExit.StatusId == Convert.ToInt32(AssociateExitStatusCodesNew.ReadyForClearance))
                                         && activity.DepartmentId == departmentId
                                   orderby associateExit.ExitDate
                                   select new ExitEmployeeResponse
                                   {
                                       EmployeeId = emp.EmployeeId,
                                       EmployeeCode = emp.EmployeeCode,
                                       FirstName = emp.FirstName,
                                       LastName = emp.LastName,
                                       DepartmentId = emp.DepartmentId.Value,
                                       DesignationId = emp.DesignationId.Value,
                                       AssociateExitId = associateExit.AssociateExitId,
                                       StatusId = associateExit.StatusId,
                                       ExitDate = Convert.ToDateTime(associateExit.ExitDate),
                                       SubStatusId = activity.StatusId
                                   }).Distinct().ToListAsync();
            return exitEmployees;
        }

        private string GetNotificationTemplate(string templatePath)
        {
            string mailBody = null;

            try
            {
                if (!string.IsNullOrEmpty(templatePath))
                {
                    //string filePath = Environment.CurrentDirectory + templatePath;
                    string filePath = templatePath;
                    using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    using var stream = new StreamReader(fs, Encoding.UTF8);

                    mailBody = stream.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex, ex.GetBaseException().StackTrace);
                //throw ex;
            }

            return mailBody;
        }

        private async Task<string> GetKTAssociateEmailAsync(int associateExitId)
        {
            string ktAssociateMail = string.Empty;

            //Getting Email Of Associate who is taking KT
            var ktDetails = await m_EmployeeContext.TransitionPlan.Where(e => e.AssociateExitId == associateExitId).FirstOrDefaultAsync();
            if (ktDetails != null)
            {
                ktAssociateMail = await GetWorkMailbyEmpId(ktDetails.TransitionTo);
            }

            return ktAssociateMail;
        }

        private static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
        #endregion

        #region GetResignedAssociateByID
        /// <summary>
        /// GetResignedAssociateByID
        /// </summary>
        /// <param name="EmployeeId">GetResignedAssociateByID</param>
        /// <returns>AssociateExitCheck</returns>
        public async Task<ServiceResponse<AssociateExitDetails>> GetResignedAssociateByID(int EmployeeId)
        {
            var response = new ServiceResponse<AssociateExitDetails>();
            try
            {
                m_Logger.LogInformation("Calling GetResignedAssociateByID method in AssociateExitService");

                AssociateExit associateExit = await m_EmployeeContext.AssociateExit.FirstOrDefaultAsync(associate => associate.EmployeeId == EmployeeId && associate.IsActive == true);
                if (associateExit != null)
                {
                    AssociateExitDetails associateExitDetails = new AssociateExitDetails()
                    {
                        EmployeeId = associateExit.EmployeeId,
                        ExitDate = associateExit.ExitDate,
                    };

                    response.Item = associateExitDetails;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to fetch Associate Exit.";

                m_Logger.LogError("Error occured in GetResignedAssociateByID method." + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region ReviewReminderNotification
        /// <summary>
        /// ReviewReminderNotification
        /// </summary>
        /// <param name="EmployeeId">GetResignedAssociateByID</param>
        /// <returns>bool</returns>
        public async Task<ServiceResponse<bool>> ReviewReminderNotification(int employeeId)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                m_Logger.LogInformation("Calling ReviewReminderNotification method in AssociateExitService");

                AssociateExit associateExit = await m_EmployeeContext.AssociateExit.FirstOrDefaultAsync(associate => associate.EmployeeId == employeeId && associate.IsActive == true);
                if (associateExit != null)
                {
                    ServiceResponse<int> notification = await AssociateExitSendNotification(associateExit.EmployeeId, Convert.ToInt32(NotificationType.ReviewReminder), null, null);
                    if (!notification.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                    }

                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                m_Logger.LogError("Error occured in ReviewReminderNotification method." + ex.StackTrace);
            }

            return response;
        }
        #endregion
    }
}

