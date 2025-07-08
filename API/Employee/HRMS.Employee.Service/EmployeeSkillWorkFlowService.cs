using AutoMapper;
using HRMS.Common.Enums;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    public class EmployeeSkillWorkFlowService : IEmployeeSkillWorkFlow
    {
        #region Global Variables
        private readonly ILogger<EmployeeSkillWorkFlowService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IOrganizationService m_OrgService;
        private readonly IProjectService m_projectService;
        private readonly IConfiguration m_configuration;
        private readonly MiscellaneousSettings m_MiscellaneousSettings;
        private readonly EmailConfigurations m_EmailConfigurations;
        #endregion

        #region Constructor
        public EmployeeSkillWorkFlowService(
            EmployeeDBContext employeeDBContext,
            ILogger<EmployeeSkillWorkFlowService> logger,
            IOrganizationService organizationService, IProjectService projectService,
            IConfiguration configuration,
            IOptions<MiscellaneousSettings> miscellaneousSettings,
            IOptions<EmailConfigurations> emailConfigurations)
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            m_OrgService = organizationService;
            m_projectService = projectService;
            m_configuration = configuration;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EmployeeSkill, Employee.Entities.EmployeeSkill>();
            });
            m_MiscellaneousSettings = miscellaneousSettings?.Value;
            m_EmailConfigurations = emailConfigurations.Value;
        }
        #endregion

        #region Create
        /// <summary>
        /// creating employee skill workflow for submitting the skill for approval
        /// </summary>
        /// <param name="employeeSkills" "EmployeeId, Id"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<EmployeeSkillWorkFlow>> Create(int employeeId, bool Approve)
        {
            var response = new ServiceResponse<EmployeeSkillWorkFlow>();
            try
            {
                List<EmployeeSkill> employeeSkills = await m_EmployeeContext.EmployeeSkills.Where(empskill => empskill.EmployeeId == employeeId).ToListAsync();

                List<EmployeeSkillWorkFlow> employeeskillworkflow = m_EmployeeContext.EmployeeSkillWorkFlow.Where(WF => WF.SubmittedBy == employeeId && WF.IsActive == true).ToList();
                List<int> employeeskillworkflowIds = employeeskillworkflow.Select(e => e.EmployeeSkillId).ToList();
                List<int> newWorkFlows = employeeskillworkflow.Where(workflow => workflow.Status == (int)SkillStatuscode.Created && workflow.IsActive == true).Select(workflow => workflow.EmployeeSkillId).ToList();
                List<int> employeeSkillIds = (from emp in employeeSkills
                                              where !(employeeskillworkflowIds.Contains(emp.Id))
                                              select emp.Id).Distinct().ToList();
                List<int> allEmployeeSkillIds = new List<int>();
                List<EmployeeSkillWorkFlow> newskillworkflow = employeeskillworkflow.Where(empskill => empskill.Status == (int)SkillStatuscode.Created).ToList();
                List<int> newskillworkflowIds = newskillworkflow.Select(workflow => workflow.EmployeeSkillId).ToList();
                if (newskillworkflowIds.Count() > 0)
                {
                    newskillworkflowIds.ForEach(skillId => allEmployeeSkillIds.Add(skillId));
                }
                if (employeeSkillIds.Count() > 0)
                {
                    employeeSkillIds.ForEach(skillId => allEmployeeSkillIds.Add(skillId));

                }

                // fetching employee reporting manager from employee table


                int? submittedToManager;
                var employee = m_EmployeeContext.Employees.Where(emp => emp.EmployeeId == employeeId).FirstOrDefault();

                if (employee.DepartmentId == 1)
                {
                    var allocation = m_projectService.GetAssociateAllocationsByEmployeeId(employeeId).Result;
                    if (!allocation.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        return response;
                    }
                    //get data based on competency when onboarding the data
                    if (allocation.IsSuccessful && allocation.Items==null)
                    {
                        //As of now we dont have clarity whom to submit the skill for review while onboarding so we are adding the competency leader.
                        // FLOW HAS TO BE CHANGED. IDEALLY WE HAVE TO ADD TECHNICAL PANAL ASSOCIATE FOR REVIEW AND APPROVAL. 

                        var competencyManagers = (await m_projectService.GetCompetencyAreaManagersDetails((int)employee.CompetencyGroup));
                        if (!competencyManagers.IsSuccessful)
                        {
                            response.IsSuccessful = false;
                            response.Message = competencyManagers.Message;
                            return response;
                        }
                        submittedToManager = competencyManagers.Item.LeadId;
                    }
                    else
                    {                       
                        submittedToManager = allocation.Items.Where(x => x.IsPrimary == true).Select(x => x.LeadId).FirstOrDefault();
                    }
                }
                else
                {
                    submittedToManager = m_EmployeeContext.Employees.Where(emp => emp.EmployeeId == employeeId && emp.ReportingManager != null).Select(emp => emp.ReportingManager).FirstOrDefault();
                }
                m_Logger.LogInformation("Calling Create method in EmployeeSkillWorkFlow");

                if (allEmployeeSkillIds.Count() != 0)
                {
                    //create new skillworkflow

                    if (employeeSkillIds.Count() > 0)
                    {
                        foreach (var skill in employeeSkillIds)
                        {
                            EmployeeSkill existingEmployeeSkill = m_EmployeeContext.EmployeeSkills.Where(empsk => empsk.Id == skill).FirstOrDefault();
                            EmployeeSkillWorkFlow employeeSkillWorkFlow = new EmployeeSkillWorkFlow();

                            employeeSkillWorkFlow.EmployeeSkillId = (int)existingEmployeeSkill.Id;
                            employeeSkillWorkFlow.Status = Approve == true ? (int)SkillStatuscode.Approved : (int)SkillStatuscode.Pending;
                            employeeSkillWorkFlow.SubmittedRating = existingEmployeeSkill.ProficiencyLevelId;
                            employeeSkillWorkFlow.SubmittedBy = employeeId;
                            employeeSkillWorkFlow.SubmittedTo = (int)submittedToManager;
                            employeeSkillWorkFlow.Experience = existingEmployeeSkill.Experience;
                            employeeSkillWorkFlow.LastUsed = existingEmployeeSkill.LastUsed;
                            m_EmployeeContext.EmployeeSkillWorkFlow.Add(employeeSkillWorkFlow);

                        }
                    }

                    //From EmployeeSkillWorkFlow table if skill is with status Created(0) then submit for approval

                    if (newskillworkflow.Count() > 0)
                    {
                        foreach (var workflow in newskillworkflow)
                        {
                            workflow.Status = (int)SkillStatuscode.Pending;
                            m_EmployeeContext.EmployeeSkillWorkFlow.Update(workflow);
                        }
                    }

                    int saveWorkflow = m_EmployeeContext.SaveChanges();

                    if (saveWorkflow != 0)
                    {
                        response.IsSuccessful = true;
                        response.Message = "Workflow created successfully";
                        m_Logger.LogInformation("EmployeeSkillWorkFlow created successfully");

                        if (Approve == false)
                        {
                            List<int> skillIds = (from skill in employeeSkills
                                                  join skillworkflow in allEmployeeSkillIds on skill.Id equals skillworkflow
                                                  select skill.SkillId).Distinct().ToList();

                            ServiceListResponse<Skill> skills = await m_OrgService.GetSkillsBySkillId(skillIds);
                            if (!skills.IsSuccessful || skills.Items == null)
                            {
                                response.IsSuccessful = false;
                                response.Message = skills.Message;
                                return response;
                            }
                            List<int> proficiencyLevelIds = (from empskill in employeeSkills
                                                             join skill in skillIds on empskill.SkillId equals skill
                                                             select (int)empskill.ProficiencyLevelId).Distinct().ToList();
                            if (newskillworkflow.Count() > 0)
                            {
                                List<int> proficiencyfromWorkFlow = newskillworkflow.Select(workflow => workflow.SubmittedRating).Cast<int>().ToList();
                                proficiencyfromWorkFlow.ForEach(prof => proficiencyLevelIds.Add(prof));
                            }

                            ServiceListResponse<ProficiencyLevel> proficiencyLevels = await m_OrgService.GetProficiencyLevelsByProficiencyLevelId(proficiencyLevelIds);
                            if (!proficiencyLevels.IsSuccessful || proficiencyLevels.Items == null)
                            {
                                response.IsSuccessful = false;
                                response.Message = skills.Message;
                                return response;
                            }
                            List<EmployeeSkillDetails> skillDetails = (from empskill in employeeSkills
                                                                       join skill in skills.Items on empskill.SkillId equals skill.SkillId
                                                                       join prof in proficiencyLevels.Items on empskill.ProficiencyLevelId equals prof.ProficiencyLevelId
                                                                       select new EmployeeSkillDetails
                                                                       {
                                                                           Id = empskill.Id,
                                                                           SkillId = empskill.SkillId,
                                                                           SkillName = skill.SkillName,
                                                                           CurrentProficiencyLevelCode = prof.ProficiencyLevelCode,
                                                                           ProficiencyLevelId = prof.ProficiencyLevelId,
                                                                           Experience=empskill.Experience,
                                                                           LastUsed=empskill.LastUsed,
                                                                           Version=GetVersions(empskill.Id)
                                                                       }).ToList();

                            skillDetails.ForEach(sk =>
                            {
                                var newProficiency = employeeskillworkflow.Where(s => s.EmployeeSkillId == sk.Id ).FirstOrDefault();
                                if (newProficiency != null)
                                {
                                    sk.ProficiencyLevelCode = proficiencyLevels.Items.Where(x=>x.ProficiencyLevelId== newProficiency.SubmittedRating).FirstOrDefault().ProficiencyLevelCode;
                                    sk.NewLastUsed = newProficiency.LastUsed;
                                    sk.NewExperience = newProficiency.Experience;
                                }
                            });

                            EmployeeDetails employeeDetails = m_EmployeeContext.Employees.Where(employee => employee.EmployeeId == employeeId)
                            .Select(employee => new EmployeeDetails
                            {
                                EmployeeCode = employee.EmployeeCode,
                                EmpName = employee.FirstName + " " + employee.LastName,
                                ReportingManagerId = submittedToManager,
                                WorkEmailAddress = employee.WorkEmailAddress
                            }).FirstOrDefault();

                            var RMDetails = m_EmployeeContext.Employees.Where(employee => employee.EmployeeId == employeeDetails.ReportingManagerId).FirstOrDefault();

                            employeeDetails.ReportingManager = RMDetails.FirstName + " " + RMDetails.LastName;

                            NotificationDetail notificationConfig = EmployeeSkillSubmitNotificationConfig.EmpSkillNotificationConfig(skillDetails, employeeDetails);

                            notificationConfig.FromEmail = m_EmailConfigurations.FromEmail;

                            if (m_MiscellaneousSettings.Environment == "PROD")
                            {
                                notificationConfig.ToEmail = RMDetails.WorkEmailAddress;
                                notificationConfig.CcEmail = employeeDetails.WorkEmailAddress;
                            }
                            if (m_MiscellaneousSettings.Environment == "QA")
                            {
                                notificationConfig.ToEmail = m_EmailConfigurations.ToEmail;
                                notificationConfig.CcEmail = m_EmailConfigurations.CcEmail;
                            }

                            bool IsNotification = m_EmailConfigurations.SendEmail;
                            if (IsNotification)
                            {
                                var emailStatus = await m_OrgService.SendEmail(notificationConfig);
                                if (!emailStatus.IsSuccessful)
                                {
                                    response.IsSuccessful = false;
                                    response.Message = "Error occurred while sending email";
                                    return response;
                                }
                            }
                        }
                    }
                    else
                    {
                        response.IsSuccessful = true;
                        response.Message = "Failed to create";
                    }
                }

                else
                {
                    response.Message = "same record(s) is already submitted";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to create skill workflow.";

                m_Logger.LogError("Error occured in Create method." + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region SkillStatusApprovedByRM
        /// <summary>
        ///employee Skill Approved By Reporting Manager
        /// </summary>
        /// <param name = "employeeId" ></ param >
        /// < returns ></ returns >

        public async Task<ServiceResponse<EmployeeSkillWorkFlow>> SkillStatusApprovedByRM(int employeeId)
        {
            var response = new ServiceResponse<EmployeeSkillWorkFlow>();
            try
            {
                //get the pending skills in EmployeeSkillWorkFlow table
                List<EmployeeSkillWorkFlow> existingEmployeeskillworkflow = m_EmployeeContext.EmployeeSkillWorkFlow.Where(emp => emp.SubmittedBy == employeeId && emp.Status == (int)SkillStatuscode.Pending && emp.IsActive == true).ToList();

                m_Logger.LogInformation("Calling update method in EmployeeSkillWorkFlow while approving");
                if (existingEmployeeskillworkflow.Count() > 0)
                {
                    foreach (var employeeskillworkflow in existingEmployeeskillworkflow)
                    {
                        employeeskillworkflow.Status = (int)SkillStatuscode.Approved;
                        employeeskillworkflow.ApprovedDate = DateTime.Now;
                        m_EmployeeContext.EmployeeSkillWorkFlow.Update(employeeskillworkflow);
                    }

                    m_Logger.LogInformation("Calling savechanges method in EmployeeSkill while updating");

                    int employeeskills = await m_EmployeeContext.SaveChangesAsync();

                    if (employeeskills != 0)
                    {
                        response.IsSuccessful = true;
                        response.Message = "Successfully Approved all the submitted skills";

                        //  send notification to associate after skills approved

                        var employeeDetails = m_EmployeeContext.Employees.Where(employee => employee.EmployeeId == employeeId).FirstOrDefault();
                        var RMDetails = m_EmployeeContext.Employees.Where(employee => employee.EmployeeId == employeeDetails.ReportingManager).FirstOrDefault();

                        NotificationDetail notificationDetail = new NotificationDetail();
                        notificationDetail.FromEmail = m_EmailConfigurations.FromEmail;

                        if (m_MiscellaneousSettings.Environment == "PROD")
                        {
                            notificationDetail.ToEmail = employeeDetails.WorkEmailAddress;
                            notificationDetail.CcEmail = RMDetails.WorkEmailAddress;
                        }
                        if (m_MiscellaneousSettings.Environment == "QA")
                        {
                            notificationDetail.ToEmail = m_EmailConfigurations.ToEmail;
                            notificationDetail.CcEmail = m_EmailConfigurations.CcEmail;
                        }

                        notificationDetail.Subject = "Skilll(s) has approved by " + RMDetails.FirstName + " " + RMDetails.LastName;
                        notificationDetail.EmailBody = "Skill(s) has approved";

                        bool IsNotification = m_EmailConfigurations.SendEmail;
                        if (IsNotification)
                        {
                            var emailStatus = await m_OrgService.SendEmail(notificationDetail);
                            if (!emailStatus.IsSuccessful)
                            {
                                response.IsSuccessful = false;
                                response.Message = "Error occurred while sending email";
                                return response;
                            }
                        }
                    }
                }
                else
                {
                    response.Message = "Employee skill already approved";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to update skill workflow while approving.";

                m_Logger.LogError("Error occured in update method." + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetSkillSubmittedByEmployee
        /// <summary>
        /// get all the skill submitted employees details 
        /// </summary>
        /// <param name="">reportingManager</param>
        /// <returns></returns>

        public async Task<ServiceListResponse<EmployeeSkillWorkflow>> GetSkillSubmittedByEmployee(int reportingManager)
        {
            var response = new ServiceListResponse<EmployeeSkillWorkflow>();
            try
            {

                // get all the skill submitted employees id's (which are pending for approval)

                List<EmployeeSkillWorkFlow> getsubmittedSkillsDetails = await m_EmployeeContext.EmployeeSkillWorkFlow.Where(emp => emp.Status == (int)SkillStatuscode.Pending && emp.IsActive == true && emp.SubmittedTo == reportingManager).ToListAsync();
                if (getsubmittedSkillsDetails.Count() > 0)
                {
                    List<int> skillSubmittedEmpIds = getsubmittedSkillsDetails.Select(emp => emp.SubmittedBy).Distinct().ToList();


                    List<EmployeeSkillWorkFlow> employeeskillsworkflow = new List<EmployeeSkillWorkFlow>();
                    List<EmployeeSkillWorkflow> getskillSubmittedEmpDetails = new List<EmployeeSkillWorkflow>();
                    skillSubmittedEmpIds.ForEach(empskill => employeeskillsworkflow.Add(getsubmittedSkillsDetails.Where(emp => emp.SubmittedBy == empskill).FirstOrDefault()));
                    var ManagerDetails = m_EmployeeContext.Employees.Where(emp => emp.EmployeeId == reportingManager && emp.IsActive == true).FirstOrDefault();
                    if (ManagerDetails.DepartmentId == 1)
                    {
                        IEnumerable<AssociateAllocation> AssociateAllocation = m_projectService.GetAssociateAllocations(skillSubmittedEmpIds).Result.Items.Where(pr => pr.IsPrimary == true && pr.IsActive == true && pr.LeadId == reportingManager);
                        List<int> getProjectIds = AssociateAllocation.Select(pr => pr.ProjectId.Value).Distinct().ToList();

                        List<Project> projectNames = m_projectService.GetProjectById(getProjectIds).Result.Items;

                        getskillSubmittedEmpDetails = (from al in AssociateAllocation
                                                                                   from prn in projectNames
                                                                                   from emp in m_EmployeeContext.Employees
                                                                                   from skillworkflow in employeeskillsworkflow
                                                                                   where al.ProjectId == prn.ProjectId && emp.EmployeeId == al.EmployeeId && skillworkflow.SubmittedBy == emp.EmployeeId
                                                                                   select new EmployeeSkillWorkflow
                                                                                   {
                                                                                       EmployeeId = al.EmployeeId,
                                                                                       ProjectName = prn.ProjectName,
                                                                                       EmployeeName = emp.FirstName + " " + emp.LastName,
                                                                                       EmployeeCode = emp.EmployeeCode,
                                                                                       SubmittedDate = skillworkflow.CreatedDate
                                                                                   }).ToList();
                    }
                    else
                    {
                        getskillSubmittedEmpDetails = (from emp in m_EmployeeContext.Employees
                                                       from skillworkflow in employeeskillsworkflow
                                                      where skillworkflow.SubmittedBy == emp.EmployeeId
                                                       select new EmployeeSkillWorkflow
                                                       {
                                                           EmployeeId = emp.EmployeeId,                                                          
                                                           EmployeeName = emp.FirstName + " " + emp.LastName,
                                                           EmployeeCode = emp.EmployeeCode,
                                                           SubmittedDate = skillworkflow.CreatedDate
                                                       }).ToList();
                    }
                    response.IsSuccessful = true;
                    response.Items = getskillSubmittedEmpDetails;
                }
                else
                {
                    response.Message = "No Record is available";
                    response.IsSuccessful = true;
                    response.Items = new List<EmployeeSkillWorkflow>();
                }
            }

            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching employee details";
                m_Logger.LogError("Error occured while fetching EmployeeDetails" + ex.StackTrace);
            }

            return response;
        }
        #endregion

        #region GetSubmittedSkillsByEmpid
        /// <summary>
        /// get all the skills submitted by employeeid
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeSkillDetails>> GetSubmittedSkillsByEmpid(int employeeId)
        {
            var response = new ServiceListResponse<EmployeeSkillDetails>();
            try
            {
                //get statusid pending employee skills based on employeeId
                List<EmployeeSkillWorkFlow> employeeSkillworkflow = await m_EmployeeContext.EmployeeSkillWorkFlow.Where(empskillworkflow => empskillworkflow.SubmittedBy == employeeId && empskillworkflow.IsActive == true && empskillworkflow.Status == (int)SkillStatuscode.Pending).ToListAsync();

                List<EmployeeSkillDetails> skillList;

                if (employeeSkillworkflow.Count > 0)
                {
                    //Fetch SkillIds
                    List<int> empskillIdsfromWorkFlow = employeeSkillworkflow.Select(skillworkflow => skillworkflow.EmployeeSkillId).ToList();

                    List<EmployeeSkill> skillDetails = m_EmployeeContext.EmployeeSkills.Where(empskill => empskillIdsfromWorkFlow.Contains(empskill.Id)).ToList();
                    List<int> skilids = skillDetails.Select(skill => (int)skill.SkillId).ToList();


                    //Fetch distinct proficiencyLevelIds
                    List<int> proficiencyLevelIds = employeeSkillworkflow.Select(skill => skill.SubmittedRating.Value).Distinct().ToList();
                    ServiceListResponse<Skill> skills = await m_OrgService.GetSkillsBySkillId(skilids);
                    if (!skills.IsSuccessful || skills.Items == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = skills.Message;
                        return response;
                    }
                    List<int> proficiencybyRM = m_EmployeeContext.EmployeeSkillWorkFlow.Where(emp => emp.SubmittedBy == employeeId && emp.ReportingManagerRating != null && emp.IsActive == true && emp.Status == (int)SkillStatuscode.Pending).Select(emp => emp.ReportingManagerRating).Cast<int>().ToList();
                    if (proficiencybyRM.Count() != 0)
                    {
                        proficiencybyRM.ForEach(item => proficiencyLevelIds.Add(item));
                    }
                    ServiceListResponse<ProficiencyLevel> proficiencyLevels = await m_OrgService.GetProficiencyLevelsByProficiencyLevelId(proficiencyLevelIds);
                    if (!proficiencyLevels.IsSuccessful || proficiencyLevels.Items == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = skills.Message;
                        return response;
                    }
                    var addProficiencyToSkill = (from emps in employeeSkillworkflow
                                                 join prof in proficiencyLevels.Items on emps.ReportingManagerRating equals prof.ProficiencyLevelId into EmployeeskillsProf
                                                 from empskpro in EmployeeskillsProf.DefaultIfEmpty()
                                                 select new
                                                 {
                                                     EmpSkillId = emps.EmployeeSkillId,
                                                     ProficiencyIDByRM = empskpro == null ? 0 : emps.ReportingManagerRating != null && emps.ReportingManagerRating == empskpro.ProficiencyLevelId ? empskpro.ProficiencyLevelId : 0,
                                                     ProficiencyByRM = empskpro == null ? null : emps.ReportingManagerRating != null && emps.ReportingManagerRating == empskpro.ProficiencyLevelId ? empskpro.ProficiencyLevelCode : null,
                                                 }).ToList();

                    skillList = (from es in skillDetails
                                 join skill in skills.Items on (int)es.SkillId equals skill.SkillId
                                 from skillworkflow in employeeSkillworkflow
                                 where es.Id == skillworkflow.EmployeeSkillId
                                 from prof in addProficiencyToSkill
                                 where es.Id == prof.EmpSkillId
                                 from pro in proficiencyLevels.Items
                                 where pro.ProficiencyLevelId == skillworkflow.SubmittedRating
                                 select new EmployeeSkillDetails
                                 {
                                     Id = es.Id,
                                     EmployeeId = es.EmployeeId,
                                     IsPrimary = es.IsPrimary,
                                     LastUsed = (int)skillworkflow.LastUsed,
                                     Experience = ConvertExperienceInYears(skillworkflow.Experience),
                                     SkillId = es.SkillId,
                                     SkillCode = skill.SkillCode,
                                     SkillName = skill.SkillName,
                                     SkillGroupId = (int)es.SkillGroupId,
                                     SkillGroupName = skill.SkillGroup.SkillGroupName,
                                     CompetencyAreaId = (int)es.CompetencyAreaId,
                                     CompetencyAreaCode = skill.CompetencyArea.CompetencyAreaCode,
                                     ProficiencyLevelId = skillworkflow.SubmittedRating,
                                     ProficiencyLevelCode = pro.ProficiencyLevelCode,
                                     ProficiencyIDByRM = prof.ProficiencyIDByRM == 0 ? skillworkflow.SubmittedRating : prof.ProficiencyIDByRM,
                                     ProficiencyByRM = prof.ProficiencyByRM == null ? pro.ProficiencyLevelCode : prof.ProficiencyByRM,
                                     Remarks = skillworkflow.Remarks,
                                     Version= GetVersions(es.Id)
                                 }).ToList();

                    if (skillList.Count() > 0)
                    {
                        response.IsSuccessful = true;
                        response.Items = skillList;
                    }
                }
                else
                {
                    skillList = new List<EmployeeSkillDetails>();
                    response.Message = "No Submitted Skills available with this Employee";
                }


            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching employeeskill details";
                m_Logger.LogError("Error occured while fetching employeeskill" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region UpdateEmpSkillDetails
        /// <summary>
        /// update employeeskill details
        /// </summary>
        /// <param name="employeeSkill"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<EmployeeSkill>> UpdateEmpSkillDetails(int employeeId)
        {
            var response = new ServiceResponse<EmployeeSkill>();
            try
            {
                //verify the proficiency is updated or not in EmployeeSkillWorkFlow table ,if updated then needs to update in EmployeeSkills table.
                var existingEmployeeskillworkflow = m_EmployeeContext.EmployeeSkillWorkFlow.Where(emp => emp.SubmittedBy == employeeId && emp.Status == (int)SkillStatuscode.Approved
                && emp.IsActive == true).ToList();

                if (existingEmployeeskillworkflow.Count() != 0)
                {
                    //update the ReportingManager proficiency column with submitted proficiency if it is null
                    List<EmployeeSkillWorkFlow> employeSKillWorkFlow = existingEmployeeskillworkflow.Where(empsk => empsk.ReportingManagerRating == null).ToList();
                    if (employeSKillWorkFlow.Count > 0)
                    {
                        foreach (var skillWorkflow in employeSKillWorkFlow)
                        {
                            EmployeeSkillWorkFlow employeeSkillWorkFlow = m_EmployeeContext.EmployeeSkillWorkFlow.Find(skillWorkflow.Id);
                            employeeSkillWorkFlow.ReportingManagerRating = employeeSkillWorkFlow.SubmittedRating;
                        }
                    }
                    //update EmployeeSkill table Proficiency/Experience/Lastused if changed in EmployeeSkillWorkFlow
                    foreach (var skillWorkflow in existingEmployeeskillworkflow)
                    {
                        EmployeeSkill employeeskill = m_EmployeeContext.EmployeeSkills.Find(skillWorkflow.EmployeeSkillId);
                        if (employeeskill.ProficiencyLevelId != skillWorkflow.ReportingManagerRating || employeeskill.Experience != skillWorkflow.Experience || employeeskill.LastUsed != skillWorkflow.LastUsed)
                        {
                            employeeskill.ProficiencyLevelId = (int)skillWorkflow.ReportingManagerRating;
                            employeeskill.Experience = (int)skillWorkflow.Experience;
                            employeeskill.LastUsed = (int)skillWorkflow.LastUsed;
                        }

                    }
                    int updateSkill = await m_EmployeeContext.SaveChangesAsync();
                    if (updateSkill != 0)
                    {
                        response.IsSuccessful = true;
                        response.Message = "updated successfully";
                    }
                    else
                    {
                        response.Message = "Failed to update";
                    }
                }
                else
                {
                    response.Message = "No record is available for update";
                }
            }

            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to update Employeeskill.";

                m_Logger.LogError("Error occured in updateEmpSkillDetails method." + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetEmployeeSkillHistory
        /// <summary>
        ///get all the skills history details.
        /// </summary>
        /// <param name="employeeid"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeSkillWorkflow>> GetEmployeeSkillHistory(int employeeid, int ID)
        {
            var response = new ServiceListResponse<EmployeeSkillWorkflow>();
            try
            {

                //verify the is same id exist in Employeeskills table
                List<EmployeeSkillWorkFlow> existingEmployeeskillworkflow = await m_EmployeeContext.EmployeeSkillWorkFlow.Where(emp => emp.SubmittedBy == employeeid && emp.EmployeeSkillId == ID).ToListAsync();
                if (existingEmployeeskillworkflow.Count() > 0)
                {
                    List<int> proficiencyLevelIds = existingEmployeeskillworkflow.Select(skill => skill.SubmittedRating.Value).Distinct().ToList();
                    List<int> proficiencyLevelIds2 = new List<int>();
                    List<EmployeeSkillWorkFlow> proficiency = existingEmployeeskillworkflow.Where(emp => emp.ReportingManagerRating != null).ToList();
                    if (proficiency.Count() != 0)
                    {
                        proficiencyLevelIds2 = proficiency.Select(emp => emp.ReportingManagerRating).Cast<int>().ToList();

                        proficiencyLevelIds2.ForEach(proficiency => proficiencyLevelIds.Add(proficiency));
                    }

                    ServiceListResponse<ProficiencyLevel> proficiencyLevels = await m_OrgService.GetProficiencyLevelsByProficiencyLevelId(proficiencyLevelIds);

                    if (!proficiencyLevels.IsSuccessful || proficiencyLevels.Items == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = proficiencyLevels.Message;
                        return response;
                    }
                    var skillID = m_EmployeeContext.EmployeeSkills.Find(ID).SkillId;
                    List<int> skillObj = new List<int>();
                    skillObj.Add(skillID);
                    //get the skill details
                    var skills = await m_OrgService.GetSkillsBySkillId(skillObj);


                    var employeeRM = m_EmployeeContext.Employees.Where(emp => emp.EmployeeId == employeeid && emp.ReportingManager != null).Select(emp => emp.ReportingManager).FirstOrDefault();
                    var ReportingManager = m_EmployeeContext.Employees.Where(emp => emp.EmployeeId == employeeRM).FirstOrDefault();
                    var employeeSkillWorkflow = new List<EmployeeSkillWorkflow>();

                    var skilProficiencyDetails = (from empski in existingEmployeeskillworkflow
                                                  from emp in existingEmployeeskillworkflow
                                                  where empski.Id == emp.Id
                                                  from pro in proficiencyLevels.Items
                                                  where empski.SubmittedRating == pro.ProficiencyLevelId || empski.ReportingManagerRating == pro.ProficiencyLevelId
                                                  select new
                                                  {
                                                      ID = emp.Id,
                                                      EmployeeSkillId = empski.EmployeeSkillId,
                                                      SubmittedRating = empski.SubmittedRating,
                                                      SubmittedRatingName = empski.SubmittedRating == pro.ProficiencyLevelId ? pro.ProficiencyLevelCode : null,
                                                      ReportingManagerRating = empski.ReportingManagerRating,
                                                      ReportingManagerRatingName = empski.ReportingManagerRating == pro.ProficiencyLevelId ? pro.ProficiencyLevelCode : null,

                                                  }).ToList();


                    var employeeSkillWorkflowHistoryForProficiency = skilProficiencyDetails.GroupBy(skill => skill.ID).Select(skill => new EmployeeSkillWorkflow
                    {
                        Id = skill.FirstOrDefault().ID,
                        SubmittedRating = skill.FirstOrDefault().SubmittedRating,
                        SubmittedRatingName = skill.Where(r => !string.IsNullOrEmpty(r.SubmittedRatingName)).Select(r => r.SubmittedRatingName).FirstOrDefault(),
                        ReportingManagerRating = skill.FirstOrDefault().ReportingManagerRating,
                        ReportingManagerRatingName = skill.Where(r => !string.IsNullOrEmpty(r.ReportingManagerRatingName)).Select(r => r.ReportingManagerRatingName).FirstOrDefault(),
                    }).ToList();

                    var skillList = (from empskills in existingEmployeeskillworkflow
                                     from empski in employeeSkillWorkflowHistoryForProficiency
                                     where empskills.Id == empski.Id
                                     from sk in skills.Items
                                     select new EmployeeSkillWorkflow
                                     {
                                         EmployeeSkillId = empski.EmployeeSkillId,
                                         EmployeeId = empskills.SubmittedBy,
                                         SkillName = sk.SkillName,
                                         Status = empskills.Status,
                                         SubmittedDate = empskills.CreatedDate,
                                         StatusName = empskills.Status == (int)SkillStatuscode.Pending ? "Pending for approval" : empskills.Status == (int)SkillStatuscode.Approved ? "Approved" : "Created",
                                         SubmittedBy = empskills.SubmittedBy,
                                         SubmittedToName = employeeRM != null ? ReportingManager.FirstName + " " + ReportingManager.LastName : null,
                                         SubmittedTo = empskills.SubmittedTo,
                                         SubmittedRating = empski.SubmittedRating,
                                         SubmittedRatingName = empski.SubmittedRatingName,
                                         ReportingManagerRating = empski.ReportingManagerRating,
                                         ReportingManagerRatingName = empski.ReportingManagerRatingName,
                                         ApprovedDate = empskills.ApprovedDate,
                                         Remarks = empskills.Remarks,
                                         Experience =ConvertExperienceInYears(empskills.Experience),
                                         LastUsed = empskills.LastUsed                                         
                                     }).OrderBy(workflow => workflow.SubmittedDate).ToList();



                    if (skillList != null)
                    {
                        response.IsSuccessful = true;
                        response.Items = skillList;
                    }
                    else
                    {
                        response.Message = "Failed to fetch records from EmployeeSkillWorkFlow";
                    }
                }
                else
                {
                    response.Message = "No history for this skill";
                }

            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "error occured while fetching records from EmployeeSkillWorkFlow table.";

                m_Logger.LogError("Error occured in GetEmployeeSkillHistory method." + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region UpdateEmpSkillProficienyByRM
        /// <summary>
        /// update employee proficiency level .
        /// </summary>
        /// <param name="employeeSkills"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<EmployeeSkill>> UpdateEmpSkillProficienyByRM(EmployeeSkillWorkflow employeeSkills)
        {
            var response = new ServiceResponse<EmployeeSkill>();
            try
            {
                //verify the is same id exist in Employeeskills table
                var employeeSkillworkflow = m_EmployeeContext.EmployeeSkillWorkFlow.Where(emp => emp.SubmittedBy == employeeSkills.EmployeeId && emp.EmployeeSkillId == employeeSkills.EmployeeSkillId && emp.IsActive == true && emp.Status == (int)SkillStatuscode.Pending).FirstOrDefault();

                m_Logger.LogInformation("Calling insery method in EmployeeSkills");

                //Add a new record

                // if proficiency level is different then insert and update
                if (employeeSkillworkflow != null)
                {
                    employeeSkillworkflow.ReportingManagerRating = employeeSkills.ReportingManagerRating;
                    employeeSkillworkflow.Remarks = employeeSkills.Remarks;
                    m_EmployeeContext.EmployeeSkillWorkFlow.Update(employeeSkillworkflow);

                    m_Logger.LogInformation("Calling savechanges method in EmployeeSkill");

                    int employeeskillAdd = await m_EmployeeContext.SaveChangesAsync();
                    if (employeeskillAdd != 0)
                    {
                        response.IsSuccessful = true;
                        response.Message = "updated successfully";
                    }
                    else
                    {
                        response.Message = "Failed to update";
                    }

                }

                else
                {
                    response.Message = "Proficiency level is null or same as existing so no need to update";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Unable to update Employeeskill.";

                m_Logger.LogError("Error occured in updateEmpSkillDetailsByRM method." + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region private methods
        private string GetVersions(int? employeeSkillId)
        {
            string versions = string.Empty;
            if(employeeSkillId != null)
                {
               versions=  string.Join(",", m_EmployeeContext.SkillVersion.Where(version => version.EmployeeSkillId == employeeSkillId).Select(v => v.Version).ToArray());
            }
            return versions;
        }

        private decimal ConvertExperienceInYears(int? experience)
        {
            decimal experienceInYears = 0;
            if (experience != null)
            {
                experienceInYears = Convert.ToDecimal(Convert.ToDecimal(experience / 12) + "." + Convert.ToDecimal(experience) % 12);
            }
            return experienceInYears;
        }
        #endregion

    }
}
