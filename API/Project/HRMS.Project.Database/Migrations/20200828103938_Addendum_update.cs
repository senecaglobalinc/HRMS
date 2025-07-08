using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Project.Database.Migrations
{
    public partial class Addendum_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addendum_Projects_ProjectId",
                table: "Addendum");

            migrationBuilder.DropForeignKey(
                name: "FK_Addendum_SOW_SOWId",
                table: "Addendum");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SOW_TempId",
                table: "SOW");

            migrationBuilder.DropIndex(
                name: "IX_Addendum_SOWId",
                table: "Addendum");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "SOW");

            migrationBuilder.CreateIndex(
                name: "IX_Addendum_Id",
                table: "Addendum",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addendum_SOW",
                table: "Addendum",
                column: "Id",
                principalTable: "SOW",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Addendum_Project",
                table: "Addendum",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addendum_SOW",
                table: "Addendum");

            migrationBuilder.DropForeignKey(
                name: "FK_Addendum_Project",
                table: "Addendum");

            migrationBuilder.DropIndex(
                name: "IX_Addendum_Id",
                table: "Addendum");

            migrationBuilder.AddColumn<string>(
                name: "TempId",
                table: "SOW",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SOW_TempId",
                table: "SOW",
                column: "TempId");

            migrationBuilder.CreateIndex(
                name: "IX_Addendum_SOWId",
                table: "Addendum",
                column: "SOWId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addendum_Projects_ProjectId",
                table: "Addendum",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Addendum_SOW_SOWId",
                table: "Addendum",
                column: "SOWId",
                principalTable: "SOW",
                principalColumn: "TempId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
