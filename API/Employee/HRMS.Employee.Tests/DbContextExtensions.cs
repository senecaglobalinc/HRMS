using HRMS.Employee.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Tests
{
    public static class DbContextExtensions
    {
        public static void Seed(this EmployeeDBContext dbContext)
        {
            // Add entities for DbContext instance

            #region [    EmployeeType    ]
            dbContext.EmployeeTypes.Add(new Entities.EmployeeType
            {
                EmployeeTypeId = 1,
                EmployeeTypeCode = "FTE",
                EmpType = "FTE",
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            dbContext.EmployeeTypes.Add(new Entities.EmployeeType
            {
                EmployeeTypeId = 2,
                EmployeeTypeCode = "Contractors",
                EmpType = "Contractors",
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });
            #endregion        

            #region [    ProspectiveAssociate    ]
            dbContext.ProspectiveAssociates.Add(new Entities.ProspectiveAssociate
            {
                Id = 1,
                FirstName = "Susil",
                LastName = "Misra",
                MiddleName = null,
                Gender = null,
                MaritalStatus = null,
                MobileNo = "9035051561",
                PersonalEmailAddress = "susilmorcl@gmail.com",
                JoiningStatusId = null,
                GradeId = 1,
                EmploymentType = "FTE",
                Technology = "Helpdesk (ITS)",
                TechnologyID = 1,
                DesignationId = 1,
                DepartmentId = 1,
                JoinDate = null,
                HRAdvisorName = "Sripradha Gulla",
                BGVStatusId = null,
                RecruitedBy = "Pavani Bujala",
                EmployeeID = 0,
                ManagerId = 214
            });

            dbContext.ProspectiveAssociates.Add(new Entities.ProspectiveAssociate
            {
                Id = 2,
                FirstName = "manasa",
                LastName = "kothapalli",
                MiddleName = null,
                Gender = null,
                MaritalStatus = null,
                MobileNo = "666666666666",
                PersonalEmailAddress = "manasa@gmail.com",
                JoiningStatusId = null,
                GradeId = 1,
                EmploymentType = "FTE",
                Technology = "Helpdesk (ITS)",
                TechnologyID = 1,
                DesignationId = 1,
                DepartmentId = 1,
                JoinDate = null,
                HRAdvisorName = "Sripradha Gulla",
                BGVStatusId = null,
                RecruitedBy = "Pavani Bujala",
                EmployeeID = 0,
                ManagerId = 214
            });

            dbContext.ProspectiveAssociates.Add(new Entities.ProspectiveAssociate
            {
                Id = 3,
                FirstName = "yagna",
                LastName = "bhargavi",
                MiddleName = null,
                Gender = null,
                MaritalStatus = null,
                MobileNo = "9035011561",
                PersonalEmailAddress = "yagna@gmail.com",
                JoiningStatusId = null,
                GradeId = 1,
                EmploymentType = "FTE",
                Technology = "Helpdesk (ITS)",
                TechnologyID = 1,
                DesignationId = 1,
                DepartmentId = 1,
                JoinDate = null,
                HRAdvisorName = "Sripradha Gulla",
                BGVStatusId = null,
                RecruitedBy = "Pavani Bujala",
                EmployeeID = 0,
                ManagerId = 214
            });

            dbContext.ProspectiveAssociates.Add(new Entities.ProspectiveAssociate
            {
                Id = 4,
                FirstName = "test4",
                LastName = "test4",
                MiddleName = null,
                Gender = null,
                MaritalStatus = null,
                MobileNo = "9035071561",
                PersonalEmailAddress = "test4@gmail.com",
                JoiningStatusId = null,
                GradeId = 1,
                EmploymentType = "FTE",
                Technology = "Helpdesk (ITS)",
                TechnologyID = 1,
                DesignationId = 1,
                DepartmentId = 1,
                JoinDate = null,
                HRAdvisorName = "Sripradha Gulla",
                BGVStatusId = null,
                RecruitedBy = "Pavani Bujala",
                EmployeeID = 0,
                ManagerId = 214
            });

            dbContext.ProspectiveAssociates.Add(new Entities.ProspectiveAssociate
            {
                Id = 5,
                FirstName = "test5",
                LastName = "test5",
                MiddleName = null,
                Gender = null,
                MaritalStatus = null,
                MobileNo = "9035081561",
                PersonalEmailAddress = "test5@gmail.com",
                JoiningStatusId = null,
                GradeId = 1,
                EmploymentType = "FTE",
                Technology = "Helpdesk (ITS)",
                TechnologyID = 1,
                DesignationId = 1,
                DepartmentId = 1,
                JoinDate = null,
                HRAdvisorName = "Sripradha Gulla",
                BGVStatusId = null,
                RecruitedBy = "Pavani Bujala",
                EmployeeID = 0,
                ManagerId = 214
            });

            dbContext.ProspectiveAssociates.Add(new Entities.ProspectiveAssociate
            {
                Id = 6,
                FirstName = "test6",
                LastName = "test6",
                MiddleName = null,
                Gender = null,
                MaritalStatus = null,
                MobileNo = "9035055561",
                PersonalEmailAddress = "test6@gmail.com",
                JoiningStatusId = null,
                GradeId = 1,
                EmploymentType = "FTE",
                Technology = "Helpdesk (ITS)",
                TechnologyID = 1,
                DesignationId = 1,
                DepartmentId = 1,
                JoinDate = null,
                HRAdvisorName = "Sripradha Gulla",
                BGVStatusId = null,
                RecruitedBy = "Pavani Bujala",
                EmployeeID = 0,
                ManagerId = 214
            });
            #endregion

            #region [    EducationDetails    ]
            dbContext.EducationDetails.Add(new Entities.EducationDetails
            {
                Id = 1,
                EmployeeId = 1,
                EducationalQualification = "Graduation",
                AcademicCompletedYear = null,
                Institution = "JNTUK",
                Specialization = "B.Tech CSE",
                ProgramType = "FullTime",
                Grade = "Percentage",
                Marks = "76",
                AcademicYearFrom = null,
                AcademicYearTo = null,
                ProgramTypeId = 0,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            dbContext.EducationDetails.Add(new Entities.EducationDetails
            {
                Id = 2,
                EmployeeId = 2,
                EducationalQualification = "PostGraduation",
                AcademicCompletedYear = null,
                Institution = "JNTUK",
                Specialization = "B.Tech CSE",
                ProgramType = "FullTime",
                Grade = "Percentage",
                Marks = "80",
                AcademicYearFrom = null,
                AcademicYearTo = null,
                ProgramTypeId = 1,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });
            #endregion

            #region [   EmployeeProject   ]
            dbContext.EmployeeProjects.Add(new Entities.EmployeeProject()
            {
                Id = 0,
                EmployeeId = 3,
                DomainId = 1,
                ProjectName = "HRMS",
                OrganizationName = "Seneca",
                KeyAchievements = "",
                Duration = 12,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.EmployeeProjects.Add(new Entities.EmployeeProject()
            {
                Id = 0,
                EmployeeId = 3,
                DomainId = 2,
                ProjectName = "VTS",
                OrganizationName = "Seneca",
                KeyAchievements = "",
                Duration = 12,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            #endregion

            #region   [  EmployeeSkill   ]
            dbContext.EmployeeSkills.Add(new Entities.EmployeeSkill
            {
                Id = 1,
                EmployeeId = 3,
                LastUsed = 2019,
                IsPrimary = true,
                Experience = 12,
                SkillId = 1,
                SkillGroupId = 1,
                ProficiencyLevelId = 1,
                CompetencyAreaId = 1
            });
            dbContext.EmployeeSkills.Add(new Entities.EmployeeSkill
            {
                Id = 2,
                EmployeeId = 3,
                LastUsed = 2019,
                IsPrimary = true,
                Experience = 12,
                SkillId = 2,
                SkillGroupId = 1,
                ProficiencyLevelId = 1,
                CompetencyAreaId = 1
            });

            #endregion

            #region [   PreviousEmploymentDetails    ]
            dbContext.PreviousEmploymentDetails.Add(new Entities.PreviousEmploymentDetails
            {
                 Id = 1,
                 EmployeeId = 1,
                 Name = "Name1",
                 Address = "Hyderabad",
                 Designation = "Database Administrator",
                 ServiceFrom = null,
                 ServiceTo = null,
                 LeavingReason = "Reason1",
                 CreatedDate = null,
                 ModifiedDate = null,
                 SystemInfo = null,
                 IsActive = true,
                 CreatedBy = null,
                 ModifiedBy = null
            });

            dbContext.PreviousEmploymentDetails.Add(new Entities.PreviousEmploymentDetails
            {
                Id = 2,
                EmployeeId = 2,
                Name = "Name2",
                Address = "Hyderabad",
                Designation = "Lead",
                ServiceFrom = null,
                ServiceTo = null,
                LeavingReason = "Reason2",
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });
            #endregion

            #region [   ProfessionalReferences    ]
            dbContext.ProfessionalReferences.Add(new Entities.ProfessionalReferences
            {
                 Id = 1,
                 EmployeeId = 1,
                 Name = "Name1",
                 Designation = "Sr. Manager",
                 CompanyName = "DST World Wide",
                 CompanyAddress = "Hyderabad",
                 OfficeEmailAddress = "xxx@gmail.com",
                 MobileNo = "9550326770",
                 CreatedDate = null,
                 ModifiedDate = null,
                 SystemInfo = null,
                 IsActive = true,
                 CreatedBy =  null,
                 ModifiedBy = null
            });

            dbContext.ProfessionalReferences.Add(new Entities.ProfessionalReferences
            {
                Id = 2,
                EmployeeId = 2,
                Name = "Name2",
                Designation = "Sr. Manager",
                CompanyName = "DST World Wide",
                CompanyAddress = "Hyderabad",
                OfficeEmailAddress = "xxx@gmail.com",
                MobileNo = "9550326770",
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });
            #endregion        

            #region [   AssociateExitInterview    ]
            dbContext.AssociateExitInterview.Add(new Entities.AssociateExitInterview
            {
                AssociateExitInterviewId = 1,
                CreatedDate = DateTime.Now.Date.AddDays(-2).Date,
                AssociateExitId = 385,
                Remarks = "/tS/tcicwSnvv3UXyXoXMqL1NtYjtagtxxSwxMyW7io="
            });

            dbContext.AssociateExitInterview.Add(new Entities.AssociateExitInterview
            {
                AssociateExitInterviewId = 2,
                CreatedDate = DateTime.Now,
                AssociateExitId = 135,
                Remarks = "2GsmKUSQ5DIHNAd5VtpcGw=="
            });
            #endregion

            #region [   AssociateExit    ]
            dbContext.AssociateExit.Add(new Entities.AssociateExit { 
                AssociateExitId = 385,
                EmployeeId = 1357
            });

            dbContext.AssociateExit.Add(new Entities.AssociateExit
            {
                AssociateExitId = 135,
                EmployeeId = 1121
            });
            #endregion

            dbContext.SaveChanges();
        }

    }
}
