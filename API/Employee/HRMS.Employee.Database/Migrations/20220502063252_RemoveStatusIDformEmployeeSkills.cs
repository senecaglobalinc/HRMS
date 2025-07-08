using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Employee.Database.Migrations
{
    public partial class RemoveStatusIDformEmployeeSkills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "EmployeeSkills");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
         
            migrationBuilder.AddColumn<int>(
                name: "StatusID",
                table: "EmployeeSkills",
                type: "integer",
                nullable: true);
        }
    }
}
