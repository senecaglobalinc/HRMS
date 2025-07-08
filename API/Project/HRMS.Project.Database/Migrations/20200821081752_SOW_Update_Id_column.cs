using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Project.Database.Migrations
{
    public partial class SOW_Update_Id_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addendum_SOW_SOWId",
                table: "Addendum");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SOW",
                table: "SOW");

            migrationBuilder.DropIndex(
                name: "IX_Addendum_SOWId",
                table: "Addendum");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SOW",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "SOWId1",
                table: "Addendum",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SOW",
                table: "SOW",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Addendum_SOWId1",
                table: "Addendum",
                column: "SOWId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Addendum_SOW_SOWId1",
                table: "Addendum",
                column: "SOWId1",
                principalTable: "SOW",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addendum_SOW_SOWId1",
                table: "Addendum");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SOW",
                table: "SOW");

            migrationBuilder.DropIndex(
                name: "IX_Addendum_SOWId1",
                table: "Addendum");

            migrationBuilder.DropColumn(
                name: "SOWId1",
                table: "Addendum");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SOW",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SOW",
                table: "SOW",
                column: "SOWId");

            migrationBuilder.CreateIndex(
                name: "IX_Addendum_SOWId",
                table: "Addendum",
                column: "SOWId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addendum_SOW_SOWId",
                table: "Addendum",
                column: "SOWId",
                principalTable: "SOW",
                principalColumn: "SOWId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
