using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Admin.Database.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryMasterId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),                  
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryMasterId);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientCode = table.Column<string>(maxLength: 20, nullable: false),
                    ClientName = table.Column<string>(nullable: true),
                    ClientRegisterName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),                 
                    ClientNameHash = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "CompetencyArea",
                columns: table => new
                {
                    CompetencyAreaId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompetencyAreaCode = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CompetencyAreaDescription = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),                   
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetencyArea", x => x.CompetencyAreaId);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentType",
                columns: table => new
                {
                    DepartmentTypeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepartmentTypeDescription = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentType", x => x.DepartmentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Domain",
                columns: table => new
                {
                    DomainID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DomainName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
   
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domain", x => x.DomainID);
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    GradeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GradeCode = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    GradeName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.GradeId);
                });

            migrationBuilder.CreateTable(
                name: "PracticeArea",
                columns: table => new
                {
                    PracticeAreaId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PracticeAreaCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    PracticeAreaDescription = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)  
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeArea", x => x.PracticeAreaId);
                });

            migrationBuilder.CreateTable(
                name: "ProficiencyLevel",
                columns: table => new
                {
                    ProficiencyLevelId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProficiencyLevelCode = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    ProficiencyLevelDescription = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProficiencyLevel", x => x.ProficiencyLevelId);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectCode = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ProjectName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PlannedStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PlannedEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProjectTypeId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    ActualStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    PracticeAreaId = table.Column<int>(type: "int", nullable: true),
                    DomainId = table.Column<int>(type: "int", nullable: true),
                    ProjectStateId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectType",
                columns: table => new
                {
                    ProjectTypeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectTypeCode = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectType", x => x.ProjectTypeId);
                });


            migrationBuilder.CreateTable(
                name: "SkillGroup",
                columns: table => new
                {
                    SkillGroupId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SkillGroupName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    CompetencyAreaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillGroup", x => x.SkillGroupId);
                    table.ForeignKey(
                        name: "FK_SkillGroup_CompetencyArea_CompetencyAreaId",
                        column: x => x.CompetencyAreaId,
                        principalTable: "CompetencyArea",
                        principalColumn: "CompetencyAreaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    DepartmentCode = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    DepartmentHeadId = table.Column<int>(nullable: true),
                    DepartmentTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentId);
                    table.ForeignKey(
                        name: "FK_Department_DepartmentType_DepartmentTypeId",
                        column: x => x.DepartmentTypeId,
                        principalTable: "DepartmentType",
                        principalColumn: "DepartmentTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Designation",
                columns: table => new
                {
                    DesignationId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DesignationCode = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    DesignationName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    GradeId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designation", x => x.DesignationId);
                    table.ForeignKey(
                        name: "FK_Designation_Grade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grade",
                        principalColumn: "GradeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectManager",
                columns: table => new
                {
                    ProjectManagerId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(nullable: true),
                    ReportingManagerId = table.Column<int>(type: "int", nullable: true),
                    ProgramManagerId = table.Column<int>(type: "int", nullable: true),
                    LeadId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectManager", x => x.ProjectManagerId);
                    table.ForeignKey(
                        name: "FK_ProjectManager_Project_ProgramManagerId",
                        column: x => x.ProgramManagerId,
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    SkillId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SkillCode = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    SkillName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    SkillDescription = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false),
  
                    CompetencyAreaId = table.Column<int>(nullable: true), 
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: true),
                    SkillGroupId = table.Column<int>(nullable: true)

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.SkillId);
                    table.ForeignKey(
                        name: "FK_Skill_CompetencyArea_CompetencyAreaId",
                        column: x => x.CompetencyAreaId,
                        principalTable: "CompetencyArea",
                        principalColumn: "CompetencyAreaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Skill_SkillGroup_SkillGroupId",
                        column: x => x.SkillGroupId,
                        principalTable: "SkillGroup",
                        principalColumn: "SkillGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Department_DepartmentTypeId",
                table: "Department",
                column: "DepartmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Designation_GradeId",
                table: "Designation",
                column: "GradeId");


            migrationBuilder.CreateIndex(
                name: "IX_ProjectManager_ProgramManagerId",
                table: "ProjectManager",
                column: "ProgramManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_CompetencyAreaId",
                table: "Skill",
                column: "CompetencyAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_SkillGroupId",
                table: "Skill",
                column: "SkillGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillGroup_CompetencyAreaId",
                table: "SkillGroup",
                column: "CompetencyAreaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Designation");

            migrationBuilder.DropTable(
                name: "Domain");

            migrationBuilder.DropTable(
                name: "PracticeArea");

            migrationBuilder.DropTable(
                name: "ProficiencyLevel");

            migrationBuilder.DropTable(
                name: "ProjectManager");

            migrationBuilder.DropTable(
                name: "ProjectType");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropTable(
                name: "DepartmentType");

            migrationBuilder.DropTable(
                name: "Grade");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "SkillGroup");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "CompetencyArea");
        }
    }
}
