using HRMS.Project.Database;
using HRMS.Project.Entities;
using System;

namespace HRMS.Project.Tests
{
    public static class DbContextExtensions
    {
        public static void Seed(this ProjectDBContext dbContext)
        {
            // Add entities for DbContext instance

            #region [    Project  ]

            dbContext.Projects.Add(new Entities.Project
            {
                ProjectId = 1,
                ProjectCode = "POJ1",
                ProjectName = "Project 1",
                CurrentUser = "Anonymous",
                PlannedStartDate = DateTime.Now.AddDays(10),
                PlannedEndDate = DateTime.Now.AddYears(5),
                ClientId = 1,
                StatusId = 1,
                ProjectTypeId = 1,
                ProjectStateId = 1,
                ActualStartDate = DateTime.Now.AddDays(10),
                ActualEndDate = DateTime.Now.AddYears(5),
                DepartmentId = 1,
                PracticeAreaId = 1,
                DomainId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Projects.Add(new Entities.Project
            {
                ProjectId = 2,
                ProjectCode = "POJ2",
                ProjectName = "Project 2",
                CurrentUser = "Anonymous",
                PlannedStartDate = DateTime.Now.AddDays(10),
                PlannedEndDate = DateTime.Now.AddYears(5),
                ClientId = 1,
                StatusId = 1,
                ProjectTypeId = 1,
                ProjectStateId = 1,
                ActualStartDate = DateTime.Now.AddDays(10),
                ActualEndDate = DateTime.Now.AddYears(5),
                DepartmentId = 1,
                PracticeAreaId = 1,
                DomainId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Projects.Add(new Entities.Project
            {
                ProjectId = 3,
                ProjectCode = "POJ3",
                ProjectName = "Project 3",
                CurrentUser = "Anonymous",
                PlannedStartDate = DateTime.Now.AddDays(10),
                PlannedEndDate = DateTime.Now.AddYears(5),
                ClientId = 1,
                StatusId = 1,
                ProjectTypeId = 1,
                ProjectStateId = 1,
                ActualStartDate = DateTime.Now.AddDays(10),
                ActualEndDate = DateTime.Now.AddYears(5),
                DepartmentId = 1,
                PracticeAreaId = 1,
                DomainId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Projects.Add(new Entities.Project
            {
                ProjectId = 4,
                ProjectCode = "POJ4",
                ProjectName = "Project 4",
                CurrentUser = "Anonymous",
                PlannedStartDate = DateTime.Now.AddDays(10),
                PlannedEndDate = DateTime.Now.AddYears(5),
                ClientId = 1,
                StatusId = 1,
                ProjectTypeId = 1,
                ProjectStateId = 1,
                ActualStartDate = DateTime.Now.AddDays(10),
                ActualEndDate = DateTime.Now.AddYears(5),
                DepartmentId = 1,
                PracticeAreaId = 1,
                DomainId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Projects.Add(new Entities.Project
            {
                ProjectId = 5,
                ProjectCode = "POJ5",
                ProjectName = "Project 5",
                CurrentUser = "Anonymous",
                PlannedStartDate = DateTime.Now.AddDays(10),
                PlannedEndDate = DateTime.Now.AddYears(5),
                ClientId = 1,
                StatusId = 1,
                ProjectTypeId = 1,
                ProjectStateId = 1,
                ActualStartDate = DateTime.Now.AddDays(10),
                ActualEndDate = DateTime.Now.AddYears(5),
                DepartmentId = 1,
                PracticeAreaId = 1,
                DomainId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [    SOW  ]

            dbContext.SOW.Add(new Entities.SOW
            {
                Id = 1,
                SOWId = "SOW1",
                SOWFileName = "SOW Test File 1",
                SOWSignedDate = DateTime.Now.AddDays(-1),
                CurrentUser = "Anonymous",
                ProjectId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.SOW.Add(new Entities.SOW
            {
                Id = 2,
                SOWId = "SOW2",
                SOWFileName = "SOW Test File 2",
                SOWSignedDate = DateTime.Now.AddDays(-1),
                CurrentUser = "Anonymous",
                ProjectId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.SOW.Add(new Entities.SOW
            {
                Id = 3,
                SOWId = "SOW3",
                SOWFileName = "SOW Test File 3",
                SOWSignedDate = DateTime.Now.AddDays(-1),
                CurrentUser = "Anonymous",
                ProjectId = 2,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.SOW.Add(new Entities.SOW
            {
                Id = 4,
                SOWId = "SOW4",
                SOWFileName = "SOW Test File 4",
                SOWSignedDate = DateTime.Now.AddDays(-1),
                CurrentUser = "Anonymous",
                ProjectId = 3,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.SOW.Add(new Entities.SOW
            {
                Id = 5,
                SOWId = "SOW5",
                SOWFileName = "SOW Test File 5",
                SOWSignedDate = DateTime.Now.AddDays(-1),
                CurrentUser = "Anonymous",
                ProjectId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [    Addendum  ]

            dbContext.Addendum.Add(new Entities.Addendum
            {
                Id = 1,
                SOWId = "SOW1",
                AddendumId = 1,
                AddendumDate = DateTime.Now.AddDays(-1),
                AddendumNo = "ADN01",
                Note = "Test Note",
                RecipientName = "Test User",
                CurrentUser = "Anonymous",
                ProjectId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Addendum.Add(new Entities.Addendum
            {
                Id = 1,
                SOWId = "SOW1",
                AddendumId = 2,
                AddendumDate = DateTime.Now.AddDays(-1),
                AddendumNo = "ADN02",
                Note = "Test Note",
                RecipientName = "Test User",
                CurrentUser = "Anonymous",
                ProjectId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Addendum.Add(new Entities.Addendum
            {
                Id = 1,
                SOWId = "SOW1",
                AddendumId = 3,
                AddendumDate = DateTime.Now.AddDays(-1),
                AddendumNo = "ADN03",
                Note = "Test Note",
                RecipientName = "Test User",
                CurrentUser = "Anonymous",
                ProjectId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Addendum.Add(new Entities.Addendum
            {
                Id = 3,
                SOWId = "SOW3",
                AddendumId = 4,
                AddendumDate = DateTime.Now.AddDays(-1),
                AddendumNo = "ADN04",
                Note = "Test Note",
                RecipientName = "Test User",
                CurrentUser = "Anonymous",
                ProjectId = 2,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Addendum.Add(new Entities.Addendum
            {
                Id = 3,
                SOWId = "SOW3",
                AddendumId = 5,
                AddendumDate = DateTime.Now.AddDays(-1),
                AddendumNo = "ADN05",
                Note = "Test Note",
                RecipientName = "Test User",
                CurrentUser = "Anonymous",
                ProjectId = 2,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [    AllocationPercentage  ]

            dbContext.AllocationPercentage.Add(new AllocationPercentage
            {
                AllocationPercentageId = 1,
                Percentage = 25,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.AllocationPercentage.Add(new AllocationPercentage
            {
                AllocationPercentageId = 2,
                Percentage = 50,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.AllocationPercentage.Add(new AllocationPercentage
            {
                AllocationPercentageId = 3,
                Percentage = 75,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.AllocationPercentage.Add(new AllocationPercentage
            {
                AllocationPercentageId = 4,
                Percentage = 100,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.AllocationPercentage.Add(new AllocationPercentage
            {
                AllocationPercentageId = 5,
                Percentage = 0,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });




            #endregion

            #region [    Client Billing Roles  ]

            dbContext.ClientBillingRoles.Add(new ClientBillingRoles
            {
                ClientBillingRoleId = 1,
                ClientBillingRoleCode = "Dev",
                ClientBillingRoleName = "Developer",
                NoOfPositions = 1,
                StartDate = DateTime.Now.AddDays(10),
                EndDate = DateTime.Now.AddYears(5),
                ClientBillingPercentage = 4,
                ClientId = 0,
                CurrentUser = "Anonymous",
                ProjectId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.ClientBillingRoles.Add(new ClientBillingRoles
            {
                ClientBillingRoleId = 2,
                ClientBillingRoleCode = "Lead",
                ClientBillingRoleName = "Lead",
                NoOfPositions = 1,
                StartDate = DateTime.Now.AddDays(10),
                EndDate = DateTime.Now.AddYears(5),
                ClientBillingPercentage = 4,
                ClientId = 0,
                CurrentUser = "Anonymous",
                ProjectId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });



            #endregion

            #region [    Project Managers  ]

            dbContext.ProjectManagers.Add(new ProjectManager
            {
                Id = 1,
                ProjectId = 50,
                ReportingManagerId = 213,
                ProgramManagerId = 213,
                LeadId = 0,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.ProjectManagers.Add(new ProjectManager
            {
                Id = 2,
                ProjectId = 39,
                ReportingManagerId = 244,
                ProgramManagerId = 244,
                LeadId = 7,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.ProjectManagers.Add(new ProjectManager
            {
                Id = 3,
                ProjectId = 5,
                ReportingManagerId = 152,
                ProgramManagerId = 152,
                LeadId = 7,
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });



            #endregion

            #region [AssociateAllocation]

            dbContext.AssociateAllocation.Add(new AssociateAllocation
            {
                AssociateAllocationId = 1,
                Trid = 1,
                ProjectId = 50,
                EmployeeId = 159,
                RoleMasterId = 2,
                AllocationPercentage = 1,
                InternalBillingPercentage = 0,
                IsCritical = true,
                EffectiveDate = DateTime.UtcNow,
                AllocationDate = DateTime.UtcNow,
                ReportingManagerId = 213,
                IsPrimary = true,
                IsBillable = true,
                InternalBillingRoleId = 11,
                ReleaseDate = null,
                ClientBillingPercentage = 25,
                ProgramManagerId = 213,
                LeadId = 30,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.AssociateAllocation.Add(new AssociateAllocation
            {
                AssociateAllocationId = 2,
                Trid = 1,
                ProjectId = 39,
                EmployeeId = 159,
                RoleMasterId = 2,
                AllocationPercentage = 2,
                InternalBillingPercentage = 0,
                IsCritical = false,
                EffectiveDate = DateTime.UtcNow,
                AllocationDate = DateTime.UtcNow,
                ReportingManagerId = 213,
                IsPrimary = true,
                IsBillable = true,
                InternalBillingRoleId = 11,
                ReleaseDate = null,
                ClientBillingPercentage = 50,
                ProgramManagerId = 214,
                LeadId = 330,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            #endregion


            #region [    ProjectClosure  ]

            dbContext.ProjectClosure.Add(new Entities.ProjectClosure
            {
                ProjectId = 1,
                ProjectClosureId=2,
                CurrentUser = "Anonymous",
                ExpectedClosureDate = DateTime.Now.AddDays(10),
                ActualClosureDate = DateTime.Now.AddYears(5),
                StatusId = 1,
                Remarks="Good",
                IsTransitionRequired=false,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.ProjectClosure.Add(new Entities.ProjectClosure
            {
                ProjectId = 2,
                ProjectClosureId = 3,
                CurrentUser = "Anonymous",
                ExpectedClosureDate = DateTime.Now.AddDays(1),
                ActualClosureDate = DateTime.Now.AddYears(4),
                StatusId = 2,
                Remarks = "xyz",
                IsTransitionRequired = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.ProjectClosure.Add(new Entities.ProjectClosure
            {
                ProjectId = 5,
                ProjectClosureId = 4,
                CurrentUser = "Anonymous",
                ExpectedClosureDate = DateTime.Now.AddDays(3),
                ActualClosureDate = DateTime.Now.AddYears(2),
                StatusId = 6,
                Remarks = "super",
                IsTransitionRequired = false,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [    ProjectClosureActivity  ]

            dbContext.ProjectClosureActivity.Add(new Entities.ProjectClosureActivity
            {
                ProjectClosureActivityId = 1,
                ProjectClosureId = 2,
                CurrentUser = "Anonymous",
                StatusId = 1,
                DepartmentId=3,
                Remarks = "Good",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.ProjectClosureActivity.Add(new Entities.ProjectClosureActivity
            {
                ProjectClosureActivityId = 3,
                ProjectClosureId = 1,
                CurrentUser = "Anonymous",
                StatusId = 3,
                DepartmentId = 2,
                Remarks = "goood",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.ProjectClosureActivity.Add(new Entities.ProjectClosureActivity
            {
                ProjectClosureActivityId = 10,
                ProjectClosureId = 3,
                CurrentUser = "Anonymous",
                StatusId = 4,
                DepartmentId = 1,
                Remarks = "Excellent",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            #endregion

            #region [    ProjectClosureActivityDetail  ]

            dbContext.ProjectClosureActivityDetail.Add(new Entities.ProjectClosureActivityDetail
            {
                ProjectClosureActivityDetailId = 1,
                ProjectClosureActivityId = 2,
                ActivityId=4,
                CurrentUser = "Anonymous",
                Value="hii",
                Remarks = "Good",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.ProjectClosureActivityDetail.Add(new Entities.ProjectClosureActivityDetail
            {
                ProjectClosureActivityDetailId = 4,
                ProjectClosureActivityId = 3,
                ActivityId = 2,
                CurrentUser = "Anonymous",
                Value = "value1",
                Remarks = "remarks",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.ProjectClosureActivityDetail.Add(new Entities.ProjectClosureActivityDetail
            {
                ProjectClosureActivityDetailId = 2,
                ProjectClosureActivityId = 1,
                ActivityId = 3,
                CurrentUser = "Anonymous",
                Value = "Activity",
                Remarks = "acti remarks",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [    ProjectClosureReport  ]

            dbContext.ProjectClosureReport.Add(new Entities.ProjectClosureReport
            {
                ProjectClosureReportId = 1,
                ProjectClosureId = 2,
                ClientFeedback="good",
                DeliveryPerformance="Good",
                ValueDelivered="Gud",
                ManagementChallenges="mna",
                TechnologyChallenges="Tech",
                EngineeringChallenges="Eng",
                BestPractices="best",
                LessonsLearned="less",
                ReusableArtifacts="reuse",
                ProcessImprovements="process",
                Awards="awarsa",
                NewTechnicalSkills="tech",
                NewTools="tools",
                CaseStudy="study",
                ClientFeedbackFile= "Poj5\\5_Project5_csat.pdf",
                DeliveryPerformanceFile= "Poj5\\5_Project5_workbook.pdf",
                CurrentUser = "Anonymous",
                Remarks = "Good",
                StatusId=2,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.ProjectClosureReport.Add(new Entities.ProjectClosureReport
            {
                ProjectClosureReportId = 2,
                ProjectClosureId = 4,
                ClientFeedback = "super",
                DeliveryPerformance = "Good",
                ValueDelivered = "good",
                ManagementChallenges = "mng",
                TechnologyChallenges = "Tehn",
                EngineeringChallenges = "Engi",
                BestPractices = "bestpr",
                LessonsLearned = "lessons",
                ReusableArtifacts = "reuseart",
                ProcessImprovements = "process1",
                Awards = "awards",
                NewTechnicalSkills = "tech1",
                NewTools = "tools1",
                CaseStudy = "study1",
                ClientFeedbackFile = "Poj5\\5_Project5_csat.pdf",
                DeliveryPerformanceFile = "Poj5\\5_Project5_workbook.pdf",
                CurrentUser = "Anonymous",
                Remarks = "Good1",
                StatusId = 2,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [    ProjectClosureWorkflow ]

            dbContext.ProjectClosureWorkflow.Add(new Entities.ProjectClosureWorkflow
            {
                ProjectClosureWorkflowId = 1,
                ProjectClosureId = 4,
                SubmittedBy=154,
                SubmittedDate= DateTime.UtcNow,
                SubmittedTo=12,
                WorkflowStatus=20,
                Comments="cmm",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.ProjectClosureWorkflow.Add(new Entities.ProjectClosureWorkflow
            {
                ProjectClosureWorkflowId = 2,
                ProjectClosureId = 3,
                SubmittedBy = 134,
                SubmittedDate = DateTime.UtcNow,
                SubmittedTo = 22,
                WorkflowStatus = 10,
                Comments = "comments",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.ProjectClosureWorkflow.Add(new Entities.ProjectClosureWorkflow
            {
                ProjectClosureWorkflowId = 12,
                ProjectClosureId = 4,
                SubmittedBy = 64,
                SubmittedDate = DateTime.UtcNow,
                SubmittedTo = 10,
                WorkflowStatus = 16,
                Comments = "cmm1",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            #endregion
            dbContext.SaveChanges();
        }

    }
}
