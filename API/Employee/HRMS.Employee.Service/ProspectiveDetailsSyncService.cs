using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Employee.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.Http;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Types;
using System.Net.Mail;
using HRMS.Employee.Types.External;
using HRMS.Common.Enums;
using HRMS.Employee.Infrastructure;
using Microsoft.Extensions.Options;

namespace HRMS.Employee.Service
{
    public class ProspectiveDetailsSyncService : IProspectiveDetailsSyncService
    {
        #region Global Variables
        private readonly ILogger<ProspectiveDetailsSyncService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;
        private readonly IConfiguration m_configuration;
        private readonly IEmployeeSkillWorkFlow m_employeeSkillWorkFlow;
        private readonly IOrganizationService m_OrgService;
        private readonly IProjectService m_ProjectService;
        private readonly MigrationEmailConfigurations m_MigrationEmailConfigurations;
        #endregion

        #region ProspectiveSubmissionService
        public ProspectiveDetailsSyncService(ILogger<ProspectiveDetailsSyncService> logger,
                    EmployeeDBContext employeeDBContext,
                    IConfiguration configuration,
                    IEmployeeSkillWorkFlow employeeSkillWorkFlow,
                    IOrganizationService orgService,
                    IProjectService projectService,
                    IOptions<MigrationEmailConfigurations> migrationEmailConfigurations)
        {
            m_Logger = logger;
            m_EmployeeContext = employeeDBContext;
            m_configuration = configuration;
            m_employeeSkillWorkFlow = employeeSkillWorkFlow;
            m_OrgService = orgService;
            m_ProjectService = projectService;
            m_MigrationEmailConfigurations = migrationEmailConfigurations.Value;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EmployeeProjectResponse, EmployeeProject>();
                cfg.CreateMap<AssociateCertificationsResponse, AssociateCertification>();
                cfg.CreateMap<AssociatesMembershipResponse, AssociateMembership>();
                cfg.CreateMap<EmergencyContactDetailsResponse, EmergencyContactDetails>();
                cfg.CreateMap<FamilyDetailsResponse, FamilyDetails>();
                cfg.CreateMap<PersonalInformationResponse, HRMS.Employee.Entities.Employee>();
                cfg.CreateMap<PersonalInformationResponse, HRMS.Employee.Entities.Contacts>();
                cfg.CreateMap<PreviousEmploymentDetailsResponse, PreviousEmploymentDetails>();
                cfg.CreateMap<ProfessionalReferencesResponse, ProfessionalReferences>();
                cfg.CreateMap<EmployeeSkillResponse, EmployeeSkill>();
                cfg.CreateMap<EducationDetailsResponse, EducationDetails>();

            });
            m_mapper = config.CreateMapper();

        }
        #endregion

        #region SyncDataFromRepository
        /// <summary>
        /// Read Repository file details
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> ReadRepository()
        {
            int isUpdated;
            int isCreated = 0;
            string filepath = m_MigrationEmailConfigurations.TobeprocessedRepoPath;
            string movefile = m_MigrationEmailConfigurations.ProcessedRepoPath;
            string[] files = Directory.GetFiles(filepath);

            var response = new ServiceResponse<bool>();
            m_Logger.LogInformation("ProspectiveDetailsSyncService: Calling \"ReadRepository\" method.");

            using (var dbContext = m_EmployeeContext.Database.BeginTransaction())
            {
                try
                {
                    if (files.Count() > 0)
                    {
                        foreach (var file in files)
                        {
                            string text = File.ReadAllText(file);

                            var person = JsonSerializer.Deserialize<PersonalDetailsResponse>(text);
                            if (person != null)
                            {
                                var employee = new Entities.Employee();
                                var contactsOne = new Contacts();
                                var contactsTwo = new Contacts();

                                if (person.PersonalDetail != null)
                                {
                                    m_mapper.Map<PersonalInformationResponse, Entities.Employee>(person.PersonalDetail, employee);
                                    employee.PersonalEmailAddress = person.PersonalDetail.PersonalEmailId;
                                    employee.MobileNo = person.PersonalDetail.MobileNumber;
                                    employee.JoinDate = person.PersonalDetail.JoiningDate;
                                    employee.IsActive = true;
                                    employee.StatusId = (int)EPCStatusCode.Approved;
                                    m_EmployeeContext.Employees.Add(employee);
                                    isUpdated = await m_EmployeeContext.SaveChangesAsync();


                                    m_mapper.Map<PersonalInformationResponse, Entities.Contacts>(person.PersonalDetail, contactsOne);
                                    contactsOne.EmployeeId = employee.EmployeeId;
                                    contactsOne.AddressType = "CurrentAddress";
                                    contactsOne.AddressLine1 = person.PersonalDetail.CurrentAddressLine1;
                                    contactsOne.AddressLine2 = person.PersonalDetail.CurrentAddressLine2;
                                    contactsOne.Country = person.PersonalDetail.CurrentCountry;
                                    contactsOne.City = person.PersonalDetail.CurrentCity;
                                    contactsOne.State = person.PersonalDetail.CurrentState;
                                    contactsOne.PostalCode = person.PersonalDetail.CurrentZip;
                                    contactsOne.IsActive = true;
                                    m_EmployeeContext.Contacts.Add(contactsOne);
                                    isUpdated = await m_EmployeeContext.SaveChangesAsync();

                                    m_mapper.Map<PersonalInformationResponse, HRMS.Employee.Entities.Contacts>(person.PersonalDetail, contactsTwo);
                                    contactsTwo.EmployeeId = employee.EmployeeId;
                                    contactsTwo.AddressType = "PermanentAddress";
                                    contactsTwo.AddressLine1 = person.PersonalDetail.PermanentAddressLine1;
                                    contactsTwo.AddressLine2 = person.PersonalDetail.PermanentAddressLine2;
                                    contactsTwo.Country = person.PersonalDetail.PermanentCountry;
                                    contactsTwo.City = person.PersonalDetail.PermanentCity;
                                    contactsTwo.State = person.PersonalDetail.PermanentState;
                                    contactsTwo.PostalCode = person.PersonalDetail.PermanentZip;
                                    contactsTwo.IsActive = true;
                                    m_EmployeeContext.Contacts.Add(contactsTwo);
                                    isUpdated = await m_EmployeeContext.SaveChangesAsync();
                                }

                                if (person.Family != null)
                                {

                                    foreach (var fam in person.Family)
                                    {
                                        var family = new FamilyDetails();
                                        m_mapper.Map<FamilyDetailsResponse, HRMS.Employee.Entities.FamilyDetails>(fam, family);
                                        family.EmployeeId = employee.EmployeeId;
                                        m_EmployeeContext.FamilyDetails.Add(family);
                                    }
                                }

                                if (person.EmergencyContact != null)
                                {
                                    foreach (var em in person.EmergencyContact)
                                    {
                                        var emergencyContacts = new EmergencyContactDetails();
                                        m_mapper.Map<EmergencyContactDetailsResponse, EmergencyContactDetails>(em, emergencyContacts);
                                        emergencyContacts.EmployeeId = employee.EmployeeId;
                                        m_EmployeeContext.EmergencyContactDetails.Add(emergencyContacts);
                                    }
                                }

                                if (person.Education != null)
                                {
                                    foreach (var edu in person.Education)
                                    {
                                        var education = new EducationDetails();
                                        m_mapper.Map<EducationDetailsResponse, EducationDetails>(edu, education);
                                        education.EmployeeId = employee.EmployeeId;
                                        m_EmployeeContext.EducationDetails.Add(education);
                                    }
                                }

                                if (person.Projects != null)
                                {
                                    foreach (var proj in person.Projects)
                                    {
                                        var projects = new EmployeeProject();
                                        m_mapper.Map<EmployeeProjectResponse, EmployeeProject>(proj, projects);
                                        projects.EmployeeId = employee.EmployeeId;
                                        m_EmployeeContext.EmployeeProjects.Add(projects);
                                    }
                                }

                                if (person.AssociateCertification != null)
                                {
                                    foreach (var cert in person.AssociateCertification)
                                    {
                                        var certification = new AssociateCertification();
                                        m_mapper.Map<AssociateCertificationsResponse, AssociateCertification>(cert, certification);
                                        certification.EmployeeId = employee.EmployeeId;
                                        m_EmployeeContext.AssociateCertifications.Add(certification);
                                    }
                                }

                                if (person.AssociateMembership != null)
                                {
                                    foreach (var mem in person.AssociateMembership)
                                    {
                                        var membership = new AssociateMembership();
                                        m_mapper.Map<AssociatesMembershipResponse, AssociateMembership>(mem, membership);
                                        membership.EmployeeId = employee.EmployeeId;
                                        m_EmployeeContext.AssociateMemberships.Add(membership);
                                    }
                                }

                                if (person.PreviousEmployeeDetails != null)
                                {
                                    foreach (var prev in person.PreviousEmployeeDetails)
                                    {
                                        var prevEmployment = new PreviousEmploymentDetails();
                                        m_mapper.Map<PreviousEmploymentDetailsResponse, PreviousEmploymentDetails>(prev, prevEmployment);
                                        prevEmployment.EmployeeId = employee.EmployeeId;
                                        m_EmployeeContext.PreviousEmploymentDetails.Add(prevEmployment);
                                    }
                                }

                                if (person.ProfessionalReference != null)
                                {
                                    foreach (var prof in person.ProfessionalReference)
                                    {
                                        var professionalRef = new ProfessionalReferences();
                                        m_mapper.Map<ProfessionalReferencesResponse, ProfessionalReferences>(prof, professionalRef);
                                        professionalRef.EmployeeId = employee.EmployeeId;
                                        m_EmployeeContext.ProfessionalReferences.Add(professionalRef);
                                    }
                                }

                                if (person.Skills != null)
                                {
                                    foreach (var skill in person.Skills)
                                    {
                                        var skills = new EmployeeSkill();
                                        m_mapper.Map<EmployeeSkillResponse, EmployeeSkill>(skill, skills);
                                        skills.EmployeeId = employee.EmployeeId;
                                        m_EmployeeContext.EmployeeSkills.Add(skills);
                                        isCreated = await m_EmployeeContext.SaveChangesAsync();
                                        if (isCreated > 0)
                                        {
                                            var result = (await m_employeeSkillWorkFlow.Create(employee.EmployeeId, true)).IsSuccessful;
                                        }
                                    }
                                }

                                // Create a Talent Pool record if the associate has CompetencyGroup
                                EmployeeDetails employeeDetails = new EmployeeDetails();
                                employeeDetails.EmployeeId = employee.EmployeeId;
                                employeeDetails.IsDepartmentChange = false;
                                employeeDetails.JoinDate = employee.JoinDate;
                                employeeDetails.CompetencyGroup = employee.CompetencyGroup;
                                if (employee.CompetencyGroup != null)
                                {
                                    var allocation = await m_ProjectService.AllocateAssociateToTalentPool(employeeDetails);
                                    if (!allocation.IsSuccessful)
                                    {
                                        response.IsSuccessful = false;
                                        response.Message = "Error occurred while allocating employee to talent pool";
                                        return response;
                                    }
                                }


                                await m_EmployeeContext.SaveChangesAsync();

                                await dbContext.CommitAsync();

                                response.IsSuccessful = true;
                                response.Message = "Successfully Migrated to HRMS";
                                response.Item = true;

                                NotificationDetail notificationDetail = new NotificationDetail();
                                notificationDetail.ToEmail = m_MigrationEmailConfigurations.ToEmail;
                                notificationDetail.CcEmail = m_MigrationEmailConfigurations.CcEmail;
                                notificationDetail.FromEmail = m_MigrationEmailConfigurations.FromEmail;
                                notificationDetail.EmailBody = "<html> Hi Team,<br/><br/>" + " " + person.PersonalDetail.FirstName + " " + person.PersonalDetail.LastName + " Personal Details has been migrated to HRMS.<br/><br/> Thank you";
                                notificationDetail.Subject = "HRMS Data Migration for " + person.PersonalDetail.FirstName + " " + person.PersonalDetail.LastName;
                                if (Convert.ToBoolean(m_MigrationEmailConfigurations.SendEmail))
                                {
                                    var emailStatus = await m_OrgService.SendEmail(notificationDetail);
                                }
                                var filename = Path.GetFileName(file);
                                var processedFilePath = Path.Combine(movefile, filename);
                                File.Copy(file, processedFilePath);
                                File.Delete(file);
                            }
                            else
                            {
                                response.Message = "No data found to process";
                                response.IsSuccessful = false;
                            }
                        }
                    }
                    else
                    {
                        response.IsSuccessful = true;
                        response.Message = "No Files to Process";

                    }
                    return response;
                }
                catch (Exception ex)
                {
                    dbContext.Rollback();
                    response.IsSuccessful = false;
                    m_Logger.LogError("Error occurred in \"ReadRepository\" of ProspectiveDetailsSyncService " + ex.StackTrace);
                }
            }
            return response;

        }
        #endregion

    }
}