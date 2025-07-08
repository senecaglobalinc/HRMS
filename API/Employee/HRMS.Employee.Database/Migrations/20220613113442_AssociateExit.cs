using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Employee.Database.Migrations
{
    public partial class AssociateExit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ReasonId",
                table: "AssociateResignation",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "AssociateExit",
                columns: table => new
                {
                    AssociateExitId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    ExitTypeId = table.Column<int>(nullable: false),
                    ActualExitReasonId = table.Column<int>(nullable: true),
                    ActualExitReasonDetail = table.Column<string>(unicode: false, maxLength: 256, nullable: true),
                    ExitReasonId = table.Column<int>(nullable: true),
                    ExitReasonDetail = table.Column<string>(unicode: false, maxLength: 256, nullable: true),
                    ResignationRecomendation = table.Column<string>(unicode: false, maxLength: 256, nullable: true),
                    CalculatedExitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualExitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RehireEligibility = table.Column<bool>(nullable: true),
                    RehireEligibilityDetail = table.Column<string>(nullable: true),
                    ResignationWithdrawn = table.Column<bool>(nullable: false),
                    WithdrawReason = table.Column<string>(nullable: true),
                    WithdrawRemarks = table.Column<string>(nullable: true),
                    TransitionRequired = table.Column<bool>(nullable: true),
                    ImpactOnClientDelivery = table.Column<bool>(nullable: true),
                    ImpactOnClientDeliveryDetail = table.Column<string>(unicode: false, maxLength: 256, nullable: true),
                    Tenure = table.Column<decimal>(nullable: true),
                    Retained = table.Column<bool>(nullable: true),
                    RetainedDetail = table.Column<string>(unicode: false, maxLength: 256, nullable: true),
                    LegalExit = table.Column<bool>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: true),
                    ResignationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AssociateRemarks = table.Column<string>(unicode: false, maxLength: 256, nullable: true),
                    HRAId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateExit", x => x.AssociateExitId);
                });

            migrationBuilder.CreateTable(
                name: "AssociateExitActivity",
                columns: table => new
                {
                    AssociateExitActivityId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    AssociateExitId = table.Column<int>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: false),
                    NoDues = table.Column<bool>(nullable: false),
                    DueAmount = table.Column<decimal>(nullable: true),
                    AssetsNotHanded = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Remarks = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    AssociateAbscondId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateExitActivity", x => x.AssociateExitActivityId);
                    table.ForeignKey(
                        name: "FK_AssociateExitActivity_AssociateExit_AssociateExitId",
                        column: x => x.AssociateExitId,
                        principalTable: "AssociateExit",
                        principalColumn: "AssociateExitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssociateExitAnalysis",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    AssociateExitId = table.Column<int>(nullable: false),
                    RootCause = table.Column<string>(nullable: true),
                    ActionItem = table.Column<string>(nullable: true),
                    Responsibility = table.Column<string>(nullable: true),
                    TagretDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    StatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateExitAnalysis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssociateExitAnalysis_AssociateExit_AssociateExitId",
                        column: x => x.AssociateExitId,
                        principalTable: "AssociateExit",
                        principalColumn: "AssociateExitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssociateExitInterview",
                columns: table => new
                {
                    AssociateExitInterviewId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    AssociateExitId = table.Column<int>(nullable: false),
                    ReasonId = table.Column<int>(nullable: false),
                    ReasonDetail = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    AlternateMobileNo = table.Column<string>(nullable: true),
                    AlternateEmail = table.Column<string>(nullable: true),
                    AlternateAddress = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    ShareEmploymentInfo = table.Column<bool>(nullable: false),
                    IncludeInExAssociateGroup = table.Column<bool>(nullable: false),
                    Remarks = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateExitInterview", x => x.AssociateExitInterviewId);
                    table.ForeignKey(
                        name: "FK_AssociateExitInterview_AssociateExit_AssociateExitId",
                        column: x => x.AssociateExitId,
                        principalTable: "AssociateExit",
                        principalColumn: "AssociateExitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssociateExitWorkflow",
                columns: table => new
                {
                    AssociateExitWorkflowId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    SubmittedBy = table.Column<int>(nullable: false),
                    SubmittedTo = table.Column<int>(nullable: false),
                    SubmittedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WorkflowStatus = table.Column<int>(nullable: false),
                    AssociateExitId = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateExitWorkflow", x => x.AssociateExitWorkflowId);
                    table.ForeignKey(
                        name: "FK_AssociateExitWorkflow_AssociateExit_AssociateExitId",
                        column: x => x.AssociateExitId,
                        principalTable: "AssociateExit",
                        principalColumn: "AssociateExitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Remarks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    AssociateExitId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Remarks_AssociateExit_AssociateExitId",
                        column: x => x.AssociateExitId,
                        principalTable: "AssociateExit",
                        principalColumn: "AssociateExitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransitionPlan",
                columns: table => new
                {
                    TransitionPlanId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    AssociateExitId = table.Column<int>(nullable: false),
                    ProjectClosureId = table.Column<int>(nullable: false),
                    AssociateReleaseId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    TransitionFrom = table.Column<int>(nullable: true),
                    TransitionTo = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    KnowledgeTransferred = table.Column<bool>(nullable: false),
                    KnowledgeTransaferredRemarks = table.Column<string>(nullable: true),
                    Others = table.Column<string>(maxLength: 56, nullable: true),
                    StatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransitionPlan", x => x.TransitionPlanId);
                    table.ForeignKey(
                        name: "FK_TransitionPlan_AssociateExit_AssociateExitId",
                        column: x => x.AssociateExitId,
                        principalTable: "AssociateExit",
                        principalColumn: "AssociateExitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssociateExitActivityDetail",
                columns: table => new
                {
                    AssociateExitActivityDetailId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    AssociateExitActivityId = table.Column<int>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false),
                    ActivityValue = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateExitActivityDetail", x => x.AssociateExitActivityDetailId);
                    table.ForeignKey(
                        name: "FK_AssociateExitActivityDetail_AssociateExitActivity_Associate~",
                        column: x => x.AssociateExitActivityId,
                        principalTable: "AssociateExitActivity",
                        principalColumn: "AssociateExitActivityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransitionPlanDetail",
                columns: table => new
                {
                    TransitionPlanDetailId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    TransitionPlanId = table.Column<int>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Status = table.Column<string>(maxLength: 256, nullable: true),
                    ActivityDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransitionPlanDetail", x => x.TransitionPlanDetailId);
                    table.ForeignKey(
                        name: "FK_TransitionPlanDetail_TransitionPlan_TransitionPlanId",
                        column: x => x.TransitionPlanId,
                        principalTable: "TransitionPlan",
                        principalColumn: "TransitionPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssociateExitActivity_AssociateExitId",
                table: "AssociateExitActivity",
                column: "AssociateExitId");

            migrationBuilder.CreateIndex(
                name: "IX_AssociateExitActivityDetail_AssociateExitActivityId",
                table: "AssociateExitActivityDetail",
                column: "AssociateExitActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_AssociateExitAnalysis_AssociateExitId",
                table: "AssociateExitAnalysis",
                column: "AssociateExitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssociateExitInterview_AssociateExitId",
                table: "AssociateExitInterview",
                column: "AssociateExitId");

            migrationBuilder.CreateIndex(
                name: "IX_AssociateExitWorkflow_AssociateExitId",
                table: "AssociateExitWorkflow",
                column: "AssociateExitId");

            migrationBuilder.CreateIndex(
                name: "IX_Remarks_AssociateExitId",
                table: "Remarks",
                column: "AssociateExitId");

            migrationBuilder.CreateIndex(
                name: "IX_TransitionPlan_AssociateExitId",
                table: "TransitionPlan",
                column: "AssociateExitId");

            migrationBuilder.CreateIndex(
                name: "IX_TransitionPlanDetail_TransitionPlanId",
                table: "TransitionPlanDetail",
                column: "TransitionPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssociateExitActivityDetail");

            migrationBuilder.DropTable(
                name: "AssociateExitAnalysis");

            migrationBuilder.DropTable(
                name: "AssociateExitInterview");

            migrationBuilder.DropTable(
                name: "AssociateExitWorkflow");

            migrationBuilder.DropTable(
                name: "Remarks");

            migrationBuilder.DropTable(
                name: "TransitionPlanDetail");

            migrationBuilder.DropTable(
                name: "AssociateExitActivity");

            migrationBuilder.DropTable(
                name: "TransitionPlan");

            migrationBuilder.DropTable(
                name: "AssociateExit");

            migrationBuilder.AlterColumn<int>(
                name: "ReasonId",
                table: "AssociateResignation",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
