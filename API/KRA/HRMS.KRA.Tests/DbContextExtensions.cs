using HRMS.KRA.Database;
using System;

namespace HRMS.KRA.Tests
{
    public static class DbContextExtensions
    {
        public static void Seed(this KRAContext dbContext)
        {
            // Add entities for DbContext instance

            #region [    Aspect    ]

            dbContext.Aspects.Add(new Entities.Aspect
            {
                AspectId = 1,
                AspectName = "CM00001",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Aspects.Add(new Entities.Aspect
            {
                AspectId = 2,
                AspectName = "CM00002",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Aspects.Add(new Entities.Aspect
            {
                AspectId = 3,
                AspectName = "CM00003",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Aspects.Add(new Entities.Aspect
            {
                AspectId = 4,
                AspectName = "CM00004",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            dbContext.Aspects.Add(new Entities.Aspect
            {
                AspectId = 5,
                AspectName = "CM00005",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            #endregion

            #region [    KRA Aspects   ]

            //dbContext.KRAAspectMasters.Add(new Entities.KRAAspectMaster
            //{
            //    KRAAspectID = 1,
            //    AspectId = 1,
            //    DepartmentId = 1,
            //    CurrentUser = "Anonymous",
            //    CreatedDate = DateTime.UtcNow,
            //    ModifiedDate = null,
            //    SystemInfo = null,
            //    IsActive = true,
            //    CreatedBy = "Anonymous",
            //    ModifiedBy = null
            //});
            //dbContext.KRAAspectMasters.Add(new Entities.KRAAspectMaster
            //{
            //    KRAAspectID = 2,
            //    AspectId = 2,
            //    DepartmentId = 2,
            //    CurrentUser = "Anonymous",
            //    CreatedDate = DateTime.UtcNow,
            //    ModifiedDate = null,
            //    SystemInfo = null,
            //    IsActive = false,
            //    CreatedBy = "Anonymous",
            //    ModifiedBy = null
            //});
            //dbContext.KRAAspectMasters.Add(new Entities.KRAAspectMaster
            //{
            //    KRAAspectID = 3,
            //    AspectId = 3,
            //    DepartmentId = 2,
            //    CurrentUser = "Anonymous",
            //    CreatedDate = DateTime.UtcNow,
            //    ModifiedDate = null,
            //    SystemInfo = null,
            //    IsActive = true,
            //    CreatedBy = "Anonymous",
            //    ModifiedBy = null
            //});
            //dbContext.KRAAspectMasters.Add(new Entities.KRAAspectMaster
            //{
            //    KRAAspectID = 4,
            //    AspectId = 3,
            //    DepartmentId = 2,
            //    CurrentUser = "Anonymous",
            //    CreatedDate = DateTime.UtcNow,
            //    ModifiedDate = null,
            //    SystemInfo = null,
            //    IsActive = false,
            //    CreatedBy = "Anonymous",
            //    ModifiedBy = null
            //});
            //dbContext.KRAAspectMasters.Add(new Entities.KRAAspectMaster
            //{
            //    KRAAspectID = 5,
            //    AspectId = 2,
            //    DepartmentId = 1,
            //    CurrentUser = "Anonymous",
            //    CreatedDate = DateTime.UtcNow,
            //    ModifiedDate = null,
            //    SystemInfo = null,
            //    IsActive = true,
            //    CreatedBy = "Anonymous",
            //    ModifiedBy = null
            //});

            #endregion

            #region [    MeasurementType   ]

            dbContext.MeasurementTypes.Add(new Entities.MeasurementType
            {
                MeasurementTypeId = 1,
                MeasurementTypeName = "MT00001",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.MeasurementTypes.Add(new Entities.MeasurementType
            {
                MeasurementTypeId = 2,
                MeasurementTypeName = "MT00002",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.MeasurementTypes.Add(new Entities.MeasurementType
            {
                MeasurementTypeId = 3,
                MeasurementTypeName = "MT00003",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.MeasurementTypes.Add(new Entities.MeasurementType
            {
                MeasurementTypeId = 4,
                MeasurementTypeName = "MT00004",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.MeasurementTypes.Add(new Entities.MeasurementType
            {
                MeasurementTypeId = 5,
                MeasurementTypeName = "MT00005",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [ Scale ]

            dbContext.Scales.Add(new Entities.Scale
            {
                ScaleId = 1,
                MinimumScale = 1,
                MaximumScale = 2,
                ScaleTitle = "Scale001",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.Scales.Add(new Entities.Scale
            {
                ScaleId = 2,
                MinimumScale = 1,
                MaximumScale = 3,
                ScaleTitle = "Scale002",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.Scales.Add(new Entities.Scale
            {
                ScaleId = 3,
                MinimumScale = 1,
                MaximumScale = 4,
                ScaleTitle = "Scale003",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.Scales.Add(new Entities.Scale
            {
                ScaleId = 4,
                MinimumScale = 1,
                MaximumScale = 5,
                ScaleTitle = "Scale004",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.Scales.Add(new Entities.Scale
            {
                ScaleId = 5,
                MinimumScale = 1,
                MaximumScale = 6,
                ScaleTitle = "Scale005",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [    KRA Scale Details  ]

            dbContext.ScaleDetails.Add(new Entities.ScaleDetails
            {
                ScaleDetailId = 1,
                ScaleId = 1,
                ScaleValue = 1,
                ScaleDescription = "Scale001",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.ScaleDetails.Add(new Entities.ScaleDetails
            {
                ScaleDetailId = 2,
                ScaleId = 2,
                ScaleValue = 2,
                ScaleDescription = "Scale002",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.ScaleDetails.Add(new Entities.ScaleDetails
            {
                ScaleDetailId = 3,
                ScaleId = 3,
                ScaleValue = 1,
                ScaleDescription = "Scale003",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.ScaleDetails.Add(new Entities.ScaleDetails
            {
                ScaleDetailId = 4,
                ScaleId = 3,
                ScaleValue = 2,
                ScaleDescription = "Scale004",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.ScaleDetails.Add(new Entities.ScaleDetails
            {
                ScaleDetailId = 5,
                ScaleId = 3,
                ScaleValue = 3,
                ScaleDescription = "Scale005",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [    Status   ]

            dbContext.Statuses.Add(new Entities.Status
            {
                StatusId = 1,
                StatusText = "Draft",
                StatusDescription = "While HR is definig the KRAs for the first time",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.Statuses.Add(new Entities.Status
            {
                StatusId = 4,
                StatusText = "ApprovedbyHOD",
                StatusDescription = "When HOD approves KRAS for a RoleType",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = true,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });
            dbContext.Statuses.Add(new Entities.Status
            {
                StatusId = 5,
                StatusText = "EditedByHOD",
                StatusDescription = "When HOD is editing KRAs for a RoleType",
                CurrentUser = "Anonymous",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                SystemInfo = null,
                IsActive = false,
                CreatedBy = "Anonymous",
                ModifiedBy = null
            });

            #endregion

            #region [    ApplicableRoleType   ]

            //dbContext.ApplicableRoleTypes.Add(new Entities.ApplicableRoleType
            //{
            //    ApplicableRoleTypeId = 11,
            //    FinancialYearId = 4,
            //    DepartmentId = 1,
            //    GradeRoleTypeId = 4,
            //    CurrentUser = "Anonymous",
            //    CreatedDate = DateTime.UtcNow,
            //    ModifiedDate = null,
            //    SystemInfo = null,
            //    IsActive = true,
            //    CreatedBy = "Anonymous",
            //    ModifiedBy = null
            //});

            //dbContext.ApplicableRoleTypes.Add(new Entities.ApplicableRoleType
            //{
            //    ApplicableRoleTypeId = 10,
            //    FinancialYearId = 6,
            //    DepartmentId = 1,              
            //    GradeRoleTypeId=6,             
            //    CurrentUser = "Anonymous",
            //    CreatedDate = DateTime.UtcNow,
            //    ModifiedDate = null,
            //    SystemInfo = null,
            //    IsActive = true,
            //    CreatedBy = "Anonymous",
            //    ModifiedBy = null
            //});

            //#endregion

            //#region [    Definition Information  ]
            //dbContext.ApplicableRoleTypes.Add(new Entities.ApplicableRoleType
            //{ 
            //    ApplicableRoleTypeId = 1,
            //    FinancialYearId = 1,
            //    DepartmentId = 1,
            //    GradeRoleTypeId = 1,
            //    StatusId = 1
            //});

            dbContext.Definitions.Add(new Entities.Definition
            {
                //DefinitionId = 1,
                //ApplicableRoleTypeId = 1,
                AspectId = 1,
                //IsHODApproved = false,
                //IsCEOApproved = false,
                //IsDeleted = false,
                //SourceDefinitionId = 0
            });

            dbContext.DefinitionDetails.Add(new Entities.DefinitionDetails
            {
                DefinitionDetailsId = 1,
                DefinitionId = 1,
                Metric = "Metric String",
                OperatorId = 1,
                MeasurementTypeId = 1,
                ScaleId = 1,
                TargetValue = "3",
                TargetPeriodId = 3
            });
            dbContext.DefinitionTransactions.Add(new Entities.DefinitionTransaction
            {
                DefinitionTransactionId = 1,
                //DefinitionDetailsId = 1,
                Metric = "Metric String",
                OperatorId = 1,
                MeasurementTypeId = 1,
                ScaleId = 1,
                TargetValue = "3",
                TargetPeriodId = 3
            });
            #endregion 

            dbContext.SaveChanges();
        }
    }
}
