using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Admin.Database.Migrations
{
    public partial class Update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SGRole",
                columns: table => new
                {
                    SGRoleID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SGRoleName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    DepartmentId = table.Column<int>(nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SGRole", x => x.SGRoleID);
                    table.ForeignKey(
                        name: "FK_SGRole_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SGRolePrefix",
                columns: table => new
                {
                    PrefixID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PrefixName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SGRolePrefix", x => x.PrefixID);
                });

            migrationBuilder.CreateTable(
                name: "SGRoleSuffix",
                columns: table => new
                {
                    SuffixID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SuffixName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SGRoleSuffix", x => x.SuffixID);
                });

            migrationBuilder.CreateTable(
                name: "RoleMaster",
                columns: table => new
                {
                    RoleMasterID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SGRoleID = table.Column<int>(nullable: false),
                    PrefixID = table.Column<int>(nullable: true),
                    SuffixID = table.Column<int>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: true),
                    RoleDescription = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    KeyResponsibilities = table.Column<string>(type: "text", nullable: true),
                    EducationQualification = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                   
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMaster", x => x.RoleMasterID);
                    table.ForeignKey(
                        name: "FK_RoleMaster_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleMaster_SGRolePrefix_PrefixID",
                        column: x => x.PrefixID,
                        principalTable: "SGRolePrefix",
                        principalColumn: "PrefixID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleMaster_SGRole_SGRoleID",
                        column: x => x.SGRoleID,
                        principalTable: "SGRole",
                        principalColumn: "SGRoleID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleMaster_SGRoleSuffix_SuffixID",
                        column: x => x.SuffixID,
                        principalTable: "SGRoleSuffix",
                        principalColumn: "SuffixID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleMaster_DepartmentId",
                table: "RoleMaster",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMaster_PrefixID",
                table: "RoleMaster",
                column: "PrefixID");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMaster_SGRoleID",
                table: "RoleMaster",
                column: "SGRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMaster_SuffixID",
                table: "RoleMaster",
                column: "SuffixID");

            migrationBuilder.CreateIndex(
                name: "IX_SGRole_DepartmentId",
                table: "SGRole",
                column: "DepartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleMaster");

            migrationBuilder.DropTable(
                name: "SGRolePrefix");

            migrationBuilder.DropTable(
                name: "SGRole");

            migrationBuilder.DropTable(
                name: "SGRoleSuffix");
        }
    }
}
