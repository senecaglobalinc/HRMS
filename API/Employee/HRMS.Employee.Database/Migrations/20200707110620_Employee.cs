using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Employee.Database.Migrations
{
    public partial class Employee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "EmployeeProjects",
               columns: table => new
               {
                   ID = table.Column<int>(type: "integer", nullable: false)
                       .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                   EmployeeId = table.Column<int>(type: "integer", nullable: true),
                   OrganizationName = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                   ProjectName = table.Column<string>(type: "text", unicode: false, nullable: true),
                   DomainId = table.Column<int>(type: "integer", nullable: true),
                   Duration = table.Column<int>(type: "integer", nullable: true),
                   RoleMasterId = table.Column<int>(type: "integer", nullable: true),                  
                   KeyAchievements = table.Column<string>(type: "text", unicode: false, nullable: true),
                   IsActive = table.Column<bool>(type: "boolean", nullable: true),
                   CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                   CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                   ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                   ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                   SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_EmployeeProjects", x => x.ID);
               });
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeProjects");
        }
    }
}
