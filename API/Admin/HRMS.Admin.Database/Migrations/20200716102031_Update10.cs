using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Admin.Database.Migrations
{
    public partial class Update10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NotificationTypeId = table.Column<int>(nullable: false),
                    NotificationCode = table.Column<string>(nullable: true),
                    NotificationDescription = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    SystemInfo = table.Column<string>(nullable: true),
                    CategoryMasterId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTypes_Categories_CategoryMasterId",
                        column: x => x.CategoryMasterId,
                        principalTable: "Categories",
                        principalColumn: "CategoryMasterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationConfiguration",
                columns: table => new
                {
                    NotificationConfigurationId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NotificationTypeId = table.Column<int>(nullable: true),
                    EmailFrom = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true),
                    EmailTo = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true),
                    EmailCC = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true),
                    EmailSubject = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    EmailContent = table.Column<string>(type: "text", nullable: true),
                    SLA = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),  
                    CategoryMasterId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationConfiguration", x => x.NotificationConfigurationId);
                    table.ForeignKey(
                        name: "FK_NotificationConfiguration_Categories_CategoryMasterId",
                        column: x => x.CategoryMasterId,
                        principalTable: "Categories",
                        principalColumn: "CategoryMasterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationConfiguration_NotificationTypes_NotificationTyp~",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationConfiguration_CategoryMasterId",
                table: "NotificationConfiguration",
                column: "CategoryMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationConfiguration_NotificationTypeId",
                table: "NotificationConfiguration",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypes_CategoryMasterId",
                table: "NotificationTypes",
                column: "CategoryMasterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationConfiguration");

            migrationBuilder.DropTable(
                name: "NotificationTypes");
        }
    }
}
