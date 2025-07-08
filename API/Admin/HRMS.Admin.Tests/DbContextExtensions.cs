using HRMS.Admin.Database;
using System;

namespace HRMS.Admin.Tests
{
    public static class DbContextExtensions
    {
        public static void Seed(this AdminContext dbContext)
        {
            // Add entities for DbContext instance

            #region [    Category Master   ]

            dbContext.Categories.Add(new Entities.CategoryMaster
            {
                CategoryMasterId = 1,
                CategoryName = "AssociateExit",
                ParentId = 0,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Categories.Add(new Entities.CategoryMaster
            {
                CategoryMasterId = 2,
                CategoryName = "CM00002",
                ParentId = 0,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Categories.Add(new Entities.CategoryMaster
            {
                CategoryMasterId = 3,
                CategoryName = "CM00003",
                ParentId = 0,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Categories.Add(new Entities.CategoryMaster
            {
                CategoryMasterId = 4,
                CategoryName = "CM00004",
                ParentId = 0,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Categories.Add(new Entities.CategoryMaster
            {
                CategoryMasterId = 5,
                CategoryName = "CM00005",
                ParentId = 0,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [    Client    ]

            dbContext.Clients.Add(new Entities.Client
            {
                ClientId = 1,
                ClientCode = "Test",
                ClientName = "Test",
                ClientRegisterName = "1",
                ClientNameHash = "0x1c77599f2a80e2373565bb53c9bc8354bbb6018b",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Clients.Add(new Entities.Client
            {
                ClientId = 2,
                ClientCode = "Test1",
                ClientName = "Test1",
                ClientRegisterName = "2",
                ClientNameHash = "0x1c77599f2a80e2373565bb53c9bc8354bbb6018b",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Clients.Add(new Entities.Client
            {
                ClientId = 3,
                ClientCode = "Test2",
                ClientName = "Test2",
                ClientRegisterName = "3",
                ClientNameHash = "0x1c77599f2a80e2373565bb53c9bc8354bbb6018b",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Clients.Add(new Entities.Client
            {
                ClientId = 4,
                ClientCode = "Test3",
                ClientName = "Test3",
                ClientRegisterName = "3",
                ClientNameHash = "0x1c77599f2a80e2373565bb53c9bc8354bbb6018b",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Clients.Add(new Entities.Client
            {
                ClientId = 5,
                ClientCode = "Test4",
                ClientName = "Test4",
                ClientRegisterName = "5",
                ClientNameHash = "0x1c77599f2a80e2373565bb53c9bc8354bbb6018b",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Clients.Add(new Entities.Client
            {
                ClientId = 6,
                ClientCode = "Test5",
                ClientName = "Test5",
                ClientRegisterName = "6",
                ClientNameHash = "0x1c77599f2a80e2373565bb53c9bc8354bbb6018b",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Clients.Add(new Entities.Client
            {
                ClientId = 7,
                ClientCode = "Test6",
                ClientName = "Test6",
                ClientRegisterName = "7",
                ClientNameHash = "0x1c77599f2a80e2373565bb53c9bc8354bbb6018b",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Clients.Add(new Entities.Client
            {
                ClientId = 8,
                ClientCode = "Test7",
                ClientName = "Test7",
                ClientRegisterName = "8",
                ClientNameHash = "0x1c77599f2a80e2373565bb53c9bc8354bbb6018b",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Clients.Add(new Entities.Client
            {
                ClientId = 9,
                ClientCode = "Test8",
                ClientName = "Test8",
                ClientRegisterName = "9",
                ClientNameHash = "0x1c77599f2a80e2373565bb53c9bc8354bbb6018b",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Clients.Add(new Entities.Client
            {
                ClientId = 10,
                ClientCode = "Test9",
                ClientName = "Test9",
                ClientRegisterName = "106",
                ClientNameHash = "0x1c77599f2a80e2373565bb53c9bc8354bbb6018b",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Clients.Add(new Entities.Client
            {
                ClientId = 11,
                ClientCode = "SenecaGlobal",
                ClientName = "SenecaGlobal",
                ClientRegisterName = "106",
                ClientNameHash = "0x1c77599f2a80e2373565bb53c9bc8354bbb6018b",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [    Competency Area    ]

            dbContext.CompetencyAreas.Add(new Entities.CompetencyArea
            {
                CompetencyAreaId = 1,
                CompetencyAreaCode = "C00001",
                CompetencyAreaDescription = "Competency Area 1",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.CompetencyAreas.Add(new Entities.CompetencyArea
            {
                CompetencyAreaId = 2,
                CompetencyAreaCode = "C00002",
                CompetencyAreaDescription = "Competency Area 2",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.CompetencyAreas.Add(new Entities.CompetencyArea
            {
                CompetencyAreaId = 3,
                CompetencyAreaCode = "C00003",
                CompetencyAreaDescription = "Competency Area 3",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.CompetencyAreas.Add(new Entities.CompetencyArea
            {
                CompetencyAreaId = 4,
                CompetencyAreaCode = "C00004",
                CompetencyAreaDescription = "Competency Area 4",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.CompetencyAreas.Add(new Entities.CompetencyArea
            {
                CompetencyAreaId = 5,
                CompetencyAreaCode = "C00005",
                CompetencyAreaDescription = "Competency Area 5",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.CompetencyAreas.Add(new Entities.CompetencyArea
            {
                CompetencyAreaId = 6,
                CompetencyAreaCode = "C00006",
                CompetencyAreaDescription = "Competency Area 6",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.CompetencyAreas.Add(new Entities.CompetencyArea
            {
                CompetencyAreaId = 7,
                CompetencyAreaCode = "C00007",
                CompetencyAreaDescription = "Competency Area 7",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.CompetencyAreas.Add(new Entities.CompetencyArea
            {
                CompetencyAreaId = 8,
                CompetencyAreaCode = "C00008",
                CompetencyAreaDescription = "Competency Area 8",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [   Department   ]
            dbContext.Departments.Add(new Entities.Department
            {
                DepartmentId = 1,
                DepartmentCode = "HR",
                Description = "HR",
                DepartmentTypeId = 2,
                DepartmentHeadId = 213,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.Departments.Add(new Entities.Department
            {
                DepartmentId = 2,
                DepartmentCode = "Admin",
                Description = "Admin",
                DepartmentTypeId = 2,
                DepartmentHeadId = 213,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            }); dbContext.Departments.Add(new Entities.Department
            {
                DepartmentId = 3,
                DepartmentCode = "Delivary",
                Description = "Delivary",
                DepartmentTypeId = 1,
                DepartmentHeadId = 213,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            }); dbContext.Departments.Add(new Entities.Department
            {
                DepartmentId = 4,
                DepartmentCode = "Finance",
                Description = "Finance",
                DepartmentTypeId = 2,
                DepartmentHeadId = 213,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            #endregion

            #region [   DepartmentType   ]
            dbContext.DepartmentTypes.Add(new Entities.DepartmentType
            {
                DepartmentTypeId = 1,
                DepartmentTypeDescription = "Delivary",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.DepartmentTypes.Add(new Entities.DepartmentType
            {
                DepartmentTypeId = 2,
                DepartmentTypeDescription = "Services",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            #endregion

            #region [    Designation    ]
            dbContext.Designations.Add(new Entities.Designation
            {
                DesignationId = 1,
                DesignationCode = "Test1",
                DesignationName = " Name1",
                GradeId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            dbContext.Designations.Add(new Entities.Designation
            {
                DesignationId = 2,
                DesignationCode = "Test2",
                DesignationName = "Name2",
                GradeId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            dbContext.Designations.Add(new Entities.Designation
            {
                DesignationId = 3,
                DesignationCode = "Test3",
                DesignationName = " Name3",
                GradeId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            dbContext.Designations.Add(new Entities.Designation
            {
                DesignationId = 4,
                DesignationCode = "Test4",
                DesignationName = " Name4",
                GradeId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            dbContext.Designations.Add(new Entities.Designation
            {
                DesignationId = 5,
                DesignationCode = "Test5",
                DesignationName = " Name5",
                GradeId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            dbContext.Designations.Add(new Entities.Designation
            {
                DesignationId = 6,
                DesignationCode = "Test6",
                DesignationName = " Name6",
                GradeId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });
            #endregion

            #region [   Domain   ]
            dbContext.Domains.Add(new Entities.Domain
            {
                DomainID = 1,
                DomainName = "HealthCare",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.Domains.Add(new Entities.Domain
            {
                DomainID = 2,
                DomainName = "Finance",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.Domains.Add(new Entities.Domain
            {
                DomainID = 3,
                DomainName = "Airlines",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.Domains.Add(new Entities.Domain
            {
                DomainID = 4,
                DomainName = "Navy",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            #endregion

            #region [    Grade    ]
            dbContext.Grades.Add(new Entities.Grade
            {
                GradeId = 5,
                GradeCode = "Test5",
                GradeName = "Test5",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            dbContext.Grades.Add(new Entities.Grade
            {
                GradeId = 6,
                GradeCode = "Test6",
                GradeName = "Test6",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            dbContext.Grades.Add(new Entities.Grade
            {
                GradeId = 7,
                GradeCode = "Test7",
                GradeName = "Test7",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            dbContext.Grades.Add(new Entities.Grade
            {
                GradeId = 8,
                GradeCode = "Test8",
                GradeName = "Test8",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            dbContext.Grades.Add(new Entities.Grade
            {
                GradeId = 9,
                GradeCode = "Test9",
                GradeName = "Test9",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            dbContext.Grades.Add(new Entities.Grade
            {
                GradeId = 10,
                GradeCode = "Test10",
                GradeName = "Test10",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });
            #endregion

            #region [   KeyFunction  ]
            dbContext.SGRoles.Add(new Entities.SGRole
            {
                SGRoleID = 1,
                SGRoleName = "Tester",
                DepartmentId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.SGRoles.Add(new Entities.SGRole
            {
                SGRoleID = 2,
                SGRoleName = "Developer",
                DepartmentId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [    Practice Area    ]

            dbContext.PracticeAreas.Add(new Entities.PracticeArea
            {
                PracticeAreaId = 1,
                PracticeAreaCode = "P00001",
                PracticeAreaDescription = "Practice Area 1",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.PracticeAreas.Add(new Entities.PracticeArea
            {
                PracticeAreaId = 2,
                PracticeAreaCode = "P00002",
                PracticeAreaDescription = "Practice Area 2",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.PracticeAreas.Add(new Entities.PracticeArea
            {
                PracticeAreaId = 3,
                PracticeAreaCode = "P00003",
                PracticeAreaDescription = "Practice Area 3",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.PracticeAreas.Add(new Entities.PracticeArea
            {
                PracticeAreaId = 4,
                PracticeAreaCode = "P00004",
                PracticeAreaDescription = "Practice Area 4",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.PracticeAreas.Add(new Entities.PracticeArea
            {
                PracticeAreaId = 5,
                PracticeAreaCode = "P00005",
                PracticeAreaDescription = "Practice Area 5",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [   ProficiencyLevel  ]
            dbContext.ProficiencyLevels.Add(new Entities.ProficiencyLevel
            {
                ProficiencyLevelId = 1,
                ProficiencyLevelCode = "Advance",
                ProficiencyLevelDescription = "Advance",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.ProficiencyLevels.Add(new Entities.ProficiencyLevel
            {
                ProficiencyLevelId = 2,
                ProficiencyLevelCode = "Expert",
                ProficiencyLevelDescription = "Expert",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.ProficiencyLevels.Add(new Entities.ProficiencyLevel
            {
                ProficiencyLevelId = 3,
                ProficiencyLevelCode = "Beginner",
                ProficiencyLevelDescription = "Beginner",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.ProficiencyLevels.Add(new Entities.ProficiencyLevel
            {
                ProficiencyLevelId = 4,
                ProficiencyLevelCode = "Basic",
                ProficiencyLevelDescription = "Basic",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            #endregion

            #region [  ProjectType]
            dbContext.ProjectTypes.Add(new Entities.ProjectType
            {
                ProjectTypeId = 1,
                ProjectTypeCode = "PZ00001",
                Description = "something",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.ProjectTypes.Add(new Entities.ProjectType
            {
                ProjectTypeId = 2,
                ProjectTypeCode = "PZ00002",
                Description = "something",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.ProjectTypes.Add(new Entities.ProjectType
            {
                ProjectTypeId = 3,
                ProjectTypeCode = "PZ00003",
                Description = "something",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.ProjectTypes.Add(new Entities.ProjectType
            {
                ProjectTypeId = 4,
                ProjectTypeCode = "PZ00004",
                Description = "something",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            #endregion

            #region [   Seniority   ]
            dbContext.SGRolePrefixes.Add(new Entities.SGRolePrefix
            {
                PrefixID = 1,
                PrefixName = "Senior",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.SGRolePrefixes.Add(new Entities.SGRolePrefix
            {
                PrefixID = 2,
                PrefixName = "Junior",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            #endregion

            #region [    Skill   ]

            dbContext.Skills.Add(new Entities.Skill
            {
                SkillId = 1,
                SkillName = "Test1",
                SkillCode = "code1",
                SkillDescription = "desc1",
                IsActive = true,
                IsApproved = true,
                CompetencyAreaId = 1,
                SkillGroupId = 1,
                CreatedBy = "Anonymous",
                ModifiedBy = null,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null

            });

            dbContext.Skills.Add(new Entities.Skill
            {
                SkillId = 2,
                SkillName = "Test2",
                SkillCode = "code2",
                SkillDescription = "desc2",
                IsActive = true,
                IsApproved = true,
                CompetencyAreaId = 2,
                SkillGroupId = 2,
                CreatedBy = "Anonymous",
                ModifiedBy = null,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null

            });

            dbContext.Skills.Add(new Entities.Skill
            {
                SkillId = 3,
                SkillName = "Test3",
                SkillCode = "code3",
                SkillDescription = "desc3",
                IsActive = true,
                IsApproved = true,
                CompetencyAreaId = 3,
                SkillGroupId = 3,
                CreatedBy = "Anonymous",
                ModifiedBy = null,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null

            });

            dbContext.Skills.Add(new Entities.Skill
            {
                SkillId = 4,
                SkillName = "Test4",
                SkillCode = "code4",
                SkillDescription = "desc4",
                IsActive = true,
                IsApproved = true,
                CompetencyAreaId = 4,
                SkillGroupId = 4,
                CreatedBy = "Anonymous",
                ModifiedBy = null,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null

            });

            dbContext.Skills.Add(new Entities.Skill
            {
                SkillId = 5,
                SkillName = "Test5",
                SkillCode = "code5",
                SkillDescription = "desc5",
                IsActive = true,
                IsApproved = true,
                CompetencyAreaId = 5,
                SkillGroupId = 5,
                CreatedBy = "Anonymous",
                ModifiedBy = null,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null

            });

            dbContext.Skills.Add(new Entities.Skill
            {
                SkillId = 6,
                SkillName = "Test6",
                SkillCode = "code6",
                SkillDescription = "desc6",
                IsActive = true,
                IsApproved = true,
                CompetencyAreaId = 6,
                SkillGroupId = 6,
                CreatedBy = "Anonymous",
                ModifiedBy = null,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null

            });

            #endregion

            #region [   Skill Group    ]

            dbContext.SkillGroups.Add(new Entities.SkillGroup
            {
                SkillGroupId = 1,
                SkillGroupName = "Test1",
                Description = "desc1",
                CompetencyAreaId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            dbContext.SkillGroups.Add(new Entities.SkillGroup
            {
                SkillGroupId = 2,
                SkillGroupName = "Test2",
                Description = "desc2",
                CompetencyAreaId = 2,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            dbContext.SkillGroups.Add(new Entities.SkillGroup
            {
                SkillGroupId = 3,
                SkillGroupName = "Test3",
                Description = "desc3",
                CompetencyAreaId = 3,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            dbContext.SkillGroups.Add(new Entities.SkillGroup
            {
                SkillGroupId = 4,
                SkillGroupName = "Test4",
                Description = "desc4",
                CompetencyAreaId = 4,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            dbContext.SkillGroups.Add(new Entities.SkillGroup
            {
                SkillGroupId = 5,
                SkillGroupName = "Test5",
                Description = "desc5",
                CompetencyAreaId = 5,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            dbContext.SkillGroups.Add(new Entities.SkillGroup
            {
                SkillGroupId = 6,
                SkillGroupName = "Test6",
                Description = "desc6",
                CompetencyAreaId = 6,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            dbContext.SkillGroups.Add(new Entities.SkillGroup
            {
                SkillGroupId = 7,
                SkillGroupName = "Test7",
                Description = "desc7",
                CompetencyAreaId = 8,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null

            });

            #endregion

            #region [   Speciality   ]
            dbContext.SGRoleSuffixes.Add(new Entities.SGRoleSuffix
            {
                SuffixID = 1,
                SuffixName = "Java",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.SGRoleSuffixes.Add(new Entities.SGRoleSuffix
            {
                SuffixID = 2,
                SuffixName = "UI",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            #endregion

            #region [    Notification Type    ]

            dbContext.NotificationTypes.Add(new Entities.NotificationType
            {
                NotificationTypeId = 1,
                NotificationCode = "NT00001",
                NotificationDescription = "Notification Description 1",
                CategoryMasterId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.NotificationTypes.Add(new Entities.NotificationType
            {
                NotificationTypeId = 2,
                NotificationCode = "NT00002",
                NotificationDescription = "Notification Description 2",
                CategoryMasterId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.NotificationTypes.Add(new Entities.NotificationType
            {
                NotificationTypeId = 3,
                NotificationCode = "NT00003",
                NotificationDescription = "Notification Description 3",
                CategoryMasterId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.NotificationTypes.Add(new Entities.NotificationType
            {
                NotificationTypeId = 4,
                NotificationCode = "NT00004",
                NotificationDescription = "Notification Description 4",
                CategoryMasterId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.NotificationTypes.Add(new Entities.NotificationType
            {
                NotificationTypeId = 5,
                NotificationCode = "NT00005",
                NotificationDescription = "Notification Description 5",
                CategoryMasterId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [   Status    ]

            dbContext.Statuses.Add(new Entities.Status
            {
                Id = 1,
                StatusId = 1,
                StatusCode = "SendBackForDepartmentHeadReview",
                StatusDescription = "Pending Review",
                CategoryMasterId = 5,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            dbContext.Statuses.Add(new Entities.Status
            {
                Id = 2,
                StatusId = 2,
                StatusCode = "Resigned",
                StatusDescription = "Resigned",
                CategoryMasterId = 1,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            dbContext.Statuses.Add(new Entities.Status
            {
                Id = 3,
                StatusId = 3,
                StatusCode = "ApprovedByPM",
                StatusDescription = "ApprovedByPM",
                CategoryMasterId = 1,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });
            #endregion

            #region [   Roles    ]

            dbContext.Roles.Add(new Entities.Role
            {
                RoleId = 1,
                RoleName = "SystemAdmin",
                RoleDescription = "System Admin",
                DepartmentId = 1,
                KeyResponsibilities = "System admin",
                EducationQualification = "Any bachelor degree",
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            dbContext.Roles.Add(new Entities.Role
            {
                RoleId = 2,
                RoleName = "HRM",
                RoleDescription = "HRM",
                DepartmentId = 2,
                KeyResponsibilities = "HRM",
                EducationQualification = "Any bachelor degree",
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            dbContext.Roles.Add(new Entities.Role
            {
                RoleId = 3,
                RoleName = "HRA",
                RoleDescription = "HRA",
                DepartmentId = 3,
                KeyResponsibilities = "HRA",
                EducationQualification = "Any bachelor degree",
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });
            #endregion

            #region [   Users    ]

            dbContext.Users.Add(new Entities.User
            {
                UserId = 1,
                UserName = "TestUser1",
                Password = null,
                EmailAddress = "TestUser1@senecaglobal.com",
                IsSuperAdmin = null,
                UserRoles = null,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            dbContext.Users.Add(new Entities.User
            {
                UserId = 2,
                UserName = "TestUser2",
                Password = null,
                EmailAddress = "TestUser2@senecaglobal.com",
                IsSuperAdmin = null,
                UserRoles = null,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            dbContext.Users.Add(new Entities.User
            {
                UserId = 3,
                UserName = "TestUser3",
                Password = null,
                EmailAddress = "TestUser3@senecaglobal.com",
                IsSuperAdmin = null,
                UserRoles = null,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });
            #endregion

            #region [   UserRole    ]

            dbContext.UserRoles.Add(new Entities.UserRole
            {
                UserRoleID = 1,
                RoleId = 1,
                UserId = 1,
                IsPrimary = true,
                Role = null,
                User = null,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            dbContext.UserRoles.Add(new Entities.UserRole
            {
                UserRoleID = 2,
                RoleId = 2,
                UserId = 2,
                IsPrimary = true,
                Role = null,
                User = null,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            dbContext.UserRoles.Add(new Entities.UserRole
            {
                UserRoleID = 3,
                RoleId = 3,
                UserId = 3,
                IsPrimary = true,
                Role = null,
                User = null,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });
            #endregion

            #region [    Notification Configuration    ]

            dbContext.NotificationConfigurations.Add(new Entities.NotificationConfiguration
            {
                NotificationConfigurationId = 1,
                NotificationTypeId = 1,
                EmailFrom = "Test@senecaglobal.com",
                EmailTo = "Test1@senecaglobal.com",
                EmailCC = "Test2@senecaglobal.com",
                EmailSubject = "Test 1",
                EmailContent = "Test 1",
                SLA = 0,
                CategoryMasterId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.NotificationConfigurations.Add(new Entities.NotificationConfiguration
            {
                NotificationConfigurationId = 2,
                NotificationTypeId = 2,
                EmailFrom = "Test@senecaglobal.com",
                EmailTo = "Test1@senecaglobal.com",
                EmailCC = "Test2@senecaglobal.com",
                EmailSubject = "Test 2",
                EmailContent = "Test 2",
                SLA = 0,
                CategoryMasterId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.NotificationConfigurations.Add(new Entities.NotificationConfiguration
            {
                NotificationConfigurationId = 3,
                NotificationTypeId = 3,
                EmailFrom = "Test@senecaglobal.com",
                EmailTo = "Test1@senecaglobal.com",
                EmailCC = "Test2@senecaglobal.com",
                EmailSubject = "Test 3",
                EmailContent = "Test 3",
                SLA = 0,
                CategoryMasterId = 1,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [   RoleType    ]

            dbContext.RoleTypes.Add(new Entities.RoleType
            {
                RoleTypeId = 1,
                RoleTypeName = "Test1",
                RoleTypeDescription = "Desc1",
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            dbContext.RoleTypes.Add(new Entities.RoleType
            {
                RoleTypeId = 2,
                RoleTypeName = "Test2",
                RoleTypeDescription = "Desc2",
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            dbContext.RoleTypes.Add(new Entities.RoleType
            {
                RoleTypeId = 3,
                RoleTypeName = "Test3",
                RoleTypeDescription = "Desc3",
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            dbContext.RoleTypes.Add(new Entities.RoleType
            {
                RoleTypeId = 4,
                RoleTypeName = "Test4",
                RoleTypeDescription = "Desc4",
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = null,
                ModifiedBy = null
            });
            #endregion

            #region [   GradeRoleType    ]

            dbContext.GradeRoleTypes.Add(new Entities.GradeRoleType
            {
                GradeRoleTypeId = 1,
                GradeId = 5,
                RoleTypeId = 1,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            dbContext.GradeRoleTypes.Add(new Entities.GradeRoleType
            {
                GradeRoleTypeId = 2,
                GradeId = 5,
                RoleTypeId = 2,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            dbContext.GradeRoleTypes.Add(new Entities.GradeRoleType
            {
                GradeRoleTypeId = 3,
                GradeId = 6,
                RoleTypeId = 2,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = null,
                ModifiedBy = null
            });

            dbContext.GradeRoleTypes.Add(new Entities.GradeRoleType
            {
                GradeRoleTypeId = 4,
                GradeId = 7,
                RoleTypeId = 2,
                CurrentUser = null,
                CreatedDate = null,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = null,
                ModifiedBy = null
            });
            #endregion

            #region [   MenuMaster and MenuRoles    ]

            dbContext.MenuMaster.Add(new Entities.MenuMaster
            {
                MenuId = 1,
                Title = "Associates",
                ParentId = 1,
                IsActive = true
            });

            dbContext.MenuMaster.Add(new Entities.MenuMaster
            {
                MenuId = 2,
                Title = "Reports",
                ParentId = 2,
                IsActive = true
            });

            dbContext.MenuMaster.Add(new Entities.MenuMaster
            {
                MenuId = 3,
                Title = "Prospective Associates",
                ParentId = 3,
                IsActive = true
            });

            dbContext.MenuMaster.Add(new Entities.MenuMaster
            {
                MenuId = 4,
                Title = "Associates Information",
                ParentId = 4,
                IsActive = true
            });

            dbContext.MenuRoles.Add(new Entities.MenuRole
            {
                MenuRoleId = 1,
                MenuId = 3,
                RoleId = 1,
                IsActive = true
            });

            dbContext.MenuRoles.Add(new Entities.MenuRole
            {
                MenuRoleId = 2,
                MenuId = 4,
                RoleId = 1,
                IsActive = true
            });

            dbContext.MenuRoles.Add(new Entities.MenuRole
            {
                MenuRoleId = 3,
                MenuId = 3,
                RoleId = 1,
                IsActive = true
            });

            dbContext.MenuRoles.Add(new Entities.MenuRole
            {
                MenuRoleId = 4,
                MenuId = 4,
                RoleId = 1,
                IsActive = true
            });
            #endregion

            dbContext.SaveChanges();
        }

    }
}
