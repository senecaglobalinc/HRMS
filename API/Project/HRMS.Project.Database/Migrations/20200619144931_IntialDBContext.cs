using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Project.Database.Migrations
{
    public partial class IntialDBContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllocationPercentage",
                columns: table => new
                {
                    AllocationPercentageId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Percentage = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "inet_client_addr()"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllocationPercentage", x => x.AllocationPercentageId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectRoleDetails",
                columns: table => new
                {
                    RoleAssignmentId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "Integer", nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    RoleMasterId = table.Column<int>(type: "Integer", nullable: true),
                    IsPrimaryRole = table.Column<bool>(nullable: true),
                    StatusId = table.Column<int>(nullable: true),
                    FromDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ToDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 50, nullable: true),
                    RejectReason = table.Column<string>(unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRoleDetails", x => x.RoleAssignmentId);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectCode = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ProjectName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PlannedStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PlannedEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProjectTypeId = table.Column<int>(type: "integer", nullable: true),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(unicode: false, maxLength: 50, nullable: true, defaultValueSql: "inet_client_addr()"),
                    ActualStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    PracticeAreaId = table.Column<int>(type: "integer", nullable: false),
                    DomainId = table.Column<int>(nullable: true),
                    ProjectStateId = table.Column<int?>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectsHistory",
                columns: table => new
                {
                    ProjectHistoryId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    ProjectCode = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    ProjectName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    ProjectTypeId = table.Column<int>(type: "integer", nullable: true),
                    ProjectStateId = table.Column<int>(nullable: false),
                    ActualStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActualEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, defaultValueSql: "inet_client_addr()"),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    PracticeAreaId = table.Column<int>(type: "integer", nullable: false),
                    DomainId = table.Column<int>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsHistory", x => x.ProjectHistoryId);
                });

            migrationBuilder.CreateTable(
                name: "TalentPool",
                columns: table => new
                {
                    TalentPoolId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PracticeAreaId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true, defaultValueSql: "inet_client_addr()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalentPool", x => x.TalentPoolId);
                });

            migrationBuilder.CreateTable(
                name: "ClientBillingRoles",
                columns: table => new
                {
                    ClientBillingRoleId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientBillingRoleCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ClientBillingRoleName = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, defaultValueSql: "inet_client_addr()"),
                    ClientId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NoOfPositions = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClientBillingPercentage = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientBillingRoles", x => x.ClientBillingRoleId);
                    table.ForeignKey(
                        name: "FK_ClientBillingRoles_Projects",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectManagers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "Integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectID = table.Column<int>(type: "Integer", nullable: true),
                    ReportingManagerID = table.Column<int>(type: "Integer", nullable: true),
                    ProgramManagerID = table.Column<int>(type: "Integer", nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, defaultValueSql: "inet_client_addr()"),
                    LeadID = table.Column<int>(type: "Integer", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectManagers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProjectManagers_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectRoles",
                columns: table => new
                {
                    ProjectRoleId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    RoleMasterId = table.Column<int>(type: "Integer", nullable: false),
                    Responsibilities = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, defaultValueSql: "inet_client_addr()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRoles", x => x.ProjectRoleId);
                    table.ForeignKey(
                        name: "FK_ProjectRoles_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectWorkFlow",
                columns: table => new
                {
                    WorkFlowId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubmittedBy = table.Column<int>(nullable: false),
                    SubmittedTo = table.Column<int>(nullable: false),
                    SubmittedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WorkFlowStatus = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    SystemInfo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectWorkFlow", x => x.WorkFlowId);
                    table.ForeignKey(
                        name: "FK_ProjectWorkFlow_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SOW",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    SOWId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    SOWFileName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    SOWSignedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, defaultValueSql: "inet_client_addr()"),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SOW_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TalentRequisition",
                columns: table => new
                {
                    TrId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepartmentId = table.Column<int>(type: "Integer", nullable: false),
                    PracticeAreaId = table.Column<int>(type: "Integer", nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    RequestedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TargetFulfillmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    ApprovedBy = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, defaultValueSql: "inet_client_addr()"),
                    Remarks = table.Column<string>(type: "varchar(1500)", maxLength: 1500, nullable: true),
                    TRCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    RequisitionType = table.Column<int>(nullable: true),
                    RaisedBy = table.Column<int>(nullable: true),
                    DraftedBy = table.Column<int>(nullable: true),
                    ClientId = table.Column<int>(type: "Integer", nullable: true),
                    ProjectDuration = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalentRequisition", x => x.TrId);
                    table.ForeignKey(
                        name: "FK_TalentRequisition_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addendum",
                columns: table => new
                {
                    AddendumId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    SOWId = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    AddendumNo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    RecipientName = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    AddendumDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, defaultValueSql: "inet_client_addr()"),
                    Id = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addendum", x => x.AddendumId);
                    table.ForeignKey(
                        name: "FK_Addendum_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addendum_Id",
                        column: x => x.Id,
                        principalTable: "SOW",

                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssociateAllocation",
                columns: table => new
                {
                    AssociateAllocationId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TRId = table.Column<int>(type: "Integer", nullable: true),
                    ProjectId = table.Column<int>(type: "Integer", nullable: true),
                    EmployeeId = table.Column<int>(type: "Integer", nullable: true),
                    RoleMasterId = table.Column<int>(type: "Integer", nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    AllocationPercentage = table.Column<int>(nullable: true),
                    InternalBillingPercentage = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    IsCritical = table.Column<bool>(nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AllocationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    ModifiedBy = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true, defaultValueSql: "inet_client_addr()"),
                    ReportingManagerId = table.Column<int>(type: "Integer", nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: true, defaultValueSql: "(('f'))"),
                    IsBillable = table.Column<bool>(nullable: true),
                    InternalBillingRoleId = table.Column<int>(nullable: true),
                    ClientBillingRoleId = table.Column<int>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClientBillingPercentage = table.Column<decimal>(type: "decimal(18, 0)", nullable: true, defaultValueSql: "((0))"),
                    ProgramManagerID = table.Column<int>(type: "Integer", nullable: true),
                    LeadID = table.Column<int>(type: "Integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateAllocation", x => x.AssociateAllocationId);
                    table.ForeignKey(
                        name: "FK_AssociateAllocation_AllocationPercentage_AllocationPercenta~",
                        column: x => x.AllocationPercentage,
                        principalTable: "AllocationPercentage",
                        principalColumn: "AllocationPercentageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssociateAllocation_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssociateAllocation_TalentRequisition_TRId",
                        column: x => x.TRId,
                        principalTable: "TalentRequisition",
                        principalColumn: "TrId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addendum_ProjectId",
                table: "Addendum",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Addendum_SOWId",
                table: "Addendum",
                column: "SOWId");

            migrationBuilder.CreateIndex(
                name: "IX_AssociateAllocation_AllocationPercentage",
                table: "AssociateAllocation",
                column: "AllocationPercentage");

            migrationBuilder.CreateIndex(
                name: "IX_AssociateAllocation_ProjectId",
                table: "AssociateAllocation",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AssociateAllocation_TRId",
                table: "AssociateAllocation",
                column: "TRId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientBillingRoles_ProjectId",
                table: "ClientBillingRoles",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectManagers_ProjectID",
                table: "ProjectManagers",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRoles_ProjectId",
                table: "ProjectRoles",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectCode",
                table: "Projects",
                column: "ProjectCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectName",
                table: "Projects",
                column: "ProjectName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectWorkFlow_ProjectId",
                table: "ProjectWorkFlow",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SOW_ProjectId",
                table: "SOW",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SOW_SOWId_ProjectId",
                table: "SOW",
                columns: new[] { "SOWId", "ProjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TalentRequisition_ProjectId",
                table: "TalentRequisition",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addendum");

            migrationBuilder.DropTable(
                name: "AssociateAllocation");

            migrationBuilder.DropTable(
                name: "ClientBillingRoles");

            migrationBuilder.DropTable(
                name: "ProjectManagers");

            migrationBuilder.DropTable(
                name: "ProjectRoleDetails");

            migrationBuilder.DropTable(
                name: "ProjectRoles");

            migrationBuilder.DropTable(
                name: "ProjectsHistory");

            migrationBuilder.DropTable(
                name: "ProjectWorkFlow");

            migrationBuilder.DropTable(
                name: "TalentPool");

            migrationBuilder.DropTable(
                name: "SOW");

            migrationBuilder.DropTable(
                name: "AllocationPercentage");

            migrationBuilder.DropTable(
                name: "TalentRequisition");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
