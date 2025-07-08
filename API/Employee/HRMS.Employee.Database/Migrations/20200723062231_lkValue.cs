using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Employee.Database.Migrations
{
    public partial class lkValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ValueType",
                columns: table => new
                {
                    ValueTypeKey = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    ValueTypeId = table.Column<int>(nullable: false),
                    ValueTypeName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueType", x => x.ValueTypeKey);
                });

            migrationBuilder.CreateTable(
                name: "lkValue",
                columns: table => new
                {
                    ValueKey = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    ValueId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    ValueName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    ValueTypeKey = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)   
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lkValue", x => x.ValueKey);
                    table.ForeignKey(
                        name: "FK_lkValue_ValueType_ValueTypeKey",
                        column: x => x.ValueTypeKey,
                        principalTable: "ValueType",
                        principalColumn: "ValueTypeKey",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lkValue_ValueTypeKey",
                table: "lkValue",
                column: "ValueTypeKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lkValue");

            migrationBuilder.DropTable(
                name: "ValueType");
        }
    }
}
