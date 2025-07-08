using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Admin.Database.Migrations
{
    public partial class alter_gradesandRolesTypetbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleTypeId",
                table: "Grade",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RoleType",
                columns: table => new
                {
                    RoleTypeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleTypeName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    RoleTypeDescription = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleType", x => x.RoleTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grade_RoleTypeId",
                table: "Grade",
                column: "RoleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_RoleType_RoleTypeId",
                table: "Grade",
                column: "RoleTypeId",
                principalTable: "RoleType",
                principalColumn: "RoleTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_RoleType_RoleTypeId",
                table: "Grade");

            migrationBuilder.DropTable(
                name: "RoleType");

            migrationBuilder.DropIndex(
                name: "IX_Grade_RoleTypeId",
                table: "Grade");

            migrationBuilder.DropColumn(
                name: "RoleTypeId",
                table: "Grade");
        }
    }
}
