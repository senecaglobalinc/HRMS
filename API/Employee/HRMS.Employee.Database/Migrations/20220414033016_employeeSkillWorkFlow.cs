using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Employee.Database.Migrations
{
    public partial class employeeSkillWorkFlow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeSkillWorkFlow",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(nullable: false),
                    SubmittedBy = table.Column<int>(nullable: false),
                    SubmittedTo = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    RequestedId = table.Column<int>(nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "DATE", nullable: false, defaultValue: new DateTime(2022, 4, 14, 9, 0, 15, 735, DateTimeKind.Local).AddTicks(8835)),
                    ApprovedDate = table.Column<DateTime>(type: "DATE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSkillWorkFlow", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeSkillWorkFlow");
        }
    }
}
