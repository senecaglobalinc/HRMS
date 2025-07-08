using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Employee.Database.Migrations
{
    public partial class PA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProspectiveAssociateId",
                table: "ProspectiveAssociates",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProspectiveAssociates",
                newName: "ProspectiveAssociateId");
        }
    }
}
