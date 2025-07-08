using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Employee.Database.Migrations
{
    public partial class updateEmployeeRoleTypeHistory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RoleTypeValidTo",
                table: "EmployeeKRARoleTypeHistory",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RoleTypeValidFrom",
                table: "EmployeeKRARoleTypeHistory",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RoleTypeValidTo",
                table: "EmployeeKRARoleTypeHistory",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RoleTypeValidFrom",
                table: "EmployeeKRARoleTypeHistory",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
