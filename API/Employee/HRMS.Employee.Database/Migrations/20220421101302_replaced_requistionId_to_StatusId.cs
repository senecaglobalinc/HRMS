using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Employee.Database.Migrations
{
    public partial class replaced_requistionId_to_StatusId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequistionId",
                table: "EmployeeSkills");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestedDate",
                table: "EmployeeSkillWorkFlow",
                type: "DATE",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldDefaultValue: new DateTime(2022, 4, 14, 9, 0, 15, 735, DateTimeKind.Local).AddTicks(8835));

            migrationBuilder.AddColumn<int>(
                name: "StatusID",
                table: "EmployeeSkills",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "EmployeeSkills");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestedDate",
                table: "EmployeeSkillWorkFlow",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(2022, 4, 14, 9, 0, 15, 735, DateTimeKind.Local).AddTicks(8835),
                oldClrType: typeof(DateTime),
                oldType: "DATE");

            migrationBuilder.AddColumn<int>(
                name: "RequistionId",
                table: "EmployeeSkills",
                type: "integer",
                nullable: true);
        }
    }
}
