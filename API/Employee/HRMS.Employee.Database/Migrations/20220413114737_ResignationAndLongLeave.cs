using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Employee.Database.Migrations
{
    public partial class ResignationAndLongLeave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssociateLongLeave",
                columns: table => new
                {
                    LeaveId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    LongLeaveStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TentativeJoinDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    IsMaternity = table.Column<bool>(type: "boolean", nullable: true),
                    Reason = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateLongLeave", x => x.LeaveId);
                    table.ForeignKey(
                        name: "FK_AssociateLongLeave_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssociateResignation",
                columns: table => new
                {
                    ResignationId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    ReasonId = table.Column<int>(nullable: false),
                    ReasonDescription = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    DateOfResignation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastWorkingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateResignation", x => x.ResignationId);
                    table.ForeignKey(
                        name: "FK_AssociateResignation_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssociateLongLeave_EmployeeId",
                table: "AssociateLongLeave",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssociateResignation_EmployeeId",
                table: "AssociateResignation",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssociateLongLeave");

            migrationBuilder.DropTable(
                name: "AssociateResignation");
        }
    }
}
