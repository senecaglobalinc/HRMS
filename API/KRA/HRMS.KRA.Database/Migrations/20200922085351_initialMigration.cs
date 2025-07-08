using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.KRA.Database.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aspect",
                columns: table => new
                {
                    AspectId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AspectName = table.Column<string>(type: "varchar(70)", maxLength: 70, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aspect", x => x.AspectId);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementType",
                columns: table => new
                {
                    MeasurementTypeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MeasurementTypeName = table.Column<string>(type: "varchar(140)", maxLength: 140, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementType", x => x.MeasurementTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Operator",
                columns: table => new
                {
                    OperatorId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OperatorValue = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operator", x => x.OperatorId);
                });

            migrationBuilder.CreateTable(
                name: "Scale",
                columns: table => new
                {
                    ScaleId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScaleTitle = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    MinimumScale = table.Column<int>(type: "integer", nullable: false),
                    MaximumScale = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scale", x => x.ScaleId);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    StatusId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusText = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    StatusDescription = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "TargetPeriod",
                columns: table => new
                {
                    TargetPeriodId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TargetPeriodValue = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetPeriod", x => x.TargetPeriodId);
                });

            migrationBuilder.CreateTable(
                name: "ScaleDetails",
                columns: table => new
                {
                    ScaleDetailId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScaleValue = table.Column<int>(type: "integer", nullable: false),
                    ScaleDescription = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ScaleId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScaleDetails", x => x.ScaleDetailId);
                    table.ForeignKey(
                        name: "FK_ScaleDetails_Scale_ScaleId",
                        column: x => x.ScaleId,
                        principalTable: "Scale",
                        principalColumn: "ScaleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicableRoleType",
                columns: table => new
                {
                    ApplicableRoleTypeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FinancialYearId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    GradeRoleTypeId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicableRoleType", x => x.ApplicableRoleTypeId);
                    table.ForeignKey(
                        name: "FK_ApplicableRoleType_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CommentText = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    FinancialYearId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    ApplicableRoleTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_Comment_ApplicableRoleType_ApplicableRoleTypeId",
                        column: x => x.ApplicableRoleTypeId,
                        principalTable: "ApplicableRoleType",
                        principalColumn: "ApplicableRoleTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Definition",
                columns: table => new
                {
                    DefinitionId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicableRoleTypeId = table.Column<int>(type: "int", nullable: false),
                    AspectId = table.Column<int>(type: "int", nullable: false),
                    IsHODApproved = table.Column<bool>(type: "boolean", nullable: true),
                    IsCEOApproved = table.Column<bool>(type: "boolean", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    SourceDefinitionId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Definition", x => x.DefinitionId);
                    table.ForeignKey(
                        name: "FK_Definition_ApplicableRoleType_ApplicableRoleTypeId",
                        column: x => x.ApplicableRoleTypeId,
                        principalTable: "ApplicableRoleType",
                        principalColumn: "ApplicableRoleTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Definition_Aspect_AspectId",
                        column: x => x.AspectId,
                        principalTable: "Aspect",
                        principalColumn: "AspectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DefinitionDetails",
                columns: table => new
                {
                    DefinitionDetailsId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DefinitionId = table.Column<int>(type: "int", nullable: false),
                    Metric = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    OperatorId = table.Column<int>(type: "int", nullable: false),
                    MeasurementTypeId = table.Column<int>(type: "int", nullable: false),
                    ScaleId = table.Column<int>(type: "int", nullable: false),
                    TargetValue = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    TargetPeriodId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefinitionDetails", x => x.DefinitionDetailsId);
                    table.ForeignKey(
                        name: "FK_DefinitionDetails_Definition_DefinitionId",
                        column: x => x.DefinitionId,
                        principalTable: "Definition",
                        principalColumn: "DefinitionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefinitionDetails_MeasurementType_MeasurementTypeId",
                        column: x => x.MeasurementTypeId,
                        principalTable: "MeasurementType",
                        principalColumn: "MeasurementTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefinitionDetails_Operator_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operator",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefinitionDetails_Scale_ScaleId",
                        column: x => x.ScaleId,
                        principalTable: "Scale",
                        principalColumn: "ScaleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefinitionDetails_TargetPeriod_TargetPeriodId",
                        column: x => x.TargetPeriodId,
                        principalTable: "TargetPeriod",
                        principalColumn: "TargetPeriodId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DefinitionTransaction",
                columns: table => new
                {
                    DefinitionTransactionId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DefinitionDetailsId = table.Column<int>(type: "int", nullable: false),
                    Metric = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    OperatorId = table.Column<int>(type: "int", nullable: false),
                    MeasurementTypeId = table.Column<int>(type: "int", nullable: false),
                    ScaleId = table.Column<int>(type: "int", nullable: false),
                    TargetValue = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    TargetPeriodId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefinitionTransaction", x => x.DefinitionTransactionId);
                    table.ForeignKey(
                        name: "FK_DefinitionTransaction_DefinitionDetails_DefinitionDetailsId",
                        column: x => x.DefinitionDetailsId,
                        principalTable: "DefinitionDetails",
                        principalColumn: "DefinitionDetailsId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefinitionTransaction_MeasurementType_MeasurementTypeId",
                        column: x => x.MeasurementTypeId,
                        principalTable: "MeasurementType",
                        principalColumn: "MeasurementTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefinitionTransaction_Operator_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operator",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefinitionTransaction_Scale_ScaleId",
                        column: x => x.ScaleId,
                        principalTable: "Scale",
                        principalColumn: "ScaleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefinitionTransaction_TargetPeriod_TargetPeriodId",
                        column: x => x.TargetPeriodId,
                        principalTable: "TargetPeriod",
                        principalColumn: "TargetPeriodId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicableRoleType_StatusId",
                table: "ApplicableRoleType",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ApplicableRoleTypeId",
                table: "Comment",
                column: "ApplicableRoleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Definition_ApplicableRoleTypeId",
                table: "Definition",
                column: "ApplicableRoleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Definition_AspectId",
                table: "Definition",
                column: "AspectId");

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionDetails_DefinitionId",
                table: "DefinitionDetails",
                column: "DefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionDetails_MeasurementTypeId",
                table: "DefinitionDetails",
                column: "MeasurementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionDetails_OperatorId",
                table: "DefinitionDetails",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionDetails_ScaleId",
                table: "DefinitionDetails",
                column: "ScaleId");

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionDetails_TargetPeriodId",
                table: "DefinitionDetails",
                column: "TargetPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionTransaction_DefinitionDetailsId",
                table: "DefinitionTransaction",
                column: "DefinitionDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionTransaction_MeasurementTypeId",
                table: "DefinitionTransaction",
                column: "MeasurementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionTransaction_OperatorId",
                table: "DefinitionTransaction",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionTransaction_ScaleId",
                table: "DefinitionTransaction",
                column: "ScaleId");

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionTransaction_TargetPeriodId",
                table: "DefinitionTransaction",
                column: "TargetPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_ScaleDetails_ScaleId",
                table: "ScaleDetails",
                column: "ScaleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "DefinitionTransaction");

            migrationBuilder.DropTable(
                name: "ScaleDetails");

            migrationBuilder.DropTable(
                name: "DefinitionDetails");

            migrationBuilder.DropTable(
                name: "Definition");

            migrationBuilder.DropTable(
                name: "MeasurementType");

            migrationBuilder.DropTable(
                name: "Operator");

            migrationBuilder.DropTable(
                name: "Scale");

            migrationBuilder.DropTable(
                name: "TargetPeriod");

            migrationBuilder.DropTable(
                name: "ApplicableRoleType");

            migrationBuilder.DropTable(
                name: "Aspect");

            migrationBuilder.DropTable(
                name: "Status");
        }
    }
}
