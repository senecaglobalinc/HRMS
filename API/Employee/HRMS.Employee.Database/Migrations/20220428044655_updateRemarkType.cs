using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Employee.Database.Migrations
{
    public partial class updateRemarkType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "EmployeeSkills",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "BYTEA",
                oldUnicode: false,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Remarks",
                table: "EmployeeSkills",
                type: "BYTEA",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
