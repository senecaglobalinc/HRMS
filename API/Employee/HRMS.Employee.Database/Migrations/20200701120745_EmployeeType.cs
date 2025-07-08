using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Employee.Database.Migrations
{
    public partial class EmployeeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeTypes",
                columns: table => new
                {
                    EmployeeTypeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeTypeCode = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    EmpType = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true)
                                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTypes", x => x.EmployeeTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ProspectiveAssociates",
                columns: table => new
                {
                    ProspectiveAssociateId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),                  
                    FirstName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    DesignationId = table.Column<int>(type: "integer", nullable: false),
                    GradeId = table.Column<int>(type: "integer", nullable: true),
                    Technology = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    HRAdvisorName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    JoiningStatusId = table.Column<int>(type: "integer", nullable: true),
                    JoinDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),                  
                    EmploymentType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    MaritalStatus = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    BGVStatusId = table.Column<int>(type: "integer", nullable: true),
                    TechnologyID = table.Column<int>(type: "integer", nullable: true),
                    EmployeeID = table.Column<int>(type: "integer", nullable: true),
                    RecruitedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    StatusID = table.Column<int>(type: "integer", nullable: true),
                    ReasonForDropOut = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    ManagerId = table.Column<int>(type: "integer", nullable: false),
                    PersonalEmailAddress = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    MobileNo = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProspectiveAssociates", x => x.ProspectiveAssociateId);
                });

            migrationBuilder.CreateTable(
                name: "SkillSearch",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeID = table.Column<int>(nullable: true),
                    FirstName = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    LastName = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    Experience = table.Column<decimal>(type: "decimal(5, 2)", nullable: true),
                    RoleMasterId = table.Column<int>(nullable: true),
                    RoleDescription = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    DesignationID = table.Column<int>(nullable: true),
                    DesignationCode = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    ProjectCode = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    ProjectName = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    IsPrimary = table.Column<bool>(nullable: true),
                    IsCritical = table.Column<bool>(nullable: true),
                    IsBillable = table.Column<bool>(nullable: true),
                    CompetencyAreaID = table.Column<int>(nullable: true),
                    CompetencyAreaCode = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    SkillIGroupID = table.Column<int>(nullable: true),
                    SkillGroupName = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    SkillID = table.Column<int>(nullable: true),
                    SkillName = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    ProficiencyLevelID = table.Column<int>(nullable: true),
                    ProficiencyLevelCode = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    EmployeeCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    IsSkillPrimary = table.Column<bool>(type: "boolean", nullable: true, defaultValueSql: "(('t'))"),
                    DesignationName = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    LastUsed = table.Column<int>(nullable: true),
                    SkillExperience = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillSearch", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),                 
                    EmployeeCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    MiddleName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Photograph = table.Column<byte[]>(type: "BYTEA", nullable: true),
                    AccessCardNo = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Gender = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    GradeId = table.Column<int>(type: "integer", nullable: true),
                    DesignationId = table.Column<int>(type: "integer", nullable: true),
                    MaritalStatus = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Qualification = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    TelephoneNo = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    MobileNo = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    WorkEmailAddress = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    PersonalEmailAddress = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DateofBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    JoinDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConfirmationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RelievingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BloodGroup = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Nationality = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PANNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PassportNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PassportIssuingOffice = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PassportDateValidUpto = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ReportingManager = table.Column<int>(type: "integer", nullable: true),
                    ProgramManager = table.Column<int>(type: "integer", nullable: true),
                    DepartmentId = table.Column<int>(type: "integer", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, defaultValueSql: "inet_client_addr()"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),                    
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),                                     
                    DocumentsUploadFlag = table.Column<bool>(type: "boolean", nullable: true),
                    CubicalNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    AlternateMobileNo = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    StatusId = table.Column<int>(type: "integer", nullable: true),
                    BGVInitiatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BGVCompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BGVStatusId = table.Column<int>(type: "integer", nullable: true),
                    Experience = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    CompetencyGroup = table.Column<int>(type: "integer", nullable: true),
                    BGVTargetDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EmployeeTypeId = table.Column<int>(nullable: true),
                    userid = table.Column<int>(type: "integer", nullable: true),
                    ResignationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BGVStatus = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PAID = table.Column<int>(nullable: true),
                    HRAdvisor = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    UANNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    AadharNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PFNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Remarks = table.Column<byte[]>(type: "BYTEA", unicode: false, nullable: true),
                    EmploymentStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CareerBreak = table.Column<int>(type: "integer", nullable: true),
                    TotalExperience = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    ExperienceExcludingCareerBreak = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    ProspectiveAssociateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employee_ProspectiveAssociates_ProspectiveAssociateId",
                        column: x => x.ProspectiveAssociateId,
                        principalTable: "ProspectiveAssociates",
                        principalColumn: "ProspectiveAssociateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ProspectiveAssociateId",
                table: "Employee",
                column: "ProspectiveAssociateId");

            migrationBuilder.CreateIndex(
                name: "IX_EMployee_UserId_IsActive",
                table: "Employee",
                columns: new[] { "EmployeeId", "userid", "IsActive" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "EmployeeTypes");

            migrationBuilder.DropTable(
                name: "SkillSearch");

            migrationBuilder.DropTable(
                name: "ProspectiveAssociates");
        }
    }
}
