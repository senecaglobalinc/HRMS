using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Employee.Database.Migrations
{
    public partial class EmployeeSkillWorkFlow_SKillId_column_replacement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
               name: "SkillId",               
               table: "EmployeeSkillWorkFlow",
               newName: "EmployeeSkillsID");    
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {           

            migrationBuilder.RenameColumn(
                name: "EmployeeSkillsID",
                table: "EmployeeSkillWorkFlow",
                 newName: "SkillId");
        }
    }
}
