using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Employee.Database.Migrations
{
    public partial class EmployeeDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_ProspectiveAssociates_ProspectiveAssociateId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_ProspectiveAssociateId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ProspectiveAssociateId",
                table: "Employee");

            migrationBuilder.CreateTable(
                name: "AssociatesMemberships",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),               
                    EmployeeId = table.Column<int>(nullable: false),
                    ProgramTitle = table.Column<string>(unicode: false, maxLength: 150, nullable: false),
                    ValidFrom = table.Column<string>(unicode: false, maxLength: 4, nullable: false),
                    Institution = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    Specialization = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    ValidUpto = table.Column<string>(unicode: false, maxLength: 4, nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociatesMemberships", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AssociatesMembershipsHistory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(nullable: true),
                    ProgramTitle = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    ValidFrom = table.Column<string>(unicode: false, maxLength: 4, nullable: true),
                    Institution = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    Specialization = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    ValidUpto = table.Column<string>(unicode: false, maxLength: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociatesMembershipsHistory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),                
                    EmployeeId = table.Column<int>(nullable: false),
                    AddressType = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    AddressLine1 = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    AddressLine2 = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    City = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    State = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    PostalCode = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    Country = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true)
                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EducationDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),              
                    EmployeeId = table.Column<int>(nullable: false),
                    EducationalQualification = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    AcademicCompletedYear = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Institution = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    Specialization = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    ProgramType = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    Grade = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    Marks = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),                            
                    AcademicYearFrom = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    AcademicYearTo = table.Column<string>(unicode: false, maxLength: 100, nullable: true)
                   
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationDetails", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyContactDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                   
                    EmployeeId = table.Column<int>(nullable: false),
                    ContactType = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    ContactName = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    Relationship = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    AddressLine1 = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    AddressLine2 = table.Column<string>(unicode: false, maxLength: 500, nullable: true),                  
                    City = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    State = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    PostalCode = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Country = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    TelePhoneNo = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    MobileNo = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    EmailAddress = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),          
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyContactDetails", x => x.Id);
                });
         

            migrationBuilder.CreateTable(
                name: "EmployeeSkills",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),               
                    EmployeeId = table.Column<int>(nullable: false),
                    SkillID = table.Column<int>(nullable: true),
                    CompetencyAreaId = table.Column<int>(nullable: true),
                    ProficiencyLevelId = table.Column<int>(nullable: true),
                    Experience = table.Column<int>(nullable: true),
                    LastUsed = table.Column<int>(nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),                  
                    SkillGroupID = table.Column<int>(nullable: true),
                    RequistionId = table.Column<int>(nullable: true)
                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSkills", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSkillsHistory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(nullable: true),
                    CompetencyAreaId = table.Column<int>(nullable: true),
                    SkillID = table.Column<int>(nullable: true),
                    ProficiencyLevelId = table.Column<int>(nullable: true),
                    Experience = table.Column<int>(nullable: true),
                    LastUsed = table.Column<int>(nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: true),
                    SkillGroupID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSkillsHistory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FamilyDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),                    
                    EmployeeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    RelationShip = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),                  
                    Occupation = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                            
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyDetails", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PreviousEmploymentDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),                 
                    EmployeeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    Address = table.Column<string>(unicode: false, maxLength: 256, nullable: true),
                    Designation = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    ServiceFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ServiceTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LeavingReason = table.Column<string>(unicode: false, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousEmploymentDetails", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ProfessionalDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    
                    EmployeeId = table.Column<int>(nullable: false),
                    ProgramTitle = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    ProgramType = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    Year = table.Column<int>(nullable: true),
                    institution = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    specialization = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    CurrentValidity = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                  
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionalDetails", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ProfessionalReferences",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),                   
                    EmployeeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    Designation = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    CompanyName = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    CompanyAddress = table.Column<string>(unicode: false, nullable: true),
                    OfficeEmailAddress = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    MobileNo = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),                   
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionalReferences", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TagAssociate",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),                   
                    TagAssociateListName = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    ManagerId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagAssociate", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UploadFiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),                   
                    EmployeeId = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(unicode: false, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                   
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadFiles", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssociatesMemberships");

            migrationBuilder.DropTable(
                name: "AssociatesMembershipsHistory");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "EducationDetails");

            migrationBuilder.DropTable(
                name: "EmergencyContactDetails");

            migrationBuilder.DropTable(
                name: "EmployeeSkills");

            migrationBuilder.DropTable(
                name: "EmployeeSkillsHistory");

            migrationBuilder.DropTable(
                name: "FamilyDetails");

            migrationBuilder.DropTable(
                name: "PreviousEmploymentDetails");

            migrationBuilder.DropTable(
                name: "ProfessionalDetails");

            migrationBuilder.DropTable(
                name: "ProfessionalReferences");

            migrationBuilder.DropTable(
                name: "TagAssociate");

            migrationBuilder.DropTable(
                name: "UploadFiles");

            migrationBuilder.AddColumn<int>(
                name: "ProspectiveAssociateId",
                table: "Employee",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ProspectiveAssociateId",
                table: "Employee",
                column: "ProspectiveAssociateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_ProspectiveAssociates_ProspectiveAssociateId",
                table: "Employee",
                column: "ProspectiveAssociateId",
                principalTable: "ProspectiveAssociates",
                principalColumn: "ProspectiveAssociateId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
