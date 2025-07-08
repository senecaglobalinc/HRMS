using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Employee.Database.Migrations
{
    public partial class AddColumns_In_EmployeeSkillWorkFlow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Experience",
                table: "EmployeeSkillWorkFlow",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastUsed",
                table: "EmployeeSkillWorkFlow",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Experience",
                table: "EmployeeSkillWorkFlow");

            migrationBuilder.DropColumn(
                name: "LastUsed",
                table: "EmployeeSkillWorkFlow");
            
        }
    }
}
