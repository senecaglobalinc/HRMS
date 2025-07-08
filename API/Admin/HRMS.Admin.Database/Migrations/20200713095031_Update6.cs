using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Admin.Database.Migrations
{
    public partial class Update6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KRAGroupId",
                table: "RoleMaster",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KRAGroupId",
                table: "RoleMaster");
        }
    }
}
