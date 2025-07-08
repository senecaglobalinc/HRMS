using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Employee.Database.Migrations
{
    public partial class updateEmpKRARoleHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "EmployeeKRARoleTypeHistory",
                newName: "ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "EmployeeKRARoleTypeHistory",
                nullable: false,
                defaultValueSql: "uuid_generate_v1()",
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "EmployeeKRARoleTypeHistory",
                newName: "Id");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "EmployeeKRARoleTypeHistory",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValueSql: "uuid_generate_v1()");
        }
    }
}
