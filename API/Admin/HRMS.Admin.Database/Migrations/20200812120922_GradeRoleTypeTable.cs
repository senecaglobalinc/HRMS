using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Admin.Database.Migrations
{
    public partial class GradeRoleTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_RoleType_RoleTypeId",
                table: "Grade");

            migrationBuilder.DropIndex(
                name: "IX_Grade_RoleTypeId",
                table: "Grade");

            migrationBuilder.DropColumn(
                name: "RoleTypeId",
                table: "Grade");

            migrationBuilder.AlterColumn<string>(
                name: "RoleTypeName",
                table: "RoleType",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "GradeRoleType",
                columns: table => new
                {
                    GradeRoleTypeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GradeId = table.Column<int>(type: "int", nullable: false),
                    RoleTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true)                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeRoleType", x => x.GradeRoleTypeId);
                    table.ForeignKey(
                        name: "FK_GradeRoleType_Grade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grade",
                        principalColumn: "GradeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GradeRoleType_RoleType_RoleTypeId",
                        column: x => x.RoleTypeId,
                        principalTable: "RoleType",
                        principalColumn: "RoleTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GradeRoleType_GradeId",
                table: "GradeRoleType",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeRoleType_RoleTypeId_GradeId",
                table: "GradeRoleType",
                columns: new[] { "RoleTypeId", "GradeId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GradeRoleType");

            migrationBuilder.AlterColumn<string>(
                name: "RoleTypeName",
                table: "RoleType",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<int>(
                name: "RoleTypeId",
                table: "Grade",
                type: "int",
                nullable: true);

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
    }
}
