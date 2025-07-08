using AutoMapper;
using HRMS.Common;
using HRMS.Common.Enums;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    public class EmployeeSkillService : IEmployeeSkillService
    {
        #region Global Variables
        private readonly ILogger<EmployeeSkillService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;
        private readonly IOrganizationService m_OrgService;
        private readonly IEmployeeSkillWorkFlow  m_employeeSkillWorkFlow;
        private readonly IProjectService  m_projectService;
        #endregion

        #region EmployeeSkillService
        public EmployeeSkillService(ILogger<EmployeeSkillService> logger,
                    EmployeeDBContext employeeDBContext,
                    IOrganizationService orgService, IEmployeeSkillWorkFlow employeeSkillWorkFlow,IProjectService projectService)
        {
            m_Logger = logger;
            m_EmployeeContext = employeeDBContext;
            m_OrgService = orgService;
            m_employeeSkillWorkFlow = employeeSkillWorkFlow;
            m_projectService = projectService;
            //Create mapper for EmployeeSkill
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EmployeeSkillDetails, EmployeeSkill>();
            });
            m_mapper = config.CreateMapper();
        }
        #endregion
        
        #region Create
        /// <summary>
        /// Create employee skill
        /// </summary>
        /// <param name="employeeSkillDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<EmployeeSkill>> Create(EmployeeSkillDetails employeeSkillDetails)
        {
            int isCreated = 0;
            var response = new ServiceResponse<EmployeeSkill>();
            m_Logger.LogInformation("EmployeeSkillService: Calling \"Create\" method.");
            using (var dbContext = m_EmployeeContext.Database.BeginTransaction())
            {
                try
                {
                    //Check if skill is valid or not
                    List<int> skillList = new List<int>();
                    skillList.Add((int)employeeSkillDetails.SkillId);
                    var skills = m_OrgService.GetSkillsBySkillId(skillList).Result;
                    if (skills.Items.Count()==0)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Skill is not valid";
                        return response;
                    }
                    //Check if skill already exists
                    int isExist = await m_EmployeeContext.EmployeeSkills
                                            .Where(skill_local => skill_local.EmployeeId == employeeSkillDetails.EmployeeId
                                                && skill_local.SkillId == employeeSkillDetails.SkillId)
                                            .CountAsync();
                    if (isExist > 0)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Skill already exists";
                        return response;
                    }
                    bool isExperienceValid = ValidateExperience(employeeSkillDetails.Experience);
                    if(!isExperienceValid)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Experience is not valid";
                        return response;
                    }
                    EmployeeSkill skill = new EmployeeSkill();
                    //map fields
                    
                    m_mapper.Map<EmployeeSkillDetails, EmployeeSkill>(employeeSkillDetails, skill);
                    skill.Experience = ConvertExperienceInMonths(employeeSkillDetails.Experience);
                    //add skill to entity
                    m_EmployeeContext.EmployeeSkills.Add(skill);

                    m_Logger.LogInformation("Calling SaveChanges method on DB Context in EmployeeSkillService");
                    isCreated = await m_EmployeeContext.SaveChangesAsync();
                    if (isCreated > 0)
                    {
                        // Add versions into table
                        if (!string.IsNullOrEmpty(employeeSkillDetails.Version))
                        {
                         var versionResponse=  AddOrUpdateSkillVersion(employeeSkillDetails.Version, skill.Id, employeeSkillDetails.EmployeeId).Result;
                            if(!versionResponse.IsSuccessful)
                            {
                                response.IsSuccessful = false;
                                response.Message = versionResponse.Message;
                                return response;
                            }
                        }
                            if (employeeSkillDetails.RoleName == "HRA" || employeeSkillDetails.RoleName == "HRM")
                        {
                            var result = (await m_employeeSkillWorkFlow.Create(employeeSkillDetails.EmployeeId, true)).IsSuccessful;
                            if (result == false)
                            {
                                response.IsSuccessful = false;
                                response.Message = "Error occurred while creating skill workflow";
                                return response;
                            }

                        }

                        response.IsSuccessful = true;
                        response.Item = skill;

                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occurred while creating skill";
                    }

                    await dbContext.CommitAsync();
                }
                catch (Exception ex)
                {
                    dbContext.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while creating employee skill details";
                    m_Logger.LogError("Error occurred in \"Create\" method of EmployeeSkillService" + ex.StackTrace);
                }
            }
            return response;
        }
        #endregion

        #region GetByEmployeeId
        /// <summary>
        /// employeeId
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeSkillDetails>> GetByEmployeeId(int employeeId,string roleName)
        {
            List<EmployeeSkillDetails> skillList;
            var response = new ServiceListResponse<EmployeeSkillDetails>();
            m_Logger.LogInformation("EmployeeSkillService: Calling \"GetByEmployeeId\" method.");
            try
            {
                //Fetch skills of the given employee
                var employeeSkills = await m_EmployeeContext.EmployeeSkills
                                    .Where(skill => skill.EmployeeId == employeeId) 
                                    .ToListAsync();


                if (employeeSkills.Count > 0)
                {
                    //Fetch SkillIds
                    List<int> skillIds = employeeSkills.Select(skill => skill.SkillId).ToList();

                    //Fetch the skills based on the above obtained SkillIds 
                    var skills = await m_OrgService.GetSkillsBySkillId(skillIds);
                    if (!skills.IsSuccessful || skills.Items == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = skills.Message;
                        return response;
                    }
                    List<EmployeeSkillWorkFlow> emplskillWorkFlow = m_EmployeeContext.EmployeeSkillWorkFlow.Where(empsk => empsk.SubmittedBy == employeeId && empsk.IsActive == true).ToList();
                    List<EmployeeSkillWorkFlow> skillworkflow = new List<EmployeeSkillWorkFlow>();
                    if (roleName == "HRA" || roleName == "HRM")
                    {
                    //fetching the skill which are available in workflow with status created and pending
                       skillworkflow = (from skill in employeeSkills
                                                                     join workflow in emplskillWorkFlow.Where(workflow => workflow.Status == (int)SkillStatuscode.Approved).ToList()
                                                                     on skill.Id equals workflow.EmployeeSkillId
                                                                     select workflow).ToList();

                        employeeSkills = (from ski in skillworkflow
                                          join wr in employeeSkills on ski.EmployeeSkillId equals wr.Id
                                          select wr).ToList();
                    }
                    else
                    {
                        //fetching the skill which are available in workflow with status created and pending

                         skillworkflow = (from skill in employeeSkills
                                                                     join workflow in emplskillWorkFlow.Where(workflow => workflow.Status != (int)SkillStatuscode.Approved).ToList()
                                                                     on skill.Id equals workflow.EmployeeSkillId
                                                                     select workflow).ToList();
                    }
                    // Replacing the skill details with updated workflow details.

                    skillworkflow.ForEach(wokrflow =>
                    {
                        employeeSkills.Where(skill => skill.Id == wokrflow.EmployeeSkillId).ToList().ForEach(skill =>
                        {
                            skill.LastUsed = (int)wokrflow.LastUsed;
                            skill.Experience = (int)wokrflow.Experience;
                            skill.ProficiencyLevelId = (int)wokrflow.SubmittedRating;
                        });
                    });

                    //Fetch distinct proficiencyLevelIds
                    List<int> proficiencyLevelIds = employeeSkills.Select(skill => skill.ProficiencyLevelId).Distinct().ToList();

                    //Fetch the proficiencyLevels based on the above obtained distinct proficiencyLevelIds
                    var proficiencyLevels = await m_OrgService.GetProficiencyLevelsByProficiencyLevelId(proficiencyLevelIds);
                    if (!proficiencyLevels.IsSuccessful || proficiencyLevels.Items == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = skills.Message;
                        return response;
                    }


                    //Join the tables and fetch employeeSkills details
                    skillList = (from es in employeeSkills
                                 join skill in skills.Items on es.SkillId equals skill.SkillId
                                 join pl in proficiencyLevels.Items on es.ProficiencyLevelId equals pl.ProficiencyLevelId
                                 join emps in emplskillWorkFlow on es.Id equals emps.EmployeeSkillId into employee
                                 from emp in employee.DefaultIfEmpty()
                                 select new EmployeeSkillDetails
                                 {
                                     Id = es.Id,
                                     EmployeeId = es.EmployeeId,
                                     IsPrimary = (bool)es.IsPrimary,
                                     LastUsed = (int)es.LastUsed,
                                     Experience = ConvertExperienceInYears(es.Experience),
                                     SkillId = (int)es.SkillId,
                                     SkillCode = skill.SkillCode,
                                     SkillName = skill.SkillName,
                                     SkillGroupId = (int)es.SkillGroupId,
                                     SkillGroupName = skill.SkillGroup.SkillGroupName,
                                     CompetencyAreaId = (int)es.CompetencyAreaId,
                                     CompetencyAreaCode = skill.CompetencyArea.CompetencyAreaCode,
                                     ProficiencyLevelId = (int)es.ProficiencyLevelId,
                                     ProficiencyLevelCode = pl.ProficiencyLevelCode,
                                     StatusId = emp == null ? (int)SkillStatuscode.Created : emp.Status,
                                     StatusName = emp == null ? "Created" : emp.Status == (int)SkillStatuscode.Pending ? "Pending for approval" : emp.Status == (int)SkillStatuscode.Approved ? "Approved" : "Created",
                                     Version = string.Join(",", m_EmployeeContext.SkillVersion.Where(version => version.EmployeeSkillId == es.Id).Select(v => v.Version).ToArray()),
                                     IsAlreadyExist=emp==null?false: IsSkillAvailable(emp.EmployeeSkillId)
                                 }).ToList();
                }
                else
                {
                    skillList = new List<EmployeeSkillDetails>();
                }
                response.IsSuccessful = true;
                response.Items = skillList;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error oocurred while fetching skill details";
                m_Logger.LogError("Error oocurred in \"GetByEmployeeId\" of EmployeeSkillService " + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update employee skill
        /// </summary>
        /// <param name="employeeSkillDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<EmployeeSkill>> Update(EmployeeSkillDetails employeeSkillDetails)
        {
            var response = new ServiceResponse<EmployeeSkill>();
            var versionRespone = new ServiceResponse<bool>();
            int ? submittedTo;
            m_Logger.LogInformation("EmployeeSkillService: Calling \"Update\" method.");
            using (var dbContext = m_EmployeeContext.Database.BeginTransaction())
            {
                try
                {
                    //check if skill exists based on Id
                    EmployeeSkill skill = m_EmployeeContext.EmployeeSkills.Find(employeeSkillDetails.Id);
                    if (skill == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Skill record not found";
                        return response;
                    }
                    bool isValidExperience = ValidateExperience(employeeSkillDetails.Experience);
                    if(!isValidExperience)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Experience is not valid";
                        return response;
                    }

                    //Check if skill already exists
                    int isExist = await m_EmployeeContext.EmployeeSkills
                                        .Where(skill_local => skill_local.EmployeeId == employeeSkillDetails.EmployeeId
                                            && skill_local.SkillId == employeeSkillDetails.SkillId
                                            && skill_local.Id != employeeSkillDetails.Id)
                                        .CountAsync();
                    if (isExist > 0)
                    {

                        response.IsSuccessful = false;
                        response.Message = "Skill already exists";
                        return response;
                    }
                    else
                    {
                        var isexistin = m_EmployeeContext.EmployeeSkillWorkFlow.Where(empsk => empsk.EmployeeSkillId == employeeSkillDetails.Id).FirstOrDefault();

                        //when associate update the record before submit
                        if (isexistin == null)
                        {

                            //map fields
                            m_mapper.Map<EmployeeSkillDetails, EmployeeSkill>(employeeSkillDetails, skill);
                            skill.Experience = ConvertExperienceInMonths(employeeSkillDetails.Experience);
                            m_Logger.LogInformation("Calling SaveChanges method on DB Context in EmployeeSkillService");
                            await m_EmployeeContext.SaveChangesAsync();
                            if (!string.IsNullOrEmpty(employeeSkillDetails.Version))
                            {
                               versionRespone= AddOrUpdateSkillVersion(employeeSkillDetails.Version, skill.Id, employeeSkillDetails.EmployeeId).Result;
                               if(!versionRespone.IsSuccessful)
                                {
                                    response.IsSuccessful = false;
                                    response.Message = versionRespone.Message;
                                    return response;
                                }
                            }
                            else
                            {
                                var versionsToRemove = m_EmployeeContext.SkillVersion.Where(version => version.EmployeeSkillId == employeeSkillDetails.Id).ToList();
                                m_EmployeeContext.SkillVersion.RemoveRange(versionsToRemove);
                                await m_EmployeeContext.SaveChangesAsync();
                            }
                            response.IsSuccessful = true;
                                response.Item = skill;
                        }
                        else
                        {
                            // update in Versions table
                            if (!string.IsNullOrEmpty(employeeSkillDetails.Version))
                            {
                                versionRespone = AddOrUpdateSkillVersion(employeeSkillDetails.Version, skill.Id, employeeSkillDetails.EmployeeId).Result;
                                if (!versionRespone.IsSuccessful)
                                {
                                    response.IsSuccessful = false;
                                    response.Message = versionRespone.Message;
                                    return response;
                                }
                            }
                            else
                            {
                                var versionsToRemove = m_EmployeeContext.SkillVersion.Where(version => version.EmployeeSkillId == employeeSkillDetails.Id).ToList();
                                m_EmployeeContext.SkillVersion.RemoveRange(versionsToRemove);
                                await m_EmployeeContext.SaveChangesAsync();
                            }
                            //update operation In Workflow table if the Status is Created
                            var isnewSkillExistInWorkFlow = m_EmployeeContext.EmployeeSkillWorkFlow.Where(empsk => empsk.EmployeeSkillId == employeeSkillDetails.Id && empsk.IsActive == true && empsk.Status == (int)SkillStatuscode.Created).FirstOrDefault();
                            if (isnewSkillExistInWorkFlow != null)
                            {
                                //updating existing record as inactive.
                                isnewSkillExistInWorkFlow.SubmittedRating = employeeSkillDetails.ProficiencyLevelId;
                                isnewSkillExistInWorkFlow.Experience =ConvertExperienceInMonths(employeeSkillDetails.Experience);
                                isnewSkillExistInWorkFlow.LastUsed = employeeSkillDetails.LastUsed;
                                m_EmployeeContext.EmployeeSkillWorkFlow.Update(isnewSkillExistInWorkFlow);                             
                            }
                          
                            var isExistInWorkFlow = m_EmployeeContext.EmployeeSkillWorkFlow.Where(empsk => empsk.EmployeeSkillId == employeeSkillDetails.Id && empsk.IsActive == true && empsk.Status == (int)SkillStatuscode.Approved).FirstOrDefault();

                            var employee = m_EmployeeContext.Employees.Where(emp => emp.EmployeeId == employeeSkillDetails.EmployeeId && emp.IsActive == true).FirstOrDefault();
                            if (employee.DepartmentId == 1)
                            {
                                var allocation = m_projectService.GetAssociateAllocationsByEmployeeId(employeeSkillDetails.EmployeeId).Result;
                                if (!allocation.IsSuccessful)
                                {
                                    response.IsSuccessful = false;
                                    return response;
                                }
                                submittedTo = allocation.Items.Where(x => x.IsPrimary == true).Select(x => x.LeadId).FirstOrDefault();
                            }
                            else
                            {
                                submittedTo = employee.ReportingManager;
                            }
                            //if the record is approved state and associate wants to update ->updated record will add as new , existing record will be Inactive.
                            if (isExistInWorkFlow != null)
                            {
                                var employeeSkillWorkFlow = new EmployeeSkillWorkFlow();
                                employeeSkillWorkFlow.EmployeeSkillId = (int)skill.Id;
                                employeeSkillWorkFlow.Status = (int)SkillStatuscode.Created;
                                employeeSkillWorkFlow.SubmittedRating = employeeSkillDetails.ProficiencyLevelId;
                                employeeSkillWorkFlow.SubmittedBy = employeeSkillDetails.EmployeeId;
                                employeeSkillWorkFlow.SubmittedTo = (int)submittedTo;
                                employeeSkillWorkFlow.Experience = ConvertExperienceInMonths(employeeSkillDetails.Experience);
                                employeeSkillWorkFlow.LastUsed = employeeSkillDetails.LastUsed;
                                m_EmployeeContext.EmployeeSkillWorkFlow.Add(employeeSkillWorkFlow);

                                //updating existing record as inactive.
                                isExistInWorkFlow.IsActive = false;
                                m_EmployeeContext.EmployeeSkillWorkFlow.Update(isExistInWorkFlow);
                            }
                            await m_EmployeeContext.SaveChangesAsync();
                            response.IsSuccessful = true;
                            response.Item = skill;
                        }
                    }
                    await dbContext.CommitAsync();                    
                }
                catch (Exception ex)
                {
                    dbContext.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while updating employee skill details";
                    m_Logger.LogError("Error occurred in \"Update\" method of EmployeeSkillService" + ex.StackTrace);
                }
            }
            return response;
        }
        #endregion

        public async Task<ServiceResponse<bool>> DeleteSkill(int id)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                var skill = await m_EmployeeContext.EmployeeSkills.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (skill == null)
                {
                    response.IsSuccessful = false;
                    response.Message = $"No skill found with the Id requested";
                }
                

                m_EmployeeContext.EmployeeSkills.Remove(skill);
                int isDeleted = await m_EmployeeContext.SaveChangesAsync();
                if (isDeleted == 1)
                {
                    response.IsSuccessful = true;
                    response.Item = true;

                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = true;
                response.Message = $"Error occurred while deleting the skill";
            }
            return response;
        }

        private async Task<ServiceResponse<bool>> AddOrUpdateSkillVersion(string Version, int employeeSkillId, int employeeId)
            {
            var response = new ServiceResponse<bool>();
            string isValidationMessage = string.Empty ;
            List<string> Versions = Utility.SplitStringByCommaToList(Version);
            //Validation of version input
            isValidationMessage = VerisonValidation(Versions);
            if (!string.IsNullOrEmpty(isValidationMessage))
            {
                response.Message = isValidationMessage;
                response.IsSuccessful = false;
                return response;
            }
            else { 
                foreach (string version in Versions)
                {
                    var isVersionExist = m_EmployeeContext.SkillVersion.Where(versn => versn.EmployeeSkillId == employeeSkillId && versn.Version == version).FirstOrDefault();
                    if (isVersionExist == null)
                    {
                        SkillVersion skillVersion = new SkillVersion
                        {
                            EmployeeSkillId = employeeSkillId,
                            EmployeeId = employeeId,
                            Version = version.Trim()
                        };
                        m_EmployeeContext.SkillVersion.Add(skillVersion);
                    }
                }
            }
           
            var versionsForRemove = m_EmployeeContext.SkillVersion.Where(version =>version.EmployeeSkillId==employeeSkillId && !Versions.Contains(version.Version)).ToList();
            m_EmployeeContext.RemoveRange(versionsForRemove);
            await m_EmployeeContext.SaveChangesAsync();          
            response.IsSuccessful = true;
            
            return response;
        }
        private string VerisonValidation(List<string> Versions)
        {
            bool isMateched=false;
            string message = string.Empty;
            foreach (string version in Versions)
            {
               isMateched = version == "0" || version == "0.0" || version == "00" || version == "00.00" || version == "00.0" || version == "0.00" ? false : true;
                if (isMateched)
                {
                    isMateched = TypeCheck(version);
                    if (!isMateched)
                    {
                        var items = version.Split('.');
                        foreach (var i in items)
                        {
                            isMateched = TypeCheck(i);
                            if (!isMateched)
                            {
                                return "Version is not valid " + version;
                            }
                        }
                    }
                }
                else
                {
                    return "Version is not valid " + version;
                }
            }
           return message;
        }
        private bool TypeCheck(string value)
        {
            bool isIntType = int.TryParse(value, out _);
            bool isDecimaltype = decimal.TryParse(value, out _);
            bool isMateched = isIntType || isDecimaltype ? true : false;
            return isMateched;
        }

        private int ConvertExperienceInMonths(decimal? experience)
        {
            int experienceInMonths=0;
            if (experience != null)
            {
                List<int> experienceList = experience.ToString().Split('.').Select(x => Convert.ToInt32(x)).ToList();
                if (experienceList.Count() > 1)
                {
                    experienceInMonths = (experienceList[0] * 12) + experienceList[1];
                }
                else
                {
                    experienceInMonths = (experienceList[0] * 12);
                }
            }
            return experienceInMonths;
        }
        private decimal ConvertExperienceInYears(int? experience)
        {
            decimal experienceInYears=0;
            if (experience != null)
            {
                experienceInYears =Convert.ToDecimal(Convert.ToDecimal(experience/12)+ "."+ Convert.ToDecimal(experience) % 12);
            }
            return experienceInYears;
        }

        private bool ValidateExperience(decimal? experience)
        {
            bool isValid = false;
            if(experience!=null)
            {
                var exp = experience.Value.ToString().Split(".").Select(x=>Convert.ToInt32(x)).ToList();
                if (exp.Count() > 1)
                {
                    isValid = exp[1] >= 0 && exp[1] <= 11 ? true : false;
                }
                else
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        private bool IsSkillAvailable(int? employeeSkillId)
        {
            bool isExist=false;
            if (employeeSkillId != null)
            {
                var isSkillExist = m_EmployeeContext.EmployeeSkillWorkFlow.Where(skillWF => skillWF.EmployeeSkillId == employeeSkillId && skillWF.Status == (int)SkillStatuscode.Approved).ToList();
                 isExist = isSkillExist.Count() > 0 ? true : false;
            }
            return isExist;
        }

    }
}
