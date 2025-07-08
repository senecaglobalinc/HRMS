using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Employee.Database.Migrations
{
    public partial class changesInEmployeeSkillWorkFlow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "EmployeeSkillWorkFlow");

            migrationBuilder.DropColumn(
                name: "RequestedDate",
                table: "EmployeeSkillWorkFlow");

            migrationBuilder.DropColumn(
                name: "RequestedId",
                table: "EmployeeSkillWorkFlow");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EmployeeSkills");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmployeeSkillWorkFlow",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EmployeeSkillWorkFlow",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EmployeeSkillWorkFlow",
                type: "boolean",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "EmployeeSkillWorkFlow",
                type: "varchar(100)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "EmployeeSkillWorkFlow",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "EmployeeSkillWorkFlow",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportingManagerRating",
                table: "EmployeeSkillWorkFlow",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkillId",
                table: "EmployeeSkillWorkFlow",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubmittedRating",
                table: "EmployeeSkillWorkFlow",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SystemInfo",
                table: "EmployeeSkillWorkFlow",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "EmployeeSkills"
               );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeeSkillWorkFlow");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EmployeeSkillWorkFlow");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EmployeeSkillWorkFlow");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "EmployeeSkillWorkFlow");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "EmployeeSkillWorkFlow");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "EmployeeSkillWorkFlow");

            migrationBuilder.DropColumn(
                name: "ReportingManagerRating",
                table: "EmployeeSkillWorkFlow");

            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "EmployeeSkillWorkFlow");

            migrationBuilder.DropColumn(
                name: "SubmittedRating",
                table: "EmployeeSkillWorkFlow");

            migrationBuilder.DropColumn(
                name: "SystemInfo",
                table: "EmployeeSkillWorkFlow");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "EmployeeSkillWorkFlow",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedDate",
                table: "EmployeeSkillWorkFlow",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RequestedId",
                table: "EmployeeSkillWorkFlow",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "EmployeeSkills",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EmployeeSkills",
                type: "boolean",
                nullable: true);
        }
    }
}
