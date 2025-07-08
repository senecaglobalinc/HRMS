using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Employee.Database.Migrations
{
    public partial class EmployeeSkillWorkFlow_SKillId_to_EmployeeSkillId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
             name: "EmployeeSkillsID",
             table: "EmployeeSkillWorkFlow",
             newName: "EmployeeSkillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmployeeSkillId",
                table: "EmployeeSkillWorkFlow",
                 newName: "EmployeeSkillsID");
        }
    }
}
