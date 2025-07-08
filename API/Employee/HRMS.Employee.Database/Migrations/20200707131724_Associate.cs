using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Employee.Database.Migrations
{
    public partial class Associate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssociateCertifications",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    
                    EmployeeId = table.Column<int>(nullable: false),
                    CertificationId = table.Column<int>(nullable: false),
                    ValidFrom = table.Column<string>(unicode: false, maxLength: 4, nullable: true),
                    Institution = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    Specialization = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    ValidUpto = table.Column<string>(unicode: false, maxLength: 4, nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),                  
                    SkillGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateCertifications", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AssociateCertificationsHistory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(nullable: false),
                    CertificationId = table.Column<int>(nullable: false),
                    ValidFrom = table.Column<string>(unicode: false, maxLength: 4, nullable: true),
                    Institution = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    Specialization = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    ValidUpto = table.Column<string>(unicode: false, maxLength: 4, nullable: true),
                    SkillGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateCertificationsHistory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AssociateDesignations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),                    
                    EmployeeId = table.Column<int>(nullable: true),
                    DesignationId = table.Column<int>(nullable: true),
                    GradeId = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),                  
                    FromDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ToDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateDesignations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AssociateHistory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(nullable: false),
                    Designation = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    GradeId = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    DepartmentId = table.Column<int>(nullable: true),
                    PracticeAreaId = table.Column<int>(nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateHistory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AssociateSkillGap",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),                   
                    EmployeeId = table.Column<int>(nullable: true),
                    ProjectSkillId = table.Column<int>(nullable: true),
                    CompetencyAreaId = table.Column<int>(nullable: true),
                    StatusId = table.Column<int>(nullable: true),
                    CurrentProficiencyLevelId = table.Column<int>(nullable: true),
                    RequiredProficiencyLevelId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateSkillGap", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssociateCertifications");

            migrationBuilder.DropTable(
                name: "AssociateCertificationsHistory");

            migrationBuilder.DropTable(
                name: "AssociateDesignations");

            migrationBuilder.DropTable(
                name: "AssociateHistory");

            migrationBuilder.DropTable(
                name: "AssociateSkillGap");
        }
    }
}
