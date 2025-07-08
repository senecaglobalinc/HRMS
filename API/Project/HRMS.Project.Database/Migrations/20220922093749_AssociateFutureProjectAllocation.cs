using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Project.Database.Migrations
{
    public partial class AssociateFutureProjectAllocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.CreateTable(
                name: "AssociateFutureProjectAllocation",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(nullable: false),
                    AssociateName = table.Column<string>(nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    TentativeDate = table.Column<DateTime>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateFutureProjectAllocation", x => x.ID);
                });
                       

            migrationBuilder.CreateIndex(
                name: "IX_AssociateFutureProjectAllocation_AssociateName",
                table: "AssociateFutureProjectAllocation",
                column: "AssociateName");

            migrationBuilder.CreateIndex(
                name: "IX_AssociateFutureProjectAllocation_EmployeeId",
                table: "AssociateFutureProjectAllocation",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssociateFutureProjectAllocation_ProjectId",
                table: "AssociateFutureProjectAllocation",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AssociateFutureProjectAllocation_ProjectName",
                table: "AssociateFutureProjectAllocation",
                column: "ProjectName");

            migrationBuilder.CreateIndex(
                name: "IX_AssociateFutureProjectAllocation_Remarks",
                table: "AssociateFutureProjectAllocation",
                column: "Remarks");

            migrationBuilder.CreateIndex(
                name: "IX_AssociateFutureProjectAllocation_TentativeDate",
                table: "AssociateFutureProjectAllocation",
                column: "TentativeDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssociateFutureProjectAllocation");

        }
    }
}
